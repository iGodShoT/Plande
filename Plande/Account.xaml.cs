using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Plande
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        string base64;
        string constr;
        string UserLogin;
        string UserID;
        string PhotoCode;
        public Account(string Login, string Constr)
        {
            InitializeComponent();
            constr = Constr;
            UserLogin = Login;
            GetUserID(Login);
            GetUserName(Login);
            GetUserPhoto(UserID);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (o.ShowDialog() == true)
            {
                Uri uri = new Uri(o.FileName);
                Photo.Source = new BitmapImage(uri);
                byte[] imageArray = File.ReadAllBytes(o.FileName);
                base64 = Convert.ToBase64String(imageArray);
                SqlConnection s = new SqlConnection(constr);
                if (PhotoCode != null)
                {
                    try
                    {
                        s.Open();
                        SqlCommand cmd = new SqlCommand($"Update [Photos] set PictureCode = '{base64}' where UserID = {UserID}", s);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        s.Close();
                    }
                }
                else
                {
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
                    }
                }
                
            }
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
        private void GetUserPhoto(string ID)
        {
            SqlConnection sa = new SqlConnection(constr);
            string t = "";
            try
            {
                sa.Open();
                SqlCommand cmd = new SqlCommand($"select [PictureCode] from Photos where [UserID] = '{ID}'", sa);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        t = reader.GetString(0);
                PhotoCode = t;
                if (PhotoCode != null)
                {
                    byte[] bytes = Convert.FromBase64String(PhotoCode);
                    BitmapImage b = new BitmapImage();
                    MemoryStream ms = new MemoryStream(bytes);
                    b.BeginInit();
                    b.StreamSource = ms;
                    b.EndInit();
                    Photo.Source = b;
                }
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
        private void GetUserName(string Login)
        {
            SqlConnection sa = new SqlConnection(constr);
            string t = "";
            try
            {
                sa.Open();
                SqlCommand cmd = new SqlCommand($"select CONCAT([Name],' ', [Surname]) from Users where [Login] = '{Login}'", sa);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        t = reader.GetString(0);
                UserName.Text = t;
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
    }
}
