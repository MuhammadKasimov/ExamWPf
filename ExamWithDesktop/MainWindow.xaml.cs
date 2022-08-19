using ExamTask.Main.Services;
using ExamWithDesktop.Controls;
using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.Pages;
using ExamWithDesktop.WPF.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ExamWithDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AddPage addPage;
        private readonly DeletePage deletePage;
        private IEnumerable<User> allUsers;
        UserService userService;
        Thread thread;

        public MainWindow()
        {
            InitializeComponent();
            deletePage = new DeletePage();
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
                await this.Dispatcher.InvokeAsync(() =>
                {
                    ShortInfo shortInfo = new ShortInfo();
                    shortInfo.NameTxt.Text = user.FirstName + " " + user.LastName;
                    shortInfo.IdTxt.Text = user.Id.ToString();
                    shortInfo.UserImg.ImageSource = user.Image is not null
                        ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Image.Path))
                        : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

                    UsersList.Items.Add(shortInfo);
                });
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.Content = addPage;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            UserFrame.Content = deletePage;
        }


        private void SearchTxtBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UsersList.Items.Clear();

            string text = SearchTxtBox.Text.ToLower();

            var users = allUsers.Where(p => p.FirstName.ToLower().Contains(text)
                || p.LastName.ToLower().Contains(text));

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
