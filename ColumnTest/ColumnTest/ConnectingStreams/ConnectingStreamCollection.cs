using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngineTest
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class ConnectingStreamCollectionTest : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private readonly List<ConnectingStreamTest> connectingStreams = new();

        public ConnectingStreamCollectionTest LiquidStreams
        {
            get
            {
                ConnectingStreamCollectionTest csc = new();
                for (int i = 0; i < connectingStreams.Count; i++)
                {
                    ConnectingStreamTest cs = connectingStreams[i];
                    if (cs.isliquid)
                        csc.Add(cs);
                }
                return csc;
            }
        }

        public ConnectingStreamCollectionTest VapourStreams
        {
            get
            {
                ConnectingStreamCollectionTest csc = new();
                for (int i = 0; i < connectingStreams.Count; i++)
                {
                    ConnectingStreamTest cs = connectingStreams[i];
                    if (!cs.isliquid)
                        csc.Add(cs);
                }
                return csc;
            }
        }

        public ConnectingStreamCollectionTest()
        {
        }

        public void GetObjectData(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                si.AddValue("PAList", connectingStreams);
            }
            catch
            {
            }
        }

        public ConnectingStreamCollectionTest(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                connectingStreams = (List<ConnectingStreamTest>)si.GetValue("PAList", typeof(List<ConnectingStreamTest>));
            }
            catch
            {
            }
        }

        public IEnumerator GetEnumerator()
        {
            return connectingStreams.GetEnumerator();
        }

        public void Clear()
        {
            connectingStreams.Clear();
        }

        public ConnectingStreamTest  Add(ConnectingStreamTest t)
        {
            connectingStreams.Add(t);
            return t;
        }

        public void AddRange(ConnectingStreamCollectionTest t)
        {
            if (t != null)
                connectingStreams.AddRange(t.connectingStreams);
        }

        internal ConnectingStreamCollectionTest Clone()
        {
            throw new NotImplementedException();
        }

        internal ConnectingStreamCollectionTest CloneDeep(COMColumn     col)
        {
            ConnectingStreamCollectionTest csc = new();

            for (int i = 0; i < connectingStreams.Count; i++)
            {
                csc.Add(connectingStreams[i].CloneDeep(col));
            }

            return csc;
        }

        public int Count
        {
            get
            {
                return connectingStreams.Count;
            }
        }

        public List<ConnectingStreamTest> StreamList => connectingStreams;
    }
}