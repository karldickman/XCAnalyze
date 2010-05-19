using System;

namespace xcanalyze.io {

	public interface IReader<T> {
		T Read();
		void Close();
	}
	
	public interface IWriter<T> {
		void Close();
		void Write(T toWrite);
	}
}
