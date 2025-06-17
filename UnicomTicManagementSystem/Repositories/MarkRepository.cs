using System;
using System.Data;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;

namespace UnicomTicManagementSystem.Repositories
{
    public class MarkRepository
    {
        public DataTable GetAllMarks()
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT * FROM Marks";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public void AddMark(int studentId, string subject, string exam, int score)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "INSERT INTO Marks (StudentID, Subject, Exam, Score) VALUES (@StudentID, @Subject, @Exam, @Score)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Exam", exam);
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMark(int markId)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "DELETE FROM Marks WHERE MarkID = @MarkID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MarkID", markId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMark(int markId, int studentId, string subject, string exam, int score)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "UPDATE Marks SET StudentID = @StudentID, Subject = @Subject, Exam = @Exam, Score = @Score WHERE MarkID = @MarkID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MarkID", markId);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Exam", exam);
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetStudentName(int studentId)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT Name FROM Students WHERE ID = @StudentID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
        }


        public DataTable GetSubjectsByStudent(int studentId)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT DISTINCT SubjectName \r\nFROM Subjects \r\nWHERE SectionId IN (SELECT SectionId FROM Students WHERE Id = @StudentID)\r\n";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetAllExams()
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT DISTINCT ExamName FROM ManageExam";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
