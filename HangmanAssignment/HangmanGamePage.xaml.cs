namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private string wordToGuess;
        private HashSet<char> guessedLetters;
        private int remainingAttempts;

        public HangmanGamePage()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            // The word to guess
            wordToGuess = "DEVELOPER";
            guessedLetters = new HashSet<char>();
            remainingAttempts = 6;
            ResetDisplay();
            UpdateDisplay();
        }

        private void ResetDisplay()
        {
            // Reset the guessed word label
            foreach (var child in ((VerticalStackLayout)this.Content).Children)
            {
                if (child is Label label && label.Text.Contains("_"))
                {
                    label.Text = string.Join(" ", new string('_', wordToGuess.Length));
                    break;
                }
            }

            // Reset the hangman image to the initial stage
            foreach (var child in ((VerticalStackLayout)this.Content).Children)
            {
                if (child is Image image)
                {
                    image.Source = "hang1.png";
                    break;
                }
            }
        }

        private void UpdateDisplay()
        {
            // Update the guessed word display
            string currentState = GetCurrentState();

            foreach (var child in ((VerticalStackLayout)this.Content).Children)
            {
                if (child is Label label && label.Text.Contains("_"))
                {
                    label.Text = currentState;
                    break;
                }
            }

            // It updates the image to show the progress of hangman stages
            foreach (var child in ((VerticalStackLayout)this.Content).Children)
            {
                if (child is Image image)
                {
                    image.Source = $"hang{7 - remainingAttempts}.png";
                    break;
                }
            }
        }

        private string GetCurrentState()
        {
            string currentState = "";
            foreach (char c in wordToGuess)
            {
                currentState += guessedLetters.Contains(c) ? c : '_';
                // Add a space between letters
                currentState += " ";
            }
            return currentState.Trim();
        }

        private void OnGuessButtonClicked(object sender, EventArgs e)
        {
            // Find the Entry control where the player enters their guess
            Entry guessEntry = null;
            foreach (var child in ((VerticalStackLayout)this.Content).Children)
            {
                if (child is Entry entry)
                {
                    guessEntry = entry;
                    break;
                }
            }

            // Validate the player's input
            if (guessEntry != null && !string.IsNullOrWhiteSpace(guessEntry.Text) &&
                char.TryParse(guessEntry.Text.ToUpper(), out char guessedLetter))
            {
                GuessLetter(guessedLetter);
                guessEntry.Text = "";
            }
            else
            {
                DisplayAlert("Invalid Input", "Please enter a single letter.", "OK");
            }
        }

        private void GuessLetter(char letter)
        {
            if (guessedLetters.Contains(letter))
            {
                // Inform the user that the letter was already guessed
                DisplayAlert("Info", "You've already guessed that letter!", "OK");
                return;
            }

            guessedLetters.Add(letter);

            if (!wordToGuess.Contains(letter))
            {
                remainingAttempts--;
            }

            UpdateDisplay();

            if (IsGameOver())
            {
                ShowGameOverMessage();
            }
        }

        private bool IsGameOver()
        {
            return remainingAttempts <= 0 || IsWordGuessed();
        }

        private bool IsWordGuessed()
        {
            foreach (char c in wordToGuess)
            {
                if (!guessedLetters.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        private async void ShowGameOverMessage()
        {
            // Determine if the player won
            bool playerWon = IsWordGuessed();
            string title = playerWon ? "You Survived!" : "You Died!";
            string message = playerWon
                ? $"You guessed the word: {wordToGuess} correctly!"
                : $"The word was: {wordToGuess}";

            // Display the appropriate message and restart the game
            await DisplayAlert(title, message, "OK");
            StartNewGame();
        }
    }
}
