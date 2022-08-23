using ExamWithDesktop.Controls;
using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.Pages;
using ExamWithDesktop.Service.Services;
using ExamWithDesktop.WPF.Pages;
using ExamWithDesktop.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExamWithDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AddPage addPage;
        private IEnumerable<User> allUsers;
        private readonly UserService userService;
        Thread thread;

        public MainWindow()
        {
            InitializeComponent();
            addPage = new AddPage();
            userService = new UserService();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MovePanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private async Task LoadUsers(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    Button button = new Button()
                    {
                        Background = new SolidColorBrush(Color.FromRgb(38, 28, 44)),
                        Padding = new Thickness(0),
                        Height = 60,
                        BorderThickness = new Thickness(0),
                    };
                    button.Click += UserFullInfoButton_Click;
                    ShortInfo shortInfo = new ShortInfo();
                    shortInfo.NameTxt.Text = user.FirstName + " " + user.LastName;
                    shortInfo.Id = user.Id;
                    shortInfo.UserImg.ImageSource = user.Image is not null
                        ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Image.Path))
                        : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

                    button.Content = shortInfo;

                    UsersList.Items.Add(button);
                });
            }
        }

        private async void UserFullInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            var shortInfo = button.Content as ShortInfo;

            User user = await userService.GetAsync(shortInfo.Id);

            SavePage savePage = new SavePage();

            if (user != null)
            {
                savePage.FirstNameTxt.Text = user.FirstName;
                savePage.LastNameTxt.Text = user.LastName;
                savePage.FacultyTxt.Text = user.Faculty;
                savePage.IdTxt.Text = user.Id.ToString();

                savePage.PortraitImg.ImageSource = user.Image is not null
                            ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Image.Path))
                            : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

                savePage.PassportImg.ImageSource = user.Passport is not null
                            ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Passport.Path))
                            : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

                UserFrame.Content = savePage;
            }
            else
            {
                new ErrorWindow().ShowDialog();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.Content = addPage;
        }



        private void SearchTxtBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UsersList.Items.Clear();

            string text = SearchTxtBox.Text.ToLower();

            var users = allUsers.Where(p => p.FirstName.ToLower().Contains(text)
                || p.LastName.ToLower().Contains(text) || p.Id.ToString() == text);

            UsersList.Items.Clear();


            var thread = new Thread(async () =>
            {
                await LoadUsers(users);
            });
            thread.Start();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.Content = new SavePage();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread = new Thread(async () =>
            {
                this.Dispatcher.Invoke(() => UsersList.Items.Clear());

                allUsers = await userService.GetAllAsync();
                await LoadUsers(allUsers);
            });

            thread.Start();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
        }
    }
}
