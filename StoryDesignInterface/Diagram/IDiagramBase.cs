using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StoryDesignInterface.Diagram
{
    public interface IDiagramBase:IDataObject, ICopySupportObject,INoteObject
    {
        double Width { get; set; }
        double Height { get; set; }
        //Guid TargetObjectID { get; set; }
        List<INode> NodeList { get; }
        List<IConnection> ConnectionList { get; }
        void LoadTargetObject(IStory story, bool AutoDelete = false);
        void CreateNode(IIdentifiedObject targetObject, UserControl control);
        void CreateConnection(INode source, INode target,ConnectionType connectionType= ConnectionType.OneWay);
        void CreateConnection(IIdentifiedObject source, IIdentifiedObject target, ConnectionType connectionType = ConnectionType.OneWay);
        void CreateConnection(IRelation target, ConnectionType connectionType = ConnectionType.OneWay);

    }


}
