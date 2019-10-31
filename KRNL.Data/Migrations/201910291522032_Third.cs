namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Location", "CooperatorId");
            AddForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator", "CooperatorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator");
            DropIndex("dbo.Location", new[] { "CooperatorId" });
        }
    }
}
