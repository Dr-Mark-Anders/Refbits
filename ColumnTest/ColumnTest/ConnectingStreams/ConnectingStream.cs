﻿using ModelEngine;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Units;
using Units.UOM;

namespace ModelEngineTest
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ConnectingStreamTest : UnitOperation, ISerializable
    {
        public ConnectingStreamTest()
        { }

        public ConnectingStreamTest(string name, TraySectionTest drawsection, TrayTest drawtray,
            TraySectionTest returnsection, TrayTest returntray, bool isliquid, double flowestimate = 0.1) : base()
        {
            this.EngineDrawSection = drawsection;
            this.engineDrawTray = drawtray;
            this.EngineReturnSection = returnsection;
            this.engineReturnTray = returntray;
            this.Name = name;
            this.isliquid = isliquid;
            this.FlowEstimate = flowestimate;
        }

        public bool isliquid = true;
        public TrayTest engineDrawTray;
        public TrayTest   engineReturnTray;
        public TraySectionTest EngineDrawSection;
        public TraySectionTest EngineReturnSection;

        public int engineDrawTrayIndex;
        public int engineReturnTrayIndex;
        public int engineDrawSectionIndex;
        public int engineReturnSectionIndex;

        public void InitialiseIndicies()
        {
            engineDrawTrayIndex = 0;
            engineReturnTrayIndex = 0;
            engineDrawSectionIndex = 0;
            engineReturnSectionIndex = 0;
        }

        public ConnectingStreamTest CloneDeep(COMColumn col)
        {
            ConnectingStreamTest cs = new();

            cs.EngineDrawSection = this.EngineDrawSection;
            cs.engineDrawTray = this.engineDrawTray;
            cs.EngineReturnSection = this.EngineReturnSection;
            cs.engineReturnTray = this.engineReturnTray;
            cs.Name = this.Name;
            cs.isliquid = isliquid;
            cs.FlowEstimate = this.FlowEstimate;

            return cs;
        }

        //public SpecificationCollectionSpecs = new SpecificationCollection();
        public double DrawFactor { get; set; } = 0.5;

        public double TempDrawFactor { get; set; }
        public StreamProperty Flow;
        public bool isNetBottomConnectedFlow = false;

        public override string ToString()
        {
            return Name;
        }

        public double MW
        {
            get
            {
                if (engineDrawTray != null)
                    return engineDrawTray.liquidDrawRight.cc.MW();
                else
                    return double.NaN;
            }
        }

        public double SG
        {
            get
            {
                if (engineDrawTray != null)
                    return engineDrawTray.liquidDrawRight.cc.SG();
                else
                    return double.NaN;
            }
        }

        public MoleFlow MoleFlow
        {
            get
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            return Flow.BaseValue;

                        case ePropID.MF:
                            return Flow.BaseValue / MW;

                        case ePropID.VF:
                            return Flow.BaseValue * SG / MW;
                    }
                return double.NaN;
            }
            set
            {
                switch (Flow.Propid)
                {
                    case ePropID.MOLEF:
                        Flow.BaseValue = value;
                        break;

                    case ePropID.MF:
                        Flow.BaseValue = value * MW;
                        break;

                    case ePropID.VF:
                        Flow.BaseValue = value * MW / SG;
                        break;
                }
            }
        }

        public MassFlow MassFlow
        {
            get
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            return Flow.BaseValue / MW;

                        case ePropID.MF:
                            return Flow.BaseValue;

                        case ePropID.VF:
                            return Flow.BaseValue * SG;
                    }
                return double.NaN;
            }
            set
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            Flow.BaseValue = value / MW;
                            break;

                        case ePropID.MF:
                            Flow.BaseValue = value;
                            break;

                        case ePropID.VF:
                            Flow.BaseValue = value * SG;
                            break;
                    }
            }
        }

        public VolumeFlow VolFlow
        {
            get
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            return Flow.BaseValue * MW / SG;

                        case ePropID.MF:
                            return Flow.BaseValue / SG;

                        case ePropID.VF:
                            return Flow.BaseValue;
                    }
                return double.NaN;
            }
            set
            {
                if (Flow != null)
                    switch (Flow.Propid)
                    {
                        case ePropID.MOLEF:
                            Flow.BaseValue = value * SG / MW;
                            break;

                        case ePropID.MF:
                            Flow.BaseValue = value * SG;
                            break;

                        case ePropID.VF:
                            Flow.BaseValue = value;
                            break;
                    }
            }
        }

        public new bool IsActive { get; set; }

        public int DrawTrayIndex
        {
            get
            {
                if (EngineDrawSection != null)
                    return EngineDrawSection.Trays.IndexOf(engineDrawTray);
                else
                    return -1;
            }
        }

        public int ReturnTrayIndex
        {
            get
            {
                if (EngineReturnSection != null)
                    return EngineReturnSection.Trays.IndexOf(engineReturnTray);
                else
                    return -1;
            }
        }

        public double FlowEstimate { get; set; }
        //public TrayEngineDrawTray{get=>engineDrawTray;set=>engineDrawTray=value;}
        //public TrayEnginereturnTray{get=>enginereturnTray;set=>enginereturnTray=value;}
        //public TraySectionEngineDrawSection{get=>enginedrawSection;set=>enginedrawSection=value;}
        //public TraySectionEnginereturn  Section{get=>enginereturn  Section;set=>enginereturn  Section=value;}
        //public int EngineDrawTrayIndex{get=>engineDrawTrayIndex;set=>engineDrawTrayIndex=value;}
        //public int EnginereturnTrayIndex{get=>enginereturnTrayIndex;set=>enginereturnTrayIndex=value;}
        //public int EngineDrawSectionIndex{get=>engineDrawSectionIndex;set=>engineDrawSectionIndex=value;}
        //public int Enginereturn  SectionIndex{get=>enginereturn  SectionIndex;set=>enginereturn  SectionIndex=value;}

        internal int ReturnSectionIndex(COMColumn column)
        {
            return column.TraySections.IndexOf(EngineReturnSection);
        }

        internal int DrawSectionIndex(COMColumn column)
        {
            return column.TraySections.IndexOf(EngineDrawSection);
        }
    }
}