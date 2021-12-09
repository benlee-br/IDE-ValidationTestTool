using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BioRad.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Set<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private List<T> data;
        /// <summary>
        /// 
        /// </summary>
        public Set()
        {
            data = new List<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public Set(int capacity)
        {
            data = new List<T>(capacity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        public Set(Set<T> original)
        {
            data = new List<T>(original.data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        public Set(IEnumerable<T> original)
        {
            data = new List<T>();
            AddRange(original);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return data.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        public void Add(T a)
        {
            if ( !data.Contains(a) )
                data.Add(a);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (T a in range)
                Add(a);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public Set<U> ConvertAll<U>(Converter<T, U> converter)
        {
            Set<U> result = new Set<U>(this.Count);
            foreach (T element in this)
                result.Add(converter(element));
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool TrueForAll(Predicate<T> predicate)
        {
			foreach (T element in this)
			{
				if (!predicate(element))
					return false;
			}
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Set<T> FindAll(Predicate<T> predicate)
        {
            Set<T> result = new Set<T>();
			foreach (T element in this)
			{
				if (predicate(element))
					result.Add(element);
			}
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action)
        {
            foreach (T element in this)
                action(element);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            data.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Contains(T a)
        {
            return data.Contains(a);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(T[] array, int index)
        {
            data.CopyTo(array, index); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool Remove(T a)
        {
            if ( data.Contains(a) )
                return data.Remove(a);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Union
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Set<T> operator |(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>(a);
            result.AddRange(b);
            return result;
        }
        /// <summary>
        /// Union
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Set<T> Union(IEnumerable<T> b)
        {
            return this|new Set<T>(b);
        }

		/// <summary>
		/// Union
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Set<T> Union(IEnumerable<T> a, IEnumerable<T> b)
		{
			return new Set<T>(a) | new Set<T>(b);
		}
		
		/// <summary>
        /// Intersection
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>All elements in set a that are also in set b.</returns>
        public static Set<T> operator &(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
			foreach (T element in a)
			{
				if (b.Contains(element))
					result.Add(element);
			}
            return result;
        }
        /// <summary>
        /// Computes the intersection of this set with another set. 
        /// The intersection of two sets is all items that appear in both of the sets. 
        /// A new set is created with the intersection of the sets and is returned. 
        /// This set and the other set are unchanged. 
        /// </summary>
        /// <param name="b"></param>
		/// <returns>All elements in set a that are also in set b.</returns>
		public Set<T> Intersection(IEnumerable<T> b)
        {
            return this & new Set<T>(b);
        }

		/// <summary>
		/// Computes the intersection of this set with another set. 
		/// The intersection of two sets is all items that appear in both of the sets. 
		/// A new set is created with the intersection of the sets and is returned. 
		/// This set and the other set are unchanged. 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>All elements in set a that are also in set b.</returns>
		public static Set<T> Intersection(IEnumerable<T> a, IEnumerable<T> b)
		{
			return new Set<T>(a) & new Set<T>(b);
		}

        /// <summary>
        /// Difference
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>All elements of set a that are not in set b.</returns>
        public static Set<T> operator -(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
			foreach (T element in a)
			{
				if (!b.Contains(element))
					result.Add(element);
			}
            return result;
        }
        /// <summary>
        /// Difference
        /// </summary>
        /// <param name="b"></param>
		/// <returns>All elements of set a that are not in set b.</returns>
		public Set<T> Difference(IEnumerable<T> b)
        {
            return this - new Set<T>(b);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>All elements of set a that are not in set b.</returns>
		public static Set<T> Difference(IEnumerable<T> a, IEnumerable<T> b)
		{
			return new Set<T>(a) - new Set<T>(b);
		}

        /// <summary>
        /// Symmetric Difference
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Set<T> operator ^(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
			foreach (T element in a)
			{
				if (!b.Contains(element))
					result.Add(element);
			}
			foreach (T element in b)
			{
				if (!a.Contains(element))
					result.Add(element);
			}
            return result;
        }
        /// <summary>
        /// Computes the symmetric difference of this set with another set. 
        /// The symmetric difference of two sets is all items that appear 
        /// in either of the sets, but not both. A new set is created with 
        /// the symmetric difference of the sets and is returned. 
        /// This set and the other set are unchanged. 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Set<T> SymmetricDifference(IEnumerable<T> b)
        {
            return this ^ new Set<T>(b);
        }
        /// <summary>
        /// 
        /// </summary>
        public static Set<T> Empty
        {
            get { return new Set<T>(0); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(Set<T> a, Set<T> b)
        {
			foreach (T element in a)
			{
				if (!b.Contains(element))
					return false;
			}
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator < (Set<T> a,Set<T> b)
        {
            return (a.Count < b.Count) && (a <= b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Set<T> a, Set<T> b)
        {
            return (a.Count == b.Count) && (a <= b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator > (Set<T> a, Set<T> b)
        {
            return b < a;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(Set<T> a, Set<T> b)
        {
            return (b <= a);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Set<T> a, Set<T> b)
        {
            return !(a == b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Set<T> a = this;
            Set<T> b = obj as Set<T>;
            if (b == null)
                return false;
            return a == b;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashcode = 0;
			foreach (T element in this)
			{
				hashcode ^= element.GetHashCode();
			}
            return hashcode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (T element in this)
            {
                sb.Append(element.ToString());
                sb.Append(" ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)data).CopyTo(array, index);
        }
        /// <summary>
        /// 
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return ((ICollection)data).SyncRoot; }
        }
        /// <summary>
        /// 
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)data).IsSynchronized; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)data).GetEnumerator();
        }
#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        public static void UnitTest()
        {
            #region correctness tests
            Set<int> a = new Set<int>(new int[] { 1,2,3 });
            Set<int> b = new Set<int>(new int[] { 3,4,5 });
            Set<int> c = new Set<int>(new int[] { 1,2,3,4 });

            // checking for changes.
            Set<int> x = new Set<int>(new int[] { 1,2,3 });//1st set of numbers
            Set<int> y = new Set<int>(new int[] { 1,2,4,5 });//2nd set of numbers
            if (x == y) //no change
            {
            }
            else// we either have missing and/or new numbers
            {
                Set<int> m = x - y;//missing numbers = 1, 3
                Set<int> n = y - x;//new numbers = 4, 5
                foreach (int i in n)
                {
                    int p = i;
                }
            }
            // end


            Set<int> t = a & b;
            Trace.Assert((a & b) == new Set<int>(new int[] { 3 }));
            Trace.Assert((a.Intersection(b)) == new Set<int>(new int[] { 3 }));

            t = a | b;
            Trace.Assert((a | b) == new Set<int>(new int[] { 1,2,3,4,5 }));

            t = a ^ b;
            Trace.Assert((a ^ b) == new Set<int>(new int[] { 1,2,4,5 }));

            t = a - b;
            Trace.Assert(a - b == new Set<int>(new int[] { 1,2 }));

            t = b - a;
            Trace.Assert(b - a == new Set<int>(new int[] { 4,5 }));

            Trace.Assert(a != c);

            Trace.Assert(a <= c);

            Trace.Assert(c >= a);

            Trace.Assert(a < c);

            Trace.Assert(c > a);
            #endregion
        }
#endif
    }
}
