using Lib.Share.Models;
using Newtonsoft.Json;
using Richasy.Controls.UWP.Popups;
using Richasy.Controls.UWP.Widgets;
using Richasy.Font.UWP;
using Richasy.Font.UWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using WorkTimer.Components.Dialog;
using WorkTimer.Components.Layout;
using WorkTimer.Models.Enums;

namespace WorkTimer.Models.Core
{
    public partial class AppViewModel
    {
        public AppViewModel()
        {
            _changeTimer.Tick += ChangeTimer_Tick;
            _durationTimer.Tick += DurationTimer_Tick;
            _changeTimer.Start();
        }

        private void DurationTimer_Tick(object sender, object e)
        {
            if (BeginStamp == DateTime.MinValue)
                return;
            var ts = DateTime.Now - BeginStamp;
            string display = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            DurationText = display;
        }

        public async Task Init()
        {
            var folderList = await App._instance.IO.GetLocalDataAsync<List<FolderItem>>(StaticString.FolderListFileName);
            var historyList = await App._instance.IO.GetLocalDataAsync<List<HistoryItem>>(StaticString.HistoryListFileName);
            FolderCollection.Clear();
            DisplayHistoryCollection.Clear();
            if(!folderList.IsNullOrEmpty())
                folderList.ForEach(p => FolderCollection.Add(p));
            else
            {
                var folderItem = new FolderItem(App._instance.App.GetLocalizationTextFromResource(LanguageName.Default), FeatherSymbol.Activity);
                FolderCollection.Add(folderItem);
                await SaveFolderList();
            }

            string lastSelectedFolderId = App._instance.App.GetLocalSetting(Settings.LastSelectFolderId, "");
            if (!FolderCollection.Any(p=>p.Id==lastSelectedFolderId))
                lastSelectedFolderId = FolderCollection.First().Id;
            
            CurrentSelectedFolder = FolderCollection.Where(p => p.Id == lastSelectedFolderId).First();
            
            if (!historyList.IsNullOrEmpty())
            {
                AllHistoryList = historyList;
                historyList.Where(p => p.FolderId == lastSelectedFolderId).ToList().ForEach(p=>DisplayHistoryCollection.Add(p));
                HistoryChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public async Task SaveFolderList()
        {
            IsFolderListChanged = false;
            List<FolderItem> folderList = new List<FolderItem>();
            folderList = FolderCollection.ToList();
            await App._instance.IO.SetLocalDataAsync(StaticString.FolderListFileName, JsonConvert.SerializeObject(folderList));
        }
        public async Task SaveHistoryList()
        {
            IsHistoryListChanged = false;
            await App._instance.IO.SetLocalDataAsync(StaticString.HistoryListFileName, JsonConvert.SerializeObject(AllHistoryList));
        }
        private async void ChangeTimer_Tick(object sender, object e)
        {
            await SaveData();
        }
        public void AddHistoryItem(HistoryItem item)
        {
            AllHistoryList.Add(item);
            if (item.FolderId == CurrentSelectedFolder.Id)
                DisplayHistoryCollection.Add(item);
            DurationText = "00:00:00";
            BeginStamp = DateTime.MinValue;
            ShowPopup(LanguageName.HasAddedHistoryItem);
            IsHistoryListChanged = true;
        }
        public void AddOrUpdateFolderItem(FolderItem item)
        {
            if (FolderCollection.Contains(item))
            {
                int sourceIndex = FolderCollection.IndexOf(item);
                FolderCollection.Remove(item);
                FolderCollection.Insert(sourceIndex, item);
                if (CurrentSelectedFolder != null && CurrentSelectedFolder.Equals(item))
                    CurrentSelectedFolderChanged?.Invoke(this, item);
                IsFolderListChanged = true;
            }
            else
            {
                FolderCollection.Add(item);
                IsFolderListChanged = true;
            }
        }
        public async Task RemoveFolder(FolderItem item)
        {
            if (FolderCollection.Count == 1)
            {
                ShowPopup(LanguageName.NeedOneFolder, true);
                return;
            }
            var confirmDialog = new ConfirmDialog(LanguageName.ConfirmRemoveFolder);
            confirmDialog.PrimaryButtonClick += (_s, _e) =>
            {
                FolderCollection.Remove(item);
                AllHistoryList.RemoveAll(p => p.FolderId == item.Id);
                if (CurrentSelectedFolder.Equals(item))
                {
                    var first = FolderCollection.First();
                    CurrentSelectedFolder = first;
                }
                IsFolderListChanged = true;
                IsHistoryListChanged = true;
            };
            await confirmDialog.ShowAsync();
        }
        public async Task RemoveHistory(HistoryItem item)
        {
            var confirmDialog = new ConfirmDialog(LanguageName.ConfirmRemoveHistory);
            confirmDialog.PrimaryButtonClick += (_s, _e) =>
            {
                AllHistoryList.Remove(item);
                DisplayHistoryCollection.Remove(item);
                IsHistoryListChanged = true;
            };
            await confirmDialog.ShowAsync();
        }
        public void MoveHistory(HistoryItem source,string newFolderId)
        {
            foreach (var item in AllHistoryList)
            {
                if (item.Id == source.Id)
                {
                    item.FolderId = newFolderId;
                    break;
                }
            }
            DisplayHistoryCollection.Remove(source);
            IsHistoryListChanged = true;
        }
        public async Task SaveData()
        {
            if (IsFolderListChanged)
                await SaveFolderList();
            if (IsHistoryListChanged)
                await SaveHistoryList();
        }
        public void ShowPopup(LanguageName name,bool isError = false)
        {
            ShowPopup(App._instance.App.GetLocalizationTextFromResource(name), isError);
        }
        public void ShowPopup(string msg, bool isError = false)
        {
            var popup = new TipPopup(App._instance, msg);
            ColorName color = isError ? ColorName.ErrorColor : ColorName.PrimaryColor;
            popup.Show(color);
        }

        public void ShowSettingPopup()
        {
            if (SettingPopup == null)
            {
                var header = new CenterPopupHeader();
                header.Padding = new Thickness(20, 10, 20, 10);
                header.Title = App._instance.App.GetLocalizationTextFromResource(LanguageName.Settings);
                header.CloseButtonStyle = App._instance.App.GetStyleFromResource(StyleName.PopupHeaderButtonStyle);
                header.TitleTextStyle = App._instance.App.GetStyleFromResource(StyleName.SubtitleTextStyle);
                header.CloseIcon = new FeatherIcon(FeatherSymbol.X) { FontSize = 13 };
                var settingPanel = new SettingPanel();
                SettingPopup = CenterPopup.CreatePopup(App._instance, header, settingPanel);
                header.CloseButtonClick += (_s, _e) =>
                {
                    SettingPopup.Hide();
                };
                SettingPopup.Style = App._instance.App.GetStyleFromResource(StyleName.BasicCenterPopupStyle);
            }
            SettingPopup.Show();
        }

        public async void CheckUpdate()
        {
            string localVersion = App._instance.App.GetLocalSetting(Settings.AppVersion, "");
            if (localVersion != VersionBlock.Version)
            {
                var main = new VersionBlock();
                main.Title = App._instance.App.GetLocalizationTextFromResource(LanguageName.UpdateTitle);
                string lan = App._instance.App.GetLocalSetting(Settings.Language, "zh_CN");
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Others/Update_{lan}.txt"));
                string content = await FileIO.ReadTextAsync(file);
                main.Description = content;
                main.LogoUri = "ms-appx:///Assets/AppLogo.png";
                main.ActionButtonStyle = App._instance.App.GetStyleFromResource(StyleName.PrimaryActionButtonStyle);
                main.ActionIcon = new FeatherIcon(FeatherSymbol.X);
                main.TitleTextStyle = App._instance.App.GetStyleFromResource(StyleName.SubtitleTextStyle);
                main.DescriptionTextStyle = App._instance.App.GetStyleFromResource(StyleName.BasicMarkdownTextBlock);
                var popup = new CenterPopup(App._instance);
                popup.Style = App._instance.App.GetStyleFromResource(StyleName.BasicCenterPopupStyle);
                popup.Main = main;
                main.ActionButtonClick += (_s, _e) =>
                {
                    popup.Hide();
                    App._instance.App.WriteLocalSetting(Settings.AppVersion, VersionBlock.Version);
                };
                popup.Show();
            }
        }
    }
}
