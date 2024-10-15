using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Units
{
    [Serializable]
    public class DataArray : ISerializable
    {
        private readonly List<string> dataNames = new();
        private List<List<int>> dataIndex = new();
        private List<List<double>> dataList = new();

        public List<List<double>> Data
        {
            get
            {
                return dataList;
            }
            set
            {
                dataList = value;
            }
        }

        public void Addvalue(int row, int column, double datapoint, int dataindex)
        {
            dataList[row][column] = datapoint;
            dataIndex[row][column] = dataindex;
        }

        public void Addvalue(string Name, double datapoint, int columnindex)
        {
            bool namefound = false;
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataNames[i] == Name)
                {
                    dataList[i].Add(datapoint);
                    dataIndex[i].Add(columnindex);
                    namefound = true;
                }
            }
            if (!namefound)
            {
                dataNames.Add(Name);
                dataList.Add(new List<double>());
                dataIndex.Add(new List<int>());
                dataList[^1].Add(datapoint);
                dataIndex[^1].Add(columnindex);
            }
        }

        public void AddRow(string Name, double[] newdata)
        {
            dataNames.Add(Name);
            dataList.Add(new List<double>(newdata));
        }

        public DataArray()
        {
            dataNames = new List<string>();
        }

        public double this[int row, int column]
        {
            get
            {
                if (dataList.Count > row && Data[0].Count > column)
                    return Data[row][column];
                else return double.NaN;
            }
        }

        public double[] this[int row]
        {
            get
            {
                if (dataList.Count > row)
                    return dataList[row].ToArray();
                else return null;
            }
            set
            {
                if (dataList.Count > row)
                    dataList[row] = new List<double>(value);
                else
                    dataList.Add(new List<double>(value));
            }
        }

        public double[] this[string row]
        {
            get
            {
                if (dataNames.Contains(row))
                {
                    return dataList[dataNames.IndexOf(row)].ToArray();
                }
                else return null;
            }
            set
            {
                if (dataNames.Contains(row))
                {
                    dataList[dataNames.IndexOf(row)] = new List<double>(value);
                }
            }
        }

        public int GetRowIndex(string rowname)
        {
            if (dataNames.Contains(rowname))
            {
                return dataNames.IndexOf(rowname);
            }
            else return -999;
        }

        public bool Contains(string row)
        {
            if (dataNames.Contains(row))
                return true;
            else return false;
        }

        public int CountValidEntries(int Col)
        {
            int sum = 0;

            for (int i = 0; i < Data.Count; i++) // number of rows (componenets)
            {
                if (Data[i][Col] != 0 && !double.IsNaN(Data[i][Col]))
                    sum += 1;
            }

            return sum;
        }

        public string[] Names()
        {
            return dataNames.ToArray();
        }

        public int Count
        {
            get
            {
                return Data.Count; // number of rows
            }
        }

        public List<List<int>> DataIndex { get => dataIndex; set => dataIndex = value; }

        public void Clear()
        {
            dataList.Clear();
            dataNames.Clear();
        }

        public void Init(int Row, int Column)
        {
            dataList.Clear();
            dataIndex.Clear();
            for (int row = 0; row < Row; row++)
            {
                dataList.Add(new List<double>(new double[Column]));
                dataIndex.Add(new List<int>(new int[Column]));
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("dataNames", dataNames);
            info.AddValue("DataIndex", DataIndex);
            info.AddValue("dataList", dataList);
            info.AddValue("Data", Data);
        }

        public DataArray(SerializationInfo info, StreamingContext context)
        {
            try
            {
                dataNames = (List<string>)info.GetValue("dataNames", typeof(List<string>));
                DataIndex = (List<List<int>>)info.GetValue("DataIndex", typeof(List<List<int>>));
                dataList = (List<List<double>>)info.GetValue("dataList", typeof(List<List<double>>));
                Data = (List<List<double>>)info.GetValue("Data", typeof(List<List<double>>));
            }
            catch { }
        }
    }
}