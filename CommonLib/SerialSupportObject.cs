﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CommonLib
{
    public abstract class SerialSupportObject : ISerialSupport
    {
        protected string _Key;
        [ParameterOperation]
        public virtual string Key
        {
            get
            {
                if (string.IsNullOrEmpty(_Key))
                {
                    var al =this.GetType().GetTypeInfo().GetCustomAttributes(typeof(SerialObjectAttribute), true);
                    if (al != null && al.Count() > 0)
                    {
                        var a = al.FirstOrDefault() as SerialObjectAttribute;
                        if (a != null && !string.IsNullOrEmpty(a.Key))
                            _Key = a.Key;

                        else
                            _Key = GetType().FullName;
                    }
                }
                return _Key;
            }
        }

        protected string _Name;
        [ParameterOperation]
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_Name))
                {
                    var al = GetType().GetTypeInfo().GetCustomAttributes(typeof(SerialObjectAttribute), true);
                    if (al != null && al.Count() > 0)
                    {
                        var a = al.FirstOrDefault() as SerialObjectAttribute;
                        if (a != null && !string.IsNullOrEmpty(a.Name))
                            _Name = a.Name;

                        else
                            _Name = GetType().Name;
                    }
                }
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        protected string _Memo;
        [ParameterOperation]
        public virtual string Memo
        {
            get
            {
                if (string.IsNullOrEmpty(_Memo))
                {
                    var al = GetType().GetTypeInfo().GetCustomAttributes(typeof(SerialObjectAttribute), true);
                    if (al != null && al.Count() > 0)
                    {
                        var a = al.FirstOrDefault() as SerialObjectAttribute;
                        if (a != null && !string.IsNullOrEmpty(a.Memo))
                            _Memo = a.Memo;
                    }
                }
                return _Memo;
            }
            set { _Memo = value; }
        }

        List<Parameter> _ParameterList = new List<Parameter>();
        public List<Parameter> ParameterList { get { return _ParameterList; } }

        //List<string> _KeyWordList = new List<string>();
        //public List<string> KeyWordList { get { return _KeyWordList; } }

        public virtual string GetSerialParameter()
        {
            SaveToParameterList();
            return CommonLib.CommonProc.ConvertObjectToString(ParameterList);
        }

        public virtual void DeserialParameter(string parameterSerialString)
        {
            var l = CommonLib.CommonProc.ConvertStringToObject<List<Parameter>>(parameterSerialString);
            if (l != null)
                l.ForEach(v => ParameterList.Add(v));
            LoadFromParameterList();
        }
        public virtual void LoadFromParameterList()
        {
            Parameter.LoadInfoFromParameterList(this, ParameterList);
        }

        public virtual void SaveToParameterList()
        {
            ParameterList.Clear();
            ParameterList.AddRange(Parameter.SaveInfoToParameterList(this));
        }
        public abstract ICopySupportObject Clone();
        public abstract string AbstractInfo { get; }
    }

    public class SerialObjectAttribute : Attribute
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
    }
}
