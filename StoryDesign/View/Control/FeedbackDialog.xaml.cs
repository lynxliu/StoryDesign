using CommonLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UISupport;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoryDesign.View.Control
{
    public sealed partial class FeedbackDialog : ContentDialog
    {
        public FeedbackDialog()
        {
            this.InitializeComponent();
            DataContext = new FeedbackViewModel();
        }

        public string GetMessage()
        {
            var dc = DataContext as FeedbackViewModel;
            if (dc == null) return null;
            var s = messageTextBox.Text;
            if (dc.IsAttachLog)
            {
                s += "\n\n\t Error: \n";

                LogSupport.LogList.ForEach(v =>
                {
                    s += "\t" + v.LogType + "\t" + v.LogTime.ToString() + "\t" + v.Message + "\n\n";
                });
            }
            return s;
        }
    }

    public class FeedbackViewModel
    {

        bool _IsAttachLog = false;
        public bool IsAttachLog
        {
            get { return _IsAttachLog; }
            set { _IsAttachLog = value; }
        }
    }
}
