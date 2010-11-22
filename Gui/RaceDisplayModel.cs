using System;
using System.Collections.Generic;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The model used by race visualizers to determine which race to draw.
    /// </summary>
    public partial class RaceDisplayModel : DataSelection<Race>
    {
        /// <summary>
        /// Is this model supposed to display a men's race or a women's race?
        /// </summary>
        public Gender Gender { get; set; }
        
        public RaceDisplayModel (Gender gender,
            IDataSelection<MeetInstance> meetSelection)
        {
            Gender = gender;
            meetSelection.SelectionChanged += OnSelectionChanged;
        }
        
        /// <summary>
        /// Callback to handle a changed to the currently selected meet.
        /// </summary>
        public void OnSelectionChanged (object sender,
            SelectionChangedArgs<MeetInstance> arguments)
        {
            //MeetInstance meet = arguments.Selected;
            //TODO FIX THIS!
            //Select (meet.Race (Gender));
        }
    }
}
