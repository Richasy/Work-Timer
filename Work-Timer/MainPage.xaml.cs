using Lib.Share.Models;
using Richasy.Controls.UWP.Models.UI;
using Richasy.Font.UWP;
using Richasy.Font.UWP.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WorkTimer.Models.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WorkTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : RichasyPage
    {
        public AppViewModel vm = App._vm;
        public MainPage():base()
        {
            this.InitializeComponent();
            TitleBox.Text = "Untitled";
            vm.IsTimingChanged += IsTiming_Changed;
            vm.CurrentSelectedFolderChanged += CurrentSelectedFolder_Changed;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += CloseRequested;
        }

        private async void CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;
            await vm.SaveData();
            App.Current.Exit();
        }

        private void CurrentSelectedFolder_Changed(object sender, FolderItem e)
        {
            GroupNameBlock.Text = e.Name;
        }

        private void IsTiming_Changed(object sender, bool e)
        {
            var icon = e ? FeatherSymbol.Pause : FeatherSymbol.Play;
            StatusIcon.Symbol = icon;
            if (e)
            {
                vm.BeginStamp = DateTime.Now;
            }
            else
            {
                var ts = DateTime.Now - vm.BeginStamp;
                var historyItem = new HistoryItem(TitleBox.Text ?? "Untitled", vm.CurrentSelectedFolder.Id, Convert.ToInt32(ts.TotalSeconds));
                vm.AddHistoryItem(historyItem);
                TitleBox.Text = "Untitled";
            }
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ShowSettingPopup();
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            vm.IsTiming = !vm.IsTiming;
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            SubSplitView.IsPaneOpen = !SubSplitView.IsPaneOpen;
        }

        private async void RichasyPage_Loaded(object sender, RoutedEventArgs e)
        {
            vm.CheckUpdate();
            await App._vm.Init();
        }

        private async void CompactButton_Click(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.ViewMode == ApplicationViewMode.CompactOverlay)
            {
                await view.TryEnterViewModeAsync(ApplicationViewMode.Default);
                VisualStateManager.GoToState(this, "Default",false);
            }
            else
            {
                await view.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
                VisualStateManager.GoToState(this, "Compact", false);
            }
                
        }

        private void RichasyPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            if (width > 1300)
            {
                MainSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                SubSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
            }
            else if (width > 900)
            {
                MainSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                SubSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
            }
            else
            {
                MainSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                SubSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
            }
        }
    }
}
