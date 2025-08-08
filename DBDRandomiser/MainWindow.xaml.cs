using System;
using System.Diagnostics.Eventing.Reader; // Added for LINQ (used in shuffling)
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives; // Add this for ViewBox
using System.Windows.Input; // Required for mouse events
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace DBDRandomiser
{
    public partial class MainWindow : Window
    {
        #region Fields

        // --- Character Data Sources ---
        // Arrays and dictionaries holding all available killers, survivors, and their perks/images.
        private string[] killers = Killers.List;
        private string[] killerPerks = KillerPerks.List;
        private Dictionary<string, string> killerPerkImages = KillerPerkImages.KillerMap;
        private string[] survivors = Survivors.List;
        private string[] survivorPerks = SurvivorPerks.List;
        private Dictionary<string, string> survivorPerkImages = SurvivorPerkImages.SurvivorMap;
        private string version = "4.1";

        // --- UI Animation State ---
        // Timer and index for sequentially revealing perks with animation.
        private System.Windows.Threading.DispatcherTimer? timer;
        private int index;

        // --- User Selections ---
        // Lists tracking which killers, survivors, and perks are currently active/selected by the user.
        private List<string>? activeSurvivors;
        private List<string>? activeKillers;
        private List<string>? activeSurvivorPerks;
        private List<string>? activeKillerPerks;

        #endregion

        #region File Paths

        // --- File Path for User Selections ---
        // Used to persist user choices between sessions.
        private static string SelectionFilePath =>
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DBDRandomiser", "user_selections.json");

        #endregion

        #region Persistence

        // --- Save and Load User Selections ---
        // Handles reading and writing user selection data to disk.
        private void SaveUserSelections()
        {
            var selection = new CharacterSelection
            {
                SelectedSurvivors = activeSurvivors ?? Survivors.List.ToList(),
                SelectedKillers = activeKillers ?? Killers.List.ToList(),
                SelectedSurvivorPerks = activeSurvivorPerks ?? SurvivorPerks.List.ToList(),
                SelectedKillerPerks = activeKillerPerks ?? KillerPerks.List.ToList()
            };
            Directory.CreateDirectory(Path.GetDirectoryName(SelectionFilePath)!);
            File.WriteAllText(SelectionFilePath, JsonSerializer.Serialize(selection));
        }

        private void LoadUserSelections()
        {
            if (File.Exists(SelectionFilePath))
            {
                var selection = JsonSerializer.Deserialize<CharacterSelection>(File.ReadAllText(SelectionFilePath));
                activeSurvivors = selection?.SelectedSurvivors ?? Survivors.List.ToList();
                activeKillers = selection?.SelectedKillers ?? Killers.List.ToList();
                activeSurvivorPerks = selection?.SelectedSurvivorPerks ?? SurvivorPerks.List.ToList();
                activeKillerPerks = selection?.SelectedKillerPerks ?? KillerPerks.List.ToList();
            }
            else
            {
                activeSurvivors = Survivors.List.ToList();
                activeKillers = Killers.List.ToList();
                activeSurvivorPerks = SurvivorPerks.List.ToList();
                activeKillerPerks = KillerPerks.List.ToList();
            }
        }

        #endregion

        #region Constructor

        // --- Window Initialization ---
        // Loads user selections and ensures the window is visible and active.
        public MainWindow()
        {
            InitializeComponent();
            LoadUserSelections();

            // Ensure the window is visible
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        #endregion

        #region Utility Methods

        // --- String Utility ---
        // Removes diacritics from character names for file path compatibility.
        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        // --- UI Helpers ---
        // Resets the timer used for perk animations.
        private void ResetTimer()
        {
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
                timer = null;
            }
        }

        // Sets the initial state for character UI elements before animation.
        private void ShowCharacterUI()
        {
            CharacterName.Opacity = 0;
            CharacterName.Visibility = Visibility.Visible;
            PerksContainer.Opacity = 0;
            PerksContainer.Visibility = Visibility.Visible;
            CharacterImage.Opacity = 0;
            CharacterImage.Visibility = Visibility.Visible;
        }

        private void ShowPerks(Dictionary<string, string> perkImages, List<string> perkPool)
        {
            Random rand = new Random();
            List<string> shuffledPerks = perkPool.OrderBy(x => rand.Next()).Distinct().ToList();
            string[] selectedPerks = shuffledPerks.Take(4).ToArray();

            index = 0;
            timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };

            timer.Tick += (s, args) =>
            {
                if (index >= selectedPerks.Length)
                {
                    timer.Stop();
                    return;
                }

                string perk = selectedPerks[index];

                StackPanel perkPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(15, 0, 15, 0),
                    Opacity = 0
                };

                if (perkImages.TryGetValue(perk, out var perkImageUri) && perkImageUri != null)
                {
                    try
                    {
                        Image perkImage = new Image
                        {
                            Source = new BitmapImage(new Uri(perkImageUri, UriKind.Absolute)),
                            Height = 50,
                            Margin = new Thickness(0, 0, 0, 5)
                        };
                        perkPanel.Children.Add(perkImage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image for perk '{perk}': {ex.Message}");
                    }
                }

                TextBlock perkText = new TextBlock
                {
                    Text = perk,
                    Foreground = System.Windows.Media.Brushes.White,
                    FontSize = 15,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                };
                perkPanel.Children.Add(perkText);

                PerksContainer.Children.Add(perkPanel);

                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                perkPanel.BeginAnimation(OpacityProperty, fadeInAnimation);

                index++;
            };

            timer.Start();
        }

        //private StackPanel CreateUI(string imagePath, string username)
        //{
        //    StackPanel userStack = new StackPanel()
        //    {
        //        Orientation = System.Windows.Controls.Orientation.Horizontal,
        //        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
        //        Margin = new Thickness(36, 24, 0, 0)
        //    };

        //    Image profilePic = new Image()
        //    {
        //        Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
        //        Name = "imgProfile",
        //        Height = 100,
        //        Width = 100,
        //        Margin = new Thickness(0, 0, 6, 0)
        //    };

        //    TextBlock userName = new TextBlock()
        //    {
        //        Text = username,
        //        Name = "txblkUserName",
        //        Foreground = new SolidColorBrush(Colors.White),
        //        FontSize = 32,
        //        Margin = new Thickness(0, 12, 0, 0)
        //    };


        //    userStack.Children.Add(profilePic);
        //    userStack.Children.Add(userName);
        //    return userStack;
        //}

        #endregion

        #region Event Handlers

        // --- Button and UI Event Handlers ---
        // Handles all user interactions from the UI.
        private void SelectKillerSurvivor_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            if (rand.Next(2) == 0)
                SelectKiller_Click(sender, e);
            else
                SelectSurvivor_Click(sender, e);
        }

        private void SelectKiller_Click(object sender, RoutedEventArgs e)
        {
            // Stop and reset the timer if it's already running
            ResetTimer();

            ShowCharacterUI();

            // Trigger the FadeInStoryboard for each element
            Storyboard? fadeInStoryboard = this.Resources["FadeInStoryboard"] as Storyboard;

            if (fadeInStoryboard != null)
            {
                Storyboard.SetTarget(fadeInStoryboard, CharacterName);
                fadeInStoryboard.Begin();

                Storyboard.SetTarget(fadeInStoryboard, CharacterImage);
                fadeInStoryboard.Begin();

                Storyboard.SetTarget(fadeInStoryboard, PerksContainer);
                fadeInStoryboard.Begin();

                if (background_image2.Opacity != 0)
                {
                    background_image2.Opacity = 1;
                }
                else
                {
                    Storyboard.SetTarget(fadeInStoryboard, background_image2);
                    fadeInStoryboard.Begin();
                }
            }

            // Clear the PerksContainer to avoid duplicates
            PerksContainer.Children.Clear();

            // Create a Random instance
            Random rand = new Random();

            // Select a random Killer
            string[] killerPool = (activeKillers != null && activeKillers.Count > 0) ? activeKillers.ToArray() : killers;
            string selectedKiller = killerPool[rand.Next(killerPool.Length)];
            CharacterName.Text = selectedKiller; // Update the text
            CharacterName.Opacity = 1; // Ensure the label is visible

            // Load the killer image dynamically
            string CharacterImageUri = $"pack://application:,,,/killer_images/{selectedKiller.Replace(" ", "_").ToLower()}.png";
            try
            {
                CharacterImage.Source = new BitmapImage(new Uri(CharacterImageUri, UriKind.Absolute));
                CharacterImage.Opacity = 1;
                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                CharacterImage.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading killer image: {ex.Message}");
            }

            PerksContainer.Opacity = 1; // Ensure the perks container is visible

            if (IncludePerksCheckBox.IsChecked == true)
                ShowPerks(killerPerkImages, activeKillerPerks ?? killerPerks.ToList());
        }

        private void SelectSurvivor_Click(object sender, RoutedEventArgs e)
        {
            // Stop and reset the timer if it's already running
            ResetTimer();

            ShowCharacterUI();

            // Trigger the FadeInStoryboard for each element
            Storyboard? fadeInStoryboard = this.Resources["FadeInStoryboard"] as Storyboard;
            Storyboard? fadeOutStoryboard = this.Resources["FadeOutStoryboard"] as Storyboard;

            if (fadeInStoryboard != null)
            {
                Storyboard.SetTarget(fadeInStoryboard, CharacterName);
                fadeInStoryboard.Begin();

                Storyboard.SetTarget(fadeInStoryboard, CharacterImage);
                fadeInStoryboard.Begin();

                Storyboard.SetTarget(fadeInStoryboard, PerksContainer);
                fadeInStoryboard.Begin();

                if (background_image2.Opacity == 0)
                {
                    background_image2.Opacity = 0;
                }
                else if (fadeOutStoryboard != null)
                {
                    Storyboard.SetTarget(fadeOutStoryboard, background_image2);
                    fadeOutStoryboard.Begin();
                }
            }

            // Clear the PerksContainer to avoid duplicates
            PerksContainer.Children.Clear();

            // Create a Random instance
            Random rand = new Random();

            // Select a random Survivor
            string[] survivorPool = (activeSurvivors != null && activeSurvivors.Count > 0) ? activeSurvivors.ToArray() : survivors;
            string selectedSurvivor = survivorPool[rand.Next(survivorPool.Length)];
            CharacterName.Text = selectedSurvivor; // Update the text
            CharacterName.Opacity = 1; // Ensure the label is visible

            // Load the survivor image dynamically
            string fileName = RemoveDiacritics(selectedSurvivor).Replace(" ", "_").ToLower() + ".png";
            string CharacterImageUri = $"pack://application:,,,/survivor_images/{fileName}";
            try
            {
                CharacterImage.Source = new BitmapImage(new Uri(CharacterImageUri, UriKind.Absolute));
                CharacterImage.Opacity = 1; // Ensure the image is visible

                // Apply a fade-in animation to the CharacterImage
                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                CharacterImage.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading survivor image: {ex.Message}");
            }
            PerksContainer.Opacity = 1; // Ensure the perks container is visible

            if (IncludePerksCheckBox.IsChecked == true)
                ShowPerks(survivorPerkImages, activeSurvivorPerks ?? survivorPerks.ToList());
        }

        private void ResetScreen_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the fade-out Storyboard from resources
            Storyboard? fadeOutStoryboard = this.Resources["FadeOutStoryboard"] as Storyboard;

            if (fadeOutStoryboard != null)
            {
                // Create a clone of the storyboard for each element
                Storyboard CharacterNameFadeOut = fadeOutStoryboard.Clone();
                Storyboard perksContainerFadeOut = fadeOutStoryboard.Clone();
                Storyboard background_image2FadeOut = fadeOutStoryboard.Clone();

                // Attach a completion handler to the storyboard for CharacterName
                CharacterNameFadeOut.Completed += (s, ev) =>
                {
                    CharacterName.Text = ""; // Clear the text
                    CharacterName.Opacity = 1; // Reset opacity
                    CharacterName.Visibility = Visibility.Visible; // Ensure visibility
                };

                // Attach a completion handler to the storyboard for PerksContainer
                perksContainerFadeOut.Completed += (s, ev) =>
                {
                    PerksContainer.Children.Clear(); // Clear the perks container
                    PerksContainer.Opacity = 1; // Reset opacity
                    PerksContainer.Visibility = Visibility.Visible; // Ensure visibility
                };

                // Attach a completion handler to the storyboard for background_image2
                background_image2FadeOut.Completed += (s, ev) =>
                {
                    background_image2.Opacity = 0; // Reset opacity
                    background_image2.Visibility = Visibility.Visible; // Ensure visibility
                };

                CharacterImage.Source = null; // Clear the image source

                // Set the target for each storyboard
                Storyboard.SetTarget(CharacterNameFadeOut, CharacterName);
                Storyboard.SetTarget(perksContainerFadeOut, PerksContainer);
                Storyboard.SetTarget(background_image2FadeOut, background_image2);

                // Start the fade-out animations
                CharacterNameFadeOut.Begin();
                perksContainerFadeOut.Begin();
                if (background_image2.Opacity == 1)
                {
                    background_image2FadeOut.Begin();
                }

            }
            else
            {
                MessageBox.Show("FadeOutStoryboard not found in resources.");
            }

            timer?.Stop(); // Stop the timer if it's running
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) // Detect left mouse button press
            {
                this.DragMove(); // Allows the window to be dragged
            }
        }

        private void Character_Click(object sender, RoutedEventArgs e)
        {
            OpenSelectionWindow();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DBD Randomiser " + version + "\n\n" +
                            "This application allows you to randomly select characters and perks from Dead by Daylight.\n" +
                            "You can customize your selections and save them for future use.\n\n" +
                            "Developed by AlphaGecko\n" +
                            "For more information, contact me @ twitch.tv/alphageck0.", "About DBD Randomiser", MessageBoxButton.OK, MessageBoxImage.Information);
            {
            }
        }


        private void OpenSelectionWindow()
        {
            var selectionWindow = new SelectionWindow(
        Survivors.List.ToList(),
        Killers.List.ToList(),
        activeSurvivors,
        activeKillers,
        SurvivorPerks.List.ToList(),
        KillerPerks.List.ToList(),
        activeSurvivorPerks,
        activeKillerPerks);

            if (selectionWindow.ShowDialog() == true)
            {
                activeSurvivors = selectionWindow.selectedSurvivors;
                activeKillers = selectionWindow.selectedKillers;
                activeSurvivorPerks = selectionWindow.selectedSurvivorPerks;
                activeKillerPerks = selectionWindow.selectedKillerPerks;
                SaveUserSelections();
            }
        }
        #endregion
    }
}