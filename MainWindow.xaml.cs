using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NatanChampavertPendu
{
    public partial class MainWindow : Window
    {
        // Liste des mots à deviner
        private List<string> GuessWords = new List<string> { "BRAS", "BING", "FLOP", "TETE" };
        private string selectedWord; // Mot sélectionné pour cette partie
        private char[] guessedWord;  // Le mot avec les lettres correctement devinées
        private int attempts;        // Nombre d'erreurs restantes
        private int maxAttempts = 6; // Nombre maximum d'erreurs
        private List<Button> letterButtons; // Liste des boutons de lettres
        private MediaPlayer backgroundMusic;
        private MediaPlayer winSound;
        private MediaPlayer

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            LoadSounds();
        }

        // Initialise une nouvelle partie
        private void InitializeGame()
        {
            // Initialisation du jeu
            Random rand = new Random();
            selectedWord = GuessWords[rand.Next(GuessWords.Count)].ToUpper();
            guessedWord = new string('_', selectedWord.Length).ToCharArray();
            attempts = maxAttempts;  // Réinitialiser le nombre d'essais

            // Mettre à jour l'affichage du mot à deviner
            UpdateDisplayedWord();

            // Liste des boutons de lettres à réactiver pour chaque nouvelle partie
            letterButtons = new List<Button> { BTN_A, BTN_B, BTN_C, BTN_D, BTN_E, BTN_F, BTN_G, BTN_H, BTN_I, BTN_J, BTN_K, BTN_L, BTN_M, BTN_N, BTN_O, BTN_P, BTN_Q, BTN_R, BTN_S, BTN_T, BTN_U, BTN_V, BTN_W, BTN_X, BTN_Y, BTN_Z };

            // Réactiver tous les boutons et supprimer l'ancienne liaison d'événement (pour éviter d'empiler des événements)
            foreach (Button btn in letterButtons)
            {
                btn.IsEnabled = true;
                btn.Click -= LetterButton_Click; // Supprimer tout ancien événement pour éviter des doublons
                btn.Click += LetterButton_Click; // Ajouter l'événement du clic
            }
        }

        // Mise à jour de l'affichage du mot à deviner
        private void UpdateDisplayedWord()
        {
            // Mettre à jour le contenu du TextBox avec le mot en cours
            Grd_Display.Children.Clear();
            TextBox txtBox = new TextBox
            {
                Text = string.Join(" ", guessedWord), // Espacer les lettres
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 613,
                Height = 88,
                Background = System.Windows.Media.Brushes.GhostWhite,
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true
            };
            Grd_Display.Children.Add(txtBox);
        }

        // Gérer le clic sur une lettre
        private void LetterButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string letter = clickedButton.Content.ToString();

            // Désactiver le bouton après le clic
            clickedButton.IsEnabled = false;

            // Vérifier si la lettre est dans le mot
            if (selectedWord.Contains(letter))
            {
                // La lettre est correcte, on la révèle dans guessedWord
                for (int i = 0; i < selectedWord.Length; i++)
                {
                    if (selectedWord[i] == letter[0])
                    {
                        guessedWord[i] = letter[0];
                    }
                }
                UpdateDisplayedWord();

                // Vérifier si le joueur a gagné
                if (!new string(guessedWord).Contains('_'))
                {
                    MessageBox.Show("Félicitations, vous avez gagné !");
                    InitializeGame(); // Redémarrer une nouvelle partie
                }
            }
            else
            {
                // La lettre est incorrecte, on réduit le nombre de tentatives
                attempts--;
                if (attempts == 0)
                {
                    MessageBox.Show($"Vous avez perdu ! Le mot était : {selectedWord}");
                    InitializeGame(); // Redémarrer une nouvelle partie
                }
            }
        }
    }
}

// Load sound files
private void LoadSounds()
{
    backgroundMusic = new MediaPlayer();
    backgroundMusic.Open(new Uri("path_to_your_background_music.mp3", UriKind.Relative));
    backgroundMusic.Volume = 0.5; // Adjust the volume

    winSound = new MediaPlayer();
    winSound.Open(new Uri("path_to_your_win_sound.mp3", UriKind.Relative));

    loseSound = new MediaPlayer();
    loseSound.Open(new Uri("sons/Lose sound effects.mp3", UriKind.Relative));
}

// Start the background music when the game starts
private void InitializeGame()
{
    // Your existing code
    backgroundMusic.Play();
}

// Play win sound
if (!new string(guessedWord).Contains('_'))
{
    winSound.Play();
    MessageBox.Show("Félicitations, vous avez gagné !");
    InitializeGame(); // Redémarrer une nouvelle partie
}

// Play lose sound
else
{
    attempts--;
    if (attempts == 0)
    {
        loseSound.Play();
        MessageBox.Show($"Vous avez perdu ! Le mot était : {selectedWord}");
        InitializeGame(); // Redémarrer une nouvelle partie
    }

