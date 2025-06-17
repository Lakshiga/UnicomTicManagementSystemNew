using System.Data;
using UnicomTicManagementSystem.Repositories;

namespace UnicomTicManagementSystem.Controllers
{
    public class MarkController
    {
        private MarkRepository repository = new MarkRepository();

        public DataTable GetAllMarks() => repository.GetAllMarks();
        public void AddMark(int studentId, string subject, string exam, int score) => repository.AddMark(studentId, subject, exam, score);
        public void DeleteMark(int markId) => repository.DeleteMark(markId);
        public void UpdateMark(int markId, int studentId, string subject, string exam, int score) => repository.UpdateMark(markId, studentId, subject, exam, score);
        public string GetStudentName(int studentId) => repository.GetStudentName(studentId);
        public DataTable GetSubjectsByStudent(int studentId) => repository.GetSubjectsByStudent(studentId);
        public DataTable GetExams() => repository.GetAllExams();
    }
}
