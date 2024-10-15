namespace ModelEngine
{
    public interface IComp
    {
        string Name
        {
            get;
            set;
        }

        bool IsPure
        {
            get;
            set;
        }

        double MoleFracVap
        {
            get;
            set;
        }

        string SourceCrude
        {
            get;
            set;
        }

        double MW
        {
            get;
            set;
        }

        object Clone();
    }

    public interface IPCComp
    {
        double PCProps
        {
            get;
            set;
        }
    }
}