namespace NoNicotineAPI.Models
{
    public class PatientHabit
    {
        public int PatientHabitId { get; set; }
        public DateTime Hour { get; set; }
        public string Days { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public int HabitId { get; set; }
        public Habit Habit { get; set; }
    }
}
