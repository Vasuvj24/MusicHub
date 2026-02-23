using MusicHub.Application.DTO.Profile;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public sealed class ProfileService
    {
        private readonly IProfileRepository _profiles;
        private readonly IUnitOfWork _uow;
        public ProfileService(IProfileRepository profiles, IUnitOfWork uow)
        {
            _profiles = profiles;
            _uow = uow;
        }
        public async Task UpdateAsync(Guid userId, UpdateProfileDto dto, CancellationToken cts)
        {
            var profile = await _profiles.GetByUserIdAsync(userId, cts);
            if (profile == null)
            {
                profile = new UserProfile(userId);
                await _profiles.AddAsync(profile, cts);
            }
            profile.Update(dto.DisplayName, dto.Bio, dto.City, dto.Genres);
            await _uow.SaveChangesAsync();
        }
        public async Task AddServiceAsync(Guid userId, AddServiceDto dto, CancellationToken ct)
        {
            var profile = await _profiles.GetByUserIdAsync(userId, ct);

            if (profile is null)
            {
                profile = new UserProfile(userId);
                await _profiles.AddAsync(profile, ct);
            }

            profile.AddService(new ServiceListing(profile.Id, dto.Title, dto.Description, dto.Price, dto.Currency));

            await _uow.SaveChangesAsync();
        }

        public async Task<ProfileResponseDto> GetAsync(Guid userId, CancellationToken ct)
        {
            var profile = await _profiles.GetByUserIdAsync(userId, ct)
                          ?? new UserProfile(userId);

            return new ProfileResponseDto
            {
                UserId = profile.UserId,
                DisplayName = profile.DisplayName,
                Bio = profile.Bio,
                City = profile.City,
                Genres = profile.Genres,
                Services = profile.Services.Select(s => new ProfileResponseDto.ServiceItem
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Price = s.Price,
                    Currency = s.Currency
                }).ToList()
            };
        }
    }
}
