using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Share.Models
{
    public class FolderItem
    {
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Id { get; set; }
        public Enum Icon { get; set; }
        public FolderItem()
        {

        }
        public FolderItem(string name,Enum icon)
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
