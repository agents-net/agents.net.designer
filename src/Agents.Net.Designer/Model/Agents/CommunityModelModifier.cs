using System;
using System.Collections.Generic;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModelUpdated))]
    public class CommunityModelModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelUpdated> collector;
        
        public CommunityModelModifier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            if (!(set.Message1.Target is CommunityModel))
            {
                return;
            }

            CommunityModel updatedModel;
            switch (set.Message1.Property)
            {
                case GeneratorSettingProperty _:
                    updatedModel = new CommunityModel(set.Message1.NewValue.AssertTypeOf<GeneratorSettings>(),
                                                      set.Message2.Model.Agents, set.Message2.Model.Messages);
                    break;
                case MessagesProperty _:
                    updatedModel = UpdateMessage(set.Message1.OldValue.AssertTypeOf<MessageModel>(),
                                                 set.Message1.NewValue.AssertTypeOf<MessageModel>(),
                                                 set.Message1.ModificationType,
                                                 set.Message2.Model);
                    break;
                case AgentsProperty _:
                    updatedModel = UpdateAgent(set.Message1.OldValue.AssertTypeOf<AgentModel>(),
                                                 set.Message1.NewValue.AssertTypeOf<AgentModel>(),
                                                 set.Message1.ModificationType,
                                                 set.Message2.Model);
                    break;
                default:
                    throw new InvalidOperationException($"Property {set.Message1.Property} unknown for model.");
            }
            OnMessage(new ModelUpdated(updatedModel, set));
        }

        private CommunityModel UpdateAgent(AgentModel oldValue, AgentModel newValue, ModelModification modificationType,
                                           CommunityModel model)
        {
            List<AgentModel> models = new List<AgentModel>(model.Agents);
            switch (modificationType)
            {
                case ModelModification.Add:
                    models.Add(newValue);
                    break;
                case ModelModification.Remove:
                    models.Remove(oldValue);
                    break;
                case ModelModification.Change:
                    models[models.FindIndex(m => m == oldValue)] = newValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown modification {modificationType}");
            }
            return new CommunityModel(model.GeneratorSettings, models.ToArray(), model.Messages);
        }

        private CommunityModel UpdateMessage(MessageModel oldValue, MessageModel newValue, ModelModification modificationType,
                                             CommunityModel model)
        {
            List<MessageModel> models = new List<MessageModel>(model.Messages);
            switch (modificationType)
            {
                case ModelModification.Add:
                    models.Add(newValue);
                    break;
                case ModelModification.Remove:
                    models.Remove(oldValue);
                    break;
                case ModelModification.Change:
                    models[models.FindIndex(m => m == oldValue)] = newValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown modification {modificationType}");
            }
            return new CommunityModel(model.GeneratorSettings, model.Agents, models.ToArray());
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}