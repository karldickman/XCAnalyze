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

        #region Physical implementation

        private Race _race;

        #endregion

        /// <summary>
        /// The default tag used for the text.
        /// </summary>
        public TextTag DefaultTag { get; set; }

        /// <summary>
        /// The object used to format the race results.
        /// </summary>
        public readonly RaceFormatter Formatter;

        /// <summary>
        /// The <see cref="Race" /> currently on display.
        /// </summary>
        /// <exception cref='ArgumentNullException'>
        /// Thrown when an attempt is made to set the value of this property
        /// to <see langword="null" />.
        /// </exception>
        public Race Race
        {
            get { return _race; }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _race = value;
                Race.ComputeScore();
                Text = "\n".Join(Formatter.Format(Race));
                ApplyTag(DefaultTag, StartIter, EndIter);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="RaceResultsBuffer" />.
        /// </summary>
        public RaceResultsBuffer() : base(null)
        {
            DefaultTag = new TextTag("default") { Font = "courier", };
            TagTable.Add(DefaultTag);
            Formatter = new RaceFormatter();
        }
        
        #endregion
    }
}
