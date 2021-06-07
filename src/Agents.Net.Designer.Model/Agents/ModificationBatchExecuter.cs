using System;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModelModified))]
    [Produces(typeof(ModelModificationBatch))]
    public class ModificationBatchExecuter : Agent
    {
        public ModificationBatchExecuter(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.MessageDomain.Root.TryGet(out ModelModificationBatch batch) &&
                ModelModificationBatch.TryContinue(batch, new []{messageData}, out ModelModificationBatch newBatch))
            {
                OnMessage(newBatch);
            }
        }
    }
}