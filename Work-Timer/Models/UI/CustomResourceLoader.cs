using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Resources;
using WorkTimer.Models.Enums;

namespace WorkTimer.Models.UI
{
    public class CustomResourceLoader : CustomXamlResourceLoader
    {
        protected override object GetResource(string resourceId, string objectType, string propertyName, string propertyType)
        {
            if (resourceId == "Basic")
            {
                return new FontFamily(App._instance.App.GetLocalSetting(Settings.FontFamily, "Microsoft YaHei UI"));
            }
            return null;
        }
    }
}
