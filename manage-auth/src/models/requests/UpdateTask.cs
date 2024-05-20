public class UpdateTask
{
    public UpdateTask()
    {
        
    }

    public int UserId { get; set; }
    
    public int TaskId { get; set; }
    
    public int ColumnId { get; set; }

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }
}