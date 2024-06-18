using System;
using Microsoft.AspNetCore.Http;

namespace RunningApp.Controllers
{
    public class RunningSessionDTO
    {
        public double Kilometers { get; set; }
        public double Minutes { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public double Time { get; set; }
        public DateTime Date { get; set; }
       
        public string UserId { get; set; }

    }
}
