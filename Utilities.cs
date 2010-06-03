using System;
using System.IO;

namespace XCAnalyze
{
	/// <summary>
	/// A class used to compare nullable types.
	/// </summary>
	public class NullableComparer
	{  
        /// <summary>
        /// Compare two nullable items of the same type.
        /// </summary>
        /// <param name="nullVsValue">
        /// The expected value for Compare(null, nonNull).
        /// </param>
        public static int Compare (int? item1, int? item2, int nullVsValue)
        {
            if (item1 == null && item2 == null) {
                return 0;
            }
            if (item1 == null) {
                return nullVsValue;
            }
            if (item2 == null) 
            {
                return -nullVsValue;
            }
            return item1.Value.CompareTo (item2);
        }
	}
    
    /// <summary>
    /// A class used to compare reference types.
    /// </summary>
    public class ObjectComparer<T> where T : IComparable<T>
    {
        /// <summary>
        /// Compare two comparable items of the same type.
        /// </summary>
        /// <param name="nullVsValue">
        /// The expected value for Compare(null, nonNull).
        /// </param>
        public static int Compare (T item1, T item2, int nullVsValue)
        {    
            if (item1 == null && item2 == null) {
                return 0;
            }
            if (item1 == null) {
                return nullVsValue;
            }
            if (item2 == null) {
                return -nullVsValue;
            }
            return item1.CompareTo (item2);
        }
    }
}