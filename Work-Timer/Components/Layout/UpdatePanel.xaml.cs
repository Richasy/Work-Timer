using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WorkTimer.Models.Enums;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace WorkTimer.Components.Layout
{
    public sealed partial class UpdatePanel : UserControl
    {
        public UpdatePanel()
        {
            this.InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string lan = App._instance.App.GetLocalSetting(Settings.Language, "zh_CN");
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Others/Update_{lan}.txt"));
            string content = await FileIO.ReadTextAsync(file);
            VersionBlock.Description = content;
        }
    }
}
