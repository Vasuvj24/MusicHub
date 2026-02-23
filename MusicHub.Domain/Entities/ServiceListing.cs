using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    //services provided by users
    public class ServiceListing
    {
        public Guid Id { get; private set; }
        public Guid ProfileId { get; private set; }
        public string Title { get; private set; } = "";
        public string Description { get; private set; } = "";
        public decimal Price { get; private set; }
        public Currency Currency { get; private set; }
        public ServiceListing() { } 
        public ServiceListing(Guid profileId, string title, string description, decimal price, Currency currency)
        {
            //Id = id;
            ProfileId = profileId;
            Title = title;
            Description = description;
            Price = price;
            Currency = currency;
        }
    }
}
