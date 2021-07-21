#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;

namespace Agents.Net.Designer.CodeGenerator
{
    public class FileGenerationResult
    {
        public FileGenerationResult(FileType fileType, string name, string ns,
                                   string path)
        {
            FileType = fileType;
            Name = name;
            Namespace = ns;
            Path = path;
        }

        public override string ToString()
        {
            return $"{nameof(FileType)}: {FileType}, {nameof(Name)}: {Name}, {nameof(Namespace)}: {Namespace}, {nameof(Path)}: {Path}";
        }

        public FileType FileType { get; }
        public string Name { get; }
        public string Namespace { get; }
        public string Path { get; }
    }

    [Flags]
    public enum FileType
    {
        Message = 0,
        Agent = 1,
        Intercepting = 2,
        Decorator = 4,
        InterceptorAgent = Agent | Intercepting,
        MessageDecorator = Message | Decorator
    }
}