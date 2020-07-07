using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Share.Models
{
    public class HistoryItem
    {
        public string Name { get; set; }
        public string FolderId { get; set; }
        public string Id { get; set; }
        public int TotalSeconds { get; set; }
        public DateTime CreateTime { get; set; }
        public HistoryItem()
        {

        }
        public HistoryItem(string name, string folderId, int totalSeconds = 0)
        {
            Name = name;
            FolderId = folderId;
            Id = Guid.NewGuid().ToString("N");
            TotalSeconds = totalSeconds;
            CreateTime = DateTime.Now;
        }
        public string GetReadTime()
        {
            var ts = TimeSpan.FromSeconds(TotalSeconds);
            string result = "";
            if (ts.Hours > 0)
                result += ts.Hours + "h";
            if (ts.Minutes > 0)
                result += ts.Minutes + "m";
            else if (ts.Hours > 0)
                result += "00m";
            result += ts.Seconds.ToString("00") + "s";
            return result;
        }
    }
}
