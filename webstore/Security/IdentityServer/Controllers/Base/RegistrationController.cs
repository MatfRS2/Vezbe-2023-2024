using AutoMapper;
using IdentityServer.DTOs;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers.Base;

public class RegistrationControllerBase: ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegistrationControllerBase(IMapper mapper, ILogger<AuthenticationController> logger,
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }
    protected async Task<IActionResult> RegisterNewUserWithRoles(NewUserDto newUser, IEnumerable<string> roles)
    {
        var user = _mapper.Map<User>(newUser);

        var result = await _userManager.CreateAsync(user, newUser.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        _logger.LogInformation("Successfully registered user: {NewUser}.", user.UserName);

        foreach (var role in roles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (roleExists)
            {
                await _userManager.AddToRoleAsync(user, role);
                _logger.LogInformation("Added role {AddedRole} to user: {Username}.", role, user.UserName);
            }
            else
            {
                _logger.LogInformation("Role {NonExistingRole} does not exist.", role);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }
}