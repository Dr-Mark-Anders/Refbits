using System;
using System.Collections.Generic;

namespace Extensions
{
    public struct Number<T> where T : IComparable<T>, IEquatable<T>
    {
        private T _Value;

        public Number(T value)
        {
            _Value = value;
        }

        #region Comparison

        public bool Equals(Number<T> other)
        {
            return _Value.Equals(other._Value);
        }

        public bool Equals(T other)
        {
            return _Value.Equals(other);
        }

        public int CompareTo(Number<T> other)
        {
            return _Value.CompareTo(other._Value);
        }

        public int CompareTo(T other)
        {
            return _Value.CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is T)
                return _Value.Equals((T)obj);
            if (obj is Number<T>)
                return _Value.Equals(((Number<T>)obj)._Value);
            return false;
        }

        public override int GetHashCode()
        {
            return (_Value == null) ? 0 : _Value.GetHashCode();
        }

        public static bool operator ==(Number<T> a, Number<T> b)
        {
            return a._Value.Equals(b._Value);
        }

        public static bool operator !=(Number<T> a, Number<T> b)
        {
            return !a._Value.Equals(b._Value);
        }

        public static bool operator <(Number<T> a, Number<T> b)
        {
            return a._Value.CompareTo(b._Value) < 0;
        }

        public static bool operator <=(Number<T> a, Number<T> b)
        {
            return a._Value.CompareTo(b._Value) <= 0;
        }

        public static bool operator >(Number<T> a, Number<T> b)
        {
            return a._Value.CompareTo(b._Value) > 0;
        }

        public static bool operator >=(Number<T> a, Number<T> b)
        {
            return a._Value.CompareTo(b._Value) >= 0;
        }

        public static Number<T> operator !(Number<T> a)
        {
            return new Number<T>(Calculator<T>.Negate(a._Value));
        }

        #endregion Comparison

        #region Arithmatic operations

        public static Number<T> operator +(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Add(a._Value, b._Value));
        }

        public static Number<T> operator -(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Subtract(a._Value, b._Value));
        }

        public static Number<T> operator *(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Multiply(a._Value, b._Value));
        }

        public static Number<T> operator /(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Divide(a._Value, b._Value));
        }

        public static Number<T> operator %(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Modulo(a._Value, b._Value));
        }

        public static Number<T> operator -(Number<T> a)
        {
            return new Number<T>(Calculator<T>.Negate(a._Value));
        }

        public static Number<T> operator +(Number<T> a)
        {
            return new Number<T>(Calculator<T>.Plus(a._Value));
        }

        static public Number<T> operator ++(Number<T> a)
        {
            return new Number<T>(Calculator<T>.Increment(a._Value));
        }

        public static Number<T> operator --(Number<T> a)
        {
            return new Number<T>(Calculator<T>.Decrement(a._Value));
        }

        #endregion Arithmatic operations

        #region Bitwise operations

        public static Number<T> operator <<(Number<T> a, int b)
        {
            return new Number<T>(Calculator<T>.LeftShift(a._Value, b));
        }

        public static Number<T> operator >>(Number<T> a, int b)
        {
            return new Number<T>(Calculator<T>.RightShift(a._Value, b));
        }

        public static Number<T> operator &(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.And(a._Value, b._Value));
        }

        static public Number<T> operator |(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Or(a._Value, b._Value));
        }

        public static Number<T> operator ^(Number<T> a, Number<T> b)
        {
            return new Number<T>(Calculator<T>.Xor(a._Value, b._Value));
        }

        public static Number<T> operator ~(Number<T> a)
        {
            return new Number<T>(Calculator<T>.OnesComplement(a._Value));
        }

        #endregion Bitwise operations

        #region Casts

        public static implicit operator Number<T>(T value)
        {
            return new Number<T>(value);
        }

        public static explicit operator T(Number<T> value)
        {
            return value._Value;
        }

        public static implicit operator double(Number<T> value)
        {
            return Convert.ToDouble(value._Value);
        }

        public static implicit operator int(Number<T> value)
        {
            return Convert.ToInt32(value._Value);
        }

        /*  static public implicit operator Number<T>(int value)
          {
              Number<T> n = new Number<T>();
              n._Value = (T)value;
              return n;
          }*/

        #endregion Casts

        #region Other members

        public override string ToString()
        {
            return (_Value == null) ? string.Empty : _Value.ToString();
        }

        #endregion Other members
    }

    public static partial class Ext
    {
        public static List<T> NumberList<T>(this T a) where T : struct
        {
            List<T> res = new List<T>();
            res.Add(a);
            return res;
        }
    }
}