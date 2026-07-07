using System.ComponentModel.DataAnnotations;

public class CreateTaskDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 3)]
    public int Status { get; set; }
}