using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using UnicomTicManagementSystem.Repositories;


namespace UnicomTicManagementSystem.Controllers
{
    internal class StudentController
    {
        //private readonly StudentService _studentService;

        public StudentController()
        {
            //_studentService = new StudentService();
        }

        //public List<Student> GetAllStudents() => _studentService.GetAll();

        //public void AddStudent(Student student) => _studentService.Add(student);

        //public void UpdateStudent(Student student) => _studentService.Update(student);

        //public void DeleteStudent(int studentId) => _studentService.Delete(studentId);


        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand(@"
                    SELECT s.Id, s.Name, s.Address, s.SectionId, sec.Name AS SectionName
                    FROM Students s
                    LEFT JOIN Sections sec ON s.SectionId = sec.Id", conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Student student = new Student
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        SectionId = reader.GetInt32(3),
                        SectionName = reader.GetString(4),
                    };
                    students.Add(student);
                                   
                }
            }

            return students;
        }

        public void AddStudent(Student student, string username, string password)
        {
            using (var conn = DbCon.GetConnection()) // Already opens inside
            {
                // Check if user exists
                int userId = UserRepository.GetUserIdByUsername(username);
                if (userId == -1)
                {
                    // Add user first and get new user id
                    userId = UserRepository.AddUser(new User
                    {
                        Username = username,
                        Password = password,
                        Role = "student"
                    });
                }

                // Insert student with UserId FK
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Students (Name, Address, SectionId,UserId) VALUES (@name, @address, @sectionId, @userId)";
                cmd.Parameters.AddWithValue("@name", student.Name);
                cmd.Parameters.AddWithValue("@address", student.Address);
                cmd.Parameters.AddWithValue("@sectionId", student.SectionId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateStudent(Student student, string username, string password)
        {
            using (var conn = DbCon.GetConnection())
            {
                // Update user info
                UserRepository.UpdateUser(new User
                {
                    Id = student.UserId,
                    Username = username,
                    Password = password,
                    Role = "student"
                });

                // Update student info
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Students SET Name = @name, Address = @address, SectionId = @sectionId, SectionName = @sectionName WHERE Id = @id";
                cmd.Parameters.AddWithValue("@name", student.Name);
                cmd.Parameters.AddWithValue("@address", student.Address);
                cmd.Parameters.AddWithValue("@sectionId", student.SectionId);
                cmd.Parameters.AddWithValue("@sectionName", student.SectionName ?? "");
                cmd.Parameters.AddWithValue("@id", student.Id);
                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteStudent(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                // Get UserId of student to delete
                int userId = -1;
                var cmdSelect = conn.CreateCommand();
                cmdSelect.CommandText = "SELECT UserId FROM Students WHERE Id = @id";
                cmdSelect.Parameters.AddWithValue("@id", id);
                var result = cmdSelect.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    userId = Convert.ToInt32(result);
                }

                // Delete student
                var cmdDeleteStudent = conn.CreateCommand();
                cmdDeleteStudent.CommandText = "DELETE FROM Students WHERE Id = @id";
                cmdDeleteStudent.Parameters.AddWithValue("@id", id);
                cmdDeleteStudent.ExecuteNonQuery();

                if (userId != -1)
                {
                    var cmdDeleteUser = conn.CreateCommand();
                    cmdDeleteUser.CommandText = "DELETE FROM Users WHERE Id = @userId";
                    cmdDeleteUser.Parameters.AddWithValue("@userId", userId);
                    cmdDeleteUser.ExecuteNonQuery();
                }
            }
        }


        public Student GetStudentById(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand("SELECT * FROM Students WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Address = reader.GetString(2),
                            SectionId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        };
                    }
                }
            }

            return null;
        }

    }
}
