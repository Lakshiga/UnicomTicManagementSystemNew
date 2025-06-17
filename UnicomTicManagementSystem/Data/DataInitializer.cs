using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTicManagementSystem.Data;

namespace UnicomTicManagementSystem.Data
{
    public static class DatabaseInitializer
    {
        public static void CreateTables()
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd = conn.CreateCommand();

                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        Role TEXT NOT NULL
                    );   

                    CREATE TABLE IF NOT EXISTS Staff (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Address TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        UserId INTEGER,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );


                    CREATE TABLE IF NOT EXISTS Sections (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Teachers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Phone TEXT NOT NULL,
                        Address TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Students (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Address TEXT NOT NULL,
                        SectionId INTEGER,
                        UserId INTEGER,
                        FOREIGN KEY (SectionId) REFERENCES Sections(Id),
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );

                    CREATE TABLE IF NOT EXISTS StudentTeacher (
                        StudentId INTEGER,
                        TeacherId INTEGER,
                        PRIMARY KEY (StudentId, TeacherId),
                        FOREIGN KEY (StudentId) REFERENCES Students(Id),
                        FOREIGN KEY (TeacherId) REFERENCES Teachers(Id)
                    );

                    CREATE TABLE IF NOT EXISTS TeacherSection (
                        TeacherId INTEGER,
                        SectionId INTEGER,
                        PRIMARY KEY (TeacherId, SectionId),
                        FOREIGN KEY (TeacherId) REFERENCES Teachers(Id),
                        FOREIGN KEY (SectionId) REFERENCES Sections(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Subjects (
                        SubjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                        SubjectName TEXT NOT NULL,
                        SectionID INTEGER NOT NULL,
                        FOREIGN KEY (SectionID) REFERENCES Sections(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Timetable (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Subject TEXT NOT NULL,
                        TimeSlot TEXT NOT NULL,
                        Room TEXT NOT NULL,
                        Date TEXT NOT NULL  -- or use DATE type
                        );

                    
                    CREATE TABLE IF NOT EXISTS ManageExam (
                        ExamID INTEGER PRIMARY KEY AUTOINCREMENT,
                        SubjectID INTEGER NOT NULL,
                        ExamName TEXT NOT NULL,
                        FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID)
                    );
                     
                    CREATE TABLE IF NOT EXISTS Rooms (
                        RoomID INTEGER PRIMARY KEY AUTOINCREMENT,
                        RoomName TEXT NOT NULL,
                        RoomType TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Marks (
                        MarkID INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentID INTEGER NOT NULL,
                        Subject TEXT NOT NULL,
                        Exam TEXT NOT NULL,
                        Score INTEGER NOT NULL,
                        FOREIGN KEY(StudentID) REFERENCES Students(StudentID)
                    );

                ";

                cmd.ExecuteNonQuery();
            }
        }

    }
}
