using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace UISupport
{
    public interface IEditSupportViewModel
    {

    }
    public class UIManager
    {
        Dictionary<IEditSupportViewModel, List<object>> _ViewModelList = new Dictionary<IEditSupportViewModel, List<object>>();
        public Dictionary<IEditSupportViewModel, List<object>> ViewModelList { get { return _ViewModelList; } }

        public static async Task<BitmapImage> LoadImageFromFile(StorageFile file)
        {
            IRandomAccessStream irandom = await file.OpenAsync(FileAccessMode.Read);

            //对图像源使用流源
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DecodePixelWidth = 160;
            bitmapImage.DecodePixelHeight = 100;
            await bitmapImage.SetSourceAsync(irandom);

            return bitmapImage;

        }

        public static void ShowMenu(Func< MenuFlyout> GetContextFlyout,UIElement sender, RightTappedRoutedEventArgs e)
        {
            if (GetContextFlyout == null) return;
            if (GetContextFlyout() != null)
            {
                var contextFlyout = GetContextFlyout();
                contextFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
                e.Handled = true;
            }
        }
    }
}
