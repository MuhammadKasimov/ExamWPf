using ExamTask.Main.Services;
using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.WPF.Interfaces;
using ExamWithDesktop.WPF.Models;
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
        readonly IUserService userService;
        User user;
        UserForCreation userForCreation;
        public AddPage()
        {
            userForCreation = new UserForCreation();
            userService = new UserService();
            user = new User();
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
            new SuccessWindow().Show();
        }
    }
}
