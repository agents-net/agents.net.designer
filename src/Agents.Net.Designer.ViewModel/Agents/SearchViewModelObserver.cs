#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModelVersionCreated))]
    [Consumes(typeof(SearchViewModelCreated))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class SearchViewModelObserver : Agent
    {
        private SearchViewModel viewModel;
        private ModelVersionCreated latestVersion;
        public SearchViewModelObserver(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out SearchViewModelCreated viewModelCreated))
            {
                viewModel = viewModelCreated.ViewModel;
                viewModel.SearchRequested += ViewModelOnSearchRequested;
            }

            latestVersion = messageData.Get<ModelVersionCreated>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && viewModel != null)
            {
                viewModel.SearchRequested-= ViewModelOnSearchRequested;
            }
            base.Dispose(disposing);
        }

        private void ViewModelOnSearchRequested(object sender, SearchRequestEventArgs e)
        {
            ModelVersionCreated version = latestVersion;
            object modelObject = (object) version?.Model.Agents.FirstOrDefault(a => a.Name.Equals(e.SearchTerm))
                                 ?? version?.Model.Messages.FirstOrDefault(a => a.Name.Equals(e.SearchTerm));
            if (modelObject != null)
            {
                OnMessage(new SelectedModelObjectChanged(modelObject, version, SelectionSource.Internal));
            }
        }
    }
}