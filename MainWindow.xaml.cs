using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        private MediaPlayer loseSound;

        public MainWindow()
        {
            InitializeComponent();
            LoadSounds();
            InitializeGame();
        }

        // Charge les fichiers audio
        private void LoadSounds()
        {
            backgroundMusic = new MediaPlayer();
            backgroundMusic.Open(new Uri("Resources/Sounds/back.mp3", UriKind.Relative));

            winSound = new MediaPlayer();
            winSound.Open(new Uri("Resources/Sounds/win.mp3", UriKind.Relative)); // Vérifie que ce chemin est correct

            loseSound = new MediaPlayer();
            loseSound.Open(new Uri("Resources/Sounds/lose.mp3", UriKind.Relative)); // Vérifie que ce chemin est correct
        }

        // Initialise une nouvelle partie
        private void InitializeGame()
        {
            // Initialisation du jeu
            Random rand = new Random();
            selectedWord = GuessWords[rand.Next(GuessWords.Count)].ToUpper();
            guessedWord = new string('_', selectedWord.Length).ToCharArray();
            attempts = maxAttempts; // Réinitialiser le nombre d'essais

            // Mettre à jour l'affichage du mot à deviner
            UpdateDisplayedWord();

            // Liste des boutons de lettres à réactiver pour chaque nouvelle partie
            letterButtons = new List<Button>
            {
                BTN_A, BTN_B, BTN_C, BTN_D, BTN_E, BTN_F, BTN_G, BTN_H,
                BTN_I, BTN_J, BTN_K, BTN_L, BTN_M, BTN_N, BTN_O, BTN_P,
                BTN_Q, BTN_R, BTN_S, BTN_T, BTN_U, BTN_V, BTN_W, BTN_X,
                BTN_Y, BTN_Z
            };

            // Réactiver tous les boutons et supprimer l'ancienne liaison d'événement
            foreach (Button btn in letterButtons)
            {
                btn.IsEnabled = true;
                btn.Click -= LetterButton_Click; // Supprimer tout ancien événement pour éviter des doublons
                btn.Click += LetterButton_Click; // Ajouter l'événement du clic
            }

            // Commencer la musique de fond
            backgroundMusic.Stop(); // Arrête la musique si elle était en cours
            backgroundMusic.Play();
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
                    backgroundMusic.Stop(); // Arrête la musique de fond
                    winSound.Play(); // Joue le son de victoire
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
                    backgroundMusic.Stop(); // Arrête la musique de fond
                    loseSound.Play(); // Joue le son de défaite
                    MessageBox.Show($"Vous avez perdu ! Le mot était : {selectedWord}");
                    InitializeGame(); // Redémarrer une nouvelle partie
                }
            }
        }
    }
}
