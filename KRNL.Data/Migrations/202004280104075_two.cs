namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class two : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Location", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Location", "Rating", c => c.Double(nullable: false));
        }
    }
}
