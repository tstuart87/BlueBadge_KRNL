namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "FullName");
        }
    }
}
