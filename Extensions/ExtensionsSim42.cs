using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }

    public class CloneableDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : ICloneable
    {
        public IDictionary<TKey, TValue> Clone()
        {
            CloneableDictionary<TKey, TValue> clone = new CloneableDictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> pair in this)
                clone.Add(pair.Key, (TValue)pair.Value.Clone());

            return clone;
        }
    }

    public static partial class Ext
    {
        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                    .GetTypeInfo()
                    .GetMember(e.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault()
                    as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }

        public static bool IsNan(this double d)
        {
            return double.IsNaN(d);
        }

        public static bool HasValue(this double d)
        {
            return !double.IsNaN(d);
        }

        public static void RemoveFromTo(this List<object> theList, int start, int range)
        {
            for (int i = start + range; i > start; i--)
            {
                theList.RemoveRange(start, range);
            }
        }

        public static double[][] Array(List<List<double>> data)
        {
            double[][] res = new double[data.Count()][];

            for (int i = 0; i < data.Count; i++)
            {
                res[i] = new double[data[i].Count];
                for (int y = 0; y < data[i].Count; y++)
                {
                    res[i][y] = data[i][y];
                }
            }
            return res;
        }

        public static double[][] Array(Tuple<int, int> data)
        {
            double[][] res = new double[data.Item1][];
            for (int i = 0; i < data.Item2; i++)
            {
                res[i] = new double[data.Item2];
            }
            return res;
        }

        public static double[] Array(int dim, double value)
        {
            double[] res = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                res[i] = value;
            }
            return res;
        }

        public static double[][] Array(double[][] data)
        {
            return new double[0][];
        }

        public static double[][][] Array(Tuple<int, int, int> shape)
        {
            double[][][] res = new double[shape.Item1][][];

            for (int i = 0; i < shape.Item1; i++)
            {
                res[i] = new double[shape.Item2][];
                for (int y = 0; y < shape.Item2; y++)
                {
                    res[i][y] = new double[shape.Item3];
                }
            }
            return res;
        }

        public static double[][] Array(int X, int Y, double defaultVal)
        {
            double[][] res = new double[X][];
            for (int i = 0; i < Y; i++)
            {
                res[i] = new double[Y];
                for (int y = 0; y < Y; y++)
                {
                    res[i][y] = defaultVal;
                }
            }
            return res;
        }

        public static double[] Array(double[] data)
        {
            return new double[0];
        }

        public static double[] Array(List<double> data)
        {
            return data.ToArray();
        }

        public static double[] Array(int size)
        {
            return new double[size];
        }

        public static double[] zeros(int size)
        {
            return new double[size];
        }

        public static double[][] zeros(int size, int size2)
        {
            double[][] res = new double[size][];

            for (int i = 0; i < size; i++)
            {
                res[i] = new double[size2];
            }
            return res;
        }

        public static double[][] ones(int size, int size2)
        {
            double[][] res = new double[size][];

            for (int i = 0; i < size; i++)
            {
                res[i] = new double[size2];
                for (int y = 0; y < size2; y++)
                {
                    res[i][y] = 1;
                }
            }
            return res;
        }

        public static double[] zerosnull(int size)
        {
            return new double[size];
        }

        public static double[][] Array(int a, int b)
        {
            double[][] res = new double[a][];

            for (int i = 0; i < a; i++)
            {
                res[i] = new double[b];
            }
            return res;
        }

        public static int[] ArrayInt(int a, int b)
        {
            int[] res = new int[a];
            for (int i = 0; i < a; i++)
            {
                res[i] = b;
            };
            return res;
        }

        public static int[] ArrayInt(int a)
        {
            int[] res = new int[a];
            for (int i = 0; i < a; i++)
            {
                res[i] = 0;
            };
            return res;
        }

        public static bool[] ArrayBool(int p, bool val)
        {
            bool[] res = new bool[p];
            for (int i = 0; i < p; i++)
            {
                res[i] = val;
            }
            return res;
        }

        public static bool HasAttr(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }

        public static object GetAttr(object m, string methodName)
        {
            var property = m.GetType().GetProperty(methodName);
            var res = property.GetValue(m);
            return res;
        }

        public static void SetAttr(object m, string att, object value, string name = null)
        {
            var property = m.GetType().GetProperty(att);
            property.SetValue(m, value);
        }

        public static void SetAttr(object m, string att, List<string> value, int val)
        {
            var property = m.GetType().GetProperty(att);
            property.SetValue(m, value);
        }

        public static double[][] Identity(int nIterationValues)
        {
            double[][] res = new double[nIterationValues][];

            for (int i = 0; i < nIterationValues; i++)
            {
                res[i] = new double[nIterationValues];
                res[i][i] = 1;
            };
            return res;
        }

        public static List<double> NewList(int v1, double v2)
        {
            List<double> res = new List<double>(v1);
            for (int i = 0; i < v1; i++)
            {
                res[i] = v2;
            }
            return res;
        }

        public static double[][] InitArray(int nuEquations1, int nuEquations2)
        {
            double[][] res = new double[nuEquations1][];
            for (int i = 0; i < nuEquations1; i++)
            {
                res[i] = new double[nuEquations2];
            };
            return res;
        }

        public static double[] InitArray(List<double> list)
        {
            double[] res = new double[list.Count()];
            for (int i = 0; i < list.Count(); i++)
            {
                res[i] = (double)list[i];
            };
            return res;
        }

        public static double[] InitArray(double[] list)
        {
            double[] res = new double[list.Count()];
            for (int i = 0; i < list.Count(); i++)
            {
                res[i] = (double)list[i];
            };
            return res;
        }

        public static double[][] InitArrayNull(int nuEquations1, int nuEquations2)
        {
            double[][] res = new double[nuEquations1][];
            for (int i = 0; i < nuEquations2; i++)
            {
                res[i] = new double[nuEquations2];
            };
            return res;
        }

        public static double[] InitArray(int nuEquations)
        {
            double[] res = new double[nuEquations];
            return res;
        }

        public static double[] InitArrayNull(int nuEquations)
        {
            double[] res = new double[nuEquations];
            return res;
        }

        public static int[] InitValues(int count, int v)
        {
            var res = new int[count];
            for (int i = 0; i < count; i++)
            {
                res[i] = v;
            }
            return res;
        }

        public static double[] InitValues(int count, double v)
        {
            var res = new double[count];
            for (int i = 0; i < count; i++)
                res[i] = v;

            return res;
        }

        public static double[][] InitValues(int count, int count2, double v)
        {
            var res = new double[count][];
            for (int i = 0; i < count; i++)
            {
                res[i] = new double[count2];
            }

            for (int i = 0; i < count; i++)
            {
                for (int y = 0; y < count2; y++)
                {
                    res[i][y] = v;
                }
            }

            return res;
        }

        public static List<double> ReverseSign(List<double> list)
        {
            var res = list;
            for (int i = 0; i < list.Count; i++)
            {
                res[i] = -list[i];
            }
            return res;
        }

        public static double[] ReverseSign(double[] list)
        {
            var res = list;
            for (int i = 0; i < list.Length; i++)
            {
                res[i] = -list[i];
            }
            return res;
        }

        public static List<double> take(double[] x, List<int> dutyIndex)
        {
            throw new NotImplementedException();
        }

        public static List<double> ReverseAndAccess(this List<List<double>> thelist, int index)
        {
            List<List<double>> temp = thelist;
            temp.Reverse();
            return temp[index];
        }

        public static bool Contains(this Tuple<string, string, string> thelist, string val)
        {
            if (thelist.Item1 == val || thelist.Item2 == val || thelist.Item3 == val)
                return true;
            else
                return false;
        }

        /// <summary> Gets the value of specified key. Simply returns the default value if dic or key are null or specified key does not exists.</summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            return (dic != null && key != null && dic.TryGetValue(key, out TValue value)) ? value : defaultValue;
        }

        public static List<double> ToDouble(this List<double> thelist)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < thelist.Count; i++)
            {
                res.Add((double)thelist[i]);
            }
            return res;
        }

        public static List<double> ToNullDouble(this List<double> thelist)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < thelist.Count; i++)
            {
                res.Add(thelist[i]);
            }
            return res;
        }

        public static T pop<T>(this List<T> list)
        {
            T d = list[0];
            list.RemoveAt(0);
            return d;
        }

        public static int pop(this List<int?> list, int pos)
        {
            dynamic d = list[pos];
            list.RemoveAt(pos);
            return d;
        }

        public static int pop(this List<int> list, int pos)
        {
            int d = list[pos];
            list.RemoveAt(pos);
            return d;
        }

        public static int pop(this List<double> list, int pos)
        {
            dynamic d = list[pos];
            list.RemoveAt(pos);
            return d;
        }

        public static string pop(this List<string> list, int pos)
        {
            dynamic d = list[pos];
            list.RemoveAt(pos);
            return d;
        }

        public static bool Contains(this Tuple<string, string> tup, string val)
        {
            if (tup.Item1 == val || tup.Item2 == val)
                return true;
            else
                return false;
        }

        public static List<string> ToList(this Tuple<string, string> tup)
        {
            List<string> res = new List<string>();
            res.Add(tup.Item1);
            res.Add(tup.Item2);
            return res;
        }

        public static dynamic get(this Dictionary<string, dynamic> dict, dynamic val)
        {
            return dict[val];
        }

        public static dynamic get(this Dictionary<string, dynamic> dict, dynamic val, dynamic def)
        {
            if (!dict.ContainsKey(val))
                return null;
            return dict[val];
        }

        public static string get(this Dictionary<int, string> dict, dynamic val, dynamic def)
        {
            if (!dict.ContainsKey(val))
                return null;
            return dict[val];
        }

        /* public static List<double> MidBP(this List<boilingpoint> me)
         {
             List<double> res = new List<double>();

             for (int i = 0; i < me.Count; i++)
             {
                 res.Add((me[i].icp + me[i].fbp)/2);
             }
             return res;
         }*/

        /// <summary>
        /// From Index to end of List
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static List<double> GetRange(this List<double> m, int Index)
        {
            List<double> res = new List<double>();

            for (int i = Index; i < m.Count; i++)
            {
                res.Add(m[i]);
            }
            return res;
        }

        public static List<double> GetRange(this double[] m, int Index, int Count)
        {
            List<double> res = new List<double>();

            for (int i = Index; i < m.Length; i++)
            {
                res.Add(m[i]);
            }
            return res;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);
        }


        public static PointF Center(this RectangleF rect)
        {
            return new PointF(rect.Left + rect.Width / 2f,
                             rect.Top + rect.Height / 2f);
        }

        public static int Bottom(this Rectangle rect)
        {
            return rect.Top + rect.Height;
        }

        public static int Right(this Rectangle rect)
        {
            return rect.Left + rect.Width;
        }

        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);
        }

        public static double[][] CloneDeep(this double[][] source)
        {
            var len = source.Length;
            var dest = new double[len][];

            for (var x = 0; x < len; x++)
            {
                var inner = source[x];
                var ilen = inner.Length;
                var newer = new double[ilen];
                System.Array.Copy(inner, newer, ilen);
                dest[x] = newer;
            }

            return dest;
        }

        public static double[] Vector(this double[][] d)
        {
            double[] res;
            int counter = 0;

            for (int a = 0; a < d.Length; a++)
            {
                counter += d[a].Length;
            }

            res = new double[counter];

            counter = 0;
            for (int a = 0; a < d.Length; a++)
            {
                for (int i = 0; i < d[a].Length; i++)
                {
                    res[counter] = d[a][i];
                    counter++;
                }
            }
            return res;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute),
                                                       false);
            return attributes.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}