namespace WpfApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    /// <summary>
    ///     Interaction logic for DateWindow.xaml
    /// </summary>
    public partial class DateWindow : Window
    {
        public DateWindow(DateTime currentDate)
        {
            this.items = new List<CalendarPart>();
            this.InitializeComponent();
            this.dateLabel.Content += currentDate.Date.ToString("dd/MM/yyyy");
            this.UpdateBox();
        }

        /// <summary>
        ///     The items.
        /// </summary>
        public List<CalendarPart> items { get; }

        public void AddPart(CalendarPart part)
        {
            part.deletePartEvent += this.DeleteItem;
            this.items.Add(part);
            this.eventPanel.Children.Add(part);
            this.UpdateBox();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var sad = new AddWindow();
            sad.addPartEvent += this.AddPart;
            sad.Show();
        }

        private void DeleteItem(CalendarPart part)
        {
            this.items.Remove(part);
            this.eventPanel.Children.Remove(part);
            this.UpdateBox();
        }

        private void UpdateBox()
        {
            this.nothingBox.Visibility = this.items.Any() ? Visibility.Hidden : Visibility.Visible;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}