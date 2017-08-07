namespace WpfApplication1
{
    using System.Windows;

    /// <summary>
    ///     Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            this.InitializeComponent();
        }

        public delegate void returnPassword(string str);

        public event returnPassword SavePasswordEvent;

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.SavePasswordEvent(this.passwordBox.Password);
            this.Close();
        }
    }
}