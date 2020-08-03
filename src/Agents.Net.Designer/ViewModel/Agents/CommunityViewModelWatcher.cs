using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedTreeViewItemChanged))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModifyModel))]
    public class CommunityViewModelWatcher : Agent, IDisposable
    {
        private Message changedMessage;
        private CommunityViewModel viewModel;
        private CommunityModel latestModel;

        public CommunityViewModelWatcher(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out ModelUpdated modelUpdated))
            {
                latestModel = modelUpdated.Model;
                return;
            }
            SelectedTreeViewItemChanged viewModelChanged = messageData.Get<SelectedTreeViewItemChanged>();
            if (!(viewModelChanged.SelectedItem is CommunityViewModel communityViewModel))
            {
                return;
            }

            CommunityViewModel oldViewModel = Interlocked.Exchange(ref viewModel, communityViewModel);
            if (oldViewModel != null)
            {
                oldViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            changedMessage = messageData;
            communityViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CommunityModel oldModel = latestModel;
            switch (e.PropertyName)
            {
                case nameof(CommunityViewModel.Name):
                    string newNamespace = viewModel.Name == "<Root>" ? string.Empty : viewModel.Name;
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.GeneratorSettings.PackageNamespace,
                                              newNamespace,
                                              oldModel.GeneratorSettings,
                                              new GeneratorSettingsPackageNamespaceProperty(), 
                                              changedMessage));
                    break;
                case nameof(CommunityViewModel.GenerateAutofacModule):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.GeneratorSettings.GenerateAutofacModule,
                                              viewModel.GenerateAutofacModule,
                                              oldModel.GeneratorSettings,
                                              new GeneratorSettingsGenerateAutofacProperty(), 
                                              changedMessage));
                    break;
            }
        }

        public void Dispose()
        {
            if (viewModel != null)
            {
                viewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
        }
    }
}
