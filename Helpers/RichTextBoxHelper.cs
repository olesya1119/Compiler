using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Compiler.Helpers
{
    public static class RichTextBoxHelper
    {
        public static readonly DependencyProperty BoundDocumentProperty =
            DependencyProperty.RegisterAttached(
                "BoundDocument",
                typeof(FlowDocument),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(null, OnBoundDocumentChanged));

        public static FlowDocument GetBoundDocument(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(BoundDocumentProperty);
        }

        public static void SetBoundDocument(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(BoundDocumentProperty, value);
        }

        private static void OnBoundDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox richTextBox)
            {
                richTextBox.Document = e.NewValue as FlowDocument ?? new FlowDocument(new Paragraph(new Run("")));
            }
        }
    }
}
