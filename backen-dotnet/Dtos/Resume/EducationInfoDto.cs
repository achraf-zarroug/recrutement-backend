namespace backen_dotnet.Dtos.Resume
{
    public class EducationInfoDto
    {
        public string SchoolName { get; set; }
        public string SchoolLocation { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndMonth { get; set; }
        public DateTime EndYear { get; set; }
        public string Degree { get; set; }
        public string FieldofStudy { get; set; }
    }
}
