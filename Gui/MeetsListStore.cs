using System;
using System.Collections.Generic;

using Gtk;

using XCAnalyze.Collections;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// A list store that provides the model for a list of meets.
    /// </summary>
    public class MeetsListStore : ListStore
    {
        /// <summary>
        /// The list of meets contained in this store.
        /// </summary>
        protected IXList<Meet> Meets { get; set; }
        
        public MeetsListStore (IDataSelection<Meet> meetSelection)
        : base(typeof(Meet),//The meet itself
                typeof(string),//Name of meet
                typeof(string),//Date of meet
                typeof(Venue),//Venue where held
                typeof(int),//Men's distance
                typeof(int))//Women's distance
        {
            meetSelection.ContentReplaced += OnContentReplaced;
            meetSelection.ItemsAdded += OnItemsAdded;
            meetSelection.ItemsDeleted += OnItemsDeleted;
        }
        
        /// <summary>
        /// A comparer used to sort meets in this list store.
        /// </summary>
        public class MeetComparer : IComparer<Meet>
        {
            #region IComparer[Meet] implementation
            /// <summary>
            /// Meets are compared first by date, then by name.  If both are
            /// identical, then it is the same meet.
            /// </summary>
            public int Compare (Meet first, Meet second)
            {
                int comparison;
                if (first == second)
                {
                    return 0;
                }
                comparison = first.Date.CompareTo (second.Date);
                if (comparison != 0)
                {
                    return comparison;
                }
                if (first.Name == second.Name)
                {
                    return 0;
                }
                if (first.Name == null)
                {
                    return 1;
                }
                if (second.Name == null)
                {
                    return 0;
                }
                return first.Name.CompareTo (second.Name);
            }
            #endregion
        }
        
        /// <summary>
        /// Append a meet to the store.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to append.
        /// </param>
        protected void Append (Meet meet)
        {
            AppendValues (meet, meet.Name,
                string.Format("{0:yyyy/MM/dd}", meet.Date),
                meet.Location, meet.MensDistance, meet.WomensDistance);
        }
        
        /// <summary>
        /// Add a bunch of meets.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IEnumerable<Meet>"/> to add.
        /// </param>
        protected void Append(IEnumerable<Meet> meets)
        {
            foreach (Meet meet in meets) 
            {
                Append (meet);
            }
        }
        
        /// <summary>
        /// Handler for when the list of meets is changed.
        /// </summary>
        protected void OnContentReplaced (object sender,
            ContentModifiedArgs<Meet> arguments)
        {
            Clear ();
            Meets = new XList<Meet> (arguments.Items);
            Meets.Sort (new MeetComparer ());
            Append (Meets);
        }
        
        /// <summary>
        /// Handler for when items needs to be added to the store.
        /// </summary>
        protected void OnItemsAdded (object sender,
            ContentModifiedArgs<Meet> arguments)
        {
            Clear ();
            Meets.AddRange (arguments.Items);
            Meets.Sort ();
            Append (Meets);
        }
        
        protected void OnItemsDeleted (object sender,
            ContentModifiedArgs<Meet> arguments)
        {
            Clear ();
            IList<Meet> meets = new List<Meet> (arguments.Items);
            Meets.RemoveAll (meet => meets.Contains (meet));
            Append (meets);
        }
    }
}