using ExamTask.Main.Services;
using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.Windows;
using ExamWithDesktop.WPF.Interfaces;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ExamWithDesktop.Controls
{
    /// <summary>
    /// Логика взаимодействия для ShortInfo.xaml
    /// </summary>
    public partial class ShortInfo : UserControl
    {
        IUserService userService;
        public ShortInfo()
        {
            userService = new UserService();
            InitializeComponent();
        }

        private async void UserShortInfoBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            User user = await userService.GetAsync(long.Parse(IdTxt.Text));
            UserFullInfoWindow userFullInfoWindow = new UserFullInfoWindow();

            userFullInfoWindow.FirstNameTxt.Text = user.FirstName;
            userFullInfoWindow.LastNameTxt.Text = user.LastName;
            userFullInfoWindow.FacultyTxt.Text = user.Faculty;
            userFullInfoWindow.IdTxt.Text = user.Id.ToString();

            userFullInfoWindow.PortraitImg.ImageSource = user.Image is not null
                        ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Image.Path))
                        : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

            userFullInfoWindow.PassportImg.ImageSource = user.Passport is not null
                        ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Passport.Path))
                        : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));


            userFullInfoWindow.Show();
        }

    }
}