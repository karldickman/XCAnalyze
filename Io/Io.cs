using System;
using XCAnalyze.Model;
using XCAnalyze.Io.Sql;

namespace XCAnalyze.Io {

	public interface IReader<T>
    {
		void Close();
		T Read();
	}
	
	public interface IWriter<T>
    {
		void Close();
		void Write(T toWrite);
	}

//    public class XcaWriter : SqliteDatabaseWriter, IWriter<Data> {}
}
