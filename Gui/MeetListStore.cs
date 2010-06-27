using Gtk;
using System;
using System.Collections.Generic;

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
        protected List<Meet> Meets { get; set; }
        
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
        /// Append a meet to the store.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to append.
        /// </param>
        protected void AppendMeet (Meet meet)
        {
            AppendValues (meet, meet.Name, meet.Date.ToString (), meet.Venue,
                meet.MensDistance, meet.WomensDistance);
        }
        
        /// <summary>
        /// Handler for when the list of meets is changed.
        /// </summary>
        protected void OnContentReplaced (object sender,
            ContentModifiedArgs<Meet> arguments)
        {
            Clear ();
            Meets = new List<Meet> (arguments.Items);
            Meets.Sort ();
            foreach (Meet meet in Meets)
            {
                AppendMeet (meet);
            }
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
            foreach (Meet meet in arguments.Items)
            {
                AppendMeet (meet);
            }
        }
        
        protected void OnItemsDeleted (object sender,
            ContentModifiedArgs<Meet> arguments)
        {
            Clear ();
            IList<Meet> meets = new List<Meet> (arguments.Items);
            Meets.RemoveAll (meet => meets.Contains (meet));
            foreach (Meet meet in Meets)
            {
                AppendMeet (meet);
            }
        }
    }
}