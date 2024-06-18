using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunningApp.Models
{
    public class RunningSession
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; } 
        public double Kilometers { get; set; }
        public double Minutes { get; set; }
       
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public DateTime Date { get; set; }
    }
}
