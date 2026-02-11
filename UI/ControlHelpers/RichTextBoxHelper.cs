using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WT.UI.ControlHelpers
{
    public static class RichTextBoxHelper
    {
        public static FlowDocument GetBoundDocument(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(BoundDocumentProperty);
        }

        public static void SetBoundDocument(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(BoundDocumentProperty, value);
        }

        public static readonly DependencyProperty BoundDocumentProperty =
            DependencyProperty.RegisterAttached(
                "BoundDocument",
                typeof(FlowDocument),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBoundDocumentChanged));

        private static void OnBoundDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not RichTextBox richTextBox)
                return;

            if (e.NewValue is FlowDocument flowDocument)
            {
                richTextBox.Document = flowDocument;
            }
        }
    }

}
