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
using WorkTimer.Models.Enums;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace WorkTimer.Components.Dialog
{
    public sealed partial class ConfirmDialog : ContentDialog
    {
        public ConfirmDialog()
        {
            this.InitializeComponent();
            Title = App._instance.App.GetLocalizationTextFromResource(LanguageName.Warning);
            PrimaryButtonText = App._instance.App.GetLocalizationTextFromResource(LanguageName.Confirm);
            CloseButtonText = App._instance.App.GetLocalizationTextFromResource(LanguageName.Cancel);
        }
        public ConfirmDialog(string msg):this()
        {
            DisplayBlock.Text = msg;
        }
        public ConfirmDialog(LanguageName language):this()
        {
            DisplayBlock.Text = App._instance.App.GetLocalizationTextFromResource(language);
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
