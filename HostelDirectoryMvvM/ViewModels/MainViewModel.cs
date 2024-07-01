using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;

namespace HostelDirectoryMvvM.ViewModels
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string Message { get; private set; }

        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }
    }

    public class MainViewModel : BaseViewModel
    {
        private readonly StudentService _studentService;
        private StudentViewModel _currentStudent;
        private string _filter;
        private ObservableCollection<StudentViewModel> _studentsList;
        private ListCollectionView _filteredStudents;
        private string _message;

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
                    IsStudentIdReadOnly = !string.IsNullOrEmpty(_currentStudent.StudentID);
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

        private void AddNewStudent()
        {
            CurrentStudent = new StudentViewModel(new StudentDTO()); // Create a new student view model
            IsStudentIdReadOnly = false;  // Allow editing of ID for new student
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

        private ValidationResult ValidateStudent(StudentDTO student)
        {
            if (string.IsNullOrWhiteSpace(student.Name))
            {
                return new ValidationResult(false, "Student name cannot be empty.");
            }

            if (student.Age <= 0 || student.RoomNumber <= 0)
            {
                return new ValidationResult(false, "Age and room number must be greater than zero.");
            }

            if (student.Age < 21 || student.Age > 25)
            {
                return new ValidationResult(false, "Age limit for student is 21-25.");
            }

            return new ValidationResult(true, "");
        }

        public void Save()
        {
            try
            {
                ValidationResult validationResult = ValidateStudent(CurrentStudent.Student);

                if (!validationResult.IsValid)
                {
                    Message = validationResult.Message;
                    return;
                }

                var isSaved = _studentService.Add(CurrentStudent.Student);
                if (isSaved)
                {
                    CurrentStudent.Student.IsDeletable = !_studentService.IsPredefinedStudent(CurrentStudent.StudentID);
                    StudentsList.Add(CurrentStudent);
                    OnPropertyChanged(nameof(StudentsList));
                    //ClearCurrentStudent();
                    CurrentStudent.IsStudentIdReadOnly = true;
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
            var originalStudent = StudentsList.FirstOrDefault(s => s.StudentID == CurrentStudent.StudentID);
            ValidationResult validationResult = ValidateStudent(CurrentStudent.Student);

            if (originalStudent != null)
            {
                // Debug or log the values to ensure correctness
                Debug.WriteLine($"Original Student: {originalStudent.StudentID} - {originalStudent.Age}");
                Debug.WriteLine($"Current Student: {CurrentStudent.StudentID} - {CurrentStudent.Age}");

                // Ensure Clone and CopyFrom methods are correctly implemented
                CurrentStudent.CopyFrom(originalStudent);
                OnPropertyChanged(nameof(StudentsList)); // Notify UI of the change
            }

            if (!validationResult.IsValid)
            {
                Message = validationResult.Message;
                if (originalStudent != null)
                {
                    // Revert to original values
                    CurrentStudent.CopyFrom(originalStudent);
                    OnPropertyChanged(nameof(StudentsList));
                }
                return;
            }

            try
            {
                var isUpdated = _studentService.Update(CurrentStudent.Student);

                if (isUpdated)
                {
                    var studentToUpdate = StudentsList.FirstOrDefault(s => s.StudentID == CurrentStudent.StudentID);
                    if (studentToUpdate != null)
                    {
                        // Update the student only if the update operation is successful
                        studentToUpdate.Student.Name = CurrentStudent.Name;
                        studentToUpdate.Student.Age = CurrentStudent.Age;
                        studentToUpdate.Student.RoomNumber = CurrentStudent.RoomNumber;

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
        }
    }
}
