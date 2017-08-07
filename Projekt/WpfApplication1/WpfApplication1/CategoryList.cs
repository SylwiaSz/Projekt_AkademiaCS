// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryList.cs" company="">
//   
// </copyright>
// <summary>
//   The category list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfApplication1
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    /// <summary>
    ///     The category list.
    /// </summary>
    public class CategoryList
    {
        /// <summary>
        ///     The instance.
        /// </summary>
        private static CategoryList instance;

        /// <summary>
        ///     Prevents a default instance of the <see cref="CategoryList" /> class from being created.
        /// </summary>
        private CategoryList()
        {
            this.categoryList =
                new ObservableCollection<Category>
                    {
                        new Category(
                            TimeSpan.FromMinutes(1),
                            Brushes.MediumVioletRed,
                            "Meeting"),
                        new Category(TimeSpan.FromDays(1), Brushes.Purple, "Birthday"),
                        new Category(
                            TimeSpan.FromMinutes(30),
                            Brushes.DarkSlateBlue,
                            "Reminder")
                    };
        }

        /// <summary>
        ///     Gets the singleton.
        /// </summary>
        public static CategoryList Singleton => instance ?? (instance = new CategoryList());

        /// <summary>
        ///     Gets or sets the category list.
        /// </summary>
        public ObservableCollection<Category> categoryList { get; set; }
    }
}