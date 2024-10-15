using DotNetMatrix;
using Extensions;
using MathNet.Numerics.LinearAlgebra;
using ModelEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SolverType
{ NewtonRaphson, Broyden, Secant }

namespace RefBitsEquationSolver
{
    public class Bisection
    {
        private Func<double, double> func;

        public Bisection(Func<double, double> function)
        {
            func = function;
        }

        public double Solve(double Start, double End, int steps = 10)
        {
            double Err2;
            double V1 = double.NaN, V2;
            double Err1;
            int count = 0;
            double stepsize;

            do
            {
                Err1 = func(Start);
                stepsize = Math.Abs(End - Start) / steps;

                for (int i = 0; i < steps; i++)
                {
                    V1 = stepsize * i + Start;
                    V2 = stepsize * (i + 1) + Start;

                    Err1 = func(V1);
                    Err2 = func(V2);

                    if ((Err1 < 0 && Err2 > 0) || (Err1 > 0 && Err2 < 0)) // Then  ' solution crossed
                    {
                        Start = V1;
                        End = V2;
                        break;
                    }
                }

                count++;
            } while (Math.Abs(Err1) > 0.00001 && count < 10000 && Math.Abs(End - Start) > 1e-10);

            if (Math.Abs(Err1) > 0.00001)
                return double.NaN;
            else
                return V1;
        }
    }

    public class NonLinearSolver
    {
        private static double Maxdelta = 0.1;

        public static bool SolveEquations(ISolveable UO, SolverType solver)
        {
            double[] grads;
            double[] X = UO.XInitial;

            switch (solver)
            {
                case SolverType.NewtonRaphson:
                    for (int i = 0; i < 100; i++)
                    {
                        double[] B = UO.CalculateRHS(X);
                        if (CheckConvergence(B))
                        {
                            UO.XFinal = X;
                            return true;
                        }

                        double[][] A = UO.CalculateJacobian(X, B);
                        CalcGradients(A, B, out grads);
                        AdjustEstimates(X, grads, Maxdelta);
                    }
                    if (UO is UnitOperation)
                        ((UnitOperation)UO).EraseNonFixedValues();
                    break;

                case SolverType.Broyden:
                    break;

                case SolverType.Secant:
                    break;
            }
            return false;
        }

        private static void AdjustEstimates(double[] x, double[] grads, double maxdelta)
        {
            double delta;
            for (int i = 0; i < x.Length; i++)
            {
                delta = grads[i] * 0.1;
                if (delta > maxdelta)
                    delta = maxdelta;
                x[i] -= delta;
            }
        }

        private static bool CheckConvergence(double[] b)
        {
            return b.SumSQR() < 1e-10;
        }

        public static bool CalcGradients(double[][] Jacobian, double[] errors, out double[] grads)
        {
            enumInverseMethod method = enumInverseMethod.MathNet;
            grads = null;

            if (errors != null)
            {
                switch (method)
                {
                    case enumInverseMethod.GeneralMatrix:
                        GeneralMatrix A = new GeneralMatrix(Jacobian);
                        GeneralMatrix B = new GeneralMatrix(errors, errors.Length);
                        A = A.Inverse();

                        if (A != null)
                            A = A.Multiply(B);
                        else
                            return false;

                        grads = A.Transpose().Array[0];

                        var Aa = Matrix<double>.Build.DenseOfRowArrays(Jacobian);
                        var Bb = Vector<double>.Build.DenseOfArray(errors);
                        var D = Aa.Solve(Bb).ToArray();

                        return true;

                    case enumInverseMethod.Crouts:
                        double[][] A1 = MatrixInverse.MatrixInverseProgram.MatrixInverse(Jacobian);
                        double[][] B1 = new double[1][];
                        B1[0] = errors;

                        double[][] res = MatrixInverse.MatrixInverseProgram.MatrixProduct(Jacobian, B1);

                        grads = MatrixInverse.MatrixInverseProgram.Transpose(res)[0];

                        return true;

                    case enumInverseMethod.Alglib:
                        /*alglib.densesolverlsreport rep;
                        int info;
                        double[][] A = alglib.rmatrixsolvem(Jacobian, 1, Errors, 2, true, out info, out rep, out grads);
                        double[][] B = new double[1][];
                        B[0] = errors;

                        double[][] res = MatrixInverse.MatrixInverseProgram.MatrixProduct(Jacobian, B);

                        grads = MatrixInverse.MatrixInverseProgram.Transpose(res)[0];

                        return true;*/
                        break;

                    case enumInverseMethod.MathNet:
                        var A2 = Matrix<double>.Build.DenseOfRowArrays(Jacobian);
                        var B2 = Vector<double>.Build.DenseOfArray(errors);
                        try
                        {
                            grads = A2.Solve(B2).ToArray();
                        }
                        catch { return false; }
                        return true;
                }
            }
            return false;
        }
    }

    public class Variable
    {
        private StreamProperty property;

        public Variable(StreamProperty property)
        {
            this.property = property;
        }

        public double value
        {
            get
            {
                return property.BaseValue;
            }
            set
            {
                property.BaseValue = value;
            }
        }

        public void Perturb(double value)
        {
            property.BaseValue *= value;
        }
    }

    public class Variables : IList<Variable>
    {
        private List<Variable> list = new List<Variable>();

        public double[] ToArray()
        {
            double[] res = new double[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                res[i] = list[i].value;
            }

            return res;
        }

        public Variable this[int index] { get => ((IList<Variable>)list)[index]; set => ((IList<Variable>)list)[index] = value; }

        public int Count => ((ICollection<Variable>)list).Count;

        public bool IsReadOnly => ((ICollection<Variable>)list).IsReadOnly;

        public void Add(Variable item)
        {
            ((ICollection<Variable>)list).Add(item);
        }

        public void Clear()
        {
            ((ICollection<Variable>)list).Clear();
        }

        public bool Contains(Variable item)
        {
            return ((ICollection<Variable>)list).Contains(item);
        }

        public void CopyTo(Variable[] array, int arrayIndex)
        {
            ((ICollection<Variable>)list).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Variable> GetEnumerator()
        {
            return ((IEnumerable<Variable>)list).GetEnumerator();
        }

        public int IndexOf(Variable item)
        {
            return ((IList<Variable>)list).IndexOf(item);
        }

        public void Insert(int index, Variable item)
        {
            ((IList<Variable>)list).Insert(index, item);
        }

        public bool Remove(Variable item)
        {
            return ((ICollection<Variable>)list).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Variable>)list).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }

    public class Specification
    {
        private Port_Signal property;
        private SpecType spectype;

        public Specification(Port_Signal property, SpecType spectype)
        {
            this.property = property;
            this.spectype = spectype;
        }

        public SpecType SpecType
        {
            get
            {
                return spectype;
            }
        }

        public Port_Signal Value
        {
            get
            {
                return property;
            }
        }
    }

    public class Specifications : IList<Specification>
    {
        private List<Specification> list = new List<Specification>();

        public Specifications()
        {
        }

        public Specifications(List<Specification> lists)
        {
            this.list = lists;
        }

        public Specification this[int index] { get => ((IList<Specification>)list)[index]; set => ((IList<Specification>)list)[index] = value; }

        public int Count => ((ICollection<Specification>)list).Count;

        public bool IsReadOnly => ((ICollection<Specification>)list).IsReadOnly;

        public void Add(Specification item)
        {
            ((ICollection<Specification>)list).Add(item);
        }

        public void Add(Port_Energy item, SpecType spectype)
        {
            Port_Signal port = new Port_Signal(item);
            ((ICollection<Specification>)list).Add(new Specification(port, spectype));
        }

        public void Add(Port_Signal item, SpecType spectype)
        {
            ((ICollection<Specification>)list).Add(new Specification(item, spectype));
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Specification item)
        {
            return ((ICollection<Specification>)list).Contains(item);
        }

        public void CopyTo(Specification[] array, int arrayIndex)
        {
            ((ICollection<Specification>)list).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Specification> GetEnumerator()
        {
            return ((IEnumerable<Specification>)list).GetEnumerator();
        }

        public int IndexOf(Specification item)
        {
            return ((IList<Specification>)list).IndexOf(item);
        }

        public void Insert(int index, Specification item)
        {
            ((IList<Specification>)list).Insert(index, item);
        }

        public bool Remove(Specification item)
        {
            return ((ICollection<Specification>)list).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Specification>)list).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }

    public class FalsiMethod_Secant
    {
        private double f(double x)
        {
            return Math.Cos(x) - x * x * x;
        }

        /* s,t: endpoints of an interval where we search
           e: half of upper bound for relative error
           m: maximal number of iterations */

        private double FalsiMethod(double s, double t, double e, int m)
        {
            double r = 0, fr;
            int n, side = 0;
            /* starting values at endpoints of interval */
            double fs = f(s);
            double ft = f(t);

            for (n = 0; n < m; n++)
            {
                r = (fs * t - ft * s) / (fs - ft);
                if (Math.Abs(t - s) < e * Math.Abs(t + s)) break;
                fr = f(r);

                if (fr * ft > 0)
                {
                    /* fr and ft have same sign, copy r to t */
                    t = r; ft = fr;
                    if (side == -1) fs /= 2;
                    side = -1;
                }
                else if (fs * fr > 0)
                {
                    /* fr and fs have same sign, copy r to s */
                    s = r; fs = fr;
                    if (side == +1) ft /= 2;
                    side = +1;
                }
                else
                {
                    /* fr * f_ very small (looks like zero) */
                    break;
                }
            }
            return r;
        }
    }
}