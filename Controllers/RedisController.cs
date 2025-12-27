using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("api/redis")]
    public class RedisController : ControllerBase
    {
        private readonly IDatabase _redis;

        public RedisController(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            await _redis.StringSetAsync(key, value, TimeSpan.FromMinutes(5));
            return Ok("Saved to Redis");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetValue(string key)
        {
            var value = await _redis.StringGetAsync(key);
            if (value == RedisValue.Null)
                return NotFound($"Value not found in redis for key {key}");
            return Ok(value.ToString());
        }
    }
}
