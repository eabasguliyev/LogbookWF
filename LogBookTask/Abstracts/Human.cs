using System;

namespace LogBookTask.Abstracts
{
    public abstract class Human : Id
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}