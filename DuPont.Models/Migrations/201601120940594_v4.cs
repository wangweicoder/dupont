namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_ADMIN_USER",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsSuperAdmin = c.Boolean(nullable: false),
                        RealName = c.String(maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100, unicode: false),
                        LoginToken = c.String(maxLength: 200, unicode: false),
                        AvartarUrl = c.String(maxLength: 300, unicode: false),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Region = c.String(maxLength: 50),
                        Township = c.String(maxLength: 50),
                        Village = c.String(maxLength: 50),
                        DetailedAddress = c.String(maxLength: 200),
                        IsLock = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_APP_VERSION",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(nullable: false, maxLength: 50),
                        VersionCode = c.Int(nullable: false),
                        DownloadURL = c.String(maxLength: 1000),
                        ChangeLog = c.String(maxLength: 1000),
                        CREATE_USER = c.Int(nullable: false),
                        CREATE_DATE = c.DateTime(nullable: false),
                        UPDATE_USER = c.Int(),
                        UPDATE_DATE = c.DateTime(),
                        Platform = c.String(maxLength: 15),
                        IsOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_AREA",
                c => new
                    {
                        AID = c.String(nullable: false, maxLength: 100, unicode: false),
                        DisplayName = c.String(nullable: false, maxLength: 200),
                        ParentAID = c.String(nullable: false, maxLength: 100, unicode: false),
                        Level = c.Byte(nullable: false),
                        Lng = c.String(maxLength: 200, unicode: false),
                        Lat = c.String(maxLength: 200, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AID);
            
            CreateTable(
                "dbo.T_ARTICLE",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300),
                        Content = c.String(nullable: false),
                        Url = c.String(maxLength: 300),
                        CreateTime = c.DateTime(nullable: false),
                        Click = c.Long(nullable: false),
                        CatId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(),
                        CreateUserId = c.Long(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsPutOnCarousel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_ARTICLE_CATEGORY", t => t.CatId, cascadeDelete: true)
                .Index(t => t.CatId);
            
            CreateTable(
                "dbo.T_ARTICLE_CATEGORY",
                c => new
                    {
                        CategoryId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.T_LEARNING_GARDEN_CAROUSEL",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ArticleId = c.Long(nullable: false),
                        CatId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_ARTICLE", t => t.ArticleId)
                .ForeignKey("dbo.T_ARTICLE_CATEGORY", t => t.CatId)
                .Index(t => t.ArticleId)
                .Index(t => t.CatId);
            
            CreateTable(
                "dbo.T_BUSINESS_DEMAND_RESPONSE_RELATION",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BonusDPoint = c.Int(nullable: false),
                        DemandId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        Comments = c.String(maxLength: 500, unicode: false),
                        ReplyTime = c.DateTime(nullable: false),
                        Score = c.Int(nullable: false),
                        WeightRangeTypeId = c.Int(nullable: false),
                        Address = c.String(maxLength: 200),
                        PhoneNumber = c.String(nullable: false, maxLength: 11, unicode: false),
                        Brief = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_BUSINESS_PUBLISHED_DEMAND",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DemandTypeId = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 11, unicode: false),
                        CropId = c.Int(),
                        PublishStateId = c.Int(nullable: false),
                        ExpectedDate = c.String(nullable: false, maxLength: 500, unicode: false),
                        TimeSlot = c.String(maxLength: 3, unicode: false),
                        ExpectedStartPrice = c.Decimal(precision: 18, scale: 2),
                        ExpectedEndPrice = c.Decimal(precision: 18, scale: 2),
                        AcquisitionWeightRangeTypeId = c.Int(nullable: false),
                        FirstWeight = c.Int(nullable: false),
                        Brief = c.String(maxLength: 300),
                        Province = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        Region = c.String(nullable: false, maxLength: 50),
                        Township = c.String(nullable: false, maxLength: 50),
                        Village = c.String(maxLength: 50),
                        DetailedAddress = c.String(maxLength: 200),
                        CreateUserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        ModifiedUserId = c.Long(),
                        DeletedUserId = c.Long(),
                        DeletedTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_BUSINESS_VERIFICATION_INFO",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RealName = c.String(maxLength: 50),
                        PurchaseType = c.String(nullable: false, maxLength: 100),
                        Introduction = c.String(maxLength: 200),
                        PictureIds = c.String(maxLength: 200, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        AuditUserId = c.Long(),
                        AuditTime = c.DateTime(),
                        AuditState = c.Int(nullable: false),
                        RejectReason = c.String(maxLength: 500),
                        Province = c.String(maxLength: 50, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        Region = c.String(maxLength: 50, unicode: false),
                        Township = c.String(maxLength: 50, unicode: false),
                        Village = c.String(maxLength: 50, unicode: false),
                        DetailAddress = c.String(maxLength: 200),
                        PhoneNumber = c.String(maxLength: 11, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_CAROUSEL",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Long(nullable: false),
                        Order = c.Int(nullable: false),
                        IsDisplay = c.Boolean(nullable: false),
                        CreateUserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        EditUserId = c.Long(),
                        EditTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_DEMONSTRATION_FARM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProvinceAid = c.String(nullable: false, maxLength: 100, unicode: false),
                        CityAid = c.String(nullable: false, maxLength: 100, unicode: false),
                        RegionAid = c.String(nullable: false, maxLength: 100, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsOpen = c.Boolean(nullable: false),
                        OpenStartDate = c.DateTime(),
                        OpenEndDate = c.DateTime(),
                        PlantArea = c.String(nullable: false, maxLength: 50),
                        Variety = c.String(nullable: false, maxLength: 50),
                        SowTime = c.String(nullable: false, maxLength: 50),
                        PlantPoint = c.String(nullable: false, maxLength: 200),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_FARM_AREA",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FarmId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Url = c.String(maxLength: 300, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsFarmMachinery = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DEMONSTRATION_FARM", t => t.FarmId, cascadeDelete: true)
                .Index(t => t.FarmId);
            
            CreateTable(
                "dbo.T_FARM_BOOKING",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FarmId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        VisitDate = c.DateTime(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_DEMONSTRATION_FARM", t => t.FarmId, cascadeDelete: true)
                .Index(t => t.FarmId);
            
            CreateTable(
                "dbo.T_EXPERT",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        LastModifiedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_FARMER_DEMAND_RESPONSE_RELATION",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BonusDPoint = c.Int(nullable: false),
                        DemandId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        Comments = c.String(maxLength: 500, unicode: false),
                        ReplyTime = c.DateTime(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_FARMER_PUBLISHED_DEMAND",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DemandTypeId = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 11, unicode: false),
                        PublishStateId = c.Int(nullable: false),
                        CropId = c.Int(nullable: false),
                        VarietyId = c.Int(),
                        AcresId = c.Int(nullable: false),
                        Brief = c.String(maxLength: 300),
                        ExpectedDate = c.String(nullable: false, maxLength: 500, unicode: false),
                        TimeSlot = c.String(maxLength: 3, unicode: false),
                        Province = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        Region = c.String(nullable: false, maxLength: 50),
                        Township = c.String(nullable: false, maxLength: 50),
                        Village = c.String(maxLength: 50),
                        DetailedAddress = c.String(maxLength: 200),
                        ExpectedStartPrice = c.Decimal(precision: 18, scale: 2),
                        ExpectedEndPrice = c.Decimal(precision: 18, scale: 2),
                        CreateUserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        ModifiedUserId = c.Long(),
                        DeletedUserId = c.Long(),
                        DeletedTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_FARMER_VERIFICATION_INFO",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RealName = c.String(maxLength: 50),
                        DupontOrderNumbers = c.String(maxLength: 2000, unicode: false),
                        PurchasedProducts = c.String(maxLength: 1000, unicode: false),
                        Land = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        AuditUserId = c.Long(),
                        AuditTime = c.DateTime(),
                        AuditState = c.Int(nullable: false),
                        RejectReason = c.String(maxLength: 500),
                        Province = c.String(maxLength: 50, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        Region = c.String(maxLength: 50, unicode: false),
                        Township = c.String(maxLength: 50, unicode: false),
                        Village = c.String(maxLength: 50, unicode: false),
                        DetailAddress = c.String(maxLength: 200),
                        PhoneNumber = c.String(maxLength: 11, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_FileInfo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Path = c.String(nullable: false, maxLength: 500),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_MACHINERY_OPERATOR_VERIFICATION_INFO",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RealName = c.String(maxLength: 50),
                        Machinery = c.String(maxLength: 2000, unicode: false),
                        PicturesIds = c.String(maxLength: 200, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        AuditUserId = c.Long(),
                        AuditTime = c.DateTime(),
                        AuditState = c.Int(nullable: false),
                        RejectReason = c.String(maxLength: 500),
                        Province = c.String(maxLength: 50, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        Region = c.String(maxLength: 50, unicode: false),
                        Township = c.String(maxLength: 50, unicode: false),
                        Village = c.String(maxLength: 50, unicode: false),
                        DetailAddress = c.String(maxLength: 200),
                        OtherMachineDescription = c.String(maxLength: 200),
                        PhoneNumber = c.String(maxLength: 11, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_MENU",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MenuName = c.String(nullable: false, maxLength: 20),
                        Visible = c.Boolean(nullable: false),
                        ParentId = c.Int(nullable: false),
                        Url = c.String(maxLength: 100),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_MENU_ROLE_RELATION",
                c => new
                    {
                        MenuId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        AuditUserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.MenuId, t.RoleId });
            
            CreateTable(
                "dbo.T_PIONEERCURRENCYHISTORY",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        UserId = c.Long(nullable: false),
                        DPoint = c.Int(nullable: false),
                        AuditUserId = c.Long(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_QUESTION",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 200),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        IsOpen = c.Boolean(nullable: false),
                        ReplyCount = c.Long(nullable: false),
                        PictureIds = c.String(maxLength: 100),
                        QuestionType = c.String(maxLength: 10, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        LastModifiedTime = c.DateTime(nullable: false),
                        IsPutOnCarousel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_USER", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.T_USER",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PhoneNumber = c.String(nullable: false, maxLength: 11, unicode: false),
                        Password = c.String(nullable: false, maxLength: 100, unicode: false),
                        LoginToken = c.String(maxLength: 200, unicode: false),
                        AvartarUrl = c.String(maxLength: 300, unicode: false),
                        UserName = c.String(maxLength: 50),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        DPoint = c.Int(),
                        Region = c.String(maxLength: 50),
                        Township = c.String(maxLength: 50),
                        Village = c.String(maxLength: 50),
                        DetailedAddress = c.String(maxLength: 200),
                        CreateTime = c.DateTime(nullable: false),
                        ModifiedUserId = c.Long(),
                        DeletedUserId = c.Long(),
                        DeletedTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedTime = c.DateTime(),
                        SmsCode = c.String(maxLength: 200, unicode: false),
                        LoginUserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_QUESTION_REPLY",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        Content = c.String(maxLength: 500),
                        LikeCount = c.Long(nullable: false),
                        IsAgree = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        LastModifiedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_ROLE",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_SMS_MESSAGE",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PhoneNumber = c.String(nullable: false, maxLength: 11, unicode: false),
                        Captcha = c.String(nullable: false, maxLength: 50, unicode: false),
                        SendTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_SUPPLIERS_AREA",
                c => new
                    {
                        UserID = c.Long(nullable: false),
                        AID = c.String(nullable: false, maxLength: 100, unicode: false),
                        State = c.Boolean(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.AID });
            
            CreateTable(
                "dbo.T_SYS_ADMIN",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        IsSuper = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        CreateUserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        ModifiedUserId = c.Long(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.T_SYS_DICTIONARY",
                c => new
                    {
                        Code = c.Int(nullable: false),
                        ParentCode = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.T_SYS_LOG",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Level = c.String(nullable: false, maxLength: 50, unicode: false),
                        StackTrace = c.String(nullable: false, maxLength: 3000),
                        Message = c.String(nullable: false, maxLength: 2000),
                        UserId = c.Long(),
                        UserName = c.String(maxLength: 50),
                        Url = c.String(maxLength: 500, unicode: false),
                        CreateTime = c.DateTime(nullable: false),
                        RequestParameter = c.String(maxLength: 2000, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_SYS_SETTING",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SETTING_ID = c.String(nullable: false, maxLength: 3),
                        SETTING_VALUE = c.String(nullable: false, maxLength: 255),
                        COMMENT = c.String(nullable: false, maxLength: 255),
                        CREATE_USER = c.Int(nullable: false),
                        CREATE_DATE = c.DateTime(nullable: false),
                        UPDATE_USER = c.Int(),
                        UPDATE_DATE = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.T_USER_PASSWORD_HISTORY",
                c => new
                    {
                        UserID = c.Long(nullable: false),
                        Password = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.T_USER_ROLE_DEMANDTYPELEVEL_RELATION",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        DemandId = c.Int(nullable: false),
                        Star = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId, t.DemandId });
            
            CreateTable(
                "dbo.T_USER_ROLE_RELATION",
                c => new
                    {
                        UserID = c.Long(nullable: false),
                        RoleID = c.Int(nullable: false),
                        MemberType = c.Boolean(nullable: false),
                        Star = c.Byte(),
                        AuditUserId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.RoleID, t.MemberType });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_QUESTION", "UserId", "dbo.T_USER");
            DropForeignKey("dbo.T_FARM_BOOKING", "FarmId", "dbo.T_DEMONSTRATION_FARM");
            DropForeignKey("dbo.T_FARM_AREA", "FarmId", "dbo.T_DEMONSTRATION_FARM");
            DropForeignKey("dbo.T_ARTICLE", "CatId", "dbo.T_ARTICLE_CATEGORY");
            DropForeignKey("dbo.T_LEARNING_GARDEN_CAROUSEL", "CatId", "dbo.T_ARTICLE_CATEGORY");
            DropForeignKey("dbo.T_LEARNING_GARDEN_CAROUSEL", "ArticleId", "dbo.T_ARTICLE");
            DropIndex("dbo.T_QUESTION", new[] { "UserId" });
            DropIndex("dbo.T_FARM_BOOKING", new[] { "FarmId" });
            DropIndex("dbo.T_FARM_AREA", new[] { "FarmId" });
            DropIndex("dbo.T_LEARNING_GARDEN_CAROUSEL", new[] { "CatId" });
            DropIndex("dbo.T_LEARNING_GARDEN_CAROUSEL", new[] { "ArticleId" });
            DropIndex("dbo.T_ARTICLE", new[] { "CatId" });
            DropTable("dbo.T_USER_ROLE_RELATION");
            DropTable("dbo.T_USER_ROLE_DEMANDTYPELEVEL_RELATION");
            DropTable("dbo.T_USER_PASSWORD_HISTORY");
            DropTable("dbo.T_SYS_SETTING");
            DropTable("dbo.T_SYS_LOG");
            DropTable("dbo.T_SYS_DICTIONARY");
            DropTable("dbo.T_SYS_ADMIN");
            DropTable("dbo.T_SUPPLIERS_AREA");
            DropTable("dbo.T_SMS_MESSAGE");
            DropTable("dbo.T_ROLE");
            DropTable("dbo.T_QUESTION_REPLY");
            DropTable("dbo.T_USER");
            DropTable("dbo.T_QUESTION");
            DropTable("dbo.T_PIONEERCURRENCYHISTORY");
            DropTable("dbo.T_MENU_ROLE_RELATION");
            DropTable("dbo.T_MENU");
            DropTable("dbo.T_MACHINERY_OPERATOR_VERIFICATION_INFO");
            DropTable("dbo.T_FileInfo");
            DropTable("dbo.T_FARMER_VERIFICATION_INFO");
            DropTable("dbo.T_FARMER_PUBLISHED_DEMAND");
            DropTable("dbo.T_FARMER_DEMAND_RESPONSE_RELATION");
            DropTable("dbo.T_EXPERT");
            DropTable("dbo.T_FARM_BOOKING");
            DropTable("dbo.T_FARM_AREA");
            DropTable("dbo.T_DEMONSTRATION_FARM");
            DropTable("dbo.T_CAROUSEL");
            DropTable("dbo.T_BUSINESS_VERIFICATION_INFO");
            DropTable("dbo.T_BUSINESS_PUBLISHED_DEMAND");
            DropTable("dbo.T_BUSINESS_DEMAND_RESPONSE_RELATION");
            DropTable("dbo.T_LEARNING_GARDEN_CAROUSEL");
            DropTable("dbo.T_ARTICLE_CATEGORY");
            DropTable("dbo.T_ARTICLE");
            DropTable("dbo.T_AREA");
            DropTable("dbo.T_APP_VERSION");
            DropTable("dbo.T_ADMIN_USER");
        }
    }
}
