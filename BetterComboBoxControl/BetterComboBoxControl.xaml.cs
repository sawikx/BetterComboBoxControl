using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BetterComboBoxControl
{    
    public partial class BetterComboBoxControl : UserControl
    {
        public BetterComboBoxControl()
        {
            InitializeComponent();
            //ValueComboBox(0);
        }

        // The property you will be binding (final result)
        public static readonly DependencyProperty CurrentSelectionProperty =
        DependencyProperty.Register("CurrentSelection", typeof(string), typeof(BetterComboBoxControl),
            new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCurrentSelectionChanged)); 

        public string CurrentSelection
        {
            get { return (string)GetValue(CurrentSelectionProperty); }
            set { SetValue(CurrentSelectionProperty, value); }
        }

        // This method will execute when the DataGrid binding “pushes” a value into the control
        private static void OnCurrentSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BetterComboBoxControl)d;
            control.UpdateUIFromSelection((string)e.NewValue);
        }
        public int SelectionOptionsSet
        {
            get { return (int)GetValue(SelectionOptionsSetProperty); }
            set { SetValue(SelectionOptionsSetProperty, value); }
        }

        public static readonly DependencyProperty SelectionOptionsSetProperty =
        DependencyProperty.Register("SelectionOptionsSet", typeof(int), typeof(BetterComboBoxControl),
            new FrameworkPropertyMetadata(-1,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectionOptionsSetChanged)); 
        private static void OnSelectionOptionsSetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BetterComboBoxControl)d;
            control.ValueComboBox((int)e.NewValue);
        }
        public void ValueComboBox( string[] D) //custom value to ComboBox
        {
            SelectionComboBox.Items.Clear();
            foreach (string s in D) 
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = s;
                SelectionComboBox.Items.Add(item);
            }
        }
        public void ValueComboBox(int WhichCollection) //Selecting a value from a collection
        {
            SelectionComboBox.Items.Clear();
            string[] Value;
            switch (WhichCollection)
            {
                case 0:
                default:
                    Value = ["a1", "a2", "a3", "a4"];                   
                    foreach (string i in Value)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = i;
                        SelectionComboBox.Items.Add(item);
                    }
                    break;
                case 1:
                    Value = ["b1", "b2", "b3"];                   
                    foreach (string i in Value)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = i;
                        SelectionComboBox.Items.Add(item);
                    }                                        
                    break;
                case 2:
                    Value = ["c1", "c2", "c3"];
                    foreach (string i in Value)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = i;
                        SelectionComboBox.Items.Add(item);
                    }                                        
                    break;                
            }

        }

        // Handling changes in a ComboBox
        private void SelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomInputGrid == null) return;

            var selectedItem = (SelectionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            
            if (selectedItem == "a2" || SelectionComboBox.SelectedIndex == (SelectionComboBox.Items.Count-1)) //last element from ComboBox or custom to show txt Input
            {
                CustomInputGrid.Visibility = Visibility.Visible;
                CurrentSelection = txtInput.Text; // txt Input Visible
            }
            else
            {
                CustomInputGrid.Visibility = Visibility.Collapsed;
                CurrentSelection = selectedItem; // txt Input Collapsed
            }
        }

        // logic for the TextBox (Ghost Text)
        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbBackgroundText == null) return;

            tbBackgroundText.Visibility = string.IsNullOrEmpty(txtInput.Text)
                ? Visibility.Visible : Visibility.Hidden;

            if (SelectionComboBox.SelectedIndex == (SelectionComboBox.Items.Count - 1))
            {
                CurrentSelection = txtInput.Text;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }

        // Logic for setting the UI based on text 
        private void UpdateUIFromSelection(string Selection)
        {
            if (string.IsNullOrEmpty(Selection)) return;

            bool found = false;
            foreach (ComboBoxItem item in SelectionComboBox.Items)
            {
                if (item.Content.ToString() == Selection)
                {
                    SelectionComboBox.SelectionChanged -= SelectionComboBox_SelectionChanged; 
                    SelectionComboBox.SelectedItem = item;
                    SelectionComboBox.SelectionChanged += SelectionComboBox_SelectionChanged;
                    CustomInputGrid.Visibility = Visibility.Collapsed;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                SelectionComboBox.SelectionChanged -= SelectionComboBox_SelectionChanged;
                SelectionComboBox.SelectedIndex = (SelectionComboBox.Items.Count - 1); // Last option
                SelectionComboBox.SelectionChanged += SelectionComboBox_SelectionChanged;

                CustomInputGrid.Visibility = Visibility.Visible;
                if (txtInput.Text != Selection) txtInput.Text = Selection;
            }
        }
    }
}
