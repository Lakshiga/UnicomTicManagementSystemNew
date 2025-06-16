using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Services;

namespace UnicomTicManagementSystem.Repositories
{
    public static class UserRepository
    {
        public static int AddUser(User user)
        {
            using (var conn = DbCon.GetConnection())
            {
                // Check if the username already exists and get the Id
                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = "SELECT Id FROM Users WHERE Username = @username";
                checkCmd.Parameters.AddWithValue("@username", user.Username);

                var existingId = checkCmd.ExecuteScalar();

                if (existingId != null && existingId != DBNull.Value)
                {
                    // User already exists, return their actual Id
                    return Convert.ToInt32(existingId);
                }

                // Proceed with insert
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Users (Username, Password, Role) 
                            VALUES (@username, @password, @role); 
                            SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@role", user.Role);

                long id = (long)cmd.ExecuteScalar();
                return (int)id;
            }
        }

        public static User GetUserById(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }

            return null;
        }



        public static int GetUserIdByUsername(string username)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id FROM Users WHERE Username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return -1; // not found
            }
        }

        public static void UpdateUser(User user)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Users SET Username = @username, Password = @password WHERE Id = @id";
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@id", user.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static string connectionString = "Data Source=SchoolManageDB.db;Version=3;";
        public static User Authenticate(string username, string password)
        {
            User user = null;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    conn.Open(); // ✅ This is OK because you're not using DbCon here
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            return user;
        }

        public static bool UserExists(string username)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                cmd.Parameters.AddWithValue("@username", username);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public static bool HasAnyUsers()
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Users";
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
