#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model
{
    public class Modification
    {
        public Modification(ModificationType modificationType, object oldValue, object newValue,
                            object target, PropertySpecifier property)
        {
            ModificationType = modificationType;
            OldValue = oldValue;
            NewValue = newValue;
            Target = target;
            Property = property;
        }

        public ModificationType ModificationType { get; }
        public object OldValue { get; }
        public object NewValue { get; }
        public object Target { get; }
        public PropertySpecifier Property { get; }

        public override string ToString()
        {
            return $"{nameof(ModificationType)}: {ModificationType}, {nameof(OldValue)}: {OldValue}, {nameof(NewValue)}: {NewValue}, {nameof(Target)}: {Target}, {nameof(Property)}: {Property}";
        }
    }
}