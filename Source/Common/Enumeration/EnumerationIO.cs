using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Xml;

namespace BioRad.Common.Enumeration
{
    #region Documentation Tags
    /// <summary>
    /// Converts connected instrument data from XML to nonlinear tree and vice versus.
    /// </summary>
    /// <classinformation>
    ///		<list type="bullet">
    ///			<item name="authors">Authors:Ralph Ansell</item>
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
    ///			<item name="vssfile">$Workfile: EnumerationIO.cs $</item>
    ///			<item name="vssfilepath">$Archive: /Denali3/Source/Core/Common/Enumeration/EnumerationIO.cs $</item>
    ///			<item name="vssrevision">$Revision: 2 $</item>
    ///			<item name="vssauthor">Last Check-in by:$Author: Ransell $</item>
    ///			<item name="vssdate">$Date: 7/19/07 6:16a $</item>
    ///		</list>
    /// </archiveinformation>
    #endregion

    public class EnumerationIO
    {
        #region Methods
        /// <summary>
        /// Convert nonlinear structure to flat XML structure.
        /// </summary>
        /// <param name="tree">Nonlinear tree structure of connected instruments.</param>
        /// <returns>Flat XML representation of nonlinear structure.</returns>
        public static string Serialize(ITree<ConnectedInstrument> tree)
        {
            if ( tree == null )
                return string.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Length = 0;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader reader = null;
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(stringWriter);

            if (tree != null)
            {
                //string s = m_Tree.Root.ToStringRecursive();

                writer.WriteStartElement("ConnectedInstruments");
                for (INode<ConnectedInstrument> node = tree.Root.Child; node != null; node = node.Next)
                {
                    writer.WriteStartElement("node");
                    writer.WriteAttributeString("level", node.Depth.ToString());

                    writer.WriteStartElement("device");
                    reader = XmlReader.Create(
                        new System.IO.StringReader(node.Data.ToXml()), readerSettings);
                    writer.WriteNode(reader, true);
                    writer.WriteEndElement();//device

                    for (INode<ConnectedInstrument> n = node.Child; n != null; n = n.Next)
                    {
                        writer.WriteStartElement("node");
                        writer.WriteAttributeString("level", n.Depth.ToString());

                        writer.WriteStartElement("device");
                        reader = XmlReader.Create(
                            new System.IO.StringReader(n.Data.ToXml()), readerSettings);
                        writer.WriteNode(reader, true);
                        writer.WriteEndElement();//device

                        writer.WriteEndElement();//node
                    }
                    writer.WriteEndElement();//node
                }

                writer.WriteEndElement();//ConnectedInstruments
            }
            stringWriter.Flush();
            sb.Append(stringWriter.ToString());
            writer.Close();
            return sb.ToString();
        }
        /// <summary>
        /// Convert flat XML structure to nonlinear tree structure.
        /// </summary>
        /// <param name="xml">String containing flat XML connected instruments.</param>
        /// <returns>Tree representations of connected instruments.</returns>
        public static ITree<ConnectedInstrument> Deserialize(string xml)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");

            ITree<ConnectedInstrument> tree =
                NodeTree<ConnectedInstrument>.NewTree("ConnectedInstruments");
            INode<ConnectedInstrument> parentNode = null;

            string instrumentXml;
            int level = -1;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlTextReader.Create(new StringReader(xml), settings);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    string.Compare(reader.Name, "device", true) == 0)
                {
                    ConnectedInstrument newInstrument = new ConnectedInstrument();
                    instrumentXml = reader.ReadInnerXml();
                    bool ok = newInstrument.FromXml(instrumentXml);
					if (!ok)//TFS Bug 2106
						continue;
                    if (level == 0)
                        parentNode = tree.Root.AddChild(newInstrument);
                    else if (level == 1)
                        parentNode.AddChild(newInstrument);
                }
                if (reader.NodeType == XmlNodeType.Element &&
                    string.Compare(reader.Name, "node", true) == 0)
                {
                    string s = reader.GetAttribute("level");
                    level = int.Parse(s);
                }
            }

            //string stree = tree.Root.ToStringRecursive();

            return tree;
        }
        #endregion
    }
}
