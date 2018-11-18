using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace StoryDesignInterface
{
    public class Resource
    {
        //public string Name { get; set; }
        public string Value { get; set; }
        public ResourceType ResourceType { get; set; }

        public Resource Clone()
        {
            return new Resource() {  ResourceType = ResourceType,Value=Value };
        }

        public BitmapImage Icon
        {

        }
    }

    public enum ResourceType
    {
        Text,Image,Video,Url, BasicGeoposition,File
    }
}
