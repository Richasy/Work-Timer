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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace WorkTimer.Components.Widget
{
    public sealed partial class WorkDurationBlock : UserControl
    {
        public WorkDurationBlock()
        {
            this.InitializeComponent();
            UpdatePresetDuration();
            App._vm.WorkDurationBlock = this;
            App._vm.HistoryChanged += HistoryChanged;
        }

        private void HistoryChanged(object sender, EventArgs e)
        {
            var todayList = App._vm.AllHistoryList.Where(p => p.CreateTime.Date == DateTime.Now.Date).ToList();
            if (todayList.Count > 0)
            {
                int totalSeconds = 0;
                todayList.ForEach(p => totalSeconds += p.TotalSeconds);
                ActualDurationBlock.Text = (totalSeconds / 60.0).ToString("0") + " m";
            }
            else
                ActualDurationBlock.Text = "0 m";
        }
        public void UpdatePresetDuration()
        {
            string presetDuration = App._instance.App.GetLocalSetting(Settings.PresetDuration, "360");
            PresetDurationBlock.Text = presetDuration + " m";
        }
    }
}
