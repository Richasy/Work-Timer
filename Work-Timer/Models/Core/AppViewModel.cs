using Lib.Share.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Richasy.Controls.UWP.Popups;
using Richasy.Font.UWP.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WorkTimer.Models.Enums;

namespace WorkTimer.Models.Core
{
    public partial class AppViewModel
    {
        public AppViewModel()
        {
            _changeTimer.Tick += ChangeTimer_Tick;
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
            }
        }
        public async Task SaveFolderList()
        {
            _isFolderListChanged = false;
            List<FolderItem> folderList = new List<FolderItem>();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                folderList = FolderCollection.ToList();
            });
            await App._instance.IO.SetLocalDataAsync(StaticString.FolderListFileName, JsonConvert.SerializeObject(folderList));
        }
        private async void ChangeTimer_Tick(object sender, object e)
        {
            var tasks = new List<Task>();
            if (_isFolderListChanged)
                tasks.Add(Task.Run(async () => { await SaveFolderList(); }));
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks.ToArray());
            }
        }
        public void AddHistoryItem(HistoryItem item)
        {
            AllHistoryList.Add(item);
            if (item.FolderId == CurrentSelectedFolder.Id)
                DisplayHistoryCollection.Add(item);
            DurationText = "00:00:00";
            BeginStamp = DateTime.MinValue;
            ShowPopup(LanguageName.HasAddedHistoryItem);
            _isHistoryListChanged = true;
        }
        public void AddOrUpdateFolderItem(FolderItem item)
        {
            if (FolderCollection.Contains(item))
            {
                int sourceIndex = FolderCollection.IndexOf(item);
                FolderCollection.Remove(item);
                FolderCollection.Insert(sourceIndex, item);
                _isFolderListChanged = true;
            }
            else
            {
                FolderCollection.Add(item);
                _isFolderListChanged = true;
            }
        }
        public void ShowPopup(LanguageName name,bool isError = false)
        {
            ShowPopup(App._instance.App.GetLocalizationTextFromResource(name), isError);
        }
        public void ShowPopup(string msg, bool isError = false)
        {
            var popup = new TipPopup(App._instance, msg);
            ColorName color = isError ? ColorName.PrimaryColor : ColorName.ErrorColor;
            popup.Show(color);
        }
    }
}
