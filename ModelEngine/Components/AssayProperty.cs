using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelEngine
{
    [Serializable]
    public class AssayProperty : ISerializable
    {
        private string name;
        private enumMassVolMol blendType;
        private enumAssayPCProperty prop;

        public AssayProperty(string Name, enumMassVolMol BlendType, enumAssayPCProperty prop, bool allownegative = true)
        {
            this.name = Name;
            this.blendType = BlendType;
            this.prop = prop;
            this.AllowNegative = allownegative;
        }

        public AssayProperty(SerializationInfo info, StreamingContext context)
        {
            name = info.GetString("name");
            blendType = (enumMassVolMol)info.GetValue("blendType", typeof(enumMassVolMol));
            prop = (enumAssayPCProperty)info.GetValue("Prop", typeof(enumAssayPCProperty));
        }

        public enumMassVolMol BlendType { get => blendType; set => blendType = value; }
        public string Name { get => name; set => name = value; }
        public enumAssayPCProperty Prop { get => prop; set => prop = value; }
        public bool AllowNegative { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("blendType", blendType);
            info.AddValue("Prop", Prop);
        }
    }

    [Serializable]
    public class AssayPropertyCollection : ISerializable
    {
        private readonly List<AssayProperty> thelist = new();

        public AssayPropertyCollection(SerializationInfo info, StreamingContext context)
        {
            thelist = (List<AssayProperty>)info.GetValue("AssapProps", typeof(List<AssayProperty>));
        }

        public AssayPropertyCollection()
        {
            thelist.Add(new AssayProperty("ANILINE POINT", enumMassVolMol.Wt_PCT, enumAssayPCProperty.ANILINEPOINT, false));
            thelist.Add(new AssayProperty("BASIC NITROGEN", enumMassVolMol.Wt_PCT, enumAssayPCProperty.BASICNITROGEN, false));
            thelist.Add(new AssayProperty("C7 ASPHALTENES", enumMassVolMol.Wt_PCT, enumAssayPCProperty.C7ASPHALTENES, false));
            thelist.Add(new AssayProperty("NITROGEN", enumMassVolMol.Wt_PCT, enumAssayPCProperty.NITROGEN, false));
            thelist.Add(new AssayProperty("VANADIUM", enumMassVolMol.Wt_PCT, enumAssayPCProperty.VANADIUM, false));
            thelist.Add(new AssayProperty("NICKEL", enumMassVolMol.Wt_PCT, enumAssayPCProperty.NICKEL, false));
            thelist.Add(new AssayProperty("CONCARBON", enumMassVolMol.Wt_PCT, enumAssayPCProperty.CONCARBON, false));
            thelist.Add(new AssayProperty("IRON", enumMassVolMol.Wt_PCT, enumAssayPCProperty.IRON, false));
            thelist.Add(new AssayProperty("MERCAPTAN SULFUR", enumMassVolMol.Wt_PCT, enumAssayPCProperty.MERCAPTANSULFUR, false));
            thelist.Add(new AssayProperty("MICROCARBON RESIDUE", enumMassVolMol.Wt_PCT, enumAssayPCProperty.MICROCARBONRESIDUE, false));
            thelist.Add(new AssayProperty("TOTAL NITROGEN", enumMassVolMol.Wt_PCT, enumAssayPCProperty.TOTALNITROGEN, false));
            thelist.Add(new AssayProperty("SODIUM", enumMassVolMol.Wt_PCT, enumAssayPCProperty.SODIUM, false));
            thelist.Add(new AssayProperty("RAMSBOTTOM CARBON", enumMassVolMol.Wt_PCT, enumAssayPCProperty.RAMSBOTTOMCARBON, false));
            thelist.Add(new AssayProperty("TOTAL ACIDNUMER", enumMassVolMol.Wt_PCT, enumAssayPCProperty.TOTALACIDNUMER, false));

            thelist.Add(new AssayProperty("CETANE INDEX", enumMassVolMol.Vol_PCT, enumAssayPCProperty.CETANEINDEX));
            thelist.Add(new AssayProperty("CLOUD POINT", enumMassVolMol.Vol_PCT, enumAssayPCProperty.CLOUDPOINT));
            thelist.Add(new AssayProperty("MONC", enumMassVolMol.Vol_PCT, enumAssayPCProperty.MONC, false));
            thelist.Add(new AssayProperty("RONC", enumMassVolMol.Vol_PCT, enumAssayPCProperty.RONC, false));
            thelist.Add(new AssayProperty("POUR POINT", enumMassVolMol.Vol_PCT, enumAssayPCProperty.POURPOINT));
            thelist.Add(new AssayProperty("DENSITY15", enumMassVolMol.Vol_PCT, enumAssayPCProperty.DENSITY15, false));
            thelist.Add(new AssayProperty("SULFUR", enumMassVolMol.Vol_PCT, enumAssayPCProperty.SULFUR, false));
            thelist.Add(new AssayProperty("POUR POINT", enumMassVolMol.Vol_PCT, enumAssayPCProperty.POURPOINT));
            thelist.Add(new AssayProperty("FREEZE POINT", enumMassVolMol.Vol_PCT, enumAssayPCProperty.FREEZEPOINT));

            thelist.Add(new AssayProperty("PARAFFINS", enumMassVolMol.Vol_PCT, enumAssayPCProperty.PARAFFINS, false));
            thelist.Add(new AssayProperty("NAPHTHENES", enumMassVolMol.Vol_PCT, enumAssayPCProperty.NAPHTHENES, false));
            thelist.Add(new AssayProperty("AROMATICS", enumMassVolMol.Vol_PCT, enumAssayPCProperty.AROMATICS, false));
            thelist.Add(new AssayProperty("NAPHTHALENES", enumMassVolMol.Vol_PCT, enumAssayPCProperty.NAPHTHALENES, false));

            thelist.Add(new AssayProperty("HYDROGEN", enumMassVolMol.Vol_PCT, enumAssayPCProperty.HYDROGEN, false));
            thelist.Add(new AssayProperty("WAX", enumMassVolMol.Vol_PCT, enumAssayPCProperty.WAX, false));
            //thelist.Add(new AssayProperty("HYDROGEN", enumMassVolMol.Vol_PCT, enumAssayPCProperty.HYDROGEN));

            thelist.Add(new AssayProperty("VIS 20", enumMassVolMol.Vol_PCT, enumAssayPCProperty.VIS20, false));
            thelist.Add(new AssayProperty("VIS 40", enumMassVolMol.Vol_PCT, enumAssayPCProperty.VIS40, false));
            thelist.Add(new AssayProperty("VIS 50", enumMassVolMol.Vol_PCT, enumAssayPCProperty.VIS50, false));
            thelist.Add(new AssayProperty("VIS 60", enumMassVolMol.Vol_PCT, enumAssayPCProperty.VIS60, false));
            thelist.Add(new AssayProperty("VIS 100", enumMassVolMol.Vol_PCT, enumAssayPCProperty.VIS100, false));
            thelist.Add(new AssayProperty("VIS 130", enumMassVolMol.Vol_PCT, enumAssayPCProperty.VIS130, false));

            thelist.Add(new AssayProperty("0%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.IBP));
            thelist.Add(new AssayProperty("1%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP1));
            thelist.Add(new AssayProperty("5%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP5));
            thelist.Add(new AssayProperty("10%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP10));
            thelist.Add(new AssayProperty("20%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP20));
            thelist.Add(new AssayProperty("30%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP30));
            thelist.Add(new AssayProperty("40%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP40));
            thelist.Add(new AssayProperty("50%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP50));
            thelist.Add(new AssayProperty("60%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP60));
            thelist.Add(new AssayProperty("70%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP70));
            thelist.Add(new AssayProperty("80%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP80));
            thelist.Add(new AssayProperty("90%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP90));
            thelist.Add(new AssayProperty("95%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP95));
            thelist.Add(new AssayProperty("99%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.BP99));
            thelist.Add(new AssayProperty("100%", enumMassVolMol.Vol_PCT, enumAssayPCProperty.FBP));

            thelist.Add(new AssayProperty("TBP CUTPOINT", enumMassVolMol.Vol_PCT, enumAssayPCProperty.TBPCUTPOINT));
        }

        public List<string> Names()
        {
            List<String> res = new();
            for (int i = 0; i < thelist.Count; i++)
            {
                res.Add(thelist[i].Name);
            }
            return res;
        }

        public bool Contains(string prop)
        {
            for (int i = 0; i < thelist.Count; i++)
            {
                if (thelist[i].Name == prop)
                    return true;
            }
            return false;
        }

        public bool Contains(enumAssayPCProperty prop)
        {
            for (int i = 0; i < thelist.Count; i++)
            {
                if (thelist[i].Prop == prop)
                    return true;
            }
            return false;
        }

        public AssayProperty this[enumAssayPCProperty prop]
        {
            get
            {
                for (int i = 0; i < thelist.Count; i++)
                {
                    if (thelist[i].Prop == prop)
                        return thelist[i];
                }
                return null;
            }
        }

        public AssayProperty this[string prop]
        {
            get
            {
                for (int i = 0; i < thelist.Count; i++)
                {
                    if (thelist[i].Name == prop)
                        return thelist[i];
                }
                return null;
            }
        }

        public void Add(AssayProperty prop)
        {
            thelist.Add(prop);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AssapProps", thelist);
        }
    }
}