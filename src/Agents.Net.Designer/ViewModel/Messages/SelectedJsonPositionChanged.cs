using System.Collections.Generic;

namespace Agents.Net.Designer.ViewModel.Messages
{
    public class SelectedJsonPositionChanged : Message
    {        public SelectedJsonPositionChanged(int startLineNumber, int startLineColumn, int endLineNumber,
                                           int endLineColumn, Message predecessorMessage,
                                           params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            StartLineNumber = startLineNumber;
            StartLineColumn = startLineColumn;
            EndLineNumber = endLineNumber;
            EndLineColumn = endLineColumn;
        }

        public SelectedJsonPositionChanged(int startLineNumber, int startLineColumn, int endLineNumber,
                                           int endLineColumn, IEnumerable<Message> predecessorMessages,
                                           params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            StartLineNumber = startLineNumber;
            StartLineColumn = startLineColumn;
            EndLineNumber = endLineNumber;
            EndLineColumn = endLineColumn;
        }

        public int StartLineNumber { get; }
        public int StartLineColumn { get; }

        public int EndLineNumber { get; }
        public int EndLineColumn { get; }

        protected override string DataToString()
        {
            return $"{nameof(StartLineNumber)}: {StartLineNumber}; {nameof(StartLineColumn)}: {StartLineColumn};" +
                   $"{nameof(EndLineNumber)}: {EndLineNumber}; {nameof(EndLineColumn)}: {EndLineColumn}";
        }
    }
}
