using System;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModificationCompleted))]
    [Consumes(typeof(ModelLoaded))]
    [Produces(typeof(ModelVersionCreated))]
    public class ModelVersionCreator : Agent
    {
        public ModelVersionCreator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out ModificationCompleted completed))
            {
                OnMessage(new ModelVersionCreated(messageData, completed.Model));
            }
            else if(messageData.TryGet(out ModelLoaded loaded))
            {
                OnMessage(new ModelVersionCreated(messageData, loaded.Model));
            }
        }
    }
}