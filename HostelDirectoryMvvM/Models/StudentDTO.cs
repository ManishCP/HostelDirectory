namespace HostelDirectoryMvvM.Models
{
    public class StudentDTO
    {
        public string StudentID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int RoomNumber { get; set; }
        public bool IsDeletable { get; set; }
    }
}