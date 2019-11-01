namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fifth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Location", "Longitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Location", "Latitude", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Location", "Latitude", c => c.String());
            AlterColumn("dbo.Location", "Longitude", c => c.String());
        }
    }
}
