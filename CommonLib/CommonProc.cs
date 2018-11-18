using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace CommonLib
{
    //support serialize function 
    //last modified 2017-5-17
    public class CommonProc
    {
        public static readonly string CurrentDebug = "Weight Strategy";
        public static readonly double EPSILON = 0.00000000000000000000001;
        private static readonly JsonSerializer jsonSerializer = JsonSerializer.Create(
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //public static void SaveObjToFile(object o, string url)
        //{
        //    if (File.Exists(url)) File.Delete(url);
        //    using (var tw = File.CreateText(url))
        //    using (var jtw = new JsonTextWriter(tw) { Formatting = Newtonsoft.Json.Formatting.Indented })
        //    {
        //        jsonSerializer.Serialize(jtw, o);
        //    }
        //}
        public static async void SaveStringToFile(Windows.Storage.StorageFile file,string s)
        {
            using (StorageStreamTransaction transaction = await file.OpenTransactedWriteAsync())
            {
                using (DataWriter dataWriter = new DataWriter(transaction.Stream))
                {
                    dataWriter.WriteString(s);
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.CommitAsync();
                }
            }
        }
        public static async void SaveStringToFile(string s, Dictionary<string,List<string>> filter = null)
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker();
                picker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

                if (filter == null)
                {
                    filter = new Dictionary<string, List<string>>();
                    filter.Add("Text File", new List<string>() { ".txt" });
                }
                foreach (var kv in filter)
                    picker.FileTypeChoices.Add(kv.Key, kv.Value);
                Windows.Storage.StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    SaveStringToFile(file, s);
                    //using (StorageStreamTransaction transaction = await file.OpenTransactedWriteAsync())
                    //{
                    //    using (DataWriter dataWriter = new DataWriter(transaction.Stream))
                    //    {
                    //        dataWriter.WriteString(s);
                    //        transaction.Stream.Size = await dataWriter.StoreAsync();
                    //        await transaction.CommitAsync();
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {

                LogSupport.Error(ex);
            }
        }
        public static void SaveToFile(StorageFile file,object content)
        {
            if (file != null)
            {
                var s = ConvertObjectToString(content);
                SaveStringToFile(file, s);
            }
        }
        public static async void SaveToFile(Action<StorageFile> callBack,object o, Dictionary<string, List<string>> filter = null)
        {
            if (callBack == null) return;
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            if (filter == null)
            {
                filter = new Dictionary<string, List<string>>();
                filter.Add("Text File", new List<string>() { ".txt" });
            }
            foreach (var kv in filter)
                picker.FileTypeChoices.Add(kv.Key, kv.Value);

            Windows.Storage.StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                var s = ConvertObjectToString(o);
                SaveStringToFile(file, s);
                callBack(file);
            }
            //SaveObjToFile(o, file.Name);
            
        }
        public static async void LoadStringFromFile(Action<StorageFile, string> callBack,string filter = null)
        {
            if (callBack == null) return;
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            if (filter == null)
                filter = ".txt";
            picker.FileTypeFilter.Add(filter);
            StorageFile file = await picker.PickSingleFileAsync();
            LoadStringFromFile((s) => { callBack(file, s); }, file);
        }

        public static async void LoadStringFromFile(Action<string> callBack,StorageFile file)
        {
            if (callBack == null) return;
            if (file != null)
            {
                using (IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    using (DataReader dataReader = new DataReader(readStream))
                    {
                        UInt64 size = readStream.Size;
                        if (size <= UInt32.MaxValue)
                        {
                            UInt32 numBytesLoaded = await dataReader.LoadAsync((UInt32)size);
                            string fileContent = dataReader.ReadString(numBytesLoaded);
                            callBack(fileContent);
                        }
                    }
                }

            }

        }

        public static void LoadFromFile<T>(Action< T> callBack, StorageFile file)
        {
            LoadStringFromFile(( s) =>
            {
                var obj = ConvertStringToObject<T>(s);
                callBack(obj);
            }, file);
        }
            public static void LoadFromFile<T>(Action<StorageFile, T> callBack,string filter = null)
        {
            LoadStringFromFile( (path,s)=> 
            {
                var obj=ConvertStringToObject<T>(s);
                callBack(path,obj);
            }, filter);

            //var picker = new Windows.Storage.Pickers.FileOpenPicker();
            //picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            //picker.SuggestedStartLocation =
            //    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            //if (filter == null)
            //    filter = ".txt";
            //picker.FileTypeFilter.Add(filter);
            //Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            //if (file != null)
            //{
            //    callBack( LoadObjFromFile<T>(file.Path));
            //}

        }
        //public static T LoadObjFromFile<T>(string url)
        //{
        //    try
        //    {

        //        var s = File.ReadAllText(url);
        //        return ConvertStringToObject<T>(s);


        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //    //return default(T);
        //}


        public static T ConvertStringToObject<T>(string s)
        {
            var obj = default(T);
            using (var tr = new StringReader(s))
            {
                using (var jtr = new JsonTextReader(tr))
                {
                    var o = jsonSerializer.Deserialize(jtr, typeof(T));
                    if (o != null)
                        obj = (T)o;
                }
                
            }
            return obj;
        }
        public static object ConvertStringToObject(string s)
        {
            using (var tr = new StringReader(s))
            {
                using (var jtr = new JsonTextReader(tr))
                {
                    var obj = jsonSerializer.Deserialize(jtr);
                    if (obj != null)
                        return obj;
                }
                return null;
            }
            
        }
        public static object ConvertStringToObject(string s, Type t)
        {
            using (var tr = new StringReader(s))
            {
                using (var jtr = new JsonTextReader(tr))
                {
                    var obj = jsonSerializer.Deserialize(jtr, t);
                    if (obj != null)
                        return obj;
                }
                return null;
            }
            
        }
        public static T Copy<T>(T o)
        {
            string s = ConvertObjectToString(o);
            return ConvertStringToObject<T>(s);
        }
        public static TypeInfo FindType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;
            if (typeName.Contains(","))
                typeName = typeName.Substring(0, typeName.IndexOf(","));
            
            foreach (var t in Application.Current.GetType().GetTypeInfo().Assembly.DefinedTypes)
            {
                if (t.Name != null&&t.Name==typeName)
                    return t;
            }
            return null;
        }
        public static string ConvertObjectToString(object o)
        {
            if (o == null) return null;
            if (o is string) return o as string;
            //if (o is Enum) return o.ToString(); 
            string s;
            using (var tw = new StringWriter())
            using (var jtw = new JsonTextWriter(tw) { Formatting = Newtonsoft.Json.Formatting.None })
            {
                jsonSerializer.Serialize(jtw, o);
                s = tw.ToString();
            }
            return s;
        }

        public static void TrimDoubleList(List<double> l, bool trimLeft = true, bool trimRight = true)
        {
            int si = 0, ei = 0;
            for (int i = 0; i < l.Count; i++)
            {
                if (Math.Abs(l[i]) > EPSILON)
                {
                    si = i;
                    break;
                }
            }
            for (int i = l.Count - 1; i >= 0; i--)
            {
                if (Math.Abs(l[i]) > EPSILON)
                {

                    break;
                }
                ei++;
            }

            if (trimLeft)
            {
                for (int i = 0; i < si; i++)
                    l.RemoveAt(0);
            }
            if (trimRight)
                for (int i = 0; i < ei; i++)
                    l.RemoveAt(l.Count - 1);


        }

        public static void SynchroniseList<T>(IEnumerable<T> source, ICollection<T> target)
        {
            lock (source)
            {
                var l = target.Except(source).ToList();
                foreach (var v in l)
                    target.Remove(v);
                foreach (var v in source)
                    if (!target.Contains(v)) target.Add(v);
            }
        }

        public static double[] GetMA(double[] data, int n)
        {
            if (n <= 0) return data;
            double[] result = new double[data.Length];
            double[] top = new double[n];
            if (data.Length < n) return result;
            Array.Copy(data, top, n);

            double sum = top.Sum();
            result[n - 1] = sum / n;
            for (int i = n; i < data.Length; i++)
            {
                sum = sum + data[i] - data[i - n];
                result[i] = sum / n;
            }
            return result;
        }

        public static Dictionary<string, string> GetParameterDictionary(string[] args)
        {//every parameter starts with / or - and use : seperate name and value. like /targetTime:2014-5-23 /reportFormat:pdf /mailTo:yinxiao.liu@prcm.com
            var dl = new Dictionary<string, string>();
            foreach (var s in args)
            {
                var ts = s.Trim();
                if (ts.StartsWith("/") || ts.StartsWith("-"))
                {
                    if (ts.Contains(":"))
                    {
                        var ns = ts.Substring(1, ts.IndexOf(":", StringComparison.Ordinal) - 1).Trim();
                        var vs = ts.Substring(ts.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
                        dl.Add(ns, vs);
                    }
                }

            }
            return dl;
        }

        public static DateTime? GetWeekMonday(DateTime t)
        {
            if (t.DayOfWeek == DayOfWeek.Monday) return t;
            if (t.DayOfWeek == DayOfWeek.Tuesday) return t - TimeSpan.FromDays(1);
            if (t.DayOfWeek == DayOfWeek.Wednesday) return t - TimeSpan.FromDays(2);
            if (t.DayOfWeek == DayOfWeek.Thursday) return t - TimeSpan.FromDays(3);
            if (t.DayOfWeek == DayOfWeek.Friday) return t - TimeSpan.FromDays(4);
            if (t.DayOfWeek == DayOfWeek.Saturday) return t - TimeSpan.FromDays(5);
            if (t.DayOfWeek == DayOfWeek.Sunday) return t - TimeSpan.FromDays(6);
            return null;
        }
        public static object GetBasicTypeValue(Type t, object value)
        {
            if (t == typeof(bool))
                return Convert.ToBoolean(value);
            if (t == typeof(byte))
                return Convert.ToByte(value);
            if (t == typeof(short))
                return Convert.ToInt16(value);
            if (t == typeof(int))
                return Convert.ToInt32(value);
            if (t == typeof(long))
                return Convert.ToInt64(value);
            if (t == typeof(sbyte))
                return Convert.ToSByte(value);
            if (t == typeof(ushort))
                return Convert.ToUInt16(value);
            if (t == typeof(uint))
                return Convert.ToUInt32(value);
            if (t == typeof(ulong))
                return Convert.ToUInt64(value);
            if (t == typeof(float))
                return Convert.ToSingle(value);
            if (t == typeof(double))
                return Convert.ToDouble(value);
            if (t == typeof(decimal))
                return Convert.ToDecimal(value);
            if (t == typeof(DateTime))
                return Convert.ToDateTime(value);
            if (t == typeof(string))
                return Convert.ToString(value);
            if (t == typeof(char))
                return Convert.ToChar(value);
            if (t.GetTypeInfo().IsSubclassOf(typeof(Enum)))
                return Enum.Parse(t, Convert.ToString(value));

            return value;
        }
        public static bool SetProperty(object targetObject, PropertyInfo property, object value)
        {
            try
            {
                property.SetValue(targetObject, GetBasicTypeValue(property.PropertyType, value));
                return true;

            }
            catch
            {
                return false;
            }
        }

        public static string GetListString<T>(List<T> list, string splitStr = ",", bool keepLastSplitString = false, Func<T, string> toStringFunc = null)
        {
            if (list == null) return "";
            if (toStringFunc == null)
                toStringFunc = (t) => { return t.ToString(); };
            string s = "";
            list.ForEach(v =>
            {
                s += toStringFunc(v) + splitStr;
            });
            if (!keepLastSplitString)
            {
                s = s.Substring(0, s.Length - splitStr.Length);
            }
            return s;
        }

        public static string GetSpellCode(string CnStr)//get first chinese py for chinese string
        {

            string strTemp = "";

            int iLen = CnStr.Length;

            int i = 0;

            for (i = 0; i <= iLen - 1; i++)
            {

                strTemp += GetCharSpellCode(CnStr.Substring(i, 1));

            }

            return strTemp;

        }
        private static string GetCharSpellCode(string CnChar)
        {

            long iCnChar;

            byte[] ZW = System.Text.Encoding.Unicode.GetBytes(CnChar);

            //如果是字母，则直接返回

            if (ZW.Length == 1)
            {

                return CnChar.ToUpper();

            }

            else
            {

                // get the array of byte from the single char

                int i1 = (short)(ZW[0]);

                int i2 = (short)(ZW[1]);

                iCnChar = i1 * 256 + i2;

            }

            // iCnChar match the constant

            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {

                return "A";

            }

            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {

                return "B";

            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {

                return "C";

            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {

                return "D";

            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {

                return "E";

            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {

                return "F";

            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {

                return "G";

            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {

                return "H";

            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {

                return "J";

            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {

                return "K";

            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {

                return "L";

            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {

                return "M";

            }
            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {

                return "N";

            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {

                return "O";

            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {

                return "P";

            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {

                return "Q";

            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {

                return "R";

            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {

                return "S";

            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {

                return "T";

            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {

                return "W";

            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {

                return "X";

            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {

                return "Y";

            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {

                return "Z";

            }
            else

                return ("?");

        }
        public static int GetNumberInt(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }

        public static Page GetMainPage()
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null)
                return frame.Content as Page;
            return null;
        }

        public static async Task<bool> Confirm(string title,string content,string confirmString="OK",string cancelString="Cancel")
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = confirmString,
                CloseButtonText = cancelString,
                FullSizeDesired = false,
            };
            bool r = false;
            dialog.PrimaryButtonClick += (s, e) => { r= true; };
            await dialog.ShowAsync();
            return r;
        }

        public static async void  ShowMessage(string title,string content,string buttonString="OK")
        {
            var dialog = new MessageDialog(content, title);

            //dialog.Commands.Add(new UICommand(buttonString, cmd => { }, commandId: 0));

            await dialog.ShowAsync();
        }

        public static async void ShowMessage(Action commitAction, string title, FrameworkElement content, string closeString = "OK")
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = closeString,
                FullSizeDesired = false,
                Width=1080
            };

            dialog.PrimaryButtonClick += (_s, _e) => { commitAction(); };
            await dialog.ShowAsync();
        }

        public static List<T> GetValueList<T>() 
        {
            var l = new List<T>();
            var array = Enum.GetValues(typeof(T));
            if (array != null)
            {
                foreach (var v in array)
                    l.Add((T)v);
            }
            return l;
        }

        public static DateTime GetMinTime(DateTime time1,DateTime time2)
        {
            if (time1 > time2)
                return time2;
            return time1;
        }
        public static DateTime GetMaxTime(DateTime time1, DateTime time2)
        {
            if (time1 > time2)
                return time1;
            return time2;
        }
        public static object GetPropertyValue(object targetObject,List<string> propertyNameList)
        {
            if (targetObject == null) return null;
            var targetType = targetObject.GetType();
            PropertyInfo p = null;
            foreach(var n in propertyNameList)
            {
                if(p==null)
                    p = targetType.GetProperty(n);
            }
            if (p == null)
                return null;
            return p.GetValue(targetObject);
        }
        public static Guid? GetID(object value)
        {
            if (value == null) throw new Exception("No valid object to get id");
            var sl = new List<string>() { "ObjectID", "ObjectId", "ID", "Id",
                "Identity", "TargetObjectID", "TargetObjectId" };
            var v = GetPropertyValue(value, sl);
            if (v != null)
                return (Guid)v;
            return null;
            
        }
        //同名属性在两个对象之间尝试拷贝
        public static void CopySameNameProperty(object source,object target)
        {
            if (source == null || target == null) return;
            foreach(var property in source.GetType().GetRuntimeProperties())
            {
                var p = target.GetType().GetProperty(property.Name);
                if (p != null)
                {
                    try
                    {
                        p.SetValue(target, property.GetValue(source));
                    }
                    catch { }
                }
            }
        }

        //snapshot
        public static async Task SnapshotAsync(StorageFile sFile,FrameworkElement control)
        {
            if (sFile != null&&control!=null)
            {
                // 在用户完成更改并调用CompleteUpdatesAsync之前，阻止对文件的更新  
                CachedFileManager.DeferUpdates(sFile);
                //把控件变成图像  
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                //传入参数Image控件  
                await renderTargetBitmap.RenderAsync(control);

                var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

                using (var fileStream = await sFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        pixelBuffer.ToArray()
                        );
                    //刷新图像  
                    await encoder.FlushAsync();
                }
            }
        }
    }
}
