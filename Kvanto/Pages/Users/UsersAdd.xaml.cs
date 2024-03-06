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

namespace Kvanto.Pages.Users
{
    /// <summary>
    /// Логика взаимодействия для UsersAdd.xaml
    /// </summary>
    public partial class UsersAdd : Page
    {
        MainWindow mainWindow;
        Classes.Users users;
        public Page PrPage;
        Pages.Main.Main main;
        public UsersAdd(MainWindow mainWindow,Classes.Users user,Page page, Pages.Main.Main _main)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            users = user;
            PrPage = page;
            main = _main;
            if (MainWindow.user.Role == "users" || MainWindow.user.Role=="teacher")
            {
                role.Visibility = Visibility.Collapsed;
                kvan.Visibility = Visibility.Collapsed;
                Teacher.Visibility = Visibility.Collapsed;
            }
            if (user != null)
            {
                zagl.Content = "Изменение пользователя";
                Entrance.Content = "Изменить";
            }
            if (user != null)
            {
                login.Text = user.Login;
                fio.Text = user.FIO;
                password.Text = user.Password;
                role.Text = user.Role;
                kvan.Text=user.Kvantum;
            }
        }
        public string tLogin;
        public string tFio;
        public string tpasswordd;
        public string trole;
        public string tkvan;
        public string tTeacher;
        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tLogin = login.Text;
                tFio = fio.Text;
                tpasswordd = password.Text;
                trole = role.Text;
                tkvan = kvan.Text;
                tTeacher = Teacher.Text;
                MySqlConnection mySqlConnection = new MySqlConnection(Classes.WorkingBD.GetConnectionString());
                if (MainWindow.user.Role == "users" || MainWindow.user.Role == "teacher")
                {
                    if (role.Text == "Учитель")
                    {
                        if (login.Text != "" && fio.Text != "" && password.Text != "" && recoveryPassword.Text != "" && kvan.Text != "")
                        {
                            if (!Classes.Check.ItsOnlyFIO(fio.Text))
                            {
                                MessageBox.Show("Вы не правильно заполнили поле ФИО!");
                                return;
                            }
                            if (password.Text != recoveryPassword.Text)
                            {
                                MessageBox.Show("Пароли не совпадают!");
                                return;
                            }
                            if (users == null)
                            {
                                mySqlConnection.Open();
                                bool userExists = false; //Для отслеживания наличия пользователя с таким логином
                                foreach (Classes.Users Users in mainWindow.UsersList)
                                {
                                    if (Users.Login == login.Text) //Если пользователь с таким логином уже существует
                                    {
                                        userExists = true;
                                        break;
                                    }
                                }
                                if (!userExists) // Если равен false (т.е. пользователь с таким логином не существует)
                                {
                                    MySqlDataReader add = Classes.WorkingBD.Query($" UPDATE `kvanto`.`users` SET `Login` = '{tLogin}', `FIO` = '{tFio}',  `Password` = '{tpasswordd}', `Kvantum` = '{tkvan}' WHERE(`Id` = '{users.Id}');", mySqlConnection);
                                    MessageBox.Show("Успешное изменение пользователя", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                                    mySqlConnection.Close();
                                    mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));
                                }
                                else
                                {
                                    MessageBox.Show("Пользователь с данным логином уже имеется");
                                }
                            }
                        }
                        else
                            MessageBox.Show("Заполните предметы");
                    }
                    else
                    {
                        if (login.Text != "" && fio.Text != "" && password.Text != "" && recoveryPassword.Text != "")
                        {
                            if (!Classes.Check.ItsOnlyFIO(fio.Text))
                            {
                                MessageBox.Show("Вы не правильно заполнили поле ФИО!");
                                return;
                            }
                            if (password.Text != recoveryPassword.Text)
                            {
                                MessageBox.Show("Пароли не совпадают!");
                                return;
                            }
                            mySqlConnection.Open();
                            MySqlDataReader add = Classes.WorkingBD.Query($" UPDATE `kvanto`.`users` SET `Login` = '{tLogin}', `FIO` = '{tFio}',  `Password` = '{tpasswordd}' WHERE(`Id` = '{users.Id}');", mySqlConnection);
                            MessageBox.Show("Успешное изменение пользователя", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            mySqlConnection.Close();

                            mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));
                        }
                        else
                            MessageBox.Show("Заполните предметы");
                    }
                }
                else
                {
                    if (login.Text != "" && fio.Text != "" && password.Text != "" && recoveryPassword.Text != "" && role.Text!="")
                    {
                        if (!Classes.Check.ItsOnlyFIO(fio.Text))
                        {
                            MessageBox.Show("Вы не правильно заполнили поле ФИО!");
                            return;
                        }
                        if (password.Text != recoveryPassword.Text)
                        {
                            MessageBox.Show("Пароли не совпадают!");
                            return;
                        }
                        if (users == null)
                        {
                            mySqlConnection.Open();
                            bool userExists = false; //Для отслеживания наличия пользователя с таким логином
                            foreach (Classes.Users Users in mainWindow.UsersList)
                            {
                                if (Users.Login == login.Text) //Если пользователь с таким логином уже существует
                                {
                                    userExists = true;
                                    break;
                                }
                            }
                            if (!userExists) // Если равен false (т.е. пользователь с таким логином не существует)
                            {
                                MySqlDataReader add = Classes.WorkingBD.Query($"INSERT INTO `kvanto`.`users` (`Login`, `FIO`, `Password`, `Role`) VALUES ('{tLogin}', '{tFio}', '{tpasswordd}', '{trole}');", mySqlConnection);
                                MessageBox.Show("Успешное добавление пользователя", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                                mySqlConnection.Close();
                                main.OpenPage(mainWindow, new Pages.Users.Users_main(mainWindow, PrPage, main));
                            }
                            else
                            {
                                MessageBox.Show("Пользователь с данным логином уже имеется");
                            }
                        }
                        else if (users != null)
                        {
                            mySqlConnection.Open();
                            MySqlDataReader add = Classes.WorkingBD.Query($" UPDATE `kvanto`.`users` SET `Login` = '{tLogin}', `FIO` = '{tFio}',  `Password` = '{tpasswordd}',`Role` = '{trole}' WHERE(`Id` = '{users.Id}');", mySqlConnection);
                            MessageBox.Show("Успешное изменение пользователя", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            mySqlConnection.Close();
                            Classes.Users.LoadUsers(mainWindow);
                            mainWindow.OpenPage(mainWindow,this);
                            mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));
                        }
                    }
                    else MessageBox.Show("Заполните все поля!");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void InfoUser_Click(object sender, MouseButtonEventArgs e)
        {
            main.OpenPage(mainWindow, new Pages.Users.Users_Info(mainWindow, MainWindow.user,null, main));
        }

    }
}
