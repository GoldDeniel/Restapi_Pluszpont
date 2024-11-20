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
    public class BetterUserController : ControllerBase
    {
        private readonly BetterUserService _BetterUserService;
        public BetterUserController(BetterUserService BetterUserService)
        {
            _BetterUserService = BetterUserService;
        }
        [HttpGet]
        public async Task<ActionResult<List<BetterUser>>> Get()
        {
            return await _BetterUserService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BetterUser>> Get(string id)
        {
            var BetterUser = await _BetterUserService.GetAsync(id);

            if (BetterUser is null)
            {
                return NotFound();
            }

            return BetterUser;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BetterUser newBetterUser)
        {
            await _BetterUserService.CreateAsync(newBetterUser);

            return CreatedAtAction(nameof(Get), new { id = newBetterUser.Id }, newBetterUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, BetterUser updatedBetterUser)
        {
            var BetterUser = await _BetterUserService.GetAsync(id);

            if (BetterUser is null)
            {
                return NotFound();
            }

            updatedBetterUser.Id = BetterUser.Id;

            await _BetterUserService.UpdateAsync(id, updatedBetterUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var BetterUser = await _BetterUserService.GetAsync(id);

            if (BetterUser is null)
            {
                return NotFound();
            }

            await _BetterUserService.RemoveAsync(id);

            return NoContent();
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin BetterUserLogin)
        {
            var BetterUsers = await _BetterUserService.GetAsync();
            var BetterUser = BetterUsers.FirstOrDefault(u => u.Name == BetterUserLogin.Name);

            if (BetterUser == null)
            {
                return NotFound("BetterUser not found");
            }

            // var hashedSecret = HashSecret(BetterUserLogin.Secret);
            // if (BetterUser.Secret != hashedSecret)
            // {
            //     return Unauthorized("Invalid secret");
            // }

            if (BetterUser.Secret != BetterUserLogin.Secret)
            {
                return Unauthorized("Invalid secret");
            }

            return Ok(BetterUser);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(BetterUser BetterUser)
        {
            //BetterUser.Secret = HashSecret(BetterUser.Secret!);
            
            if (BetterUser.Name == null)
            {
                return BadRequest("Name is required");
            }

            if (BetterUser.Secret == null)
            {
                return BadRequest("Secret is required");
            }

           
            var BetterUsers = await _BetterUserService.GetAsync();
            var existingBetterUser = BetterUsers.FirstOrDefault(u => u.Name == BetterUser.Name);
            
            if (existingBetterUser != null)
            {
                return Conflict("User already exists");
            }

            await _BetterUserService.CreateAsync(BetterUser);

            return CreatedAtAction(nameof(Get), new { id = BetterUser.Id }, BetterUser);
        }
        
        [HttpGet]
        [Route("FriendIds/{id}")]
        public async Task<ActionResult<List<BetterUser>>> GetFriendIds(string id)
        {
            var BetterUser = await _BetterUserService.GetAsync(id);

            if (BetterUser == null)
            {
                return NotFound();
            }

            return Ok(BetterUser.FriendIds);
        }
        [HttpPost]
        [Route("addfriend/{id}/{friendId}")]
        public async Task<IActionResult> AddFriend(string id, string friendId)
        {
            var user = await _BetterUserService.GetAsync(id);
            var friend = await _BetterUserService.GetAsync(friendId);

            if (user?.FriendIds != null && user.FriendIds.Contains(friendId))
            {
                return NoContent();
            }

            if (user == null || friend == null)
            {
                return NotFound();
            }

            if (user.FriendIds == null)
            {
                user.FriendIds = new List<string> { friendId };
            }
            else
            {
                user.FriendIds.Add(friendId);
            }



            await _BetterUserService.UpdateAsync(id, user);

            return NoContent();
        }
        [HttpPost]
        [Route("removefriend/{id}/{friendId}")]
        public async Task<IActionResult> RemoveFriend(string id, string friendId)
        {
            var user = await _BetterUserService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.FriendIds == null)
            {
                return NoContent();
            }

            user.FriendIds.Remove(friendId);

            await _BetterUserService.UpdateAsync(id, user);

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