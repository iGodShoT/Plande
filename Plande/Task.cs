using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Plande
{
    public enum Priority
    {
        Высокий = 1,
        Средний,
        Низкий
    }

    public class Task : INotifyPropertyChanged
    {
        private string _name;

        private Priority _priority;

        private DateTime _deadline;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public Priority Priority
        {
            get { return _priority; }
            set { _priority = value; OnPropertyChanged("Priority"); }
        }

        public DateTime Deadline
        {
            get { return _deadline; }
            set { _deadline = value; OnPropertyChanged("Deadline"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
