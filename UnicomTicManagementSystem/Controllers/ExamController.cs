using System;
using System.Data;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;

public class ExamController
{
    // Get all exams with subject name
    public DataTable GetExams()
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = @"
                SELECT e.ExamID, e.ExamName, s.SubjectID, s.SubjectName
                FROM ManageExam e
                JOIN Subjects s ON e.SubjectID = s.SubjectID";

            using (var cmd = new SQLiteCommand(query, conn))
            using (var adapter = new SQLiteDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    // Insert a new exam
    public void AddExam(string examName, int subjectId)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "INSERT INTO ManageExam (SubjectID, ExamName) VALUES (@subjectId, @examName)";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@subjectId", subjectId);
                cmd.Parameters.AddWithValue("@examName", examName);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Update an existing exam
    public void UpdateExam(int examId, string examName, int subjectId)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "UPDATE ManageExam SET ExamName = @examName, SubjectID = @subjectId WHERE ExamID = @examId";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@examId", examId);
                cmd.Parameters.AddWithValue("@examName", examName);
                cmd.Parameters.AddWithValue("@subjectId", subjectId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Delete an exam
    public void DeleteExam(int examId)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "DELETE FROM ManageExam WHERE ExamID = @examId";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@examId", examId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Get all subjects (to bind to ComboBox)
    public DataTable GetAllSubjects()
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "SELECT SubjectID, SubjectName FROM Subjects";
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
