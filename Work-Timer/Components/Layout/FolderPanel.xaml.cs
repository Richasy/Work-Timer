using Lib.Share.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WorkTimer.Components.Dialog;
using WorkTimer.Models.Core;
using WorkTimer.Models.Enums;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace WorkTimer.Components.Layout
{
    public sealed partial class FolderPanel : UserControl
    {
        AppViewModel vm = App._vm;
        public FolderPanel()
        {
            this.InitializeComponent();
            vm.FolderPanel = this;
            vm.CurrentSelectedFolderChanged += SelectedFolder_Changed;
        }

        private void SelectedFolder_Changed(object sender, FolderItem e)
        {
            FolderListView.SelectedItem = e;
        }

        private async void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderDialog();
            await dialog.ShowAsync();
        }

        private void FolderListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var folder = e.ClickedItem as FolderItem;
            if (!vm.CurrentSelectedFolder.Equals(folder))
            {
                vm.CurrentSelectedFolder = folder;
            }
        }

        private async void ModifyItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem).Tag as FolderItem;
            var dialog = new FolderDialog(item);
            await dialog.ShowAsync();
        }

        private async void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem).Tag as FolderItem;
            await vm.RemoveFolder(item);
        }
    }
}
