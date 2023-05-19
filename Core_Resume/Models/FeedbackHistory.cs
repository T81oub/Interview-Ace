namespace Core_Resume.Models
{
	public class FeedbackHistory
	{
		public string? Question { get; set; }
		public string? Answer { get; set; }
		public string? Feedback { get; set; }
		public string? Improvement { get; set; }
        public List<Emotion> Emotions { get; set; }
    }
}
