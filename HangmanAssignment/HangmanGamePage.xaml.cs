
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
            wordToGuess = "DEVELOPER"; // Set the word to guess here
            guessedLetters = new HashSet<char>();
            remainingAttempts = 6; // Number of allowed incorrect guesses
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            // Update the display of the guessed word
            string currentState = GetCurrentState();
            var label = (Label)this.Content.FindByName("---------");
            label.Text = currentState;

            // Update remaining attempts or other UI elements as necessary
            // You can add a label for remaining attempts here if needed
        }

        private string GetCurrentState()
        {
            string currentState = "";
            foreach (char c in wordToGuess)
            {
                currentState += guessedLetters.Contains(c) ? c : '_';
                currentState += " "; // Space between letters
            }
            return currentState.Trim();
        }

        private void OnGuessButtonClicked(object sender, EventArgs e)
        {
            var entry = (Entry)this.Content.FindByName("Enter your next guess");
            if (char.TryParse(entry.Text.ToUpper(), out char guessedLetter))
            {
                GuessLetter(guessedLetter);
                entry.Text = ""; // Clear the entry after guessing
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

            if (IsGameOver())
            {
                ShowGameOverMessage();
            }

            UpdateDisplay();
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
            string message = IsWordGuessed() ?
                $"You  Survived! You've guessed the word: {wordToGuess}" :
                $"You Died! The word was: {wordToGuess}";

            await DisplayAlert("You Died", message, "OK");
            StartNewGame(); // Restart the game
        }
    }
}
    
































