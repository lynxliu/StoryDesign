using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using CommonLib;
using DesignTool.Lib.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using UISupport;

namespace DesignTool.Lib.Model
{
    public class ToolboxItem:EntityBase
    {

        public Func<FrameworkElement> CreateTargetControl { get; set; }

        public ICommand ClickCommand
        {
            get
            {
                return new CommonCommand((o) =>
                    {
                        if (CreateTargetControl != null && CurrentCanvas != null)
                            CurrentCanvas.SetCreate(CreateTargetControl);
                    });
            }
        } 

        public static DesignCanvas CurrentCanvas { get; set; }
    }
}
