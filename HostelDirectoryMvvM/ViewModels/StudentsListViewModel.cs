using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentsListViewModel : ViewModelBase
    {
        private string _filter;
        public ObservableCollection<Student> Students { get; set; }
        public ICommand RemoveStudentCommand { get; }

        public string Filter
        {
            get { return _filter; }
            set { SetField(ref _filter, value, nameof(Filter)); }
        }

        public StudentsListViewModel()
        {
            Students = new ObservableCollection<Student>
            {
                new Student { Name = "John Doe", RoomNumber = 101, Age = 20 },
                new Student { Name = "Jane Smith", RoomNumber = 102, Age = 21 },
                new Student { Name = "Samuel Green", RoomNumber = 103, Age = 22 },
                new Student { Name = "John Doe", RoomNumber = 101, Age = 20 },
                new Student { Name = "Jane Smith", RoomNumber = 102, Age = 21 },
                new Student { Name = "Samuel Green", RoomNumber = 103, Age = 22 },
                new Student { Name = "John Doe", RoomNumber = 101, Age = 20 },
                new Student { Name = "Jane Smith", RoomNumber = 102, Age = 21 },
                new Student { Name = "Samuel Green", RoomNumber = 103, Age = 22 }
            };

            RemoveStudentCommand = new RelayCommand(RemoveStudent, CanRemoveStudent);
        }

        private void RemoveStudent(object student)
        {
            if (student is Student studentToRemove)
            {
                Students.Remove(studentToRemove);
            }
        }

        private bool CanRemoveStudent(object student)
        {
            return true; // Implement logic to determine if student can be removed
        }
    }
}
