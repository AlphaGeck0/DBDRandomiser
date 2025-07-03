// SelectionWindow.xaml.cs
using System.Collections.Generic;
using System.Windows;

namespace DBDRandomiser
{
    public partial class SelectionWindow : Window
    {
        public List<string> SelectedSurvivors { get; private set; }
        public List<string> SelectedKillers { get; private set; }

        public SelectionWindow(List<string> allSurvivors, List<string> allKillers, List<string>? selectedSurvivors = null, List<string>? selectedKillers = null)
        {
            InitializeComponent();

            SurvivorsListBox.ItemsSource = allSurvivors;
            KillersListBox.ItemsSource = allKillers;

            // Select previously selected items, or all if none provided
            if (selectedSurvivors != null && selectedSurvivors.Count > 0)
            {
                foreach (var survivor in selectedSurvivors)
                    SurvivorsListBox.SelectedItems.Add(survivor);
            }
            else
            {
                SurvivorsListBox.SelectAll();
            }

            if (selectedKillers != null && selectedKillers.Count > 0)
            {
                foreach (var killer in selectedKillers)
                    KillersListBox.SelectedItems.Add(killer);
            }
            else
            {
                KillersListBox.SelectAll();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedSurvivors = new List<string>();
            foreach (var item in SurvivorsListBox.SelectedItems)
                SelectedSurvivors.Add(item.ToString());

            SelectedKillers = new List<string>();
            foreach (var item in KillersListBox.SelectedItems)
                SelectedKillers.Add(item.ToString());

            DialogResult = true;
            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}