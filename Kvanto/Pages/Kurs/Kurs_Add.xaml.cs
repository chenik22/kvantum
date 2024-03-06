using Kvanto.Classes;
using MySql.Data.MySqlClient;
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
    /// Логика взаимодействия для Kurs_Add.xaml
    /// </summary>
    public partial class Kurs_Add : Page
    {
        MainWindow mainWindow;
        Classes.Users users;
        Classes.Kvantum kvant;
        Pages.Main.Main main;
        public Kurs_Add(MainWindow mainWindow, Classes.Users user, Classes.Kvantum kvantum, Main.Main _main)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            main = _main;
            kvant = kvantum;
            if (kvantum != null)
            {
                zagl.Content = "Изменение предмета";
                Entrance.Content = "Изменить";
            }
            this.kvant = kvantum;
            if (kvantum != null)
            {
                Name.Text = kvantum.Name;
                Description.Text = kvantum.Description;
            }
            users = user;
        }
        public string tName;
        public string tDescription;
        private void KursAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tName = Name.Text;
                tDescription = Description.Text;
                if (Name.Text != "" && Description.Text != "")
                {
                    MySqlConnection mySqlConnection = new MySqlConnection(Classes.WorkingBD.GetConnectionString());
                    mySqlConnection.Open();
                    if (kvant == null)
                    {
                        bool userExists = false; //Для отслеживания наличия пользователя с таким логином
                        foreach (Classes.Kvantum kvantum in mainWindow.KvantumList)
                        {
                            if (kvantum.Name == tName) //Если пользователь с таким логином уже существует
                            {
                                userExists = true;
                                break;
                            }
                        }
                        if (!userExists) // Если равен false (т.е. пользователь с таким логином не существует)
                        {
                            MySqlDataReader add = Classes.WorkingBD.Query($"INSERT INTO `kvanto`.`kvantum` (`Name`, `Description`) VALUES ('{tName}', '{tDescription}');", mySqlConnection);
                            MessageBox.Show("Успешное добавление предмета", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            main.OpenPage(mainWindow, new Pages.Kurs.Kurs_Main(mainWindow, null, main));
                            mySqlConnection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Пользователь с данным логином уже имеется");
                        }
                    }
                    else if (kvant != null)
                    {
                        MySqlDataReader add = Classes.WorkingBD.Query($"UPDATE `kvanto`.`kvantum` SET `Name` = '{tName}', `Description` = '{tDescription}' WHERE (`Id` = '{kvant.Id}');", mySqlConnection);
                        MessageBox.Show("Успешное изменение предмета", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        main.OpenPage(mainWindow, new Pages.Kurs.Kurs_Main(mainWindow, null, main));
                        mySqlConnection.Close();
                    }
                }
                else MessageBox.Show("Заполните все поля!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void InfoUser_Click(object sender, MouseButtonEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Users.Users_Info(mainWindow, MainWindow.user, null, main));
        }
    }
}
