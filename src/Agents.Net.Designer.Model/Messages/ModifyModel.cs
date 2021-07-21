#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModifyModel : Message
    {
        public ModifyModel(Message predecessorMessage, Modification modification, bool isLast,
                           CommunityModel currentVersion)
            : base(predecessorMessage)
        {
            Modification = modification;
            IsLast = isLast;
            CurrentVersion = currentVersion;
        }

        public ModifyModel(IEnumerable<Message> predecessorMessages, Modification modification, bool isLast,
                           CommunityModel currentVersion)
            : base(predecessorMessages)
        {
            Modification = modification;
            IsLast = isLast;
            CurrentVersion = currentVersion;
        }
        
        public Modification Modification { get; }
        public bool IsLast { get; }
        public CommunityModel CurrentVersion { get; }

        protected override string DataToString()
        {
            return $"{nameof(Modification)}: {Modification}; {nameof(IsLast)}: {IsLast}";
        }
    }
}