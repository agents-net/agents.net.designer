using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModifyModel : Message
    {
        public ModifyModel(ModelModification modificationType, object oldValue, object newValue, object target,
            PropertySpecifier property, Message predecessorMessage) 
            : base(predecessorMessage)
        {
            ModificationType = modificationType;
            OldValue = oldValue;
            NewValue = newValue;
            Target = target;
            Property = property;
        }

        public ModifyModel(ModelModification modificationType, object oldValue, object newValue, object target,
            PropertySpecifier property, IEnumerable<Message> predecessorMessages) 
            : base(predecessorMessages)
        {
            ModificationType = modificationType;
            OldValue = oldValue;
            NewValue = newValue;
            Target = target;
            Property = property;
        }
        
        public ModelModification ModificationType { get; }
        public object OldValue { get; }
        public object NewValue { get; }
        public object Target { get; }
        public PropertySpecifier Property { get; }
        
        protected override string DataToString()
        {
            return $"{nameof(ModificationType)}: {ModificationType}, {nameof(OldValue)}: {OldValue}, {nameof(NewValue)}: {NewValue}, {nameof(Target)}: {Target}, {nameof(Property)}: {Property}";
        }
    }
}