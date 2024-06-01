using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelDirectoryMvvM.Models
{
    internal class StudentService
    {
        private static List<Student> ObjStudentsList;
        public StudentService() 
        {
            ObjStudentsList = new List<Student>();
            {
                new Student { Id = 101, Name = "Manish", Age = 23, RoomNumber = 508 };
            }
        }

        public List<Student> GetAll()
        { 
            return ObjStudentsList; 
        }

        public bool Add(Student objNewStudent) 
        {
            if ((objNewStudent.Age > 21 || objNewStudent.Age < 25))
                throw new ArgumentException("Invalid age limit for the student");

            ObjStudentsList.Add(objNewStudent);
            return true;
        }

        public bool Update(Student objNewStudentToUpdate)
        {
            bool IsUpdated = false;
            for (int index = 0; index < ObjStudentsList.Count; index++)
            {
                if (ObjStudentsList[index].Id == objNewStudentToUpdate.Id)
                {
                    ObjStudentsList[index].Name = objNewStudentToUpdate.Name;
                    ObjStudentsList[index].Age = objNewStudentToUpdate.Age;
                    IsUpdated = true;
                    break;
                }
            }
            return IsUpdated;
        }

        public bool Delete(int id)
        {
            bool IsDeleted = false;
            for (int index = 0; index < ObjStudentsList.Count; index++)
            {
                if (ObjStudentsList[index].Id == id)
                {
                    ObjStudentsList.RemoveAt(index);
                    IsDeleted = true;
                    break;
                }
            }
            return IsDeleted;
        }

        public Student Search(int id)
        { 
            return ObjStudentsList.FirstOrDefault(e => e.Id == id); 
        }

    }
}
