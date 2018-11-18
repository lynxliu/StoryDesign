using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using StoryDesignInterface;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace StoryDesignLib.Diagram
{
    public abstract class DiagramBase : IDiagramBase
    {
        double _Width = 3000;
        double _Height = 2000;
        public double Width { get { return _Width; } set { _Width = value; } }
        public double Height { get { return _Height; } set { _Width = _Height; } }
        //Guid _TargetObjectID = Guid.Empty;
        //public Guid TargetObjectID { get { return _TargetObjectID; } set { _TargetObjectID = value; } }
        public double IconWidth { get; set; }
        public double IconHeight { get; set; }

        public bool HaveNote { get { if (NoteList.Count > 0) return true; return false; } }
        List<INode> _NodeList = new List<INode>();
        public List<INode> NodeList { get { return _NodeList; } }

        List<IConnection> _ConnectionList = new List<IConnection>();
        public List<IConnection> ConnectionList { get { return _ConnectionList; } }

        string _Name = "UnNamed Diagram";
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Memo { get; set; }
        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name ;
                if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;

                return s;
            }
        }
        List<INote> _NoteList = new List<INote>();
        public List<INote> NoteList { get { return _NoteList; } }

        public abstract ICopySupportObject Clone();
        protected virtual void LoadData(IDiagramBase target)
        {
            target.Name = Name;
            target.Memo = Memo;
            //target.Width = Width;
            //target.Height = Height;

            NodeList.ForEach(v => target.NodeList.Add(v.Clone() as INode));
            NoteList.ForEach(v => target.NoteList.Add(v.Clone() as INote));
            ConnectionList.ForEach(v => target.ConnectionList.Add(v.Clone() as IConnection));

        }

        public void LoadTargetObject(IStory story, bool AutoDelete = false)
        {
            NodeList.ForEach(v =>
            {
                var o = story.GetEntityByID(v.TargetObjectID);
                if (o == null)
                    v.TargetObjectID = Guid.Empty;
            });
            if (AutoDelete)
                NodeList.RemoveAll(v => v.TargetObjectID == Guid.Empty);
            ConnectionList.ForEach(v =>
            {
                var o = story.RelationList.FirstOrDefault(r => r.ObjectID == v.TargetObjectID);
                if (o == null)
                    v.TargetObjectID = Guid.Empty;
            });
            if (AutoDelete)
                ConnectionList.RemoveAll(v => v.TargetObjectID == Guid.Empty);
        }

        public void CreateNode(IIdentifiedObject targetObject ,UserControl control)
        {
            var node = new Node() { TargetObjectID = targetObject.ObjectID };
            node.Left = Canvas.GetLeft(control);
            node.Top = Canvas.GetTop(control);
            node.ZIndex = Canvas.GetZIndex(control);
            node.Width = control.ActualWidth;
            node.Height = control.ActualHeight;
            NodeList.Add(node);

        }
        Point GetLeftPoint(INode node)
        {
            return new Point(node.Left, node.Top + node.Height / 2);
        }
        Point GetRightPoint(INode node)
        {
            return new Point(node.Left+node.Width, node.Top + node.Height / 2);
        }
        Point GetTopPoint(INode node)
        {
            return new Point(node.Left+node.Width/2, node.Top );
        }
        Point GetBottomPoint(INode node)
        {
            return new Point(node.Left+node.Width/2, node.Top + node.Height);
        }
        Point GetCenterPoint(INode node)
        {
            return new Point(node.Left + node.Width / 2, node.Top + node.Height / 2);
        }
        public NodeRelativePosition GetRelativePoint(INode start,INode end)
        {
            var sourceCenter = GetCenterPoint(start);
            var targetCenter = GetCenterPoint(end);
            var deltaX = sourceCenter.X - targetCenter.X;
            var deltaY = sourceCenter.Y - targetCenter.Y;
            if (deltaX >= 0)
            {
                if (deltaY >= 0)
                {
                    if (deltaX >= deltaY)
                    {
                        return NodeRelativePosition.LeftTop;
                    }
                    else
                    {
                        return NodeRelativePosition.TopLeft;
                    }
                }
                else
                {
                    if (deltaX >= deltaY)
                    {
                        return NodeRelativePosition.LeftBottom;
                    }
                    else
                    {
                        return NodeRelativePosition.BottomLeft;
                    }
                }
            }
            else
            {
                if (deltaY >= 0)
                {
                    if (deltaX >= deltaY)
                    {
                        return NodeRelativePosition.RightTop;
                    }
                    else
                    {
                        return NodeRelativePosition.TopRight;
                    }
                }
                else
                {
                    if (deltaX >= deltaY)
                    {
                        return NodeRelativePosition.RightBottom;
                    }
                    else
                    {
                        return NodeRelativePosition.BottomRight;
                    }
                }

            }
        }
        public Tuple<Point,Point> GetRelativePoint(NodeRelativePosition position, INode start, INode end)
        {
            if (position == NodeRelativePosition.LeftTop)
                return new Tuple<Point, Point>(GetLeftPoint(start), GetRightPoint(end));
            if (position == NodeRelativePosition.TopLeft)
                return new Tuple<Point, Point>(GetTopPoint(start), GetBottomPoint(end));
            if (position == NodeRelativePosition.TopRight)
                return new Tuple<Point, Point>(GetTopPoint(start), GetBottomPoint(end));
            if (position == NodeRelativePosition.RightTop)
                return new Tuple<Point, Point>(GetRightPoint(start), GetLeftPoint(end));
            if (position == NodeRelativePosition.RightBottom)
                return new Tuple<Point, Point>(GetRightPoint(start), GetLeftPoint(end));
            if (position == NodeRelativePosition.BottomRight)
                return new Tuple<Point, Point>(GetBottomPoint(start), GetTopPoint(end));
            if (position == NodeRelativePosition.BottomLeft)
                return new Tuple<Point, Point>(GetBottomPoint(start), GetTopPoint(end));
            if (position == NodeRelativePosition.LeftBottom)
                return new Tuple<Point, Point>(GetLeftPoint(start), GetRightPoint(end));
            return null;
        }
        public void CreateConnection(INode source, INode target, ConnectionType connectionType = ConnectionType.OneWay)
        {
            var r = new Connection() { StartObjectID = source.TargetObjectID, EndObjectID = target.TargetObjectID ,ConnectionType=connectionType};
            var p = GetRelativePoint(source, target);
            var tp = GetRelativePoint(p, source, target);
            if (tp == null) return;
            r.StartLeft = tp.Item1.X;
            r.StartTop = tp.Item1.Y;
            r.EndLeft = tp.Item2.X;
            r.EndTop = tp.Item2.Y;
            r.Memo = connectionType.ToString();
            ConnectionList.Add(r);
        }

        public void CreateConnection(IIdentifiedObject source, IIdentifiedObject target, ConnectionType connectionType = ConnectionType.OneWay)
        {
            var sObj = NodeList.FirstOrDefault(v => v.TargetObjectID == source.ObjectID);
            var tObj= NodeList.FirstOrDefault(v => v.TargetObjectID == target.ObjectID);
            if (sObj == null || tObj == null)
                return;
            CreateConnection(sObj, tObj, connectionType);
        }

        public void CreateConnection(IRelation target, ConnectionType connectionType = ConnectionType.OneWay)
        {
            var r = ConnectionList.FirstOrDefault(v => v.TargetObjectID == target.ObjectID);
            if (r != null)
                ConnectionList.Remove(r);
            var sObj = NodeList.FirstOrDefault(v => v.TargetObjectID == target.SourceID);
            var tObj = NodeList.FirstOrDefault(v => v.TargetObjectID == target.TargetID);
            if (sObj == null || tObj == null)
                return;
            CreateConnection(sObj, tObj, connectionType);
        }
    }

    public enum NodeRelativePosition
    {
        LeftTop,TopLeft,TopRight,RightTop,RightBottom,BottomRight,BottomLeft,LeftBottom
    }
}
