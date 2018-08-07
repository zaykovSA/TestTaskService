using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TasksService.Models;

namespace TasksService.Controllers
{
    [RoutePrefix("task")]
    public class ProcessTasksController : ApiController
    {
        private TasksServiceContext db = new TasksServiceContext();

        [Route("")]
        [ResponseType(typeof(Guid))]
        public async Task<IHttpActionResult> CreateTask()
        {
            var status = "created";
            var time = DateTime.UtcNow;

            var added = db.ProcessTasks.Add(new ProcessTask() { Status = status, StatusChangeDate = time });
            await db.SaveChangesAsync();

            AsyncItemUpdate(added.TaskId).ContinueWith(AsyncItemUpdateFailed, TaskContinuationOptions.OnlyOnFaulted);

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted, added.TaskId));
        }

        [Route("{id}")]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> GetProcessTask(Guid id)
        {
            var processTask = await db.ProcessTasks
                .Where(t => t.TaskId == id)
                .FirstOrDefaultAsync();
            if (processTask == null)
                return NotFound();

            return Ok(new { status = processTask.Status, timestamp = processTask.StatusChangeDate.ToString("o") });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task AsyncItemUpdate(Guid taskId)
        {
            await Task.Run(() => {
                using (var dbContext = new TasksServiceContext())
                {
                    var processTask = dbContext.ProcessTasks
                        .Where(t => t.TaskId == taskId)
                        .FirstOrDefault();
                    processTask.StatusChangeDate = DateTime.Now;
                    processTask.Status = "running";
                    dbContext.SaveChanges();
                    Task.Delay(new TimeSpan(0, 2, 0)).Wait();
                    processTask.StatusChangeDate = DateTime.Now;
                    processTask.Status = "finished";
                    dbContext.SaveChanges();
                }  
            });
        }

        public static void AsyncItemUpdateFailed(Task task)
        {
            Exception ex = task.Exception;
            // Логируем исключение
        }
    }
}