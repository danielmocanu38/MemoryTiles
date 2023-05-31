using MemoryTiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MVP_Tema_1_Mocanu_Daniel_10LF212
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private List<User> usersList;
        private int currentIndex = -1;
        private List<string> profileImagesPaths;
        private List<string> tilesImagesPaths;
        public LoginPage()
        {
            InitializeComponent();

            usersList = new List<User>();
            profileImagesPaths = new List<string>();
            tilesImagesPaths = new List<string>();

            profileImagesPaths = GetProfileImagesPaths();
            ReadUsers();
            LoadUsers();
        }

        public List<string> GetProfileImagesPaths()
        {
            string directoryPath = "../../../resources/images/User_images";
            string imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath);
            string[] imageFiles = Directory.GetFiles(imageDirectory, "*.jpg");

            List<string> imageFilePaths = new List<string>();

            foreach (string imageFile in imageFiles)
            {
                string relativePath = Path.Combine(directoryPath, Path.GetFileName(imageFile));
                imageFilePaths.Add(relativePath);
            }

            return imageFilePaths;
        }

        private void ReadUsers()
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../users");

            // Get all XML files in the directory
            string[] userFiles = Directory.GetFiles(directoryPath, "*.xml");

            // Deserialize each file and add the user to the list
            foreach (string userFile in userFiles)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(User));
                using (FileStream stream = new FileStream(userFile, FileMode.Open))
                {
                    User user = (User)serializer.Deserialize(stream);
                    usersList.Add(user);
                }
            }
        }

        private void LoadUsers()
        {
            // Add users to userListBox
            userListBox.ItemsSource = null;
            userListBox.ItemsSource = usersList;

            //Select first user in list and display their profile picture
            userListBox.SelectedIndex = 0;
            UpdateProfileImage();
        }

        private void UpdateProfileImage()
        {
            if (usersList.Count > 0)
            {
                userListBox.SelectedIndex = 0;
                var selectedUser = usersList[0];
                string imagePath = selectedUser.ProfileImagePath;

                if (File.Exists(imagePath))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(imagePath, UriKind.Relative);
                    image.EndInit();
                    userImage.Source = image;
                }
                else
                {
                    userImage.Source = null;
                }
            }
            else { return; }

        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            var currentProfileImageIndex = 0;

            var newUserWindow = new Window();
            newUserWindow.Title = "New User";
            newUserWindow.Width = 600;
            newUserWindow.Height = 400;

            var stackPanel = new StackPanel();
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            newUserWindow.Content = stackPanel;

            var nameLabel = new Label();
            nameLabel.Content = "Enter name:";
            stackPanel.Children.Add(nameLabel);

            var nameTextBox = new TextBox();
            stackPanel.Children.Add(nameTextBox);

            var selectImageLabel = new Label();
            selectImageLabel.Content = "Select image:";
            stackPanel.Children.Add(selectImageLabel);

            var imageStackPanel = new StackPanel();
            imageStackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(imageStackPanel);

            var imagePreviewImage = new Image();
            imagePreviewImage.Width = 250;
            imagePreviewImage.Height = 250;

            var leftButton = new Button();
            leftButton.Content = "<<";
            leftButton.Click += (s, ea) =>
            {
                if (profileImagesPaths.Count > 0 && currentProfileImageIndex > 0)
                {
                    currentProfileImageIndex = currentProfileImageIndex - 1;
                    imagePreviewImage.Source = new BitmapImage(new Uri(profileImagesPaths[currentProfileImageIndex], UriKind.Relative));
                }
            };
            imageStackPanel.Children.Add(leftButton);

            if (profileImagesPaths.Count > 0)
            {
                imagePreviewImage.Source = new BitmapImage(new Uri(profileImagesPaths[currentProfileImageIndex], UriKind.Relative));

            }
            imageStackPanel.Children.Add(imagePreviewImage);

            var rightButton = new Button();
            rightButton.Content = ">>";
            rightButton.Click += (s, ea) =>
            {
                if (profileImagesPaths.Count > 0 && currentProfileImageIndex < profileImagesPaths.Count - 1)
                {
                    currentProfileImageIndex = currentProfileImageIndex + 1;
                    imagePreviewImage.Source = new BitmapImage(new Uri(profileImagesPaths[currentProfileImageIndex], UriKind.Relative));
                }
            };
            imageStackPanel.Children.Add(rightButton);

            var selectImageButton = new Button();
            selectImageButton.Content = "Create User";
            selectImageButton.Click += (s, ea) =>
            {
                string userName = nameTextBox.Text;
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(profileImagesPaths[currentProfileImageIndex]))
                {
                    var user = new User(userName, profileImagesPaths[currentProfileImageIndex]);

                    //create a user file
                    string fileName = $"{user.Name}.xml";
                    string filePath = Path.Combine("../../../users", fileName);

                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(User));
                        serializer.Serialize(streamWriter, user);
                    }

                    //save user in the displayed user list
                    usersList.Add(user);
                    userListBox.ItemsSource = null;
                    userListBox.ItemsSource = usersList;
                    MessageBox.Show($"User '{userName}' added successfully.");
                    newUserWindow.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a name and select an image.");
                }
            };
            stackPanel.Children.Add(selectImageButton);

            newUserWindow.ShowDialog();
            UpdateProfileImage();
        }


        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var deletedUser = usersList[userListBox.SelectedIndex];

            string userName = deletedUser.Name;
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the user '{userName}'?",
                                                      "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            string fileName = $"{userName}.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../users", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            usersList.Remove(deletedUser);
            userListBox.ItemsSource = null;
            userListBox.ItemsSource = usersList;

            userImage.Source = null;
            userListBox.SelectedIndex = 0;

            MessageBox.Show($"User '{userName}' deleted successfully.");
            UpdateProfileImage();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)userListBox.SelectedItem;

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            Frame frame = mainWindow.MainFrame;

            if (selectedUser != null)
            {
                frame.Navigate(new MenuPage(selectedUser));
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            userListBox.SelectedItem = null;
            userImage.Source = null;
        }

        private void ScrollPrevButton_Click(object sender, EventArgs e)
        {
            int index = userListBox.SelectedIndex;
            if (index > 0)
            {
                userListBox.SelectedIndex = index - 1;
                BitmapImage bitmap = new BitmapImage(new Uri(usersList[index - 1].ProfileImagePath, UriKind.Relative));
                userImage.Source = bitmap;
            }
        }

        private void ScrollNextButton_Click(object sender, EventArgs e)
        {
            int index = userListBox.SelectedIndex;
            if (index < userListBox.Items.Count - 1)
            {
                userListBox.SelectedIndex = index + 1;
                BitmapImage bitmap = new BitmapImage(new Uri(usersList[index + 1].ProfileImagePath, UriKind.Relative));
                userImage.Source = bitmap;
            }
        }
    }
}
