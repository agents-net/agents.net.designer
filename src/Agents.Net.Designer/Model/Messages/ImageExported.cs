using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ImageExported : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition ImageExportedDefinition { get; } =
            new MessageDefinition(nameof(ImageExported));

        #endregion

        public ImageExported(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, ImageExportedDefinition, childMessages)
        {
            Path = path;
        }

        public ImageExported(string path, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, ImageExportedDefinition, childMessages)
        {
            Path = path;
        }

        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Path)}: {Path}";
        }
    }
}
