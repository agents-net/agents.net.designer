using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(JsonViewModelApplied))]
    public class JsonTextExampleLoader : Agent
    {
        private const string ExampleText = 
@"{
   ""Messages"":[
   ],
   ""Agents"":[
   ]
}";        public JsonTextExampleLoader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            JsonViewModelApplied applied = messageData.Get<JsonViewModelApplied>();
            applied.ViewModel.Text = ExampleText;
        }
    }
}
