using System.ComponentModel.DataAnnotations;

namespace MapProject.Models
{
    public class GeoEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public string GeometryType { get; set; } = string.Empty; // Point, LineString, Polygon
        
        [Required]
        public string GeoJson { get; set; } = string.Empty;
        
        public string Color { get; set; } = "#3388ff";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
