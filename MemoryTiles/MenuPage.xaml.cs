using MemoryTiles;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace MVP_Tema_1_Mocanu_Daniel_10LF212
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        User user = new User();
        public MenuPage(User receivedUser)
        {
            InitializeComponent();
            user = receivedUser;
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            // Create the pop-up window
            var dialog = new Window()
            {
                Title = "New Game",
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            // Create the main stack panel to hold the controls
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10)
            };

            // Create the label for the rows text box
            var rowsLabel = new Label()
            {
                Content = "Rows:",
                Margin = new Thickness(0, 10, 0, 5)
            };

            // Create the rows text box
            var rowsTextBox = new TextBox()
            {
                Text = "5",
                Margin = new Thickness(0, 0, 0, 5)
            };

            // Create the label for the columns text box
            var columnsLabel = new Label()
            {
                Content = "Columns:",
                Margin = new Thickness(0, 10, 0, 5)
            };

            // Create the columns text box
            var columnsTextBox = new TextBox()
            {
                Text = "5",
                Margin = new Thickness(0, 0, 0, 5)
            };

            // Create the OK button
            var okButton = new Button()
            {
                Content = "OK",
                Width = 100,
                Margin = new Thickness(0, 10, 0, 0)
            };

            // Set the OK button's click event handler
            okButton.Click += (s, ev) =>
            {
                // Parse the rows and columns values
                if (int.TryParse(rowsTextBox.Text, out int rows) && int.TryParse(columnsTextBox.Text, out int columns))
                {
                    // Check that the values are between 2 and 7
                    if (rows >= 2 && rows <= 7 && columns >= 2 && columns <= 7)
                    {
                        // Close the dialog
                        dialog.Close();

                        // Create a new instance of the BoardPage page with the entered dimensions
                        var boardPage = new BoardPage(rows, columns, user);

                        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                        Frame frame = mainWindow.MainFrame;

                        // Navigate to the BoardPage page
                        frame.Navigate(boardPage);
                    }
                    else
                    {
                        // Show an error message
                        MessageBox.Show("The rows and columns values must be between 2 and 7.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Show an error message
                    MessageBox.Show("The rows and columns values must be integers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };


            stackPanel.Children.Add(rowsLabel);
            stackPanel.Children.Add(rowsTextBox);
            stackPanel.Children.Add(columnsLabel);
            stackPanel.Children.Add(columnsTextBox);
            stackPanel.Children.Add(okButton);

            dialog.Content = stackPanel;

            dialog.ShowDialog();
        }

        private void OpenGame_Click(object sender, RoutedEventArgs e)
        {



        }


        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            string fileName = $"{user.Name}.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../users", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(User));
                serializer.Serialize(streamWriter, user);
            }

        }



        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            int playedGames = user.PlayedGames;
            int wonGames = user.WonGames;

            var statsWindow = new Window();

            statsWindow.Title = "Statistics";
            statsWindow.Width = 500;
            statsWindow.Height = 300;

            var statsPanel = new StackPanel();

            var playedGamesText = new TextBlock();
            playedGamesText.Text = "Played games: " + playedGames.ToString();
            statsPanel.Children.Add(playedGamesText);

            var wonGamesText = new TextBlock();
            wonGamesText.Text = "Won games: " + wonGames.ToString();
            statsPanel.Children.Add(wonGamesText);

            statsWindow.Content = statsPanel;

            statsWindow.ShowDialog();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
