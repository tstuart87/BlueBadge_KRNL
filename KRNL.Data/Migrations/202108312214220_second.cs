namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cooperator",
                c => new
                    {
                        CooperatorId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        AreaCode = c.String(),
                        PhoneFirst = c.String(),
                        PhoneSecond = c.String(),
                        FullName = c.String(),
                        Phone = c.String(),
                        ContactType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CooperatorId);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Guid(nullable: false),
                        DocName = c.String(),
                        DocString = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DocType = c.Int(nullable: false),
                        LocationId = c.Int(),
                        DateCreated = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false),
                        State = c.Int(nullable: false),
                        LocationCode = c.String(nullable: false),
                        OwnerId = c.Guid(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        GDUs = c.String(),
                        CumulativePrecip = c.String(),
                        GrowthStage = c.String(),
                        MonthOfPlanting = c.Int(nullable: false),
                        DayOfPlanting = c.Int(nullable: false),
                        YearOfPlanting = c.Int(nullable: false),
                        MonthOfHarvest = c.Int(nullable: false),
                        DayOfHarvest = c.Int(nullable: false),
                        YearOfHarvest = c.Int(nullable: false),
                        DatePlanted = c.DateTimeOffset(nullable: false, precision: 7),
                        DateHarvested = c.DateTimeOffset(nullable: false, precision: 7),
                        IsPlanted = c.Boolean(nullable: false),
                        IsStaked = c.Boolean(nullable: false),
                        IsRowbanded = c.Boolean(nullable: false),
                        IsHarvested = c.Boolean(nullable: false),
                        MapLink = c.String(),
                        CRM = c.Int(nullable: false),
                        Tag = c.String(),
                        SearchString = c.String(),
                        Rating = c.Int(nullable: false),
                        RequestCount = c.Int(nullable: false),
                        LastVisitor = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CooperatorId = c.Int(),
                        Map = c.String(),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Cooperator", t => t.CooperatorId)
                .Index(t => t.CooperatorId);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        OwnerId = c.Guid(nullable: false),
                        Comment = c.String(),
                        DateCreated = c.DateTimeOffset(precision: 7),
                        LocationCode = c.String(),
                        JobOne = c.Int(nullable: false),
                        JobTwo = c.Int(nullable: false),
                        JobThree = c.Int(nullable: false),
                        HumanGrowthStage = c.Int(nullable: false),
                        PredictedGrowthStage = c.String(),
                        Rating = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsRequest = c.Boolean(nullable: false),
                        CooperatorId = c.Int(),
                        FullName = c.String(),
                        DocumentId = c.Int(),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Cooperator", t => t.CooperatorId)
                .ForeignKey("dbo.Document", t => t.DocumentId)
                .ForeignKey("dbo.Location", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.CooperatorId)
                .Index(t => t.DocumentId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.Message", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Message", "DocumentId", "dbo.Document");
            DropForeignKey("dbo.Message", "CooperatorId", "dbo.Cooperator");
            DropForeignKey("dbo.Document", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Location", "CooperatorId", "dbo.Cooperator");
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.Message", new[] { "LocationId" });
            DropIndex("dbo.Message", new[] { "DocumentId" });
            DropIndex("dbo.Message", new[] { "CooperatorId" });
            DropIndex("dbo.Location", new[] { "CooperatorId" });
            DropIndex("dbo.Document", new[] { "LocationId" });
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.Message");
            DropTable("dbo.Location");
            DropTable("dbo.Document");
            DropTable("dbo.Cooperator");
        }
    }
}
