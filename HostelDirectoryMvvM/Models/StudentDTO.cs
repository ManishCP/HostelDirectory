using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel;
namespace HostelDirectoryMvvM.Models
{
    public class StudentDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private string studentID;
        public string StudentID
        {
            get { return studentID; }
            set { studentID = value; OnPropertyChanged("Id"); }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }


        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; OnPropertyChanged("Age");}
        }


        private int roomNumber;
        public int RoomNumber
        {
            get { return roomNumber; }
            set { roomNumber = value; OnPropertyChanged("RoomNumber");}
        }
    }
}
