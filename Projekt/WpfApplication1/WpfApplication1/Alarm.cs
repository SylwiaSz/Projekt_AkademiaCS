namespace WpfApplication1
{
    using System;

    public class Alarm : TimedCalendarPart
    {
        public string Label;

        private DateTime _reminderTime;

        public Alarm(DateTime time, string label)
        {
            this.ReminderTime = time;
            this.Label = label;
            this.TimeText = this.ReminderTime.ToString("HH:mm");
        }

        public override DateTime ReminderTime
        {
            get
            {
                return this._reminderTime;
            }

            set
            {
                this._reminderTime = value - TimeSpan.FromMilliseconds(value.Millisecond)
                                     - TimeSpan.FromSeconds(value.Second);
            }
        }

        public override void EditMethod(CalendarPart part)
        {
            var alarm = (Alarm)part;
            this.ReminderTime = alarm.ReminderTime;
            this.Label = alarm.Label;
            this.TimeText = this.ReminderTime.ToString("HH:mm");
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return $"Alarm: {this.Label}\nTime: {this.ReminderTime:HH:mm}";
        }
    }
}