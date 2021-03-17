using System.Collections.Generic;
using LogBookTask.Abstracts;

namespace LogBookTask.Entities
{
    public class Class:Id
    {
        public string ClassName { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public Dictionary<string, string> Subjects { get; set; }

        public Class()
        {
            Teachers = new List<Teacher>();
            Students = new List<Student>();
            Subjects = new Dictionary<string, string>();
        }
    }
}