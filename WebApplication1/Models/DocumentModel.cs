namespace WebApplication1.Models
{
    public class DocumentModel
    {
        public string DocURL { get; set; }
        public ApplicationConfigurations GetApplicationConfigurations { get; set; }
    }

    public class ApplicationConfigurations
    {
        public string? ApplicationApiUrl { get; set; }
        public string? DocumentPath { get; set; }
    }
}
