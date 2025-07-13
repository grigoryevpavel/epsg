using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EPSG.API.Models;
using System.Globalization;

namespace EPSG.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldsController : ControllerBase
    {
        private const string Er_InvalidCoordinate = "Некорректные координаты. Убедитесь, что они находятся в формате EPSG:4326.";
        private readonly FieldsDbContext _db;
        public FieldsController(FieldsDbContext db)
        {
            _db = db;
        }

        [HttpGet("all")]
        public IActionResult GetAllFields()
        {
            var fields = _db.Fields.Include(f => f.Locations).ThenInclude(l => l.Polygon).ToList();
            return Ok(fields);
        }

        [HttpGet("size/{id}")]
        public IActionResult GetFieldSize(int id)
        {
            var field = _db.Fields.FirstOrDefault(f => f.Id == id);
            if (field == null) return NotFound();
            return Ok(field.Size);
        }

        [HttpGet("distance")]
        public IActionResult GetDistanceToPoint(int id, [FromQuery] double lat, [FromQuery] double lng)
        {
            if (!IsValidCoordinate(lat, lng))
            {
                return BadRequest(Er_InvalidCoordinate);
            }

            var field = _db.Fields.Include(f => f.Locations).FirstOrDefault(f => f.Id == id);
            if (field == null) return NotFound();
            var fieldCenter = field.Locations.Center;
            var distance = CalculateDistance(fieldCenter, new[] { lat, lng });
            return Ok(distance);
        }

        [HttpGet("contains")]
        public IActionResult IsPointInField([FromQuery] double lat, [FromQuery] double lng)
        {
            if (!IsValidCoordinate(lat, lng))
            {
                return BadRequest(Er_InvalidCoordinate);
            }

            var fields = _db.Fields.Include(f => f.Locations).ThenInclude(l => l.Polygon).ToList();
            foreach (var field in fields)
            {
                var polygon = field.Locations.Polygon.Select(p => new[] { p.Lat, p.Lng }).ToArray();
                if (IsPointInPolygon(lat, lng, polygon))
                {
                    return Ok(new { field.Id, field.Name });
                }
            }
            return Ok(false);
        }

        private bool IsPointInPolygon(double lat, double lng, double[][] polygon)
        {
            bool isInside = false;
            int j = polygon.Length - 1;
            for (int i = 0; i < polygon.Length; i++)
            {
                if (polygon[i][1] < lng && polygon[j][1] >= lng || polygon[j][1] < lng && polygon[i][1] >= lng)
                {
                    if (polygon[i][0] + (lng - polygon[i][1]) / (polygon[j][1] - polygon[i][1]) * (polygon[j][0] - polygon[i][0]) < lat)
                    {
                        isInside = !isInside;
                    }
                }
                j = i;
            }
            return isInside;
        }

        private double CalculateDistance(double[] point1, double[] point2)
        {
            var R = 6371000; // Radius of Earth in meters
            var lat1 = DegreesToRadians(point1[0]);
            var lat2 = DegreesToRadians(point2[0]);
            var deltaLat = DegreesToRadians(point2[0] - point1[0]);
            var deltaLng = DegreesToRadians(point2[1] - point1[1]);
            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLng / 2) * Math.Sin(deltaLng / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double DegreesToRadians(double degrees) => degrees * Math.PI / 180;

        private bool IsValidCoordinate(double lat, double lng)
        {
            return lat >= -90 && lat <= 90 && lng >= -180 && lng <= 180;
        }
    }
}