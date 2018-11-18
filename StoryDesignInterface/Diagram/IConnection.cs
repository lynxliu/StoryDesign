using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace StoryDesignInterface.Diagram
{
    public interface IConnection:ICopySupportObject
    {
        Guid SourceDesignItemID { get; set; }
        Guid TargetDesignItemID { get; set; }
        Guid DesignObjectID { get; set; }
        Guid TargetObjectID { get; set; }
        Guid StartObjectID { get; set; }
        Guid EndObjectID { get; set; }
        ConnectionType ConnectionType { get; set; }
        double StartLeft { get; set; }
        double StartTop { get; set; }
        double EndLeft { get; set; }
        double EndTop { get; set; }
        string Memo { get; set; }
        bool? IsValid(IStory story, DateTime time);

        LinkInfo LinkLineInfo { get; set; }
    }

    public enum ConnectionType
    {
        OneWay, TwoWay
    }

    public class LinkInfo
    {
        double _RadiusRadio = 0;
        public double RadiusRadio
        {
            get { return _RadiusRadio; }
            set { _RadiusRadio = value;  }
        }

        SweepDirection _LineSweepDirection = SweepDirection.Clockwise;
        public SweepDirection LineSweepDirection
        {
            get { return _LineSweepDirection; }
            set { _LineSweepDirection = value;  }

        }

        public RelativePosition SourcePosition { get; set; }
        public RelativePosition TargetPosition { get; set; }
    }
}
