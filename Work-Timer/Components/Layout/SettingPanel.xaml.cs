using Lib.Share.Models;
using Richasy.Font.UWP;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WorkTimer.Components.Dialog;
using WorkTimer.Models.Enums;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace WorkTimer.Components.Layout
{
    public sealed partial class SettingPanel : UserControl
    {
        private ObservableCollection<SystemFont> FontCollection = new ObservableCollection<SystemFont>();
        private bool _isInit = false;
        public SettingPanel()
        {
            this.InitializeComponent();
            string lan = ApplicationLanguages.PrimaryLanguageOverride.Equals("zh-CN", StringComparison.OrdinalIgnoreCase) ? "zh-Hans-CN" : "en-US";
            var fonts = SystemFont.GetSystemFonts(lan);
            fonts.ForEach(p => FontCollection.Add(p));
        }

        private async void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInit)
                return;
            var item = (ThemeComboBox.SelectedItem as ComboBoxItem).Tag.ToString();
            string oldTheme = App._instance.App.GetLocalSetting(Settings.Theme, StaticString.ThemeSystem);
            if (oldTheme != item)
            {
                App._instance.App.WriteLocalSetting(Settings.Theme, item);
                await ShowRestartDialog();
            }
        }

        private async void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInit)
                return;
            var item = FontComboBox.SelectedItem as SystemFont;
            string oldFont = App._instance.App.GetLocalSetting(Settings.FontFamily, StaticString.FontDefault);
            if (item.Name != oldFont)
            {
                App._instance.App.WriteLocalSetting(Settings.FontFamily, item.Name);
                await ShowRestartDialog();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string theme = App._instance.App.GetLocalSetting(Settings.Theme, StaticString.ThemeSystem);
            switch (theme)
            {
                case StaticString.ThemeSystem:
                    ThemeComboBox.SelectedIndex = 0;
                    break;
                case StaticString.ThemeLight:
                    ThemeComboBox.SelectedIndex = 1;
                    break;
                case StaticString.ThemeDark:
                    ThemeComboBox.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
            FontInit();
            string lan = App._instance.App.GetLocalSetting(Settings.Language, StaticString.LanZh);
            LanguageComboBox.SelectedIndex = lan == StaticString.LanZh ? 0 : 1;
            _isInit = true;
        }
        private async Task ShowRestartDialog()
        {
            var dialog = new ConfirmDialog(LanguageName.StaticResourceRestart) { PrimaryButtonText = App._instance.App.GetLocalizationTextFromResource(LanguageName.RestartNow) };
            dialog.PrimaryButtonClick += async (_s, _e) =>
            {
                await CoreApplication.RequestRestartAsync("restart");
            };
            await dialog.ShowAsync();
        }
        private void FontInit()
        {
            FontComboBox.IsEnabled = false;
            if (FontCollection != null && FontCollection.Count > 0)
            {
                string fontName = App._instance.App.GetLocalSetting(Settings.FontFamily, StaticString.FontDefault);
                var font = FontCollection.Where(p => p.Name == fontName).FirstOrDefault();
                if (font != null)
                {
                    FontComboBox.SelectedItem = font;
                }
            }
            FontComboBox.IsEnabled = true;
        }

        private async void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInit)
                return;
            var item = (LanguageComboBox.SelectedItem as ComboBoxItem).Tag.ToString();
            string oldLan = App._instance.App.GetLocalSetting(Settings.Language, StaticString.LanZh);
            if (oldLan != item)
            {
                App._instance.App.WriteLocalSetting(Settings.Language, item);
                await ShowRestartDialog();
            }
        }
    }
}
