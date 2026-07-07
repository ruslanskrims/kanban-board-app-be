using System.ComponentModel.DataAnnotations;

namespace KanbanBoardAPI.DTO
{
    public class MoveTaskDto
    {
        [Required]
        [Range(1, 3)]
        public int Status { get; set; }
    }
}