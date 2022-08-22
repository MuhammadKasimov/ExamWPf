using ExamWithDesktop.Service.Interfaces;
using ExamWithDesktop.Service.Models;
using ExamWithDesktop.Service.Services;
using ExamWithDesktop.WPF.Windows;
using System.Threading;
using System.Windows.Controls;

namespace ExamWithDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
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
    }
}
