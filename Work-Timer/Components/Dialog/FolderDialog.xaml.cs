using Lib.Share.Models;
using Richasy.Font.UWP.Enums;
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
    public sealed partial class FolderDialog : ContentDialog
    {
        public ObservableCollection<FeatherSymbol> SymbolCollection = App._vm.SymbolCollection;
        private FolderItem _source = null;
        public FolderDialog()
        {
            this.InitializeComponent();
            Title = App._instance.App.GetLocalizationTextFromResource(LanguageName.FolderDialogTitle);
            DescriptionBlock.Text = App._instance.App.GetLocalizationTextFromResource(LanguageName.FolderDialogDescription);
            PrimaryButtonText= App._instance.App.GetLocalizationTextFromResource(LanguageName.Confirm);
            CloseButtonText = App._instance.App.GetLocalizationTextFromResource(LanguageName.Cancel);
        }

        public FolderDialog(FolderItem item):this()
        {
            _source = item;
            ShowIcon.Symbol = (FeatherSymbol)item.Icon;
            SymbolGridView.SelectedItem = (FeatherSymbol)item.Icon;
            NameBox.Text = item.Name;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            if (string.IsNullOrEmpty(NameBox.Text))
            {
                App._vm.ShowPopup(LanguageName.FieldEmpty, true);
                return;
            }
            FolderItem item = null;
            if (_source == null)
                item = new FolderItem(NameBox.Text, ShowIcon.Symbol);
            else
            {
                item = _source;
                item.Name = NameBox.Text;
                item.Icon = ShowIcon.Symbol;
            }
            App._vm.AddOrUpdateFolderItem(item);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void SymbolGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var icon = (FeatherSymbol)e.ClickedItem;
            ShowIcon.Symbol = icon;
            SymbolFlyout.Hide();
        }

        private void SymbolFlyout_Opened(object sender, object e)
        {
            if (SymbolGridView.SelectedItem != null)
            {
                SymbolGridView.ScrollIntoView(SymbolGridView.SelectedItem);
            }
        }
    }
}
