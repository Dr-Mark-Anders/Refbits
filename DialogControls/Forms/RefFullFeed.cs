﻿using ModelEngine;
using System;
using System.Windows.Forms;

namespace DialogControls
{
    public partial class RefFullFeed : Form
    {
        private readonly RefomerKineticLumpList comps;

        public RefFullFeed(RefomerKineticLumpList data)
        {
            comps = data;
            InitializeComponent();
        }

        private void FullGC_FormClosing(object sender, FormClosingEventArgs e)
        {
            comps.basis = GCBasis.Value;
            comps.SetFractions(pg1.GetFractions());
        }

        private void FullGC_Load(object sender, EventArgs e)
        {
            pg1.AddRange(comps);
            FormControls.GCCompGrid.UpdateProps();

            GCBasis.Value = comps.basis;
        }
    }
}