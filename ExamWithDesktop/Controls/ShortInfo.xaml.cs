using ExamWithDesktop.Service.Interfaces;
using ExamWithDesktop.Service.Services;
using System.Windows.Controls;

namespace ExamWithDesktop.Controls
{
    /// <summary>
    /// Логика взаимодействия для ShortInfo.xaml
    /// </summary>
    public partial class ShortInfo : UserControl
    {
        IUserService userService;
        public long Id { get; set; }
        public ShortInfo()
        {
            userService = new UserService();
            InitializeComponent();
        }
    }
}