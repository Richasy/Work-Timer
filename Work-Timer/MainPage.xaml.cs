using Lib.Share.Models;
using Richasy.Font.UWP;
using Richasy.Font.UWP.Enums;
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
using WorkTimer.Models.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WorkTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public AppViewModel vm = App._vm;
        public MainPage()
        {
            this.InitializeComponent();
            TitleBox.Text = "Untitled";
            vm.IsTimingChanged += IsTiming_Changed;
            vm.CurrentSelectedFolderChanged += CurrentSelectedFolder_Changed;
        }

        private void CurrentSelectedFolder_Changed(object sender, FolderItem e)
        {
            GroupNameBlock.Text = e.Name;
        }

        private void IsTiming_Changed(object sender, bool e)
        {
            var icon = e ? new FeatherIcon(FeatherSymbol.Square) : new FeatherIcon(FeatherSymbol.Play);
            if (e)
            {
                vm.BeginStamp = DateTime.Now;
            }
            else
            {
                var ts = DateTime.Now - vm.BeginStamp;
                var historyItem = new HistoryItem(TitleBox.Text ?? "Untitled", vm.CurrentSelectedFolder.Id, Convert.ToInt32(ts.TotalSeconds));
                vm.AddHistoryItem(historyItem);
            }
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
