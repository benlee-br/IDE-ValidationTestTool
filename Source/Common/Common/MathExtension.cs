using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Extend .Net original Math class, added functions Square, Standard deviation and Mean.
	/// </summary>
	/// <remarks>
	/// Remarks section for class.
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ben Lee</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="conformancereview">Conformance review:2/09/04, Ben Lee</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href="">Replace this text with ID</see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: MathExtension.cs $</item>
	///			<item name="vssfilepath">$Archive: /CFX_15/Source/Core/Common/Common/MathExtension.cs $</item>
	///			<item name="vssrevision">$Revision: 29 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Thouser $</item>
	///			<item name="vssdate">$Date: 10/23/08 11:20a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class MathExtension
	{
		/// <summary>
		/// TDistributionTable
		/// </summary>
		static public double[] TDistributionTable  =
		{ //  0      1      2      3      4      5      6      7      8      9   Index
			10000000, 318.3, 22.3, 10.2, 7.17, 5.89,  5.21,  4.78,   4.5,   4.3,  // 0n  
			4.14,     4.03,  3.93, 3.85, 3.79, 3.73,  3.69,  3.65,   3.61,  3.58,  // 1n
			3.55,     3.53,  3.51, 3.49, 3.47, 3.45,  3.44,  3.42,   3.41,  3.4,  // 2n
			3.39,     3.38,  3.37, 3.36, 3.35, 3.34,  3.33,  3.33,   3.32,  3.31,  // 3n
			3.31,     3.3,   3.3,  3.29, 3.28, 3.28,  3.27,  3.27,   3.27,  3.26, // 4n
			3.17
		};  
		
		
		#region Member data
		/// <summary>
		/// Random number generator data member
		/// </summary>
		static Random m_rnd= new Random();
		#endregion

		#region Methods
		/// <summary>
		/// Square operation of double type number
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double Square(double d)
		{
			return d*d;
		}
		/// <summary>
		/// Sum of Squared numbers
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double SumSquared(double[] d)
		{
			double squaredSum =0;			
			try
			{
				if ((d != null)&& (d.Length>0))
					for (int i=0; i<d.Length;i++)
					{
						squaredSum += Square(d[i]);
					}
			}
			catch
			{

			}
			return squaredSum;
		}
		/// <summary>
		/// Sum of numbers
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double Sum(double[] d)
		{
			double sum =0;			
			try
			{
				if ((d != null)&& (d.Length>0))
					for (int i=0; i<d.Length;i++)
					{
						sum += d[i];
					}
			}
			catch
			{

			}
			return sum;
		}	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double Maximum(double[] d)
		{
			double max = double.MinValue;
			try
			{
				if ((d != null)&& (d.Length>0))
					for (int i=0; i<d.Length;i++)
					{
						if (max < d[i])
							max = d[i];
					}
			}
			catch
			{

			}
			return max;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double Minimun(double[] d)
		{
			double min = double.MaxValue;
			try
			{
				if ((d != null)&& (d.Length>0))
					for (int i=0; i<d.Length;i++)
					{
						if (min > d[i])
							min = d[i];
					}
			}
			catch
			{

			}
			return min;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public int MaxIndex(double[] d)
		{
			double max = double.MinValue;
			int index = -1;
			try
			{
				if ((d != null)&& (d.Length>0))
					for (int i=0; i<d.Length;i++)
					{
						if (max < d[i])
						{
							max = d[i];
							index = i;
						}
					}
			}
			catch
			{

			}
			return index;
		}


		/// <summary>
		/// Genetate a random number within a specified range(-r, r)
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		static public int RandInt(int r)
		{
			return m_rnd.Next(-r, r);
		}
		/// <summary>
		/// Genetate a random double number within a specified range(0, 1.0)
		/// </summary>
		/// <returns></returns>
		static public double RandDouble()
		{
			return m_rnd.NextDouble();
		}
		/// <summary>
		/// Returns the sum of squares of deviations of data points from their sample mean.
		/// </summary>
		/// <param name="d"></param>
		static public double DeviationSquare(double[] d)
		{
			try
			{
				if (d.Length == 1)
					return 0;
				
				double sum =0;
				double mean = Mean(d);
				for (int i =0; i< d.Length;i++)
					sum  += Square(d[i]- mean);
				
				return sum;

			}

			catch
			{
			}
			return 0;			

		}		
		/// <summary>
		/// Standard deviation of array list of double type numbers
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static double StdDev( double [] d)
		{
			try
			{
				if (d.Length <= 1)
					return 0;

			    double mean = Mean(d);
			    double sum = d.Sum(t => Square(t - mean));
			    sum = Math.Sqrt(sum/(d.Length-1));
				return sum;
			}
			catch
			{
                return double.NaN;
			}
		}

        /// <summary>
        /// Standard Error of Mean SEM
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double StdErrorOfMean(double[] d)
        {
            try
            {
                if (d.Length <= 1)
                    return 0;

                double mean = Mean(d);
                double sum = d.Sum(t => Square(t - mean));
                sum = Math.Sqrt(sum / (d.Length - 1));
                sum = sum/Math.Sqrt(d.Length);
                return sum;
            }
            catch
            {
                return double.NaN;
            }
        }
        /// <summary>
        /// Variance of array list of double type numbers
        /// SumSquaredValue/n
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        static public double SampleVariance( double [] d)
		{
			try
			{
				double sum =0;
				double mean = Mean(d);
				for (int i =0; i< d.Length;i++)
					sum  += Square(d[i]- mean);
				return sum/(d.Length-1);

			}

			catch
			{
			}
			return 0;
		}
		/// <summary>
		/// Average of array list of double type numbers
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double Mean( double [] d)
		{
			double mean =0;			
			try
			{

				if ((d != null)&& (d.Length>0))
					for (int i=0; i<d.Length;i++)
						mean += d[i];
				mean = mean/d.Length;
				return mean;
			}
			catch
			{

			}
			return mean;
		}
		/// <summary>
		/// returns the geometric mean of array list of double type numbers
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static double GeoMean( double [] d)
		{
			double geoMean =1;
			if (d == null)
				return geoMean;
			try
			{
			    d = d.Where(t => double.IsNaN(t) == false).ToArray();
			    if (d.Length > 0)
			        geoMean = d.Aggregate(geoMean, (current, t) => current*t);
			    else
			        return double.NaN;
			    geoMean = Math.Pow(geoMean, (1/(double)d.Length));
				return geoMean;
			}
			catch
			{
			    return double.NaN;
			}
		}

		/// <summary>
		/// Average of array list of double type numbers
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		static public double IntervalRoundUp( double  d)
		{
			double mean =0;			
			try
			{
				if (d > 0)
				{
					double power = Math.Log10(d);
					double interval = 0;
					double round = 1;

					if (power > 1)
					{
						power = Math.Floor(power);
						round = Math.Round(d / Math.Pow(10, power));
					}
					else
					{
						power = Math.Floor(power);
						round = Math.Round(d / Math.Pow(10, power));
					}
					interval = round * Math.Pow(10, power);
					return interval;
					
				}
				else
					return double.NaN;


			}
			catch
			{

			}
			return mean;
		}
		/// <summary>
		/// One dimension Median filter of double array, This function take 
		/// mask size parameter(Mask size range of 3,5,9..)
		/// </summary>
		/// <param name="array"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		static public double[] MedianFilter1D(double[] array, int size )
		{
			double[] smoothArray = new double[array.Length];
			int radius = size/2;
			ArrayList arrayList = new ArrayList();
			for(int i=0; i< array.Length; i++)
			{
				if (array[i] > 0) 
				{
					for (int j=0; j< size; j++) 
					{
						int index = i - radius + j ;
						if ((index < array.Length) && (index > -1)) 
						{
							arrayList.Add( array[index]);
						}
						else
							arrayList.Add(0);
					}
					arrayList.Sort();
					double [] filter = (double []) arrayList.ToArray(typeof(double));
					smoothArray[i] = filter[filter.Length/2];
				}
			}
			return smoothArray;
		}
		/// <summary>
		/// One dimension Mean filter of double array, This function take 
		/// mask size parameter(Mask size range of 3,5,9..)
		/// </summary>
		/// <param name="array"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		static public double[] MeanFilter1D(double[] array, int size )
		{
			double[] smoothArray = new double[array.Length];
			int radius = size/2;
			ArrayList arrayList = new ArrayList();
			for(int i=0; i< array.Length; i++)
			{
				if (array[i] > 0) 
				{
					for (int j=0; j< size; j++) 
					{
						int index = i - radius + j ;
						if ((index < array.Length) && (index > -1)) 
						{
							arrayList.Add( array[index]);
						}
					}
					double [] filter = (double []) arrayList.ToArray(typeof(double));
					smoothArray[i] = Mean(filter);
				}
			}
			return smoothArray;
		}
		/// <summary>
		/// One dimension convolution filter of double array, This function take 
		/// kernel filter as filtering parameter(length of 3,5,9..)
		/// </summary>
		/// <param name="array"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		static public double[] Convolve1D(double[] array, double[] mask )
		{
			double[] smoothArray = new double[array.Length];
			int radius = mask.Length/2;
			double sum =0;
			for(int i=0; i< array.Length; i++)
			{
				if (array[i] > 0) 
				{
					sum=0;
					for (int j=0; j< mask.Length; j++) 
					{
						int index = i - radius + j ;
						if ((index < array.Length) && (index > -1)) 
						{
							sum += ( mask[j] * array[index]);
						}
					}
					smoothArray[i]=sum;
				}
			}
			return smoothArray;
		}
		/// <summary>
		/// One dimension Gaussian filter convolution of double array, This function makes an 
		/// intensity profile smoother with a Gaussian filter of size = mask. 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		static public double[] GaussianFilter(double[] array, double mask )
		{
			// This function makes an intensity profile smoother with a Gaussian
			// filter of size = mask.  

			int nd,md;
			double coeff,term;
			if (mask > 0) 
			{
				// Generate Gaussian mask
				coeff = -1/(2*mask*mask);
				term = 1/(2*Math.PI*mask*mask);

				nd= (int) (4 * mask+0.5);  //round
				md=2*nd+1;

				double[] kernel = new double[md];

				// Bell shape kernel
				for (int i=0; i < md; i++) 
				{
					int x=i-nd;
					kernel[i]=1/Math.Sqrt(term)*Math.Exp(x*x*coeff);
				}
				return Convolve1D( array, kernel );
			}
			return new double[0];
		}
		/// <summary>
		/// generate histogram (frequency distribution) base on bin array length, 
		/// range from minimum to maximum of dataArray
		/// </summary>
		/// <param name="dataArray"></param>
		/// <param name="bin"></param>
		public static void histogram(double[] dataArray, double[] bin)
		{
			double Max = double.MinValue;
			double Min = double.MaxValue;

			//find maximum and minimum
			for(int index =0; index < dataArray.Length ; index++)
			{
				if (Max < dataArray[index])
					Max = dataArray[index];
				else if (Min > dataArray[index])
					Min = dataArray[index];
			}

			double range = (double)(Max -Min)/ bin.Length;

			for(int i =0; i < dataArray.Length ; i++)
			{
				int index =(int) Math.Floor(((dataArray[i] - Min)/ range));
				if (index> bin.Length-1)
					index = bin.Length-1;
				if (index<0)
					index = 0;
				bin[index]++;
			}
		}
		/// <summary>
		/// LinearRegression with Least square fit
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		static public LinearRegressionResult LinearRegression(double[] x,double[] y)
		{
			if ((x.Length == 0) || (x.Length != y.Length))
				return null;
			int NumObservations = x.Length;
			LinearRegressionResult result = new LinearRegressionResult(NumObservations);
			double SquaredSum = BioRad.Common.MathExtension.SumSquared( x);
			double x_average = BioRad.Common.MathExtension.Mean(x);

			double SSxx = SquaredSum - NumObservations * BioRad.Common.MathExtension.Square(x_average);
			SquaredSum = BioRad.Common.MathExtension.SumSquared( y);
			double y_average = BioRad.Common.MathExtension.Mean(y);
			double SSyy = SquaredSum - NumObservations * BioRad.Common.MathExtension.Square(y_average);
			double SSxy = 0;
			for ( int i  =0 ; i < x.Length ; i++)
			{
				SSxy += (x[i]* y[i]);
			}
			SSxy = SSxy - NumObservations * x_average * y_average;

			// Linear regression  y = a + bx
			//		coefficient b = SumSquareValue_xy/SumSquareValue_xx  ;(b: slope)
			result.Slope =  SSxy/SSxx;

			//		a = y_average - b * x_average
			result.Intercept = y_average - result.Slope * x_average;

			//double PCREfficiency = ComputeLeastsquareFitEfficiency(result.Slope);

			// R^2 = SumSquareValue_xy^2 /(SumSquareValue_xx * SumSquareValue_yy)
			result.CorrelationCoeffcient =( SSxy * SSxy) /(SSxx *SSyy);
			result.CorrelationValue = Math.Sqrt(result.CorrelationCoeffcient);
			// residual = (yi_expected - yi_observed) 
			double ssresid =0;
			double sstotal =0;
			for(int i =0; i < NumObservations; i++)
			{
				result.Residuals[i] = y[i] - result.Intercept - result.Slope* x[i];
				ssresid += MathExtension.Square(result.Residuals[i]);
				sstotal += MathExtension.Square(y[i]-y_average);
			}

			/* The following section all calculation are based on Excel LINEST function calculation. 
			http://support.microsoft.com/default.aspx?scid=kb;en-us;828533#XSLTH3162121121120121120120
			Total DF(Degree of freedom) equals the number of rows (or datapoints) minus two 
			if fitted (EXCEL:LINEST third arg = TRUE) */

			int DF = NumObservations - 2;
			result.StdErrorEstimatedY  = Math.Sqrt(ssresid/DF); // see EXCEL:STEYX
			result.StdErrorSlope  = result.StdErrorEstimatedY*Math.Sqrt(NumObservations/(NumObservations * 
				MathExtension.SumSquared(x)-MathExtension.Square(MathExtension.Sum(x))));
			result.StdErrorIntercept = result.StdErrorEstimatedY*Math.Sqrt(MathExtension.SumSquared(x)/(NumObservations * 
				MathExtension.SumSquared(x)-MathExtension.Square(MathExtension.Sum(x))));
			result.ResidualSumSquare = ssresid;
			result.TotalSumSquare = sstotal;
			result.RegressionSumSquare = sstotal - ssresid;

			result.StandardDeviation = BioRad.Common.MathExtension.StdDev(result.Residuals);
			return result;
		}

		#endregion 
	}
	/// <summary>
	/// Linear Regression Result with statistics numbers
	/// </summary>
	public class LinearRegressionResult
	{
		#region Member Data
		private double m_Slope =0 ;
		private double m_Intercept =0 ;
		private double m_CorrelationCoefficient =0 ;
		private double m_CorrelationValue = 0;
		private double m_StandardDeviation = 0;
		private double[] m_Residuals ;
		private double m_TotalSumSquare;
		private double m_ResidualSumSquare;
		private double m_RegressionSumSquare;
		private double m_StdErrorSlope =0 ;
		private double m_StdErrorIntercept =0 ;
		private double m_StdErrorEstimatedY =0 ;

		#endregion

		#region Accessors
		/// <summary>
		/// Slope
		/// </summary>
		public double StdErrorSlope 
		{
			get { return m_StdErrorSlope;}
			set { m_StdErrorSlope = value;}
		}		
		/// <summary>
		/// Slope
		/// </summary>
		public double StdErrorIntercept 
		{
			get { return m_StdErrorIntercept;}
			set { m_StdErrorIntercept = value;}
		}		
		/// <summary>
		/// Slope
		/// </summary>
		public double StdErrorEstimatedY 
		{
			get { return m_StdErrorEstimatedY;}
			set { m_StdErrorEstimatedY = value;}
		}		
        
		/// <summary>
		/// Slope
		/// </summary>
		public double Slope 
		{
			get { return m_Slope;}
			set { m_Slope = value;}
		}
		/// <summary>
		/// Intercept
		/// </summary>
		public double Intercept
		{
			get { return m_Intercept;}
			set { m_Intercept = value;}
		}
		/// <summary>
		/// Correlation Coeffcient(R^2)
		/// </summary>
		public double CorrelationCoeffcient 
		{
			get { return m_CorrelationCoefficient;}
			set { m_CorrelationCoefficient = value;}
		}
		/// <summary>
		/// Correlation Value(R)
		/// </summary>
		public double CorrelationValue 
		{
			get { return m_CorrelationValue;}
			set { m_CorrelationValue = value;}
		} 
		/// <summary>
		/// Standard Deviation of Residuals
		/// </summary>
		public double StandardDeviation 
		{
			get { return m_StandardDeviation;}
			set { m_StandardDeviation = value;}
		}
		/// <summary>
		/// Residuals
		/// </summary>
		public double[] Residuals 
		{
			get { return m_Residuals;}
			set { m_Residuals = value;}
		}

		/// <summary>
		/// The total sum of squares is the sum of the squares of actual y-values
		/// </summary>
		public double TotalSumSquare 
		{
			get { return m_TotalSumSquare;}
			set { m_TotalSumSquare = value;}
		}
		/// <summary>
		/// The residual sum of squares is the sum of the squares difference between the y-value 
		/// estimated and actual y-values
		/// </summary>
		public double ResidualSumSquare 
		{
			get { return m_ResidualSumSquare;}
			set { m_ResidualSumSquare = value;}
		}
		/// <summary>
		/// The regression sum of squares = TotalSumSquare - ResidualSumSquare
		/// </summary>
		public double RegressionSumSquare 
		{
			get { return m_RegressionSumSquare;}
			set { m_RegressionSumSquare = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="NumObservations"></param>
		public LinearRegressionResult(int NumObservations)
		{
			m_Residuals = new double[NumObservations];
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// gets the y value of the line at the given x value.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public double GetY(double x)
		{
			return Slope * x + Intercept;
		}
		#endregion
	}
	/// <summary>
	/// Histogram 
	/// </summary>
	public class Histogram
	{
		#region Member Data
		private double[] m_DataArray;
		private int[] m_Bin;
		private double m_Maximum;
		private double m_Minimum;
		private double m_Range;

		#endregion

		#region Accessors
		/// <summary>
		/// Slope
		/// </summary>
		public double[] DataArray 
		{
			get { return m_DataArray;}
			set { m_DataArray = value;}
		}
		/// <summary>
		/// Intercept
		/// </summary>
		public int[] Bin
		{
			get { return m_Bin;}
			set { m_Bin = value;}
		}

		/// <summary>
		/// Maximum
		/// </summary>
		public double Maximum 
		{
			get { return m_Maximum;}
		}
		/// <summary>
		/// Minimum
		/// </summary>
		public double Minimum 
		{
			get { return m_Minimum;}
		}
		/// <summary>
		/// Mean
		/// </summary>
		public double Mean 
		{
			get { return MathExtension.Mean(m_DataArray);}
		} 
		/// <summary>
		/// Median
		/// </summary>
		public double Median 
		{
			get { return m_DataArray[m_DataArray.Length/2];}
		} 
		/// <summary>
		/// Standard Deviation of Residuals
		/// </summary>
		public double StandardDeviation 
		{
			get { return MathExtension.StdDev(m_DataArray);}
		}
		/// <summary>Access the value at the given location.</summary>
		public int this[int i]
		{
			set { m_Bin[i]= value; }
			get 
			{ 
				return m_Bin[i];
			}
		}
		#endregion
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataArray"></param>
		/// <param name="sizeOfBin"></param>
		public Histogram(double[] dataArray,int sizeOfBin)
		{
			m_DataArray = dataArray;
			m_Bin = new int[sizeOfBin];
			Calculate();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataArray"></param>
		/// <param name="sizeOfBin"></param>
		public Histogram(ushort[] dataArray,int sizeOfBin)
		{
			m_DataArray = new double[dataArray.Length];
			for(int i =0; i <dataArray.Length ; i++)
			{
				m_DataArray[i] = (double) dataArray[i];
			}
			m_Bin = new int[sizeOfBin];
			Calculate();
		}		
		/// <summary>
		/// Calculate
		/// </summary>
		private void Calculate()
		{
			Array.Sort(m_DataArray);
			m_Minimum = 0;
			m_Maximum = 256;
			m_Range = 1;
			if (m_DataArray[m_DataArray.Length-1] -m_DataArray[0] > m_Bin.Length)
			{
				m_Minimum = m_DataArray[0];
				m_Maximum = m_DataArray[m_DataArray.Length-1];
				m_Range= (double)(m_Maximum -m_Minimum)/ m_Bin.Length;
			}
			for(int i =0; i < m_DataArray.Length ; i++)
			{
				int index =(int) Math.Floor(((m_DataArray[i] - m_Minimum)/ m_Range));
				if (index> m_Bin.Length-1)
					index = m_Bin.Length-1;
				if (index<0)
					index = 0;
				m_Bin[index]++;
			}
		}
		/// <summary>
		/// Calculate
		/// </summary>
		public int MaxBin()
		{
			int max = int.MinValue;
			for(int i =0; i < m_Bin.Length ; i++)
			{
				if (max < m_Bin[i])
					max = m_Bin[i];
			}
            return max;
		}
		/// <summary>
		/// return related Bin Value by given bin index number
		/// </summary>
		public int BinValue(int binIndex)
		{
			return   (int)(binIndex * m_Range + m_Minimum);

		}
	}
}
