using ICSharpCode.AvalonEdit;
using System.Windows;

namespace Compiler.Helpers
{
    public static class AvalonEditHelper
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(AvalonEditHelper),
                new PropertyMetadata("", OnTextChanged));

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextEditor editor)
            {
                var newValue = (string)e.NewValue;
                if (editor.Text != newValue)
                {
                    editor.Text = newValue;
                }
            }
        }
    }
}