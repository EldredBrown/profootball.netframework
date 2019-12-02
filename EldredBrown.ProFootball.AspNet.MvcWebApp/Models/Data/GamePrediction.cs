namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Models.Data
{
    /// <summary>
    /// Class to represent a game prediction
    /// </summary>
    public class GamePrediction
    {
        public int GuestSeasonID { get; set; }
        public string GuestName { get; set; }
        public int GuestScore { get; set; }
        public int HostSeasonID { get; set; }
        public string HostName { get; set; }
        public int HostScore { get; set; }

        public virtual Season Season { get; set; }
        public virtual Team Guest { get; set; }
        public virtual Team Host { get; set; }
    }
}
