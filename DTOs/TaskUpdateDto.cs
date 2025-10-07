namespace ApiJwtEfOracle.DTOs
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public int Completed { get; set; }
    }
}
