#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class FilesGenerated : Message
    {
        public FilesGenerated(FileGenerationResult[] fileResults, IEnumerable<FileGenerated> predecessorMessages)
            : base(predecessorMessages)
        {
            FileResults = fileResults;
        }

        public FileGenerationResult[] FileResults { get; }

        protected override string DataToString()
        {
            return $"{nameof(FileResults)}: {FileResults.Length}";
        }
    }
}
