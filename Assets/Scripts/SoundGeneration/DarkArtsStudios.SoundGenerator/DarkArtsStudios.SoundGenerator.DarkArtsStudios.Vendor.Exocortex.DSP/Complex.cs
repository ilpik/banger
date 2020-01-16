using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.Exocortex.DSP
{
	internal struct Complex : IComparable, ICloneable
	{
		public double Re;

		public double Im;

		public static Complex Zero => new Complex(0.0, 0.0);

		public static Complex I => new Complex(0.0, 1.0);

		public static Complex MaxValue => new Complex(double.MaxValue, double.MaxValue);

		public static Complex MinValue => new Complex(double.MinValue, double.MinValue);

		public Complex(double real, double imaginary)
		{
			Re = real;
			Im = imaginary;
		}

		public Complex(Complex c)
		{
			Re = c.Re;
			Im = c.Im;
		}

		public static Complex FromRealImaginary(double real, double imaginary)
		{
			Complex result = default(Complex);
			result.Re = real;
			result.Im = imaginary;
			return result;
		}

		public static Complex FromModulusArgument(double modulus, double argument)
		{
			Complex result = default(Complex);
			result.Re = modulus * Math.Cos(argument);
			result.Im = modulus * Math.Sin(argument);
			return result;
		}

		object ICloneable.Clone()
		{
			return new Complex(this);
		}

		public Complex Clone()
		{
			return new Complex(this);
		}

		public double GetModulus()
		{
			double re = Re;
			double im = Im;
			return Math.Sqrt(re * re + im * im);
		}

		public double GetModulusSquared()
		{
			double re = Re;
			double im = Im;
			return re * re + im * im;
		}

		public double GetArgument()
		{
			return Math.Atan2(Im, Re);
		}

		public Complex GetConjugate()
		{
			return FromRealImaginary(Re, 0.0 - Im);
		}

		public void Normalize()
		{
			double modulus = GetModulus();
			if (modulus == 0.0)
			{
				throw new DivideByZeroException("Can not normalize a complex number that is zero.");
			}
			Re /= modulus;
			Im /= modulus;
		}

		public static explicit operator Complex(ComplexF cF)
		{
			Complex result = default(Complex);
			result.Re = cF.Re;
			result.Im = cF.Im;
			return result;
		}

		public static explicit operator Complex(double d)
		{
			Complex result = default(Complex);
			result.Re = d;
			result.Im = 0.0;
			return result;
		}

		public static explicit operator double(Complex c)
		{
			return c.Re;
		}

		public static bool operator ==(Complex a, Complex b)
		{
			if (a.Re == b.Re)
			{
				return a.Im == b.Im;
			}
			return false;
		}

		public static bool operator !=(Complex a, Complex b)
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
			if (o is Complex)
			{
				Complex b = (Complex)o;
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
			if (o is Complex)
			{
				return GetModulus().CompareTo(((Complex)o).GetModulus());
			}
			if (o is double)
			{
				return GetModulus().CompareTo((double)o);
			}
			if (o is ComplexF)
			{
				return GetModulus().CompareTo(((ComplexF)o).GetModulus());
			}
			if (o is float)
			{
				return GetModulus().CompareTo((float)o);
			}
			throw new ArgumentException();
		}

		public static Complex operator +(Complex a)
		{
			return a;
		}

		public static Complex operator -(Complex a)
		{
			a.Re = 0.0 - a.Re;
			a.Im = 0.0 - a.Im;
			return a;
		}

		public static Complex operator +(Complex a, double f)
		{
			a.Re += f;
			return a;
		}

		public static Complex operator +(double f, Complex a)
		{
			a.Re += f;
			return a;
		}

		public static Complex operator +(Complex a, Complex b)
		{
			a.Re += b.Re;
			a.Im += b.Im;
			return a;
		}

		public static Complex operator -(Complex a, double f)
		{
			a.Re -= f;
			return a;
		}

		public static Complex operator -(double f, Complex a)
		{
			a.Re = (float)(f - a.Re);
			a.Im = (float)(0.0 - a.Im);
			return a;
		}

		public static Complex operator -(Complex a, Complex b)
		{
			a.Re -= b.Re;
			a.Im -= b.Im;
			return a;
		}

		public static Complex operator *(Complex a, double f)
		{
			a.Re *= f;
			a.Im *= f;
			return a;
		}

		public static Complex operator *(double f, Complex a)
		{
			a.Re *= f;
			a.Im *= f;
			return a;
		}

		public static Complex operator *(Complex a, Complex b)
		{
			double re = a.Re;
			double im = a.Im;
			double re2 = b.Re;
			double im2 = b.Im;
			a.Re = re * re2 - im * im2;
			a.Im = re * im2 + im * re2;
			return a;
		}

		public static Complex operator /(Complex a, double f)
		{
			if (f == 0.0)
			{
				throw new DivideByZeroException();
			}
			a.Re /= f;
			a.Im /= f;
			return a;
		}

		public static Complex operator /(Complex a, Complex b)
		{
			double re = a.Re;
			double im = a.Im;
			double re2 = b.Re;
			double im2 = b.Im;
			double num = re2 * re2 + im2 * im2;
			if (num == 0.0)
			{
				throw new DivideByZeroException();
			}
			a.Re = (re * re2 + im * im2) / num;
			a.Im = (im * re2 - re * im2) / num;
			return a;
		}

		public static Complex Parse(string s)
		{
			throw new NotImplementedException("Complex Complex.Parse( string s ) is not implemented.");
		}

		public override string ToString()
		{
			return $"( {Re}, {Im}i )";
		}

		public static bool IsEqual(Complex a, Complex b, double tolerance)
		{
			if (Math.Abs(a.Re - b.Re) < tolerance)
			{
				return Math.Abs(a.Im - b.Im) < tolerance;
			}
			return false;
		}
	}
}
