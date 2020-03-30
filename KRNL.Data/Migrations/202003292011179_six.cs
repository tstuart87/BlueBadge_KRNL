namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class six : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator");
            DropIndex("dbo.Location", new[] { "CooperatorId" });
            AlterColumn("dbo.Location", "CooperatorId", c => c.Int());
            CreateIndex("dbo.Location", "CooperatorId");
            AddForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator", "CooperatorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator");
            DropIndex("dbo.Location", new[] { "CooperatorId" });
            AlterColumn("dbo.Location", "CooperatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Location", "CooperatorId");
            AddForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator", "CooperatorId", cascadeDelete: true);
        }
    }
}
