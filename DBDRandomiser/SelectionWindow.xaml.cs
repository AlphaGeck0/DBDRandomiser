// SelectionWindow.xaml.cs
using System.Collections.Generic;
using System.Windows;

namespace DBDRandomiser
{
    public partial class SelectionWindow : Window
    {
        public List<string> selectedSurvivors { get; private set; }
        public List<string> selectedKillers { get; private set; }
        public List<string> selectedSurvivorPerks { get; private set; }
        public List<string> selectedKillerPerks { get; private set; }

        public SelectionWindow(List<string> allSurvivors, List<string> allKillers, List<string>? selectedSurvivors = null, List<string>? selectedKillers = null,
                               List<string>? allSurvivorPerks = null, List<string>? allKillerPerks = null, List<string>? selectedSurvivorPerks = null, 
                               List<string>? selectedKillerPerks = null)
        {
            InitializeComponent();

            SurvivorsListBox.ItemsSource = allSurvivors;
            KillersListBox.ItemsSource = allKillers;
            SurvivorPerksListBox.ItemsSource = allSurvivorPerks ?? new List<string>();
            KillerPerksListBox.ItemsSource = allKillerPerks ?? new List<string>();

            // Select previously selected items, or all if none provided
            if (selectedSurvivors != null && selectedSurvivors.Count > 0)
                foreach (var s in selectedSurvivors) SurvivorsListBox.SelectedItems.Add(s);
            else SurvivorsListBox.SelectAll();

            if (selectedKillers != null && selectedKillers.Count > 0)
                foreach (var k in selectedKillers) KillersListBox.SelectedItems.Add(k);
            else KillersListBox.SelectAll();

            if (selectedSurvivorPerks != null && selectedSurvivorPerks.Count > 0)
                foreach (var p in selectedSurvivorPerks) SurvivorPerksListBox.SelectedItems.Add(p);
            else SurvivorPerksListBox.SelectAll();

            if (selectedKillerPerks != null && selectedKillerPerks.Count > 0)
                foreach (var p in selectedKillerPerks) KillerPerksListBox.SelectedItems.Add(p);
            else KillerPerksListBox.SelectAll();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            selectedSurvivors = new List<string>();
            foreach (var item in SurvivorsListBox.SelectedItems)
                selectedSurvivors.Add(item.ToString());

            selectedKillers = new List<string>();
            foreach (var item in KillersListBox.SelectedItems)
                selectedKillers.Add(item.ToString());

            selectedSurvivorPerks = new List<string>();
            foreach (var item in SurvivorPerksListBox.SelectedItems)
                selectedSurvivorPerks.Add(item.ToString());

            selectedKillerPerks = new List<string>();
            foreach (var item in KillerPerksListBox.SelectedItems)
                selectedKillerPerks.Add(item.ToString());

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