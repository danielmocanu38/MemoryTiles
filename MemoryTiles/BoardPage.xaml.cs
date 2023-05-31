using MemoryTiles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVP_Tema_1_Mocanu_Daniel_10LF212
{
    /// <summary>
    /// Interaction logic for BoardPage.xaml
    /// </summary>
    public partial class BoardPage : Page
    {
        int rows;
        int columns;
        User user;
        Board board;
        Game game;
        public BoardPage(int rows, int columns, User user)
        {
            InitializeComponent();

            this.rows = rows;
            this.columns = columns;
            this.user = user;

            user.StartNewGame(rows, columns);
            game = user.Game;
            board = game.getBoard();

            CreateGrid(rows, columns);
        }

        private void CreateGrid(int rows, int columns)
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < columns; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            grid.Children.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Tile tile = board.GetTile(i, j);
                    Button button = new Button
                    {
                        Content = tile.Value,
                        Margin = new Thickness(5),
                        Tag = tile,
                        IsEnabled = !tile.IsMatched
                    };
                    button.Click += (sender, args) => { TileClicked(sender, args); };

                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);

                    grid.Children.Add(button);
                }
            }

            Button backButton = new Button
            {
                Content = "Back",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            backButton.Click += (sender, args) =>
            {
                var menuPage = new MenuPage(user);

                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                Frame frame = mainWindow.MainFrame;
                // Navigate to the BoardPage page
                frame.Navigate(menuPage);
            };

            Grid.SetRow(backButton, 0);
            Grid.SetColumn(backButton, 0);

            grid.Children.Add(backButton);
        }



        private void TileClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int row = (int)button.GetValue(Grid.RowProperty);
            int col = (int)button.GetValue(Grid.ColumnProperty);

            // Flip the tile
            board.FlipTile(row, col);

            // Check if any other flipped tiles match this tile
            List<Tile> flippedTiles = new List<Tile>();
            foreach (UIElement element in grid.Children)
            {
                if (element is Button tileButton)
                {
                    int tileRow = (int)tileButton.GetValue(Grid.RowProperty);
                    int tileCol = (int)tileButton.GetValue(Grid.ColumnProperty);
                    Tile tile = board.GetTile(tileRow, tileCol);

                    if (tile.IsFlipped && !tile.IsMatched)
                    {
                        flippedTiles.Add(tile);
                    }
                }
            }

            if (flippedTiles.Count == 2)
            {
                Tile firstTile = flippedTiles[0];
                Tile secondTile = flippedTiles[1];

                if (firstTile.Value == secondTile.Value)
                {
                    firstTile.IsMatched = true;
                    secondTile.IsMatched = true;
                }
                else
                {
                    // If the tiles don't match, unflip them after a short delay
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            board.UnflipTile(row, col);
                            board.UnflipTile((int)flippedTiles[0].Row, (int)flippedTiles[0].Column);
                        });
                    });
                }
            }

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = (Button)grid.Children
                        .Cast<UIElement>()
                        .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);

                    Tile tile = board.GetTile(i, j);

                    if (tile.IsFlipped)
                    {
                        button.Content = tile.Value.ToString();
                    }
                    else
                    {
                        button.Content = "";
                    }

                    button.IsEnabled = !tile.IsMatched;

                    // Check if all tiles have been matched
                    if (!tile.IsMatched)
                    {
                        return;
                    }
                }
            }

            // If we get here, all tiles have been matched
            game.incrementLevel();

            if (game.Level == 3)
            {
                user.IncrementPlayedGames();
                user.IncrementWonGames();
                // Game has been won
                MessageBox.Show("Congratulations! You won the game!");

                var menuPage = new MenuPage(user);

                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                Frame frame = mainWindow.MainFrame;
                // Navigate to the BoardPage page
                frame.Navigate(menuPage);
            }
        }




    }
}
