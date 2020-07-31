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

    public class AgentNameProperty : PropertySpecifier
    {
        public AgentNameProperty() : base(nameof(AgentModel.Name))
        {
        }
    }

    public class AgentNamespaceProperty : PropertySpecifier
    {
        public AgentNamespaceProperty() : base(nameof(AgentModel.Namespace))
        {
        }
    }

    public class AgentConsumingMessagesProperty : PropertySpecifier
    {
        public AgentConsumingMessagesProperty() : base(nameof(AgentModel.ConsumingMessages))
        {
        }
    }

    public class AgentProducedMessagesProperty : PropertySpecifier
    {
        public AgentProducedMessagesProperty() : base(nameof(AgentModel.ProducedMessages))
        {
        }
    }

    public class AgentIncomingEventsProperty : PropertySpecifier
    {
        public AgentIncomingEventsProperty() : base(nameof(AgentModel.IncomingEvents))
        {
        }
    }

    public class AgentProducedEventsProperty : PropertySpecifier
    {
        public AgentProducedEventsProperty() : base(nameof(AgentModel.ProducedEvents))
        {
        }
    }
}