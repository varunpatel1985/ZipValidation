using System.Collections.Generic;

namespace API.DTO
{
    public class FileSetupDto
    {
         public string source { get; set; }
        public string target { get; set; }
        public string adminEmail { get; set; }

        public ICollection<AllowedFileTypeDto> allowedFileTypes { get; set; }
    }
}