using System.Data.SqlClient;
using System.Windows;

namespace Plande
{
    public partial class Authorization : Window
    {
        string constr = @"Data Source=95.165.158.109\GODSHOT;Initial Catalog=Plande;Persist Security Info=True;User ID=Test;Password=123";
        public Authorization()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text.Trim().Length != 0)
            {
                using (var connection = new SqlConnection(constr))
                {
                    connection.Open();
                    var command = new SqlCommand("Select * From Users WHERE Login = @login AND Password = @password",
                        connection);
                    command.Parameters.AddWithValue("@login", Login.Text);
                    command.Parameters.AddWithValue("@password", Password.Password);
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            connection.Close();
                            MainWindow m = new MainWindow(Login.Text, constr);
                            m.Show();
                            this.Close();
                        }
                    }
                }
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Registration r = new Registration(constr);
            r.ShowDialog();
        }
    }
}