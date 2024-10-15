namespace ModelEngine.ThermodynamicMethods.UNIFAC
{
    public class BaseUnifacData
    {
        public PrimaryDataCollection GroupList = new PrimaryDataCollection();
        public double[,] GROUP_INTERACT_PARAMS;
    }
}