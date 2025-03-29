using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanFrameBuilder.View.CustomControls
{
    public class NumberBox : TextBox
    {
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumberBox), new PropertyMetadata(int.MinValue));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumberBox), new PropertyMetadata(int.MaxValue));

        public int MinValue
        {
            get => (int)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public NumberBox()
        {
            PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(this, OnPaste);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
                return;
            }

            string newText = Text.Insert(SelectionStart, e.Text);
            if (!IsWithinRange(newText))
            {
                ShowErrorWindow($"Value must be between {MinValue} and {MaxValue}.");
                e.Handled = true;
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string pasteText = e.DataObject.GetData(DataFormats.Text) as string;
                if (!IsTextAllowed(pasteText) || !IsWithinRange(pasteText))
                {
                    ShowErrorWindow("Invalid or out-of-range input.");
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, @"^[-+]?[0-9]+(\.[0-9]*)?$");
        }

        private bool IsWithinRange(string text)
        {
            return int.TryParse(text, out int value) && value >= MinValue && value <= MaxValue;
        }

        private void ShowErrorWindow(string message)
        {
            MessageBox.Show(message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
