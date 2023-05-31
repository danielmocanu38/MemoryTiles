using System.Windows;


namespace MVP_Tema_1_Mocanu_Daniel_10LF212
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new LoginPage());
        }
    }
}
