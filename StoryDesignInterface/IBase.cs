using CommonLib;
using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StoryDesignInterface
{
    public interface IStoryEntityObject: IDataObject, ITimeSensitive,ISerialSupport,INoteObject, ISubject
    {
        double Grade { get; set; }
        DateTime CurrentTime { get; set; }
        List<IMeasure> MeasureList { get; }

        string Icon { get; set; }
    }

    public interface ISubject:IIdentifiedObject, IDataObject
    {
        List<FateEntity> GetFate(IStory story);
        void RefreshFateDiagram();
        IFateDiagram TargetFate { get; set; }
    }

    public interface ITimeSensitive
    {
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        TimeSpan ContinueTime { get; }
    }

    public interface EditObject
    {
        UserControl GetControl();
    }
    public enum FilterType
    {
        NoFilter,EntityName,EntityType,RelationType
    }
}
