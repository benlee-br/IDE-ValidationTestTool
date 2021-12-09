
using System;
using System.Collections;
using System.Threading;
using System.Text;
using System.Diagnostics;

//todo: object pool pattern paper?
namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// Object pooling of reference types.
	/// </summary>
	/// <remarks>
	/// The object pool class allows for the reuse of previously released 
	/// allocated objects.
	/// How to use.
	/// <example>
	/// To use you must first register the type you want to reuse by as follows:
	/// <code>
	/// ObjectPool.GetInstance().RegisterType(typeof(WellData), 10);
	///	</code>
	/// </example>	
	/// Unregister the type when reuse of the object is no longer required as follows:
	/// <example>
	/// <code>
	/// ObjectPool.GetInstance().UnregisterType(typeof(WellData));
	///	</code>
	/// </example>
	/// Requirements for a type to be reused.
	/// <list>
	/// <item>
	///	Type must have a default constructor.
	///	</item>
	///	<item>
	///	Type must have a finalizer (destructor) as follows to release
	///	objects back to the pool:
	/// <example>
	/// <code>
	/// ~WellData()
	/// {
	///		// Required to return a release object to pool.
	///		if ( ObjectPool.GetInstance().IsRegisterType(typeof(WellData)) )
	///		{
	///			GC.ReRegisterForFinalize(this);
	///			ObjectPool.GetInstance().ReleaseObject(this);
	///		}
	/// }	
	///	</code>
	/// </example>
	/// </item>
	/// </list>
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:Ralph Ansell</item>
	///			<item name="review">Last design/code review:</item>
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
	///			<item name="vssfile">$Workfile: ObjectPool.cs $</item>
	///			<item name="vssfilepath">$Archive: /Denali/Source/Core/Common/Common/ObjectPool.cs $</item>
	///			<item name="vssrevision">$Revision: 6 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
	///			<item name="vssdate">$Date: 3/03/05 10:16a $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public class ObjectPool
	{
		#region Nexted Classes
		private class ObjectData
		{
			public Type objectType;
			public int initialSize;
			public int maxSize;
			public int NumberClients;
			public Queue pool;
		}
		#endregion

		#region Member Data
		/// <summary>
		/// 
		/// </summary>
		private Hashtable table;
		#endregion

		#region Accessors
		/// <summary>
		/// 
		/// </summary>
		protected Hashtable Table { get { return table; } }
		#endregion

		#region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		public ObjectPool()
		{
			table = new Hashtable();
		}
		#endregion

		#region Static Members
		/// <summary>
		/// 
		/// </summary>
		private static ObjectPool pool;
		/// <summary>
		/// 
		/// </summary>
		static ObjectPool()
		{
			pool = new ObjectPool();
		}
		/// <summary>
		/// Retrieves the shared ObjectPool instance.
		/// </summary>
		/// <returns>the shared ObjectPool instance</returns>
		public static ObjectPool GetInstance()
		{
			return pool;
		}
		#endregion // Static Members

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool IsRegisterType(Type t)
		{
			bool b = false;
			lock(Table)
			{
				b = Table.Contains(t.FullName);
			}
			return b;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="poolInitialSize"></param>
		/// <param name="maxSize"></param>
		public void RegisterType(Type t,
			int poolInitialSize, int maxSize)
		{
			if ( t == null ) 
				throw new ArgumentNullException("t");

			ObjectData data = null;

			// We allow multiplet clients. Keep track of the number of clients
			// that are using this type.
			if ( Table[t.FullName] != null )
			{
				data = GetObjectData(t);

				lock(data)
				{
					data.NumberClients++;
				}
				return;
			}

			// Create the object data for this type.
			data = new ObjectData();
			lock(data)
			{
				if ( poolInitialSize > maxSize )
					poolInitialSize = maxSize;

				data.objectType = t;
				data.initialSize = poolInitialSize;
				data.maxSize = maxSize;
				data.NumberClients++;

				data.pool = new Queue();

				// pre-allocate initial size.
				for ( int i=0; i<poolInitialSize; i++ )
					data.pool.Enqueue(AllocateObject(data));
			}

			// Add the new data to the hash table.
			lock (Table)
			{
				Table.Add(t.FullName, data);
			}
		}
		/// <summary>
		/// Terminate pooling for the given type.
		/// </summary>
		/// <param name="t">pooled type</param>
		public void UnregisterType(Type t)
		{
			ObjectData data = GetObjectData(t);

			lock(data)
			{
				// Update number clients.
				data.NumberClients--;

				// if last client then remove the type from the pool.
				if ( data.NumberClients == 0 )
				{
					// Remove the type from the hash table
					// so no further pooling will be permitted.
					Table.Remove(t.FullName);

					// Any types still in use are abandoned
					// Clean up any objects still in the pool.
					object o = null;
					foreach (object obj in data.pool)
					{
						o = obj;
						if (o != null && o is IDisposable)
						{
							// Dispose of the object
							IDisposable d = (IDisposable)o;
							d.Dispose();
						}
						o = null;
					}
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public void DumpPoolStats()
		{
			lock (Table)
			{
				foreach (DictionaryEntry t in Table)
				{
					ObjectData data = (ObjectData)t.Value;
					Trace.Write("Object pool type: ");
					Trace.Write(data.objectType.Name);
					Trace.Write(", size=");
					Trace.WriteLine(data.pool.Count);
				}
			}
		}
		/// <summary>
		/// Get an object of the given type from the object pool.
		/// </summary>
		/// <param name="t">type to retrieve from the pool</param>
		/// <returns>object of the given type</returns>
		/// <remarks>
		/// Note that this returns an object from the pool, or creates a new object
		/// for the pool, if possible.  If the maximum number of objects have been
		/// created, then this method waits for one to become available.
		/// </remarks>
		public object GetObject(Type t)
		{
			ObjectData data = GetObjectData(t);
			// Retrieve an object of the desired type from the pool.
			return RetrieveFromPool(data);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public void ReleaseObject(object obj)
		{
			Object o = obj;

			Type t = obj.GetType();
			ObjectData data = GetObjectData(t);
			
			// Add this object back into the pool.
			ReturnToPool(obj, data);
		}
		/// <summary>
		/// Get the ObjectData for the given type.
		/// </summary>
		/// <param name="t">pooled object type.</param>
		/// <returns>ObjectData associated with given type.</returns>
		private ObjectData GetObjectData(Type t)
		{
			ObjectData data = Table[t.FullName] as ObjectData;
			if (data == null)
			{
				StringBuilder sb = new StringBuilder(t.FullName);
				sb.Append(" is not registered in the object pool.");
				throw new ArgumentException(sb.ToString());
			}
			return data;
		}
		/// <summary>
		/// Creates a new object of the indicated type.
		/// </summary>
		/// <param name="data">Object pool data for desired type.</param>
		/// <returns>An object of the associated type.</returns>
		private object AllocateObject(ObjectData data)
		{
			return Activator.CreateInstance(data.objectType);
		}
		/// <summary>
		/// Retreives an object of the indicated type from the pool.
		/// </summary>
		/// <param name="data">Object pool data for desired type</param>
		/// <returns>An object of the associated type</returns>
		/// <exception cref="ApplicationException">A creation timeout occurred.</exception>
		private object RetrieveFromPool(ObjectData data)
		{
			object obj = null;

			lock(data)
			{
				if ( data.pool.Count == 0 )
					data.pool.Enqueue(AllocateObject(data));

				// Retrieve an object from the pool, if possible
				if ( data.pool.Count > 0 )
					obj = data.pool.Dequeue();

				// The result is null if pool was empty,
				// or only contained collected weak references.
				if (obj == null)
					throw new ApplicationException("empty pool?");			
			}

			//System.Diagnostics.Trace.Write("RetrieveFromPool = ");
			//System.Diagnostics.Trace.WriteLine(data.pool.Count);

			return obj;
		}
		/// <summary>
		/// Returns the given object to the object pool
		/// </summary>
		/// <param name="obj">object to return to the pool</param>
		/// <param name="data">object data asosciated with object's type</param>
		/// <remarks>
		/// This method queues the object into the pool.  If the minimum number
		/// of objects are not available, then a reference to the object is
		/// enqueued to ensure the object remains available.  If the minimum
		/// number of objects are already available, then only a weak reference
		/// to the object is enqueued.  This permits the garbage collector to
		/// reclaim this memory so that the pool will eventually return to
		/// the minimum size.  In a busy system, however, the object can be
		/// reclaimed from the weak reference.
		/// </remarks>
		private void ReturnToPool(object obj, ObjectData data)
		{
			lock(data)
			{
				if ( data.pool.Count < data.maxSize )
					data.pool.Enqueue(obj);// Return actual object to the pool
				else
				{	//dispose of object.
					if (obj != null && obj is IDisposable)
					{
						// Dispose of the object
						IDisposable d = (IDisposable)obj;
						d.Dispose();
					}
					obj = null;
				}
			}
		}
		#endregion 
	}
}
