﻿using System.Collections.Generic;
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
            List<Node> messages = new List<Node>();
            foreach (MessageModel messageModel in updated.Model.Messages)
            {
                Node messageNode = new Node(messageModel.Name)
                {
                    Attr =
                    {
                        Shape = Shape.Box,
                        FillColor = Color.White
                    }, 
                    UserData = messageModel
                };
                messages.Add(messageNode);
                graph.AddNode(messageNode);
            }

            foreach (AgentModel agentModel in updated.Model.Agents)
            {
                Node agentNode =  new Node(agentModel.Name)
                {
                    Attr =
                    {
                        Shape = Shape.Ellipse,
                        FillColor = Color.White
                    },
                    UserData = agentModel
                };
                graph.AddNode(agentNode);
                foreach (string consumingMessage in agentModel.ConsumingMessages)
                {
                    if (string.IsNullOrEmpty(consumingMessage))
                    {
                        continue;
                    }
                    Edge edge = graph.AddEdge(consumingMessage, agentModel.Name);
                    if (!messages.Contains(edge.SourceNode))
                    {
                        edge.SourceNode.Attr.Shape = Shape.Box;
                        edge.SourceNode.Attr.FillColor = Color.Gray;
                    }
                    edge.Attr.Color = Color.Green;
                }

                foreach (string producingMessage in agentModel.ProducedMessages)
                {
                    if (string.IsNullOrEmpty(producingMessage))
                    {
                        continue;
                    }
                    Edge edge = graph.AddEdge(agentModel.Name, producingMessage);
                    if (!messages.Contains(edge.TargetNode))
                    {
                        edge.TargetNode.Attr.Shape = Shape.Box;
                        edge.TargetNode.Attr.FillColor = Color.Gray;
                    }
                    edge.Attr.Color = Color.Blue;
                }

                foreach (string incomingEvent in agentModel.IncomingEvents??Enumerable.Empty<string>())
                {
                    if (string.IsNullOrEmpty(incomingEvent))
                    {
                        continue;
                    }

                    Edge edge = graph.AddEdge(incomingEvent, agentModel.Name);
                    edge.SourceNode.Attr.Shape = Shape.Diamond;
                    edge.SourceNode.Attr.FillColor = Color.Gray;
                    edge.Attr.Color = Color.Violet;
                }

                foreach (string producedEvent in agentModel.ProducedEvents??Enumerable.Empty<string>())
                {
                    if (string.IsNullOrEmpty(producedEvent))
                    {
                        continue;
                    }

                    Edge edge = graph.AddEdge(agentModel.Name, producedEvent);
                    edge.TargetNode.Attr.Shape = Shape.Diamond;
                    edge.TargetNode.Attr.FillColor = Color.Gray;
                    edge.Attr.Color = Color.Orange;
                }
            }

            OnMessage(new GraphCreated(graph, messageData));
        }
    }
}
