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

namespace Kvanto.Pages.Kurs.Elements
{
    /// <summary>
    /// Логика взаимодействия для items_Kurs.xaml
    /// </summary>
    public partial class items_Kurs : UserControl
    {
        MainWindow mainWindow;
        Classes.Kvantum kvan;
        Pages.Main.Main main;
        public items_Kurs(MainWindow mainWindow, Classes.Kvantum kvantum, Pages.Main.Main _main)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            kvan = kvantum;
            this.main = _main;
            Name.Text = kvantum.Name;
            Description.Text = kvantum.Description;
        }
        private void Change_Click(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.user.Role == "Admin" && main != null)
            {
                main.frame1.Navigate(new Pages.Kurs.Kurs_Add(mainWindow, null, kvan, main));
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
                        MySqlDataReader userQuery = Classes.WorkingBD.Query($"DELETE FROM `kvanto`.`kvantum` WHERE (`Id` = '{kvan.Id}');", mySqlConnection);
                        mySqlConnection.Close();
                        MessageBox.Show("Успешное удаление пользователя", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    main.OpenPage(mainWindow, new Pages.Kurs.Kurs_Main(mainWindow,null, main));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
