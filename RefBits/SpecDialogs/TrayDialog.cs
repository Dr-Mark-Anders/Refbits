using ModelEngine;
using System;
using System.Windows.Forms;

namespace Units
{
    public partial class TrayDialog : Form
    {
        private DrawColumn drawcolumn;
        private Specification spec;
        private TraySection section;

        internal TrayDialog(DrawColumn drawColumn, Specification spec)
        {
            this.drawcolumn = drawColumn;
            section = drawcolumn.Column.TraySections[spec.engineSectionGuid];

            this.spec = spec;
            InitializeComponent();

            cbStage.Items.Clear();
            cbSpec.Items.Clear();

            cbSpec.Items.AddRange(Enum.GetNames(typeof(eSpecType)));
            cbSpec.SelectedItem = spec.graphicSpecType.ToString();

            for (int i = 0; i < drawcolumn.Column.NoSections; i++)
                cbSection.Items.Add(drawcolumn.Column[i].Name);

            DrawColumnTraySection dcts = drawcolumn.DrawTraySections[spec.drawObjectGuid];

            if (dcts != null)
                cbSection.SelectedItem = dcts.Name;

            if (section != null)
            {
                for (int i = 0; i < section.Trays.Count; i++)
                    cbStage.Items.Add(i);

                cbStage.SelectedIndex = section.IndexOf(spec.engineStageGuid);
            }
        }

        private void cbSection_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string sectionName = (string)cbSection.SelectedItem;
            DrawColumnTraySection dsection = drawcolumn.DrawTraySections[sectionName];
            if (dsection != null)
            {
                spec.drawObjectGuid = dsection.Guid;
                section = drawcolumn.Column[dsection.Name];
                spec.engineSectionGuid = section.Guid;
            }

            cbStage.Items.Clear();
            for (int i = 0; i < dsection.DrawTrays.Count; i++)
                cbStage.Items.Add(i);
        }

        private void cbStage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int StageNo = cbStage.SelectedIndex;
            if (section != null && StageNo < section.Trays.Count)
                spec.engineStageGuid = section.Trays[StageNo].Guid;
        }

        private void cbSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse(cbSpec.SelectedItem.ToString(), out eSpecType espec))
                spec.graphicSpecType = espec;
            spec.propID = Specification.Spec_ePropID(espec);
            spec.ChangeUnits();
        }

        private void TrayDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            int.TryParse(cbStage.Text, out int StageNo);
            if (StageNo >= 0 && section != null)
            {
                spec.engineStageGuid = section.Trays[StageNo].Guid;
                spec.engineSectionGuid = section.Guid;
                spec.engineObjectguid = drawcolumn.Column.Guid;
            }
            spec.ChangeUnits();
        }
    }
}