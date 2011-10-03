using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Ngol.Hytek;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;

namespace Ngol.XcAnalyze.UI.Views.Gtk.ViewModels
{
    /// <summary>
    /// A text buffer that proviedes the model for a view of a race.
    /// </summary>
    public class RaceResultsBuffer : TextBuffer
    {
        #region Properties

        /// <summary>
        /// The default tag used for the text.
        /// </summary>
        public TextTag DefaultTag
        {
            get;
            set;
        }

        /// <summary>
        /// The object used to format the race results.
        /// </summary>
        public RaceFormatter Formatter
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="RaceResultsBuffer" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="races"/> is <see langword="null" />.
        /// </exception>
        public RaceResultsBuffer(IEnumerable<Race> races) : this()
        {
            if(races == null)
                throw new ArgumentNullException("race");
            foreach(Race race in races)
            {
                race.ComputeScore();
                Text += "\n" + "\n".Join(Formatter.Format(race));
            }
            ApplyTag(DefaultTag, StartIter, EndIter);
        }

        /// <summary>
        /// Construct a new <see cref="RaceResultsBuffer" />.
        /// </summary>
        protected RaceResultsBuffer() : base(null)
        {
            DefaultTag = new TextTag("default");
            DefaultTag.Font = "courier";
            TagTable.Add(DefaultTag);
            Formatter = new RaceFormatter();
        }
        
        #endregion
    }
}
