using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Plande
{

    public partial class Registration : Window
    {
        string constr;
        string base64;
        string UserID;
        public Registration(string Constr)
        {
            InitializeComponent();
            constr = Constr;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;
            if (Name.Text.Trim().Length != 0 && Surname.Text.Trim().Length != 0 && Login.Text.Trim().Length != 0 && Password.Password.Trim().Length != 0)
            {
                if (base64 == null)
                {
                    Message("Загрузите фотографию");
                }
                else
                {
                    SqlConnection connection = new SqlConnection(constr);
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand($"INSERT INTO [Users] ([Login], [Password], [Name], [Surname]) values ('{Login.Text}', '{Password.Password}', '{Name.Text}', '{Surname.Text}')", connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Повторяющееся значение ключа"))
                        {
                            Message("Ваш логин не уникален, попробуйте заново");
                            Login.Text = "";
                            error = true;
                        }
                    }
                    finally
                    {
                        connection.Close();
                        if (!error)
                        {
                            GetUserID(Login.Text);
                            SqlConnection s = new SqlConnection(constr);
                            try
                            {
                                s.Open();
                                SqlCommand cmd = new SqlCommand($"Insert into [Photos] (PictureCode, UserID) values ('{base64}', '{UserID}')", s);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                s.Close();
                                this.Close();
                            }
                        }
                        
                    }
                }
                
            }
        }
        private void Message(string message)
        {
            var duration = 4;
            SnackbarOne.MessageQueue?.Enqueue(
            message,
            null,
            null,
            null,
            false,
            true,
            TimeSpan.FromSeconds(duration));
        }
        private void GetUserID(string Login)
        {
            SqlConnection sa = new SqlConnection(constr);
            string t = "";
            try
            {
                sa.Open();
                SqlCommand cmd = new SqlCommand($"select [ID] from Users where [Login] = '{Login}'", sa);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        t = Convert.ToString(reader.GetInt32(0));
                UserID = t;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sa.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoadPhoto_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (o.ShowDialog() == true)
            {
                Uri uri = new Uri(o.FileName);
                Photo.Source = new BitmapImage(uri);
                byte[] imageArray = File.ReadAllBytes(o.FileName);
                base64 = Convert.ToBase64String(imageArray);
            }
        }
    }
}