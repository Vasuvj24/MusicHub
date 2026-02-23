using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Profile
{
    //returning the profile to the users - privacy to display user whos profile it belongs to
    public sealed class ProfileResponseDto
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = "";
        public string Bio { get; set; } = "";
        public string City { get; set; } = "";
        public string Genres { get; set; } = "";

        public List<ServiceItem> Services { get; set; } = new();

        public sealed class ServiceItem
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public decimal Price { get; set; }
            public Currency Currency { get; set; }
        }
    }
}
