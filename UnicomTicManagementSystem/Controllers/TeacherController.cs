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
    }
}
