namespace MicroBlog.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            // если нет ролей, то создаём
            var isRolesExists = await _roleManager.Roles.AnyAsync();
            if (!isRolesExists)
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });

                await _roleManager.CreateAsync(new Role
                {
                    Name = "Author",
                    NormalizedName = "AUTHOR"
                });

                await _roleManager.CreateAsync(new Role
                {
                    Name = "User",
                    NormalizedName = "USER"
                });
            }

            var isUsersExists = await _userManager.Users.AnyAsync();

            //TODO: проверка по email и LockoutEnabled = false после проверки, сейчас true
            //TODO: изменить потом в IdentityConfigurations => options.Lockout.AllowedForNewUsers = false;
            //TODO: тут убрать EmailConfirmed = true
            var user = new User { UserName = userForRegistration.Email, Email = userForRegistration.Email, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ErrorMessage.Create(errors));
            }

            // первый пользователь будет админом
            if (!isUsersExists)
            {
                result = await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                result = await _userManager.AddToRoleAsync(user, "User");
            }

            if (!result.Succeeded)
            {
                // есть вероятность, что пользователь будет создан, но не добавлен к роли
                // и будет ошибка, но пользователь будет создан
                // значит удалить пользователя
                await _userManager.DeleteAsync(user);
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ErrorMessage.Create(errors));
            }

            // Send Email

            //TODO: пока сразу логиним, потом логику изменим
            return await Login(new UserForAuthDto
            {
                Email = userForRegistration.Email,
                Password = userForRegistration.Password
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserForAuthDto userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);

            // пользователь не найден
            if (user == null)
            {
                return Unauthorized(ErrorMessage.Create(Strings.InvalidAuthentication));
            }

            // подтверждение почты
            if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return Unauthorized(ErrorMessage.Create(Strings.EmailNotConfirmed));
            }

            // пользователь заблокирован
            if (await _userManager.IsLockedOutAsync(user))
            {
                return Unauthorized(ErrorMessage.Create(Strings.LockoutEnabled));
            }

            // неверный пароль
            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                await _userManager.AccessFailedAsync(user);
                return Unauthorized(ErrorMessage.Create(Strings.InvalidAuthentication));
            }

            // если всё успешно, генерируем токен
            var token = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenService.RefreshTokenExpiration);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _userManager.UpdateAsync(user);
            await _userManager.ResetAccessFailedCountAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var user = await GetUserByClaims();
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
            }

            return Ok();
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto tokenDto)
        {
            // открытый вызов, доп информацию о неудачах обновления токена не даём!

            ClaimsPrincipal? principal;
            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.Token);
            }
            catch
            {
                return Unauthorized();
            }

            var username = principal?.Identity?.Name ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return Unauthorized();
            }

            var isRefreshTokenValid = user.RefreshTokenExpiryTime > DateTime.UtcNow;

            if (!isRefreshTokenValid)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);

                return Unauthorized();
            }

            var token = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_tokenService.RefreshTokenExpiration);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _userManager.UpdateAsync(user);
            await _userManager.ResetAccessFailedCountAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto model)
        {
            var user = await GetUserByClaims();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ErrorMessage.Create(errors));
            }

            return NotFound();
        }

        private Task<User?> GetUserByClaims()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            return _userManager.FindByEmailAsync(email);
        }
    }
}
