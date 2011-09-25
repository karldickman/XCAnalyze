using System;
using System.Collections.Generic;

namespace Ngol.XcAnalyze.Persistence.Interfaces
{
    /// <summary>
    /// Generic interface to be implemented by all repository classes.
    /// </summary>
    public interface IRepository<T> : ICollection<T>, IDisposable
    {
        /// <summary>
        /// Update the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to update.
        /// </param>
        void Update(T item);
    }
}

