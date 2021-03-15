using System.Collections.Generic;
using LogBookTask.Abstracts;

namespace LogBookTask.Entities
{
    public class Lesson : Id
    {
        public string Name { get; set; }
        public List<Subject> Subjects { get; set; }

        public Lesson()
        {
            Subjects = new List<Subject>();
        }
    }
}