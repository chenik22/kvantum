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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Kvanto.Pages.Users
{
    /// <summary>
    /// Логика взаимодействия для Users_Info.xaml
    /// </summary>
    public partial class Users_Info : Page
    {
        MainWindow mainWindow;
        Classes.Users users;
        public Page PrPage;

        Pages.Main.Main main1;

        public Users_Info(MainWindow mainWindow,Classes.Users user, Page page, Pages.Main.Main main)
        {
            InitializeComponent();
            Classes.Users.LoadUsers(mainWindow);
            Name(mainWindow,  user,  page, main);
            AnimBut();
        }
        public void Name(MainWindow mainWindow, Classes.Users user, Page page, Pages.Main.Main main)
        {
            Classes.Users.LoadUsers(mainWindow);
            string userName;

            try
            {
                userName = MainWindow.user.FIO.Split(' ')[0] + "  " + MainWindow.user.FIO.Split(' ')[1];
            }
            catch
            {
                userName = "Error";
            }


            main1 = main;
            this.mainWindow = mainWindow;
            users = user;
            PrPage = page;
            Login.Content = "Логин: " + " " + user.Login;
            FIO.Content = userName;
            if (MainWindow.user.Role == "Admin")
            {
                Kvan.Visibility = Visibility.Hidden;
                Teacher.Visibility = Visibility.Hidden;
            }
            else
            {
                Kvan.Content = "Предмет: " + " " + user.Kvantum;
                Teacher.Content = "Учитель: " + " " + user.Teacher;
            }
        }
        public void AnimBut()
        {
            RadialGradientBrush gradientBrush = new RadialGradientBrush();
            gradientBrush.GradientOrigin = new Point(0.5, 0.5);
            gradientBrush.Center = new Point(0.5, 0.5);
            gradientBrush.RadiusX = 0.5;
            gradientBrush.RadiusY = 0.5;

            // Определение цветовых остановок градиента
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 39, 83, 216), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 0, 131, 197), 1));

            // Применение градиентного фона к элементу
            Entrance.Background = gradientBrush;

            // Создание анимации для замены цветов
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = Color.FromArgb(255, 39, 83, 216);
            colorAnimation.To = Color.FromArgb(255, 0, 131, 197);
            colorAnimation.Duration = TimeSpan.FromSeconds(2);
            colorAnimation.AutoReverse = true;
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Применение анимации к цветовым остановкам градиента
            gradientBrush.GradientStops[0].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
            gradientBrush.GradientStops[1].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
        }

        private void ChageUsers_Click(object sender, RoutedEventArgs e)
        {
            Classes.Users.LoadUsers(mainWindow);
            // Обновление данных пользователя в MainWindow.user
            MainWindow.user = mainWindow.UsersList.FirstOrDefault(u => u.Id == MainWindow.user.Id);
            // Обновляем данные пользователя в базе данных
            Classes.Users.UpdateUser(MainWindow.user);
            // Обновляем данные на странице
            Name(mainWindow, MainWindow.user, PrPage, main1);
            main1.OpenPage(mainWindow, new Pages.Users.UsersAdd(mainWindow, MainWindow.user, null, main1));
        }

    }
}
