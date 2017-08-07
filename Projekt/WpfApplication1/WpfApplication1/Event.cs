// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="">
//   
// </copyright>
// <summary>
//   The event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfApplication1
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    ///     The event.
    /// </summary>
    public class Event : TimedCalendarPart
    {
        /// <summary>
        /// The event time.
        /// </summary>
        private DateTime eventTime;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        /// <param name="time">
        ///     The time.
        /// </param>
        /// <param name="category">
        ///     The category.
        /// </param>
        /// <param name="priority">
        ///     The priority.
        /// </param>
        /// <param name="label">
        ///     The label.
        /// </param>
        public Event(DateTime time, Category category, Priority priority, string label)
        {
            this.ReminderTime = time;
            this.EventCategory = category;
            this.EventPriority = priority;
            this.EventLabel = label;

            var firstOrDefault = CategoryList.Singleton.categoryList.FirstOrDefault(x => x == this.EventCategory);
            if (firstOrDefault != null)
            {
                this.Background = firstOrDefault.categoryColor;
            }

            this.TimeText = this.eventTime.ToString("HH:mm");
            var textBox = new TextBox
                              {
                                  Name = "TextBox",
                                  Text = this.EventLabel,
                                  VerticalAlignment = VerticalAlignment.Center,
                                  HorizontalAlignment = HorizontalAlignment.Left,
                                  Margin = new Thickness(
                                      this.Children.OfType<TextBox>().First(x => x.Name.Contains("TimeBox"))
                                          .ActualWidth + 50,
                                      0,
                                      this.Children.OfType<Button>().First(x => x.Name.Contains("deleteButton")).Margin
                                          .Right + this.Children.OfType<Button>().First(x => x.Name.Contains("deleteButton")).Width + 10,
                                      0),
                                  Width = double.NaN,
                                  Height = double.NaN,
                                  IsEnabled = false
                              };

            this.Children.Add(textBox);
        }

        /// <summary>
        ///     The event category.
        /// </summary>
        public Category EventCategory { get; set; }

        /// <summary>
        ///     The event label.
        /// </summary>
        public string EventLabel { get; set; }

        /// <summary>
        ///     The event priority.
        /// </summary>
        public Priority EventPriority { get; set; }

        /// <summary>
        /// Gets or sets the reminder time.
        /// </summary>
        public override sealed DateTime ReminderTime
        {
            get
            {
                return this.eventTime - CategoryList.Singleton.categoryList.FirstOrDefault(x => x == this.EventCategory)
                           .reportOffset;
            }

            set
            {
                this.eventTime = value - TimeSpan.FromMilliseconds(value.Millisecond)
                                 - TimeSpan.FromSeconds(value.Second);
            }
        }

        /// <summary>
        ///     The edit method.
        /// </summary>
        /// <param name="part">
        ///     The part.
        /// </param>
        public override void EditMethod(CalendarPart part)
        {
            var editedEvent = (Event)part;
            this.ReminderTime = editedEvent.eventTime;
            this.EventCategory = editedEvent.EventCategory;
            this.EventPriority = editedEvent.EventPriority;
            this.EventLabel = editedEvent.EventLabel;

            this.TimeText = this.eventTime.ToString("HH:mm");
            this.Children.OfType<TextBox>().First(x => x.Name.Contains("TextBox")).Text = this.EventLabel;

            var firstOrDefault = CategoryList.Singleton.categoryList.FirstOrDefault(x => x == this.EventCategory);
            if (firstOrDefault != null)
            {
                this.Background = firstOrDefault.categoryColor;
            }
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return
                $"Event: {this.EventLabel}\nTime: {this.eventTime:HH:mm}\nPriority: {this.EventPriority}\nCategory: {this.EventCategory}";
        }
    }
}