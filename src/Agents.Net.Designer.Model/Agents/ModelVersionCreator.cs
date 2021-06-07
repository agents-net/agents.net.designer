using System;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModelModified))]
    [Consumes(typeof(ModelLoaded))]
    [Produces(typeof(ModelVersionCreated))]
    public class ModelVersionCreator : Agent
    {
        public ModelVersionCreator(IMessageBoard messageBoard, string name = null)
            : base(messageBoard, name)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out ModelModified modified))
            {
                OnMessage(new ModelVersionCreated(messageData, modified.Model));
            }
            else if(messageData.TryGet(out ModelLoaded loaded))
            {
                OnMessage(new ModelVersionCreated(messageData, loaded.Model));
            }
        }
    }
}