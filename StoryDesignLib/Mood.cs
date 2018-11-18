using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Windows.UI;

namespace StoryDesignLib
{
    public class Mood : IMood
    {
        public string MoodInfo
        {
            get
            {
                return MoodType.ToString() + "(" + Rank.ToString("p") + ")";
            }
        }
        public string Description { get; set; }
        public double Rank { get; set; }
        public MoodBaseType MoodType { get; set; }

        public ICopySupportObject Clone()
        {
            var o = new Mood() { Description=Description,Rank=Rank};
            return o;
        }

        public static List<MoodBaseType> GetMoodList()
        {
            return new List<MoodBaseType>() { MoodBaseType.Anger, MoodBaseType.Curious, MoodBaseType.Disgust, MoodBaseType.Exciting, MoodBaseType.Fear, MoodBaseType.Joy, MoodBaseType.Neutral, MoodBaseType.Panic, MoodBaseType.Sadness, MoodBaseType.Surprise };
        }
        public static Color GetColor(IMood mood)
        {
            if (mood.MoodType == MoodBaseType.Neutral) return Colors.White;
            if (mood.MoodType == MoodBaseType.Joy) return Colors.Yellow;
            if (mood.MoodType == MoodBaseType.Sadness) return Colors.Blue;
            if (mood.MoodType == MoodBaseType.Disgust) return Colors.Green;
            if (mood.MoodType == MoodBaseType.Anger) return Colors.Red;
            if (mood.MoodType == MoodBaseType.Fear) return Colors.DeepPink;
            if (mood.MoodType == MoodBaseType.Curious) return Colors.LightSkyBlue;
            if (mood.MoodType == MoodBaseType.Panic) return Colors.SandyBrown;
            if (mood.MoodType == MoodBaseType.Surprise) return Colors.Orange;
            if (mood.MoodType == MoodBaseType.Exciting) return Colors.Pink;
            return Colors.White;
        }
    }
}
