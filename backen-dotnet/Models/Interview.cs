namespace backen_dotnet.Models
{
    public class Interview
{
    public int InterviewId { get; set; }  
    public string AppUserId { get; set; }  
    public virtual AppUser AppUser { get; set; }  

    public int OffreId { get; set; }  
    public virtual Offre Offre { get; set; }  

    public DateTime AppointmentDateTime { get; set; }  

    public string Status { get; set; }
    public string? Link { get; set; }
}
}
