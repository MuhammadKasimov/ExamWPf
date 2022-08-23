using ExamWithDesktop.Service.Interfaces;
using ExamWithDesktop.Service.Models;
using ExamWithDesktop.Service.Services;
using ExamWithDesktop.WPF.Windows;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ExamWithDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        private string pathOfPassportImage;
        private string portraitPath;

        private readonly IUserService userService;
        private readonly UserForCreation userForCreation;
        public AddPage()
        {
            userForCreation = new UserForCreation();
            userService = new UserService();
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            userForCreation.FirstName = FirstNameTxt.Text;
            userForCreation.LastName = LastNameTxt.Text;
            userForCreation.Faculty = FacultyTxt.Text;

            Thread thread = new Thread(async () =>
            {
                await userService.CreateAsync(userForCreation);
            });
            thread.Start();
            SuccessWindow successWindow = new SuccessWindow();
            successWindow.ShowDialog();

        }

        private void PassportBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pathOfPassportImage = ChooseFile();

            if (pathOfPassportImage != null)
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
            if (portraitPath != null)
                PortraitImg.ImageSource = new BitmapImage(new Uri(portraitPath));
        }
    }
}
