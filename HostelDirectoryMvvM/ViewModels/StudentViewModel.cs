using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        StudentService ObjStudentService;

        public StudentViewModel()
        {
            ObjStudentService = new StudentService();
            LoadData();
            CurrentStudent = new StudentDTO(); // Initialize with empty values
            saveCommand = new RelayCommand(Save);
            searchCommand = new RelayCommand(Search);
            updateCommand = new RelayCommand(Update);
            deleteCommand = new RelayCommand(SendDeleteMessage);
            filterTextChangedCommand = new RelayCommand(FilterStudentsTextChanged);

            Messenger.Subscribe<DeleteMessage>(HandleDeleteMessage);
        }

        private ObservableCollection<StudentDTO> studentsList;
        public ObservableCollection<StudentDTO> StudentsList
        {
            get { return studentsList; }
            set { studentsList = value; OnPropertyChanged(nameof(StudentsList)); }
        }

        private void LoadData()
        {
            var students = ObjStudentService.GetAll();
            StudentsList = new ObservableCollection<StudentDTO>(students);
            FilteredStudents = new ListCollectionView(StudentsList);
            OnPropertyChanged(nameof(StudentsList));
            OnPropertyChanged(nameof(FilteredStudents));
        }

        private StudentDTO currentStudent;
        public StudentDTO CurrentStudent
        {
            get { return currentStudent; }
            set { currentStudent = value; OnPropertyChanged(nameof(CurrentStudent)); }
        }

        public void ClearCurrentStudent()
        {
            CurrentStudent = new StudentDTO(); ;
            Message = "Student deselected";
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.IsSelected)
            {
                item.IsSelected = false;
                CurrentStudent = null;
                Message = "Student deselected";
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        private string filter;
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged(nameof(Filter));
                FilterStudents();
            }
        }

        public ListCollectionView FilteredStudents { get; set; }

        private RelayCommand filterTextChangedCommand;
        public RelayCommand FilterTextChangedCommand
        {
            get { return filterTextChangedCommand; }
        }

        private void FilterStudentsTextChanged()
        {
            FilterStudents();
            FilteredStudents.Refresh();
        }

        private void FilterStudents()
        {
            if (FilteredStudents == null) return;

            if (string.IsNullOrWhiteSpace(Filter))
            {
                FilteredStudents.Filter = null; // Clear the filter
            }
            else
            {
                FilteredStudents.Filter = student =>
                {
                    var s = student as StudentDTO;
                    return s != null && s.Name.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }
            FilteredStudents.Refresh(); // Refresh to apply the filter
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }

        public void Save()
        {
            try
            {
                if (CurrentStudent == null || string.IsNullOrEmpty(CurrentStudent.Name) || CurrentStudent.Age <= 0 || CurrentStudent.RoomNumber <= 0)
                {
                    Message = "Missing or invalid student information. Please check all fields.";
                    return;
                }

                var IsSaved = ObjStudentService.Add(CurrentStudent);
                if (IsSaved)
                {
                    StudentsList.Add(CurrentStudent); // Directly add the new student to the collection
                    Message = "Student saved";
                    CurrentStudent = new StudentDTO(); // Clear the current student after saving
                    OnPropertyChanged(nameof(StudentsList)); // Notify the UI of the update
                }
                else
                {
                    Message = "Save Operation Failed!";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }

        public void Search()
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentStudent.Name))
                {
                    Message = "Please enter a name to search.";
                    return;
                }

                var searchResults = ObjStudentService.Search(CurrentStudent.Name);
                if (searchResults != null && searchResults.Any())
                {
                    StudentsList = new ObservableCollection<StudentDTO>(searchResults);
                    FilteredStudents = new ListCollectionView(StudentsList);
                    OnPropertyChanged(nameof(StudentsList));
                    OnPropertyChanged(nameof(FilteredStudents));
                }
                else
                {
                    Message = "Student Not Found";
                    StudentsList.Clear();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
        }

        public void Update()
        {
            try
            {
                if (CurrentStudent == null || string.IsNullOrEmpty(CurrentStudent.Name) || CurrentStudent.Age <= 0 || CurrentStudent.RoomNumber <= 0)
                {
                    Message = "Missing or invalid student information. Please check all fields.";
                    return;
                }

                var IsUpdated = ObjStudentService.Update(CurrentStudent);
                if (IsUpdated)
                {
                    var student = StudentsList.FirstOrDefault(s => s.StudentID == CurrentStudent.StudentID);
                    if (student != null)
                    {
                        student.Name = CurrentStudent.Name;
                        student.Age = CurrentStudent.Age;
                        student.RoomNumber = CurrentStudent.RoomNumber;
                        OnPropertyChanged(nameof(StudentsList)); // Notify the UI of the update
                        Message = "Student updated";
                    }
                    else
                    {
                        Message = "Student not found in the list.";
                    }
                }
                else
                {
                    Message = "Update Operation Failed!";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
        }

        public void SendDeleteMessage()
        {
            Messenger.Publish(new DeleteMessage(CurrentStudent.StudentID));
        }

        private void HandleDeleteMessage(DeleteMessage message)
        {
            Delete(message.StudentID);
        }

        public void Delete(string studentId)
        {
            try
            {
                if (string.IsNullOrEmpty(studentId))
                {
                    Message = "Please select a student to delete.";
                    return;
                }

                var IsDeleted = ObjStudentService.Delete(studentId);
                if (IsDeleted)
                {
                    var student = StudentsList.FirstOrDefault(s => s.StudentID == studentId);
                    if (student != null)
                    {
                        StudentsList.Remove(student);
                        Message = "Student deleted";
                    }
                    else
                    {
                        Message = "Student not found in the list.";
                    }
                }
                else
                {
                    Message = "Delete Operation Failed!";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}
