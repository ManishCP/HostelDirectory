using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using HostelDirectoryMvvM.Models;
using HostelDirectoryMvvM.Commands;
using System.Collections.ObjectModel;
namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged_Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
               PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        StudentService ObjStudentService;
        public StudentViewModel()
        {
            ObjStudentService = new StudentService();
            LoadData();
            CurrentStudent = new Student();
            saveCommand = new RelayCommand(Save);
            searchCommand = new RelayCommand(Search);
            updateCommand = new RelayCommand(Update);
        }

        #region DisplayOperation

        private ObservableCollection<Student> studentsList;

        public ObservableCollection<Student> StudentsList
        {
            get { return studentsList; }
            set { studentsList = value; OnPropertyChanged("StudentsList");}
        }

        private void LoadData() //Helper method to call the Getall method and puts it in the Student List
        {
            StudentsList = new ObservableCollection<Student>(ObjStudentService.GetAll());  //Previously implemented normal List so we were not able to observe the changes
        }

        #endregion

        private Student currentStudent;

        public Student CurrentStudent
        {
            get { return currentStudent; }
            set { currentStudent = value; OnPropertyChanged("CurrentStudent"); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }


        #region SaveOperation
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; } //Since it's not two data binding and only one way we do not need setters
        }

        public void Save()
        {
            try
            {
                var IsSaved = ObjStudentService.Add(CurrentStudent);
                LoadData();
                if (IsSaved)
                    Message = "Student saved";
                else
                    Message = "Save Operation Failed!";
            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }

        }
        #endregion

        #region SearchOperation

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }
        public void Search()
        {
            try
            {
                var ObjStudent = ObjStudentService.Search(CurrentStudent.Id);
                if (ObjStudent != null)
                {
                    CurrentStudent.Name = ObjStudent.Name;
                    CurrentStudent.Age = ObjStudent.Age;
                    CurrentStudent.RoomNumber = ObjStudent.RoomNumber;
                }
                else
                {
                    Message = "Student Not Found";
                }

            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
        }
        #endregion

        #region UpdateOperation
        private RelayCommand updateCommand;

        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
        }
        public void Update()
        {
            try
            {
                var IsUpdated = ObjStudentService.Update(CurrentStudent);
                if (IsUpdated)
                {
                    Message = "Student Info Updated";
                    LoadData();
                }
                else
                {
                    Message = "Could not Update";
                }

            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
        }
        #endregion


    }

}
