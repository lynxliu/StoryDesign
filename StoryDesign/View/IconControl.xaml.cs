using StoryDesign.ViewModel.DetailViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace StoryDesign.View
{
    public sealed partial class IconControl : UserControl
    {
        public IconControl()
        {
            this.InitializeComponent();
        }
        //MenuFlyout contextFlyout;
        //public void InitContextMemu()
        //{
        //    contextFlyout = new MenuFlyout();

        //    MenuFlyoutItem modifyItem = new MenuFlyoutItem
        //    {
        //        Text = "Set Icon",
        //    };
        //    modifyItem.Click +=async (s, e) =>
        //    {
        //        var data = DataContext ;
        //        var id = CommonLib.CommonProc.GetPropertyValue(data, new List<string>() { "ObjectID" });
        //        var icon = data.GetType().GetProperty("Icon");
        //        Windows.Storage.Pickers.FileOpenPicker picker = new Windows.Storage.Pickers.FileOpenPicker();
        //        //显示方式
        //        picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        //        //选择最先的位置
        //        picker.SuggestedStartLocation =
        //            Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        //        //后缀名
        //        picker.FileTypeFilter.Add(".jpg");
        //        picker.FileTypeFilter.Add(".png");
        //        picker.FileTypeFilter.Add(".bmp");
        //        picker.FileTypeFilter.Add(".psd");
        //        picker.FileTypeFilter.Add(".tif");
        //        StorageFile file = await picker.PickSingleFileAsync();
        //        if (file != null)
        //        {
        //            var fn = file.Name;
        //            var li = fn.LastIndexOf('.');
        //            var en = fn.Substring(li);
        //            var n =  id.ToString() + en;
        //            StorageFolder folder =await MainPageViewModel.mainViewModel.GetIconFolder();
        //            if (folder != null)
        //            {
        //                await file.CopyAsync(folder,n, NameCollisionOption.ReplaceExisting);
        //                //await file.RenameAsync(n, NameCollisionOption.ReplaceExisting);
        //                //var iconFolder = await MainPageViewModel.mainViewModel.GetProjectIconFolder();

        //                //await file.CopyAsync(iconFolder, file.Name, NameCollisionOption.ReplaceExisting);
        //                // var path = "ms-appx:///Assets/ProjectIcon/" + n;
        //                //icon.SetValue(data, new Uri(path));
        //                var m=DataContext.GetType().GetTypeInfo().GetDeclaredMethod("CheckIcon");
        //                m.Invoke(DataContext,new object[0]);
        //            }
        //        }
        //    };
        //    contextFlyout.Items.Add(modifyItem);

            

        //}

        //private void Image_RightTapped(object sender, RightTappedRoutedEventArgs e)
        //{
        //    if (contextFlyout == null)
        //        InitContextMemu();
        //    contextFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        //    //e.Handled = true;
        //}

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            var data = DataContext;
            var id = CommonLib.CommonProc.GetPropertyValue(data, new List<string>() { "ObjectID" });

            Windows.Storage.Pickers.FileOpenPicker picker = new Windows.Storage.Pickers.FileOpenPicker();
            //显示方式
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            //选择最先的位置
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            //后缀名
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".ico");
            picker.FileTypeFilter.Add(".tif");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var fn = file.Name;
                var li = fn.LastIndexOf('.');
                var en = fn.Substring(li);
                var n = id.ToString() + en;
                StorageFolder folder = await MainViewModel.mainViewModel.GetIconFolder();
                if (folder != null)
                {
                    await file.CopyAsync(folder, n, NameCollisionOption.ReplaceExisting);
                    var m = data as IconSupport;
                    m.UpdateIcon();
                }
            }
        }
    }
}
