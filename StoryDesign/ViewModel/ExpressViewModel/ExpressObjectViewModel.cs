using StoryDesignInterface.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace StoryDesign.ViewModel.ExpressViewModel
{
    public class ExpressObjectViewModel : EntityViewModelBase<IExpressObject>
    {
        public BitmapImage IconImage
        {
            get
            {
                if (TargetObjectID != Guid.Empty)
                    return MainViewModel.mainViewModel.GetEntityIconByID(TargetObjectID);
                return null;
            }
        }

        public Guid TargetObjectID { get { return TargetObject.TargetObjectID; } set { TargetObject.TargetObjectID = value;OnPropertyChanged("TargetObjectID"); } }
        //public TimeSpan ExpressStartTime { get { return TargetObject.ExpressStartTime; } set { TargetObject.ExpressStartTime = value;OnPropertyChanged("ExpressStartTime"); } }
        //public TimeSpan ExpressEndTime { get { return TargetObject.ExpressEndTime; } set { TargetObject.ExpressEndTime = value; OnPropertyChanged("ExpressEndTime"); } }

        public string Name { get { return TargetObject.Name; } }
        public string Memo { get { return TargetObject.Memo; } }
        public string Description
        {
            get { return TargetObject.Description; }
            set
            {
                TargetObject.Description = value; OnPropertyChanged("Description");
            }
        }
        //public string ExpressType { get {return TargetObject.ExpressType; }  }
        //public string Title
        //{
        //    get
        //    {
        //        return Name + "(" + Description + ")";
        //    }
        //}
    }
}
