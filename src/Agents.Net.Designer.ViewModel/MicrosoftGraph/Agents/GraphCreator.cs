using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(GraphCreated))]
    public class GraphCreator : Agent
    {
        public GraphCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            //Check uniqueness of nodes
            ModelUpdated updated = messageData.Get<ModelUpdated>();
            Graph graph = new Graph();
            List<Node> messages = AddMessages(updated, graph);

            foreach (AgentModel agentModel in updated.Model.Agents)
            {
                AddAgentNode(agentModel, graph);
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
                              true, agentModel.Id, graph);
                AddEventEdges(agentModel.ProducedEvents ?? Enumerable.Empty<string>(),
                              false, agentModel.Id, graph);
            }

            OnMessage(new GraphCreated(graph, messageData));
        }

        private void AddEventEdges(IEnumerable<string> events, bool addAsSource, Guid agentModelId,
                                   Graph graph)
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
                    throw new InvalidOperationException("Cannot happen.");
                }

                edge.Attr.Color = addMessageAsSource ? Color.Green : Color.Blue;
                if (isInterception)
                {
                    edge.Attr.Color = Color.Red;
                    edge.Attr.ArrowheadAtSource = ArrowStyle.Normal;
                }
            }
        }

        private static void AddAgentNode(AgentModel agentModel, Graph graph)
        {
            Node agentNode = new Node(agentModel.Id.ToString("D"))
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
        }

        private static List<Node> AddMessages(ModelUpdated updated, Graph graph)
        {
            List<Node> messages = new List<Node>();
            foreach (MessageModel messageModel in updated.Model.Messages)
            {
                Node messageNode = new Node(messageModel.Id.ToString("D"))
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
            }

            foreach (MessageDecoratorModel decoratorModel in updated.Model.Messages.OfType<MessageDecoratorModel>())
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
