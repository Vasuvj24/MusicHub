using MusicHub.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
    }
}
