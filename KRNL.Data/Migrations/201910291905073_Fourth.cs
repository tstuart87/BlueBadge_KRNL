namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cooperator", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cooperator", "FullName");
        }
    }
}
