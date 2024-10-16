﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [TypeConverter(typeof(SpecificationExpander)), Serializable]
    public class ConnectingStreamCollection  : ExpandableObjectConverter, IEnumerable, ISerializable
    {
        private readonly List<ConnectingStream> connectingStreams = new();

        public ConnectingStreamCollection LiquidStreams
        {
            get
            {
                ConnectingStreamCollection csc = new();
                for (int i = 0; i < connectingStreams.Count; i++)
                {
                    ConnectingStream cs = connectingStreams[i];
                    if (cs.isliquid)
                        csc.Add(cs);
                }
                return csc;
            }
        }

        public ConnectingStreamCollection VapourStreams
        {
            get
            {
                ConnectingStreamCollection csc = new();
                for (int i = 0; i < connectingStreams.Count; i++)
                {
                    ConnectingStream cs = connectingStreams[i];
                    if (!cs.isliquid)
                        csc.Add(cs);
                }
                return csc;
            }
        }

        public ConnectingStreamCollection()
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

        public ConnectingStreamCollection(SerializationInfo si, StreamingContext ctx)
        {
            try
            {
                connectingStreams = (List<ConnectingStream>)si.GetValue("PAList", typeof(List<ConnectingStream>));
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

        public ConnectingStream Add(ConnectingStream t)
        {
            connectingStreams.Add(t);
            return t;
        }

        public void AddRange(ConnectingStreamCollection t)
        {
            if (t != null)
                connectingStreams.AddRange(t.connectingStreams);
        }

        internal ConnectingStreamCollection Clone()
        {
            throw new NotImplementedException();
        }

        internal ConnectingStreamCollection CloneDeep(Column col)
        {
            ConnectingStreamCollection csc = new();

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

        public List<ConnectingStream> StreamList => connectingStreams;
    }
}