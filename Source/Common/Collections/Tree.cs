using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Security.Permissions;

namespace BioRad.Common
{
    /// <summary>
    /// 
    /// </summary>
	public interface IDeepCopy
	{
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		object CreateDeepCopy();
	}
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEnumerableCollection<T> : IEnumerable<T>, ICollection
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		bool Contains( T item );
	}
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IEnumerableCollectionPair<T>
	{
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollection<INode<T>> Nodes { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollection<T> Values { get; }
	}
    /// <summary>
    /// 
    /// </summary>
	public enum NodeTreeInsertOperation
	{
        /// <summary>
        /// 
        /// </summary>
		Previous,
        /// <summary>
        /// 
        /// </summary>
		Next,
        /// <summary>
        /// 
        /// </summary>
		Child,
        /// <summary>
        /// 
        /// </summary>
		Tree
	}
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface INode<T> : IEnumerableCollectionPair<T>, IDisposable
	{
        /// <summary>
        /// 
        /// </summary>
		T Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		string ToStringRecursive();
        /// <summary>
        /// 
        /// </summary>
		int Depth { get; }
        /// <summary>
        /// 
        /// </summary>
		int BranchIndex { get; }
        /// <summary>
        /// 
        /// </summary>
		int BranchCount { get; }
        /// <summary>
        /// 
        /// </summary>
		int Count { get; }
        /// <summary>
        /// 
        /// </summary>
		int DirectChildCount { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Parent { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Previous { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Next { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Child { get; }
        /// <summary>
        /// 
        /// </summary>
		ITree<T> Tree { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Root { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Top { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> First { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Last { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> LastChild { get; }
        /// <summary>
        /// 
        /// </summary>
		bool IsTree { get; }
        /// <summary>
        /// 
        /// </summary>
		bool IsRoot { get; }
        /// <summary>
        /// 
        /// </summary>
		bool IsTop { get; }
        /// <summary>
        /// 
        /// </summary>
		bool HasParent { get; }
        /// <summary>
        /// 
        /// </summary>
		bool HasPrevious { get; }
        /// <summary>
        /// 
        /// </summary>
		bool HasNext { get; }
        /// <summary>
        /// 
        /// </summary>
		bool HasChild { get; }
        /// <summary>
        /// 
        /// </summary>
		bool IsFirst { get; }
        /// <summary>
        /// 
        /// </summary>
		bool IsLast { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		INode<T> this[ T item ] { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		bool Contains( INode<T> item );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		bool Contains( T item );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> InsertPrevious( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> InsertNext( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> InsertChild( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> Add( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> AddChild( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void InsertPrevious( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void InsertNext( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void InsertChild( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void Add( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void AddChild( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		ITree<T> Cut( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		ITree<T> Copy( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		ITree<T> DeepCopy( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		bool Remove( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		ITree<T> Cut();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		ITree<T> Copy();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		ITree<T> DeepCopy();
        /// <summary>
        /// 
        /// </summary>
		void Remove();
        /// <summary>
        /// 
        /// </summary>
		bool CanMoveToParent { get; }
        /// <summary>
        /// 
        /// </summary>
		bool CanMoveToPrevious { get; }
        /// <summary>
        /// 
        /// </summary>
		bool CanMoveToNext { get; }
        /// <summary>
        /// 
        /// </summary>
		bool CanMoveToChild { get; }
        /// <summary>
        /// 
        /// </summary>
		bool CanMoveToFirst { get; }
        /// <summary>
        /// 
        /// </summary>
		bool CanMoveToLast { get; }
        /// <summary>
        /// 
        /// </summary>
		void MoveToParent();
        /// <summary>
        /// 
        /// </summary>
		void MoveToPrevious();
        /// <summary>
        /// 
        /// </summary>
		void MoveToNext();
        /// <summary>
        /// 
        /// </summary>
		void MoveToChild();
        /// <summary>
        /// 
        /// </summary>
		void MoveToFirst();
        /// <summary>
        /// 
        /// </summary>
		void MoveToLast();
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> All { get; }
        /// <summary>
        /// 
        /// </summary>
        IEnumerableCollectionPair<T> AllLeafs { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> AllChildren { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> DirectChildren { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> DirectChildrenInReverse { get; }
	}
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ITree<T> : IEnumerableCollectionPair<T>, IDisposable
	{
        /// <summary>
        /// 
        /// </summary>
		Type DataType { get; }
        /// <summary>
        /// 
        /// </summary>
		IEqualityComparer<T> DataComparer { get; set; }
        /// <summary>
        /// 
        /// </summary>
		void Clear();
        /// <summary>
        /// 
        /// </summary>
		int Count { get; }
        /// <summary>
        /// 
        /// </summary>
		int DirectChildCount { get; }
        /// <summary>
        /// 
        /// </summary>
		INode<T> Root { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> this[ T o ] { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		string ToStringRecursive();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		bool Contains( T item );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		bool Contains( INode<T> item );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> InsertChild( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		INode<T> AddChild( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void InsertChild( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		void AddChild( ITree<T> tree );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		ITree<T> Cut( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		ITree<T> Copy( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		ITree<T> DeepCopy( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		bool Remove( T o );
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		ITree<T> Copy();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		ITree<T> DeepCopy();
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> All { get; }
        /// <summary>
        /// 
        /// </summary>
        IEnumerableCollectionPair<T> AllLeafs { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> AllChildren { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> DirectChildren { get; }
        /// <summary>
        /// 
        /// </summary>
		IEnumerableCollectionPair<T> DirectChildrenInReverse { get; }
	}
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class NodeTree<T> : INode<T>, ITree<T>
	{
		private T _Data = default( T );

		private NodeTree<T> _Parent = null;
		private NodeTree<T> _Previous = null;
		private NodeTree<T> _Next = null;
		private NodeTree<T> _Child = null;
        /// <summary>
        /// 
        /// </summary>
		protected NodeTree() 
        { 
        }
        /// <summary>
        /// 
        /// </summary>
		public void Dispose()
		{
			Dispose( true );

			GC.SuppressFinalize( this );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
			
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ITree<T> NewTree()
		{
			return new RootObject();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITree<T> NewTree(string rootName)
        {
            return new RootObject(rootName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataComparer"></param>
        /// <returns></returns>
		public static ITree<T> NewTree( IEqualityComparer<T> dataComparer )
		{
			return new RootObject( dataComparer );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected static INode<T> NewNode()
		{
			return new NodeTree<T>();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected virtual NodeTree<T> CreateTree()
		{
			return new RootObject();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected virtual NodeTree<T> CreateNode()
		{
			return new NodeTree<T>();
		}
		/// <summary>Obtains the <see cref="String"/> representation of this instance.</summary>
		/// <returns>The <see cref="String"/> representation of this instance.</returns>
		/// <remarks>
		/// <p>
		/// This method returns a <see cref="String"/> that represents this instance.
		/// </p>
		/// </remarks>
		public override string ToString()
		{
			T data = Data;
			if ( data == null ) 
                return String.Empty;
			return data.ToString();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public virtual string ToStringRecursive()
		{
            StringBuilder sb = new StringBuilder();

            foreach (NodeTree<T> node in All.Nodes)
                sb.AppendFormat("{0}{1}{2}", new String('\t', node.Depth), node, Environment.NewLine);

            return sb.ToString();
		}
        /// <summary>
        /// 
        /// </summary>
		public virtual int Depth
		{
			get
			{
				int i = -1;

				for ( INode<T> node = this ; !node.IsRoot ; node = node.Parent ) 
                    i++;

				return i;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public virtual int BranchIndex
		{
			get
			{
				int i = -1;

				for ( INode<T> node = this ; node != null ; node = node.Previous ) 
                    i++;

				return i;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public virtual int BranchCount
		{
			get
			{
				int i = 0;

				for ( INode<T> node = First ; node != null ; node = node.Next ) 
                    i++;

				return i;
			}
		}

		//-----------------------------------------------------------------------------
		// DeepCopyData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		[ReflectionPermission( SecurityAction.Demand, Unrestricted = true )]
		protected virtual T DeepCopyData( T data )
		{
			if ( data == null ) 
            { 
                Debug.Assert( true ); 
                return default( T ); 
            }

			// IDeepCopy
			IDeepCopy deepCopy = data as IDeepCopy;
			if ( deepCopy != null ) 
                return ( T ) deepCopy.CreateDeepCopy();

			// ICloneable
			ICloneable cloneable = data as ICloneable;
			if ( cloneable != null ) 
                return ( T ) cloneable.Clone();

			// Copy constructor
			ConstructorInfo ctor = data.GetType().GetConstructor(
				 BindingFlags.Instance | BindingFlags.Public,
				 null, new Type[] { typeof( T ) }, null );
			if ( ctor != null ) 
                return ( T ) ctor.Invoke( new object[] { data } );

			// give up
			return data;
		}

		//-----------------------------------------------------------------------------
		// RootObject
        /// <summary>
        /// 
        /// </summary>
		[Serializable]
		protected class RootObject : NodeTree<T>
		{
            private string m_RootName = string.Empty;
			private int _Version = 0;
            /// <summary>
            /// 
            /// </summary>
			protected override int Version
			{
				get { return _Version; }
				set { _Version = value; }
			}

			private IEqualityComparer<T> _DataComparer;
            /// <summary>
            /// 
            /// </summary>
			public override IEqualityComparer<T> DataComparer
			{
				get
				{
					if ( _DataComparer == null ) 
                        _DataComparer = EqualityComparer<T>.Default;

					return _DataComparer;
				}

				set { _DataComparer = value; }
			}
            /// <summary>
            /// 
            /// </summary>
			public RootObject()
			{
                m_RootName = "ROOT: " + DataType.Name; 
			}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            public RootObject(string name)
            {
                m_RootName = name;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dataComparer"></param>
			public RootObject( IEqualityComparer<T> dataComparer )
			{
				_DataComparer = dataComparer;
			}

			/// <summary>Obtains the <see cref="String"/> representation of this instance.</summary>
			/// <returns>The <see cref="String"/> representation of this instance.</returns>
			/// <remarks>
			/// <p>
			/// This method returns a <see cref="String"/> that represents this instance.
			/// </p>
			/// </remarks>
			public override string ToString() 
            {
                return m_RootName;
            }
		}

		//-----------------------------------------------------------------------------
		// GetRootObject
        /// <summary>
        /// 
        /// </summary>
		protected virtual RootObject GetRootObject
		{
			get
			{
				return ( RootObject ) Root;
			}
		}

		//-----------------------------------------------------------------------------
		// DataComparer
        /// <summary>
        /// 
        /// </summary>
		public virtual IEqualityComparer<T> DataComparer
		{
			get
			{
				if ( !Root.IsTree ) 
                    throw new InvalidOperationException( "This is not a Tree" );

				return GetRootObject.DataComparer;
			}

			set
			{
				if ( !Root.IsTree ) 
                    throw new InvalidOperationException( "This is not a Tree" );

				GetRootObject.DataComparer = value;
			}
		}

		//-----------------------------------------------------------------------------
		// Version
        /// <summary>
        /// 
        /// </summary>
		protected virtual int Version
		{
			get
			{
				INode<T> root = Root;

				if ( !root.IsTree ) 
                    throw new InvalidOperationException( "This is not a Tree" );

				return GetNodeTree( root ).Version;
			}

			set
			{
				INode<T> root = Root;

				if ( !root.IsTree ) 
                    throw new InvalidOperationException( "This is not a Tree" );

				GetNodeTree( root ).Version = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
		protected bool HasChanged( int version ) { return ( Version != version ); }
        /// <summary>
        /// 
        /// </summary>
		protected void IncrementVersion()
		{
			INode<T> root = Root;

			if ( !root.IsTree ) 
                throw new InvalidOperationException( "This is not a Tree" );

			GetNodeTree( root ).Version++;
		}

		//-----------------------------------------------------------------------------
		// INode<T>
        /// <summary>
        /// 
        /// </summary>
		public T Data
		{
			get
			{
				return _Data;
			}

			set
			{
				if ( IsRoot ) 
                    throw new InvalidOperationException( "This is a Root" );

				OnSetting( this, value );

				_Data = value;

				OnSetDone( this, value );
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Parent { get { return _Parent; } }
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Previous { get { return _Previous; } }
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Next { get { return _Next; } }
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Child { get { return _Child; } }

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
		public ITree<T> Tree
		{
			get
			{
				return ( ITree<T> ) Root;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Root
		{
			get
			{
				INode<T> node = this;

				while ( node.Parent != null )
					node = node.Parent;

				return node;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Top
		{
			get
			{
				if ( !Root.IsTree ) 
                    throw new InvalidOperationException( "This is not a tree" );
				//if ( this.IsRoot ) throw new InvalidOperationException( "This is a Root" );
				if ( this.IsRoot ) 
                    return null;

				INode<T> node = this;

				while ( node.Parent.Parent != null )
					node = node.Parent;

				return node;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public INode<T> First
		{
			get
			{
				INode<T> node = this;

				while ( node.Previous != null )
					node = node.Previous;

				return node;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public INode<T> Last
		{
			get
			{
				INode<T> node = this;

				while ( node.Next != null )
					node = node.Next;

				return node;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public INode<T> LastChild
		{
			get
			{
				if ( Child == null ) 
                    return null;
				return Child.Last;
			}
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
		public bool HasPrevious { get { return Previous != null; } }
        /// <summary>
        /// 
        /// </summary>
		public bool HasNext { get { return Next != null; } }
        /// <summary>
        /// 
        /// </summary>
		public bool HasChild { get { return Child != null; } }
        /// <summary>
        /// 
        /// </summary>
		public bool IsFirst { get { return Previous == null; } }
        /// <summary>
        /// 
        /// </summary>
		public bool IsLast { get { return Next == null; } }
        /// <summary>
        /// 
        /// </summary>
		public bool IsTree
		{
			get
			{
				if ( !IsRoot ) 
                    return false;
				return this is RootObject;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool IsRoot
		{
			get
			{
				bool b = ( Parent == null );

				if ( b )
				{
					Debug.Assert( Previous == null );
					Debug.Assert( Next == null );
				}

				return b;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool HasParent
		{
			get
			{
				if ( IsRoot ) 
                    return false;
				return Parent.Parent != null;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool IsTop
		{
			get
			{
				if ( IsRoot ) 
                    return false;
				return Parent.Parent == null;
			}
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public virtual INode<T> this[ T item ]
		{
			get
			{
				if ( !Root.IsTree ) 
                    throw new InvalidOperationException( "This is not a tree" );

				IEqualityComparer<T> comparer = DataComparer;

				foreach ( INode<T> n in All.Nodes )
					if ( comparer.Equals( n.Data, item ) )
						return n;

				return null;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public virtual bool Contains( INode<T> item )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			return All.Nodes.Contains( item );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public virtual bool Contains( T item )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			return All.Values.Contains( item );
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public INode<T> InsertPrevious( T o )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> newNode = CreateNode();
			newNode._Data = o;

			this.InsertPreviousCore( newNode );

			return newNode;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public INode<T> InsertNext( T o )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> newNode = CreateNode();
			newNode._Data = o;

			this.InsertNextCore( newNode );

			return newNode;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public INode<T> InsertChild( T o )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> newNode = CreateNode();
			newNode._Data = o;

			this.InsertChildCore( newNode );

			return newNode;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public INode<T> Add( T o )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			return this.Last.InsertNext( o );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public INode<T> AddChild( T o )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			if ( Child == null )
				return InsertChild( o );
			else
				return Child.Add( o );
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		public void InsertPrevious( ITree<T> tree )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> newTree = GetNodeTree( tree );

			if ( !newTree.IsRoot ) 
                throw new ArgumentException( "Tree is not a Root" );
			if ( !newTree.IsTree ) 
                throw new ArgumentException( "Tree is not a tree" );

			for ( INode<T> n = newTree.Child ; n != null ; n = n.Next )
			{
				NodeTree<T> node = GetNodeTree( n );
				NodeTree<T> copy = node.CopyCore();
				InsertPreviousCore( copy );
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		public void InsertNext( ITree<T> tree )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> newTree = GetNodeTree( tree );

			if ( !newTree.IsRoot ) 
                throw new ArgumentException( "Tree is not a Root" );
			if ( !newTree.IsTree ) 
                throw new ArgumentException( "Tree is not a tree" );

			for ( INode<T> n = newTree.LastChild ; n != null ; n = n.Previous )
			{
				NodeTree<T> node = GetNodeTree( n );
				NodeTree<T> copy = node.CopyCore();
				InsertNextCore( copy );
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		public void InsertChild( ITree<T> tree )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> newTree = GetNodeTree( tree );

			if ( !newTree.IsRoot ) 
                throw new ArgumentException( "Tree is not a Root" );
			if ( !newTree.IsTree ) 
                throw new ArgumentException( "Tree is not a tree" );

			for ( INode<T> n = newTree.LastChild ; n != null ; n = n.Previous )
			{
				NodeTree<T> node = GetNodeTree( n );
				NodeTree<T> copy = node.CopyCore();
				InsertChildCore( copy );
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		public void Add( ITree<T> tree )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			this.Last.InsertNext( tree );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
		public void AddChild( ITree<T> tree )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			if ( this.Child == null )
				this.InsertChild( tree );
			else
				this.Child.Add( tree );
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newINode"></param>
		protected virtual void InsertPreviousCore( INode<T> newINode )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !newINode.IsRoot ) 
                throw new ArgumentException( "Node is not a Root" );
			if ( newINode.IsTree ) 
                throw new ArgumentException( "Node is a tree" );

			IncrementVersion();

			OnInserting( this, NodeTreeInsertOperation.Previous, newINode );

			NodeTree<T> newNode = GetNodeTree( newINode );

			newNode._Parent = this._Parent;
			newNode._Previous = this._Previous;
			newNode._Next = this;
			this._Previous = newNode;

			if ( newNode.Previous != null )
			{
				NodeTree<T> Previous = GetNodeTree( newNode.Previous );
				Previous._Next = newNode;
			}
			else // this is a first node
			{
				NodeTree<T> Parent = GetNodeTree( newNode.Parent );
				Parent._Child = newNode;
			}

			OnInserted( this, NodeTreeInsertOperation.Previous, newINode );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newINode"></param>
		protected virtual void InsertNextCore( INode<T> newINode )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !newINode.IsRoot ) 
                throw new ArgumentException( "Node is not a Root" );
			if ( newINode.IsTree ) 
                throw new ArgumentException( "Node is a tree" );

			IncrementVersion();

			OnInserting( this, NodeTreeInsertOperation.Next, newINode );

			NodeTree<T> newNode = GetNodeTree( newINode );

			newNode._Parent = this._Parent;
			newNode._Previous = this;
			newNode._Next = this._Next;
			this._Next = newNode;

			if ( newNode.Next != null )
			{
				NodeTree<T> Next = GetNodeTree( newNode.Next );
				Next._Previous = newNode;
			}

			OnInserted( this, NodeTreeInsertOperation.Next, newINode );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newINode"></param>
		protected virtual void InsertChildCore( INode<T> newINode )
		{
			if ( !newINode.IsRoot ) 
                throw new ArgumentException( "Node is not a Root" );
			if ( newINode.IsTree ) 
                throw new ArgumentException( "Node is a tree" );

			IncrementVersion();

			OnInserting( this, NodeTreeInsertOperation.Child, newINode );

			NodeTree<T> newNode = GetNodeTree( newINode );

			newNode._Parent = this;
			newNode._Next = this._Child;
			this._Child = newNode;

			if ( newNode.Next != null )
			{
				NodeTree<T> Next = GetNodeTree( newNode.Next );
				Next._Previous = newNode;
			}

			OnInserted( this, NodeTreeInsertOperation.Child, newINode );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newINode"></param>
		protected virtual void AddCore( INode<T> newINode )
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );

			NodeTree<T> lastNode = GetNodeTree( Last );

			lastNode.InsertNextCore( newINode );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newINode"></param>
		protected virtual void AddChildCore( INode<T> newINode )
		{
			if ( this.Child == null )
				this.InsertChildCore( newINode );
			else
			{
				NodeTree<T> childNode = GetNodeTree( Child );

				childNode.AddCore( newINode );
			}
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public ITree<T> Cut( T o )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			INode<T> n = this[ o ];
			if ( n == null ) return null;
			return n.Cut();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public ITree<T> Copy( T o )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			INode<T> n = this[ o ];
			if ( n == null ) return null;
			return n.Copy();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public ITree<T> DeepCopy( T o )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			INode<T> n = this[ o ];
			if ( n == null ) return null;
			return n.DeepCopy();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
		public bool Remove( T o )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			INode<T> n = this[ o ];
			if ( n == null ) 
                return false;

			n.Remove();
			return true;
		}

		//-----------------------------------------------------------------------------

		private NodeTree<T> BoxInTree( NodeTree<T> node )
		{
			if ( !node.IsRoot ) 
                throw new ArgumentException( "Node is not a Root" );
			if ( node.IsTree ) 
                throw new ArgumentException( "Node is a tree" );

			NodeTree<T> tree = CreateTree();

			tree.AddChildCore( node );

			return tree;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public ITree<T> Cut()
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			NodeTree<T> node = CutCore();

			return BoxInTree( node );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public ITree<T> Copy()
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			if ( IsTree )
			{
				NodeTree<T> NewTree = CopyCore();

				return NewTree;
			}
			else
			{
				NodeTree<T> NewNode = CopyCore();

				return BoxInTree( NewNode );
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public ITree<T> DeepCopy()
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			if ( IsTree )
			{
				NodeTree<T> NewTree = DeepCopyCore();

				return NewTree;
			}
			else
			{
				NodeTree<T> NewNode = DeepCopyCore();

				return BoxInTree( NewNode );
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public void Remove()
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			RemoveCore();
		}

		//-----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected virtual NodeTree<T> CutCore()
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );

			IncrementVersion();

			INode<T> OldRoot = Root;

			if ( this._Next != null )
				this._Next._Previous = this._Previous;

			if ( this.Previous != null )
				this._Previous._Next = this._Next;
			else // this is a first node
			{
				Debug.Assert( Parent.Child == this );
				this._Parent._Child = this._Next;
				Debug.Assert( this.Next == null || this.Next.Previous == null );
			}

			this._Parent = null;
			this._Previous = null;
			this._Next = null;

			return this;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected virtual NodeTree<T> CopyCore()
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );
			if ( IsRoot && !IsTree ) 
                throw new InvalidOperationException( "This is a Root" );

			if ( IsTree )
			{
				NodeTree<T> NewTree = CreateTree();

				CopyChildNodes( this, NewTree, false );

				return NewTree;
			}
			else
			{
				NodeTree<T> NewNode = CreateNode();

				NewNode._Data = Data;

				CopyChildNodes( this, NewNode, false );

				return NewNode;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected virtual NodeTree<T> DeepCopyCore()
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );
			if ( IsRoot && !IsTree ) 
                throw new InvalidOperationException( "This is a Root" );

			if ( IsTree )
			{
				NodeTree<T> NewTree = CreateTree();

				CopyChildNodes( this, NewTree, true );

				return NewTree;
			}
			else
			{
				NodeTree<T> NewNode = CreateNode();

				NewNode._Data = DeepCopyData( Data );

				OnDeepCopying( this, NewNode );

				CopyChildNodes( this, NewNode, true );

				OnDeepCopied( this, NewNode );

				return NewNode;
			}
		}

		private void CopyChildNodes( INode<T> oldNode, NodeTree<T> newNode, bool bDeepCopy )
		{
			NodeTree<T> previousNewChildNode = null;

			for ( INode<T> oldChildNode = oldNode.Child ; oldChildNode != null ; oldChildNode = oldChildNode.Next )
			{
				NodeTree<T> newChildNode = CreateNode();

				if ( !bDeepCopy )
					newChildNode._Data = oldChildNode.Data;
				else
					newChildNode._Data = DeepCopyData( oldChildNode.Data );

				if ( oldChildNode.Previous == null ) newNode._Child = newChildNode;

				newChildNode._Parent = newNode;
				newChildNode._Previous = previousNewChildNode;
				if ( previousNewChildNode != null ) previousNewChildNode._Next = newChildNode;

				CopyChildNodes( oldChildNode, newChildNode, bDeepCopy );

				previousNewChildNode = newChildNode;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		protected virtual void RemoveCore()
		{
			if ( this.IsRoot ) 
                throw new InvalidOperationException( "This is a Root" );

			CutCore();
		}
        /// <summary>
        /// 
        /// </summary>
		public bool CanMoveToParent
		{
			get
			{
				if ( this.IsRoot ) 
                    return false;
				if ( this.IsTop ) 
                    return false;

				return true;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool CanMoveToPrevious
		{
			get
			{
				if ( this.IsRoot ) 
                    return false;
				if ( this.IsFirst ) 
                    return false;

				return true;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool CanMoveToNext
		{
			get
			{
				if ( this.IsRoot ) 
                    return false;
				if ( this.IsLast ) 
                    return false;

				return true;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool CanMoveToChild
		{
			get
			{
				if ( this.IsRoot ) 
                    return false;
				if ( this.IsFirst ) 
                    return false;

				return true;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool CanMoveToFirst
		{
			get
			{
				if ( this.IsRoot ) 
                    return false;
				if ( this.IsFirst ) 
                    return false;

				return true;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public bool CanMoveToLast
		{
			get
			{
				if ( this.IsRoot ) 
                    return false;
				if ( this.IsLast ) 
                    return false;

				return true;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public void MoveToParent()
		{
			if ( !CanMoveToParent ) 
                throw new InvalidOperationException( "Cannot move to Parent" );

			NodeTree<T> parentNode = GetNodeTree( this.Parent );

			NodeTree<T> thisNode = this.CutCore();

			parentNode.InsertNextCore( thisNode );
		}
        /// <summary>
        /// 
        /// </summary>
		public void MoveToPrevious()
		{
			if ( !CanMoveToPrevious ) 
                throw new InvalidOperationException( "Cannot move to Previous" );

			NodeTree<T> previousNode = GetNodeTree( this.Previous );

			NodeTree<T> thisNode = this.CutCore();

			previousNode.InsertPreviousCore( thisNode );
		}
        /// <summary>
        /// 
        /// </summary>
		public void MoveToNext()
		{
			if ( !CanMoveToNext ) 
                throw new InvalidOperationException( "Cannot move to Next" );

			NodeTree<T> nextNode = GetNodeTree( this.Next );

			NodeTree<T> thisNode = this.CutCore();

			nextNode.InsertNextCore( thisNode );
		}
        /// <summary>
        /// 
        /// </summary>
		public void MoveToChild()
		{
			if ( !CanMoveToChild ) 
                throw new InvalidOperationException( "Cannot move to Child" );

			NodeTree<T> previousNode = GetNodeTree( this.Previous );

			NodeTree<T> thisNode = this.CutCore();

			previousNode.AddChildCore( thisNode );
		}
        /// <summary>
        /// 
        /// </summary>
		public void MoveToFirst()
		{
			if ( !CanMoveToFirst ) 
                throw new InvalidOperationException( "Cannot move to first" );

			NodeTree<T> firstNode = GetNodeTree( this.First );

			NodeTree<T> thisNode = this.CutCore();

			firstNode.InsertPreviousCore( thisNode );
		}
        /// <summary>
        /// 
        /// </summary>
		public void MoveToLast()
		{
			if ( !CanMoveToLast ) 
                throw new InvalidOperationException( "Cannot move to last" );

			NodeTree<T> lastNode = GetNodeTree( this.Last );

			NodeTree<T> thisNode = this.CutCore();

			lastNode.InsertNextCore( thisNode );
		}

		//-----------------------------------------------------------------------------
		// Enumerators
        /// <summary>
        /// 
        /// </summary>
		public virtual IEnumerableCollection<INode<T>> Nodes
		{
			get
			{
				return All.Nodes;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public virtual IEnumerableCollection<T> Values
		{
			get
			{
				return All.Values;
			}
		}

		//-----------------------------------------------------------------------------
		// BaseEnumerableCollectionPair
        /// <summary>
        /// 
        /// </summary>
		protected abstract class BaseEnumerableCollectionPair : IEnumerableCollectionPair<T>
		{
			private NodeTree<T> _Root = null;
            /// <summary>
            /// 
            /// </summary>
			protected NodeTree<T> Root
			{
				get { return _Root; }
				set { _Root = value; }
			}
            /// <summary>
            /// 
            /// </summary>
            /// <param name="root"></param>
			protected BaseEnumerableCollectionPair( NodeTree<T> root )
			{
				_Root = root;
			}

			/// <summary>
			/// 
			/// </summary>
			public abstract IEnumerableCollection<INode<T>> Nodes { get; }
            /// <summary>
            /// 
            /// </summary>
			protected abstract class BaseNodesEnumerableCollection : IEnumerableCollection<INode<T>>, IEnumerator<INode<T>>
			{
				private int _Version = 0;
				private object _SyncRoot = new object();

				private NodeTree<T> _Root = null;
                /// <summary>
                /// 
                /// </summary>
				protected NodeTree<T> Root
				{
					get { return _Root; }
					set { _Root = value; }
				}

				private INode<T> _CurrentNode = null;
                /// <summary>
                /// 
                /// </summary>
				protected INode<T> CurrentNode
				{
					get { return _CurrentNode; }
					set { _CurrentNode = value; }
				}

				private bool _BeforeFirst = true;
                /// <summary>
                /// 
                /// </summary>
				protected bool BeforeFirst
				{
					get { return _BeforeFirst; }
					set { _BeforeFirst = value; }
				}

				private bool _AfterLast = false;
                /// <summary>
                /// 
                /// </summary>
				protected bool AfterLast
				{
					get { return _AfterLast; }
					set { _AfterLast = value; }
				}
                /// <summary>
                /// 
                /// </summary>
                /// <param name="root"></param>
				protected BaseNodesEnumerableCollection( NodeTree<T> root )
				{
					_Root = root;
					_CurrentNode = root;

					_Version = _Root.Version;
				}
                /// <summary>
                /// 
                /// </summary>
				~BaseNodesEnumerableCollection()
				{
					Dispose( false );
				}
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
				protected abstract BaseNodesEnumerableCollection CreateCopy();
                /// <summary>
                /// 
                /// </summary>
				protected virtual bool HasChanged { get { return _Root.HasChanged( _Version ); } }

				// IDisposable
                /// <summary>
                /// 
                /// </summary>
				public void Dispose()
				{
					Dispose( true );

					GC.SuppressFinalize( this );
				}
                /// <summary>
                /// 
                /// </summary>
                /// <param name="disposing"></param>
				protected virtual void Dispose( bool disposing )
				{
				}

				// IEnumerable
				IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

				// IEnumerable<INode<T>>
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
				public virtual IEnumerator<INode<T>> GetEnumerator()
				{
					return this;
				}

				// ICollection
                /// <summary>
                /// 
                /// </summary>
				public virtual int Count
				{
					get
					{
						BaseNodesEnumerableCollection e = CreateCopy();

						int i = 0;
						foreach ( INode<T> o in e ) 
                            i++;
						return i;
					}
				}
                /// <summary>
                /// 
                /// </summary>
				public virtual bool IsSynchronized { get { return false; } }
                /// <summary>
                /// 
                /// </summary>
				public virtual object SyncRoot { get { return _SyncRoot; } }

				void ICollection.CopyTo( Array array, int index )
				{
					if ( array == null ) 
                        throw new ArgumentNullException( "array" );
					if ( array.Rank > 1 ) 
                        throw new ArgumentException( "array is multidimensional", "array" );
					if ( index < 0 ) 
                        throw new ArgumentOutOfRangeException( "index" );

					int count = Count;

					if ( count > 0 )
						if ( index >= array.Length ) 
                            throw new ArgumentException( "index is out of bounds", "index" );

					if ( index + count > array.Length ) 
                        throw new ArgumentException( "Not enough space in array", "array" );

					BaseNodesEnumerableCollection e = CreateCopy();

					foreach ( INode<T> n in e )
						array.SetValue( n, index++ );
				}
                /// <summary>
                /// 
                /// </summary>
                /// <param name="array"></param>
                /// <param name="index"></param>
				public virtual void CopyTo( T[] array, int index )
				{
					( ( ICollection ) this ).CopyTo( array, index );
				}

				/// <summary>
				/// 
				/// </summary>
				/// <param name="item"></param>
				/// <returns></returns>
				public virtual bool Contains( INode<T> item )
				{
					BaseNodesEnumerableCollection e = CreateCopy();

					IEqualityComparer<INode<T>> comparer = EqualityComparer<INode<T>>.Default;

					foreach ( INode<T> n in e )
						if ( comparer.Equals( n, item ) )
							return true;

					return false;
				}

				// IEnumerator
				object IEnumerator.Current
				{
					get { return Current; }
				}

				/// <summary>
				/// 
				/// </summary>
				public virtual void Reset()
				{
					if ( HasChanged ) 
                        throw new InvalidOperationException( "Tree has been modified." );

					_CurrentNode = _Root;

					_BeforeFirst = true;
					_AfterLast = false;
				}
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
				public virtual bool MoveNext()
				{
					if ( HasChanged ) 
                        throw new InvalidOperationException( "Tree has been modified." );

					_BeforeFirst = false;

					return true;
				}
                /// <summary>
                /// 
                /// </summary>
				public virtual INode<T> Current
				{
					get
					{
						if ( _BeforeFirst ) 
                            throw new InvalidOperationException( "Enumeration has not started." );
						if ( _AfterLast ) 
                            throw new InvalidOperationException( "Enumeration has finished." );

						return _CurrentNode;
					}
				}

			}

			/// <summary>
			/// 
			/// </summary>
			public virtual IEnumerableCollection<T> Values
			{
				get{return new ValuesEnumerableCollection( _Root.DataComparer, this.Nodes );}
			}

			private class ValuesEnumerableCollection : IEnumerableCollection<T>, IEnumerator<T>
			{
				IEqualityComparer<T> _DataComparer;
				private IEnumerableCollection<INode<T>> _Nodes;
				private IEnumerator<INode<T>> _Enumerator;

				public ValuesEnumerableCollection( IEqualityComparer<T> dataComparer, IEnumerableCollection<INode<T>> nodes )
				{
					_DataComparer = dataComparer;
					_Nodes = nodes;
					_Enumerator = _Nodes.GetEnumerator();
				}

				protected ValuesEnumerableCollection( ValuesEnumerableCollection o )
				{
					_Nodes = o._Nodes;
					_Enumerator = _Nodes.GetEnumerator();
				}

				protected virtual ValuesEnumerableCollection CreateCopy()
				{
					return new ValuesEnumerableCollection( this );
				}

				// IDisposable
				~ValuesEnumerableCollection()
				{
					Dispose( false );
				}

				public void Dispose()
				{
					Dispose( true );

					GC.SuppressFinalize( this );
				}

				protected virtual void Dispose( bool disposing )
				{
				}

				// IEnumerable
				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}

				// IEnumerable<T>
				public virtual IEnumerator<T> GetEnumerator()
				{
					return this;
				}

				// ICollection
				public virtual int Count
				{
					get{return _Nodes.Count;}
				}

				public virtual bool IsSynchronized { get { return false; } }

				public virtual object SyncRoot { get { return _Nodes.SyncRoot; } }

				public virtual void CopyTo( Array array, int index )
				{
					if ( array == null ) 
                        throw new ArgumentNullException( "array" );
					if ( array.Rank > 1 ) 
                        throw new ArgumentException( "array is multidimensional", "array" );
					if ( index < 0 ) 
                        throw new ArgumentOutOfRangeException( "index" );

					int count = Count;

					if ( count > 0 )
						if ( index >= array.Length ) 
                            throw new ArgumentException( "index is out of bounds", "index" );

					if ( index + count > array.Length )
                        throw new ArgumentException( "Not enough space in array", "array" );

					ValuesEnumerableCollection e = CreateCopy();

					foreach ( T n in e )
						array.SetValue( n, index++ );
				}

				// IEnumerableCollection<T>
				public virtual bool Contains( T item )
				{
					ValuesEnumerableCollection e = CreateCopy();

					foreach ( T n in e )
						if ( _DataComparer.Equals( n, item ) )
							return true;

					return false;
				}

				// IEnumerator
				object IEnumerator.Current
				{
					get { return Current; }
				}

				// IEnumerator<T> Members
				public virtual void Reset()
				{
					_Enumerator.Reset();
				}

				public virtual bool MoveNext()
				{
					return _Enumerator.MoveNext();
				}

				public virtual T Current
				{
					get
					{
						if ( _Enumerator == null ) 
                        { 
                            Debug.Assert( false ); 
                            return default( T ); 
                        }
						if ( _Enumerator.Current == null ) 
                        { 
                            Debug.Assert( false ); 
                            return default( T ); 
                        }

						return _Enumerator.Current.Data;
					}
				}
			}
		}

        //-----------------------------------------------------------------------------
        // AllLeafsEnumerator
        /// <summary>
        /// A node of degree zero has no subtrees. Such a node is called a leaf.
        /// </summary>
        public IEnumerableCollectionPair<T> AllLeafs
        {
            get{return new AllLeafsEnumerator(this);}
        }
        /// <summary>
        /// 
        /// </summary>
        protected class AllLeafsEnumerator : BaseEnumerableCollectionPair
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="root"></param>
            public AllLeafsEnumerator(NodeTree<T> root) : base(root) { }
            /// <summary>
            /// 
            /// </summary>
            public override IEnumerableCollection<INode<T>> Nodes
            {
                get { return new NodesEnumerableCollection(Root); }
            }
            /// <summary>
            /// 
            /// </summary>
            protected class NodesEnumerableCollection : BaseNodesEnumerableCollection
            {
                private bool _First = true;
                private int listIndex = 0;
                private List<INode<T>> list = new List<INode<T>>();
                private void CreateLeafList(INode<T> root)
                {
                    //if ( root.Data != null )
                    //    Console.WriteLine(root.Data.ToString());
                    if (root.HasChild)
                    {
                        foreach (INode<T> node in root.DirectChildren.Nodes)
                        {
                            if (node.HasChild)
                                CreateLeafList(node);
                            else
                            {
                                //if (node.Data != null)
                                //    Console.WriteLine("   add " + node.Data.ToString());
                                list.Add(node);
                            }
                        }
                    }
                    else
                    {
                        //if (root.Data != null)
                        //    Console.WriteLine("   add " + root.Data.ToString());
                        list.Add(root);
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="root"></param>
                public NodesEnumerableCollection(NodeTree<T> root) : base(root) 
                {
                    CreateLeafList(root);
                    listIndex = 0;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="o"></param>
                protected NodesEnumerableCollection(NodesEnumerableCollection o) : base(o.Root) 
                {
                    CreateLeafList(o.Root);
                    listIndex = 0;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                protected override BaseNodesEnumerableCollection CreateCopy()
                {
                    return new NodesEnumerableCollection(this);
                }
                /// <summary>
                /// 
                /// </summary>
                public override void Reset()
                {
                    list.Clear();
                    listIndex = 0;
                    base.Reset();
                    _First = true;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                public override bool MoveNext()
                {
                    if (!base.MoveNext())
                        return false;

                    if ( listIndex >= 0 && listIndex < list.Count )
                        CurrentNode = list[listIndex];
                    else
                        return false;

                    listIndex++;

                    if (_First)
                    {
                        _First = false;
                        return true;
                    }

                    return true;
                }
            }

        }

		//-----------------------------------------------------------------------------
		// AllEnumerator
        /// <summary>
        /// 
        /// </summary>
		public IEnumerableCollectionPair<T> All 
        { 
            get{ return new AllEnumerator( this ); } 
        }
        /// <summary>
        /// 
        /// </summary>
		protected class AllEnumerator : BaseEnumerableCollectionPair
		{
            /// <summary>
            /// 
            /// </summary>
            /// <param name="root"></param>
			public AllEnumerator( NodeTree<T> root ) : base( root ) { }
            /// <summary>
            /// 
            /// </summary>
			public override IEnumerableCollection<INode<T>> Nodes
			{
				get{return new NodesEnumerableCollection( Root );}
			}
            /// <summary>
            /// 
            /// </summary>
			protected class NodesEnumerableCollection : BaseNodesEnumerableCollection
			{
				private bool _First = true;
                /// <summary>
                /// 
                /// </summary>
                /// <param name="root"></param>
				public NodesEnumerableCollection( NodeTree<T> root ) : base( root ) { }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="o"></param>
				protected NodesEnumerableCollection( NodesEnumerableCollection o ) : base( o.Root ) { }
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
				protected override BaseNodesEnumerableCollection CreateCopy()
				{
					return new NodesEnumerableCollection( this );
				}
                /// <summary>
                /// 
                /// </summary>
				public override void Reset()
				{
					base.Reset();

					_First = true;
				}
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
				public override bool MoveNext()
				{
					if ( !base.MoveNext() ) 
                        goto hell;

					if ( CurrentNode == null ) 
                        throw new InvalidOperationException( "Current is null" );

					if ( CurrentNode.IsRoot )
					{
						CurrentNode = CurrentNode.Child;
						if ( CurrentNode == null ) 
                            goto hell;
					}

					if ( _First ) 
                    { 
                        _First = false; 
                        return true; 
                    }

					if ( CurrentNode.Child != null ) 
                    { 
                        CurrentNode = CurrentNode.Child; 
                        return true; 
                    }

					for ( ; CurrentNode.Parent != null ; CurrentNode = CurrentNode.Parent )
					{
						if ( CurrentNode == Root ) 
                            goto hell;
						if ( CurrentNode.Next != null ) 
                        { 
                            CurrentNode = CurrentNode.Next; 
                            return true; 
                        }
					}

				hell:

					AfterLast = true;
					return false;
				}
			}

		}

		//-----------------------------------------------------------------------------
		// AllChildrenEnumerator
        /// <summary>
        /// 
        /// </summary>
		public IEnumerableCollectionPair<T> AllChildren 
        { 
            get { return new AllChildrenEnumerator( this ); } 
        }

		private class AllChildrenEnumerator : BaseEnumerableCollectionPair
		{
			public AllChildrenEnumerator( NodeTree<T> root ) : base( root ) 
            { 
            }

			public override IEnumerableCollection<INode<T>> Nodes
			{
				get{return new NodesEnumerableCollection( Root );}
			}

			protected class NodesEnumerableCollection : BaseNodesEnumerableCollection
			{
				public NodesEnumerableCollection( NodeTree<T> root ) : base( root ) { }

				protected NodesEnumerableCollection( NodesEnumerableCollection o ) : base( o.Root ) { }

				protected override BaseNodesEnumerableCollection CreateCopy()
				{
					return new NodesEnumerableCollection( this );
				}

				public override bool MoveNext()
				{
					if ( !base.MoveNext() ) 
                        goto hell;

					if ( CurrentNode == null ) 
                        throw new InvalidOperationException( "Current is null" );

					if ( CurrentNode.Child != null ) 
                    { 
                        CurrentNode = CurrentNode.Child; 
                        return true; 
                    }

					for ( ; CurrentNode.Parent != null ; CurrentNode = CurrentNode.Parent )
					{
						if ( CurrentNode == Root ) 
                            goto hell;
						if ( CurrentNode.Next != null ) 
                        { 
                            CurrentNode = CurrentNode.Next; 
                            return true; 
                        }
					}

				hell:

					AfterLast = true;
					return false;
				}
			}
		}

		//-----------------------------------------------------------------------------
		// DirectChildrenEnumerator
        /// <summary>
        /// 
        /// </summary>
		public IEnumerableCollectionPair<T> DirectChildren 
        { 
            get { return new DirectChildrenEnumerator( this );} 
        }

		private class DirectChildrenEnumerator : BaseEnumerableCollectionPair
		{
			public DirectChildrenEnumerator( NodeTree<T> root ) : base( root ) { }

			public override IEnumerableCollection<INode<T>> Nodes
			{
				get{return new NodesEnumerableCollection( Root );}
			}

			protected class NodesEnumerableCollection : BaseNodesEnumerableCollection
			{
				public NodesEnumerableCollection( NodeTree<T> root ) : base( root ) { }

				protected NodesEnumerableCollection( NodesEnumerableCollection o ) : base( o.Root ) { }

				protected override BaseNodesEnumerableCollection CreateCopy()
				{
					return new NodesEnumerableCollection( this );
				}

				public override int Count
				{
					get{return Root.DirectChildCount;}
				}

				public override bool MoveNext()
				{
					if ( !base.MoveNext() ) 
                        goto hell;

					if ( CurrentNode == null ) 
                        throw new InvalidOperationException( "Current is null" );

					if ( CurrentNode == Root )
						CurrentNode = Root.Child;
					else
						CurrentNode = CurrentNode.Next;

					if ( CurrentNode != null ) return true;

				hell:

					AfterLast = true;
					return false;
				}
			}
		}

		//-----------------------------------------------------------------------------
		// DirectChildrenInReverseEnumerator
        /// <summary>
        /// 
        /// </summary>
		public IEnumerableCollectionPair<T> DirectChildrenInReverse { get { return new DirectChildrenInReverseEnumerator( this ); } }

		private class DirectChildrenInReverseEnumerator : BaseEnumerableCollectionPair
		{
			public DirectChildrenInReverseEnumerator( NodeTree<T> root ) : base( root ) { }

			public override IEnumerableCollection<INode<T>> Nodes
			{
				get
				{
					return new NodesEnumerableCollection( Root );
				}
			}

			protected class NodesEnumerableCollection : BaseNodesEnumerableCollection
			{
				public NodesEnumerableCollection( NodeTree<T> root ) : base( root ) { }

				protected NodesEnumerableCollection( NodesEnumerableCollection o ) : base( o.Root ) { }

				protected override BaseNodesEnumerableCollection CreateCopy()
				{
					return new NodesEnumerableCollection( this );
				}

				public override int Count
				{
					get
					{
						return Root.DirectChildCount;
					}
				}

				public override bool MoveNext()
				{
					if ( !base.MoveNext() ) 
                        goto hell;

					if ( CurrentNode == null ) 
                        throw new InvalidOperationException( "Current is null" );

					if ( CurrentNode == Root )
						CurrentNode = Root.LastChild;
					else
						CurrentNode = CurrentNode.Previous;

					if ( CurrentNode != null ) 
                        return true;

				hell:

					AfterLast = true;
					return false;
				}
			}
		}

		//-----------------------------------------------------------------------------
		// DirectChildCount
        /// <summary>
        /// 
        /// </summary>
		public int DirectChildCount
		{
			get
			{
				int i = 0;

				for ( INode<T> n = this.Child ; n != null ; n = n.Next ) 
                    i++;

				return i;
			}
		}

		//-----------------------------------------------------------------------------
		// ITree<T>
        /// <summary>
        /// 
        /// </summary>
		public virtual Type DataType
		{
			get
			{
				return typeof( T );
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public void Clear()
		{
			if ( !this.IsRoot ) 
                throw new InvalidOperationException( "This is not a Root" );
			if ( !this.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );

			_Child = null;
		}

		//-----------------------------------------------------------------------------
		// GetNodeTree
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
		protected static NodeTree<T> GetNodeTree( ITree<T> tree )
		{
			if ( tree == null ) 
                throw new ArgumentNullException( "Tree is null" );

			return ( NodeTree<T> ) tree; // can throw an InvalidCastException.
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
		protected static NodeTree<T> GetNodeTree( INode<T> node )
		{
			if ( node == null ) 
                throw new ArgumentNullException( "Node is null" );

			return ( NodeTree<T> ) node; // can throw an InvalidCastException.
		}

		//-----------------------------------------------------------------------------
		// ICollection
        /// <summary>
        /// 
        /// </summary>
		public virtual int Count
		{
			get
			{
				int i = IsRoot ? 0 : 1;

				for ( INode<T> n = this.Child ; n != null ; n = n.Next )
					i += n.Count;

				return i;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public virtual bool IsReadOnly { get { return false; } }
		
		//-----------------------------------------------------------------------------
		// Validate
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
		protected virtual void OnValidate( INode<T> node, T data )
		{
			if ( !Root.IsTree ) 
                throw new InvalidOperationException( "This is not a tree" );
			if ( data is INode<T> ) 
                throw new ArgumentException( "Object is a node" );

			if ( ( !typeof( T ).IsClass ) || ( ( object ) data ) != null )
				if ( !DataType.IsInstanceOfType( data ) )
					throw new ArgumentException( "Object is not a " + DataType.Name );

			if ( !IsRoot ) 
                GetNodeTree( Root ).OnValidate( node, data );
		}
		//-----------------------------------------------------------------------------
		// Set
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
		protected virtual void OnSetting( INode<T> node, T data )
		{
			OnSettingCore( node, data, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnSettingCore( INode<T> node, T data, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnSettingCore( node, data, false );

			if ( raiseValidate ) 
                OnValidate( node, data );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
		protected virtual void OnSetDone( INode<T> node, T data )
		{
			OnSetDoneCore( node, data, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnSetDoneCore( INode<T> node, T data, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnSetDoneCore( node, data, false );
		}

		//-----------------------------------------------------------------------------
		// Insert
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="operation"></param>
        /// <param name="newNode"></param>
		protected virtual void OnInserting( INode<T> oldNode, NodeTreeInsertOperation operation, INode<T> newNode )
		{
			OnInsertingCore( oldNode, operation, newNode, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="operation"></param>
        /// <param name="newNode"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnInsertingCore( INode<T> oldNode, NodeTreeInsertOperation operation, INode<T> newNode, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnInsertingCore( oldNode, operation, newNode, false );

			if ( raiseValidate ) 
                OnValidate( oldNode, newNode.Data );

			if ( raiseValidate ) 
                OnInsertingTree( newNode );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
		protected virtual void OnInsertingTree( INode<T> newNode )
		{
			for ( INode<T> child = newNode.Child ; child != null ; child = child.Next )
			{
				OnInsertingTree( newNode, child );

				OnInsertingTree( child );
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="child"></param>
		protected virtual void OnInsertingTree( INode<T> newNode, INode<T> child )
		{
			OnInsertingTreeCore( newNode, child, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="child"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnInsertingTreeCore( INode<T> newNode, INode<T> child, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnInsertingTreeCore( newNode, child, false );

			if ( raiseValidate ) 
                OnValidate( newNode, child.Data );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="operation"></param>
        /// <param name="newNode"></param>
		protected virtual void OnInserted( INode<T> oldNode, NodeTreeInsertOperation operation, INode<T> newNode )
		{
			OnInsertedCore( oldNode, operation, newNode, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="operation"></param>
        /// <param name="newNode"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnInsertedCore( INode<T> oldNode, NodeTreeInsertOperation operation, INode<T> newNode, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnInsertedCore( oldNode, operation, newNode, false );

			if ( raiseValidate ) OnInsertedTree( newNode );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
		protected virtual void OnInsertedTree( INode<T> newNode )
		{
			for ( INode<T> child = newNode.Child ; child != null ; child = child.Next )
			{
				OnInsertedTree( newNode, child );

				OnInsertedTree( child );
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="child"></param>
		protected virtual void OnInsertedTree( INode<T> newNode, INode<T> child )
		{
			OnInsertedTreeCore( newNode, child, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="child"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnInsertedTreeCore( INode<T> newNode, INode<T> child, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnInsertedTreeCore( newNode, child, false );

		}

		//-----------------------------------------------------------------------------
		// DeepCopy
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
		protected virtual void OnDeepCopying( INode<T> oldNode, INode<T> newNode )
		{
			OnDeepCopyingCore( oldNode, newNode, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnDeepCopyingCore( INode<T> oldNode, INode<T> newNode, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnDeepCopyingCore( oldNode, newNode, false );

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
		protected virtual void OnDeepCopied( INode<T> oldNode, INode<T> newNode )
		{
			OnDeepCopiedCore( oldNode, newNode, true );
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        /// <param name="raiseValidate"></param>
		protected virtual void OnDeepCopiedCore( INode<T> oldNode, INode<T> newNode, bool raiseValidate )
		{
			if ( !IsRoot ) 
                GetNodeTree( Root ).OnDeepCopiedCore( oldNode, newNode, false );
		}
	} 
}



