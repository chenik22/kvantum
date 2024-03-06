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
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        MainWindow mainWindow;
        Autorization autorization;
        public Autorization(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            Classes.Users.LoadUsers(mainWindow);

            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = 0;
            doubleAnimation2.To = 1;
            doubleAnimation2.Duration = TimeSpan.FromSeconds(2);
            BorderStart.BeginAnimation(Border.OpacityProperty, doubleAnimation2);
            AnimalTextPrev();

            // Создание RadialGradientBrush
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

        public async void AnimalTextPrev()
        {
            await Task.Delay(1000);
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();
            thicknessAnimation.From = new Thickness(-200, 20, 10, 0);
            thicknessAnimation.To = new Thickness(10, 20, 10, 0);
            thicknessAnimation.Duration = TimeSpan.FromMilliseconds(100);

            // Применение анимации к свойству Margin элемента управления
            textIsled.BeginAnimation(Button.MarginProperty, thicknessAnimation);

            await Task.Delay(300);
            ThicknessAnimation thicknessAnimation2 = new ThicknessAnimation();
            thicknessAnimation2.From = new Thickness(-200, 0, 10, 0);
            thicknessAnimation2.To = new Thickness(10, 0, 10, 0);
            thicknessAnimation2.Duration = TimeSpan.FromMilliseconds(100);

            // Применение анимации к свойству Margin элемента управления
            textYchis.BeginAnimation(Button.MarginProperty, thicknessAnimation2);

            await Task.Delay(300);
            ThicknessAnimation thicknessAnimation3 = new ThicknessAnimation();
            thicknessAnimation3.From = new Thickness(-250, 0, 10, 0);
            thicknessAnimation3.To = new Thickness(10, 0, 10, 0);
            thicknessAnimation3.Duration = TimeSpan.FromMilliseconds(100);

            // Применение анимации к свойству Margin элемента управления
            textStavOp.BeginAnimation(Button.MarginProperty, thicknessAnimation3);
        }
        public string Login;
        public string Password;
        public static string Userstats;
        
        private void Entrance_Click(object sender, RoutedEventArgs e)
        {
            bool IsLog = false;
            Password = Passwordd.Password;
            Login = LoginName.Text;
            foreach (Classes.Users User in mainWindow.UsersList)
            {
                if (User.Login == Login && User.Password == Password)
                {
                    Userstats = User.Role;
                    MainWindow.user.Id = User.Id;
                    MainWindow.user.Login = User.Login;
                    MainWindow.user.FIO = User.FIO;
                    MainWindow.user.Password = User.Password;
                    MainWindow.user.Kvantum = User.Kvantum;
                    MainWindow.user.Role = User.Role;
                    IsLog = true;
                }
            }
            if (IsLog)
            {
                if (Userstats == "users")
                {
                    if (MainWindow.user.Kvantum == "")
                    {
                        mainWindow.OpenPage(mainWindow, new Pages.Test.Test(mainWindow));
                    }
                    else mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));

                }    
                else
                {
                    mainWindow.OpenPage(mainWindow, new Pages.Main.Main(mainWindow));
                }
                    
            }
            else
            {
                MessageOn();
            }
        }

        private void LoginName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MessageOff();
        }

        private void Passwordd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            MessageOff();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            frame1.Visibility = Visibility.Visible;
            OpenPage(mainWindow, new Pages.Main.Registration(mainWindow, this));
        }

        #region ---Ошибка ввода---
        int MessErrCheck = 0;

        public async void MessageOn()
        {
            if (MessErrCheck == 0)
            {
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
                LoginName.BorderBrush = colorBrush2;
                Passwordd.BorderBrush = colorBrush2;
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
