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

namespace Kvanto.Pages.Users.Elements
{
    /// <summary>
    /// Логика взаимодействия для ItemsUser.xaml
    /// </summary>
    public partial class ItemsUser : UserControl
    {
        MainWindow mainWindow;
        Classes.Users users;
        Pages.Main.Main main;
        public ItemsUser(MainWindow mainWindow,Classes.Users user, Main.Main _main)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Name.Text = user.FIO;
            Kvan.Text = user.Kvantum;
            if(Kvan.Text.Length == 0)
            {
                Kvan.Visibility = Visibility.Collapsed;
            }
            Role.Text = user.Role;
            users = user;
            this.main = _main;
        }
        private void Change_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (MainWindow.user.Role == "Admin")
                    main.frame1.Navigate(new Pages.Users.UsersAdd(mainWindow,users,null, main));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (MainWindow.user.Role == "Admin")
                {
                    var message = MessageBox.Show("Вы хотите удалить пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (message == MessageBoxResult.Yes)
                    {
                        MySqlConnection mySqlConnection = new MySqlConnection(Classes.WorkingBD.GetConnectionString());
                        mySqlConnection.Open();
                        MySqlDataReader userQuery = Classes.WorkingBD.Query($"DELETE FROM `kvanto`.`users` WHERE (`Id` = '{users.Id}');", mySqlConnection);
                        mySqlConnection.Close();
                        MessageBox.Show("Успешное удаление пользователя", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    main.OpenPage(mainWindow, new Pages.Users.Users_main(mainWindow,null, main));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
