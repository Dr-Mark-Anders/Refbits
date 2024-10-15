namespace ModelEngine
{
    public interface ISolveable
    {
        double[][] CalculateJacobian(double[] x, double[] RHS);

        double[] CalculateRHS(double[] x);

        double[] XInitial
        {
            get;
        }

        double[] XFinal
        {
            get;
            set;
        }
    }
}