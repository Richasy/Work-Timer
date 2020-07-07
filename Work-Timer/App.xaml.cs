using Richasy.Helper.UWP;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Resources;
using WorkTimer.Models.Core;
using WorkTimer.Models.Enums;
using WorkTimer.Models.UI;

namespace WorkTimer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public static Instance _instance = new Instance("WorkTimer");
        public static AppViewModel _vm = new AppViewModel();
        public App()
        {
            this.InitializeComponent();
            ChangeLanguage();
            this.Suspending += OnSuspending;
            CustomXamlResourceLoader.Current = new CustomResourceLoader();
            string theme = _instance.App.GetLocalSetting(Settings.Theme, Current.RequestedTheme.ToString());
            RequestedTheme = theme == "Light" ? ApplicationTheme.Light : ApplicationTheme.Dark;
            UnhandledException += UnhandleExceptionHandle;
        }

        private void UnhandleExceptionHandle(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            _vm.ShowPopup(e.Message, true);
        }
        /// <summary>
        /// 更改语言首选项
        /// </summary>
        private void ChangeLanguage()
        {
            string lan = _instance.App.GetLocalSetting(Settings.Language, "");

            if (lan == "")
            {
                var Languages = Windows.System.UserProfile.GlobalizationPreferences.Languages;
                if (Languages.Count > 0)
                {
                    var language = Languages[0];
                    if (language.ToLower().IndexOf("zh") != -1)
                    {
                        _instance.App.WriteLocalSetting(Settings.Language, "zh_CN");
                    }
                    else
                    {
                        _instance.App.WriteLocalSetting(Settings.Language, "en_US");
                    }
                }
                else
                {
                    _instance.App.WriteLocalSetting(Settings.Language, "zh_CN");
                }
            }
            lan = _instance.App.GetLocalSetting(Settings.Language, "zh_CN");
            string code = "";
            switch (lan)
            {
                case "zh_CN":
                    code = "zh-CN";
                    break;
                case "en_US":
                    code = "en-US";
                    break;
                default:
                    code = "en-US";
                    break;
            }
            ApplicationLanguages.PrimaryLanguageOverride = code;
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            _instance = new Instance("WorkTimer");
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
            _instance.App.SetTitleBarColor();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
