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

//    public class XcaWriter : SqliteDatabaseWriter, IWriter<Data> {}
}
