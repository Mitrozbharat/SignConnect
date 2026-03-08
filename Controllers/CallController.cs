using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartVideoCallApp.Data;
using SmartVideoCallApp.Data.Entities;
using System.Text.Json;

namespace SmartVideoCallApp.Controllers
{
    public class CallController : Controller
    {
        private readonly AppSignDbContext _dbContext;

        public CallController(AppSignDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TrainModel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveSignCoordinate([FromBody] SaveSignCoordinateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Label))
            {
                return BadRequest(new { message = "Label is required." });
            }

            if (request.Coordinates.ValueKind == JsonValueKind.Undefined || request.Coordinates.ValueKind == JsonValueKind.Null)
            {
                return BadRequest(new { message = "Coordinates are required." });
            }

            var entity = new SignCoordinate
            {
                Label = request.Label.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                TimeId = request.TimeId ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                CoordinatesJson = request.Coordinates.GetRawText(),
                CreatedAtUtc = DateTime.UtcNow
            };

            _dbContext.SignCoordinates.Add(entity);
            await _dbContext.SaveChangesAsync();

            return Ok(new { id = entity.Id, message = "Coordinate saved." });
        }

        [HttpGet]
        public async Task<IActionResult> GetSignCoordinates()
        {
            var rows = await _dbContext.SignCoordinates
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new
                {
                    x.Id,
                    x.Label,
                    x.Description,
                    x.TimeId,
                    x.CoordinatesJson
                })
                .ToListAsync();

            return Ok(rows);
        }

        public class SaveSignCoordinateRequest
        {
            public string Label { get; set; } = string.Empty;
            public string? Description { get; set; }
            public long? TimeId { get; set; }
            public JsonElement Coordinates { get; set; }
        }
    }
}
