using ModelEngine;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Units
{
    public partial class CreateAssayClass
    {
        private readonly DataGridView DGV;
        private readonly DataArrays data;
        private readonly enumAssayType assayType;
        private readonly AssayPropertyCollection apc = new();

        public CreateAssayClass(DataGridView DGV, DataArrays Data, enumAssayType assaytype)
        {
            this.DGV = DGV;
            this.assayType = assaytype;
            this.data = Data;
        }

        public CreateAssayClass()
        {
        }

        public void LoadDataIntoArrays(Port_Material port)
        {
            string DataBaseName;
            this.data.Components.Clear();
            this.data.Components.Add(port.cc.Pseudo());

            Dictionary<string, string> CompDict = new();
            /* Name
            Methane
            Ethane
            Propane
            i-Butane
            n-Butane
            i-Pentane
            n-Pentane
            n-Hexane
            n-Heptane
            n-Octane
            n-Nonane
            n-Decane
            */

            CompDict.Add("H2", "Hydrogen");
            CompDict.Add("H2S", "H2S");
            CompDict.Add("N2", "Nitrogen");
            CompDict.Add("C1", "Methane");
            CompDict.Add("C2", "Ethane");
            CompDict.Add("C3", "Propane");
            CompDict.Add("iC4", "i-Butane");
            CompDict.Add("nC4", "n-Butane");
            CompDict.Add("iC5", "i-Pentane");
            CompDict.Add("nC5", "n-Pentane");

            //this.data.ComponentList.Clear();
            this.data.StreamNames.Clear();
            this.data.DistDataRaw.Clear();
            this.data.DistTypeRaw.Clear();
            this.data.PureCompDataRaw.Clear();
            this.data.StreamFlowsRaw.Clear();
            this.data.StreamTotalDensity.Clear();
            this.data.PureCompDataType.Clear();
            this.data.MidBP.Clear();
            this.data.Properties.Clear();
            Components smallcomplist = new();

            //this.data.ComponentList.AddRange(fs.ComponentList.Pure());
            this.data.PureCompDataRaw = new DataArray();

            for (int DGVRow = 0; DGVRow < DGV.RowCount; DGVRow++)
            {
                string RowName = DGV[0, DGVRow].Value.ToString();
                switch (RowName)
                {
                    case "Name":
                        this.data.StreamNames = GetStringRow(DGVRow);
                        break;

                    case "H2":
                    case "N2":
                    case "H2S":
                    case "C1":
                    case "C2":
                    case "C3":
                    case "iC4":
                    case "nC4":
                    case "iC5":
                    case "nC5":
                        DataBaseName = CompDict[RowName];
                        GetCompData(port, DGVRow, DataBaseName, smallcomplist);  // Pure Componenents
                        break;

                    case "1%":
                    case "5%":
                    case "10%":
                    case "20%":
                    case "30%":
                    case "50%":
                    case "70%":
                    case "80%":
                    case "90%":
                    case "95%":
                    case "99%":
                        double[] row = GetDistData(DGVRow);
                        this.data.DistDataRaw.Add(row); // Distillation Data
                        break;

                    case "DENSITY15":
                        GetNumericData(this.data.StreamTotalDensity, DGVRow);
                        GetPropertyData(this.data.Properties, DGVRow, enumAssayPCProperty.DENSITY15);
                        break;

                    case "TBPCUTPOINT":
                    case "TBP CUTPOINT":
                        GetNumericData(this.data.TBPCutPointsRaw, DGVRow);
                        enumTemp res;
                        if (Enum.TryParse(DGV[1, DGVRow].Value.ToString(), out res))
                            this.data.TBPCutPointBasis = res;
                        else
                            this.data.DistTemperatureTypeRaw = enumTemp.NON;
                        break;

                    case "Composition Basis":
                        GetCompositionBasis(this.data.PureCompDataType, DGVRow);
                        break;

                    case "Dist Type":
                        GetDistType(this.data.DistTypeRaw, DGVRow);
                        enumTemp restbp;
                        if (DGV[1, DGVRow].Value != null && Enum.TryParse(DGV[1, DGVRow].Value.ToString(), out restbp))
                            this.data.DistTemperatureTypeRaw = restbp;
                        else
                            this.data.DistTemperatureTypeRaw = enumTemp.NON;
                        break;

                    case "Flow/Percent":
                        GetNumericData(this.data.StreamFlowsRaw, DGVRow);
                        enumMassVolMol resflow;
                        if (Enum.TryParse(DGV[1, DGVRow].Value.ToString(), out resflow))
                            this.data.StreamFlowBasis = resflow;
                        else
                            this.data.StreamFlowBasis = enumMassVolMol.Vol_PCT;
                        break;

                    default:
                        enumAssayPCProperty prop;
                        if (apc.Contains(RowName))
                        {
                            prop = apc[RowName].Prop;
                            GetPropertyData(this.data.Properties, DGVRow, prop);
                        }
                        break;
                }
            }

            for (int i = smallcomplist.Count - 1; i >= 0; i--)
            {
                this.data.Components.Insert(smallcomplist[i]);
            }

            //this.Data.Components.SortByBP(); dont do this here, messes up the array orders
        }

        private void GetCompositionBasis(List<enumPCTType> dataArray, int row)
        {
            for (int i = 2; i < DGV.ColumnCount; i++)
            {
                if (Enum.TryParse(DGV[i, row].Value.ToString(), out enumPCTType res))
                    dataArray.Add(res);
                else
                    dataArray.Add(enumPCTType.NaN);
            }
        }

        private void GetDistType(List<enumDistType> dataArray, int row)
        {
            for (int i = 2; i < DGV.ColumnCount; i++)
            {
                if (Enum.TryParse(DGV[i, row].Value.ToString(), out enumDistType res))
                    dataArray.Add(res);
                else
                    dataArray.Add(enumDistType.NON);
            }
        }

        private void GetNumericData(List<double> dataArray, int row)
        {
            for (int i = 2; i < DGV.ColumnCount; i++)
            {
                if (DGV[i, row].Value != null && double.TryParse(DGV[i, row].Value.ToString(), out double res))
                    dataArray.Add(res);
                else
                    dataArray.Add(double.NaN);
            }
        }

        private void GetPropertyData(DataArray dataArray, int DGVrow, enumAssayPCProperty prop)
        {
            for (int DVGCol = 2; DVGCol < DGV.ColumnCount; DVGCol++)
            {
                if (DGV[DVGCol, DGVrow].Value is not null)
                    if (double.TryParse(DGV[DVGCol, DGVrow].Value.ToString(), out double res))
                        dataArray.Addvalue(apc[prop].Name, res, DVGCol - 3);
            }
        }

        private double[] GetDistData(int row)
        {
            double[] data = new double[this.data.StreamNames.Count];
            if (data.Length == 0)
                return data;

            for (int i = 2; i < DGV.ColumnCount; i++)
            {
                if (DGV[i, row].Value != null)
                    if (double.TryParse(DGV[i, row].Value.ToString(), out double res))
                        data[i - 2] = res;
                    else
                        data[i - 2] = double.NaN;
                else
                    data[i - 2] = double.NaN;
            }
            return data;
        }

        private List<string> GetStringRow(int row)
        {
            List<string> list = new();
            var res = DGV.Rows[row];
            for (int i = 2; i < res.Cells.Count; i++)
            {
                if (res.Cells[i].Value != null)
                    list.Add(res.Cells[i].Value.ToString());
            }

            return list;
        }

        private void GetCompData(Port_Material port, int row, string name, Components smallcomplist)
        {
            double[] data = new double[this.data.StreamNames.Count];

            for (int i = 0; i < this.data.StreamNames.Count - 1; i++)
            {
                if (DGV[i + 2, row].Value != null)
                {
                    if (double.TryParse(DGV[i + 2, row].Value.ToString(), out double res))
                        data[i] = res;
                    else
                        data[i] = 0;
                }
                else
                    data[i] = 0;
            }

            BaseComp bc = port.cc[name];
            if (bc != null) // componenent exists
            {
                smallcomplist.Add(bc);
                this.data.PureCompDataRaw.Data.Add(new List<double>(data));
            }
            else
                MessageBox.Show("Component " + name + " is not present in the component list", "Warining", MessageBoxButtons.OK);
            return;
        }

        private int findRow(string val)
        {
            for (int i = 0; i < DGV.RowCount; i++)
                if (val.ToUpper() == DGV[0, i].Value.ToString().ToUpper())
                    return i;

            return 9999;
        }
    }
}