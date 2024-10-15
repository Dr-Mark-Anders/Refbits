using ModelEngine;

namespace ModelEngine
{
    public partial class LLESEP
    {
        public SideStreamCollection LiquidSideStreams
        {
            get { return liquidSideDraws; }
            set { liquidSideDraws = value; }
        }

        public SideStreamCollection VapourSideStreams
        {
            get { return vapourSideDraws; }
            set { vapourSideDraws = value; }
        }

        public SideStreamCollection SideStreamsAll()
        {
            SideStreamCollection ssc = new SideStreamCollection();
            ssc.AddRange(liquidSideDraws);
            ssc.AddRange(vapourSideDraws);
            return ssc;
        }

        public TraySection MainSectionStages
        {
            get
            {
                return traySections[0];
            }
            set
            {
                traySections[0] = value;
            }
        }

        public int NoComps
        {
            get
            {
                if (Components != null)
                    return Components.Count;
                else
                    return 0;
            }
        }

        public int TotNoStages
        {
            get
            {
                if (traySections != null)
                    return traySections.TotNoTrays();
                else
                    return 0;
            }
        }

        public int NoSections
        {
            get
            {
                if (traySections != null)
                    return traySections.Count;
                else
                    return 0;
            }
        }

        public int JacobianSize
        {
            get
            {
                return TotNoStages * 2;
            }
        }

        public int NonJacobianSpecsCount
        {
            get
            {
                int count = 0;
                count += specs.GetActiveSpecs().Count();
                count += pumpArounds.Count;
                count += liquidSideDraws.Count;
                return count;
            }
        }

        public int RequiredSpecsCount
        {
            get
            {
                int count = 0;
                count += pumpArounds.Count * 2;
                count += liquidSideDraws.Count;
                count += vapourSideDraws.Count;
                count += connectingDraws.Count;
                count += this.traySections.UnheatBalancedTraysCount;
                return count;
            }
        }

        public bool Totalreflux
        {
            get
            {
                return totalreflux;
            }

            set
            {
                totalreflux = value;
            }
        }

        public double Subcoolduty
        {
            get
            {
                return subcoolduty;
            }
            set
            {
                subcoolduty = value;
            }
        }

        public double SubcoolDT
        {
            get
            {
                return subcoolDT;
            }
            set
            {
                subcoolDT = value;
            }
        }

        public double FeedRatio
        {
            get
            {
                if (feedRatio == 0)
                    return 1;
                return feedRatio;
            }
            set
            {
                feedRatio = value;
            }
        }

        public double MaxTemperatureLoopCount
        {
            get
            {
                return maxInnerIterations;
            }
            set
            {
                maxInnerIterations = value;
            }
        }

        public double MaxOuterIterations
        {
            get
            {
                return maxOuterIterations;
            }

            set
            {
                maxOuterIterations = value;
            }
        }

        public bool ThomasAlgorithm
        {
            get
            {
                return thomasAlgorithm;
            }
            set
            {
                thomasAlgorithm = value;
            }
        }

        public double SpecificationTolerance
        {
            get
            {
                return innertolerance;
            }
            set
            {
                innertolerance = value;
            }
        }

        public double TrayTempTolerance
        {
            get
            {
                return outertolerance;
            }
            set
            {
                outertolerance = value;
            }
        }

        public bool ResetInitialTemps
        {
            get
            {
                return resetInitialTemps;
            }
            set
            {
                resetInitialTemps = value;
            }
        }

        public bool SplitMainFeed
        {
            get
            {
                return splitMainFeed;
            }
            set
            {
                splitMainFeed = value;
            }
        }

        public bool LineariseEnthalpies
        {
            get
            {
                return lineariseEnthalpies;
            }

            set
            {
                lineariseEnthalpies = value;
            }
        }

        public double Waterdraw
        {
            get
            {
                return waterdraw;
            }
            set
            {
                waterdraw = value;
            }
        }

        public double ChangeIndicator
        {
            get
            {
                return changeIndicator;
            }
            set
            {
                changeIndicator = value;
            }
        }

        public bool SolutionConverged
        {
            get
            {
                return solutionConverged;
            }
            set
            {
                solutionConverged = value;
            }
        }
    }
}