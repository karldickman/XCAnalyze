using System;
using System.Collections;
using System.Collections.Generic;
using TextFormat;
using TextFormat.Table;

namespace XCAnalyze.Hytek
{
    /// <summary>
    /// The interface to which all formatters must adhere.
    /// </summary>
    public interface IFormatter<T>
    {
        /// <summary>
        /// Format a value into a list of lines.
        /// </summary>
        /// <param name="thing">
        /// The value to format.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines.
        /// </returns>
        IList<string> Format(T thing);
    }
    
    /// <summary>
    /// A formatter that produces Hytek-style tables.
    /// </summary>
    public class HytekFormatter
    {
        /// <summary>
        /// The Header of the hytek table.
        /// </summary>
        protected internal IList<string> Header { get; set; }

        /// <summary>
        /// The title of the table.
        /// </summary>
        protected internal string Title { get; set; }

        /// <summary>
        /// The table formatter used internally.
        /// </summary>
        protected internal LabeledTableFormatter TableFormatter { get; set; }
        
        /// <summary>
        /// Create a formatter that produces tables without titles.
        /// </summary>
        /// <param name="header">
        /// A <see cref="System.String[]"/>.  The header of the table.
        /// </param>
        public HytekFormatter (IList<string> header) : this(null, header) {}
        
        /// <summary>
        /// Create a formatter that produces tables with titles.
        /// </summary>
        /// <param name="title">
        /// A <see cref="System.String"/>.  The title of the table.
        /// </param>
        /// <param name="header">
        /// A <see cref="System.String[]"/>.  The header of the table.
        /// </param>
        public HytekFormatter (string title, IList<string> header)
        {
            Title = title;
            Header = header;
            TableFormatter = new LabeledTableFormatter ('\0', ' ', '\0', '=',
                '\0', '=', '\0', '=');
        }
        
        /// <summary>
        /// Format a list of values.
        /// </summary>
        /// <param name="values">
        /// The <see cref="IList<System.Object[]>"/> of values to format.
        /// </param>
        /// <param name="alignments">
        /// A <see cref="Alignment[]"/>.  The alignments of the columns.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of table lines.
        /// </returns>
        protected internal IList<string> Format (IList<IList> values,
            IList<Alignment> alignments)
        {
            IList header = new ArrayList();
            foreach(string title in Header)
            {
                header.Add(title);
            }
            IList<string> tableLines = TableFormatter.Format (header, values,
                alignments);
            IList<string> lines = new List<string>();
            if(Title == null)
            {
                return tableLines;
            }
            lines.Add(StringFormatting.Centered(Title, tableLines[0].Length));
            foreach(string line in tableLines)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
}