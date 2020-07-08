using Richasy.Font.UWP.Enums;
using System;
using System.Collections.Generic;

namespace Lib.Share.Models
{
    public class FolderItem
    {
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Id { get; set; }
        public FeatherSymbol Icon { get; set; }
        public FolderItem()
        {

        }
        public FolderItem(string name,FeatherSymbol icon)
        {
            Name = name;
            CreateTime = DateTime.Now;
            Id = Guid.NewGuid().ToString("N");
            Icon = icon;
        }

        public override bool Equals(object obj)
        {
            return obj is FolderItem item &&
                   Id == item.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
    }
}
