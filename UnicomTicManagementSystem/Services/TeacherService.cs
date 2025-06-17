using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTicManagementSystem.Services
{
    internal class TeacherService
    {
        public void Add(Teacher teacher)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Teachers (Name, Phone, Address) VALUES (@name, @phone, @address)";
                cmd.Parameters.AddWithValue("@name", teacher.Name);
                cmd.Parameters.AddWithValue("@phone", teacher.Phone);
                cmd.Parameters.AddWithValue("@address", teacher.Address);
                cmd.ExecuteNonQuery();
            }
        }

        public void AddUser(string username, string password, string role)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Teacher> SearchTeachers(string keyword)
        {
            var teachers = new List<Teacher>();
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Teachers WHERE Name LIKE @kw OR Phone LIKE @kw OR Address LIKE @kw";
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Phone = reader.GetString(2),
                            Address = reader.GetString(3)
                        });
                    }
                }
            }
            return teachers;
        }


        public List<Teacher> GetAll()
        {
            var teachers = new List<Teacher>();
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Teachers";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new Teacher
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Phone = reader.GetString(2),
                            Address = reader.GetString(3)
                        });
                    }
                }
            }
            return teachers;
        }

        public void Update(Teacher teacher)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Teachers SET Name = @name, Phone = @phone, Address = @address WHERE Id = @id";
                cmd.Parameters.AddWithValue("@name", teacher.Name);
                cmd.Parameters.AddWithValue("@phone", teacher.Phone);
                cmd.Parameters.AddWithValue("@address", teacher.Address);
                cmd.Parameters.AddWithValue("@id", teacher.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Teachers WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Section> GetAllSections()
        {
            var sections = new List<Section>();
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Sections";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sections.Add(new Section
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return sections;
        }

        public int AddWithReturnId(Teacher teacher)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Teachers (Name, Phone, Address) VALUES (@name, @phone, @address); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@name", teacher.Name);
                cmd.Parameters.AddWithValue("@phone", teacher.Phone);
                cmd.Parameters.AddWithValue("@address", teacher.Address);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void AssignSectionToTeacher(int teacherId, int sectionId)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT OR REPLACE INTO TeacherSection (TeacherId, SectionId) VALUES (@tid, @sid)";
                cmd.Parameters.AddWithValue("@tid", teacherId);
                cmd.Parameters.AddWithValue("@sid", sectionId);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
