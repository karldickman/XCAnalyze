using System;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The signature of callbacks for a ContentChanged event.
    /// </summary>
    public delegate void ContentModifier<T> (object sender,
        ContentModifiedArgs<T> arguments);

    /// <summary>
    /// The signature of callbacks for a SelectionChanged event.
    /// </summary>
    public delegate void Selector<T> (object sender,
        SelectionChangedArgs<T> arguments);
}
