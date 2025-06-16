using System.Data;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;

public class SubjectController
{
    public DataTable GetSubjects()
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = @"
            SELECT s.SubjectID, s.SubjectName, s.SectionID, sec.Name AS SectionName
            FROM Subjects s
            JOIN Sections sec ON s.SectionID = sec.Id";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

    public DataTable GetSubjectsBySection(int sectionId)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "SELECT * FROM Subjects WHERE SectionID = @SectionID";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@SectionID", sectionId);
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public void AddSubject(string name, int sectionId)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "INSERT INTO Subjects (SubjectName, SectionID) VALUES (@name, @sectionId)";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@sectionId", sectionId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void UpdateSubject(int id, string name, int sectionId)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "UPDATE Subjects SET SubjectName = @name, SectionID = @sectionId WHERE SubjectID = @id";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@sectionId", sectionId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteSubject(int id)
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "DELETE FROM Subjects WHERE SubjectID = @id";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }


    public DataTable GetAllSections()
    {
        using (var conn = DbCon.GetConnection())
        {
            string query = "SELECT Id, Name FROM Sections";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

}
