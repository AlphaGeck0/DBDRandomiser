using System.Windows.Controls;

namespace DBDKillerSelector
{
    internal class ViewBox
    {
        public object Stretch { get; internal set; }
        public StretchDirection StretchDirection { get; internal set; }
        public TextBlock Child { get; internal set; }
    }
}