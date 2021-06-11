using System;

namespace Agents.Net.Designer.ViewModel
{
    public class SearchRequestEventArgs : EventArgs
    {
        public SearchRequestEventArgs(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public string SearchTerm { get; }
    }
}