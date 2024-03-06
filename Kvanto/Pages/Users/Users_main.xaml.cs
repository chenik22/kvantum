using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kvanto.Pages.Users
{
    /// <summary>
    /// Логика взаимодействия для Users_main.xaml
    /// </summary>
    public partial class Users_main : Page
    {
        MainWindow mainWindow;
        public Page PrPage;
        Pages.Main.Main main;
        public Users_main(MainWindow mainWindow, Page PrPage, Pages.Main.Main _main)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.PrPage = PrPage;
            main = _main;
            LoadUsers();
        }
        public void LoadUsers()
        {
            if (MainWindow.user.Role == "teacher")
            {
                parrent.Children.Clear();
                foreach (Classes.Users USers in mainWindow.UsersList)
                {
                    if (USers.Teacher ==MainWindow.user.FIO)
                    {
                        parrent.Children.Add(new Elements.ItemsUser(mainWindow, USers, main));
                    }
                }
            }
            else
            {
                parrent.Children.Clear();
                foreach (Classes.Users USers in mainWindow.UsersList)
                {
                    parrent.Children.Add(new  Elements.ItemsUser(mainWindow, USers, main));
                }
            }

        }
        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Users.Users_main(mainWindow, PrPage, main));
        }

        private void AddUsers_Click(object sender, MouseButtonEventArgs e)
        {
            mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));
        }

        private void InfoUser_Click(object sender, MouseButtonEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Users.Users_Info(mainWindow, MainWindow.user,this,main));
        }

    }
}
