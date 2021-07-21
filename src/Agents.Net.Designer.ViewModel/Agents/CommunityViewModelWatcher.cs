using System;
using System.Collections.Generic;
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
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModifyModel))]
    public class CommunityViewModelWatcher : Agent
    {
        private Tuple<CommunityViewModel, Message[], CommunityModel> latestData;
        private readonly MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged> collector;

        public CommunityViewModelWatcher(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModelVersionCreated, SelectedTreeViewItemChanged> set)
        {
            if (set.Message2.SelectedItem is not CommunityViewModel communityViewModel)
            {
                return;
            }
            Tuple<CommunityViewModel, Message[], CommunityModel> oldData = Interlocked.Exchange(ref latestData, new Tuple<CommunityViewModel, Message[], CommunityModel>(communityViewModel, set.ToArray(), set.Message1.Model));
            if (oldData?.Item1 != null)
            {
                oldData.Item1.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            communityViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CommunityModel oldModel = latestData.Item3;
            switch (e.PropertyName)
            {
                case nameof(CommunityViewModel.Name):
                    string newNamespace = latestData.Item1.Name == "<Root>" ? string.Empty : latestData.Item1.Name;
                    OnMessage(new ModificationRequest( 
                                              latestData.Item2, new Modification(ModificationType.Change,
                                              oldModel.GeneratorSettings.PackageNamespace,
                                              newNamespace,
                                              oldModel.GeneratorSettings,
                                              new GeneratorSettingsPackageNamespaceProperty())));
                    break;
                case nameof(CommunityViewModel.GenerateAutofacModule):
                    OnMessage(new ModificationRequest( 
                                              latestData.Item2, new Modification(ModificationType.Change,
                                              oldModel.GeneratorSettings.GenerateAutofacModule,
                                              latestData.Item1.GenerateAutofacModule,
                                              oldModel.GeneratorSettings,
                                              new GeneratorSettingsGenerateAutofacProperty())));
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && latestData != null)
            {
                latestData.Item1.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            base.Dispose(disposing);
        }
    }
}
