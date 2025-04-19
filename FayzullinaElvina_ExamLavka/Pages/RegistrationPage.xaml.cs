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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public static List<Users> users {  get; set; }
        public static List<Workers> workers { get; set; }
        public static Users newUser = new Users();
        public RegistrationPage()
        {
            InitializeComponent();
            users = new List<Users>(DBConnect.DB.Users.ToList());
            workers =  new List<Workers>(DBConnect.DB.Workers.ToList());
            this.DataContext = this;
        }

        private void RegistrBT_Click(object sender, RoutedEventArgs e)
        {
           
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(FullNameTB.Text) || string.IsNullOrWhiteSpace(LoginTB.Text) || string.IsNullOrWhiteSpace(PasswordPB.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            var allUsersLogin = users.FirstOrDefault(x => x.Login ==  LoginTB.Text);
            var allWorkersLogin = workers.FirstOrDefault(x => x.Login == LoginTB.Text);
            if (allUsersLogin != null || allWorkersLogin != null)
            {
                MessageBox.Show("Логин уже занят другим пользователем!");
                return;
            }

            if (LoginTB.Text.Length != 3)
            {
                MessageBox.Show("Неккореткный логин (длина должна быть 3)!");
                return;
            }

            if (PasswordPB.Password.Length != 5)
            {
                MessageBox.Show("Неккоректный пароль (длина должна быть 5)!");
                return;
            }

            newUser.FullName = FullNameTB.Text.Trim();
            newUser.Login = LoginTB.Text.Trim();
            newUser.Password = PasswordPB.Password.Trim();
            DBConnect.DB.Users.Add(newUser);
            DBConnect.DB.SaveChanges();
            NavigationService.Navigate(new ServicesUsersMainPage(newUser));
           
               
        }

        private void ExitBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
