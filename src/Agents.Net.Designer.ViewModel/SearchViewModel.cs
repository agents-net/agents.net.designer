using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MaterialDesignExtensions.Model;

namespace Agents.Net.Designer.ViewModel
{
    public class SearchViewModel : ISearchSuggestionsSource, INotifyPropertyChanged
    {
        private string searchTerm;
        private AvailableItemsViewModel searchSource;

        public SearchViewModel()
        {
            SearchCommand = new RelayCommand(o =>
            {
                string searchResult = GetAutoCompletion(SearchTerm).FirstOrDefault() ?? string.Empty;
                OnSearchRequested(new SearchRequestEventArgs(searchResult));
            });
        }

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                if (value == searchTerm) return;
                searchTerm = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<TreeViewItem> SearchItems =>
            searchSource != null ? searchSource.AvailableMessages.Concat<TreeViewItem>(searchSource.AvailableAgents) : Enumerable.Empty<TreeViewItem>();
        
        public ICommand SearchCommand { get; }

        public event EventHandler<SearchRequestEventArgs> SearchRequested;

        public IList<string> GetSearchSuggestions()
        {
            return SearchItems.Select(i => i.Name)
                              .Distinct()
                              .OrderBy(n => n).ToList();
        }

        public IList<string> GetAutoCompletion(string searchTerm)
        {
            IList<string> suggestions = GetSearchSuggestions();
            SortedHashSet result = new();
            result.AddRange(GetDirectMatch());
            result.AddRange(GetSplitStartsWithMatch());
            result.AddRange(GetStartsWithMatch());
            result.AddRange(GetContainsMatch());
            
            return result;

            IEnumerable<string> GetDirectMatch()
            {
                return suggestions.Where(n => n.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            IEnumerable<string> GetSplitStartsWithMatch()
            {
                string[] searchSplit = Regex.Split(searchTerm, @"[A-Z]");
                foreach (string suggestion in suggestions)
                {
                    string[] split = Regex.Split(suggestion, @"[A-Z]");
                    int index = 0;
                    foreach (string s in searchSplit)
                    {
                        index = split.Skip(index).TakeWhile(p => !p.StartsWith(s, StringComparison.Ordinal))
                                     .Count() + index + 1;
                        if (index > split.Length)
                        {
                            break;
                        }
                    }

                    if (index <= split.Length)
                    {
                        yield return suggestion;
                    }
                }
            }

            IEnumerable<string> GetStartsWithMatch()
            {
                return suggestions.Where(n => n.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            IEnumerable<string> GetContainsMatch()
            {
                return suggestions.Where(n => n.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));
            }
        }

        public void SetSearchSource(AvailableItemsViewModel source)
        {
            searchSource = source;
        }

        protected virtual void OnSearchRequested(SearchRequestEventArgs e)
        {
            SearchRequested?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class SortedHashSet : IList<string>
        {
            private readonly List<string> inner = new List<string>();
            private readonly HashSet<string> hashSet = new HashSet<string>();
            
            public void AddRange(IEnumerable<string> items)
            {
                foreach (string item in items)
                {
                    Add(item);
                }
            }

            #region IList impl

            public IEnumerator<string> GetEnumerator()
            {
                return inner.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) inner).GetEnumerator();
            }

            public void Add(string item)
            {
                if (hashSet.Add(item))
                {
                    inner.Add(item);
                }
            }

            public void Clear()
            {
                hashSet.Clear();
                inner.Clear();
            }

            public bool Contains(string item)
            {
                return inner.Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                inner.CopyTo(array, arrayIndex);
            }

            public bool Remove(string item)
            {
                hashSet.Remove(item);
                return inner.Remove(item);
            }

            public int Count => inner.Count;

            public bool IsReadOnly => false;

            public int IndexOf(string item)
            {
                return inner.IndexOf(item);
            }

            public void Insert(int index, string item)
            {
                if (hashSet.Add(item))
                {
                    inner.Insert(index, item);
                }
            }

            public void RemoveAt(int index)
            {
                hashSet.Remove(inner[index]);
                inner.RemoveAt(index);
            }

            public string this[int index]
            {
                get => inner[index];
                set => inner[index] = value;
            }

            #endregion
        }
    }
}