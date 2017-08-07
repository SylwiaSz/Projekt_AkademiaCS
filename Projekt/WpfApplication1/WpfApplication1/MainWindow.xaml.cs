// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Newtonsoft.Json;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     The instance.
        /// </summary>
        private static MainWindow instance;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.DayDictionary = new Dictionary<DateTime, DateWindow>();
            this.InitializeComponent();

            if (File.Exists("file.txt"))
            {
                var js = File.ReadAllText("file.txt");
                object content = JsonConvert.DeserializeObject<KeyValuePair<DateTime, DateWindow>[]>(js)
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
                this.DayDictionary = (Dictionary<DateTime, DateWindow>)content;
            }

            Watcher.StartWatch();
            instance = this;
        }

        /// <summary>
        ///     The singleton.
        /// </summary>
        public static MainWindow Singleton => instance ?? (instance = new MainWindow());

        /// <summary>
        ///     Gets or sets the day dictionary.
        /// </summary>
        public Dictionary<DateTime, DateWindow> DayDictionary { get; set; }

        /// <summary>
        ///     The on closing.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            foreach (var day in this.DayDictionary) day.Value.Close();

            /* var js = JsonConvert.SerializeObject(
                dayDictionary.ToArray(),
                (Newtonsoft.Json.Formatting)Formatting.Indented,
                new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                });
            System.IO.File.WriteAllText("file.txt", js);*/
        }

        /// <summary>
        ///     The calendar mouse double click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void CalendarMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedDate = ((Calendar)sender).SelectedDate;
            if (selectedDate == null) return;

            var choosenDate = (DateTime)selectedDate;

            if (!this.DayDictionary.ContainsKey(choosenDate))
                this.DayDictionary.Add(choosenDate, new DateWindow(choosenDate));

            this.DayDictionary[choosenDate].Show();
            ((Calendar)sender).SelectedDate = null;
        }

        /// <summary>
        ///     The category button click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void CategoryButtonClick(object sender, RoutedEventArgs e)
        {
            new EditCategory().Show();
        }

        /// <summary>
        ///     The text box text changed.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}