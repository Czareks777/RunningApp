using System.ComponentModel.DataAnnotations.Schema;

namespace RunningApp.Models
{
    public class RunningSession
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public string UserId {  get; set; }
        public string Name { get; set; }
        public static double Kilometers { get; set; }
        public static double Minutes { get; set; }
        public double Tempo = GetTempo(Kilometers, Minutes);
        public string Description { get; set; }
        public  string Image { get; set; }
        public DateTime Date { get; set; }

        public static double GetTempo(double Kilometers, double Minutes)
        {
            return Minutes / Kilometers;
        }
    }
}
