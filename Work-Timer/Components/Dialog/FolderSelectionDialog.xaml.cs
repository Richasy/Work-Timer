using Lib.Share.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WorkTimer.Models.Enums;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace WorkTimer.Components.Dialog
{
    public sealed partial class FolderSelectionDialog : ContentDialog
    {
        private ObservableCollection<FolderItem> FolderCollection = App._vm.FolderCollection;
        private HistoryItem _source = null;
        public FolderSelectionDialog()
        {
            this.InitializeComponent();
            Title = App._instance.App.GetLocalizationTextFromResource(LanguageName.MoveHistory);
            PrimaryButtonText = App._instance.App.GetLocalizationTextFromResource(LanguageName.Confirm);
            CloseButtonText = App._instance.App.GetLocalizationTextFromResource(LanguageName.Cancel);
        }

        public FolderSelectionDialog(HistoryItem item):this()
        {
            _source = item;
            FolderComboBox.SelectedItem = FolderCollection.Where(p => p.Id == item.FolderId).FirstOrDefault();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            var newFolder = FolderComboBox.SelectedItem as FolderItem;
            if (newFolder.Id != _source.Id)
            {
                App._vm.MoveHistory(_source, newFolder.Id);
            }
            Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
