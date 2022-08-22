using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.Service.Interfaces;
using ExamWithDesktop.Service.Models;
using ExamWithDesktop.Service.Services;
using ExamWithDesktop.WPF.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ExamWithDesktop.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для SavePage.xaml
    /// </summary>
    public partial class SavePage : Page
    {
        private string pathOfPassportImage;
        private string portraitPath;
        readonly IUserService userService;
        private User user;
        UserForCreation userForCreation;
        public SavePage()
        {
            InitializeComponent();
            userService = new UserService();
            user = new User();
            userForCreation = new UserForCreation();
        }

        private async void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            long id;

            if (long.TryParse(IdTxt.Text, out id))
            {
                user = await userService.GetAsync(id);
                if (user != null)
                {
                    userForCreation.FirstName = String.IsNullOrEmpty(FirstNameTxt.Text) ? user.FirstName : FirstNameTxt.Text;

                    userForCreation.LastName = String.IsNullOrEmpty(LastNameTxt.Text) ? user.LastName : LastNameTxt.Text;

                    userForCreation.Faculty = String.IsNullOrEmpty(FacultyTxt.Text) ? user.Faculty : FacultyTxt.Text;

                    await userService.UpdateAsync(id, userForCreation);

                    await userService.UploadImageAsync(id, pathOfPassportImage, portraitPath);

                    new SuccessWindow().ShowDialog();
                }

                else
                    new ErrorWindow().ShowDialog();
            }
            else
                new ErrorWindow().ShowDialog();

        }

        private void PassportBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pathOfPassportImage = ChooseFile();
            PassportImg.ImageSource = new BitmapImage(new Uri(pathOfPassportImage));

        }

        private string ChooseFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.PNG,*.JPG;)|*.JPG;*.PNG";
            openFileDialog.InitialDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.MyPictures);


            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        private void PortraitBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            portraitPath = ChooseFile();

            PortraitImg.ImageSource = new BitmapImage(new Uri(portraitPath));
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            long id;
            if (long.TryParse(IdTxt.Text, out id))
            {
                if (await userService.DeleteAsync(id))
                    new SuccessWindow().ShowDialog();
                else
                    new ErrorWindow().ShowDialog();
            }
            else
                new ErrorWindow().ShowDialog();
        }
    }
}
