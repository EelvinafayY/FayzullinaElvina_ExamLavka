using FayzullinaElvina_ExamLavka.DBConnection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FayzullinaElvina_ExamLavka.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public static List<Users> users {  get; set; }
        public static List<Workers> workers { get; set; }
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void AuthBT_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text.Trim();
            string password = PasswordPB.Password.Trim();


            users = new List<Users>(DBConnect.DB.Users.ToList());
            Users currentUser = users.FirstOrDefault(i => i.Login == login && i.Password == password);
            DBConnect.loggedUsers = currentUser;

            workers = new List<Workers>(DBConnect.DB.Workers.ToList());
            Workers currentWorker = workers.FirstOrDefault(i => i.Login == login && i.Password == password);
            DBConnect.loginedWorker = currentWorker;

            
            if (currentUser != null)
            {
                NavigationService.Navigate(new ServicesUsersMainPage(currentUser));
            }
            if (currentWorker != null)
            {
                NavigationService.Navigate(new ServicesMainPage(currentWorker));
            }
           
            if (currentUser == null && currentWorker == null)
            {
                MessageBox.Show("Такого пользователя не существует(((");
            }
        }

        private void RegistrBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
