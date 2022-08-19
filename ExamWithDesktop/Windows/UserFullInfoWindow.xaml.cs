using System.Windows;

namespace ExamWithDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserFullInfoWindow.xaml
    /// </summary>
    public partial class UserFullInfoWindow : Window
    {
        public UserFullInfoWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
