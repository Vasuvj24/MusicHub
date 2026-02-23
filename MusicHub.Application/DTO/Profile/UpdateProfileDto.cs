using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Profile
{
    //for displaying the userprofile to the other users public profile
        public sealed class UpdateProfileDto
        {
            public string DisplayName { get; set; } = "";
            public string Bio { get; set; } = "";
            public string City { get; set; } = "";
            public string Genres { get; set; } = "";
        }
}
