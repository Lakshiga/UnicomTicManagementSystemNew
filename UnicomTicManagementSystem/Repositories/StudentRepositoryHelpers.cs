using System;
using System.Data.SqlClient;
using UnicomTicManagementSystem.Models;

internal static class StudentRepositoryHelpers
{

    public static Student GetStudentByUsername(string username)
    {
        Student student = null;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Student WHERE Username = @Username";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader["Id"] != DBNull.Value ? reader["Id"].ToString() : string.Empty,
                            Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                            Username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() : string.Empty,
                            Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : string.Empty,
                            Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : string.Empty,
                            Stream = reader["Stream"] != DBNull.Value ? reader["Stream"].ToString() : string.Empty
                        };
                    }
                }
            }
        }

        return student;
    }
}