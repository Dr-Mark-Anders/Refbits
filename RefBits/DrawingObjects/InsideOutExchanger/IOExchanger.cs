using ModelEngine;
using System;

namespace Units.InsideOutExchanger
{
    public enum Temperaturemode
    { HSICSI, HSICSO, HSOCSI, HSOCSO, HSICSFULL, HSOCSFULL, CSIHSFULL, CSOHSFULL };

    public enum specmode
    { CSI, CSO, HSI, HSO, HSICSO, HSOCSI, HSOCSO };

    public enum constraints
    { CSIN, CSOUT, HSIN, HSOUT, CSDT, HSDT, DUTY, UA, COLDFFLOW, HOTFLOW, NOTSET };

    public class IOExchanger // at least one Temperature  must be provided for each stream.// If duty 0 or negative then fail.
    {
        private double ua, lmtd, dutyperkg, hsin, hsout, csin, csout;
        private bool counter, csinprovided, hsinprovided, hsoutprovided, csideoutprovided;
        private double dutyperkghotfeed = 60;
        private double dutyperkgcoldfeed = 20;
        private double HotSideDT = 0, ColdSideDT = 0;
        private double CPcold, CPhot;
        private double uAPerkg;

        private constraints constraint1 = constraints.NOTSET;
        private constraints constraint2 = constraints.NOTSET;

        private double constraintvalue1 = 0;
        private double constraintvalue2 = 0;

        private double[] BaseValue = new double[11];
        private double[] BaseResults1 = new double[11];
        private double[] BaseResults2 = new double[11];

        private double[,] jacobian = new double[2, 2];
        private double[] constrainederr = new double[2];

        private MatrixAlgebra.IMatrix res;

        public double UAPerkg
        {
            get { return uAPerkg; }
            set { uAPerkg = value; }
        }

        public double Duty
        {
            get { return dutyperkg; }
            set { dutyperkg = value; }
        }

        public IOExchanger(double CPCold, double CPHot, UOMProperty hsin, UOMProperty hsout, UOMProperty csin, UOMProperty csout, bool counter)
        {
            this.hsin = hsin;
            this.hsout = hsout;
            this.csin = csin;
            this.csout = csout;
            this.counter = counter;

            this.CPcold = CPCold;
            this.CPhot = CPHot;

            csinprovided = csin.Source == SourceEnum.Transferred;
            csideoutprovided = csout.Source == SourceEnum.Transferred;
            hsinprovided = hsin.Source == SourceEnum.Transferred;
            hsoutprovided = hsout.Source == SourceEnum.Transferred;
        }

        public void Init()
        {
            Run(BaseValue);
        }

        public void Run(double[] arr)
        {
            HotSideDT = dutyperkghotfeed / CPhot;
            ColdSideDT = dutyperkgcoldfeed / CPcold;

            if (csinprovided)
                csout = csin + ColdSideDT;
            else if (csideoutprovided)
                csin = csout - ColdSideDT;

            if (hsinprovided)
                hsout = hsin - HotSideDT;
            else if (hsoutprovided)
                hsin = hsout + HotSideDT;

            lmtd = LMTD(hsin, hsout, csin, csout, true);
            dutyperkg = CPcold * ColdSideDT;
            uAPerkg = dutyperkg / lmtd;

            updatearray(arr);
        }

        public void setconstraints(constraints c1, double val1, constraints c2, double val2)
        {
            constraint1 = c1;
            constraint2 = c2;
            constraintvalue1 = val1;
            constraintvalue2 = val2;
        }

        public void updatearray(double[] arr)
        {
            arr[(int)constraints.CSOUT] = csout;
            arr[(int)constraints.HSOUT] = hsout;
            arr[(int)constraints.CSIN] = csin;
            arr[(int)constraints.HSIN] = hsin;

            arr[(int)constraints.CSDT] = ColdSideDT;
            arr[(int)constraints.HSDT] = HotSideDT;

            arr[(int)constraints.UA] = uAPerkg;
            arr[(int)constraints.DUTY] = dutyperkg;
        }

        public void CalcGradients()
        {
            dutyperkghotfeed++;
            Run(BaseResults1);
            dutyperkghotfeed--;
            dutyperkgcoldfeed++;
            Run(BaseResults2);
            dutyperkgcoldfeed--;
        }

        public void createjacobian()
        {
            double[] arr;

            for (int n = 0; n < 2; n++)
            {
                if (n == 0)
                    arr = BaseResults1;
                else
                    arr = BaseResults2;

                for (int m = 0; m < 2; m++)
                {
                    if (m == 0)
                        jacobian[m, n] = (arr[(int)constraint1] - BaseValue[(int)constraint1]) / 1;
                    else
                        jacobian[m, n] = (arr[(int)constraint2] - BaseValue[(int)constraint2]) / 1;
                }
            }
        }

        private MatrixAlgebra.IMatrix cerr;

        public void calcerrors()
        {
            constrainederr[0] = BaseValue[(int)constraint1] - constraintvalue1;
            constrainederr[1] = BaseValue[(int)constraint2] - constraintvalue2;
            cerr = new MatrixAlgebra.Matrix(constrainederr);
        }

        public bool IsSquare()
        {
            //cerr.GetSingularValueDecomposition;
            return true;
        }

        public double geterror()
        {
            double err = 0;
            for (int n = 0; n < 2; n++)
            {
                err += cerr[n, 0];
            }
            return err;
        }

        public bool InverseJacobian()
        {
            MatrixAlgebra.IMatrix mat = new MatrixAlgebra.Matrix(jacobian);
            MatrixAlgebra.IMatrix inv;

            try
            {
                inv = mat.Inverse;
                res = inv.Multiply(cerr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateDutyperKG()
        {
            if (res != null)
            {
                dutyperkghotfeed -= res[0, 0];
                dutyperkgcoldfeed -= res[1, 0];
            }
        }

        public double Csout
        {
            get { return csout; }
            set { csout = value; }
        }

        public double Csin
        {
            get { return csin; }
            set { csin = value; }
        }

        public double Hsout
        {
            get { return hsout; }
            set { hsout = value; }
        }

        public double Lmtd
        {
            get { return lmtd; }
            set { lmtd = value; }
        }

        public double UA
        {
            get { return ua; }
            set { ua = value; }
        }

        public double LMTD(double HSin, double HSOut, double CSin, double Csout, bool countercurrent)
        {
            double DT1, DT2, lmtd;

            if (countercurrent)
            {
                DT1 = hsin - csout;
                DT2 = hsout - csin;
            }
            else
            {
                DT1 = hsin - csin;
                DT2 = hsout - csout;
            }

            if (DT1 == DT2)
                return DT1;

            lmtd = (DT1 - DT2) / Math.Log(DT1 / DT2);

            return lmtd;
        }
    }
}