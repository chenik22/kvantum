using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace Kvanto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(this, new Pages.Main.Autorization(this));
        }
        public List<Classes.Users> UsersList = new List<Classes.Users>();
        public List<Classes.Kvantum> KvantumList = new List<Classes.Kvantum>();
        public List<Classes.Test> TestList = new List<Classes.Test>();
        public static Classes.Users user = new Classes.Users(0, "", "", "", "", "","");
        public enum pages
        {
            authorization,
            main,
            registration
        }
        public void OpenPage(MainWindow mainWindow, Page ToPage)
        {
            Classes.Users.LoadUsers(this);
            Classes.Kvantum.LoadKvantum(this);
            Classes.Test.LoadTest(this);
            mainWindow.frame.Navigate(ToPage);
        }


        public void ErrMessOn(string Message, string MessageInfo)
        {
            ErrMessBord.Visibility = Visibility.Visible;
            ErrMess.Text = MessageInfo;
            ErrText.Text = Message;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = TimeSpan.FromSeconds(0.5);
            ErrMessBord.BeginAnimation(OpacityProperty, animation);
            ErrMessBord.Child = null;
            var bg = new BrushConverter();
            Border border = new Border();
            border.Height = 300;
            border.Width = 550;
            border.Background = (Brush)bg.ConvertFrom("#FF1A1A1A");
            border.CornerRadius = new CornerRadius(50, 50, 50, 50);
            ErrMessBord.Child = border;
            Grid grid = new Grid();
            border.Child = grid;
            TextBlock TBErrMess = new TextBlock();
            TBErrMess.Text = MessageInfo;
            TBErrMess.VerticalAlignment = VerticalAlignment.Top;
            TBErrMess.HorizontalAlignment = HorizontalAlignment.Left;
            TBErrMess.Foreground = Brushes.White;
            TBErrMess.FontSize = 20;
            TBErrMess.Margin = new Thickness(30, 15, 0, 0);
            grid.Children.Add(TBErrMess);
            Border border2 = new Border();
            border2.VerticalAlignment = VerticalAlignment.Stretch;
            border2.HorizontalAlignment = HorizontalAlignment.Stretch;
            border2.Margin = new Thickness(30, 60, 30, 60);
            border2.BorderBrush = Brushes.White;
            border2.BorderThickness = new Thickness(1, 1, 1, 1);
            grid.Children.Add(border2);
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            border2.Child = scrollViewer;
            StackPanel stackPanel = new StackPanel();
            scrollViewer.Content = stackPanel;
            TextBlock TBErrText = new TextBlock();
            TBErrText.Text = Message;
            TBErrText.Foreground = Brushes.Red;
            TBErrText.FontSize = 15;
            TBErrText.Margin = new Thickness(10,10,10,10);
            TBErrText.TextWrapping = TextWrapping.Wrap;
            stackPanel.Children.Add(TBErrText);
            Button button = new Button();
            button.Margin = new Thickness(0, 0, 30, 15);
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.Background = null;
            button.Foreground = Brushes.White;
            button.Width = 90;
            button.BorderBrush = (Brush)bg.ConvertFrom("#FF03A9F4");
            TextBlock TBBut = new TextBlock();
            TBBut.Text = "Ok";
            TBBut.TextAlignment = TextAlignment.Center;
            TBBut.Margin = new Thickness(0, 0, 0, 4);
            button.Content = TBBut;
            button.Click += delegate
            {
                ErrMessOff();
                ErrMessBord.Child = null;
            };
            grid.Children.Add(button);
        }

        public async void ErrMessOff()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1;
            animation.To = 0;
            animation.Duration = TimeSpan.FromSeconds(0.2);
            ErrMessBord.BeginAnimation(OpacityProperty, animation);
            await Task.Delay(200);
            ErrMessBord.Visibility = Visibility.Collapsed;
        }

        private void ErrClick(object sender, RoutedEventArgs e)
        {
            ErrMessOff();
        }
    }
}
