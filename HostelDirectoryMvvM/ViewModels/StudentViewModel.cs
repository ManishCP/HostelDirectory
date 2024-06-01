using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using HostelDirectoryMvvM.Models;
namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        StudentService ObjStudentService;
        public StudentViewModel()
        {
            ObjStudentService = new StudentService();
            LoadData();
        }

        private List<Student> studentsList;

        public List<Student> StudentsList
        {
            get { return studentsList; }
            set { studentsList = value; OnPropertyChanged("StudentsList");}
        }

        private void LoadData() //Helper method to call the Getall method and puts it in the Student List
        {
            StudentsList = ObjStudentService.GetAll();
        }


    }
}
