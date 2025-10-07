using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CustomControls
{
    /// <summary>
    /// Interaction logic for CustomRichTextBox.xaml
    /// </summary>
    public partial class CustomRichTextBox : UserControl
    {
        public CustomRichTextBox()
        {
            InitializeComponent();
            NotesRichTextBox.Document = new FlowDocument(new Paragraph(new Run()));
        }

        #region Properties

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text), typeof(string), typeof(CustomRichTextBox),
                new PropertyMetadata("", OnTextPropertyChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomRichTextBox control)
            {
                var newText = e.NewValue as string ?? string.Empty;
                var currentText = new TextRange(
                    control.NotesRichTextBox.Document.ContentStart,
                    control.NotesRichTextBox.Document.ContentEnd).Text.TrimEnd('\r', '\n');

                if (currentText != newText)
                {
                    control.NotesRichTextBox.Document.Blocks.Clear();
                    control.NotesRichTextBox.Document.Blocks.Add(new Paragraph(new Run(newText)));
                }
            }
        }

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register(
                nameof(IsEditable), typeof(bool), typeof(CustomRichTextBox),
                new PropertyMetadata(false));

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        #endregion

        #region Event handlers

        private void NotesRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = new TextRange(NotesRichTextBox.Document.ContentStart, NotesRichTextBox.Document.ContentEnd).Text.TrimEnd('\r', '\n');
            if (text != null && Text != text)
            {
                Text = text;
            }
        }

        #endregion
    }
}
