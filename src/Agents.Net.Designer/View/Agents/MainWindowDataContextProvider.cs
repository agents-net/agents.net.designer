using System;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.View.Agents
{
    public class MainWindowDataContextProvider : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition MainWindowDataContextProviderDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      JsonViewModelCreated.JsonViewModelCreatedDefinition,
                                      GraphViewModelCreated.GraphViewModelCreatedDefinition,
                                      MainWindowCreated.MainWindowCreatedDefinition
                                  },
                                  Array.Empty<MessageDefinition>());

        #endregion

        public MainWindowDataContextProvider(IMessageBoard messageBoard) : base(MainWindowDataContextProviderDefinition, messageBoard)
        {
            collector = new MessageCollector<JsonViewModelCreated, GraphViewModelCreated, MainWindowCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<JsonViewModelCreated, GraphViewModelCreated, MainWindowCreated> set)
        {
            set.Message3.Window.Dispatcher.Invoke(() =>
            {
                set.Message3.Window.DataContext = set.Message2.ViewModel;
                set.Message3.Window.JsonTextBox.DataContext = set.Message1.ViewModel;
            });
        }

        private readonly MessageCollector<JsonViewModelCreated, GraphViewModelCreated, MainWindowCreated> collector;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
