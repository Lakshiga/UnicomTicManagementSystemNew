using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;

namespace UnicomTicManagementSystem.Controllers
{
    public class RoomController
    {
        public List<Room> GetAllRooms()
        {
            var roomList = new List<Room>();

            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT RoomID, RoomName, RoomType FROM Rooms";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roomList.Add(new Room
                        {
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            RoomName = reader["RoomName"].ToString(),
                            RoomType = reader["RoomType"].ToString()
                        });
                    }
                }
            }

            return roomList;
        }

        public void AddRoom(Room room)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Rooms (RoomName, RoomType) VALUES (@name, @type)";
                cmd.Parameters.AddWithValue("@name", room.RoomName);
                cmd.Parameters.AddWithValue("@type", room.RoomType);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateRoom(Room room)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Rooms SET RoomName = @name, RoomType = @type WHERE RoomID = @id";
                cmd.Parameters.AddWithValue("@name", room.RoomName);
                cmd.Parameters.AddWithValue("@type", room.RoomType);
                cmd.Parameters.AddWithValue("@id", room.RoomID);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteRoom(int roomId)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Rooms WHERE RoomID = @id";
                cmd.Parameters.AddWithValue("@id", roomId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
