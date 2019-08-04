using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace WebTrace.Service
{
    /// <summary>
    /// Used to represent a FilePath in a Configuration object
    /// 
    /// Any FilePaths will be displayed with a special editor when displaying the configuration of a component.
    /// </summary>
    [Serializable]
    [XmlRoot("FilePath")]
    public class FilePath : BasePath
    {
        private const int CurrentVersion = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePath"/> class.  Used for XML serialization ONLY.
        /// </summary>
        public FilePath() { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="TraceLabSDK.Component.Config.FilePath"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(FilePath path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return path.Absolute;
        }

        #region IXmlSerializable Members

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (reader.LocalName == "FilePath")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/FilePath/Version");
                if (iter != null)
                {
                    long ver = iter.ValueAsLong;

                    if (ver == CurrentVersion)
                    {
                        ReadCurrentVersion(nav);
                    }
                    else if (ver == 1)
                    {
                        ReadVersion1(nav);
                    }
                    else
                    {
                        throw new NotSupportedException("Version not supported.");
                    }
                }
                else
                {
                    ReadNonVersioned(nav);
                }
            }
        }

        private void ReadNonVersioned(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("/FilePath/Value");
            if (iter == null)
                throw new XmlSchemaException("FilePath elements do not match the required elements for this version.");

            Absolute = iter.Value;
        }


        /// <summary>
        /// If read FilePath was version1 the path was relative to the dataroot, not experiment location root. It has to be transformed in post process.
        /// </summary>
        [Obsolete]
        internal bool relativeToDataRoot = false;
        private void ReadVersion1(XPathNavigator nav)
        {
            //version one is the same, but it is relative to base root of the tracelab environment
            //it should be transformed to be relative to the experiment directory

            var iter = nav.SelectSingleNode("/FilePath/Relative");
            if (iter == null)
                throw new XmlSchemaException("FilePath elements do not match the required elements for this version.");

            Relative = iter.Value;
            relativeToDataRoot = true;
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("/FilePath/Relative");
            if (iter == null)
                throw new XmlSchemaException("FilePath elements do not match the required elements for this version.");

            Relative = iter.Value;
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteElementString("Version", CurrentVersion.ToString(CultureInfo.CurrentCulture));
            writer.WriteElementString("Relative", Relative);
        }

        
        #endregion

        /// <summary>
        /// Gets or sets the string value representing the path itself.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public override string Relative
        {
            get
            {
                return base.Relative;
            }
            set
            {
                base.Relative = value;
            }
        }

        /// <summary>
        /// Gets or sets the absolute path represented by this FilePath
        /// </summary>
        /// <value>
        /// The absolute path.
        /// </value>
        public override string Absolute
        {
            get
            {
                return base.Absolute;
            }
            set
            {
                base.Absolute = value;
            }
        }

        /// <summary>
        /// Gets the data root of this FilePath.
        /// </summary>
        public override string DataRoot
        {
            get
            {
                return base.DataRoot;
            }
        }
    }
}