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

        public bool Update(StudentDTO objUpdatedStudent)
        {
            bool isUpdated = false;
            if (objUpdatedStudent.Age < 21 || objUpdatedStudent.Age > 25)
                throw new ArgumentException("Invalid age limit for student");

            string sql = "EXEC UpdateStudent @Name, @Age, @RoomNumber, @StudentID";

            try
            {
                var nameParam = new SqlParameter("@Name", objUpdatedStudent.Name);
                var ageParam = new SqlParameter("@Age", objUpdatedStudent.Age);
                var roomNumberParam = new SqlParameter("@RoomNumber", objUpdatedStudent.RoomNumber);
                var idParam = new SqlParameter("@StudentID", objUpdatedStudent.StudentID);

                // Execute the stored procedure
                int result = ObjContext.Database.ExecuteSqlCommand(sql, nameParam, ageParam, roomNumberParam, idParam);
                isUpdated = result > 0;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Failed to update student: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the student.", ex);
            }

            return isUpdated;
        }


        public bool Delete(string studentId)
        {
            bool isDeleted = false;

            try
            {
                var student = ObjContext.Students.FirstOrDefault(s => s.StudentID == studentId);
                if (student == null)
                    throw new InvalidOperationException("Student not found");

                ObjContext.Students.Remove(student);
                ObjContext.SaveChanges();
                isDeleted = true;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Failed to delete student: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the student.", ex);
            }

            return isDeleted;
        }


        public List<StudentDTO> Search(string searchTerm)
        {
            List<StudentDTO> searchResults = new List<StudentDTO>();

            try
            {
                var ObjQuery = from student
                               in ObjContext.Students
                               where student.Name.Contains(searchTerm) || student.StudentID.Contains(searchTerm)
                               select student;
                foreach (var student in ObjQuery)
                {
                    searchResults.Add(new StudentDTO { StudentID = student.StudentID, Name = student.Name, Age = student.Age, RoomNumber = student.RoomNumber });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return searchResults;
        }


    }
}
