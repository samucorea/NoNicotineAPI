
namespace NoNicotine_Data.Entities
{
    public class Habit : BaseEntity
    {
        public string Name { get; set; }

        public List<PatientHabit> PatientHabits { get; set; }
    }
}
