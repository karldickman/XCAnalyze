using System;

namespace XcAnalyze
{
	/// <summary>
	/// Some commonly used functions.
	/// </summary>
	public class Utilities
	{
		/// <summary>
		/// Compare two nullable items of the same type.
		/// </summary>
		/// <param name="nullVsValue">
		/// The expected value for Compare(null, nonNull).
		/// </param>
		public static int CompareNullable (int? item1, int? item2, int nullVsValue)
		{
			if (item1 == null && item2 == null) 
			{
				return 0;
			}
			if (item1 == null) 
			{
				return nullVsValue;
			}
			if (item2 == null) 
			{
				return -nullVsValue;
			}
			return item1.Value.CompareTo (item2);
		}
	}
}