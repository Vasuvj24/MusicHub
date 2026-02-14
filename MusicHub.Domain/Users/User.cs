using MusicHub.Domain.Common;
using MusicHub.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Users
{
    public class User : BaseEntity
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public Role Role { get; private set; }
        private User()
        {

        }
        public User(string email,string passwordHash, Role role)
        {
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            AddDomainEvent(new UserRegisteredEvent(this.Id));
        }
        public void PromoteToAdmin()
        {
            Role = Role.Admin;
        }
    }
}
