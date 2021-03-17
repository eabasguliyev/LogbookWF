using LogBookTask.Enums;

namespace LogBookTask.Entities
{
    public class StudentRecord
    {
        public Student Student { get; set; }
        public RecordType RecordType { get; set; }
        public int AssignmentPoint { get; set; }
        public int ClassWorkPoint { get; set; }
        public int Diamonds { get; set; }

        public string Comment { get; set; }
    }
}