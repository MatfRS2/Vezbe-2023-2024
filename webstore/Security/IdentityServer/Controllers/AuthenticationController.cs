using AutoMapper;
using IdentityServer.Controllers.Base;
using IdentityServer.DTOs;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController: RegistrationControllerBase
{
    public AuthenticationController(IMapper mapper, ILogger<AuthenticationController> logger, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, logger, userManager, roleManager)
    {
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterBuyer([FromBody] NewUserDto newUser)
    {
        return await RegisterNewUserWithRoles(newUser, new[] { Roles.Buyer });
    }
    
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAdministrator([FromBody] NewUserDto newUser)
    {
        return await RegisterNewUserWithRoles(newUser, new[] { Roles.Admin });
    }

}