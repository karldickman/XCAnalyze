using System;
using System.Collections.Generic;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The model used by race visualizers to determine which race to draw.
    /// </summary>
    public class RaceDisplayModel : DataSelection<Race>
    {
        /// <summary>
        /// Is this model supposed to display a men's race or a women's race?
        /// </summary>
        public Gender Gender { get; set; }
        
        public RaceDisplayModel (Gender gender,
            IDataSelection<Meet> meetSelection)
        {
            Gender = gender;
            meetSelection.SelectionChanged += OnSelectionChanged;
        }
        
        /// <summary>
        /// Callback to handle a changed to the currently selected meet.
        /// </summary>
        public void OnSelectionChanged (object sender,
            SelectionChangedArgs<Meet> arguments)
        {
            Meet meet = arguments.Selected;
            Select (meet.Race (Gender));
        }
    }
}
