using System.Collections.Generic;

namespace LogBookTask.Entities
{
    public class LogBook
    {
        public List<Class> Classes { get; set; }

        public LogBook()
        {
            Classes = new List<Class>();
        }
    }
}