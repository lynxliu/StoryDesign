using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;

namespace StoryDesign.ViewModel
{
    public class MoodViewModel:ViewModelBase
    {
        public MoodViewModel(IMood mood)
        {
            if (mood == null) throw new Exception("No valid mood");
            Target = mood;
        }

        public IMood Target
        {
            get;set;
        }

        public double Rank
        {
            get { return Target.Rank; }
            set { Target.Rank = value;OnPropertyChanged("Rank"); OnPropertyChanged("MoodInfo"); }
        }

        public MoodBaseType MoodType
        {
            get
            {
                return Target.MoodType;
            }
            set
            {
                Target.MoodType = value;OnPropertyChanged("MoodType"); OnPropertyChanged("MoodInfo");
            }
        }

        //public string Description
        //{
        //    get { return Target.Description; }
        //    set { Target.Description = value;OnPropertyChanged("Description"); }
        //}

        public string MoodInfo
        {
            get { return MoodType.ToString() + "(" + Rank.ToString("p") + ")"; }
        }

        ObservableCollection<MoodBaseType> _MoodList;
        public ObservableCollection<MoodBaseType> MoodList
        {
            get
            {
                if (_MoodList == null)
                {
                    _MoodList = new ObservableCollection<MoodBaseType>();
                    var l = Mood.GetMoodList();
                    l.ForEach(v => _MoodList.Add(v));
                }
                return _MoodList;
            }
        }
    }
}
