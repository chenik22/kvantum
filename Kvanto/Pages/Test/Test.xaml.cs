using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Kvanto.Pages.Test
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Page
    {
        MainWindow mainWindow;
        Pages.Test.CastomRadioButton CastomRadioButton;
        //для подсчета количества баллов за определенныйй предмет
        private double med = 0;
        private double teh = 0;
        private double dis = 0;
        private double him = 0;
        private double bio = 0;
        private double prog = 0;
        private double vr = 0;
        private double rob = 0;
        private double cos = 0;
        private double avi = 0;
        public Test(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;


            if (MainWindow.user.Kvantum != "")
            {
                myTabControl.Visibility = Visibility.Hidden;
                progressBar.Visibility = Visibility.Hidden;
                Border_TextNumber.Visibility = Visibility.Hidden;
                System.Windows.MessageBox.Show("Вы уже прошли тест", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                LoadQuestionsFromFile();
            progressBar.Value = 1;
        }
        public void Back_Click(object sender, MouseButtonEventArgs e)
        {
            mainWindow.OpenPage(mainWindow, new Pages.Main.Autorization(mainWindow));
        }
        private void CalculateResults()
        {
            myTabControl.Visibility = Visibility.Hidden;
            // Создаем словарь для хранения баллов каждого предмета
            Dictionary<string, double> scores = new Dictionary<string, double>
            {
                { "Медиаквантум", med },
                { "Промышленный дизайн", teh },
                { "Хайтек", dis },
                { "Наноквантум", him },
                { "Биоквантум", bio },
                { "IT-квантум", prog },
                { "VR / Ar", vr },
                { "Промробоквантум", rob },
                { "Космоквантум", cos },
                { "Аэроквантум", avi }
            };

            // Находим предмет с максимальным количеством баллов
            KeyValuePair<string, double> maxScore = scores.OrderByDescending(x => x.Value).First();

            // Выводим сообщение с результатом на экран
            string message = "Итог теста:\n\n";
            foreach (KeyValuePair<string, double> score in scores)
            {
                message += $"{score.Key}: {score.Value} баллов\n";
            }
            message += $"Предмет с наибольшим количеством баллов: {maxScore.Key}";

            MessageBox.Show(message, "Результат теста", MessageBoxButton.OK, MessageBoxImage.Information);

            // Отображение сообщения с вопросом
            MessageBoxResult result = MessageBox.Show("Хотите ли вы записаться на этот предмет?", "Запись на " + maxScore.Key, MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Обработка ответа пользователя
            if (result == MessageBoxResult.Yes)
            {
                // Выполнять действия при выборе "Да"
                MySqlConnection mySqlConnection = new MySqlConnection(Classes.WorkingBD.GetConnectionString());
                mySqlConnection.Open();
                bool teacherFound = false; // Для отслеживания наличия учителя

                foreach (Classes.Users Users in mainWindow.UsersList)
                {
                    if (Users.Role == "teacher" && Users.Kvantum == maxScore.Key.ToString())
                    {
                        MySqlDataReader add = Classes.WorkingBD.Query($"UPDATE `kvanto`.`users` SET `Kvantum` = '{maxScore.Key.ToString()}', `Teacher`='{Users.FIO}' WHERE (`Id` = '{MainWindow.user.Id}');", mySqlConnection);
                        MessageBox.Show("Вы успешно записаны на урок!", "Успешная запись", MessageBoxButton.OK, MessageBoxImage.Information);
                        mySqlConnection.Close();
                        mainWindow.OpenPage(mainWindow, new Pages.Main.Autorization(mainWindow));
                        teacherFound = true; // Если учитель найден
                        break; // Выход, как только найден учитель
                    }
                }
                // Сообщение если учетеля на данный момент нет
                if (!teacherFound)
                {
                    MessageBox.Show("На данном предмете пока нет учителя.", "Отсутствие учителя", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (result == MessageBoxResult.No)
            {
                // Выполнять действия при выборе "Нет"
                MessageBox.Show("Ок, вы не будете записаны на урок.", "Отмена записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        int numberVopr = 1;
        private void NextTabButton_Click(object sender, RoutedEventArgs e)
        {

            TabItem currentTab = myTabControl.SelectedItem as TabItem;
            int currentIndex = myTabControl.SelectedIndex;
            int nextIndex = (currentIndex + 1) % myTabControl.Items.Count;

            Grid currentGrid = currentTab.Content as Grid;
            StackPanel answersStackPanel = FindVisualChild<StackPanel>(currentGrid);

            bool isAnswered = false;
            foreach (RadioButton radioButton in answersStackPanel.Children.OfType<RadioButton>())
            {
                if (radioButton.IsChecked == true)
                {
                    isAnswered = true;
                    break;
                }
            }

            if (isAnswered)
            {
                myTabControl.SelectedIndex = nextIndex;
                UpdateButtonVisibility(nextIndex);
                progressBar.Value++;

                // Получаем выбранный ответ
                RadioButton selectedAnswer = answersStackPanel.Children.OfType<RadioButton>()
                                            .FirstOrDefault(r => r.IsChecked == true);
                string answer = selectedAnswer?.Content.ToString();
                if (numberVopr != 20)
                {
                    numberVopr++;
                    TextNumberVopr.Text = numberVopr + " / 20";
                }
                // Увеличиваем баллы для соответствующего предмета
                switch (currentIndex)
                {
                    case 0:
                        if (answer == "А) Редактирование фотографий или видео")
                            med++;
                        if (answer == "Б) Сборка мебели своими руками")
                        {
                            teh++;
                            dis += 0.25;
                        }
                        if (answer == "В) Рисование или зарисовка эскизов")
                            dis++;
                        if (answer == "D) Проведение экспериментов")
                        {
                            bio += 0.25;
                            him += 1;
                        }
                        if (answer == "Д) Изучение живых организмов")
                            bio += 1;
                        break;
                    case 1:
                        if (answer == "А) Творческий")
                        {
                            dis += 0.5;
                            bio += 0.5;
                        }
                        if (answer == "Б) Практический")
                        {
                            med += 0.5;
                            rob += 0.5;
                        }
                        if (answer == "В) Художественный")
                        {
                            prog += 0.5;
                            cos += 0.5;
                        }
                        if (answer == "D) Аналитический")
                        {
                            teh += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "E) Систематический")
                        {
                            avi += 0.5;
                            him += 0.5;
                        }
                        break;
                    case 2:
                        if (answer == "А) Коммуникация или журналистика")
                        {
                            dis += 0.5;
                            rob += 0.5;
                        }
                        if (answer == "Б) Производство или машиностроение")
                        {
                            med += 0.5;
                            teh += 0.5;
                        }
                        if (answer == "В) Художественный или графический дизайн")
                        {
                            him += 0.5;
                            cos += 0.5;
                        }
                        if (answer == "D) Химия или химическая инженерия")
                        {
                            bio += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "E) Биология или науки о жизни")
                        {
                            prog += 0.5;
                            avi += 0.5;
                        }
                        break;
                    case 3:
                        if (answer == "А) Совместная работа и использование мультимедиа")
                            med++;
                        if (answer == "Б) Мастерская или заводской цех")
                            teh++;
                        if (answer == "C) Студия или дизайнерское пространство")
                            dis++;
                        if (answer == "D) Лаборатория или исследовательская среда")
                            him++;
                        if (answer == "E) Полевые работы или исследования")
                            bio++;
                        break;
                    case 4:
                        if (answer == "A) Программное обеспечение для редактирования видео")
                            med++;
                        if (answer == "Б) Токарный или фрезерный станок")
                            teh++;
                        if (answer == "C) Программное обеспечение для графического дизайна")
                            dis++;
                        if (answer == "D) Оборудование для химического анализа")
                            him++;
                        if (answer == "E) Микроскоп или инструменты для увеличения")
                            bio++;
                        break;
                    case 5:
                        if (answer == "А) Создание или использование мультимедийного контента")
                        {
                            prog++;
                            vr += 0.5;
                        }
                        if (answer == "Б) Строительство или ремонт чего-либо")
                            teh++;
                        if (answer == "В) Рисование, раскрашивание или рукоделие")
                            dis++;
                        if (answer == "Г) Экспериментирование с химическими реакциями")
                            him++;
                        if (answer == "E) Документальные фильмы об ученых")
                        {
                            med += 0.25;
                            bio += 0.5;
                        }
                        break;
                    case 6:
                        if (answer == "А) Виртуальная реальность (VR) или дополненная реальность (AR)")
                        {
                            vr++;
                            prog += 0.25;
                        }
                        if (answer == "Б) 3D-принтеры")
                        {
                            teh += 0.5;
                            rob += 0.5;
                        }
                        if (answer == "C) Веб-дизайн или дизайн пользовательского интерфейса (UI)")
                        {
                            prog++;
                            med += 0.25;
                        }
                        if (answer == "D) Нанотехнологии")
                        {
                            him++;
                            rob += 0.25;
                        }
                        if (answer == "E) Геномика и биоинформатика")
                        {
                            med++;
                            him += 0.25;
                        }
                        break;
                    case 7:
                        if (answer == "А) Путем наблюдения")
                        {
                            med += 0.5;
                            cos += 0.5;
                        }
                        if (answer == "Б) Делая или практикуя")
                        {
                            teh += 0.5;
                            avi += 0.5;
                        }
                        if (answer == "В) Путем создания или имитации")
                        {
                            dis += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "Г) Читая, изучая или экспериментируя")
                        {
                            him += 0.5;
                            bio += 0.5;
                        }
                        if (answer == "Д) Путем наблюдения и анализа")
                        {
                            prog += 0.5;
                            rob += 0.5;
                        }
                        break;
                    case 8:
                        if (answer == "А) Совместный и визуально привлекательный")
                        {
                            med += 0.5;
                            prog += 0.25;
                        }
                        if (answer == "Б) Осязаемый и функциональный")
                        {
                            prog += 0.75;
                            rob += 0.5;
                        }
                        if (answer == "В) Эстетически приятный")
                        {
                            teh += 0.5;
                            avi++;
                        }
                        if (answer == "D) Ориентированный на исследования и новаторский подход")
                        {
                            cos += 0.5;
                            him += 0.25;
                        }
                        if (answer == "E) Аналитический и содержательный")
                        {
                            dis += 0.5;
                            him += 0.5;
                        }
                        break;
                    case 9:
                        if (answer == "А) Коммуникация и рассказывание историй")
                        {
                            bio += 0.25;
                            prog += 0.5;
                        }
                        if (answer == "Б) Точность и внимание к деталям")
                        {
                            teh += 0.5;
                            dis += 0.5;
                        }
                        if (answer == "В) Креативность и художественный талант")
                        {
                            cos += 0.75;
                            med += 0.5;
                        }
                        if (answer == "D) Анализ и решение проблем")
                        {
                            vr += 0.5;
                            him += 0.5;
                        }
                        if (answer == "Д) Критическое мышление и наблюдательность")
                        {
                            bio += 0.5;
                            cos += 0.25;
                        }
                        break;
                    case 10:
                        if (answer == "А) Создание рекламных видеороликов")
                        {
                            med += 0.5;
                            dis += 0.5;
                        }
                        if (answer == "Б) Сборка или модификация оборудования")
                        {
                            rob += 0.5;
                            teh += 0.5;
                        }
                        if (answer == "В) Разработка логотипа или корпоративного брендинга")
                        {
                            dis += 0.5;
                            prog += 0.5;
                        }
                        if (answer == "D) Анализ и оптимизация химических процессов")
                            him += 0.5;
                        if (answer == "Д) Изучение поведения животных")
                            bio++;
                        break;
                    case 11:
                        if (answer == "А) Выразительный и обладающий богатым воображением")
                        {
                            teh += 0.25;
                            dis += 0.25;
                            med += 0.25;
                        }
                        if (answer == "Б) Настойчивый и дотошный")
                        {
                            prog += 0.5;
                            teh += 0.25;
                        }
                        if (answer == "C) Инновационный и оригинальный")
                        {
                            vr += 0.5;
                            rob += 0.5;
                            avi += 0.5;
                        }
                        if (answer == "Г) Любознательный и рациональный")
                        {
                            him += 0.25;
                            bio += 0.5;
                            cos += 0.5;
                        }
                        if (answer == "Д) Любознательный и ориентированный на детали")
                        {
                            cos += 0.5;
                            him += 0.5;
                            avi += 0.5;
                        }
                        break;
                    case 12:
                        if (answer == "А) Путем многозадачности и расстановки приоритетов")
                        {
                            teh += 0.5;
                            avi += 0.5;
                        }
                        if (answer == "Б) Сосредоточившись и методично работая")
                        {
                            prog += 0.5;
                            med += 0.5;
                        }
                        if (answer == "В) Путем поиска творческих решений")
                        {
                            him += 0.5;
                            bio += 0.5;
                        }
                        if (answer == "Г) Спокойно проанализировав ситуацию")
                        {
                            rob += 0.5;
                            bio += 0.5;
                        }
                        if (answer == "E) Путем систематического разбиения задач")
                        {
                            him += 0.5;
                            vr += 0.5;
                        }
                        break;
                    case 13:
                        if (answer == "А) Междисциплинарное сотрудничество")
                        {
                            bio += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "Б) Функциональное и практическое сотрудничество")
                        {
                            prog += 0.5;
                            rob += 0.5;
                        }
                        if (answer == "В) Творческий мозговой штурм")
                        {
                            him += 0.5;
                            teh += 0.5;
                        }
                        if (answer == "D) Сотрудничество в области исследований и разработок")
                        {
                            med += 0.5;
                            bio += 0.5;
                        }
                        if (answer == "E) Совместный анализ данных")
                        {
                            him += 0.5;
                            avi += 0.5;
                        }
                        break;
                    case 14:
                        if (answer == "А) Платформы социальных сетей")
                        {
                            prog += 0.5;
                            cos += 0.5;
                        }
                        if (answer == "Б) Промышленные роботы")
                        {
                            rob += 0.5;
                            avi += 0.5;
                        }
                        if (answer == "C) Приложения виртуальной или дополненной реальности")
                        {
                            vr += 0.5;
                        }
                        if (answer == "D) Жизненно важные фармацевтические препараты")
                        {
                            him += 0.5;
                        }
                        if (answer == "E) Прорывы в области биотехнологий")
                        {
                            bio += 0.5;
                        }
                        break;
                    case 15:
                        if (answer == "А) Вирусная видеокампания")
                            med++;
                        if (answer == "Б) Высокопроизводительная машина или изделие")
                            teh++;
                        if (answer == "В) Выдающаяся визуальная идентичность бренда")
                            dis++;
                        if (answer == "D) Новаторское химическое открытие")
                            him++;
                        if (answer == "Д) Значительное научное открытие")
                            bio++;
                        break;
                    case 16:
                        if (answer == "А) Необходим для выполнения некоторых задач")
                            prog++;
                        if (answer == "Б) Полезен для технической работы")
                            rob++;
                        if (answer == "C) Иногда уместно в дизайне")
                            vr++;
                        if (answer == "D) Решающее значение для научных открытий")
                            avi++;
                        if (answer == "E) Важно для анализа данных и решения проблем")
                            cos++;
                        break;
                    case 17:
                        if (answer == "А) Мрачные размышления СМИ")
                        {
                            med += 0.5;
                            dis += 0.5;
                        }
                        if (answer == "Б) Футуристические фабрики и производство")
                        {
                            prog += 0.5;
                            avi += 0.5;
                        }
                        if (answer == "В) Инопланетные миры и фантастические конструкции")
                        {
                            him += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "Г) Безумный ученый и экспериментальная катастрофа")
                        {
                            rob += 0.5;
                            teh += 0.5;
                        }
                        if (answer == "E) Передовые медицинские и биологические открытия")
                        {
                            bio += 0.5;
                            cos += 0.5;
                        }
                        break;
                    case 18:
                        if (answer == "А) Выражение сообщения или идеи")
                        {
                            avi += 0.5;
                            cos += 0.5;
                        }
                        if (answer == "Б) Получение ощутимых и функциональных результатов")
                        {
                            dis += 0.5;
                            prog += 0.5;
                        }
                        if (answer == "C) Создание визуально ошеломляющей работы")
                        {
                            bio += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "D) Расширение знаний и понимания")
                        {
                            rob += 0.5;
                            teh += 0.5;
                        }
                        if (answer == "Д) Обнаружение или раскрытие новой информации")
                        {
                            med += 0.5;
                            him += 0.5;
                        }
                        break;
                    case 19:
                        if (answer == "А) Мультимедийный рассказчик")
                        {
                            rob += 0.5;
                            dis += 0.5;
                        }
                        if (answer == "Б) Новатор продукта или оборудования")
                        {
                            med += 0.5;
                            vr += 0.5;
                        }
                        if (answer == "C) Всемирно известный дизайнер")
                        {
                            cos += 0.5;
                            bio += 0.5;
                        }
                        if (answer == "D) Химик-первопроходец")
                        {
                            him += 0.5;
                            teh += 0.5;
                        }
                        if (answer == "E) Биолог-новатор")
                        {
                            avi += 0.5;
                            prog += 0.5;
                        }
                        CalculateResults(); // Вызываем функцию подсчета результатов
                        break;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ответ на вопрос", "Незавершенный ответ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PreviousTabButton_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = myTabControl.SelectedIndex;
            int previousIndex = (currentIndex - 1 + myTabControl.Items.Count) % myTabControl.Items.Count;
            myTabControl.SelectedIndex = previousIndex;
            UpdateButtonVisibility(previousIndex);
            progressBar.Value--;
            if (numberVopr != 1)
            {
                numberVopr--;
                TextNumberVopr.Text = numberVopr + " / 20";
            }
        }
        Style blueRadioButtonStyle = (Style)Application.Current.Resources["BlueRadioButtonStyle"];
        Style BlueButtonStyle = (Style)Application.Current.Resources["BlueButtonStyle"];
        Style PrevButtonStyle = (Style)Application.Current.Resources["PrevButtonStyle"];
        private void LoadQuestionsFromFile()
        {
            string filePath = "C:\\Users\\Chenik\\Downloads\\Kvanto2\\asd.txt";
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                string question = parts[0];
                string[] answers = parts.Skip(1).ToArray();

                TabItem tabItem = new TabItem();
                Grid grid = new Grid();
                tabItem.Header = question;
                tabItem.Content = grid;
                tabItem.IsEnabled = false;
                if (tabItem != null)
                {
                    tabItem.Visibility = Visibility.Collapsed;
                }

                TextBlock questionTextBlock = new TextBlock();
                questionTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                questionTextBlock.TextWrapping = TextWrapping.Wrap;
                questionTextBlock.Text = question;
                questionTextBlock.FontSize = 23;
                questionTextBlock.Margin = new Thickness(10, 10, 0, 0);
                grid.Children.Add(questionTextBlock);

                StackPanel answersStackPanel = new StackPanel();
                answersStackPanel.Margin = new Thickness(10, 74, 0, 0);
                grid.Children.Add(answersStackPanel);

                foreach (string answer in answers)
                {
                    RadioButton radioButton2 = new RadioButton();
                    radioButton2.Content = answer;
                    radioButton2.Margin = new Thickness(0, 5, 0, 5);
                    radioButton2.Style = blueRadioButtonStyle;
                    radioButton2.Cursor = Cursors.Hand;
                    answersStackPanel.Children.Add(radioButton2);
                }

                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Orientation = Orientation.Horizontal;
                answersStackPanel.Children.Add(stackPanel2);

                Button nextButton = new Button();
                nextButton.Width = 150;
                nextButton.Height = 45;
                nextButton.Style = BlueButtonStyle;
                nextButton.Cursor = Cursors.Hand;
                nextButton.Margin = new Thickness(10, 10, 0, 10);
                nextButton.Content = "Далее";
                nextButton.HorizontalAlignment = HorizontalAlignment.Left;
                nextButton.VerticalAlignment = VerticalAlignment.Bottom;
                nextButton.Click += NextTabButton_Click;
                stackPanel2.Children.Add(nextButton);

                Button prevButton = new Button();
                prevButton.Width = 150;
                prevButton.Height = 45;
                prevButton.Margin = new Thickness(10, 10, 0, 10);
                prevButton.Style = PrevButtonStyle;
                prevButton.Cursor = Cursors.Hand;
                prevButton.Content = "Назад";
                prevButton.HorizontalAlignment = HorizontalAlignment.Left;
                prevButton.VerticalAlignment = VerticalAlignment.Bottom;
                prevButton.Click += PreviousTabButton_Click;
                prevButton.Visibility = i == 0 ? Visibility.Collapsed : Visibility.Visible; // Hide "Назад" button for the first question
                stackPanel2.Children.Add(prevButton);

                myTabControl.Items.Add(tabItem);
                UpdateButtonVisibility(0); // Show/hide "Назад" button for the first question initially
            }
        }

        private void UpdateButtonVisibility(int currentIndex)
        {
            for (int i = 0; i < myTabControl.Items.Count; i++)
            {
                TabItem tabItem = myTabControl.Items[i] as TabItem;
                Grid grid = tabItem.Content as Grid;
                Button prevButton = FindVisualChild<Button>(grid);
            }
        }
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}
