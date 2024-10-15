using DialogControls;
using EngineThermo;
using System;

namespace NaphthaReformerSI
{
    public partial class FeedDistillation : Distillations
    {
        private DistPoints feeddistillation;

        public FeedDistillation(DistPoints feeddistillation, Tuple<double, double, double, double> PNAO):base(feeddistillation, PNAO)
        {
            this.feeddistillation = feeddistillation;
            InitializeComponent();
        }

        public FeedDistillation() : base(null, null)
        {
            InitializeComponent();
        }

        public FeedDistillation(DistPoints feeddistillation)
        {
            this.feeddistillation = feeddistillation;
            InitializeComponent();
        }
    }
}
