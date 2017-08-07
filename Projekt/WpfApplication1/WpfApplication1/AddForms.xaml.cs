namespace WpfApplication1
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for AddForms.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private readonly bool editMode;

        /// <summary>
        ///     The part.
        /// </summary>
        private CalendarPart Part;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddWindow" /> class.
        /// </summary>
        /// <param name="part">
        ///     The part.
        /// </param>
        public AddWindow(CalendarPart part)
            : this()
        {
            this.Part = part;
            if (part is Alarm)
            {
                var alarm = (Alarm)part;
                this.eventTypeBox.SelectedIndex = 0;
                this.BlankFields();
                this.labelTextBox.Text = alarm.Label;
                this.timePicker.Value = alarm.ReminderTime;
            }
            else if (part is Event)
            {
                var partAsEvent = (Event)part;
                this.eventTypeBox.SelectedIndex = 1;
                this.BlankFields();
                this.labelTextBox.Text = partAsEvent.EventLabel;
                this.timePicker.Value = partAsEvent.ReminderTime;
                this.categoryBox.SelectedValue = partAsEvent.EventCategory;
                this.priorityComboBox.SelectedValue = partAsEvent.EventPriority;
            }
            else if (part is Note)
            {
                this.eventTypeBox.SelectedIndex = 2;
                this.BlankFields();
                this.noteTextbox.Text = ((Note)part).noteText;
            }

            this.editMode = true;
            this.addButton.Content = "Save";
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddWindow" /> class.
        /// </summary>
        public AddWindow()
        {
            this.InitializeComponent();
            this.timePicker.Value = DateTime.Now;
            this.categoryBox.ItemsSource = CategoryList.Singleton.categoryList;
            this.priorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
        }

        public delegate void AddPart(CalendarPart part);

        public event AddPart addPartEvent;

        /// <summary>
        ///     Gets or sets the current date time.
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.editMode)
                switch (this.eventTypeBox.SelectedIndex)
                {
                    case 0:
                        this.Part = new Alarm((DateTime)this.timePicker.Value, this.labelTextBox.Text);
                        break;
                    case 1:
                        if (!this.CheckForEvent()) return;

                        this.Part = new Event(
                            (DateTime)this.timePicker.Value,
                            (Category)this.categoryBox.SelectedItem,
                            (Priority)this.priorityComboBox.SelectedItem,
                            this.labelTextBox.Text);
                        break;
                    case 2:
                        this.Part = new Note(this.noteTextbox.Text);
                        break;
                    default:
                        MessageBox.Show("Event type not choosen");
                        return;
                }
            else
                switch (this.eventTypeBox.SelectedIndex)
                {
                    case 0:

                        ((Alarm)this.Part).ReminderTime = (DateTime)this.timePicker.Value;
                        ((Alarm)this.Part).Label = this.labelTextBox.Text;
                        break;
                    case 1:

                        if (!this.CheckForEvent()) return;

                        ((Event)this.Part).ReminderTime = (DateTime)this.timePicker.Value;
                        ((Event)this.Part).EventCategory = (Category)this.categoryBox.SelectedItem;
                        ((Event)this.Part).EventPriority = (Priority)this.priorityComboBox.SelectedItem;
                        ((Event)this.Part).EventLabel = this.labelTextBox.Text;
                        break;
                    case 2:
                        ((Note)this.Part).noteText = this.noteTextbox.Text;
                        break;
                    default:
                        MessageBox.Show("Event type not choosen");
                        return;
                }

            this.addPartEvent.Invoke(this.Part);
            this.Close();
        }

        private void BlankFields()
        {
            switch (this.eventTypeBox.SelectedIndex)
            {
                case 0:
                    this.priorityComboBox.IsEnabled = false;
                    this.categoryBox.IsEnabled = false;
                    this.noteTextbox.IsEnabled = false;

                    this.timePicker.IsEnabled = true;
                    this.labelTextBox.IsEnabled = true;
                    break;
                case 1:
                    this.noteTextbox.IsEnabled = false;

                    this.priorityComboBox.IsEnabled = true;
                    this.categoryBox.IsEnabled = true;
                    this.timePicker.IsEnabled = true;
                    this.labelTextBox.IsEnabled = true;
                    break;
                case 2:
                    this.priorityComboBox.IsEnabled = false;
                    this.categoryBox.IsEnabled = false;
                    this.timePicker.IsEnabled = false;
                    this.labelTextBox.IsEnabled = false;

                    this.noteTextbox.IsEnabled = true;
                    break;
            }
        }

        private bool CheckForEvent()
        {
            if (this.categoryBox.SelectedItem == null)
            {
                MessageBox.Show("Please select category");
                return false;
            }

            if (this.priorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select priority");
                return false;
            }

            return true;
        }

        private void SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            this.BlankFields();
        }
    }
}