namespace d2o_backend.Models
{
    public class AddTaskDto
    {
        public required string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
