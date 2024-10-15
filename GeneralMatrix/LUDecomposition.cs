using System;
using System.Runtime.Serialization;

namespace DotNetMatrix
{
    /// <summary>LU Decomposition.
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    /// unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    /// and a permutation vector piv of length m so that A(piv,:) = L*U.
    /// <code> If m < n, then L is m-by-m and U is m-by-n. </code>
    /// The LU decompostion with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    /// LU decomposition is in the solution of square systems of simultaneous
    /// linear equations.  This will fail if IsNonSingular() return  s false.
    /// </summary>

    [Serializable]
    public class LUDecomposition : System.Runtime.Serialization.ISerializable
    {
        #region class  variables

        /// <summary>Array for public  storage of decomposition.
        /// @serial public  array storage.
        /// </summary>
        private double[][] LU;

        /// <summary>Row and column dimensions, and pivot sign.
        /// @serial column dimension.
        /// @serial row dimension.
        /// @serial pivot sign.
        /// </summary>
        private int m, n, pivsign;

        /// <summary>public  storage of pivot vector.
        /// @serial pivot vector.
        /// </summary>
        private int[] piv;

        #endregion class  variables

        #region Constructor

        /// <summary>LU Decomposition</summary>
        /// <param name="A">  Rectangular matrix
        /// </param>
        /// <return  s>     Structure to access L, U and piv.
        /// </return  s>

        public LUDecomposition(GeneralMatrix A)
        {
            // Use a "left-looking", dot-product, Crout/Doolittle algorithm.

            LU = A.ArrayCopy;
            m = A.RowDimension;
            n = A.ColumnDimension;
            piv = new int[m];
            for (int i = 0; i < m; i++)
            {
                piv[i] = i;
            }
            pivsign = 1;
            double[] LUrowi;
            double[] LUcolj = new double[m];

            // Outer loop.

            for (int j = 0; j < n; j++)
            {
                // Make a copy of the j-th column to localize references.

                for (int i = 0; i < m; i++)
                {
                    LUcolj[i] = LU[i][j];
                }

                // Apply previous transformations.

                for (int i = 0; i < m; i++)
                {
                    LUrowi = LU[i];

                    // Most of the time is spent in the following dot product.

                    int kmax = System.Math.Min(i, j);
                    double s = 0.0;
                    for (int k = 0; k < kmax; k++)
                    {
                        s += LUrowi[k] * LUcolj[k];
                    }

                    LUrowi[j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.

                int p = j;
                for (int i = j + 1; i < m; i++)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                    {
                        p = i;
                    }
                }
                if (p != j)
                {
                    for (int k = 0; k < n; k++)
                    {
                        double t = LU[p][k]; LU[p][k] = LU[j][k]; LU[j][k] = t;
                    }
                    int k2 = piv[p]; piv[p] = piv[j]; piv[j] = k2;
                    pivsign = -pivsign;
                }

                // Compute multipliers.

                if (j < m & LU[j][j] != 0.0)
                {
                    for (int i = j + 1; i < m; i++)
                    {
                        LU[i][j] /= LU[j][j];
                    }
                }
            }
        }

        #endregion Constructor

        #region public  Properties

        /// <summary>Is the matrix nonsingular?</summary>
        /// <return  s>     true if U, and hence A, is nonsingular.
        /// </return  s>
        public virtual bool IsNonSingular
        {
            get
            {
                for (int j = 0; j < n; j++)
                {
                    if (LU[j][j] == 0)
                        return false;
                }
                return true;
            }
        }

        /// <summary>return   lower triangular factor</summary>
        /// <return  s>     L
        /// </return  s>
        public virtual GeneralMatrix L
        {
            get
            {
                GeneralMatrix X = new GeneralMatrix(m, n);
                double[][] L = X.Array;
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i > j)
                        {
                            L[i][j] = LU[i][j];
                        }
                        else if (i == j)
                        {
                            L[i][j] = 1.0;
                        }
                        else
                        {
                            L[i][j] = 0.0;
                        }
                    }
                }
                return X;
            }
        }

        /// <summary>return   upper triangular factor</summary>
        /// <return  s>     U
        /// </return  s>
        public virtual GeneralMatrix U
        {
            get
            {
                GeneralMatrix X = new GeneralMatrix(n, n);
                double[][] U = X.Array;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i <= j)
                        {
                            U[i][j] = LU[i][j];
                        }
                        else
                        {
                            U[i][j] = 0.0;
                        }
                    }
                }
                return X;
            }
        }

        /// <summary>return   pivot permutation vector</summary>
        /// <return  s>     piv
        /// </return  s>
        public virtual int[] Pivot
        {
            get
            {
                int[] p = new int[m];
                for (int i = 0; i < m; i++)
                {
                    p[i] = piv[i];
                }
                return p;
            }
        }

        /// <summary>return   pivot permutation vector as a one-dimensional double  array</summary>
        /// <return  s>     (double ) piv
        /// </return  s>
        public virtual double[] doublePivot
        {
            get
            {
                double[] vals = new double[m];
                for (int i = 0; i < m; i++)
                {
                    vals[i] = (double)piv[i];
                }
                return vals;
            }
        }

        #endregion public  Properties

        #region public  Methods

        /// <summary>Determinant</summary>
        /// <return  s>     det(A)
        /// </return  s>
        /// <exception cref="System.ArgumentException">  Matrix must be square
        /// </exception>

        public virtual double Determinant()
        {
            if (m != n)
            {
                throw new System.ArgumentException("Matrix must be square.");
            }
            double d = (double)pivsign;
            for (int j = 0; j < n; j++)
            {
                d *= LU[j][j];
            }
            return d;
        }

        /// <summary>Solve A*X = B</summary>
        /// <param name="B">  A Matrix with as many rows as A and any number of columns.
        /// </param>
        /// <return  s>     X so that L*U*X = B(piv,:)
        /// </return  s>
        /// <exception cref="System.ArgumentException"> Matrix row dimensions must agree.
        /// </exception>
        /// <exception cref="System.SystemException"> Matrix is singular.
        /// </exception>

        public virtual GeneralMatrix Solve(GeneralMatrix B)
        {
            if (B.RowDimension != m)
            {
                throw new System.ArgumentException("Matrix row dimensions must agree.");
            }
            if (!this.IsNonSingular)
            {
                throw new System.SystemException("Matrix is singular.");
                //global::System.Windows.Forms.MessageBox.Show("Matrix is Singular");
                //return   B;
            }

            // Copy right hand side with pivoting
            int nx = B.ColumnDimension;
            GeneralMatrix Xmat = B.GetMatrix(piv, 0, nx - 1);
            double[][] X = Xmat.Array;

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i][j] -= X[k][j] * LU[i][k];
                    }
                }
            }
            // Solve U*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                {
                    X[k][j] /= LU[k][k];
                }
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i][j] -= X[k][j] * LU[i][k];
                    }
                }
            }
            return Xmat;
        }

        #endregion public  Methods

        // A method called when serializing this class .
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}