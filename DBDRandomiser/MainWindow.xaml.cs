using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input; // Required for mouse events
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives; // Add this for ViewBox
using System.Linq; // Added for LINQ (used in shuffling)

namespace DBDKillerSelector
{
    public partial class MainWindow : Window
    {
        private string[] killers = {
                "The Trapper", "The Wraith", "The Hillbilly", "The Nurse", "The Shape", "The Hag", "The Doctor", "The Huntress", "The Cannibal", "The Nightmare","The Pig",
                "The Clown","The Spirit","The Legion","The Plague","The Ghost Face","The Demogorgon","The Oni","The Deathslinger","The Executioner","The Blight","The Twins",
                "The Trickster","The Nemesis","The Cenobite","The Artist","The Onryo","The Dredge","The Mastermind","The Knight","The Skull Merchant","The Singularity","The Xenomorph",
                "The Good Guy","The Unknown","The Lich","The Dark Lord","The Houndmaster","The Ghoul"
            };

        private string[] perks = {
                "A Nurse's Calling",
                "Agitation",
                "Alien Instinct",
                "All-Shaking Thunder",
                "Awakened Awareness",
                "Bamboozle",
                "Barbecue & Chilli",
                "Batteries Included",
                "Beast of Prey",
                "Bitter Murmur",
                "Blood Echo",
                "Blood Warden",
                "Bloodhound",
                "Brutal Strength",
                "Call of Brine",
                "Corrupt Intervention",
                "Coulrophobia",
                "Coup de Grace",
                "Cruel Limits",
                "Dark Arrogance",
                "Dark Devotion",
                "Darkness Revealed",
                "Dead Man's Switch",
                "Deadlock",
                "Deathbound",
                "Deerstalker",
                "Discordance",
                "Dissolution",
                "Distressing",
                "Dominance",
                "Dragon's Grip",
                "Dying Light",
                "Enduring",
                "Eruption",
                "Fire Up",
                "Forced Hesitation",
                "Forced Penance",
                "Forever Entwined",
                "Franklin's Demise",
                "Friends 'til the End",
                "Furtive Chase",
                "Game Afoot",
                "Gearhead",
                "Genetic Limits",
                "Grim Embrace",
                "Hex: Blood Favor",
                "Hex: Crowd Control",
                "Hex: Devour Hope",
                "Hex: Face the Darkness",
                "Hex: Haunted Ground",
                "Hex: Huntress Lullaby",
                "Hex: No One Escapes Death",
                "Hex: Nothing but Misery",
                "Hex: Pentimento",
                "Hex: Plaything",
                "Hex: Retribution",
                "Hex: Ruin",
                "Hex: The Third Seal",
                "Hex: Thrill of the Hunt",
                "Hex: Two Can Play",
                "Hex: Undying",
                "Hex: Wretched Fate",
                "Hoarder",
                "Hubris",
                "Human Greed",
                "Hysteria",
                "I'm All Ears",
                "Infectious Fright",
                "Insidious",
                "Iron Grasp",
                "Iron Maiden",
                "Knock Out",
                "Languid Touch",
                "Lethal Pursuer",
                "Leverage",
                "Lightborn",
                "Machine Learning",
                "Mad Grit",
                "Make Your Choice",
                "Merciless Storm",
                "Mindbreaker",
                "Monitor & Abuse",
                "Nemesis",
                "No Quarter",
                "No Way Out",
                "None are Free",
                "Nowhere to Hide",
                "Oppression",
                "Overcharge",
                "Overwhelming Presence",
                "Play with Your Food",
                "Pop Goes the Weasel",
                "Predator",
                "Rancor",
                "Rapid Brutality",
                "Remember Me",
                "Save the Best for Last",
                "Scourge Hook: Floods of Rage",
                "Scourge Hook: Gift of Pain",
                "Scourge Hook: Hangman's Trick",
                "Scourge Hook: Jagged Compass",
                "Scourge Hook: Monstrous Shrine",
                "Scourge Hook: Pain Resonance",
                "Septic Touch",
                "Shadowborn",
                "Shattered Hope",
                "Sloppy Butcher",
                "Spies from the Shadows",
                "Spirit Fury",
                "Starstruck",
                "Stridor",
                "Superior Anatomy",
                "Surge",
                "Surveillance",
                "Terminus",
                "Territorial Imperative",
                "Thanatophobia",
                "Thrilling Tremors",
                "THWACK!",
                "Tinkerer",
                "Trail of Torment",
                "Ultimate Weapon",
                "Unbound",
                "Undone",
                "Unforeseen",
                "Unnerving Presence",
                "Unrelenting",
                "Weave Attunement",
                "Whispers",
                "Zanshin Tactics"
            };

        private Dictionary<string, string> perkImages = new Dictionary<string, string>
            {
                { "A Nurse's Calling", "pack://application:,,,/perk_images/A_Nurse's_Calling.png" },
                { "Agitation", "pack://application:,,,/perk_images/Agitation.png" },
                { "Alien Instinct", "pack://application:,,,/perk_images/Alien_Instinct.png" },
                { "All-Shaking Thunder", "pack://application:,,,/perk_images/All-Shaking_Thunder.png" },
                { "Awakened Awareness", "pack://application:,,,/perk_images/Awakened_Awareness.png" },
                { "Bamboozle", "pack://application:,,,/perk_images/Bamboozle.png" },
                { "Barbecue & Chilli", "pack://application:,,,/perk_images/Barbecue_&_Chilli.png" },
                { "Batteries Included", "pack://application:,,,/perk_images/Batteries_Included.png" },
                { "Beast of Prey", "pack://application:,,,/perk_images/Beast_of_Prey.png" },
                { "Bitter Murmur", "pack://application:,,,/perk_images/Bitter_Murmur.png" },
                { "Blood Echo", "pack://application:,,,/perk_images/Blood_Echo.png" },
                { "Blood Warden", "pack://application:,,,/perk_images/Blood_Warden.png" },
                { "Bloodhound", "pack://application:,,,/perk_images/Bloodhound.png" },
                { "Brutal Strength", "pack://application:,,,/perk_images/Brutal_Strength.png" },
                { "Call of Brine", "pack://application:,,,/perk_images/Call_of_Brine.png" },
                { "Corrupt Intervention", "pack://application:,,,/perk_images/Corrupt_Intervention.png" },
                { "Coulrophobia", "pack://application:,,,/perk_images/Coulrophobia.png" },
                { "Coup de Grace", "pack://application:,,,/perk_images/Coup_de_Grace.png" },
                { "Cruel Limits", "pack://application:,,,/perk_images/Cruel_Limits.png" },
                { "Dark Arrogance", "pack://application:,,,/perk_images/Dark_Arrogance.png" },
                { "Dark Devotion", "pack://application:,,,/perk_images/Dark_Devotion.png" },
                { "Darkness Revealed", "pack://application:,,,/perk_images/Darkness_Revealed.png" },
                { "Dead Man's Switch", "pack://application:,,,/perk_images/Dead_Man's_Switch.png" },
                { "Deadlock", "pack://application:,,,/perk_images/Deadlock.png" },
                { "Deathbound", "pack://application:,,,/perk_images/Deathbound.png" },
                { "Deerstalker", "pack://application:,,,/perk_images/Deerstalker.png" },
                { "Discordance", "pack://application:,,,/perk_images/Discordance.png" },
                { "Dissolution", "pack://application:,,,/perk_images/Dissolution.png" },
                { "Distressing", "pack://application:,,,/perk_images/Distressing.png" },
                { "Dominance", "pack://application:,,,/perk_images/Dominance.png" },
                { "Dragon's Grip", "pack://application:,,,/perk_images/Dragon's_Grip.png" },
                { "Dying Light", "pack://application:,,,/perk_images/Dying_Light.png" },
                { "Enduring", "pack://application:,,,/perk_images/Enduring.png" },
                { "Eruption", "pack://application:,,,/perk_images/Eruption.png" },
                { "Fire Up", "pack://application:,,,/perk_images/Fire_Up.png" },
                { "Forced Hesitation", "pack://application:,,,/perk_images/Forced_Hesitation.png" },
                { "Forced Penance", "pack://application:,,,/perk_images/Forced_Penance.png" },
                { "Forever Entwined", "pack://application:,,,/perk_images/Forever_Entwined.png" },
                { "Franklin's Demise", "pack://application:,,,/perk_images/Franklins_Demise.png" },
                { "Friends 'til the End", "pack://application:,,,/perk_images/Friends_til_the_End.png" },
                { "Furtive Chase", "pack://application:,,,/perk_images/Furtive_Chase.png" },
                { "Game Afoot", "pack://application:,,,/perk_images/Game_Afoot.png" },
                { "Gearhead", "pack://application:,,,/perk_images/Gearhead.png" },
                { "Genetic Limits", "pack://application:,,,/perk_images/Genetic_Limits.png" },
                { "Grim Embrace", "pack://application:,,,/perk_images/Grim_Embrace.png" },
                { "Hex: Blood Favor", "pack://application:,,,/perk_images/Hex_Blood_Favor.png" },
                { "Hex: Crowd Control", "pack://application:,,,/perk_images/Hex_Crowd_Control.png" },
                { "Hex: Devour Hope", "pack://application:,,,/perk_images/Hex_Devour_Hope.png" },
                { "Hex: Face the Darkness", "pack://application:,,,/perk_images/Hex_Face_the_Darkness.png" },
                { "Hex: Haunted Ground", "pack://application:,,,/perk_images/Hex_Haunted_Ground.png" },
                { "Hex: Huntress Lullaby", "pack://application:,,,/perk_images/Hex_Huntress_Lullaby.png" },
                { "Hex: No One Escapes Death", "pack://application:,,,/perk_images/Hex_No_One_Escapes_Death.png" },
                { "Hex: Nothing but Misery", "pack://application:,,,/perk_images/Hex_Nothing_but_Misery.png" },
                { "Hex: Pentimento", "pack://application:,,,/perk_images/Hex_Pentimento.png" },
                { "Hex: Plaything", "pack://application:,,,/perk_images/Hex_Plaything.png" },
                { "Hex: Retribution", "pack://application:,,,/perk_images/Hex_Retribution.png" },
                { "Hex: Ruin", "pack://application:,,,/perk_images/Hex_Ruin.png" },
                { "Hex: The Third Seal", "pack://application:,,,/perk_images/Hex_The_Third_Seal.png" },
                { "Hex: Thrill of the Hunt", "pack://application:,,,/perk_images/Hex_Thrill_of_the_Hunt.png" },
                { "Hex: Two Can Play", "pack://application:,,,/perk_images/Hex_Two_Can_Play.png" },
                { "Hex: Undying", "pack://application:,,,/perk_images/Hex_Undying.png" },
                { "Hex: Wretched Fate", "pack://application:,,,/perk_images/Hex_Wretched_Fate.png" },
                { "Hoarder", "pack://application:,,,/perk_images/Hoarder.png" },
                { "Hubris", "pack://application:,,,/perk_images/Hubris.png" },
                { "Human Greed", "pack://application:,,,/perk_images/Human_Greed.png" },
                { "Hysteria", "pack://application:,,,/perk_images/Hysteria.png" },
                { "I'm All Ears", "pack://application:,,,/perk_images/I'm_All_Ears.png" },
                { "Infectious Fright", "pack://application:,,,/perk_images/Infectious_Fright.png" },
                { "Insidious", "pack://application:,,,/perk_images/Insidious.png" },
                { "Iron Grasp", "pack://application:,,,/perk_images/Iron_Grasp.png" },
                { "Iron Maiden", "pack://application:,,,/perk_images/Iron_Maiden.png" },
                { "Knock Out", "pack://application:,,,/perk_images/Knock_Out.png" },
                { "Languid Touch", "pack://application:,,,/perk_images/Languid_Touch.png" },
                { "Lethal Pursuer", "pack://application:,,,/perk_images/Lethal_Pursuer.png" },
                { "Leverage", "pack://application:,,,/perk_images/Leverage.png" },
                { "Lightborn", "pack://application:,,,/perk_images/Lightborn.png" },
                { "Machine Learning", "pack://application:,,,/perk_images/Machine_Learning.png" },
                { "Mad Grit", "pack://application:,,,/perk_images/Mad_Grit.png" },
                { "Make Your Choice", "pack://application:,,,/perk_images/Make_Your_Choice.png" },
                { "Merciless Storm", "pack://application:,,,/perk_images/Merciless_Storm.png" },
                { "Mindbreaker", "pack://application:,,,/perk_images/Mindbreaker.png" },
                { "Monitor & Abuse", "pack://application:,,,/perk_images/Monitor_&_Abuse.png" },
                { "Nemesis", "pack://application:,,,/perk_images/Nemesis.png" },
                { "No Quarter", "pack://application:,,,/perk_images/No_Quarter.png" },
                { "No Way Out", "pack://application:,,,/perk_images/No_Way_Out.png" },
                { "None are Free", "pack://application:,,,/perk_images/None_are_Free.png" },
                { "Nowhere to Hide", "pack://application:,,,/perk_images/Nowhere_to_Hide.png" },
                { "Oppression", "pack://application:,,,/perk_images/Oppression.png" },
                { "Overcharge", "pack://application:,,,/perk_images/Overcharge.png" },
                { "Overwhelming Presence", "pack://application:,,,/perk_images/Overwhelming_Presence.png" },
                { "Play with Your Food", "pack://application:,,,/perk_images/Play_with_Your_Food.png" },
                { "Pop Goes the Weasel", "pack://application:,,,/perk_images/Pop_Goes_the_Weasel.png" },
                { "Predator", "pack://application:,,,/perk_images/Predator.png" },
                { "Rancor", "pack://application:,,,/perk_images/Rancor.png" },
                { "Rapid Brutality", "pack://application:,,,/perk_images/Rapid_Brutality.png" },
                { "Remember Me", "pack://application:,,,/perk_images/Remember_Me.png" },
                { "Save the Best for Last", "pack://application:,,,/perk_images/Save_the_Best_for_Last.png" },
                { "Scourge Hook: Floods of Rage", "pack://application:,,,/perk_images/Scourge_Hook_Floods_of_Rage.png" },
                { "Scourge Hook: Gift of Pain", "pack://application:,,,/perk_images/Scourge_Hook_Gift_of_Pain.png" },
                { "Scourge Hook: Hangman's Trick", "pack://application:,,,/perk_images/Scourge_Hook_Hangman's_Trick.png" },
                { "Scourge Hook: Jagged Compass", "pack://application:,,,/perk_images/Scourge_Hook_Jagged_Compass.png" },
                { "Scourge Hook: Monstrous Shrine", "pack://application:,,,/perk_images/Scourge_Hook_Monstrous_Shrine.png" },
                { "Scourge Hook: Pain Resonance", "pack://application:,,,/perk_images/Scourge_Hook_Pain_Resonance.png" },
                { "Septic Touch", "pack://application:,,,/perk_images/Septic_Touch.png" },
                { "Shadowborn", "pack://application:,,,/perk_images/Shadowborn.png" },
                { "Shattered Hope", "pack://application:,,,/perk_images/Shattered_Hope.png" },
                { "Sloppy Butcher", "pack://application:,,,/perk_images/Sloppy_Butcher.png" },
                { "Spies from the Shadows", "pack://application:,,,/perk_images/Spies_from_the_Shadows.png" },
                { "Spirit Fury", "pack://application:,,,/perk_images/Spirit_Fury.png" },
                { "Starstruck", "pack://application:,,,/perk_images/Starstruck.png" },
                { "Stridor", "pack://application:,,,/perk_images/Stridor.png" },
                { "Superior Anatomy", "pack://application:,,,/perk_images/Superior_Anatomy.png" },
                { "Surge", "pack://application:,,,/perk_images/Surge.png" },
                { "Surveillance", "pack://application:,,,/perk_images/Surveillance.png" },
                { "Terminus", "pack://application:,,,/perk_images/Terminus.png" },
                { "Territorial Imperative", "pack://application:,,,/perk_images/Territorial_Imperative.png" },
                { "Thanatophobia", "pack://application:,,,/perk_images/Thanatophobia.png" },
                { "Thrilling Tremors", "pack://application:,,,/perk_images/Thrilling_Tremors.png" },
                { "THWACK!", "pack://application:,,,/perk_images/THWACK!.png" },
                { "Tinkerer", "pack://application:,,,/perk_images/Tinkerer.png" },
                { "Trail of Torment", "pack://application:,,,/perk_images/Trail_of_Torment.png" },
                { "Ultimate Weapon", "pack://application:,,,/perk_images/Ultimate_Weapon.png" },
                { "Unbound", "pack://application:,,,/perk_images/Unbound.png" },
                { "Undone", "pack://application:,,,/perk_images/Undone.png" },
                { "Unforeseen", "pack://application:,,,/perk_images/Unforeseen.png" },
                { "Unnerving Presence", "pack://application:,,,/perk_images/Unnerving_Presence.png" },
                { "Unrelenting", "pack://application:,,,/perk_images/Unrelenting.png" },
                { "Weave Attunement", "pack://application:,,,/perk_images/Weave_Attunement.png" },
                { "Whispers", "pack://application:,,,/perk_images/Whispers.png" },
                { "Zanshin Tactics", "pack://application:,,,/perk_images/Zanshin_Tactics.png" }
            };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectKiller_Click(object sender, RoutedEventArgs e)
        {
            // Clear the PerksContainer to avoid duplicates
            PerksContainer.Children.Clear();

            // Use a DispatcherTimer to wait for the fade-out animation to complete
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.5) // Match the duration of the fade-out animation
            };

            timer.Tick += (s, args) =>
            {
                timer.Stop(); // Stop the timer after it fires

                // Create a Random instance
                Random rand = new Random();

                // Select a random Killer
                string selectedKiller = killers[rand.Next(killers.Length)];
                KillerName.Text = selectedKiller; // Update the text
                KillerName.Opacity = 1; // Ensure the label is visible

                // Shuffle the perk list and pick the first four unique perks
                List<string> shuffledPerks = perks.OrderBy(x => rand.Next()).ToList();
                string[] selectedPerks = shuffledPerks.Take(4).ToArray();

                // Add new perks to PerksContainer
                foreach (string perk in selectedPerks)
                {
                    // Create a StackPanel to hold the image and text
                    StackPanel perkPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(15, 0, 15, 0) // Adds spacing between perks
                    };

                    // Add the perk image
                    if (perkImages.TryGetValue(perk, out string perkImageUri))
                    {
                        try
                        {
                            Image perkImage = new Image
                            {
                                Source = new BitmapImage(new Uri(perkImageUri, UriKind.Absolute)),
                                Height = 75, // Set a fixed height for the image
                                Margin = new Thickness(0, 0, 0, 5) // Add spacing below the image
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
                }

                // Load the killer image dynamically
                string killerImageUri = $"pack://application:,,,/killer_images/{selectedKiller.Replace(" ", "_").ToLower()}.png";
                try
                {
                    KillerImage.Source = new BitmapImage(new Uri(killerImageUri, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading killer image: {ex.Message}");
                }

                // Trigger fade-in animations using the defined Storyboard
                Storyboard fadeInStoryboard = this.Resources["FadeInStoryboard"] as Storyboard;

                if (fadeInStoryboard != null)
                {
                    fadeInStoryboard.Begin(KillerName);       // Fade in the KillerName
                    fadeInStoryboard.Begin(KillerImage);     // Fade in the KillerImage
                    fadeInStoryboard.Begin(PerksContainer);  // Fade in the PerksContainer
                }
                else
                {
                    MessageBox.Show("FadeInStoryboard not found in resources.");
                }
            };

            timer.Start(); // Start the timer
        }



        private void ResetScreen_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the fade-out Storyboard from resources
            Storyboard fadeOutStoryboard = this.Resources["FadeOutStoryboard"] as Storyboard;

            if (fadeOutStoryboard != null)
            {
                // Attach a completion handler to the Storyboard
                fadeOutStoryboard.Completed += (s, ev) =>
                {
                    // Clear all elements after fade-out animation finishes
                    KillerName.Text = ""; // Clear the text
                    PerksContainer.Children.Clear();
                    KillerImage.Source = null;
                };

                // Start the fade-out animation for all elements
                fadeOutStoryboard.Begin(KillerName);
                fadeOutStoryboard.Begin(KillerImage);
                fadeOutStoryboard.Begin(PerksContainer);
            }
            else
            {
                MessageBox.Show("FadeOutStoryboard not found in resources.");
            }
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