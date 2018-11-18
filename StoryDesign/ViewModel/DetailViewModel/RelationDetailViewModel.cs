using StoryDesign.View.Control;
using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;

namespace StoryDesign.ViewModel.DetailViewModel
{
    public class RelationDetailViewModel : EntityViewModelBase<IRelation>, IUIActiveSupport
    {
        //public DateTime BeginDate
        //{
        //    get { if (TargetObject != null) return TargetObject.BeginTime.Date; return DateTime.Now.Date; }
        //    set { if (TargetObject != null) TargetObject.BeginTime = new DateTime(value.Year, value.Month, value.Day); OnPropertyChanged("BeginDate"); }
        //}

        //public DateTime EndDate
        //{
        //    get { if (TargetObject != null) return TargetObject.EndTime.Date; return DateTime.Now.Date; }
        //    set { if (TargetObject != null) TargetObject.EndTime = new DateTime(value.Year, value.Month, value.Day); OnPropertyChanged("EndDate"); }
        //}
        ObservableCollection<EntityShow> _ObjectList = new ObservableCollection<EntityShow>();
        public ObservableCollection<EntityShow> ObjectList { get { return _ObjectList; } }

        public void RefreshObjectList()
        {
            ObjectList.Clear();
            foreach (var v in MainViewModel.mainViewModel.ActorList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                ObjectList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.EventList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                ObjectList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.GroupList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                ObjectList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.LocationList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                ObjectList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.StuffList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                ObjectList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.TaskList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                ObjectList.Add(e);
            }
        }
        public EntityShow Source
        {
            get
            {
                if (SourceID == null|| SourceID==Guid.Empty)
                    return null;
                return ObjectList.FirstOrDefault(v => v.Target.ObjectID == SourceID);
            }
            set
            {
                if (value != null&&value.Target.ObjectID != TargetID)
                    SourceID = value.Target.ObjectID;
                else
                    SourceID = Guid.Empty;
                OnPropertyChanged("SourceID");

                OnPropertyChanged("Source"); OnPropertyChanged("SourceName");
            }
        }
        public EntityShow Target
        {
            get
            {
                if (TargetID == null || TargetID == Guid.Empty)
                    return null;
                return ObjectList.FirstOrDefault(v => v.Target.ObjectID == TargetID);

            }
            set
            {
                if (value != null && value.Target.ObjectID != SourceID)
                    TargetID = value.Target.ObjectID;
                else
                    TargetID = Guid.Empty;

                OnPropertyChanged("TargetID");
                OnPropertyChanged("Target");
                OnPropertyChanged("TargetName");
            }
        }
        public string SourceName
        {
            get
            {
                if (TargetObject == null) return null;
                var s = MainViewModel.mainViewModel.Target.TargetStory.GetEntityByID(SourceID);
                if (s != null)
                    return s.Name;
                return null;
            }
        }
        public string TargetName
        {
            get
            {
                if (TargetObject == null) return null;
                var s = MainViewModel.mainViewModel.Target.TargetStory.GetEntityByID(TargetID);
                if (s != null)
                    return s.Name;
                return null;
            }
        }
        public DateTime BeginTime
        {
            get { if (TargetObject != null) return TargetObject.BeginTime; return DateTime.Now; }
            set
            {
                if (TargetObject != null) TargetObject.BeginTime = value;
                OnPropertyChanged("BeginTime");
            }
        }

        public DateTime EndTime
        {
            get { if (TargetObject != null) return TargetObject.EndTime; return DateTime.Now; }
            set { if (TargetObject != null) TargetObject.EndTime = value; OnPropertyChanged("EndTime"); }
        }

        List<RelationBaseType> _RelationTypeList = new List<RelationBaseType>() { RelationBaseType.Appear, RelationBaseType.Attend,
            RelationBaseType.Friend, RelationBaseType.Kin,RelationBaseType.Mate, RelationBaseType.Office,
            RelationBaseType.Others, RelationBaseType.Own, RelationBaseType.Participate, RelationBaseType.Responsive };
        public List<RelationBaseType> RelationTypeList { get { return _RelationTypeList; } }

        public RelationBaseType RelationType { get { return TargetObject.RelationType; } set { TargetObject.RelationType = value;OnPropertyChanged("RelationType"); } }
        public string Memo { get { return TargetObject.Memo; } set { TargetObject.Memo = value;OnPropertyChanged("Memo"); MainViewModel.RefreshTitle(value, this); } }
        public Guid SourceID { get { return TargetObject.SourceID; } set { TargetObject.SourceID = value;OnPropertyChanged("SourceID"); } }
        public Guid TargetID { get { return TargetObject.TargetID; } set { TargetObject.TargetID = value; OnPropertyChanged("TargetID"); } }
        public Guid ObjectID { get { if (TargetObject != null) return TargetObject.ObjectID; return Guid.Empty; } set { if (TargetObject != null) TargetObject.ObjectID = value; OnPropertyChanged("ObjectID"); } }

        public string RelationShowTitle
        {
            get
            {
                if (string.IsNullOrEmpty(Memo)) return "Relation:"+ RelationType.ToString();
                return Memo + ":" + RelationType.ToString();
            }
        }
        public string RelationDescription
        {
            get
            {
                return "From " + SourceName + " to " + TargetName + "," + RelationType.ToString() +
                    "(" + BeginTime.ToString() + " - " + EndTime.ToString() + "):" + Memo;
            }
        }
        //bool _IsActiveEnable = true;
        //public bool IsActiveEnable { get { return _IsActiveEnable; } set { _IsActiveEnable = value;OnPropertyChanged("IsActiveEnable"); } }

        //List<Action> _SetActive = new List<Action>();
        //public List<Action> SetActive { get { return _SetActive; } }

        //List<Action> _DeSetActive = new List<Action>();
        //public List<Action> DeSetActive { get { return _DeSetActive; } }

        #region NoteCommand
        public bool HaveNote { get { if (NoteList.Count > 0) return true; return false; } }

        ObservableCollection<NoteViewModel> _NoteList = new ObservableCollection<NoteViewModel>();
        public ObservableCollection<NoteViewModel> NoteList { get { return _NoteList; } }
        public NoteViewModel CurrentNote { get; set; }

        public CommonCommand AddNoteCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    CurrentNote = new NoteViewModel() { TargetObject = new Note() };
                    NoteList.Add(CurrentNote);
                    SaveNoteList();
                });
            }
        }
        public CommonCommand RemoveNoteCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (CurrentNote != null)
                    {
                        NoteList.Remove(CurrentNote);
                    }
                    SaveNoteList();
                });
            }
        }
        void SaveNoteList()
        {
            TargetObject.NoteList.Clear();
            foreach (var note in NoteList)
            {
                TargetObject.NoteList.Add(note.TargetObject);
            }
            CurrentNote = NoteList.FirstOrDefault();
        }
        void LoadNoteList()
        {
            if (TargetObject == null) return;
            NoteList.Clear();
            foreach (var note in TargetObject.NoteList)
            {
                NoteList.Add(new NoteViewModel() { TargetObject = note });
            }
            CurrentNote = NoteList.FirstOrDefault();
        }
        #endregion
    }
}
