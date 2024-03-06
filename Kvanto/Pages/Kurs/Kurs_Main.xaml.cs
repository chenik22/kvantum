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
    /// Логика взаимодействия для Kurs_Main.xaml
    /// </summary>
    public partial class Kurs_Main : Page
    {
        MainWindow mainWindow;
        public Page PrPage;
        Pages.Main.Main main;
        public Kurs_Main(MainWindow mainWindow, Page PrPage, Main.Main main)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.PrPage = PrPage;
            if (MainWindow.user.Role == "teacher")
            {
                Entrance.Visibility = Visibility.Hidden;
            }
            LoadKurs();
            this.main = main;
            this.Loaded += Kurs_Main_Loaded;
        }
        private void Kurs_Main_Loaded(object sender, RoutedEventArgs e)
        {
            LoadKurs();
        }
        public void LoadKurs()
        {
            parrent.Children.Clear();
            foreach (Classes.Kvantum KVantum in mainWindow.KvantumList)
            {
                parrent.Children.Add(new Elements.items_Kurs(mainWindow, KVantum, main));
            }
        }
        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));
        }

        private void AddUsers_Click(object sender, MouseButtonEventArgs e)
        {
            mainWindow.OpenPage(mainWindow, new Pages.Kurs.Kurs_Add(mainWindow, null, null, main));
        }

        private void InfoUser_Click(object sender, MouseButtonEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Kurs.Kurs_Info(mainWindow, null, main));
        }
        private void Kurs_Click(object sender, RoutedEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Kurs.Kurs_Add(mainWindow,null,null, main));
        }
    }
}
