using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.Controllers
{
    internal class StudentTeacherController
    {
        private readonly StudentTeacherService _service;

        public StudentTeacherController()
        {
            _service = new StudentTeacherService();
        }

        public void AssignTeacherToStudent(int studentId, int teacherId)
            => _service.AssignTeacherToStudent(studentId, teacherId);

        public void RemoveTeacherFromStudent(int studentId, int teacherId)
            => _service.RemoveTeacherFromStudent(studentId, teacherId);

        public List<Teacher> GetTeachersForStudent(int studentId)
            => _service.GetTeachersForStudent(studentId);
    }
}
