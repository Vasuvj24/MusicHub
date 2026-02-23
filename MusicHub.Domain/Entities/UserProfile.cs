using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        private readonly List<ServiceListing> _services = new();
        public Guid UserId { get; private set; }
        public string DisplayName { get; private set; } = "";
        public string Bio { get; private set; } = "";
        public string City { get; private set; } = "";
        public string Genres { get; private set; } = ""; // simple comma string for now
        //this property gives the get of _services
        public IReadOnlyCollection<ServiceListing> Services => _services;
        //making this parameter less constructor because ef core instatiate this while getting the data and then append values on them if we don't make will throw runtime error
        private UserProfile() { }
        public UserProfile(Guid userId)
        {
            UserId = userId;
        }
        public void Update(string displayName,string bio,string city,string genres)
        {
            DisplayName = displayName?.Trim() ?? "";
            Bio = bio?.Trim() ?? "";
            City = city?.Trim() ?? "";
            Genres = genres?.Trim() ?? "";
        }
        public void AddService(ServiceListing service)
        {
            _services.Add(service);
        }
    }
}
