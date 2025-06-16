using System;
using System.Data;
using System.Data.SQLite;

namespace UnicomTicManagementSystem.Data
{
    public class TimeTableRepository
    {
        public DataTable GetAllTimeTables()
        {
            DataTable table = new DataTable();
            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT * FROM Timetable";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }
            return table;
        }

        public void AddTimeTable(string subject, string timeSlot, string room, DateTime date)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "INSERT INTO Timetable (Subject, TimeSlot, Room, Date) VALUES (@Subject, @TimeSlot, @Room, @Date)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@TimeSlot", timeSlot);
                    cmd.Parameters.AddWithValue("@Room", room);
                    cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTimeTable(int id, string subject, string timeSlot, string room, DateTime date)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "UPDATE Timetable SET Subject = @Subject, TimeSlot = @TimeSlot, Room = @Room, Date = @Date WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@TimeSlot", timeSlot);
                    cmd.Parameters.AddWithValue("@Room", room);
                    cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTimeTable(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "DELETE FROM Timetable WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
