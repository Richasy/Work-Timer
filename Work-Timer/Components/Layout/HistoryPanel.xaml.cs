using Lib.Share.Models;
using System;
using Windows.UI.Xaml.Controls;
using WorkTimer.Models.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WorkTimer.Components.Layout
{
    public sealed partial class HistoryPanel : UserControl
    {
        AppViewModel vm = App._vm;
        public HistoryPanel()
        {
            this.InitializeComponent();
            vm.HistoryPanel = this;
            vm.CurrentSelectedFolderChanged += SelectedFolderChanged;
        }

        private void SelectedFolderChanged(object sender, FolderItem e)
        {
            TitleBlock.Text = e.Name;
        }

        private void HistoryListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private async void RemoveItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem).Tag as HistoryItem;
            await vm.RemoveHistory(item);
        }
    }
}
