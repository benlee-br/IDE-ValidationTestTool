using System;

namespace BioRad.Common
{
    /// <summary>
    /// Undo/redo Manager
    /// </summary>
    public class UndoRedoManager : IDisposable
    {
        const int MaxItems = 10;//TT438 reduce number of saved events to 10

        /// <summary>
        /// Delegate and arguments.
        /// </summary>
        public class Undoable
        {
            /// <summary>
            /// 
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public UndoableDelegate Handler { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public object[] Args { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="undo"></param>
            /// <param name="args"></param>
            public Undoable(UndoableDelegate undo, object[] args)
            {
                Text = string.Empty;
                Handler = undo;
                Args = args;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class BeforeAfterStatePair
        {
            /// <summary>
            /// 
            /// </summary>
            public Undoable BeforeState { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public Undoable AfterState { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="beforeUndoable"></param>
            /// <param name="afterUndoable"></param>
            public BeforeAfterStatePair(Undoable beforeUndoable, Undoable afterUndoable)
            {
                BeforeState = beforeUndoable;
                AfterState = afterUndoable;
            }
        }

        private CircularStack<BeforeAfterStatePair> m_UndoStack;
        private CircularStack<BeforeAfterStatePair> m_RedoStack;

        /// <summary>
        /// Delegate method that takes in object[] for parameters and returns void.
        /// </summary>
        /// <param name="args"></param>
        public delegate void UndoableDelegate(object[] args);

        /// <summary>
        /// 
        /// </summary>
        public UndoRedoManager()
        {
            m_UndoStack = new CircularStack<BeforeAfterStatePair>(MaxItems);
            m_RedoStack = new CircularStack<BeforeAfterStatePair>(MaxItems);
        }

        /// <summary>
        /// Gets number of undo actions available
        /// </summary>
        public int UndoCount
        {
            get { return m_UndoStack.Count; }
        }

        /// <summary>
        /// Gets number of redo actions available
        /// </summary>
        public int RedoCount
        {
            get { return m_RedoStack.Count; }
        }

        /// <summary>
        /// Saves before and after object state changes.
        /// </summary>
        /// <param name="beforeUndoable">Object state before changes applied.</param>
        /// <param name="afterUndoable">Object state after changes applied.</param>
        public void AddUndoable(Undoable beforeUndoable, Undoable afterUndoable)
        {
            if (beforeUndoable == null)
                throw new ArgumentNullException("fromUndoable");

            m_RedoStack.Clear();
            m_UndoStack.Push(new BeforeAfterStatePair(beforeUndoable, afterUndoable));
        }

        /// <summary>
        /// Clear all undo and redo
        /// </summary>
        public void Clear()
        {
            m_UndoStack.Clear();
            m_RedoStack.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            if (m_UndoStack.Count > 0)
            {
                BeforeAfterStatePair undoState = m_UndoStack.Pop();
                if (undoState != null)
                {
                    undoState.BeforeState.Handler(undoState.BeforeState.Args); // invoke delegate

                    m_RedoStack.Push(undoState);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            if (m_RedoStack.Count > 0)
            {
                BeforeAfterStatePair redoState = m_RedoStack.Pop();
                if (redoState != null)
                {
                    redoState.AfterState.Handler(redoState.AfterState.Args); // invoke delegate

                    m_UndoStack.Push(redoState);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (m_UndoStack != null)
            {
                while (m_UndoStack.Count > 0)
                {
                    BeforeAfterStatePair undoable = m_UndoStack.Pop();
                    if (undoable != null)
                    {
                        undoable.BeforeState.Handler = null;
                        undoable.BeforeState.Args = null;

                        undoable.AfterState.Handler = null;
                        undoable.AfterState.Args = null;
                    }
                }
                m_UndoStack.Clear();
            }
            m_UndoStack = null;

            if (m_RedoStack != null)
            {
                while (m_RedoStack.Count > 0)
                {
                    BeforeAfterStatePair undoable = m_RedoStack.Pop();
                    if (undoable != null)
                    {
                        undoable.BeforeState.Handler = null;
                        undoable.BeforeState.Args = null;

                        undoable.AfterState.Handler = null;
                        undoable.AfterState.Args = null;
                    }
                }
                m_RedoStack.Clear();
            }
            m_RedoStack = null;
        }
    }

    /// <summary>
    /// Stack with capacity, bottom items beyond the capacity are discarded.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CircularStack<T>
    {
        private T[] items; // items.Length is Capacity + 1

        private int top = 1;
        private int bottom = 0;

        /// <summary>
        /// Gets if the stack is full.
        /// </summary>
        public bool IsFull
        {
            get { return top == bottom; }
        }

        /// <summary>
        /// Gets the number of elements contained in the stack.
        /// </summary>
        public int Count
        {
            get
            {
                int count = top - bottom - 1;
                if (count < 0)
                    count += items.Length;
                return count;
            }
        }

        /// <summary>
        /// Gets the capacity of the stack.
        /// </summary>
        public int Capacity
        {
            get { return items.Length - 1; }
        }

        /// <summary>
        /// Creates stack with given capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public CircularStack(int capacity)
        {
            if (capacity < 1)
                capacity = 1;
            items = new T[capacity + 1];
        }

        /// <summary>
        /// Removes and returns the object at the top.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T removed = default(T);
            if (Count > 0)
            {
                removed = items[top];
                items[top--] = default(T);
                if (top < 0)
                    top += items.Length;
            }
            return removed;
        }

        /// <summary>
        /// Inserts an object at the top.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            if (IsFull)
            {
                bottom++;
                if (bottom >= items.Length)
                    bottom -= items.Length;
            }
            if (++top >= items.Length)
                top -= items.Length;
            items[top] = item;
        }

        /// <summary>
        /// Removes all the objects from the stack.
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = default(T);
                }
                top = 1;
                bottom = 0;
            }
        }
    }
}