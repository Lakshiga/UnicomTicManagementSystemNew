using System;
using System.Data;
using System.Data.SQLite;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Data;
using System.Collections.Generic;

namespace UnicomTicManagementSystem.Repositories
{
    public static class StudentRepository
    {
        public static Student GetStudentById(int id)
        {
            Student student = null;

            using (SQLiteConnection conn = DbCon.GetConnection())
            {
                string query = @"SELECT s.*, u.Username, u.Password 
                                 FROM Students s 
                                 INNER JOIN Users u ON s.UserId = u.Id 
                                 WHERE s.Id = @Id";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = MapStudentFromReader(reader);
                        }
                    }
                }
            }

            return student;
        }

        public static Student GetStudentByUsername(string username)
        {
            Student student = null;

            using (SQLiteConnection conn = DbCon.GetConnection())
            {
                // Step 1: Get user by username
                string userQuery = "SELECT Id FROM Users WHERE Username = @Username";

                int userId = -1;
                using (SQLiteCommand cmd = new SQLiteCommand(userQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        userId = Convert.ToInt32(result);
                    else
                        return null; // user not found
                }

                // Step 2: Get student by UserId
                string studentQuery = @"
                    SELECT s.*, u.Username, u.Password, sec.Name AS SectionName
                    FROM Students s
                    INNER JOIN Users u ON s.UserId = u.Id
                    LEFT JOIN Sections sec ON s.SectionId = sec.Id
                    WHERE s.UserId = @UserId";

                using (SQLiteCommand cmd = new SQLiteCommand(studentQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = MapStudentFromReader(reader);
                        }
                    }
                }
            }

            return student;
        }

        public static List<string> GetSubjectsBySectionName(string sectionName)
        {
            List<string> subjects = new List<string>();

            using (var conn = DbCon.GetConnection())
            {
                string query = @"
                    SELECT sub.SubjectName
                    FROM Subjects sub
                    INNER JOIN Sections sec ON sub.SectionId = sec.Id
                    WHERE sec.Name = @SectionName";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SectionName", sectionName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return subjects;
        }

        public static DataTable GetTimetableBySection(string sectionName)
        {
            DataTable dt = new DataTable();

            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT Date AS Day, TimeSlot, Subject, Room FROM Timetable;";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }




        public static DataTable GetExamMarksByUsername(string username)
        {
            DataTable dt = new DataTable();

            using (var conn = DbCon.GetConnection())
            {
                string query = @"
                    SELECT m.Exam AS Exam, m.Subject AS Subject, m.Score AS Marks
                    FROM Marks m
                    INNER JOIN Students s ON m.StudentID = s.Id
                    INNER JOIN Users u ON s.UserId = u.Id
                    WHERE u.Username = @Username";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }



        private static Student MapStudentFromReader(SQLiteDataReader reader)
        {
            return new Student
            {
                Id = ColumnExists(reader, "Id") && reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                Name = ColumnExists(reader, "Name") ? reader["Name"].ToString() : "",
                Address = ColumnExists(reader, "Address") ? reader["Address"].ToString() : "",
                Stream = ColumnExists(reader, "Stream") ? reader["Stream"].ToString() : "",
                SectionId = ColumnExists(reader, "SectionId") && reader["SectionId"] != DBNull.Value ? Convert.ToInt32(reader["SectionId"]) : 0,
                SectionName = ColumnExists(reader, "SectionName") ? reader["SectionName"].ToString() : "",
                UserId = ColumnExists(reader, "UserId") && reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
                Username = ColumnExists(reader, "Username") ? reader["Username"].ToString() : "",
                Password = ColumnExists(reader, "Password") ? reader["Password"].ToString() : ""
            };
        }

        private static bool ColumnExists(SQLiteDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
