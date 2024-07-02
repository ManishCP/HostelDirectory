using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;

namespace HostelDirectoryMvvM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly StudentService _studentService;
        private StudentViewModel _currentStudent;
        private StudentViewModel _temporaryStudent;
        private string _filter;
        private ObservableCollection<StudentViewModel> _studentsList;
        private ListCollectionView _filteredStudents;
        private string _message;
        private bool _isListBoxEnabled;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
                if (CurrentStudent != null)
                {
                    CurrentStudent.Message = value;
                }
            }
        }

        public StudentViewModel TemporaryStudent
        { 
            get { return _temporaryStudent; }
            set { }
        }

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

        public bool IsListBoxEnabled
        {
            get { return _isListBoxEnabled; }
            set
            {
                _isListBoxEnabled = value;
                OnPropertyChanged(nameof(IsListBoxEnabled));
            }
        }

        public StudentViewModel CurrentStudent
        {
            get { return _currentStudent; }
            set
            {
                if (_currentStudent != value)
                {
                    _currentStudent = value;
                    OnPropertyChanged(nameof(CurrentStudent));

                    // Initialize or update the temporary student
                    if (_currentStudent != null)
                    {
                        _temporaryStudent = new StudentViewModel(new StudentDTO() { StudentID = _currentStudent.StudentID });
                        _temporaryStudent.CopyFrom(_currentStudent);
                    }
                    else
                    {
                        _temporaryStudent = null;
                    }

                    IsStudentIdReadOnly = _currentStudent != null && !string.IsNullOrEmpty(_currentStudent.StudentID);
                    Message = "Student Selected";

                    OnPropertyChanged(nameof(TemporaryStudent));
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
        public RelayCommand AddCommand { get; }

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
            AddCommand = CreateCommand(AddNewStudent);
            IsListBoxEnabled = false;

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
                    CurrentStudent = student;
                }
            }
        }

        private void AddNewStudent()
        {
            CurrentStudent = new StudentViewModel(new StudentDTO()); // Create a new student view model
            IsStudentIdReadOnly = false;  // Allow editing of ID for new student
            IsListBoxEnabled = true;
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
            try
            {
                var isUpdated = _studentService.Update(_temporaryStudent.Student);

                if (isUpdated)
                {
                    var studentToUpdate = StudentsList.FirstOrDefault(s => s.StudentID == _temporaryStudent.StudentID);
                    if (studentToUpdate != null)
                    {
                        // Update the student only if the update operation is successful
                        studentToUpdate.Student.Name = _temporaryStudent.Name;
                        studentToUpdate.Student.Age = _temporaryStudent.Age;
                        studentToUpdate.Student.RoomNumber = _temporaryStudent.RoomNumber;

                        OnPropertyChanged(nameof(StudentsList));

                        Message = "Student updated";
                        IsListBoxEnabled = true;

                        // Copy the updated details back to CurrentStudent
                        CurrentStudent.CopyFrom(studentToUpdate);
                    }
                    else
                    {
                        Message = "Student not found in the list.";
                        IsListBoxEnabled = true;
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
            CurrentStudent = null; // Clear CurrentStudent to prevent showing invalid data in the list
            Message = "Student deselected";
        }

        public void Clear()
        {
            CurrentStudent = null; // Clear CurrentStudent
            Message = "";
            IsListBoxEnabled = true;
        }
    }
}
