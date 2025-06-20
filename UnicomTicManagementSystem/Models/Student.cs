﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnicomTicManagementSystem.Models
{ 
  public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; } // <-- Fixed property declaration
        public string Address { get; set; }
        public string Password { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; } // <-- Fixed typo 'tring' to 'string'
        public string Stream { get; set; }
        public int UserId { get; set; }
    }
}