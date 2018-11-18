using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public interface IStatusSupportObject
    {
        string GetStatus();
        void SetStatus(string status);
    }
    public interface ISerialSupport : IParameterSupportObject, ICopySupportObject, IDataObject
    {
        string Key { get; }
        string GetSerialParameter();
        void DeserialParameter(string parameterSerialString);

    }

    public class SerialInfo : ICopySupportObject
    {
        public string Key { get; set; }
        public string ParameterInfo { get; set; }

        public ISerialSupport CreateInstance(List<ISerialSupport> validObjectList)
        {
            if (string.IsNullOrEmpty(Key) || validObjectList == null) return null;
            var o = validObjectList.FirstOrDefault(v => v.Key == Key);
            if (o == null) return null;
            var obj = o.Clone() as ISerialSupport;
            obj.DeserialParameter(ParameterInfo);
            return obj;
        }

        public static SerialInfo GetSerialInfo(ISerialSupport obj)
        {
            if (obj == null) return null;
            return new SerialInfo() { Key = obj.Key, ParameterInfo = obj.GetSerialParameter() };
        }

        public ICopySupportObject Clone()
        {
            return new SerialInfo() { Key = Key, ParameterInfo = ParameterInfo };
        }
    }
}
