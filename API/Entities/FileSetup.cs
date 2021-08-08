using System.Collections.Generic;

namespace API.Entities
{
    public class FileSetup
    {
        public int Id { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string adminEmail { get; set; }

        public ICollection<AllowedFileTypes> allowedFileTypes { get; set; }
        
    }
}