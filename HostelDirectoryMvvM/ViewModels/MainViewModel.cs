using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace HostelDirectoryMvvM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly StudentService _studentService;
        private StudentViewModel _currentStudent;
        private string _filter;
        private ObservableCollection<StudentViewModel> _studentsList;
        private ListCollectionView _filteredStudents;

        public ObservableCollection<StudentViewModel> StudentsList
        {
            get { return _studentsList; }
            private set
            {
                _studentsList = value;
                OnPropertyChanged(nameof(StudentsList));
            }
        }

        public ListCollectionView FilteredStudents
        {
            get { return _filteredStudents; }
            private set
            {
                _filteredStudents = value;
                OnPropertyChanged(nameof(FilteredStudents));
            }
        }

        public StudentViewModel CurrentStudent
        {
            get { return _currentStudent; }
            set
            {
                _currentStudent = value;
                OnPropertyChanged(nameof(CurrentStudent));
                if (_currentStudent != null)
                {
                    IsStudentIdReadOnly = _currentStudent.StudentID != null;
                    if (IsStudentIdReadOnly)
                    {
                        Message = "Student Selected";
                    }
                }
            }
        }

        public bool IsStudentIdReadOnly { get; set; }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
                ClearCurrentStudent();
                FilterStudents();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand ListBoxItemPreviewMouseDownCommand { get; }

        public MainViewModel()
        {
            _studentService = new StudentService();
            LoadData();
            CurrentStudent = new StudentViewModel(new StudentDTO());
            IsStudentIdReadOnly = false;

            SaveCommand = CreateCommand(Save);
            UpdateCommand = CreateCommand(Update);
            ClearCommand = CreateCommand(Clear);
            ListBoxItemPreviewMouseDownCommand = CreateCommand(DeselectOrReselectCurrentStudent);

            Messenger.Instance.Subscribe<DeleteMessage>(HandleDeleteMessage);
        }

        private void LoadData()
        {
            var students = _studentService.GetAll().Select(s => new StudentViewModel(s));
            StudentsList = new ObservableCollection<StudentViewModel>(students);
            FilteredStudents = new ListCollectionView(StudentsList);
            OnPropertyChanged(nameof(StudentsList));
            OnPropertyChanged(nameof(FilteredStudents));
        }

        private void DeselectOrReselectCurrentStudent(object parameter)
        {
            if (parameter is StudentViewModel student)
            {
                if (CurrentStudent != null && CurrentStudent.StudentID == student.StudentID)
                {
                    ClearCurrentStudent();
                }
                else
                {
                    // Update the IsStudentIdReadOnly property of the selected student
                    student.IsStudentIdReadOnly = true;
                    CurrentStudent = student;
                }
            }
        }

        private void FilterStudents()
        {
            if (string.IsNullOrWhiteSpace(Filter))
            {
                FilteredStudents.Filter = null;
            }
            else
            {
                FilteredStudents.Filter = student =>
                {
                    var s = student as StudentViewModel;
                    return s != null && s.Name.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }
        }

        public void Save()
        {
            try
            {
                if (CurrentStudent == null || string.IsNullOrEmpty(CurrentStudent.Name) || CurrentStudent.Age <= 0 || CurrentStudent.RoomNumber <= 0 || string.IsNullOrEmpty(CurrentStudent.StudentID))
                {
                    Message = "Missing or invalid student information. Please check all fields.";
                    return;
                }

                var isSaved = _studentService.Add(CurrentStudent.Student);
                if (isSaved)
                {
                    CurrentStudent.Student.IsDeletable = !_studentService.IsPredefinedStudent(CurrentStudent.StudentID);
                    StudentsList.Add(CurrentStudent);
                    OnPropertyChanged(nameof(StudentsList));
                    ClearCurrentStudent();
                    Message = "Student saved";
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


        public void Update()
        {
            if (CurrentStudent == null || string.IsNullOrEmpty(CurrentStudent.Name) || CurrentStudent.Age <= 0 || CurrentStudent.RoomNumber <= 0)
            {
                Message = "Missing or invalid student information. Please check all fields.";
                return;
            }
            else if (CurrentStudent.Age < 21 || CurrentStudent.Age > 25)
            {
                Message = "Age limit for student is 21-25";
                return;
            }
            else
            {
                var isUpdated = _studentService.Update(CurrentStudent.Student);

                if (isUpdated)
                {
                    var student = StudentsList.FirstOrDefault(s => s.StudentID == CurrentStudent.StudentID);
                    if (student != null)
                    {
                        // Update the student only if the update operation is successful
                        student.Student.Name = CurrentStudent.Name;
                        student.Student.Age = CurrentStudent.Age;
                        student.Student.RoomNumber = CurrentStudent.RoomNumber;
                        OnPropertyChanged(nameof(StudentsList));
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
        }


        private void HandleDeleteMessage(DeleteMessage message)
        {
            var studentToRemove = StudentsList.FirstOrDefault(s => s.StudentID == message.StudentID);
            if (studentToRemove != null)
            {
                StudentsList.Remove(studentToRemove);
                _studentService.Delete(studentToRemove.StudentID); // Ensure the student is deleted from the database
                OnPropertyChanged(nameof(StudentsList));
                ClearCurrentStudent();
                Message = "Student deleted";
            }
            else
            {
                Message = "Student not found in the list.";
            }
        }

        public void ClearCurrentStudent()
        {
            CurrentStudent = new StudentViewModel(new StudentDTO());
            Message = "Student deselected";
        }

        public void Clear()
        {
            CurrentStudent = new StudentViewModel(new StudentDTO());
            Message = "";
        }
    }
}