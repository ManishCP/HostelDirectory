using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System;

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
        deleteCommand = new RelayCommand(Delete);
        filterTextChangedCommand = new RelayCommand(FilterStudentsTextChanged);
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
            if (CurrentStudent == null)
            {
                Message = "No student selected. Please select a student to update.";
                return;
            }

            if (string.IsNullOrEmpty(CurrentStudent.StudentID))
            {
                Message = "Student ID is missing.";
                return;
            }

            if (string.IsNullOrEmpty(CurrentStudent.Name))
            {
                Message = "Student name is missing.";
                return;
            }

            if (CurrentStudent.Age <= 0)
            {
                Message = "Student age must be greater than zero.";
                return;
            }

            if (CurrentStudent.RoomNumber <= 0)
            {
                Message = "Room number must be greater than zero.";
                return;
            }

            var IsUpdated = ObjStudentService.Update(CurrentStudent);
            if (IsUpdated)
            {
                Message = "Student Info Updated";
                LoadData();
                CurrentStudent = null; // Clear the selection
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

    private RelayCommand deleteCommand;
    public RelayCommand DeleteCommand
    {
        get { return deleteCommand; }
    }

    public void Delete()
    {
        try
        {
            if (CurrentStudent == null || string.IsNullOrEmpty(CurrentStudent.StudentID))
            {
                Message = "Please select a valid student to delete.";
                return;
            }

            var IsDelete = ObjStudentService.Delete(CurrentStudent.StudentID);
            if (IsDelete)
            {
                Message = "Student Records Deleted";
                LoadData();
                CurrentStudent = null;
            }
            else
            {
                Message = "Could Not Delete";
            }
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }
    }
}
