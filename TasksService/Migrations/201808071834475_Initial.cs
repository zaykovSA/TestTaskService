namespace TasksService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessTasks",
                c => new
                    {
                        TaskId = c.Guid(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                        StatusChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId);
        }
        
        public override void Down()
        {
            DropTable("dbo.ProcessTasks");
        }
    }
}
