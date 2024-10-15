using System;
using System.Runtime.Serialization;

namespace DotNetMatrix
{
    ///<summary>CholeskyDecomposition.
    ///Forasymmetric,positivedefinitematrixA,theCholeskydecomposition
    ///isanlowertriangularmatrixLsothatA=L*L'.
    ///Ifthematrixisnotsymmetricorpositivedefinite,theconstructor
    ///return  sapartialdecompositionandsetsanpublic flagthatmay
    ///bequeriedbytheisSPD()method.
    ///</summary>

    [Serializable]
    public class CholeskyDecomposition : System.Runtime.Serialization.ISerializable
    {
        #region class variables

        ///<summary>Arrayforpublic storageofdecomposition.
        ///@serialpublic arraystorage.
        ///</summary>
        private double[][] L;

        ///<summary>Rowandcolumndimension(squarematrix).
        ///@serialmatrixdimension.
        ///</summary>
        private int n;

        ///<summary>Symmetricandpositivedefiniteflag.
        ///@serialissymmetricandpositivedefiniteflag.
        ///</summary>
        private bool isspd;

        #endregion class variables

        #region Constructor

        ///<summary>Choleskyalgorithmforsymmetricandpositivedefinitematrix.</summary>
        ///<paramname="Arg">Square,symmetricmatrix.
        ///</param>
        ///<return  s>StructuretoaccessLandisspdflag.
        ///</return  s>

        public CholeskyDecomposition(GeneralMatrix Arg)
        {
            //Initialize.
            double[][] A = Arg.Array;
            n = Arg.RowDimension;
            L = new double[n][];
            for (int i = 0; i < n; i++)
            {
                L[i] = new double[n];
            }
            isspd = (Arg.ColumnDimension == n);
            //Mainloop.
            for (int j = 0; j < n; j++)
            {
                double[] Lrowj = L[j];
                double d = 0.0;
                for (int k = 0; k < j; k++)
                {
                    double[] Lrowk = L[k];
                    double s = 0.0;
                    for (int i = 0; i < k; i++)
                    {
                        s += Lrowk[i] * Lrowj[i];
                    }
                    Lrowj[k] = s = (A[j][k] - s) / L[k][k];
                    d = d + s * s;
                    isspd = isspd & (A[k][j] == A[j][k]);
                }
                d = A[j][j] - d;
                isspd = isspd & (d > 0.0);
                L[j][j] = System.Math.Sqrt(System.Math.Max(d, 0.0));
                for (int k = j + 1; k < n; k++)
                {
                    L[j][k] = 0.0;
                }
            }
        }

        #endregion Constructor

        #region public Properties

        ///<summary>Isthematrixsymmetricandpositivedefinite?</summary>
        ///<return  s>trueifAissymmetricandpositivedefinite.
        ///</return  s>
        public virtual bool SPD
        {
            get
            {
                return isspd;
            }
        }

        #endregion public Properties

        #region public Methods

        ///<summary>return  triangularfactor.</summary>
        ///<return  s>L
        ///</return  s>

        public virtual GeneralMatrix GetL()
        {
            return new GeneralMatrix(L, n, n);
        }

        ///<summary>SolveA*X=B</summary>
        ///<paramname="B">AMatrixwithasmanyrowsasAandanynumberofcolumns.
        ///</param>
        ///<return  s>XsothatL*L'*X=B
        ///</return  s>
        ///<exceptioncref="System.ArgumentException">Matrixrowdimensionsmustagree.
        ///</exception>
        ///<exceptioncref="System.SystemException">Matrixisnotsymmetricpositivedefinite.
        ///</exception>

        public virtual GeneralMatrix Solve(GeneralMatrix B)
        {
            if (B.RowDimension != n)
            {
                throw new System.ArgumentException("Matrixrowdimensionsmustagree.");
            }
            if (!isspd)
            {
                throw new System.SystemException("Matrixisnotsymmetricpositivedefinite.");
            }

            //Copyrighthandside.
            double[][] X = B.ArrayCopy;
            int nx = B.ColumnDimension;

            //SolveL*Y=B;
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i][j] -= X[k][j] * L[i][k];
                    }
                }
                for (int j = 0; j < nx; j++)
                {
                    X[k][j] /= L[k][k];
                }
            }

            //SolveL'*X=Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                {
                    X[k][j] /= L[k][k];
                }
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i][j] -= X[k][j] * L[k][i];
                    }
                }
            }
            return new GeneralMatrix(X, n, nx);
        }

        #endregion public Methods

        //Amethodcalledwhenserializingthisclass .
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}