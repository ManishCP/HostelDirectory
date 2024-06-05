using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelDirectoryMvvM.Models
{
    public class StudentService
    {
        HostelDirectoryDemoDbEntities1 ObjContext;
        //private static List<StudentDTO> ObjStudentsList;
        public StudentService() 
        {
            ObjContext = new HostelDirectoryDemoDbEntities1();            
        }

        public List<StudentDTO> GetAll()
        { 
            List<StudentDTO> ObjStudentsList = new List<StudentDTO>();
            try
            {
                var ObjQuery = from student
                               in ObjContext.Students 
                               select student;
                foreach (var student in ObjQuery)
                {
                    ObjStudentsList.Add(new StudentDTO { Id=student.Id, Name=student.Name, Age=student.Age, RoomNumber=student.RoomNumber});
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return ObjStudentsList;
        }

        public bool Add(StudentDTO objNewStudent) 
        {
            bool IsAdded = false;
            if (objNewStudent.Age < 21 || objNewStudent.Age > 25)
                throw new ArgumentException("Invalid age limit for sutdent");

            try
            {
                var ObjStudent = new Student();
                ObjStudent.Id = objNewStudent.Id;
                ObjStudent.Name = objNewStudent.Name;
                ObjStudent.Age = objNewStudent.Age;
                ObjStudent.RoomNumber = objNewStudent.RoomNumber;

                ObjContext.Students.Add(ObjStudent);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return IsAdded;
        }

        public bool Update(StudentDTO objStudentToUpdate)
        {
            bool IsUpdated = false;

            try
            {
                var ObjStudent = ObjContext.Students.Find(objStudentToUpdate.Id);
                ObjStudent.Name = objStudentToUpdate.Name;
                ObjStudent.Age = objStudentToUpdate.Age;
                ObjStudent.RoomNumber= objStudentToUpdate.RoomNumber;

                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsUpdated = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return IsUpdated;
        }

        public bool Delete(int id)
        {
            bool IsDeleted = false;
            try
            {
                var ObjStudentToDelete = ObjContext.Students.Find(id);
                ObjContext.Students.Remove(ObjStudentToDelete);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsDeleted = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return IsDeleted;
        }

        public StudentDTO Search(int id)
        {
            StudentDTO ObjStudent = null;

            try
            {
                var ObjStudentToFind = ObjContext.Students.Find(id);
                if (ObjStudentToFind != null)
                {
                    ObjStudent = new StudentDTO()
                    {
                        Id = ObjStudentToFind.Id,
                        Name = ObjStudentToFind.Name,
                        Age = ObjStudentToFind.Age,
                        RoomNumber = ObjStudentToFind.RoomNumber,
                    };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ObjStudent;
        }

    }
}
