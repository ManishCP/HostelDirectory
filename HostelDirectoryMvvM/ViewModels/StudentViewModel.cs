using HostelDirectoryMvvM.Commands;
using HostelDirectoryMvvM.Models;

namespace HostelDirectoryMvvM.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        private readonly StudentService _studentService;
        private readonly RelayCommand _deleteCommand;
        private bool _isStudentIdReadOnly;
        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private StudentDTO _student;
        public StudentDTO Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged(nameof(Student));
            }
        }

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
            _student = student;
            _studentService = new StudentService();
            _deleteCommand = new RelayCommand(Delete, () => IsDeletable);
            IsStudentIdReadOnly = Student.StudentID != null;
        }

        public void CopyFrom(StudentViewModel other)
        {
            this.StudentID = other.StudentID;
            this.Name = other.Name;
            this.Age = other.Age;
            this.RoomNumber = other.RoomNumber;
        }

        private void Delete()
        {
            Messenger.Instance.Publish(new DeleteMessage(Student.StudentID));
        }
    }
}
