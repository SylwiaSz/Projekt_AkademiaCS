namespace WpfApplication1
{
    using System;
    using System.Windows.Media;

    public class Category
    {
        public Category(TimeSpan offset, Brush color, string tag)
        {
            this.reportOffset = offset;
            this.categoryColor = color;
            this.Tag = tag;
        }

        public Brush categoryColor { get; set; }

        public TimeSpan reportOffset { get; set; }

        public string Tag { get; set; }

        public void EditCategory()
        {
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return this.Tag;
        }
    }
}