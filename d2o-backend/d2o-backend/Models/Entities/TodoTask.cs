namespace d2o_backend.Models.Entities
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
