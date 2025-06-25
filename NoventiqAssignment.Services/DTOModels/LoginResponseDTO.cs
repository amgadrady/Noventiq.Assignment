using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoventiqAssignment.Services.DTOModels
{
    public class LoginResponseDTO
    {

        public string userId { get; set; }=string.Empty;
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}
