using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        #region Fields
        private readonly StudentService ObjStudentService;
        private ObservableCollection<StudentDTO> studentsList;
        private StudentDTO currentStudent;
        private bool isStudentIdReadOnly;
        private string filter;
        private RelayCommand saveCommand;
        private RelayCommand searchCommand;
        private RelayCommand updateCommand;
        private RelayCommand deleteCommand;
        private RelayCommand clearCommand;
        private RelayCommand filterTextChangedCommand;
        #endregion

        #region Constructor
        public StudentViewModel()
        {
            ObjStudentService = new StudentService();
            LoadData();
            CurrentStudent = new StudentDTO();
            IsStudentIdReadOnly = false;
            saveCommand = CreateCommand(Save);
            searchCommand = CreateCommand(Search);
            updateCommand = CreateCommand(Update);
            deleteCommand = CreateCommand(SendDeleteMessage);
            clearCommand = CreateCommand(Clear);
            filterTextChangedCommand = CreateCommand(FilterStudentsTextChanged);

            SubscribeToMessenger<DeleteMessage>(HandleDeleteMessage);
        }
        #endregion

        #region Properties
        public ObservableCollection<StudentDTO> StudentsList
        {
            get { return studentsList; }
            set { studentsList = value; OnPropertyChanged(nameof(StudentsList)); }
        }

        public StudentDTO CurrentStudent
        {
            get { return currentStudent; }
            set
            {
                if (currentStudent != value)
                {
                    currentStudent = value;
                    OnPropertyChanged(nameof(CurrentStudent));

                    if (currentStudent != null)
                    {
                        IsStudentIdReadOnly = currentStudent.StudentID != null;
                        if (IsStudentIdReadOnly)
                        {
                            Message = "Student Selected";
                        }

                    }
                }
            }
        }

        public bool IsStudentIdReadOnly
        {
            get { return isStudentIdReadOnly; }
            set { isStudentIdReadOnly = value; OnPropertyChanged(nameof(IsStudentIdReadOnly)); }
        }

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

        public RelayCommand SaveCommand => saveCommand;
        public RelayCommand SearchCommand => searchCommand;
        public RelayCommand UpdateCommand => updateCommand;
        public RelayCommand DeleteCommand => deleteCommand;
        public RelayCommand ClearCommand => clearCommand;
        public RelayCommand FilterTextChangedCommand => filterTextChangedCommand;
        #endregion

        #region Methods
        private void LoadData()
        {
            var students = ObjStudentService.GetAll();
            StudentsList = new ObservableCollection<StudentDTO>(students);
            FilteredStudents = new ListCollectionView(StudentsList);
            OnPropertyChanged(nameof(StudentsList));
            OnPropertyChanged(nameof(FilteredStudents));
        }

        public void ClearCurrentStudent()
        {
            CurrentStudent = new StudentDTO();
            Message = "Student deselected";
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.IsSelected)
            {
                item.IsSelected = false;
                ClearCurrentStudent();
                IsStudentIdReadOnly = false;
            }
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
                FilteredStudents.Filter = null;
            }
            else
            {
                FilteredStudents.Filter = student =>
                {
                    var s = student as StudentDTO;
                    return s != null && s.Name.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }
            FilteredStudents.Refresh();
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

                var isSaved = ObjStudentService.Add(CurrentStudent);
                if (isSaved)
                {
                    CurrentStudent.IsDeletable = !ObjStudentService.IsPredefinedStudent(CurrentStudent.StudentID);
                    StudentsList.Add(CurrentStudent);
                    Message = "Student saved";
                    CurrentStudent = new StudentDTO();
                    OnPropertyChanged(nameof(StudentsList));
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

        public void Update()
        {
            try
            {
                if (CurrentStudent == null || string.IsNullOrEmpty(CurrentStudent.Name) || CurrentStudent.Age <= 0 || CurrentStudent.RoomNumber <= 0)
                {
                    Message = "Missing or invalid student information. Please check all fields.";
                    return;
                }

                var isUpdated = ObjStudentService.Update(CurrentStudent);
                if (isUpdated)
                {
                    var student = StudentsList.FirstOrDefault(s => s.StudentID == CurrentStudent.StudentID);
                    if (student != null)
                    {
                        student.Name = CurrentStudent.Name;
                        student.Age = CurrentStudent.Age;
                        student.RoomNumber = CurrentStudent.RoomNumber;
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

        public void SendDeleteMessage()
        {
            PublishMessage(new DeleteMessage(CurrentStudent.StudentID));
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

                var isDeleted = ObjStudentService.Delete(studentId);
                if (isDeleted)
                {
                    var student = StudentsList.FirstOrDefault(s => s.StudentID == studentId);
                    if (student != null)
                    {
                        StudentsList.Remove(student);
                        OnPropertyChanged(nameof(StudentsList));                        
                        ClearCurrentStudent();
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

        public void Clear()
        {
            CurrentStudent = new StudentDTO();
            Message = "";
        }
        #endregion

    }
}
