namespace Agents.Net.Designer.Model.Messages
{
    public class PropertySpecifier
    {
        private readonly string propertyName;

        public PropertySpecifier(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override string ToString()
        {
            return $"{nameof(propertyName)}: {propertyName}";
        }

        protected bool Equals(PropertySpecifier other)
        {
            return propertyName == other.propertyName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((PropertySpecifier) obj);
        }

        public override int GetHashCode()
        {
            return propertyName.GetHashCode();
        }
    }
    
    public class GeneratorSettingProperty : PropertySpecifier
    {
        public GeneratorSettingProperty() : base(nameof(CommunityModel.GeneratorSettings))
        {
        }
    }

    public class MessagesProperty : PropertySpecifier
    {
        public MessagesProperty() : base(nameof(CommunityModel.Messages))
        {
        }
    }

    public class AgentsProperty : PropertySpecifier
    {
        public AgentsProperty() : base(nameof(CommunityModel.Agents))
        {
        }
    }
}