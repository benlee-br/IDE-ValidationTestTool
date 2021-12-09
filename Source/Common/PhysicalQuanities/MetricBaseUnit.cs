using System;
using System.Text;
using System.Diagnostics;

namespace BioRad.Common.PhysicalQuanities
{
	#region Documentation Tags
	/// <summary>
	/// Metric Conversion class
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: MetricBaseUnit.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/PhysicalQuanities/MetricBaseUnit.cs $</item>
	///			<item name="vssrevision">$Revision: 3 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 3/30/10 8:21a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public class MetricBaseUnit
	{
		#region Constants
		/// <summary>
		/// A prefix may be added to a unit to produce a multiple of the original unit. All multiples are integer powers of ten. 
		/// </summary>
		public enum Prefix
		{
			/// <summary>
			/// 1 = 1e+24
			/// </summary>
			Yotta,
			/// <summary>
			/// 1 = 1e+21
			/// </summary>
			Zetta,
			/// <summary>
			/// 1 = 1e+18
			/// </summary>
			Exa,
			/// <summary>
			/// 1 = 1e+15
			/// </summary>
			Peta,
			/// <summary>
			/// 1 = 1e+12
			/// </summary>
			Tera,
			/// <summary>
			/// 1 = 1e+9
			/// </summary>
			Giga,
			/// <summary>
			/// 1 = 1e+6
			/// </summary>
			Mega,
			/// <summary>
			/// 1 = 1e+3
			/// </summary>
			Kilo,
			/// <summary>
			/// 1 = 1e+2
			/// </summary>
			Hecto,
			/// <summary>
			/// 1 = 1e+1
			/// </summary>
			Deka,
			/// <summary>
			/// = 0
			/// </summary>
			BaseUnit,
			/// <summary>
			/// 1 = 1e-1
			/// </summary>
			deci,
			/// <summary>
			/// 1 = 1e-2
			/// </summary>
			cent,
			/// <summary>
			/// 1 = 1e-3
			/// </summary>
			milli,
			/// <summary>
			/// 1 = 1e-6 
			/// </summary>
			micro,
			/// <summary>
			/// 1 = 1e-9 
			/// </summary>
			nano,
			/// <summary>
			/// 1 = 1e-12
			/// </summary>
			pico,
			/// <summary>
			/// 1 = 1e-15
			/// </summary>
			femto,
			/// <summary>
			/// 1 = 1e-18
			/// </summary>
			atto,
			/// <summary>
			/// 1 = 1e-21 
			/// </summary>
			zepto,
			/// <summary>
			/// 1 = 1e-24
			/// </summary>
			yocto
		}
		#endregion

		#region Menber Data
		/// <summary>
		/// Value
		/// </summary>
		protected double m_Value;
		/// <summary>
		/// Prefix Units
		/// </summary>
		protected MetricBaseUnit.Prefix m_Units;
		/// <summary>
		/// Symbol
		/// </summary>
		protected string m_Symbol;
		/// <summary>
		/// Conversion factors.
		/// </summary>
		private static int[] m_Factors = 
		{
		+24, // Yotta = 1e+24
		+21, // Zetta = 1e+21
		+18, // Exa = 1e+18
		+15, // Peta = 1e+15
		+12, // Tera = 1e+12
		+9, // Giga = 1e+9
		+6, // Mega = 1e+6
		+3, // Kilo = 1e+3
		+2, // Hecto = 1e+2
		+1, // Deka = 1e+1
		0,  // base unit no prefix
		-1, // deci = 1e-1
		-2, // cent = 1e-2
		-3, // milli = 1e-3
		-6, // micro = 1e-6 
		-9, // nano = 1e-9 
		-12, // pico = 1e-12
		-15, // femto = 1e-15
		-18, // atto = 1e-18
		-21, // zepto = 1e-21 
		-24 // yocto = 1e-24
		};
		#endregion

		#region Accessors
		/// <summary>
		/// Get unit symbol.
		/// </summary>
		public string Symbol { get { return m_Symbol; } }

		/// <summary>
		/// Get current vaue in current prefix units.
		/// </summary>
		public double Value { get { return m_Value; } }

		/// <summary>
		/// Get prefix units.
		/// </summary>
		public MetricBaseUnit.Prefix Units { get { return m_Units; } }
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// Initializes a new instance of the MetricBaseUnit class.
		/// </summary>
		/// <param name="initialValue">Initial value in units.</param>
		/// <param name="unit">Base units for initial value.</param>
		/// <param name="symbol"></param>
		public MetricBaseUnit(double initialValue, MetricBaseUnit.Prefix unit, string symbol)
		{
			m_Value = initialValue;
			m_Units = unit;
			m_Symbol = symbol;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Convert value from one prefix to another prefix.
		/// </summary>
		/// <param name="value">Value to convert</param>
		/// <param name="from">Current prefix of value.</param>
		/// <param name="to">CPrefix to convert value to.</param>
		/// <returns></returns>
		public static MetricBaseUnit Convert(double value, MetricBaseUnit.Prefix from, MetricBaseUnit.Prefix to)
		{
			double v = MetricBaseUnit.ConvertPrefix2Base(value, from);
			v = MetricBaseUnit.ConvertBase2Prefix(v, to);
			return new MetricBaseUnit(v, to, string.Empty);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="unit"></param>
		/// <returns></returns>
		private static int GetConversionfactor(MetricBaseUnit.Prefix unit)
		{
			return m_Factors[(int)unit];
		}
		/// <summary>
		/// Convert from prefixed unit to base unit.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <returns></returns>
		private static double ConvertPrefix2Base(double value, MetricBaseUnit.Prefix unit)
		{
			int factor = GetConversionfactor(unit);
			double baseValue = value * System.Math.Pow(10, factor);
			return baseValue;
		}
		/// <summary>
		/// Convert base unit to prefixed units.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <returns></returns>
		private static double ConvertBase2Prefix(double value, MetricBaseUnit.Prefix unit)
		{
			int factor = GetConversionfactor(unit);
			if (factor >= 0)
				value /= System.Math.Pow(10, factor);
			else
				value *= System.Math.Pow(10, System.Math.Abs(factor));
			return value;
		}
		/// <summary>
		/// Validation method.
		/// </summary>
		public static void Validate()
		{
#if DEBUG
			const double c_Epsilon = 0.001;
			foreach (MetricBaseUnit.Prefix m in Enum.GetValues(typeof(MetricBaseUnit.Prefix)))
			{
				int i = MetricBaseUnit.GetConversionfactor(m);
			}

			// Yotta = 1e+24
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Yotta, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+24, c_Epsilon));
			// Zetta = 1e+21
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Zetta, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+21, c_Epsilon));
			// Exa = 1e+18
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Exa, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+18, c_Epsilon));
			// Peta = 1e+15
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Peta, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+15, c_Epsilon));
			// Tera = 1e+12
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Tera, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+12, c_Epsilon));
			// Giga = 1e+9
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Giga, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+9, c_Epsilon));
			// Mega = 1e+6
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Mega, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+6, c_Epsilon));
			// Kilo = 1e+3
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Kilo, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+3, c_Epsilon));
			// Hecto = 1e+2
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Hecto, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+2, c_Epsilon));
			// Deka = 1e+1
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.Deka, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e+1, c_Epsilon));
			// base
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.BaseUnit, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0, c_Epsilon));
			// deci = 1e-1
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.deci, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-1, c_Epsilon));
			// cent = 1e-2
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.cent, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-2, c_Epsilon));
			// milli = 1e-3
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.milli, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-3, c_Epsilon));
			// micro = 1e-6 
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.micro, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-6, c_Epsilon));
			// nano = 1e-9 
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.nano, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-9, c_Epsilon));
			// pico = 1e-12
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.pico, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-12, c_Epsilon));
			// femto = 1e-15
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.femto, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-15, c_Epsilon));
			// atto = 1e-18
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.atto, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-18, c_Epsilon));
			// zepto = 1e-21 
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.zepto, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-21, c_Epsilon));
			// yocto = 1e-24
			Debug.Assert(FloatingPoint.equal(Convert(1, MetricBaseUnit.Prefix.yocto, MetricBaseUnit.Prefix.BaseUnit).Value, 1.0e-24, c_Epsilon));

			Debug.Assert(FloatingPoint.equal(Convert(1000, MetricBaseUnit.Prefix.BaseUnit, MetricBaseUnit.Prefix.Kilo).Value, 1.0, c_Epsilon));

			Debug.Assert(FloatingPoint.equal(Convert(1000, MetricBaseUnit.Prefix.Hecto, MetricBaseUnit.Prefix.Kilo).Value, 100.0, c_Epsilon));

			Debug.Assert(FloatingPoint.equal(Convert(1000, MetricBaseUnit.Prefix.Tera, MetricBaseUnit.Prefix.Kilo).Value, 1.0e+12, c_Epsilon));

			double d = 1000;
			MetricBaseUnit mbu1 = Convert(d, MetricBaseUnit.Prefix.Tera, MetricBaseUnit.Prefix.Kilo);
			MetricBaseUnit mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.Kilo, MetricBaseUnit.Prefix.Tera);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));

			d = 1000000000000;
			mbu1 = Convert(d, MetricBaseUnit.Prefix.yocto, MetricBaseUnit.Prefix.Kilo);
			mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.Kilo, MetricBaseUnit.Prefix.yocto);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));

			d = 1.5;
			mbu1 = Convert(d, MetricBaseUnit.Prefix.yocto, MetricBaseUnit.Prefix.Kilo);
			mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.Kilo, MetricBaseUnit.Prefix.yocto);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));

			d = 15;
			mbu1 = Convert(d, MetricBaseUnit.Prefix.micro, MetricBaseUnit.Prefix.Kilo);
			mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.Kilo, MetricBaseUnit.Prefix.micro);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));

			d = 100;
			mbu1 = Convert(d, MetricBaseUnit.Prefix.Mega, MetricBaseUnit.Prefix.femto);
			mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.femto, MetricBaseUnit.Prefix.Mega);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));

			d = 0.0001;
			mbu1 = Convert(d, MetricBaseUnit.Prefix.atto, MetricBaseUnit.Prefix.Hecto);
			mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.Hecto, MetricBaseUnit.Prefix.atto);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));

			d = 0.1;
			mbu1 = Convert(d, MetricBaseUnit.Prefix.nano, MetricBaseUnit.Prefix.pico);
			mbu2 = Convert(mbu1.Value, MetricBaseUnit.Prefix.pico, MetricBaseUnit.Prefix.nano);
			Debug.Assert(FloatingPoint.equal(mbu2.Value, d, c_Epsilon));
		
#endif
		}
		#endregion
	}
}
