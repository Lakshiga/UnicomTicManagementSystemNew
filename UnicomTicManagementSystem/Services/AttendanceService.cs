using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;

namespace UnicomTicManagementSystem.Services
{
    public class AttendanceService
    {
        public void AddAttendance(Attendance attendance)
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"
                                    INSERT INTO Attendance (StudentID, SubjectID, Date, Status)
                                    VALUES (@StudentID, @SubjectID, @Date, @Status)";

                        cmd.Parameters.AddWithValue("@StudentID", attendance.StudentID);
                        cmd.Parameters.AddWithValue("@SubjectID", attendance.SubjectID);
                        cmd.Parameters.AddWithValue("@Date", attendance.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Status", attendance.Status);

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
            }
        }

        public void UpdateAttendance(Attendance attendance)
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"
                            UPDATE Attendance 
                            SET StudentID = @StudentID, SubjectID = @SubjectID, Date = @Date, Status = @Status 
                            WHERE AttendanceID = @AttendanceID";

                    cmd.Parameters.AddWithValue("@AttendanceID", attendance.AttendanceID);
                    cmd.Parameters.AddWithValue("@Date", attendance.Date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@StudentID", attendance.StudentID);
                    cmd.Parameters.AddWithValue("@SubjectID", attendance.SubjectID);
                    cmd.Parameters.AddWithValue("@Status", attendance.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAttendance(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand("DELETE FROM Attendance WHERE AttendanceID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetAllAttendance()
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand(conn))
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        cmd.CommandText = @"
                            SELECT 
                                A.AttendanceID, 
                                A.Date, 
                                A.StudentID, 
                                S.Name AS StudentName, 
                                A.SubjectID, 
                                Sub.SubjectName, 
                                A.Status
                            FROM Attendance A
                            JOIN Students S ON A.StudentID = S.Id
                            JOIN Subjects Sub ON A.SubjectID = Sub.SubjectID";

                        var table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }


        public DataTable SearchAttendance(DateTime date, string studentId, string studentName, string subjectName, string status)
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;

                    string query = @"
                                    SELECT A.AttendanceID, A.Date, A.StudentID, S.Name AS StudentName, 
                                           Sub.SubjectName, A.Status, A.SubjectID
                                    FROM Attendance A
                                    JOIN Students S ON A.StudentID = S.Id
                                    JOIN Subjects Sub ON A.SubjectID = Sub.SubjectID
                                    WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(studentId))
                    {
                        query += " AND A.StudentID LIKE @StudentID";
                        cmd.Parameters.AddWithValue("@StudentID", $"%{studentId}%");
                    }

                    if (!string.IsNullOrWhiteSpace(studentName))
                    {
                        query += " AND S.StudentName LIKE @StudentName";
                        cmd.Parameters.AddWithValue("@StudentName", $"%{studentName}%");
                    }

                    if (!string.IsNullOrWhiteSpace(subjectName))
                    {
                        query += " AND Sub.SubjectName LIKE @SubjectName";
                        cmd.Parameters.AddWithValue("@SubjectName", $"%{subjectName}%");
                    }

                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        query += " AND A.Status LIKE @Status";
                        cmd.Parameters.AddWithValue("@Status", $"%{status}%");
                    }

                    query += " AND A.Date = @Date";
                    cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));

                    cmd.CommandText = query;

                    var dt = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                    return dt;
                }
            }
        }

        public dynamic GetStudentByID(string studentId)
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand("SELECT * FROM Students WHERE ID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new
                            {
                                StudentID = reader["ID"].ToString(),
                                StudentName = reader["Name"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public DataTable GetAllSubjects()
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand("SELECT * FROM Subjects", conn))
                { 
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetSubjectsByStudentID(string studentId)
        {
            using (var conn = DbCon.GetConnection())
            {
                using (var cmd = new SQLiteCommand(@"
                        SELECT s.SubjectID, s.SubjectName
                        FROM Students st
                        JOIN Sections sec ON st.SectionId = sec.Id
                        JOIN Subjects s ON s.SectionID = sec.Id
                        WHERE st.Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}
