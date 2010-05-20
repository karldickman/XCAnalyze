using System;

namespace xcanalyze.io {

	public interface IReader<T> {
		void Close();
		T Read();
	}
	
	public interface IWriter<T> {
		void Close();
		void Write(T toWrite);
	}
}
