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

namespace Kvanto.Pages.Kurs
{
    /// <summary>
    /// Логика взаимодействия для Kurs_Info.xaml
    /// </summary>
    public partial class Kurs_Info : Page
    {
        MainWindow mainWindow;
        Classes.Kvantum kvantum;
        Pages.Main.Main main;
        public Kurs_Info(MainWindow _mainWindow, Classes.Kvantum _kvantum, Pages.Main.Main _main)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            main = _main;
            Classes.Users.LoadUsers(mainWindow);
            kvantum = _kvantum;
            if (MainWindow.user.Kvantum != "")
            {
                Name.Content = "Название предмета: " + " " + kvantum.Name;
                Description.Content = "Описание предмета: " + " " + kvantum.Description;
            }
            else 
            { 
                Name.Visibility = Visibility.Hidden;
                Description.Visibility = Visibility.Hidden;
            } 


        }
        private void ChageUsers_Click(object sender, MouseButtonEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Kurs.Kurs_Add(mainWindow, MainWindow.user, null, main));
        }
    }
}
