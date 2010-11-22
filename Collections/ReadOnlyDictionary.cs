using System;
using System.Collections;
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
        
        #region IDictionary[K, T] implementation
        /// <summary>
        /// The keys in the dictionary.
        /// </summary>
        public ICollection<K> Keys
        {
            get { return InternalDictionary.Keys; }
        }
        
        public ICollection<T> Values
        {
            get { return InternalDictionary.Values; }
        }
        
        public void Add (K key, T value)
        {
            throw new NotSupportedException ();
        }
        
        /// <summary>
        /// Does the dictionary contain a particular key.
        /// </summary>
        /// <param name="key">
        /// The key for which to search.
        /// </param>
        public bool ContainsKey (K key)
        {
            return InternalDictionary.ContainsKey (key);
        }
        
        public bool Remove (K key)
        {
            throw new NotSupportedException ();
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
        
        #region ICollection[KeyValuePair[K,T]] implementation
        public int Count
        {
            get { return InternalDictionary.Count; }
        }
        
        public bool IsReadOnly
        {
            get { return true; }
        }
        
        public void Add (KeyValuePair<K, T> item)
        {
            throw new NotSupportedException ();
        }
        
        public void Clear ()
        {
            throw new NotSupportedException ();
        }
        
        public bool Contains (KeyValuePair<K, T> item)
        {
            return InternalDictionary.Contains (item);
        }

        public void CopyTo (KeyValuePair<K, T>[] array, int index)
        {
            InternalDictionary.CopyTo (array, index);
        }        
        
        public bool Remove (KeyValuePair<K, T> item)
        {
            throw new NotSupportedException();
        }
        #endregion
        
        #region IEnumerable implementation
        
        IEnumerator IEnumerable.GetEnumerator ()
        {
            return InternalDictionary.GetEnumerator ();
        }
        
        #endregion
        
        #region IEnumerable[KeyValuePair[K, T]] implementation
        
        IEnumerator<KeyValuePair<K, T>> System.Collections.Generic.IEnumerable<KeyValuePair<K, T>>.GetEnumerator ()
        {
            return InternalDictionary.GetEnumerator ();
        }
        
        #endregion
    }
}
