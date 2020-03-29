namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cooperator", "AreaCode", c => c.String());
            AddColumn("dbo.Cooperator", "PhoneFirst", c => c.String());
            AddColumn("dbo.Cooperator", "PhoneSecond", c => c.String());
            DropColumn("dbo.Cooperator", "StreetAddress");
            DropColumn("dbo.Cooperator", "City");
            DropColumn("dbo.Cooperator", "State");
            DropColumn("dbo.Cooperator", "Zip");
            DropColumn("dbo.Cooperator", "ContactPreference");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cooperator", "ContactPreference", c => c.Int(nullable: false));
            AddColumn("dbo.Cooperator", "Zip", c => c.String());
            AddColumn("dbo.Cooperator", "State", c => c.String());
            AddColumn("dbo.Cooperator", "City", c => c.String());
            AddColumn("dbo.Cooperator", "StreetAddress", c => c.String());
            DropColumn("dbo.Cooperator", "PhoneSecond");
            DropColumn("dbo.Cooperator", "PhoneFirst");
            DropColumn("dbo.Cooperator", "AreaCode");
        }
    }
}
