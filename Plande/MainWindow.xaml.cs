using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Plande
{
    public partial class MainWindow : Window
    {
        string constr;
        List<Task> ValueTasks;
        List<Task> SavingTasks;
        List<Task> UselessTasks;
        string Login;
        string UserID;
        int check;
        int c;

        public MainWindow(string UserLogin, string Constr)
        {
            InitializeComponent();
            constr = Constr;
            Login = UserLogin;
            GetUserID(UserLogin);
            ValueTasks = new List<Task>();
            SavingTasks = new List<Task>();
            UselessTasks = new List<Task>();
            c = 0;
            SqlConnection s = new SqlConnection(constr);
            try
            {
                s.Open();
                SqlCommand cmd = new SqlCommand($"SELECT [UserID] FROM [First] where [UserID] = '{UserID}'", s);
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Tutorial();
                }
                else
                {
                    Greeting();
                    GetUserName(UserLogin);
                    GetUserPhoto(UserID);
                    ValuableTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
                    NotValuableTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
                    SaveTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
                    GetValuableTasks();
                    GetSavingTasks();
                    GetUselessTasks();
                    Productivity.Value = 50;
                    check = 0;
                }
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

        private void Tutorial()
        {
            NotValuableTasksListView.Visibility = Visibility.Hidden;
            Productivity.Visibility = Visibility.Hidden;
            add1.Visibility = Visibility.Hidden;
            add2.Visibility = Visibility.Hidden;
            add3.Visibility = Visibility.Hidden;
            valuegrid.Visibility = Visibility.Hidden;
            nonvaluegrid.Visibility = Visibility.Hidden;
            savegrid.Visibility = Visibility.Hidden;
            StinsonPhoto.Visibility = Visibility.Visible;
            gonext.Visibility = Visibility.Visible;
            PlandeText.Text =
                "Привет! Я - Плэнди, твой лучший друг, и теперь я научу тебя жить. Ты ведь за этим пришел сюда, верно? Неважно, нажми на кнопку, чтобы продолжить";
            PlandeText.Visibility = Visibility.Visible;
        }

        private void gonext_Click(object sender, RoutedEventArgs e)
        {
            nonvaluegrid.Visibility = Visibility.Visible;
            savegrid.Visibility = Visibility.Visible;
            valuegrid.Visibility = Visibility.Visible;
            StinsonPhoto.Source = new BitmapImage(new Uri("/Wine.png", UriKind.Relative))
                {CreateOptions = BitmapCreateOptions.IgnoreImageCache};
            PlandeText.Text =
                "Снизу ты видишь панели с надписями. Каждая из них обозначает степень важности конкретной задачи для тебя";
            gonext.Content = "Понятно";
            gonext.Click -= gonext_Click;
            gonext.Click += Gonext_Click;
        }

        private void Gonext_Click(object sender, RoutedEventArgs e)
        {
            add1.Visibility = Visibility.Visible;
            StinsonPhoto.Source = new BitmapImage(new Uri("/Nice.png", UriKind.Relative))
                {CreateOptions = BitmapCreateOptions.IgnoreImageCache};
            ValuableTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            PlandeText.Text =
                "Теперь давай попробуем добавить новую задачу в список приносящих ценность. Нажми на плюсик и заполни поля, а потом нажми на Окей. Учти, что выбирать прошедшую дату нельзя";
            gonext.Content = "Окей";
            if (c <= 0) return;
            gonext.Click -= Gonext_Click;
            gonext.Click += Gonext_Click1;
            Gonext_Click1(sender, e);
        }

        private void Gonext_Click1(object sender, RoutedEventArgs e)
        {
            StinsonPhoto.Source = new BitmapImage(new Uri("/Cool.png", UriKind.Relative))
                {CreateOptions = BitmapCreateOptions.IgnoreImageCache};
            PlandeText.Text =
                "Ты отлично справляешься! Кстати, при добавлении задачи в список, твой показатель продуктивности будет увеличиваться";
            Productivity.Visibility = Visibility.Visible;
            gonext.Content = "Ого";
            gonext.Click -= Gonext_Click1;
            gonext.Click += Gonext_Click2;
        }

        private void Gonext_Click2(object sender, RoutedEventArgs e)
        {
            StinsonPhoto.Source = new BitmapImage(new Uri("/HighFive.png", UriKind.Relative))
                {CreateOptions = BitmapCreateOptions.IgnoreImageCache};
            PlandeText.Text =
                "Пожалуй, на этом все. Рад был познакомиться и отличного тебе дня. Кстати, при каждом заходе сюда ты будешь видеть мои лучшие мотивационные фразы, которые будут поддерживать тебя. До встречи!";
            gonext.Content = "Увидимся";
            gonext.Click -= Gonext_Click2;
            gonext.Click += Gonext_Click3;
        }

        private void Gonext_Click3(object sender, RoutedEventArgs e)
        {
            add2.Visibility = Visibility.Visible;
            add3.Visibility = Visibility.Visible;
            StinsonPhoto.Visibility = Visibility.Hidden;
            PlandeText.Visibility = Visibility.Hidden;
            NotValuableTasksListView.Visibility = Visibility.Visible;
            gonext.Visibility = Visibility.Hidden;
            Greeting();
            GetUserName(Login);
            GetUserPhoto(UserID);
            ValuableTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            NotValuableTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            SaveTasksPriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            Productivity.Value = 50;
            check = 0;
            SqlConnection connection = new SqlConnection(constr);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO [First] ([UserID]) values ('{UserID}')", connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
                gonext.Click -= Gonext_Click3;
            }
        }

        private void Greeting()
        {
            SqlConnection s = new SqlConnection(constr);
            string text = "";
            try
            {
                s.Open();
                SqlCommand cmd = new SqlCommand($"SELECT TOP 1 [Text] FROM [MotivationalWords] order by NEWID()", s);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        text = reader.GetString(0);
                MotivationalWords.Text = text;
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

        private void GetValuableTasks()
        {
            SqlConnection s = new SqlConnection(constr);
            try
            {
                Priority p = Priority.Высокий;
                s.Open();
                SqlCommand cmd =
                    new SqlCommand(
                        $"select Tasks.[Name], [Priorities].[Name] as [Priority], [Deadline] from [Tasks] inner join [Priorities] on [Priorities].ID = Tasks.PriorityID where [Valuable] = 1 and [UserID] = '{UserID}'",
                        s);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "Высокий")
                            p = Priority.Высокий;
                        if (reader.GetString(1) == "Средний")
                            p = Priority.Средний;
                        if (reader.GetString(1) == "Низкий")
                            p = Priority.Низкий;
                        Task task = new Task
                            {Name = reader.GetString(0), Priority = p, Deadline = reader.GetDateTime(2)};
                        ValueTasks.Add(task);
                        ValuableTasksListView.ItemsSource = new List<Task>();
                        ValuableTasksListView.ItemsSource = ValueTasks.OrderBy(x => x.Priority).ThenBy(x => x.Deadline);
                        ValuableTasksListView.Items.Refresh();
                    }
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

        private void GetSavingTasks()
        {
            SqlConnection s = new SqlConnection(constr);
            try
            {
                Priority p = Priority.Высокий;
                s.Open();
                SqlCommand cmd =
                    new SqlCommand(
                        $"select Tasks.[Name], [Priorities].[Name] as [Priority], [Deadline] from [Tasks] inner join [Priorities] on [Priorities].ID = Tasks.PriorityID where [Saving] = 1 and [UserID] = '{UserID}'",
                        s);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "Высокий")
                            p = Priority.Высокий;
                        if (reader.GetString(1) == "Средний")
                            p = Priority.Средний;
                        if (reader.GetString(1) == "Низкий")
                            p = Priority.Низкий;
                        Task task = new Task
                            {Name = reader.GetString(0), Priority = p, Deadline = reader.GetDateTime(2)};
                        SavingTasks.Add(task);
                        SaveTasksListView.ItemsSource = new List<Task>();
                        SaveTasksListView.ItemsSource = SavingTasks.OrderBy(x => x.Priority).ThenBy(x => x.Deadline);
                        SaveTasksListView.Items.Refresh();
                    }
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

        private void GetUselessTasks()
        {
            SqlConnection s = new SqlConnection(constr);
            try
            {
                Priority p = Priority.Высокий;
                s.Open();
                SqlCommand cmd =
                    new SqlCommand(
                        $"select Tasks.[Name], [Priorities].[Name] as [Priority], [Deadline] from [Tasks] inner join [Priorities] on [Priorities].ID = Tasks.PriorityID where [Useless] = 1 and [UserID] = '{UserID}'",
                        s);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "Высокий")
                            p = Priority.Высокий;
                        if (reader.GetString(1) == "Средний")
                            p = Priority.Средний;
                        if (reader.GetString(1) == "Низкий")
                            p = Priority.Низкий;
                        Task task = new Task
                            {Name = reader.GetString(0), Priority = p, Deadline = reader.GetDateTime(2)};
                        UselessTasks.Add(task);
                        NotValuableTasksListView.ItemsSource = new List<Task>();
                        NotValuableTasksListView.ItemsSource =
                            UselessTasks.OrderBy(x => x.Priority).ThenBy(x => x.Deadline);
                        NotValuableTasksListView.Items.Refresh();
                    }
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

        private void GetUserName(string Login)
        {
            SqlConnection sa = new SqlConnection(constr);
            string t = "";
            try
            {
                sa.Open();
                SqlCommand cmd =
                    new SqlCommand(
                        $"select CONCAT([Name],' ', LEFT([Surname],1),'.') from Users where [Login] = '{Login}'", sa);
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
                string PhotoCode = t;
                if (PhotoCode != null)
                {
                    byte[] bytes = Convert.FromBase64String(PhotoCode);
                    BitmapImage b = new BitmapImage();
                    MemoryStream ms = new MemoryStream(bytes);
                    b.BeginInit();
                    b.StreamSource = ms;
                    b.EndInit();
                    UserAvatar.Source = b;
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

        private void Message(string message)
        {
            var duration = 5;
            SnackbarOne.MessageQueue?.Enqueue(
                message,
                null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(duration));
        }

        private void UserName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Account a = new Account(Login, constr);
            a.ShowDialog();
        }

        private void Productivity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Productivity.Value >= 100)
                Message("Вы сегодня очень продуктивны, так держать!");
        }

        private void TaskDoneConfirmation(ListView lv, List<Task> list)
        {
            if (lv.SelectedIndex >= 0)
            {
                var item = list[lv.SelectedIndex];
                list.RemoveAt(lv.SelectedIndex);
                SqlConnection connection = new SqlConnection(constr);
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand($"Delete from [Tasks] where [Name] = '{item.Name}'", connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                lv.ItemsSource = new List<Task>();
                lv.ItemsSource = list;
                lv.Items.Refresh();
            }

            lv.SelectedIndex = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (check)
            {
                case 1:
                    TaskDoneConfirmation(ValuableTasksListView, ValueTasks);
                    break;
                case 2:
                    TaskDoneConfirmation(SaveTasksListView, SavingTasks);
                    break;
                case 3:
                    TaskDoneConfirmation(NotValuableTasksListView, UselessTasks);
                    break;
            }
        }

        private void ValuableTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ValuableTasksListView.SelectedIndex >= 0)
            {
                TaskIsDone.ShowDialog(TaskIsDone.DialogContent);
                check = 1;
            }
        }

        private void SaveTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SaveTasksListView.SelectedIndex >= 0)
            {
                TaskIsDone.ShowDialog(TaskIsDone.DialogContent);
                check = 2;
            }
        }

        private void NotValuableTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotValuableTasksListView.SelectedIndex >= 0)
            {
                TaskIsDone.ShowDialog(TaskIsDone.DialogContent);
                check = 3;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ValuableTasksNameTextBox.Text.Trim().Length != 0 &&
                ValuableTasksPriorityComboBox.SelectedItem != null && ValuableTasksDatePicker.SelectedDate != null)
            {
                var now = DateTime.Now;
                var hour = DateTime.Now.Hour;
                var minute = DateTime.Now.Minute;
                var second = DateTime.Now.Second + 1;
                TimeSpan t = new TimeSpan(hour, minute, second);
                var a = now - t;
                if (ValuableTasksDatePicker.SelectedDate >= a)
                {
                    Priority chosen = (Priority) Enum.Parse(typeof(Priority),
                        ValuableTasksPriorityComboBox.SelectedValue.ToString());
                    int index = (int) chosen;
                    Task task = new Task
                    {
                        Name = ValuableTasksNameTextBox.Text, Deadline = ValuableTasksDatePicker.SelectedDate.Value,
                        Priority = chosen
                    };
                    ValueTasks.Add(task);
                    ValuableTasksListView.ItemsSource = new List<Task>();
                    ValuableTasksListView.ItemsSource = ValueTasks.OrderBy(x => x.Priority).ThenBy(x => x.Deadline);
                    ValuableTasksListView.Items.Refresh();
                    Message("Задача успешно добавлена в список");
                    Productivity.Value += 20;
                    c += 1;
                    SqlConnection connection = new SqlConnection(constr);
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(
                            $"INSERT INTO [Tasks] ([Name], [Deadline], [Saving], [Valuable], [Useless], [IsCool], [PriorityID], [UserID]) values ('{task.Name}', '{task.Deadline.Date}', 0, 1, 0, 0, '{index}', '{UserID}')",
                            connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    Message("Некорректная дата");
                }
            }
            else
            {
                Message("Не все поля заполнены");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SaveTasksNameTextBox.Text.Trim().Length != 0 && SaveTasksPriorityComboBox.SelectedItem != null &&
                SaveTasksDatePicker.SelectedDate != null)
            {
                var now = DateTime.Now;
                var hour = DateTime.Now.Hour;
                var minute = DateTime.Now.Minute;
                var second = DateTime.Now.Second + 1;
                TimeSpan t = new TimeSpan(hour, minute, second);
                var a = now - t;
                if (SaveTasksDatePicker.SelectedDate >= a)
                {
                    Priority chosen = (Priority) Enum.Parse(typeof(Priority),
                        SaveTasksPriorityComboBox.SelectedValue.ToString());
                    int index = (int) chosen;
                    Task task = new Task
                    {
                        Name = SaveTasksNameTextBox.Text, Deadline = SaveTasksDatePicker.SelectedDate.Value,
                        Priority = chosen
                    };
                    SavingTasks.Add(task);
                    SaveTasksListView.ItemsSource = new List<Task>();
                    SaveTasksListView.ItemsSource = SavingTasks.OrderBy(x => x.Priority).ThenBy(x => x.Deadline);
                    SaveTasksListView.Items.Refresh();
                    Message("Задача успешно добавлена в список");
                    Productivity.Value += 10;
                    SqlConnection connection = new SqlConnection(constr);
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(
                            $"INSERT INTO [Tasks] ([Name], [Deadline], [Saving], [Valuable], [Useless], [IsCool], [PriorityID], [UserID]) values ('{task.Name}', '{task.Deadline}', 1, 0, 0, 0, '{index}', '{UserID}')",
                            connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                    Message("Некорректная дата");
            }
            else
                Message("Не все поля заполнены");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (NotValuableTasksNameTextBox.Text.Trim().Length != 0 &&
                NotValuableTasksPriorityComboBox.SelectedItem != null &&
                NotValuableTasksDatePicker.SelectedDate != null)
            {
                var now = DateTime.Now;
                var hour = DateTime.Now.Hour;
                var minute = DateTime.Now.Minute;
                var second = DateTime.Now.Second + 1;
                TimeSpan t = new TimeSpan(hour, minute, second);
                var a = now - t;
                if (NotValuableTasksDatePicker.SelectedDate >= a)
                {
                    Priority chosen = (Priority) Enum.Parse(typeof(Priority),
                        NotValuableTasksPriorityComboBox.SelectedValue.ToString());
                    int index = (int) chosen;
                    Task task = new Task
                    {
                        Name = NotValuableTasksNameTextBox.Text,
                        Deadline = NotValuableTasksDatePicker.SelectedDate.Value,
                        Priority = chosen
                    };
                    UselessTasks.Add(task);
                    NotValuableTasksListView.ItemsSource = new List<Task>();
                    NotValuableTasksListView.ItemsSource =
                        UselessTasks.OrderBy(x => x.Priority).ThenBy(x => x.Deadline);
                    NotValuableTasksListView.Items.Refresh();
                    Message("Задача успешно добавлена в список");
                    Productivity.Value += 7;
                    SqlConnection connection = new SqlConnection(constr);
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(
                            $"INSERT INTO [Tasks] ([Name], [Deadline], [Saving], [Valuable], [Useless], [IsCool], [PriorityID], [UserID]) values ('{task.Name}', '{task.Deadline}', 0, 0, 1, 0, '{index}', '{UserID}')",
                            connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                    Message("Некорректная дата");
            }
            else
                Message("Не все поля заполнены");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Button_Click_5(object sender, RoutedEventArgs e) => this.WindowState =
            this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

        private void Button_Click_6(object sender, RoutedEventArgs e) => this.Close();
        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}