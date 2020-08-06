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

    public class GeneratorSettingsPackageNamespaceProperty : PropertySpecifier
    {
        public GeneratorSettingsPackageNamespaceProperty() : base(nameof(GeneratorSettings.PackageNamespace))
        {
        }
    }

    public class GeneratorSettingsGenerateAutofacProperty : PropertySpecifier
    {
        public GeneratorSettingsGenerateAutofacProperty() : base(nameof(GeneratorSettings.GenerateAutofacModule))
        {
        }
    }

    public class PackageMessagesProperty : PropertySpecifier
    {
        public PackageMessagesProperty() : base(nameof(CommunityModel.Messages))
        {
        }
    }

    public class PackageAgentsProperty : PropertySpecifier
    {
        public PackageAgentsProperty() : base(nameof(CommunityModel.Agents))
        {
        }
    }

    public class MessageNameProperty : PropertySpecifier
    {
        public MessageNameProperty() : base(nameof(MessageModel.Name))
        {
        }
    }

    public class MessageNamespaceProperty : PropertySpecifier
    {
        public MessageNamespaceProperty() : base(nameof(MessageModel.Namespace))
        {
        }
    }

    public class MessageDecoratorDecoratedMessageProperty : PropertySpecifier
    {
        public MessageDecoratorDecoratedMessageProperty() : base(nameof(MessageDecoratorModel.DecoratedMessage))
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

    public class InterceptorAgentInterceptingMessagesProperty : PropertySpecifier
    {
        public InterceptorAgentInterceptingMessagesProperty() : base(nameof(InterceptorAgentModel.InterceptingMessages))
        {
        }
    }
}