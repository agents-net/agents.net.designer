#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.IO;
using System.Xml.Linq;
using Agents.Net.Designer.FileSystem.Messages;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;
using Ganss.XSS;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Agents
{
    [Consumes(typeof(ExportImageRequested))]
    [Consumes(typeof(GraphCreated))]
    [Consumes(typeof(FileOpened))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(ImageExported))]
    public class GraphToSvgConverter : Agent
    {
        private readonly MessageCollector<ExportImageRequested, GraphCreated> collector;
        private readonly string svgNamespace = "http://www.w3.org/2000/svg";
        private readonly MessageGate<FileOpening, FileOpened> gate = new();

        public GraphToSvgConverter(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ExportImageRequested, GraphCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ExportImageRequested, GraphCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            Graph layoutGraph = set.Message2.Graph;
            gate.SendAndContinue(new FileOpening(set.Message1.Path, set), OnMessage,
                                 result =>
                                 {
                                     Stream svgFile = result.EndMessage.Data;
                                     svgFile.SetLength(0);
                                     HtmlSanitizer sanitizer = new();
                                     SvgGraphWriter graphWriter = new(svgFile, layoutGraph)
                                     {
                                         NodeSanitizer = s => sanitizer.Sanitize(s),
                                         AttrSanitizer = s => sanitizer.Sanitize(s)
                                     };
                                     graphWriter.Write();

                                     //patch font
                                     svgFile.Seek(0, SeekOrigin.Begin);
                                     XDocument document = XDocument.Load(svgFile);
                                     foreach (XElement textElement in document.Descendants(
                                         XName.Get("text", svgNamespace)))
                                     {
                                         XAttribute family = textElement.Attribute("font-family");
                                         XAttribute size = textElement.Attribute("font-size");
                                         family?.SetValue(Label.DefaultFontName);
                                         size?.SetValue(Label.DefaultFontSize);
                                     }

                                     svgFile.Seek(0, SeekOrigin.Begin);
                                     document.Save(svgFile);

                                     OnMessage(new ImageExported(set.Message1.Path, result.EndMessage));
                                 });
        }

        protected override void ExecuteCore(Message messageData)
        {
            gate.Check(messageData);
            collector.TryPush(messageData);
        }
    }
}
