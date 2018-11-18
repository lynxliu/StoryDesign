using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;

namespace StoryDesign.ViewModel
{
    public class NoteViewModel: EntityViewModelBase<INote>
    {
        bool _IsEditMode = false;
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set { _IsEditMode = true; OnPropertyChanged("IsEditMode"); OnPropertyChanged("IsReadOnly"); }
        }
        public bool IsReadOnly
        {
            get { return !_IsEditMode; }

        }
        public CommonCommand SwitchEditModeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    IsEditMode = !IsEditMode;
                });
            }
        }
        public DateTime CreateTime { get { return TargetObject.CreateTime; } }
        public DateTime LastModifyTime { get { return TargetObject.LastModified; } set { TargetObject.LastModified = value;OnPropertyChanged("LastModifiedTime"); } }
        public string Description { get { return TargetObject.Description; } set { TargetObject.Description = value;OnPropertyChanged("Description"); } }
    }
}
