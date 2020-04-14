namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "CooperatorId", c => c.Int());
            CreateIndex("dbo.Message", "CooperatorId");
            AddForeignKey("dbo.Message", "CooperatorId", "dbo.Cooperator", "CooperatorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "CooperatorId", "dbo.Cooperator");
            DropIndex("dbo.Message", new[] { "CooperatorId" });
            DropColumn("dbo.Message", "CooperatorId");
        }
    }
}
