using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Xml.Linq;
using Units.UOM;

/*DataSet1TableAdapters.EncryptedDatabaseTableAdapter dataTableAdapter;
'DataSet1 thermodata;
'static DataTable compData;'

'public  Thermodata()
'{
'    dataTableAdapter = new  DataSet1TableAdapters.EncryptedDatabaseTableAdapter();
'    thermodata = new  DataSet1();
'    dataTableAdapter.Fill(ThermoData.EncryptedDatabase);
'*/

public enum CompType
{
    Normal, Mixed,
    Short,
    YieldPattern
}

namespace ModelEngine
{
    public class Thermodata
    {
        private static Dictionary<string, string> shortNames = new();

        private static MixedComponents mix = new MixedComponents();

        public static DataTable compData
        {
            get
            {
                return ComponentData.data;
            }
        }

        public List<string> GetComponentNames
        {
            get
            {
                List<string> res = new List<string>();
                object dbres;
                for (int n = 0; n < compData.Rows.Count; n++)
                {
                    dbres = compData.Rows[n][0];
                    if (dbres.GetType() != typeof(DBNull))
                    {
                        res.Add((string)compData.Rows[n][0]);
                    }
                }
                return res;
            }
        }

        public static MixedComponents Mix { get => mix; set => mix = value; }
        public static Dictionary<string, string> ShortNames { get => shortNames; set => shortNames = value; }

        public static double GetProp(DataRow row, string property)
        {
            object o;

            if (compData.Columns.Contains(property))
                o = row[property];
            else
                return 0;

            if (o.GetType() == typeof(double))
            {
                return (double)o;
            }
            if (o.GetType() == typeof(string))
            {
                double res;
                if (double.TryParse(o.ToString(), out res))
                    return Convert.ToDouble(res);
                else
                    return 0;
            }
            return 0;
        }

        public static string GetString(DataRow row, string property)
        {
            object o;

            if (compData.Columns.Contains(property))
                o = row[property];
            else
                return "";

            if (o is string)
            {
                return (string)o;
            }
            return "";
        }

        public static bool TryGetComponent(string InputName, out BaseComp comp)
        {
            DataRow[] foundRows;
            string expression = "Name = '" + InputName + "'";
            // Use the Select method to find all rows matching the filter.
            foundRows = compData.Select(expression);
            if (foundRows is null)
            {
                comp = null;
                return false;
            }
            else
            {
                comp = GetComponent(InputName);
                return true;
            }
        }


        public static BaseComp GetComponent(string InputName, bool ShowWarning = false)
        {
            BaseComp p = null;
            InputName = InputName.ToUpper();
            string Name = InputName;

            if (mix.Contains(InputName)) // get mix component
            {
                p = mix.NewBaseComp(InputName);
                return p;
            }

            if (shortNames.ContainsKey(InputName)) // get real name
            {
                Name = shortNames[InputName].ToUpper();
            }

            string expression = "Name = '" + Name + "'";
            DataRow[] foundRows;
            string compname;
            // Use the Select method to find all rows matching the filter.
            foundRows = compData.Select(expression);

            if (foundRows is null)
            {
                compname = checkpseudonym(Name); // cehck for pseudonym

                if (compname is not null)
                {
                    Name = compname;
                    foundRows = compData.Select("Name = '" + Name + "'");
                }
            }

            if (foundRows.Length > 0)
            {
                DataRow row = foundRows[0];
                p = new BaseComp();
                p.Name = Name;
                p.AntK = GetAntArray(row);
                p.BP = new Temperature(GetProp(row, "TB"));
                p.IdealVapCP = GetVapArray(row);
                p.MW = GetProp(row, "MW");
                //p.Density = GetProp(row, "Density");
                p.Unifac = GetString(row, "UNIFAC");
                p.SG_60F = GetProp(row, "SG6060");
                p.CritT = new Temperature(GetProp(row, "TC"));
                p.CritP = new Pressure(GetProp(row, "PC"), PressureUnit.BarA);
                p.CritV = GetProp(row, "VC");
                p.Omega = GetProp(row, "OMEGA");
                p.HForm25 = GetProp(row, "HFORM25") * 1000;
                p.GForm25 = GetProp(row, "GFORM25") * 1000;
                p.Formula = GetString(row, "Formula").Trim();
                p.CAS = GetString(row, "CAS");
                p.GibbsFree = GetGibbsArray(row);
                p.SecondaryName = InputName;
                return p;
            }

            if (p == null & ShowWarning)
            {
                System.Windows.Forms.MessageBox.Show("Component " + Name + " Not Found");
                return null;
            }

            return p;
        }

        private static string checkpseudonym(string name)
        {
            Dictionary<string, string> pseudonyms = new();
            pseudonyms.Add("", "");

            if (pseudonyms.ContainsKey(name))
                return pseudonyms[name];
            else return null;
        }

        public static BaseComp GetRealComponentCAS(string CAS)
        {
            BaseComp p = null;

            string expression = "CAS = '" + CAS + "'";
            DataRow[] foundRows;

            // Use the Select method to find all rows matching the filter.
            foundRows = compData.Select(expression);

            if (foundRows.Length > 0)
            {
                DataRow row = foundRows[0];
                p = new BaseComp();
                p.Name = GetString(row, "NAME");
                p.AntK = GetAntArray(row);
                p.BP = new Temperature(GetProp(row, "TB"));
                p.IdealVapCP = GetVapArray(row);
                p.MW = GetProp(row, "MW");
                //p.Density = GetProp(row, "Density");
                p.Unifac = GetString(row, "UNIFAC");
                p.SG_60F = GetProp(row, "SG6060");
                p.CritT = new Temperature(GetProp(row, "TC"));
                p.CritP = new Pressure(GetProp(row, "PC"), PressureUnit.BarA);
                p.CritV = GetProp(row, "VC");
                p.Omega = GetProp(row, "OMEGA");
                p.HForm25 = GetProp(row, "HFORM25") * 1000;
                p.GForm25 = GetProp(row, "GFORM25") * 1000;
                p.Formula = GetString(row, "Formula").Trim();
                p.CAS = GetString(row, "CAS");
                p.GibbsFree = GetVapArray(row);
                return p;
            }
            return p;
        }

        public static double[] GetAntArray(DataRow row)
        {
            double[] res = new double[6];
            res[0] = GetProp(row, "ANTA");
            res[1] = GetProp(row, "ANTB");
            res[2] = GetProp(row, "ANTC");
            res[3] = GetProp(row, "ANTD");
            res[4] = GetProp(row, "ANTE");
            res[5] = GetProp(row, "ANTF");
            return res;
        }

        public static double[] GetVapArrayOld(DataRow row)
        {
            double[] res = new double[6];
            res[0] = GetProp(row, "VCPA");
            res[1] = GetProp(row, "VCPB");
            res[2] = GetProp(row, "VCPC");
            res[3] = GetProp(row, "VCPD");
            res[4] = GetProp(row, "VCPE");
            res[5] = GetProp(row, "VCPF");
            return res;
        }

        public static double[] GetVapArray(DataRow row)
        {
            double[] res = new double[6];
            res[0] = GetProp(row, "CPA");
            res[1] = GetProp(row, "CPB");
            res[2] = GetProp(row, "CPC");
            res[3] = GetProp(row, "CPD");
            res[4] = GetProp(row, "CPE");
            res[5] = GetProp(row, "CPF");
            return res;
        }

        public static double[] GetGibbsArray(DataRow row)
        {
            double[] res = new double[3];
            res[0] = GetProp(row, "GIBBSA");
            res[1] = GetProp(row, "GIBBSB");
            res[2] = GetProp(row, "GIBBSC");
            return res;
        }
    }
}