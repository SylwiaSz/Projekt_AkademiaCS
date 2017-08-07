namespace WpfApplication1
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public abstract class CalendarPart : Grid
    {
        public CalendarPart()
        {
            this.Background = Brushes.DarkOliveGreen;
            this.Height = 50;
            this.Width = 350;

            var editButton = new Button
                                 {
                                     Name = "editButton",
                                     Content = "EDIT",
                                     HorizontalAlignment = HorizontalAlignment.Right,
                                     VerticalAlignment = VerticalAlignment.Center,
                                     Foreground = Brushes.DarkTurquoise,
                                     Background = Brushes.DarkSlateGray,
                                     BorderBrush = Brushes.DarkTurquoise,
                                     Margin = new Thickness(0, 0, 10, 0),
                                     Width = 40,
                                     Height = 30
                                 };
            editButton.Click += (s, e) => this.Edit();

            var deleteButton = new Button
                                   {
                                       Name = "deleteButton",
                                       Content = "DELETE",
                                       HorizontalAlignment = HorizontalAlignment.Right,
                                       VerticalAlignment = VerticalAlignment.Center,
                                       Foreground = Brushes.DarkTurquoise,
                                       Background = Brushes.DarkSlateGray,
                                       BorderBrush = Brushes.DarkTurquoise,
                                       Margin = new Thickness(
                                           0,
                                           0,
                                           editButton.Width + editButton.Margin.Right + 10,
                                           0),
                                       Width = 50,
                                       Height = 30
                                   };

            deleteButton.Click += (s, e) => this.Delete();

            this.Children.Add(deleteButton);
            this.Children.Add(editButton);
        }

        public delegate void DeletePart(CalendarPart part);

        public event DeletePart deletePartEvent;

        public void Delete()
        {
            this.deletePartEvent.Invoke(this);
        }

        public void Edit()
        {
            var addWindow = new AddWindow(this);
            addWindow.addPartEvent += this.EditMethod;
            addWindow.Show();
        }

        public virtual void EditMethod(CalendarPart part)
        {
        }
    }
}