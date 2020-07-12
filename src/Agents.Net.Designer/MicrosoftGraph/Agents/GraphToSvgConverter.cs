using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Agents.Net;
using Agents.Net.Designer.MicrosoftGraph.Messages;
using Agents.Net.Designer.Model.Messages;
using Ganss.XSS;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.MicrosoftGraph.Agents
{
    public class GraphToSvgConverter : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition GraphToSvgConverterDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      ExportImageRequested.ExportImageRequestedDefinition,
                                      GraphCreated.GraphCreatedDefinition
                                  },
                                  new []
                                  {
                                      ImageExported.ImageExportedDefinition
                                  });

        #endregion

        private readonly MessageCollector<ExportImageRequested, GraphCreated> collector;
        private readonly HashSet<ExportImageRequested> processedRequests = new HashSet<ExportImageRequested>();
        private readonly string svgNamespace = "http://www.w3.org/2000/svg";

        public GraphToSvgConverter(IMessageBoard messageBoard) : base(GraphToSvgConverterDefinition, messageBoard)
        {
            collector = new MessageCollector<ExportImageRequested, GraphCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ExportImageRequested, GraphCreated> set)
        {
            lock (processedRequests)
            {
                if (!processedRequests.Add(set.Message1))
                {
                    return;
                }
            }

            
            Graph layoutGraph = set.Message2.Graph;
            using FileStream svgFile = File.Open(set.Message1.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            svgFile.SetLength(0);
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            SvgGraphWriter graphWriter = new SvgGraphWriter(svgFile, layoutGraph)
            {
                NodeSanitizer = s => sanitizer.Sanitize(s),
                AttrSanitizer = s => sanitizer.Sanitize(s)
            };
            graphWriter.Write();

            //patch font
            svgFile.Seek(0, SeekOrigin.Begin);
            XDocument document = XDocument.Load(svgFile);
            foreach (XElement textElement in document.Descendants(XName.Get("text", svgNamespace)))
            {
                XAttribute family = textElement.Attribute("font-family");
                XAttribute size = textElement.Attribute("font-size");
                family?.SetValue(Label.DefaultFontName);
                size?.SetValue(Label.DefaultFontSize);
            }
            svgFile.Seek(0, SeekOrigin.Begin);
            document.Save(svgFile);

            OnMessage(new ImageExported(set.Message1.Path, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
