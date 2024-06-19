using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;

namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        private readonly StudentService _studentService;
        private readonly RelayCommand _deleteCommand;
        private bool _isStudentIdReadOnly;

        public StudentDTO Student { get; }

        public string StudentID
        {
            get => Student.StudentID;
            set
            {
                Student.StudentID = value;
                OnPropertyChanged(nameof(StudentID));
            }
        }

        public string Name
        {
            get => Student.Name;
            set
            {
                Student.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Age
        {
            get => Student.Age;
            set
            {
                Student.Age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public int RoomNumber
        {
            get => Student.RoomNumber;
            set
            {
                Student.RoomNumber = value;
                OnPropertyChanged(nameof(RoomNumber));
            }
        }

        public bool IsDeletable => Student.IsDeletable;
        public bool IsStudentIdReadOnly
        {
            get => _isStudentIdReadOnly;
            set
            {
                if (_isStudentIdReadOnly != value)
                {
                    _isStudentIdReadOnly = value;
                    OnPropertyChanged(nameof(IsStudentIdReadOnly));
                }
            }
        }


        public RelayCommand DeleteCommand => _deleteCommand;

        public StudentViewModel(StudentDTO student)
        {
            Student = student;
            _studentService = new StudentService();
            _deleteCommand = new RelayCommand(Delete, () => IsDeletable);
            IsStudentIdReadOnly = Student.StudentID != null;
        }

        private void Delete()
        {
            Messenger.Instance.Publish(new DeleteMessage(Student.StudentID));
        }
    }
}
