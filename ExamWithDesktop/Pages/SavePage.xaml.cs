using ExamTask.Main.Services;
using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.WPF.Interfaces;
using ExamWithDesktop.WPF.Models;
using ExamWithDesktop.WPF.Windows;
using System;
using System.Windows.Controls;

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
        User user;
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

            if (long.TryParse(Id.Text, out id))
            {
                user = await userService.GetAsync(id);
                if (user != null)
                {
                    userForCreation.FirstName = String.IsNullOrEmpty(FirstNameTxt.Text) ? user.FirstName : FirstNameTxt.Text;

                    userForCreation.LastName = String.IsNullOrEmpty(LastNameTxt.Text) ? user.LastName : LastNameTxt.Text;

                    userForCreation.Faculty = String.IsNullOrEmpty(FacultyTxt.Text) ? user.Faculty : FacultyTxt.Text;

                    await userService.UpdateAsync(id, userForCreation);

                    await userService.UploadImageAsync(id, pathOfPassportImage, portraitPath);

                    new SuccessWindow().Show();
                }

                else
                    new ErrorWindow().Show();
            }
            else
                new ErrorWindow().Show();

        }

        private void PassportBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pathOfPassportImage = ChooseFile();
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
        }
    }
}
