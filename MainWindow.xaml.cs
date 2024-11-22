using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace c_project_wordle_1_IliasMPXL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string targetWord = "OPPLE";  // Example target word for comparison
        private string currentInput = "";     // To keep track of the current input string

        public MainWindow()
        {

            InitializeComponent();

        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox1.Focus();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox2.Focus();
        }
        private void TextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox4.Focus();
        }

        private void TextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox5.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)sender;
            // Get the letter from the button
            string letter = clickedButton.Content.ToString();

            // Find the first TextBox with empty content and insert the letter
            foreach (var child in ((StackPanel)TextBox1.Parent).Children)
            {
                if (child is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = letter;
                    currentInput += letter;  // Add the letter to the current input string
                    break;
                }
            }
        }

        //enter button

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (currentInput.Length == 5)
            {
                // Create a list to track whether each letter is correct (for button color change)
                List<bool> isLetterCorrect = new List<bool>();

                // Compare letters with the target word
                for (int i = 0; i < 5; i++)
                {
                    string letter = currentInput[i].ToString();
                    TextBox currentTextBox = FindName($"TextBox{i + 1}") as TextBox;

                    if (currentTextBox != null)
                    {
                        currentTextBox.Focus();  // Focus the TextBox

                        // Check correctness of the letter
                        if (letter == targetWord[i].ToString())
                        {
                            currentTextBox.Background = Brushes.Green; // Correct letter and position
                            isLetterCorrect.Add(true); // Mark as correct letter and position
                        }
                        else if (targetWord.Contains(letter))
                        {
                            currentTextBox.Background = Brushes.Orange; // Correct letter but wrong position
                            isLetterCorrect.Add(false); // Mark as correct letter, wrong position
                        }
                        else
                        {
                            currentTextBox.Background = Brushes.Gray; // Incorrect letter
                            isLetterCorrect.Add(false); // Mark as incorrect letter
                        }

                        // Change the button color based on the correctness of the letter
                        var button = this.FindName($"Button{i + 1}") as Button;
                        if (button != null)
                        {
                            // If the letter is correct, change button to Green
                            if (letter == targetWord[i].ToString())
                            {
                                button.Background = Brushes.Green;
                            }
                            // If the letter is in the word but not in the correct position, change button to Orange
                            else if (targetWord.Contains(letter))
                            {
                                button.Background = Brushes.Orange;
                            }
                            // If the letter is not in the word at all, change button to Gray
                            else
                            {
                                button.Background = Brushes.Gray;
                            }
                        }
                    }
                }

                // Now, let's check if any button corresponds to a letter that was part of the input string,
                // and change the button color accordingly, ensuring buttons change color if their letter exists in the target word.
                for (int i = 0; i < 5; i++)
                {
                    string letter = currentInput[i].ToString();
                    var button = this.FindName($"Button{i + 1}") as Button;

                    if (button != null && targetWord.Contains(letter))
                    {
                        // If the letter is correct, button should be Green
                        if (isLetterCorrect[i])
                        {
                            button.Background = Brushes.Green;
                        }
                        // If the letter is in the word but not in the correct position, button should be Orange
                        else
                        {
                            button.Background = Brushes.Orange;
                        }
                    }
                }

                // Check if the current input matches the target word
                if (currentInput == targetWord)
                {
                    MessageBox.Show("Proficiat!"); // Display congratulations message
                }
            }
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);  // Remove last letter

                // Find the last non-empty TextBox and clear it in reverse order
                var textBoxes = ((StackPanel)TextBox1.Parent).Children.OfType<TextBox>().Reverse().ToList();

                foreach (var textBox in textBoxes)
                {
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        textBox.Clear();
                        break;
                    }
                }


            }
        }

        //delete button
        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {

            {
                // reset window door te sluiten
                var newWindow = new MainWindow();
                newWindow.Show();
                // open een nieuwe
                this.Close();
            }
        }

        // Declare inputText as a class-level variable
        private string inputText = "";

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            // Ensure sender is a TextBox
            if (sender is TextBox currentTextBox)
            {
                // Update inputText by collecting text from all the TextBoxes
                inputText = string.Join("", TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text);

                // Ensure only 1 character is entered in the current TextBox
                if (currentTextBox.Text.Length >= 1)
                {
                    currentTextBox.Text = currentTextBox.Text.Substring(0, 1); // Keep only one character
                    currentTextBox.CaretIndex = currentTextBox.Text.Length; // Move cursor to the end
                }

                // If the inputText length equals 5 (all letters entered), process the input
                if (inputText.Length == 5)
                {
                    // Compare letters with the target word
                    for (int i = 0; i < 5; i++)
                    {
                        string letter = inputText[i].ToString();
                        TextBox textBox2 = FindName($"TextBox{i + 1}") as TextBox;

                        if (textBox2 != null)
                        {
                            // Update the TextBox background based on correctness
                            if (letter == targetWord[i].ToString())
                            {
                                textBox2.Background = Brushes.Green; // Correct letter and position
                                SetButtonColor(i, Brushes.Green); // Update corresponding button color
                            }
                            else if (targetWord.Contains(letter))
                            {
                                textBox2.Background = Brushes.Orange; // Correct letter, wrong position
                                SetButtonColor(i, Brushes.Orange); // Update corresponding button color
                            }
                            else
                            {
                                textBox2.Background = Brushes.Gray; // Incorrect letter
                                SetButtonColor(i, Brushes.Gray); // Update corresponding button color
                            }
                            // Check if the letter exists anywhere in the target word
                            var button = this.FindName($"Button{i + 1}") as Button;
                            if (button != null)
                            {
                                // If the letter is correct (green), change the button to green
                                if (letter == targetWord[i].ToString())
                                {
                                    button.Background = Brushes.Green;
                                }
                                // If the letter is in the word but not in the correct position, change the button to orange
                                else if (targetWord.Contains(letter))
                                {
                                    button.Background = Brushes.Orange;
                                }
                                // If the letter is not in the word, change the button to gray
                                else
                                {
                                    button.Background = Brushes.Gray;
                                }
                            }
                        }

                        // Check if the current input matches the target word
                        if (inputText == targetWord)
                        {
                            MessageBox.Show("Proficiat!"); // Display congratulations message
                                                           // Optional: Disable further input or reset the game
                        }

                        // Reset the input for the next round
                        inputText = "";
                        FocusNextEmptyTextBox(); // Move focus to the next empty TextBox
                    }

                    // Navigate to the next TextBox after one character is entered
                    if (currentTextBox.Text.Length == 1)
                    {
                        var parent = currentTextBox.Parent as StackPanel; // Assuming all TextBoxes are in a StackPanel
                        if (parent != null)
                        {
                            var textBoxes = parent.Children.OfType<TextBox>().ToList();
                            int currentIndex = textBoxes.IndexOf(currentTextBox);

                            // Move focus to the next TextBox if available
                            if (currentIndex < textBoxes.Count - 1)
                            {
                                textBoxes[currentIndex + 1].Focus();
                            }
                        }
                    }
                }
                else if (sender is Button currentButton)
                {
                    // Handle Button input (if any specific behavior is needed for buttons)
                    // For example, you can change the background color or trigger actions
                    Console.WriteLine($"Button {currentButton.Name} clicked");
                }
                else
                {
                    // Handle other types of user input if necessary
                    Console.WriteLine($"Unhandled input type: {sender.GetType().Name}");
                }
            }
        }

        // This method updates the background color of the corresponding button
        private void SetButtonColor(int index, Brush color)
        {
            var button = FindName($"Button{index + 1}") as Button;
            if (button != null)
            {
                button.Background = color;
            }
        }
        // This method automatically focuses on the next empty TextBox
        private void FocusNextEmptyTextBox()
        {
            // Find the next empty TextBox to focus on
            foreach (var child in ((StackPanel)TextBox1.Parent).Children)
            {
                if (child is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Focus();
                    break;
                }
            }
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}









