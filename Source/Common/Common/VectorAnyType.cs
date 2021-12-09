using System;
using System.Collections.Generic;
using System.Text;

namespace BioRad.Common
{
    #region Documentation Tags
    /// <summary>
    /// Vector Class with any data type, before use it, need to implement
    /// the abstract method CreateNewObject
    /// </summary>
    /// <remarks>
    /// Remarks section for class.
    /// </remarks>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:BL</item>
    ///			<item name="review">Last design/code review:</item>
    ///			<item name="conformancereview">Conformance review:</item>
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
    ///			<item name="vssfile">$Workfile: VectorAnyType.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Common/VectorAnyType.cs $</item>
    ///			<item name="vssrevision">$Revision: 4 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Blie $</item>
    ///			<item name="vssdate">$Date: 8/11/06 2:21p $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public abstract partial class VectorAnyType<T>
    {
        #region Member Data
        /// <summary>
        /// Dictionary index table Data
        /// </summary>
        protected Dictionary<int, T> m_Data = new Dictionary<int, T>();
        #endregion

        #region Accessors
        /// <summary>
        /// Size of vector
        /// </summary>
        public int Size
        {
            get { return m_Data.Count; }
        }

        /// <summary>
        /// Size of vector
        /// </summary>
        public Dictionary<int, T> Data
        {
            get { return m_Data; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i]
        {
            get
            {
                if (i >= m_Data.Count)
                {
                    for (int index = 0; index < i + 1; index++)
                    {
                        if (!m_Data.ContainsKey(index))
                            m_Data.Add(index, CreateNewObject());
                    }
                }
                return m_Data[i];
            }

            set
            {
                m_Data[i] = value;
            }


        }
        #endregion

        #region Constructors and Destructor
        /// <summary>Default constructor.</summary>
        public VectorAnyType()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// clear method
        /// </summary>
        public void Clear()
        {
            m_Data.Clear();
        }
        /// <summary>
        /// Adds an element to the end of the vector
        /// </summary>
        /// <param name="s"></param>
        public void PushBack(T s)
        {
            int index = m_Data.Count;
            m_Data.Add(index, s);
        }

        /// <summary>
        /// Delete an element to the end of the vector
        /// </summary>
        public void PopBack()
        {
            m_Data.Remove(m_Data.Count-1);
        }
        /// <summary>
        /// Delete an element at the position index of the vector
        /// </summary>
        public void Erase(int index)
        {
            bool isFound = false;
            if (index < m_Data.Count)
            {
                m_Data.Remove(index);
                isFound = true;
            }
            Dictionary<int, T> temp = new Dictionary<int,T>();
            if (isFound)
            {
                for (int i = index + 1; i <= m_Data.Count; i++)
                {
                    temp.Add(i - 1, m_Data[i]);
                }
                if (temp.Count > 0)
                {
                    foreach (KeyValuePair<int, T> entry in temp)
                    {
                        m_Data.Remove(entry.Key + 1);
                        m_Data.Add(entry.Key, entry.Value);
                    }
                    temp.Clear();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract T CreateNewObject();
        #endregion
    }
}
