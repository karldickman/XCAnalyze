using System;
using System.Collections.Generic;

namespace XCAnalyze.Collections
{
    /// <summary>
    /// A dictionary whose values cannot be modified.
    /// </summary>
    public class ReadOnlyDictionary<K, T> : IDictionary<K, T>
    {
        /// <summary>
        /// Create a new read-only wrapper around a particular dictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The <see cref="IDictionary<K, T>"/> around whit to wrap.
        /// </param>
        public ReadOnlyDictionary (IDictionary<K, T> dictionary)
        {
            InternalDictionary = dictionary;
        }
        
        /// <summary>
        /// The dictionary whose write access is being blocked.
        /// </summary>
        protected IDictionary<K, T> InternalDictionary { get; set; }
         
        /// <summary>
        /// Get a value int the dictionary.
        /// </summary>
        /// <param name="key">
        /// The key to search for.
        /// </param>
        public T this[K key]
        {
            get { return InternalDictionary[key]; }
            set { throw new NotSupportedException (); }
        }
        
        #region System.Collections.Generic.IDictionary<K, T> implementation
        public void Add (KeyValuePair<K, T> item)
        {
            throw new NotSupportedException ();
        }

        public void Clear ()
        {
            throw new NotSupportedException ();
        }
        
        /// <summary>
        /// The keys in the dictionary.
        /// </summary>
        public ICollection<K> Keys
        {
            get { return InternalDictionary.Keys; }
        }
        
        /// <summary>
        /// Does the dictionary contain a particular key.
        /// </summary>
        /// <param name="key">
        /// The key for which to search.
        /// </param>
        public bool ContainsKey (K key)
        {
            return InternalDictionary.ContainsKey(key);
        }
    
        /// <summary>
        /// Attempt to get a value from the dictionary.
        /// </summary>
        /// <param name="key">
        /// The key for which to search.
        /// </param>
        /// <param name="value_">
        /// The variable in which to store the value.
        /// </param>
        /// <returns>
        /// True if the value could be found, false if not.
        /// </returns>
        public bool TryGetValue (K key, out T value_)
        {
            return InternalDictionary.TryGetValue (key, out value_);
        }
        #endregion
        #region ICollection[System.Collections.Generic.KeyValuePair[XCAnalyze.Collections.ReadOnlyDictionary.K,XCAnalyze.Collections.ReadOnlyDictionary.T]] implementation

        
        
        public bool Contains (KeyValuePair<K, T> item)
        {
            throw new System.NotImplementedException();
        }
        
        
        public void CopyTo (KeyValuePair<K, T>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }
        
        
        public int Count {
            get {
                throw new System.NotImplementedException();
            }
        }
        
        
        public bool IsReadOnly {
            get {
                throw new System.NotImplementedException();
            }
        }
        
        
        public bool Remove (KeyValuePair<K, T> item)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
        #region IDictionary[XCAnalyze.Collections.ReadOnlyDictionary.K,XCAnalyze.Collections.ReadOnlyDictionary.T] implementation
        public void Add (K key, T value)
        {
            throw new System.NotImplementedException ();
        }
        

        

        
        
        public bool Remove (K key)
        {
            throw new System.NotImplementedException();
        }
        
        

        
        
        public ICollection<T> Values {
            get {
                throw new System.NotImplementedException();
            }
        }
        
        #endregion
        #region IEnumerable implementation
        public System.Collections.IEnumerator GetEnumerator ()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
    }
}
