using System.Data.Entity;

namespace TasksService.Models
{
    public class TasksServiceContext : DbContext
    {
        public TasksServiceContext() : base("name=TasksServiceContext")
        {
        }

        public DbSet<ProcessTask> ProcessTasks { get; set; }
    }
}