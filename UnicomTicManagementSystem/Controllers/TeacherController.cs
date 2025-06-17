using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTicManagementSystem.Controllers
{
    internal class TeacherController
    {
        private readonly TeacherService _teacherService;

        public TeacherController()
        {
            _teacherService = new TeacherService();
        }

        public List<Teacher> GetAllTeachers() => _teacherService.GetAll();

        public void AddTeacher(Teacher teacher) => _teacherService.Add(teacher);

        public void UpdateTeacher(Teacher teacher) => _teacherService.Update(teacher);

        public void DeleteTeacher(int teacherId) => _teacherService.Delete(teacherId);

        public void AddUser(string username, string password, string role)
        {
            _teacherService.AddUser(username, password, role);
        }

        public List<Teacher> SearchTeachers(string keyword)
        {
            return _teacherService.SearchTeachers(keyword);
        }

        public List<Section> GetAllSections() => _teacherService.GetAllSections();

        public int AddTeacherWithReturnId(Teacher teacher) => _teacherService.AddWithReturnId(teacher);

        public void AssignSectionToTeacher(int teacherId, int sectionId) => _teacherService.AssignSectionToTeacher(teacherId, sectionId);

    }

}
