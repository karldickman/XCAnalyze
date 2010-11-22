using System;

namespace XCAnalyze.Model
{
    internal class Cell<T>
    {
        #region Properties
        
        #region Fields
        
        private T _value;
        
        #endregion
        
        /// <summary>
        /// True if the value in the cell has been changed since it was loaded
        /// from the database, false otherwise.
        /// </summary>
        public bool IsChanged { get; set; }
        
        /// <summary>
        /// The value currently held in the cell.
        /// </summary>
        public T Value
        {
            get { return _value; }
            
            set
            {
                if (Value == null && value == null)
                {
                    //Do nothing
                }
                else if (Value == null && value != null)
                {
                    _value = value;
                    IsChanged = true;
                }
                else if (!Value.Equals (value))
                {
                    _value = value;
                    IsChanged = true;
                }
            }
        }
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Create a new cell.
        /// </summary>
        /// <param name="name">
        /// The column name.
        /// </param>
        public Cell ()
        {
            IsChanged = false;
        }
        
        #endregion
    }
}

