using NUnit.Framework;
using System;
using XCAnalyze.Model;
using XCAnalyze.Io.Sql;

namespace XCAnalyze.Io {

    /// <summary>
    /// An interface for classes that read data.
    /// </summary>
	public interface IReader<T>
    {
        /// <summary>
        /// Close the source from which we are reading.
        /// </summary>
		void Close();
        
        /// <summary>
        /// Read the source.
        /// </summary>
		T Read();
	}
	
    /// <summary>
    /// An interfaces for classes that write data.
    /// </summary>
	public interface IWriter<T>
    {
        /// <summary>
        /// Close the target to which we are writing.
        /// </summary>
		void Close();
        
        /// <summary>
        /// Write a value to the target.
        /// </summary>
        /// <param name="toWrite">
        /// The value to be written.
        /// </param>
		void Write(T toWrite);
	}

    /// <summary>
    /// The <see cref="IReader"/> for the default file format of XCAnalyze, .xca
    /// files.
    /// </summary>
    public class XcaReader : IReader<XcData>
    {
        /// <summary>
        /// The reader that actually does everything.
        /// </summary>
        protected internal SqliteDatabaseReader Reader { get; set; }
        
        /// <summary>
        /// Create a new reader using a particular database reader.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="SqliteDatabaseReader"/> to use.
        /// </param>
        protected internal XcaReader(SqliteDatabaseReader reader)
        {
            Reader = reader;
        }
        
        /// <summary>
        /// Create a new reader that reads from a particular file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file from which to read.
        /// </param>
        public static XcaReader NewInstance (string fileName)
        {
            return new XcaReader(SqliteDatabaseReader.NewInstance(fileName));
        }

        public void Close ()
        {
            Reader.Close ();
        }
        
        public XcData Read ()
        {
            return Reader.Read ();
        }
    }
    
    /// <summary>
    /// The <see cref="IWriter"/> for the default file format of XCAnalyze, .xca
    /// files.
    /// </summary>
    public class XcaWriter : IWriter<XcData>
    {
        /// <summary>
        /// The writer that actually does everything.
        /// </summary>
        protected internal SqliteDatabaseWriter Writer { get; set; }
               
        /// <summary>
        /// Create a new writer using a particular database writer.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="SqliteDatabaseWriter"/> to use.
        /// </param>
        protected internal XcaWriter (SqliteDatabaseWriter writer)
        {
            Writer = writer;
        }
        
        /// <summary>
        /// Create a new writer that writes to a particular file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to which to write.
        /// </param>
        public static XcaWriter NewInstance (string fileName)
        {
            return new XcaWriter(SqliteDatabaseWriter.NewInstance(fileName));
        }

        public void Close ()
        {
            Writer.Close ();
        }
        
        public void Write (XcData toWrite)
        {
            Writer.Write (toWrite);
        }
    }
    
    [TestFixture]
    public class TestXcaReader
    {
        protected internal XcaReader Reader { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Reader = XcaReader.NewInstance (SupportFiles.GetPath ("example.xca"));
        }
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Close ();
        }
        
        [Test]
        public void TestRead ()
        {
            Reader.Read ();
        }
    }
    
    [TestFixture]
    public class TextXcaWriter
    {
        protected internal XcData Data { get; set; }
        protected internal XcaWriter Writer { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            XcaReader reader;
            reader = XcaReader.NewInstance (SupportFiles.GetPath ("example.xca"));
            Data = reader.Read ();
            reader.Close ();
            Writer = XcaWriter.NewInstance (SupportFiles.GetPath ("example.xca"));
        }
        
        [TearDown]
        public void TearDown ()
        {
            Writer.Close ();
        }
        
        [Test]
        public void TestWrite ()
        {
            Writer.Write (Data);
        }
    }
}
