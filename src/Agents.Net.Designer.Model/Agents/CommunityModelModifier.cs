using System;
using System.Collections.Generic;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModificationResult))]
    public class CommunityModelModifier : Agent
    {
        private readonly MessageCollector<ModifyModel, ModelVersionCreated> collector;
        
        public CommunityModelModifier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, ModelVersionCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);
            
            ExecuteCollectedMessages(set.Message1, set.Message2.Model, set);
        }

        private void ExecuteCollectedMessages(ModifyModel modifyModel, CommunityModel model, IEnumerable<Message> set)
        {
            if (modifyModel.Target is not CommunityModel)
            {
                return;
            }

            CommunityModel updatedModel;
            switch (modifyModel.Property)
            {
                case PackageMessagesProperty _:
                    updatedModel = UpdateMessage(modifyModel.OldValue.AssertTypeOf<MessageModel>(),
                                                 modifyModel.NewValue.AssertTypeOf<MessageModel>(),
                                                 modifyModel.ModificationType,
                                                 model);
                    break;
                case PackageAgentsProperty _:
                    updatedModel = UpdateAgent(modifyModel.OldValue.AssertTypeOf<AgentModel>(),
                                               modifyModel.NewValue.AssertTypeOf<AgentModel>(),
                                               modifyModel.ModificationType,
                                               model);
                    break;
                default:
                    throw new InvalidOperationException($"Property {modifyModel.Property} unknown for model.");
            }
            OnMessage(new ModificationResult(updatedModel, set));
        }

        private CommunityModel UpdateAgent(AgentModel oldValue, AgentModel newValue, ModelModification modificationType,
                                           CommunityModel model)
        {
            List<AgentModel> models = new(model.Agents);
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
            List<MessageModel> models = new(model.Messages);
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