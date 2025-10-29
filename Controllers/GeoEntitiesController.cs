using MapProject.Models;
using MapProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MapProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoEntitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GeoEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tüm geometrileri getir
        [HttpGet]
        public async Task<ActionResult> GetGeoEntities()
        {
            try
            {
                var entities = await _context.GeoEntities
                    .OrderByDescending(g => g.CreatedAt)
                    .ToListAsync();

                Console.WriteLine($"✅ GET: {entities.Count} geometri döndürüldü");
                return Ok(entities);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GET hatası: {ex.Message}");
                return StatusCode(500, new { error = "Veri getirme hatası: " + ex.Message });
            }
        }

        // POST: Yeni geometri ekle
        [HttpPost]
        public async Task<ActionResult> PostGeoEntity([FromBody] GeoEntityDto dto)
        {
            try
            {
                Console.WriteLine("=== YENİ KAYIT İSTEĞİ ===");
                Console.WriteLine($"Name: '{dto.Name}'");
                Console.WriteLine($"Description: '{dto.Description}'");
                Console.WriteLine($"GeoJson: '{dto.GeoJson}'");
                Console.WriteLine($"GeoJson null mu: {dto.GeoJson == null}");
                Console.WriteLine($"GeoJson empty mi: {string.IsNullOrEmpty(dto.GeoJson)}");
                Console.WriteLine($"GeoJson length: {dto.GeoJson?.Length ?? 0}");
                Console.WriteLine("========================");

                // Gelen veriyi kontrol et
                if (dto == null)
                {
                    Console.WriteLine("❌ DTO null!");
                    return BadRequest(new { error = "Geçersiz veri" });
                }

                if (string.IsNullOrEmpty(dto.GeoJson))
                {
                    Console.WriteLine("❌ GeoJSON boş!");
                    return BadRequest(new { error = "GeoJSON boş olamaz" });
                }

                // GeoJSON'u parse etmeye çalış
                string geometryType = "Unknown";
                try
                {
                    using var jsonDoc = JsonDocument.Parse(dto.GeoJson);
                    var root = jsonDoc.RootElement;
                    
                    if (root.TryGetProperty("type", out var typeElement))
                    {
                        geometryType = typeElement.GetString() ?? "Unknown";
                        Console.WriteLine($"✅ Geometry Type: {geometryType}");
                    }
                    else
                    {
                        Console.WriteLine("❌ GeoJSON'da 'type' property bulunamadı");
                        return BadRequest(new { error = "GeoJSON'da 'type' property bulunamadı" });
                    }

                    // Coordinates kontrolü
                    if (!root.TryGetProperty("coordinates", out var coordinatesElement))
                    {
                        Console.WriteLine("❌ GeoJSON'da 'coordinates' property bulunamadı");
                        return BadRequest(new { error = "GeoJSON'da 'coordinates' property bulunamadı" });
                    }

                    Console.WriteLine($"✅ Coordinates bulundu: {coordinatesElement}");
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"❌ JSON parse hatası: {jsonEx.Message}");
                    return BadRequest(new { error = $"Geçersiz JSON formatı: {jsonEx.Message}" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ GeoJSON işleme hatası: {ex.Message}");
                    return BadRequest(new { error = $"GeoJSON işleme hatası: {ex.Message}" });
                }

                // GeoEntity oluştur
                var geoEntity = new GeoEntity
                {
                    Name = string.IsNullOrEmpty(dto.Name) ? "İsimsiz Şekil" : dto.Name,
                    Description = dto.Description,
                    GeometryType = geometryType,
                    GeoJson = dto.GeoJson,
                    Color = dto.Color ?? GetDefaultColor(geometryType),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.GeoEntities.Add(geoEntity);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ KAYIT BAŞARILI - ID: {geoEntity.Id}, Type: {geoEntity.GeometryType}");

                return Ok(new
                {
                    id = geoEntity.Id,
                    name = geoEntity.Name,
                    description = geoEntity.Description,
                    geometryType = geoEntity.GeometryType,
                    geoJson = geoEntity.GeoJson,
                    color = geoEntity.Color,
                    createdAt = geoEntity.CreatedAt
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ KAYIT HATASI: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return StatusCode(500, new { error = "Kayıt hatası: " + ex.Message });
            }
        }

        // DELETE: Geometri sil
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGeoEntity(int id)
        {
            try
            {
                var geoEntity = await _context.GeoEntities.FindAsync(id);
                if (geoEntity == null)
                {
                    return NotFound(new { error = "Geometri bulunamadı" });
                }

                _context.GeoEntities.Remove(geoEntity);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Geometri silindi", id = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Silme hatası: " + ex.Message });
            }
        }

        private string GetDefaultColor(string geometryType)
        {
            return geometryType.ToLower() switch
            {
                "point" => "#ff0000",
                "linestring" => "#0000ff", 
                "polygon" => "#00ff00",
                _ => "#3388ff"
            };
        }
    }

    public class GeoEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string GeoJson { get; set; } = string.Empty;
        public string? Color { get; set; }
    }
}
