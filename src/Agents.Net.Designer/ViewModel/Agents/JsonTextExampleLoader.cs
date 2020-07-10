using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    public class JsonTextExampleLoader : Agent
    {
        private const string ExampleText = 
@"{
   ""Messages"":[
      {
         ""Name"":""InitializeMessage""
      },
      {
         ""Name"":""MessageA""
      }
   ],
   ""Agents"":[
      {
         ""Name"":""AgentA"",
         ""ConsumingMessages"":[
            ""InitializeMessage""
         ],
         ""ProducingMessages"":[
            ""MessageA""
         ]
      },
      {
         ""Name"":""AgentB"",
         ""ConsumingMessages"":[
            ""MessageA""
         ],
         ""ProducingMessages"":[

         ]
      }
   ]
}";
        #region Definition

        [AgentDefinition]
        public static AgentDefinition JsonTextExampleLoaderDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      JsonViewModelApplied.JsonViewModelAppliedDefinition
                                  },
                                  Array.Empty<MessageDefinition>());

        #endregion

        public JsonTextExampleLoader(IMessageBoard messageBoard) : base(JsonTextExampleLoaderDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            JsonViewModelApplied applied = messageData.Get<JsonViewModelApplied>();
            applied.ViewModel.Text = ExampleText;
        }
    }
}
