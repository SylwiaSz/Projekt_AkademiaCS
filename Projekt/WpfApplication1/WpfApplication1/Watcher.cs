// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Watcher.cs" company="">
//   
// </copyright>
// <summary>
//   The watcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xceed.Wpf.Toolkit;

    /// <summary>
    ///     The watcher.
    /// </summary>
    public static class Watcher
    {
        /// <summary>
        ///     The start watch.
        /// </summary>
        public static void StartWatch()
        {
            var watchThread = new Thread(
                () =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        while (true)
                        {
                            var timeNow = DateTime.Now;
                            var events = MainWindow.Singleton.DayDictionary.Values.Select(x => x.items);
                            var eventList = new List<CalendarPart>();
                            foreach (var datyEvent in events) eventList.AddRange(datyEvent);
                            var timedEvents = eventList.Where(x => x is TimedCalendarPart);

                            Parallel.ForEach(
                                timedEvents,
                                timedEvent =>
                                    {
                                        if (((TimedCalendarPart)timedEvent).ReminderTime
                                            < timeNow - TimeSpan.FromMilliseconds(100)
                                            || ((TimedCalendarPart)timedEvent).ReminderTime
                                            > timeNow + TimeSpan.FromMilliseconds(100)) return;

                                        if (((TimedCalendarPart)timedEvent).Shown) return;

                                        var box = new Thread(() => { MessageBox.Show(timedEvent.ToString()); });
                                        box.SetApartmentState(ApartmentState.STA);
                                        box.Start();
                                        ((TimedCalendarPart)timedEvent).Shown = true;
                                    });
                        }
                    });

            watchThread.SetApartmentState(ApartmentState.STA);
            watchThread.Start();
        }
    }
}