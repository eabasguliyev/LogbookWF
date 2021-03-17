using System;
using System.Collections.Generic;
using LogBookTask.Abstracts;
using LogBookTask.Enums;

namespace LogBookTask.Entities
{
    public class Lesson:Id
    {
        public DateTime Date { get; set; }
        public TeacherType TeacherType { get; set; }
        public Teacher Teacher { get; set; }
        public List<StudentRecord> StudentRecords { get; set; }
        public string Subject { get; set; }
        public int TotalDiamondCount { get; set; }
        public Lesson()
        {
            StudentRecords = new List<StudentRecord>();
            TotalDiamondCount = 5;
        }
    }
}