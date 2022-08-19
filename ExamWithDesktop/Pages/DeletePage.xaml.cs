using ExamTask.Main.Services;
using ExamWithDesktop.WPF.Interfaces;
using ExamWithDesktop.WPF.Windows;
using System.Windows.Controls;

namespace ExamWithDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DeletePage.xaml
    /// </summary>
    public partial class DeletePage : Page
    {
        IUserService userService;

        public DeletePage()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            long id;
            if (long.TryParse(IdTxtBox.Text, out id))
            {
                if (await userService.DeleteAsync(id))
                    new SuccessWindow().Show();
                else
                    new ErrorWindow().Show();
            }
            else
                new ErrorWindow().Show();
        }
    }
}
