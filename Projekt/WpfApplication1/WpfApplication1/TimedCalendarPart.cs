namespace WpfApplication1
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public abstract class TimedCalendarPart : CalendarPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimedCalendarPart"/> class.
        /// </summary>
        protected TimedCalendarPart()
        {
            var timeBox = new TextBox { Name = "TimeBox", HorizontalAlignment = HorizontalAlignment.Left };

            this.Children.Add(timeBox);
        }

        /// <summary>
        ///     The reminder time.
        /// </summary>
        public abstract DateTime ReminderTime { get; set; }

        public bool Shown { get; set; }

        protected string TimeText
        {
            get
            {
                return this.Children.OfType<TextBox>().FirstOrDefault(x => x.Name.Contains("TimeBox")).Text;
            }

            set
            {
                this.Children.OfType<TextBox>().FirstOrDefault(x => x.Name.Contains("TimeBox")).Text = value;
            }
        }
    }
}