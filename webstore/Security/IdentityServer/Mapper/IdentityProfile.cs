using IdentityServer.DTOs;
using IdentityServer.Entities;
using AutoMapper;

namespace IdentityServer.Mapper;

public class IdentityProfile: Profile
{
    public IdentityProfile()
    {
        CreateMap<User, NewUserDto>().ReverseMap();
    }
}