namespace WpfApplication1
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    ///     Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory : Window
    {
        public EditCategory()
        {
            this.InitializeComponent();
            this.categoryComboBox.ItemsSource = CategoryList.Singleton.categoryList;
        }

        private Category SelectedCategory => CategoryList.Singleton.categoryList.FirstOrDefault(
            x => x == (Category)this.categoryComboBox.SelectedItem);

        private void ColorValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// The combo box selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedCategory == null)
            {
                return;
            }

            this.offsetTextBox.Text = this.SelectedCategory.reportOffset.TotalMinutes.ToString();
            this.nameTextBox.Text = this.SelectedCategory.Tag;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int newOffset;
                if (!int.TryParse(this.offsetTextBox.Text, out newOffset))
                {
                    MessageBox.Show("Cannot parse offset");
                    return;
                }

                this.SelectedCategory.reportOffset = TimeSpan.FromMinutes(newOffset);
                this.SelectedCategory.Tag = this.nameTextBox.Text;
                if (string.IsNullOrEmpty(this.rTextBox.Text) || string.IsNullOrEmpty(this.rTextBox.Text)
                    || string.IsNullOrEmpty(this.rTextBox.Text))
                {
                    return;
                }

                this.SelectedCategory.categoryColor = new SolidColorBrush(
                    Color.FromRgb(
                        (byte)int.Parse(this.rTextBox.Text),
                        (byte)int.Parse(this.gTextBox.Text),
                        (byte)int.Parse(this.bTextBox.Text)));
            }
            finally
            {
                this.Close();
            }
        }
    }
}