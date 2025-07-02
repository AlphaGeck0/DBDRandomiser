using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input; // Required for mouse events
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives; // Add this for ViewBox
using System.Linq;
using System.Diagnostics.Eventing.Reader; // Added for LINQ (used in shuffling)

namespace DBDRandomiser
{
    public partial class MainWindow : Window
    {
        private string[] killers = Killers.List;
        private string[] killerPerks = KillerPerks.List;
        private Dictionary<string, string> killerPerkImages = KillerPerkImages.KillerMap;


        private string[] survivors = Survivors.List;
        private string[] survivorPerks = SurvivorPerks.List;
        private Dictionary<string, string> survivorPerkImages = SurvivorPerkImages.SurvivorMap;

        // Declare timer and index as class-level fields
        private System.Windows.Threading.DispatcherTimer? timer;
        private int index;

        public MainWindow()
        {
            InitializeComponent();

            // Ensure the window is visible
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

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

        private void SelectKiller_Click(object sender, RoutedEventArgs e)
        {
            // Stop and reset the timer if it's already running
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
                timer = null;
            }

            // Ensure the CharacterName and PerksContainer are visible
            CharacterName.Opacity = 0;
            CharacterName.Visibility = Visibility.Visible;

            PerksContainer.Opacity = 0;
            PerksContainer.Visibility = Visibility.Visible;

            CharacterImage.Opacity = 0;
            CharacterImage.Visibility = Visibility.Visible;

            // Trigger the FadeInStoryboard for each element
            Storyboard fadeInStoryboard = this.Resources["FadeInStoryboard"] as Storyboard;

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
            string selectedKiller = killers[rand.Next(killers.Length)];
            CharacterName.Text = selectedKiller; // Update the text
            CharacterName.Opacity = 1; // Ensure the label is visible

            // Load the killer image dynamically
            string CharacterImageUri = $"pack://application:,,,/killer_images/{selectedKiller.Replace(" ", "_").ToLower()}.png";
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
                MessageBox.Show($"Error loading killer image: {ex.Message}");
            }
            PerksContainer.Opacity = 1; // Ensure the perks container is visible

            try
            {
                CharacterImage.Source = new BitmapImage(new Uri(CharacterImageUri, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading killer image: {ex.Message}");
            }

            if (IncludePerksCheckBox.IsChecked == true)
            {
                // Shuffle the perk list and pick the first four unique perks
                List<string> shuffledPerks = killerPerks.OrderBy(x => rand.Next()).Distinct().ToList();
                string[] selectedPerks = shuffledPerks.Take(4).ToArray();

                // Initialize the index and timer
                index = 0;
                timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(1000) // Delay between each perk
                };

                timer.Tick += (s, args) =>
                {
                    if (index >= selectedPerks.Length)
                    {
                        timer.Stop(); // Stop the timer when all perks are added
                        return;
                    }

                    string perk = selectedPerks[index];

                    // Create a StackPanel to hold the image and text
                    StackPanel perkPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(15, 0, 15, 0), // Adds spacing between perks
                        Opacity = 0 // Start with 0 opacity for fade-in animation
                    };

                    // Add the perk image
                    if (killerPerkImages.TryGetValue(perk, out string perkImageUri))
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

                    // Add the perk text
                    TextBlock perkText = new TextBlock
                    {
                        Text = perk,
                        Foreground = System.Windows.Media.Brushes.White,
                        FontSize = 15,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                    };
                    perkPanel.Children.Add(perkText);

                    // Add the StackPanel to the PerksContainer
                    PerksContainer.Children.Add(perkPanel);

                    // Apply a fade-in animation to the perk
                    var fadeInAnimation = new DoubleAnimation
                    {
                        From = 0.0,
                        To = 1.0,
                        Duration = TimeSpan.FromMilliseconds(500)
                    };
                    perkPanel.BeginAnimation(OpacityProperty, fadeInAnimation);

                    index++; // Move to the next perk
                };

                timer.Start(); // Start the timer
            }
        }

        private void SelectSurvivor_Click(object sender, RoutedEventArgs e)
        {
            // Stop and reset the timer if it's already running
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
                timer = null;
            }

            // Ensure the CharacterName and PerksContainer are visible
            CharacterName.Opacity = 0;
            CharacterName.Visibility = Visibility.Visible;

            PerksContainer.Opacity = 0;
            PerksContainer.Visibility = Visibility.Visible;

            CharacterImage.Opacity = 0;
            CharacterImage.Visibility = Visibility.Visible;

            // Trigger the FadeInStoryboard for each element
            Storyboard fadeInStoryboard = this.Resources["FadeInStoryboard"] as Storyboard;
            Storyboard fadeOutStoryboard = this.Resources["FadeOutStoryboard"] as Storyboard;

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
                else
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
            string selectedSurvivor = survivors[rand.Next(survivors.Length)];
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

            try
            {
                CharacterImage.Source = new BitmapImage(new Uri(CharacterImageUri, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading survivor image: {ex.Message}");
            }

            if (IncludePerksCheckBox.IsChecked == true)
            {
                // Shuffle the perk list and pick the first four unique perks
                List<string> shuffledPerks = survivorPerks.OrderBy(x => rand.Next()).Distinct().ToList();
                string[] selectedPerks = shuffledPerks.Take(4).ToArray();

                // Initialize the index and timer
                index = 0;
                timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(1000) // Delay between each perk
                };

                timer.Tick += (s, args) =>
                {
                    if (index >= selectedPerks.Length)
                    {
                        timer.Stop(); // Stop the timer when all perks are added
                        return;
                    }

                    string perk = selectedPerks[index];

                    // Create a StackPanel to hold the image and text
                    StackPanel perkPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(15, 0, 15, 0), // Adds spacing between perks
                        Opacity = 0 // Start with 0 opacity for fade-in animation
                    };

                    // Add the perk image
                    if (survivorPerkImages.TryGetValue(perk, out string perkImageUri))
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

                    // Add the perk text
                    TextBlock perkText = new TextBlock
                    {
                        Text = perk,
                        Foreground = System.Windows.Media.Brushes.White,
                        FontSize = 15,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                    };
                    perkPanel.Children.Add(perkText);

                    // Add the StackPanel to the PerksContainer
                    PerksContainer.Children.Add(perkPanel);

                    // Apply a fade-in animation to the perk
                    var fadeInAnimation = new DoubleAnimation
                    {
                        From = 0.0,
                        To = 1.0,
                        Duration = TimeSpan.FromMilliseconds(500)
                    };
                    perkPanel.BeginAnimation(OpacityProperty, fadeInAnimation);

                    index++; // Move to the next perk
                };

                timer.Start(); // Start the timer
            }
        }

        private void ResetScreen_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the fade-out Storyboard from resources
            Storyboard fadeOutStoryboard = this.Resources["FadeOutStoryboard"] as Storyboard;

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
    }
}