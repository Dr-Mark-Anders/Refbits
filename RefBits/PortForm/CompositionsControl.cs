using ModelEngine;
using ModelEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Units
{
    //public enumState{Mole,Mass,Vol};

    public partial class CompositionsControl : Form
    {
        private PortList ports;
        private List<StreamMaterial> streams = new List<StreamMaterial>();
        private Dictionary<Guid, string> names;
        private eDisplayState CurrentState = eDisplayState.Mole;

        public CompositionsControl(PortList ports, Dictionary<Guid, string> names)
        {
            BaseComp bc;
            this.ports = ports;
            this.names = names;
            InitializeComponent();
            DGV.Rows.Add(ports.GetFirst.ComponentList.Count);

            foreach (var p in ports)
            {
                if (p.IsConnected)
                {
                    DGV.Columns.Add("Column" + names[p.Guid], names[p.Guid]);
                    for (int i = 0; i < p.ComponentList.Count; i++)
                    {
                        bc = p.ComponentList[i];
                        DGV[0, i].Value = bc.Name;
                    }
                }
            }
        }

        public CompositionsControl(List<StreamMaterial> streams)
        {
            BaseComp bc;
            Port_Material port;
            ports = new PortList();
            this.streams = streams;
            this.names = null;
            InitializeComponent();
            port = streams[0].Port;
            DGV.Rows.Add(port.ComponentList.Count);
            for (int i = 0; i < port.ComponentList.Count; i++)
            {
                bc = port.ComponentList[i];
                DGV[0, i].Value = bc.Name;
            }

            int column;
            foreach (StreamMaterial stream in streams)
            {
                ports.Add(stream.Port);
                port = stream.Port;
                column = DGV.Columns.Add("Column" + stream.Name, stream.Name);
                DGV.Columns[column].Width = 100;
            }
        }

        private void CompositionForm_Load(object sender, System.EventArgs e)
        {
        }

        public void FillFractionData()
        {
            int count = 0;

            foreach (var port in ports)
            {
                count++;
                if (port.ComponentList != null)
                    for (int i = 0; i < port.ComponentList.Count; i++)
                    {
                        BaseComp bc = port.cc.ComponentList[i];
                        DGV[count, i] = new DataGridViewCompCell(bc);
                    }
            }
            DGV_UpdateValue();
        }

        private void DGV_UpdateValue()
        {
            int column = 0;
            foreach (var port in ports)
            {
                column++;
                for (int row = 0; row < port.ComponentList.Count; row++)
                {
                    if (DGV[column, row] is DataGridViewCompCell compcell)
                    {
                        compcell.ValueUpdate(CurrentState);
                    }
                }
            }
        }

        private void RB_CheckedChanged(object sender, EventArgs e)
        {
            if (RBMoleFraction.Checked)
                CurrentState = eDisplayState.Mole;
            if (RBMassFraction.Checked)
                CurrentState = eDisplayState.Mass;
            if (RBStdLiqVolume.Checked)
                CurrentState = eDisplayState.Vol;

            DGV_UpdateValue();
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SourceEnum source;

            if (DGV[e.ColumnIndex, e.RowIndex] is DataGridViewCompCell cell)
            {
                if (cell.Value != null)
                    source = cell.origin;
                else
                    source = SourceEnum.Empty;

                if (cell != null && cell.comp != null)
                {
                    if (source != SourceEnum.UnitOpCalcResult)
                    {
                        if (source == SourceEnum.Empty)
                            e.CellStyle.ForeColor = Color.Red;
                        if (source == SourceEnum.Input)
                            e.CellStyle.ForeColor = Color.Blue;
                        if (source == SourceEnum.Transferred)
                            e.CellStyle.ForeColor = Color.Black;
                    }
                    else { e.CellStyle.ForeColor = Color.Black; }
                }
            }
        }
    }
}