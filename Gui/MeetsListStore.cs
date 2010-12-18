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
        protected IXList<MeetInstance> Meets { get; set; }

        //The meet itself
        //Name of meet
        //Date of meet
        //Venue where held
        //Men's distance
        //Women's distance
        public MeetsListStore(IDataSelection<MeetInstance> meetSelection) : base(typeof(MeetInstance), typeof(string), typeof(string), typeof(Venue), typeof(int), typeof(int))
        {
            meetSelection.ContentReplaced += OnContentReplaced;
            meetSelection.ItemsAdded += OnItemsAdded;
            meetSelection.ItemsDeleted += OnItemsDeleted;
        }

        /// <summary>
        /// A comparer used to sort meets in this list store.
        /// </summary>
        public class MeetComparer : IComparer<MeetInstance>
        {
            #region IComparer[Meet] implementation
            /// <summary>
            /// Meets are compared first by date, then by name.  If both are
            /// identical, then it is the same meet.
            /// </summary>
            public int Compare(MeetInstance first, MeetInstance second)
            {
                int comparison;
                if(first == second) {
                    return 0;
                }
                comparison = first.Date.CompareTo(second.Date);
                if(comparison != 0) {
                    return comparison;
                }
                if(first.Name == second.Name) {
                    return 0;
                }
                if(first.Name == null) {
                    return 1;
                }
                if(second.Name == null) {
                    return 0;
                }
                return first.Name.CompareTo(second.Name);
            }
            #endregion
        }

        /// <summary>
        /// Append a meet to the store.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to append.
        /// </param>
        protected void Append(MeetInstance meet)
        {
            AppendValues(meet, meet.Name, string.Format("{0:yyyy/MM/dd}", meet.Date), meet.Venue);
        }

        /// <summary>
        /// Add a bunch of meets.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IEnumerable<Meet>"/> to add.
        /// </param>
        protected void Append(IEnumerable<MeetInstance> meets)
        {
            foreach(MeetInstance meet in meets) {
                Append(meet);
            }
        }

        /// <summary>
        /// Handler for when the list of meets is changed.
        /// </summary>
        protected void OnContentReplaced(object sender, ContentModifiedArgs<MeetInstance> arguments)
        {
            Clear();
            Meets = new XList<MeetInstance>(arguments.Items);
            Meets.Sort(new MeetComparer());
            Append(Meets);
        }

        /// <summary>
        /// Handler for when items needs to be added to the store.
        /// </summary>
        protected void OnItemsAdded(object sender, ContentModifiedArgs<MeetInstance> arguments)
        {
            Clear();
            Meets.AddRange(arguments.Items);
            Meets.Sort();
            Append(Meets);
        }

        protected void OnItemsDeleted(object sender, ContentModifiedArgs<MeetInstance> arguments)
        {
            Clear();
            IList<MeetInstance> meets = new List<MeetInstance>(arguments.Items);
            Meets.RemoveAll(meet => meets.Contains(meet));
            Append(meets);
        }
    }
}
