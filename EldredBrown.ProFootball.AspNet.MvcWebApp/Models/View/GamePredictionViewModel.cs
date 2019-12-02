namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.View
{
    public class GamePredictionViewModel
    {
        public int GuestSeasonID { get; set; }
        public string GuestName { get; set; }
        public int GuestScore { get; set; }
        public int HostSeasonID { get; set; }
        public string HostName { get; set; }
        public int HostScore { get; set; }
    }
}
