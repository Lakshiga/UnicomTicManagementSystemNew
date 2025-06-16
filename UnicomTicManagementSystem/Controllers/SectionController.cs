using UnicomTicManagementSystem.Data;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTicManagementSystem.Controllers
{
    internal class SectionController
    {
        //private readonly SectionService _sectionService;

        public SectionController()
        {
            //_sectionService = new SectionService();
        }

        //public List<Section> GetAllSections() => _sectionService.GetAll();

        //public void AddSection(Section section) => _sectionService.Add(section);

        //public void UpdateSection(Section section) => _sectionService.Update(section);

        //public void DeleteSection(int sectionId) => _sectionService.Delete(sectionId);

        public List<Section> GetAllSections()
        {
            var sections = new List<Section>();

            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand("SELECT * FROM Sections", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sections.Add(new Section
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }

            return sections;
        }

        public void AddSection(Section section)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand("INSERT INTO Sections (Name) VALUES (@Name)", conn);
                cmd.Parameters.AddWithValue("@Name", section.Name);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSection(Section section)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand("UPDATE Sections SET Name = @Name WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Name", section.Name);
                cmd.Parameters.AddWithValue("@Id", section.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSection(int sectionId)
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = new SQLiteCommand("DELETE FROM Sections WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", sectionId);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
