using System;
using System.Data;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;

namespace UnicomTicManagementSystem.Controllers
{
    public class TimetableController
    {
        private readonly TimeTableRepository repository = new TimeTableRepository();

        public DataTable GetSubjects()
        {
            using (var conn = DbCon.GetConnection())
            {
                string query = "SELECT SubjectName FROM Subjects"; // adjust as needed
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetAllTimetables()
        {
            return repository.GetAllTimeTables();
        }

        // ✅ Add DateTime parameter here
        public void AddTimetable(string subject, string timeSlot, string room, DateTime date)
        {
            repository.AddTimeTable(subject, timeSlot, room, date);
        }

        // ✅ Add DateTime parameter here
        public void UpdateTimetable(int id, string subject, string timeSlot, string room, DateTime date)
        {
            repository.UpdateTimeTable(id, subject, timeSlot, room, date);
        }

        public void DeleteTimetable(int id)
        {
            repository.DeleteTimeTable(id);
        }
    }
}
