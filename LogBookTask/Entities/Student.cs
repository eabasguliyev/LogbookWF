using System;
using System.Collections.Generic;
using LogBookTask.Enums;

namespace LogBookTask.Entities
{
    public class Student : User
    {
        public Dictionary<string, RecordType> Records { get; set; }
        public Dictionary<string, int> AssignmentPoints { get; set; }
        public Dictionary<string, int> ClassWorkPoints { get; set; }
        public int Diamonds { get; set; }
        public int Coins { get; set; }

        public Dictionary<string, string> Comments { get; set; }


        public Student()
        {
            Records = new Dictionary<string, RecordType>();
            AssignmentPoints = new Dictionary<string, int>();
            ClassWorkPoints = new Dictionary<string, int>();
            Comments = new Dictionary<string, string>();
        }
    }
}