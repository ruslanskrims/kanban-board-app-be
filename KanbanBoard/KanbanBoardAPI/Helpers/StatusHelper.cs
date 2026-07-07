using KanbanBoardAPI.Models;

namespace KanbanBoardAPI.Helpers
{
    public class StatusHelper
    {
        public static bool hasValidTaskStatus(int statusValue)
        {
            return Enum.IsDefined(typeof(KanbanTaskStatus), statusValue);
        }
    }
}
