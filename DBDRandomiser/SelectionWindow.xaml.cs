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

        public SelectionWindow(
            List<string> allSurvivors, List<string> allKillers,
            List<string>? selectedSurvivors = null, List<string>? selectedKillers = null,
            List<string>? allSurvivorPerks = null, List<string>? allKillerPerks = null,
            List<string>? selectedSurvivorPerks = null, List<string>? selectedKillerPerks = null)
        {
            InitializeComponent();

            selectedSurvivors = selectedSurvivors ?? new List<string>();
            selectedKillers = selectedKillers ?? new List<string>();
            selectedSurvivorPerks = selectedSurvivorPerks ?? new List<string>();
            selectedKillerPerks = selectedKillerPerks ?? new List<string>();

            SurvivorsListBox.ItemsSource = allSurvivors;
            KillersListBox.ItemsSource = allKillers;
            SurvivorPerksListBox.ItemsSource = allSurvivorPerks ?? new List<string>();
            KillerPerksListBox.ItemsSource = allKillerPerks ?? new List<string>();

            // Select previously selected items, or all if none provided
            if (selectedSurvivors.Count > 0)
                foreach (var s in selectedSurvivors) SurvivorsListBox.SelectedItems.Add(s);
            else SurvivorsListBox.SelectAll();

            if (selectedKillers.Count > 0)
                foreach (var k in selectedKillers) KillersListBox.SelectedItems.Add(k);
            else KillersListBox.SelectAll();

            if (selectedSurvivorPerks.Count > 0)
                foreach (var p in selectedSurvivorPerks) SurvivorPerksListBox.SelectedItems.Add(p);
            else SurvivorPerksListBox.SelectAll();

            if (selectedKillerPerks.Count > 0)
                foreach (var p in selectedKillerPerks) KillerPerksListBox.SelectedItems.Add(p);
            else KillerPerksListBox.SelectAll();
        }

        private void Grid_Items_Mass_Selection(string selectionType)
        {
            switch (selectionType)
            {
                case "SelectSurvivors":
                    SurvivorsListBox.SelectAll();
                    break;
                case "UnselectSurvivors":
                    SurvivorsListBox.UnselectAll();
                    break;
                case "SelectKillers":
                    KillersListBox.SelectAll();
                    break;
                case "UnselectKillers":
                    KillersListBox.UnselectAll();
                    break;
                case "SelectSurvivorPerks":
                    SurvivorPerksListBox.SelectAll();
                    break;
                case "UnselectSurvivorPerks":
                    SurvivorPerksListBox.UnselectAll();
                    break;
                case "SelectKillerPerks":
                    KillerPerksListBox.SelectAll();
                    break;
                case "UnselectKillerPerks":
                    KillerPerksListBox.UnselectAll();
                    break;
            }
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

        private void SelectAllSurvivors_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("SelectSurvivors");
        }

        private void RemoveAllSurvivors_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("UnselectSurvivors");
        }

        private void SelectAllKillers_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("SelectKillers");
        }

        private void RemoveAllKillers_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("UnselectKillers");
        }

        private void SelectAllSurvivorPerks_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("SelectSurvivorPerks");
        }

        private void RemoveAllSurvivorPerks_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("UnselectSurvivorPerks");
        }

        private void SelectAllKillerPerks_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("SelectKillerPerks");
        }

        private void RemoveAllKillerPerks_Click(object sender, RoutedEventArgs e)
        {
            Grid_Items_Mass_Selection("UnselectKillerPerks");
        }
    }
}