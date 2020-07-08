using Lib.Share.Models;
using Richasy.Controls.UWP.Popups;
using Richasy.Font.UWP;
using Richasy.Font.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using WorkTimer.Components.Layout;
using WorkTimer.Models.Enums;

namespace WorkTimer.Models.Core
{
    public partial class AppViewModel : DependencyObject
    {
        public ObservableCollection<FolderItem> FolderCollection = new ObservableCollection<FolderItem>();
        public ObservableCollection<HistoryItem> DisplayHistoryCollection = new ObservableCollection<HistoryItem>();
        public List<HistoryItem> AllHistoryList = new List<HistoryItem>();
        private bool _isFolderListChanged = false;
        private bool _isHistoryListChanged = false;
        private DispatcherTimer _changeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(10) };
        private DispatcherTimer _durationTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.5) };

        public event EventHandler<FolderItem> CurrentSelectedFolderChanged;
        private FolderItem _currentSelectedFolder = null;

        public FolderPanel FolderPanel;
        public HistoryPanel HistoryPanel;

        public CenterPopup SettingPopup;
        public FolderItem CurrentSelectedFolder
        {
            get => _currentSelectedFolder;
            set
            {
                _currentSelectedFolder = value;
                var history = AllHistoryList.Where(p => p.FolderId == value.Id).ToList();
                DisplayHistoryCollection.Clear();
                history.ForEach(p => DisplayHistoryCollection.Add(p));
                App._instance.App.WriteLocalSetting(Settings.LastSelectFolderId, value.Id);
                CurrentSelectedFolderChanged?.Invoke(this, value);
            }
        }
        public DateTime BeginStamp { get; set; }

        public event EventHandler<bool> IsTimingChanged;
        private bool _isTiming = false;
        public bool IsTiming
        {
            get => _isTiming;
            set
            {
                _isTiming = value;
                IsTimingChanged?.Invoke(this, value);
                if (value)
                    _durationTimer.Start();
                else
                    _durationTimer.Stop();
            }
        }

        public string DurationText
        {
            get { return (string)GetValue(DurationTextProperty); }
            set { SetValue(DurationTextProperty, value); }
        }

        public static readonly DependencyProperty DurationTextProperty =
            DependencyProperty.Register("DurationText", typeof(string), typeof(AppViewModel), new PropertyMetadata("00:00:00"));

             
        public ObservableCollection<FeatherSymbol> SymbolCollection = new ObservableCollection<FeatherSymbol>
        {
            FeatherSymbol.Activity,FeatherSymbol.Airplay,
            FeatherSymbol.AlertTriangle,FeatherSymbol.Anchor,
            FeatherSymbol.Aperture,FeatherSymbol.Archive,
            FeatherSymbol.Award,FeatherSymbol.BarChart,
            FeatherSymbol.BarChart2,FeatherSymbol.Battery,
            FeatherSymbol.Bluetooth,FeatherSymbol.Book,
            FeatherSymbol.Bookmark,FeatherSymbol.Box,
            FeatherSymbol.Briefcase,FeatherSymbol.Calendar,
            FeatherSymbol.Camera,FeatherSymbol.Cast,
            FeatherSymbol.Chrome,FeatherSymbol.Clipboard,
            FeatherSymbol.Clock,FeatherSymbol.Cloud,
            FeatherSymbol.Code,FeatherSymbol.Coffee,
            FeatherSymbol.Cpu,FeatherSymbol.CreditCard,
            FeatherSymbol.Disc,FeatherSymbol.Edit,
            FeatherSymbol.Eye,FeatherSymbol.Facebook,
            FeatherSymbol.Feather,FeatherSymbol.Figma,
            FeatherSymbol.File,FeatherSymbol.Folder,
            FeatherSymbol.Framer,FeatherSymbol.Gift,
            FeatherSymbol.Image,FeatherSymbol.Home,
            FeatherSymbol.Inbox,FeatherSymbol.Info
        };
    }
}
