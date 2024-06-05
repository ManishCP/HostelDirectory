using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelDirectoryMvvM.Models
{
    public class StudentService
    {
        HostelDirectoryDemoDbEntities2 ObjContext;
        public StudentService() 
        {
            ObjContext = new HostelDirectoryDemoDbEntities2();            
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
                    ObjStudentsList.Add(new StudentDTO { StudentID = student.StudentID, Name=student.Name, Age=student.Age, RoomNumber=student.RoomNumber});
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
            bool isAdded = false;
            if (objNewStudent.Age < 21 || objNewStudent.Age > 25)
                throw new ArgumentException("Invalid age limit for student");

            string sql = "EXEC AddStudent @Name, @Age, @RoomNumber, @StudentID";

            try
            {
                var nameParam = new SqlParameter("@Name", objNewStudent.Name);
                var ageParam = new SqlParameter("@Age", objNewStudent.Age);
                var roomNumberParam = new SqlParameter("@RoomNumber", objNewStudent.RoomNumber);
                var idParam = new SqlParameter("@StudentID", objNewStudent.StudentID);

                // Execute the stored procedure
                int result = ObjContext.Database.ExecuteSqlCommand(sql, nameParam, ageParam, roomNumberParam, idParam);
                isAdded = result > 0;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Failed to add student: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the student.", ex);
            }

            return isAdded;
        }

        public bool Update(StudentDTO objStudentToUpdate)
        {
            bool IsUpdated = false;

            try
            {
                var ObjStudent = ObjContext.Students.Find(objStudentToUpdate.StudentID);
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

        public bool Delete(string studentId)
        {
            bool IsDeleted = false;
            try
            {
                var ObjStudentToDelete = ObjContext.Students.Find(studentId);
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

        public StudentDTO Search(string name)
        {
            StudentDTO ObjStudent = null;

            try
            {
                var ObjStudentToFind = ObjContext.Students.Find(name);
                if (ObjStudentToFind != null)
                {
                    ObjStudent = new StudentDTO()
                    {
                        StudentID = ObjStudentToFind.StudentID,
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
