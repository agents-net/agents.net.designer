#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Produces(typeof(ModificationResult))]
    public class CommunityModelModifier : Agent
    {
        public CommunityModelModifier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        private CommunityModel UpdateAgent(AgentModel oldValue, AgentModel newValue, ModificationType modificationTypeType,
                                           CommunityModel model)
        {
            List<AgentModel> models = new(model.Agents);
            switch (modificationTypeType)
            {
                case ModificationType.Add:
                    models.Add(newValue);
                    break;
                case ModificationType.Remove:
                    models.Remove(oldValue);
                    break;
                case ModificationType.Change:
                    models[models.FindIndex(m => m == oldValue)] = newValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown modification {modificationTypeType}");
            }
            return new CommunityModel(model.GeneratorSettings, models.ToArray(), model.Messages);
        }

        private CommunityModel UpdateMessage(MessageModel oldValue, MessageModel newValue, ModificationType modificationTypeType,
                                             CommunityModel model)
        {
            List<MessageModel> models = new(model.Messages);
            switch (modificationTypeType)
            {
                case ModificationType.Add:
                    models.Add(newValue);
                    break;
                case ModificationType.Remove:
                    models.Remove(oldValue);
                    break;
                case ModificationType.Change:
                    models[models.FindIndex(m => m == oldValue)] = newValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown modification {modificationTypeType}");
            }
            return new CommunityModel(model.GeneratorSettings, model.Agents, models.ToArray());
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModifyModel modifyModel = messageData.Get<ModifyModel>();if (modifyModel.Modification.Target is not CommunityModel)
            {
                return;
            }

            CommunityModel model = modifyModel.CurrentVersion;

            CommunityModel updatedModel;
            switch (modifyModel.Modification.Property)
            {
                case PackageMessagesProperty _:
                    updatedModel = UpdateMessage(modifyModel.Modification.OldValue.AssertTypeOf<MessageModel>(),
                                                 modifyModel.Modification.NewValue.AssertTypeOf<MessageModel>(),
                                                 modifyModel.Modification.ModificationType,
                                                 model);
                    break;
                case PackageAgentsProperty _:
                    updatedModel = UpdateAgent(modifyModel.Modification.OldValue.AssertTypeOf<AgentModel>(),
                                               modifyModel.Modification.NewValue.AssertTypeOf<AgentModel>(),
                                               modifyModel.Modification.ModificationType,
                                               model);
                    break;
                default:
                    throw new InvalidOperationException($"Property {modifyModel.Modification.Property} unknown for model.");
            }
            OnMessage(new ModificationResult(updatedModel, messageData));
        }
    }
}