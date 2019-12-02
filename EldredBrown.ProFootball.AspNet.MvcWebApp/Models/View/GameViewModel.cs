using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class GameViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "A season is required.")]
        [DisplayName("Season")]
        public int SeasonID { get; set; }

        [Required(ErrorMessage = "A week is required.")]
        public int Week { get; set; }

        [Required(ErrorMessage = "The guest is required.")]
        [DisplayName("Guest")]
        [CompareForInequality("HostName")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "The guest's score is required.")]
        [DisplayName("Guest Score")]
        public double GuestScore { get; set; }

        [Required(ErrorMessage = "The host is required.")]
        [DisplayName("Host")]
        [CompareForInequality("GuestName")]
        public string HostName { get; set; }

        [Required(ErrorMessage = "The host's score is required.")]
        [DisplayName("Host Score")]
        public double HostScore { get; set; }

        [DisplayName("Playoff Game?")]
        public bool IsPlayoffGame { get; set; }

        public string Notes { get; set; }
    }
}
