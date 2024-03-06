using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kvanto.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        MainWindow mainWindow;
        public Main(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            if (MainWindow.user.Role == "teacher"|| MainWindow.user.Role == "Admin")
            {
                Entrance.Visibility = Visibility.Collapsed;
            }
            if (MainWindow.user.Role == "users")
            {
                Entra.Visibility = Visibility.Collapsed;
                Entr.Visibility = Visibility.Collapsed;
            }

        }

        public enum pages
        {
            authorization,
            main,
            registration
        }
        public void OpenPage(MainWindow mainWindow, Page ToPage)
        {
            
            frame1.Navigate(ToPage);
        }

        Style MenuButtonClick = (Style)Application.Current.Resources["menuButtonClick"];
        Style MenuButton = (Style)Application.Current.Resources["menuButton"];

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(mainWindow, new Pages.Test.Test(mainWindow));
            Entrance.Style = MenuButtonClick;
            Entra.Style = MenuButton;
            Entr.Style = MenuButton;
            UserEnter.Style = MenuButton;
        }

        private void Kurs_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(mainWindow, new Pages.Kurs.Kurs_Main(mainWindow, null, this));
            Entra.Style = MenuButtonClick;
            Entrance.Style = MenuButton;
            Entr.Style = MenuButton;
            UserEnter.Style = MenuButton;
        }
        private void Users_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(mainWindow, new Pages.Users.Users_main(mainWindow,this, this));
            Entr.Style = MenuButtonClick;
            UserEnter.Style = MenuButton;
            Entra.Style = MenuButton;
            Entrance.Style = MenuButton;
        }

        public void UserEnter_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(mainWindow, new Pages.Users.Users_Info(mainWindow, MainWindow.user, this, this));
            UserEnter.Style = MenuButtonClick;
            Entra.Style = MenuButton;
            Entrance.Style = MenuButton;
            Entr.Style = MenuButton;
        }

        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            mainWindow.OpenPage(mainWindow, new Pages.Main.Autorization(mainWindow));
        }

        
    }
}
