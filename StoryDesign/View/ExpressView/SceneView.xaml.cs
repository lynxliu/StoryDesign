using DesignTool.Lib;
using StoryDesign.ViewModel.ExpressViewModel;
using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace StoryDesign.View.ExpressView
{
    public sealed partial class SceneView : UserControl
    {
        public SceneView()
        {
            this.InitializeComponent();
        }

        private void ListBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            object data;
            if (e.DataView.Properties.TryGetValue("entity", out data))
            {
                ProcessEntity(data);
            }

        }
        void ProcessEntity(object data)
        {
            if (data == null) return;
            if(data is IStoryEntityObject)
            {
                var dc = DataContext as SceneViewModel;
                if (dc == null) return;
                dc.AddEntityAsExpressObject(data as IStoryEntityObject);
            }
            
        }
        //private void ComboBox_DropDownOpened(object sender, object e)
        //{
        //    var dc = DataContext as SceneViewModel;
        //    if (dc == null) return;
        //    dc.RefreshEntityList();
        //}
    }
}
