using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTicManagementSystem.Models
{
    public class Mark
    {
        public int MarkID { get; set; }
        public int StudentID { get; set; }
        public string Subject { get; set; }
        public string Exam { get; set; }
        public int Score { get; set; }
    }
}
