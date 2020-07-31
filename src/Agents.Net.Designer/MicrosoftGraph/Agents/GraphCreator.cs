using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.MicrosoftGraph.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.MicrosoftGraph.Agents
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
                                agentModel.Id, graph, updated.Model);
                AddMessageEdges(agentModel.ProducedMessages, messages, false,
                                agentModel.Id, graph, updated.Model);
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

        private void AddMessageEdges(string[] messages, List<Node> messageNodes, bool addMessageAsSource,
                                     Guid agentModelId, Graph graph, CommunityModel model)
        {
            foreach (string message in messages)
            {
                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }
                Node messageNode = messageNodes.FirstOrDefault(n => n.UserData.AssertTypeOf<MessageModel>().FullName(model).EndsWith(message));
                Edge edge;
                if (messageNode != null)
                {
                    edge = addMessageAsSource
                               ? graph.AddEdge(messageNode.Id, agentModelId.ToString("D"))
                               : graph.AddEdge(agentModelId.ToString("D"), messageNode.Id);
                }
                else
                {
                    edge = addMessageAsSource
                               ? graph.AddEdge(message, agentModelId.ToString("D"))
                               : graph.AddEdge(agentModelId.ToString("D"), message);
                    Node newNode = addMessageAsSource ? edge.SourceNode : edge.TargetNode;
                    newNode.Attr.Shape = Shape.Box;
                    newNode.Attr.FillColor = Color.Gray;
                }

                edge.Attr.Color = addMessageAsSource ? Color.Green : Color.Blue;
            }
        }

        private static void AddAgentNode(AgentModel agentModel, Graph graph)
        {
            Node agentNode = new Node(agentModel.Id.ToString("D"))
            {
                Attr =
                {
                    Shape = Shape.Ellipse,
                    FillColor = Color.White
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

            return messages;
        }
    }
}
