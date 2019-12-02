namespace EldredBrown.ProFootball.Shared
{
    public enum Direction
	{
		Up,
		Down
	}

	public enum WinnerOrLoser
	{
		Winner,
		Loser
	}

    public struct Matchup
    {
        public Matchup(ITeamSeason guestSeason, ITeamSeason hostSeason)
        {
            GuestSeason = guestSeason;
            HostSeason = hostSeason;
        }

        public ITeamSeason GuestSeason;
        public ITeamSeason HostSeason;
    }
}
