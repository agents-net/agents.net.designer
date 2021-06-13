using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Edge = Microsoft.Msagl.Drawing.Edge;
using Node = Microsoft.Msagl.Drawing.Node;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(ModificationResult))]
    [Consumes(typeof(ModelLoaded))]
    [Produces(typeof(GraphCreated))]
    public class GraphCreator : Agent
    {
        public GraphCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            //Check uniqueness of nodes
            CommunityModel model = messageData.TryGet(out ModificationResult modificationResult)
                                       ? modificationResult.Model
                                       : messageData.Get<ModelLoaded>().Model;
            Graph graph = new()
            {
                LayoutAlgorithmSettings =
                {
                    PackingMethod = PackingMethod.Columns,
                    EdgeRoutingSettings = new EdgeRoutingSettings
                    {
                        EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines,
                        Padding = 5,
                        PolylinePadding = 3,
                        BendPenalty = 1,
                    },
                    LiftCrossEdges = true,
                    NodeSeparation = 20,
                    ClusterMargin = 40
                }
            };
            Dictionary<string, List<Node>> subgraphCollection = new();
            List<Node> messages = AddMessages(model, graph, CheckSubgraph);

            foreach (AgentModel agentModel in model.Agents)
            {
                AddAgentNode(agentModel, graph, CheckSubgraph);
                AddMessageEdges(agentModel.ConsumingMessages, messages, true,
                                agentModel.Id, graph);
                AddMessageEdges(agentModel.ProducedMessages, messages, false,
                                agentModel.Id, graph);
                if (agentModel is InterceptorAgentModel interceptor)
                {
                    AddMessageEdges(interceptor.InterceptingMessages, messages, true,
                                    agentModel.Id, graph, true);
                }
                AddEventEdges(agentModel.IncomingEvents ?? Enumerable.Empty<string>(),
                              true, agentModel.Id, graph, CheckSubgraphForEvent);
                AddEventEdges(agentModel.ProducedEvents ?? Enumerable.Empty<string>(),
                              false, agentModel.Id, graph, CheckSubgraphForEvent);
            }

            CreateSubGraphs(graph, subgraphCollection);

            OnMessage(new GraphCreated(graph, messageData));

            void CheckSubgraphForEvent(Node eventToCheck, Guid connectedAgentId)
            {
                AgentModel agentModel = model.Agents.First(a => a.Id == connectedAgentId);
                string ns = Regex.Replace(agentModel.Namespace, @"\.Agents$", string.Empty);
                
                if (!string.IsNullOrEmpty(ns))
                {
                    AddNodeToSubgraph(eventToCheck, ns);
                }
            }
            
            void CheckSubgraph(Node nodeToCheck)
            {
                string ns;
                if (nodeToCheck.UserData is AgentModel agent)
                {
                    ns = Regex.Replace(agent.Namespace, @"\.Agents$", string.Empty);
                }
                else
                {
                    ns = Regex.Replace(((MessageModel) nodeToCheck.UserData).Namespace, @"\.Messages$", string.Empty);
                }

                if (!string.IsNullOrEmpty(ns))
                {
                    AddNodeToSubgraph(nodeToCheck, ns);
                }
            }

            void AddNodeToSubgraph(Node node, string ns)
            {
                if (!subgraphCollection.ContainsKey(ns))
                {
                    subgraphCollection.Add(ns, new List<Node>());
                }
                subgraphCollection[ns].Add(node);
            }
        }

        private void CreateSubGraphs(Graph graph, Dictionary<string,List<Node>> subgraphCollection)
        {
            Stack<(string, Subgraph)> createdGraphs = new();
            foreach (string ns in subgraphCollection.Keys.OrderBy(ns => ns.Count(c => c == '.')))
            {
                Subgraph current = new(ns)
                {
                    LayoutSettings = graph.LayoutAlgorithmSettings.Clone()
                };
                foreach (Node node in subgraphCollection[ns])
                {
                    current.AddNode(node);
                }
                Subgraph parent = FindSubgraphParent(ns);
                parent.AddSubgraph(current);
                createdGraphs.Push((ns, current));
            }

            Subgraph[] subGraphs = createdGraphs.Select(g => g.Item2).ToArray();
            foreach (Subgraph subgraph in subGraphs)
            {
                IEnumerable<Subgraph> targetGraphs = subgraph.Nodes.SelectMany(n => n.OutEdges)
                                                             .Select(e => subGraphs.FirstOrDefault(
                                                                         g => g.Nodes.Contains(e.TargetNode)))
                                                             .Distinct()
                                                             .Where(g => g != null)
                                                             .Where(g => !Equals(g, subgraph));
                foreach (Subgraph targetGraph in targetGraphs)
                {
                    graph.AddEdge(subgraph.Id, targetGraph.Id);
                }
            }

            Subgraph FindSubgraphParent(string ns)
            {
                Subgraph parent = graph.RootSubgraph;
                foreach ((string key, Subgraph subgraph) in createdGraphs)
                {
                    if (ns.StartsWith(key, StringComparison.Ordinal))
                    {
                        parent = subgraph;
                        break;
                    }
                }
                return parent;
            }
        }

        private void AddEventEdges(IEnumerable<string> events, bool addAsSource, Guid agentModelId,
                                   Graph graph, Action<Node, Guid> checkSubgraph)
        {
            foreach (string @event in events)
            {
                if (string.IsNullOrEmpty(@event))
                {
                    continue;
                }

                Node eventNode;
                if (addAsSource)
                {
                    Edge edge = graph.AddEdge(@event, agentModelId.ToString("D"));
                    edge.Attr.Color = Color.Violet;
                    eventNode = edge.SourceNode;
                }
                else
                {
                    Edge edge = graph.AddEdge(agentModelId.ToString("D"), @event);
                    edge.Attr.Color = Color.Orange;
                    eventNode = edge.TargetNode;
                }
                eventNode.Attr.Shape = Shape.Diamond;
                eventNode.Attr.FillColor = Color.Gray;
                checkSubgraph(eventNode, agentModelId);
            }
        }

        private void AddMessageEdges(Guid[] messages, List<Node> messageNodes, bool addMessageAsSource,
                                     Guid agentModelId, Graph graph, bool isInterception = false)
        {
            foreach (Guid message in messages)
            {
                Node messageNode = messageNodes.FirstOrDefault(n => n.Id == message.ToString("D"));
                Edge edge;
                if (messageNode != null)
                {
                    edge = addMessageAsSource
                               ? isInterception
                                     ? graph.AddEdge(messageNode.Id, "intercepted", agentModelId.ToString("D"))
                                     : graph.AddEdge(messageNode.Id, agentModelId.ToString("D"))
                               : graph.AddEdge(agentModelId.ToString("D"), messageNode.Id);
                }
                else
                {
                    throw new InvalidOperationException($"Could not find message {message} in graph although it was expected.");
                }

                edge.Attr.Color = addMessageAsSource ? Color.Green : Color.Blue;
                if (isInterception)
                {
                    edge.Attr.Color = Color.Red;
                    edge.Attr.ArrowheadAtSource = ArrowStyle.Normal;
                }
            }
        }

        private static void AddAgentNode(AgentModel agentModel, Graph graph, Action<Node> checkSubgraph)
        {
            Node agentNode = new(agentModel.Id.ToString("D"))
            {
                Attr =
                {
                    Shape = Shape.Ellipse,
                    FillColor = agentModel is InterceptorAgentModel? Color.LightGoldenrodYellow : Color.White
                },
                LabelText = agentModel.Name,
                UserData = agentModel
            };
            graph.AddNode(agentNode);
            checkSubgraph(agentNode);
        }

        private static List<Node> AddMessages(CommunityModel model, Graph graph, Action<Node> checkSubgraph)
        {
            List<Node> messages = new();
            foreach (MessageModel messageModel in model.Messages)
            {
                Node messageNode = new(messageModel.Id.ToString("D"))
                {
                    Attr =
                    {
                        Shape = Shape.Box,
                        FillColor = Color.White,
                    },
                    LabelText = messageModel.Name,
                    UserData = messageModel
                };

                messages.Add(messageNode);
                graph.AddNode(messageNode);
                checkSubgraph(messageNode);
            }

            foreach (MessageDecoratorModel decoratorModel in model.Messages.OfType<MessageDecoratorModel>())
            {
                Node messageNode = messages.First(n => n.Id == decoratorModel.Id.ToString("D"));
                messageNode.Attr.FillColor = Color.LightGoldenrodYellow;
                if (decoratorModel.DecoratedMessage != default)
                {
                    Edge edge = graph.AddEdge(decoratorModel.Id.ToString("D"), "decorates",
                                              decoratorModel.DecoratedMessage.ToString("D"));
                    edge.Attr.Color = new Color(0xD8, 0x8B, 0x8B);
                }
            }

            return messages;
        }
    }
}
