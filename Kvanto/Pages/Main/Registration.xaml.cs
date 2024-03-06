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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kvanto.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        MainWindow mainWindow;
        Autorization autorization;
        public Registration(MainWindow _mainWindow, Autorization _autorization)
        {
            InitializeComponent();
            autorization = _autorization;
            mainWindow = _mainWindow;
        }
        public string tlogin;
        public string tfio;
        public string tpassword;
        public string trecoveryPassword;

        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (login.Text != "" && fio.Text != "" &&  password.Password != "" && recoveryPassword.Password != "")
                {
                    if (!Classes.Check.ItsOnlyFIO(fio.Text))
                    {
                        MessageOn("Вы не правильно заполнили поле ФИО!");
                        return;
                    }
                    if (password.Password != recoveryPassword.Password)
                    {
                        MessageOn("Пароли не совпадают!");
                        return;
                    }
                    tlogin = login.Text;
                    tfio = fio.Text;
                    tpassword = password.Password;
                    string userrole = "users";
                    MySqlConnection mySqlConnection = new MySqlConnection(WorkingBD.GetConnectionString());
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
                        MySqlDataReader userQuery = Classes.WorkingBD.Query($"INSERT INTO `kvanto`.`users` (`Login`, `FIO`, `Password`, `Role`) VALUES ('{tlogin}', '{tfio}', '{tpassword}', '{userrole}');", mySqlConnection);
                        MessageBox.Show("Успешная регистрация", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        mainWindow.frame.Navigate(new Pages.Main.Autorization(mainWindow));
                    }
                    else
                    {
                        MessageOn("Пользователь с данным логином уже имеется");
                    }
                }
                else MessageOn("Заполните все поля!");
            }
            catch (Exception ex)
            {
                mainWindow.ErrMessOn(ex.Message, "Ой! Что то пошло не так!");
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            autorization.frame1.Visibility = Visibility.Collapsed;
        }

        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MessageOff();
        }

        #region ---Ошибка ввода---
        int MessErrCheck = 0;

        public async void MessageOn(string Message)
        {
            if (MessErrCheck == 0)
            {
                ErrText.Text = Message;
                SolidColorBrush colorBrush = new SolidColorBrush();
                colorBrush.Color = Color.FromArgb(0, 255, 0, 0);
                ErrText.Foreground = colorBrush;
                BackEase backEase = new BackEase()
                {
                    Amplitude = 0
                };
                backEase.EasingMode = EasingMode.EaseOut;
                ColorAnimation colorAnimation = new ColorAnimation(Color.FromArgb(255, 255, 0, 0), TimeSpan.FromSeconds(1))
                {
                    EasingFunction = backEase
                };
                colorBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                MessErrCheck = 1;

                SolidColorBrush colorBrush2 = new SolidColorBrush();
                colorBrush2.Color = Color.FromArgb(255, 0, 131, 197);
                if (ErrText.Text == "Вы не правильно заполнили поле ФИО!")
                {
                    fio.BorderBrush = colorBrush2;
                }
                else if (ErrText.Text == "Пароли не совпадают!")
                {
                    password.BorderBrush = colorBrush2;
                    recoveryPassword.BorderBrush = colorBrush2;
                }
                else if (ErrText.Text == "Пользователь с данным логином уже имеется")
                {
                    login.BorderBrush = colorBrush2;
                }
                else if (ErrText.Text == "Заполните все поля!")
                {
                    login.BorderBrush = colorBrush2;
                    fio.BorderBrush = colorBrush2;
                    password.BorderBrush = colorBrush2;
                    recoveryPassword.BorderBrush = colorBrush2;
                }
                BackEase backEase2 = new BackEase()
                {
                    Amplitude = 0
                };
                backEase2.EasingMode = EasingMode.EaseOut;
                ColorAnimation colorAnimation2 = new ColorAnimation(Color.FromArgb(255, 255, 0, 0), TimeSpan.FromSeconds(1))
                {
                    EasingFunction = backEase2
                };
                colorBrush2.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation2);
                MessErrCheck = 1;
                await Task.Delay(1500);
                ColorAnimation colorAnimation3 = new ColorAnimation(Color.FromArgb(255, 0, 131, 197), TimeSpan.FromSeconds(1))
                {
                    EasingFunction = backEase2
                };
                colorBrush2.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation3);
            }

        }

        public void MessageOff()
        {
            if (MessErrCheck == 1)
            {
                SolidColorBrush colorBrush = new SolidColorBrush();
                colorBrush.Color = Color.FromArgb(255, 255, 0, 0);
                ErrText.Foreground = colorBrush;
                BackEase backEase = new BackEase()
                {
                    Amplitude = 0
                };
                backEase.EasingMode = EasingMode.EaseOut;
                ColorAnimation colorAnimation = new ColorAnimation(Color.FromArgb(0, 255, 0, 0), TimeSpan.FromSeconds(0.5))
                {
                    EasingFunction = backEase
                };
                colorBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                MessErrCheck = 0;
            }
        }
        #endregion
    }
}
