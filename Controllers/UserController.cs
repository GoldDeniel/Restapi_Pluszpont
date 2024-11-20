using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restapi_Pluszpont.Models;
using Restapi_Pluszpont.Services;
using System.Security.Cryptography;
using System.Text;


namespace Restapi_Pluszpont.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await _userService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User newUser)
        {
            await _userService.CreateAsync(newUser);

            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            updatedUser.Id = user.Id;

            await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            await _userService.RemoveAsync(id);

            return NoContent();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var users = await _userService.GetAsync();
            var user = users.FirstOrDefault(u => u.Name == userLogin.Name);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // var hashedSecret = HashSecret(userLogin.Secret);
            // if (user.Secret != hashedSecret)
            // {
            //     return Unauthorized("Invalid secret");
            // }

            if (user.Secret != userLogin.Secret)
            {
                return Unauthorized("Invalid secret");
            }

            return Ok(user);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            //user.Secret = HashSecret(user.Secret!);
            
            if (user.Name == null)
            {
                return BadRequest("Name is required");
            }

            if (user.Secret == null)
            {
                return BadRequest("Secret is required");
            }

            await _userService.CreateAsync(user);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }
        [HttpGet]
        [Route("friends/{id}")]
        public async Task<ActionResult<List<BetterUser>>> GetFriends(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Friends);
        }
        [HttpPost]
        [Route("addfriend/{id}/{friendId}")]
        public async Task<IActionResult> AddFriend(string id, string friendId)
        {
            var user = await _userService.GetAsync(id);
            var friend = await _userService.GetAsync(friendId);

            if (user == null || friend == null)
            {
                return NotFound();
            }

            if (user.Friends == null)
            {
                user.Friends = new User[] { friend };
            }
            else
            {
                user.Friends = user.Friends.Append(friend).ToArray();
            }

            await _userService.UpdateAsync(id, user);

            return NoContent();
        }
        [HttpPost]
        [Route("removefriend/{id}/{friendId}")]
        public async Task<IActionResult> RemoveFriend(string id, string friendId)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Friends == null)
            {
                return NoContent();
            }

            user.Friends = user.Friends.Where(f => f.Id != friendId).ToArray();

            await _userService.UpdateAsync(id, user);

            return NoContent();
        }

        private string HashSecret(string secret)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(secret));
            var hashedSecret = BitConverter.ToString(bytes).Replace("-", "").ToLower();

            return hashedSecret;
        }
    }

}