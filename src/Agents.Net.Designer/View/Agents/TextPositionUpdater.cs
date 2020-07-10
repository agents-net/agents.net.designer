using System;
using Agents.Net;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.View.Agents
{
    public class TextPositionUpdater : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition TextPositionUpdaterDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      SelectedJsonPositionChanged.SelectedJsonPositionChangedDefinition,
                                      MainWindowCreated.MainWindowCreatedDefinition
                                  },
                                  Array.Empty<MessageDefinition>());

        #endregion

        private readonly MessageCollector<SelectedJsonPositionChanged, MainWindowCreated> collector;

        public TextPositionUpdater(IMessageBoard messageBoard) : base(TextPositionUpdaterDefinition, messageBoard)
        {
            collector = new MessageCollector<SelectedJsonPositionChanged, MainWindowCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<SelectedJsonPositionChanged, MainWindowCreated> set)
        {
            set.Message2.Window.Dispatcher.Invoke(() =>
            {
                int lineStart = set.Message2.Window.JsonTextBox.GetCharacterIndexFromLineIndex(set.Message1.StartLineNumber-1);
                int length = 0;
                if (set.Message1.EndLineNumber >= set.Message1.StartLineNumber)
                {
                    int lineEnd;
                    if (set.Message1.EndLineNumber > set.Message1.StartLineNumber)
                    {
                        lineEnd = set.Message2.Window.JsonTextBox.GetCharacterIndexFromLineIndex(set.Message1.EndLineNumber-3)+set.Message2.Window.JsonTextBox.GetLineLength(set.Message1.EndLineNumber-3)+1;
                    }
                    else
                    {
                        lineEnd = lineStart + set.Message1.EndLineColumn;
                    }

                    length = lineEnd - lineStart;
                }
                else
                {
                    int lastLine = set.Message2.Window.JsonTextBox.GetLineIndexFromCharacterIndex(set.Message2.Window.JsonTextBox.Text.Length - 1);
                    int lineEnd = set.Message2.Window.JsonTextBox.GetCharacterIndexFromLineIndex(lastLine-3)+set.Message2.Window.JsonTextBox.GetLineLength(lastLine-3)+1;
                    length = lineEnd - lineStart;
                }
                set.Message2.Window.JsonTextBox.Select(lineStart + set.Message1.StartLineColumn-1, length);
                set.Message2.Window.JsonTextBox.Focus();
                set.Message2.Window.JsonTextBox.ScrollToLine(set.Message1.StartLineNumber-1);
            });
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
