using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Repositories;

namespace UnicomTicManagementSystem.Controllers
{
    public class StaffController
    {
        public List<Staff> GetAllStaff()
        {
            var staffList = new List<Staff>();

            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, Name, Address, Email, UserId FROM Staff"; // ✅ fixed table name

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        staffList.Add(new Staff
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Address = reader["Address"].ToString(),
                            Email = reader["Email"].ToString(),
                            UserId = Convert.ToInt32(reader["UserId"])
                        });
                    }
                }
            }

            return staffList;
        }



        public void AddStaff(Staff staff, string username, string password)
        {
            using (var conn = DbCon.GetConnection())
            {
                int userId = UserRepository.GetUserIdByUsername(username);
                if (userId == -1)
                {
                    userId = UserRepository.AddUser(new User
                    {
                        Username = username,
                        Password = password,
                        Role = "staff"
                    });
                }

                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Staff (Name, Address, Email, UserId) VALUES (@name, @address, @email, @userId)";
                cmd.Parameters.AddWithValue("@name", staff.Name);
                cmd.Parameters.AddWithValue("@address", staff.Address);
                cmd.Parameters.AddWithValue("@email", staff.Email);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStaff(Staff staff, string username, string password)
        {
            using (var conn = DbCon.GetConnection())
            {
                UserRepository.UpdateUser(new User
                {
                    Id = staff.UserId,
                    Username = username,
                    Password = password,
                    Role = "staff"
                });

                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Staff SET Name = @name, Address = @address, Email = @email WHERE Id = @id";
                cmd.Parameters.AddWithValue("@name", staff.Name);
                cmd.Parameters.AddWithValue("@address", staff.Address);
                cmd.Parameters.AddWithValue("@email", staff.Email);
                cmd.Parameters.AddWithValue("@id", staff.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStaff(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                int userId = -1;
                var cmdSelect = conn.CreateCommand();
                cmdSelect.CommandText = "SELECT UserId FROM Staff WHERE Id = @id";
                cmdSelect.Parameters.AddWithValue("@id", id);
                var result = cmdSelect.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    userId = Convert.ToInt32(result);
                }

                var cmdDeleteStaff = conn.CreateCommand();
                cmdDeleteStaff.CommandText = "DELETE FROM Staff WHERE Id = @id";
                cmdDeleteStaff.Parameters.AddWithValue("@id", id);
                cmdDeleteStaff.ExecuteNonQuery();

                if (userId != -1)
                {
                    var cmdDeleteUser = conn.CreateCommand();
                    cmdDeleteUser.CommandText = "DELETE FROM Users WHERE Id = @userId";
                    cmdDeleteUser.Parameters.AddWithValue("@userId", userId);
                    cmdDeleteUser.ExecuteNonQuery();
                }
            }
        }

        public Staff GetStaffById(int id)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand("SELECT * FROM Staff WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Staff
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        Email = reader.GetString(3),
                        UserId = reader.GetInt32(4)
                    };
                }
            }

            return null;
        }
    }
}
