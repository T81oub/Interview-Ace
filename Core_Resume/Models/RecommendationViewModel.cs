namespace Core_Resume.Models
{
    public class RecommendationViewModel
    {
        public Registration Registration { get; set; }
        public Persnola Personal { get; set; }
        public Educational Education { get; set; }
        public WorkHistory WorkHistory { get; set; }
        public Summry_CarrerObjective Summry { get; set; }
        public IEnumerable<Skills> Skills { get; set; }
        public IEnumerable<ProjectDetails> Projects { get; set; }
        public IEnumerable<LanAndHob> Languages { get; set; }
        public IEnumerable<string> JobCompanies { get; set; }
        public IEnumerable<string> JobTitles { get; set; }
        public IEnumerable<string> JobDescriptions { get; set; }
    }
}
