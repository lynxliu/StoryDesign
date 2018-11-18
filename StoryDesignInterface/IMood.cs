using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace StoryDesignInterface
{
    public interface IMood:ICopySupportObject
    {
        MoodBaseType MoodType { get; set; }
        //string Description { get; set; }
        double Rank { get; set; }
        string MoodInfo { get; }
    }

    public enum MoodBaseType
    {
        Joy,Anger, Sadness,Disgust,Fear,Surprise,Panic,Curious,Neutral,Exciting
    }


}
