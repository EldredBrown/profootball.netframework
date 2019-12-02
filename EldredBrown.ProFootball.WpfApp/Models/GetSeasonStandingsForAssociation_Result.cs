namespace EldredBrown.ProFootball.WpfApp.Models
{
    /// <summary>
    /// Abstract class to allow the GetSeasonStandingsForConference_Result and 
    /// GetSeasonStandingsForDivision_Result to be used similarly
    /// </summary>
    public abstract class GetSeasonStandingsForAssociation_Result
    {
    }

    public partial class GetSeasonStandingsForLeague_Result : GetSeasonStandingsForAssociation_Result
    {
    }

    public partial class GetSeasonStandingsForConference_Result : GetSeasonStandingsForAssociation_Result
    {
    }

    public partial class GetSeasonStandingsForDivision_Result : GetSeasonStandingsForAssociation_Result
    {
    }
}