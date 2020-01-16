using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.Exocortex.DSP
{
	internal struct ComplexF : IComparable, ICloneable
	{
		public float Re;

		public float Im;

		public static ComplexF Zero => new ComplexF(0f, 0f);

		public static ComplexF I => new ComplexF(0f, 1f);

		public static ComplexF MaxValue => new ComplexF(float.MaxValue, float.MaxValue);

		public static ComplexF MinValue => new ComplexF(float.MinValue, float.MinValue);

		public ComplexF(float real, float imaginary)
		{
			Re = real;
			Im = imaginary;
		}

		public ComplexF(ComplexF c)
		{
			Re = c.Re;
			Im = c.Im;
		}

		public static ComplexF FromRealImaginary(float real, float imaginary)
		{
			ComplexF result = default(ComplexF);
			result.Re = real;
			result.Im = imaginary;
			return result;
		}

		public static ComplexF FromModulusArgument(float modulus, float argument)
		{
			ComplexF result = default(ComplexF);
			result.Re = (float)((double)modulus * Math.Cos(argument));
			result.Im = (float)((double)modulus * Math.Sin(argument));
			return result;
		}

		object ICloneable.Clone()
		{
			return new ComplexF(this);
		}

		public ComplexF Clone()
		{
			return new ComplexF(this);
		}

		public float GetModulus()
		{
			float re = Re;
			float im = Im;
			return (float)Math.Sqrt(re * re + im * im);
		}

		public float GetModulusSquared()
		{
			float re = Re;
			float im = Im;
			return re * re + im * im;
		}

		public float GetArgument()
		{
			return (float)Math.Atan2(Im, Re);
		}

		public ComplexF GetConjugate()
		{
			return FromRealImaginary(Re, 0f - Im);
		}

		public void Normalize()
		{
			double num = GetModulus();
			if (num == 0.0)
			{
				throw new DivideByZeroException("Can not normalize a complex number that is zero.");
			}
			Re = (float)((double)Re / num);
			Im = (float)((double)Im / num);
		}

		public static explicit operator ComplexF(Complex c)
		{
			ComplexF result = default(ComplexF);
			result.Re = (float)c.Re;
			result.Im = (float)c.Im;
			return result;
		}

		public static explicit operator ComplexF(float f)
		{
			ComplexF result = default(ComplexF);
			result.Re = f;
			result.Im = 0f;
			return result;
		}

		public static explicit operator float(ComplexF c)
		{
			return c.Re;
		}

		public static bool operator ==(ComplexF a, ComplexF b)
		{
			if (a.Re == b.Re)
			{
				return a.Im == b.Im;
			}
			return false;
		}

		public static bool operator !=(ComplexF a, ComplexF b)
		{
			if (a.Re == b.Re)
			{
				return a.Im != b.Im;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return Re.GetHashCode() ^ Im.GetHashCode();
		}

		public override bool Equals(object o)
		{
			if (o is ComplexF)
			{
				ComplexF b = (ComplexF)o;
				return this == b;
			}
			return false;
		}

		public int CompareTo(object o)
		{
			if (o == null)
			{
				return 1;
			}
			if (o is ComplexF)
			{
				return GetModulus().CompareTo(((ComplexF)o).GetModulus());
			}
			if (o is float)
			{
				return GetModulus().CompareTo((float)o);
			}
			if (o is Complex)
			{
				return GetModulus().CompareTo(((Complex)o).GetModulus());
			}
			if (o is double)
			{
				return GetModulus().CompareTo((double)o);
			}
			throw new ArgumentException();
		}

		public static ComplexF operator +(ComplexF a)
		{
			return a;
		}

		public static ComplexF operator -(ComplexF a)
		{
			a.Re = 0f - a.Re;
			a.Im = 0f - a.Im;
			return a;
		}

		public static ComplexF operator +(ComplexF a, float f)
		{
			a.Re += f;
			return a;
		}

		public static ComplexF operator +(float f, ComplexF a)
		{
			a.Re += f;
			return a;
		}

		public static ComplexF operator +(ComplexF a, ComplexF b)
		{
			a.Re += b.Re;
			a.Im += b.Im;
			return a;
		}

		public static ComplexF operator -(ComplexF a, float f)
		{
			a.Re -= f;
			return a;
		}

		public static ComplexF operator -(float f, ComplexF a)
		{
			a.Re = f - a.Re;
			a.Im = 0f - a.Im;
			return a;
		}

		public static ComplexF operator -(ComplexF a, ComplexF b)
		{
			a.Re -= b.Re;
			a.Im -= b.Im;
			return a;
		}

		public static ComplexF operator *(ComplexF a, float f)
		{
			a.Re *= f;
			a.Im *= f;
			return a;
		}

		public static ComplexF operator *(float f, ComplexF a)
		{
			a.Re *= f;
			a.Im *= f;
			return a;
		}

		public static ComplexF operator *(ComplexF a, ComplexF b)
		{
			double num = a.Re;
			double num2 = a.Im;
			double num3 = b.Re;
			double num4 = b.Im;
			a.Re = (float)(num * num3 - num2 * num4);
			a.Im = (float)(num * num4 + num2 * num3);
			return a;
		}

		public static ComplexF operator /(ComplexF a, float f)
		{
			if (f == 0f)
			{
				throw new DivideByZeroException();
			}
			a.Re /= f;
			a.Im /= f;
			return a;
		}

		public static ComplexF operator /(ComplexF a, ComplexF b)
		{
			double num = a.Re;
			double num2 = a.Im;
			double num3 = b.Re;
			double num4 = b.Im;
			double num5 = num3 * num3 + num4 * num4;
			if (num5 == 0.0)
			{
				throw new DivideByZeroException();
			}
			a.Re = (float)((num * num3 + num2 * num4) / num5);
			a.Im = (float)((num2 * num3 - num * num4) / num5);
			return a;
		}

		public static ComplexF Parse(string s)
		{
			throw new NotImplementedException("ComplexF ComplexF.Parse( string s ) is not implemented.");
		}

		public override string ToString()
		{
			return $"( {Re}, {Im}i )";
		}

		public static bool IsEqual(ComplexF a, ComplexF b, float tolerance)
		{
			if (Math.Abs(a.Re - b.Re) < tolerance)
			{
				return Math.Abs(a.Im - b.Im) < tolerance;
			}
			return false;
		}
	}
}
