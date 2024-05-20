public interface IRequestValidator
{
    bool ValidateGetTask(int taskId, int userId);

    bool ValidateGetTasks(int columnId);
    
    bool GetColumnsWithTasks(int boardId, int userId);

    bool ValidateCreateTask(CreateTask createTaskRequest);

    bool ValidateUpdateTask(UpdateTask updateTaskRequest);

    bool ValidateDeleteTask(int taskId, int userId);
}

public class RequestValidator : IRequestValidator
{
    public RequestValidator()
    {
        
    }

    public bool ValidateGetTask(int taskId, int userId)
    {
        return true;
    }

    public bool ValidateGetTasks(int userId)
    {
        return true;
    }

    public bool GetColumnsWithTasks(int boardId, int userId)
    {
        return true;
    }

    public bool ValidateCreateTask(CreateTask createTaskRequest)
    {
        return true;
    }
    
    public bool ValidateUpdateTask(UpdateTask updateTaskRequest)
    {
        return true;
    }
    
    public bool ValidateDeleteTask(int taskId, int userId)
    {
        return true;
    }
}