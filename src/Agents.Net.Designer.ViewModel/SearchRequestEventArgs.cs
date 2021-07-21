#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

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