namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class four : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Message", "DateCreated", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Message", "DateCreated", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
