namespace AlfaPeople.KingKhalidFoundation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KKF_CBP_DB_V1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicantStatus",
                c => new
                    {
                        ApplicantStatusID = c.Guid(nullable: false),
                        ApplicantStatusName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ApplicantStatusID);
            
            CreateTable(
                "dbo.CorporateApplicationStatus",
                c => new
                    {
                        CorporateApplicationFormID = c.Guid(nullable: false),
                        ApplicantStatusID = c.Guid(nullable: false),
                        ResonTypeID = c.Guid(),
                        FeadBack = c.String(),
                        Fk_BackEndMakeAction = c.Guid(),
                        DateTimeMakeAction = c.DateTime(),
                    })
                .PrimaryKey(t => t.CorporateApplicationFormID)
                .ForeignKey("dbo.CorporateApplicationForm", t => t.CorporateApplicationFormID)
                .ForeignKey("dbo.BackendUsers", t => t.Fk_BackEndMakeAction)
                .ForeignKey("dbo.ReasonTypes", t => t.ResonTypeID)
                .ForeignKey("dbo.ApplicantStatus", t => t.ApplicantStatusID)
                .Index(t => t.CorporateApplicationFormID)
                .Index(t => t.ApplicantStatusID)
                .Index(t => t.ResonTypeID)
                .Index(t => t.Fk_BackEndMakeAction);
            
            CreateTable(
                "dbo.BackendUsers",
                c => new
                    {
                        BackendUserID = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FK_AspUser = c.String(maxLength: 128),
                        Password = c.String(nullable: false),
                        BackEndPositionId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BackendUserID)
                .ForeignKey("dbo.AspNetUsers", t => t.FK_AspUser)
                .ForeignKey("dbo.BackendUserPositions", t => t.BackEndPositionId)
                .Index(t => t.FK_AspUser)
                .Index(t => t.BackEndPositionId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FrontendUsers",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        ParentID = c.Guid(),
                        CreateDate = c.DateTime(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        IsApproved = c.Boolean(nullable: false),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        FK_AspUser = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.FrontendUsers", t => t.ParentID)
                .ForeignKey("dbo.AspNetUsers", t => t.FK_AspUser)
                .Index(t => t.ParentID)
                .Index(t => t.FK_AspUser);
            
            CreateTable(
                "dbo.CorporateApplicationForm",
                c => new
                    {
                        CorporateApplicationFormID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        FoundedYear = c.DateTime(nullable: false, storeType: "date"),
                        RegistrationNumber = c.String(nullable: false, maxLength: 20),
                        TaxNumber = c.String(),
                        History = c.String(nullable: false),
                        Vision = c.String(nullable: false),
                        Mission = c.String(nullable: false),
                        Objectives = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        PostalCode = c.String(nullable: false, maxLength: 50),
                        POBox = c.String(maxLength: 50),
                        Extension = c.String(maxLength: 50),
                        TelephoneNumber = c.String(nullable: false, maxLength: 50),
                        FaxNumber = c.String(maxLength: 50),
                        OfficialEmail = c.String(nullable: false, maxLength: 50),
                        AdministratorEmail = c.String(nullable: false, maxLength: 50),
                        Website = c.String(maxLength: 50),
                        TwitterAccount = c.String(maxLength: 50),
                        YouTubeAccount = c.String(maxLength: 50),
                        SnapchatAccount = c.String(maxLength: 50),
                        InstagramAccount = c.String(maxLength: 50),
                        CorporateAdministratorName = c.String(nullable: false, maxLength: 50),
                        CorporateAdministratorJobTitle = c.String(nullable: false, maxLength: 50),
                        CorporateAdministratorMobileNumber = c.String(nullable: false, maxLength: 50),
                        CorporateAdministratorTelephoneNumber = c.String(nullable: false, maxLength: 50),
                        CorporateAdministratorExtension = c.String(nullable: false, maxLength: 50),
                        CorporationsCategoryID = c.Guid(nullable: false),
                        CorporateFieldOfWorkID = c.Guid(),
                        ClassificationSectorID = c.Guid(),
                        AuthorizationAuthorityID = c.Guid(nullable: false),
                        ProgramID = c.Guid(nullable: false),
                        RegionID = c.Guid(nullable: false),
                        GovernorateID = c.Guid(nullable: false),
                        CityID = c.Guid(),
                        FrontendUserID = c.Guid(nullable: false),
                        DateElection = c.DateTime(storeType: "date"),
                        Picture = c.Binary(storeType: "image"),
                        corporateGenderType = c.Int(),
                    })
                .PrimaryKey(t => t.CorporateApplicationFormID)
                .ForeignKey("dbo.AuthorizationAuthority", t => t.AuthorizationAuthorityID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .ForeignKey("dbo.Governorates", t => t.GovernorateID)
                .ForeignKey("dbo.Regions", t => t.RegionID)
                .ForeignKey("dbo.Programs", t => t.ProgramID)
                .ForeignKey("dbo.ClassificationSector", t => t.ClassificationSectorID)
                .ForeignKey("dbo.CorporationsCategory", t => t.CorporationsCategoryID)
                .ForeignKey("dbo.CorporateFieldOfWork", t => t.CorporateFieldOfWorkID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.CorporationsCategoryID)
                .Index(t => t.CorporateFieldOfWorkID)
                .Index(t => t.ClassificationSectorID)
                .Index(t => t.AuthorizationAuthorityID)
                .Index(t => t.ProgramID)
                .Index(t => t.RegionID)
                .Index(t => t.GovernorateID)
                .Index(t => t.CityID)
                .Index(t => t.FrontendUserID);
            
            CreateTable(
                "dbo.AuthorizationAuthority",
                c => new
                    {
                        AuthorizationAuthorityID = c.Guid(nullable: false),
                        AuthorizationAuthorityNameAR = c.String(nullable: false, maxLength: 50),
                        AuthorizationAuthorityNameEN = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorizationAuthorityID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Guid(nullable: false),
                        GovernorateID = c.Guid(nullable: false),
                        CityNameAR = c.String(nullable: false, maxLength: 100),
                        CityNameEN = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityID)
                .ForeignKey("dbo.Governorates", t => t.GovernorateID)
                .Index(t => t.GovernorateID);
            
            CreateTable(
                "dbo.Governorates",
                c => new
                    {
                        GovernorateID = c.Guid(nullable: false),
                        RegionID = c.Guid(nullable: false),
                        GovernorateAR = c.String(nullable: false, maxLength: 100),
                        GovernorateEN = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GovernorateID)
                .ForeignKey("dbo.Regions", t => t.RegionID)
                .Index(t => t.RegionID);
            
            CreateTable(
                "dbo.IncubationWorkshops",
                c => new
                    {
                        IncubationWorkshopID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        LastTimeToApply = c.DateTime(nullable: false, storeType: "date"),
                        TrainingHeadquarters = c.String(nullable: false),
                        ISPublic = c.Boolean(nullable: false),
                        ConsultantID = c.Guid(nullable: false),
                        IncubationWorkshopModelID = c.Guid(nullable: false),
                        IncubationtWorkshopStatusID = c.Guid(nullable: false),
                        RegionID = c.Guid(nullable: false),
                        GovernorateID = c.Guid(nullable: false),
                        CityID = c.Guid(),
                        FundingSourceID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.IncubationWorkshopID)
                .ForeignKey("dbo.FundingSource", t => t.FundingSourceID)
                .ForeignKey("dbo.Consultants", t => t.ConsultantID)
                .ForeignKey("dbo.IncubationtWorkshopStatus", t => t.IncubationtWorkshopStatusID)
                .ForeignKey("dbo.IncubationWorkshopModels", t => t.IncubationWorkshopModelID)
                .ForeignKey("dbo.Regions", t => t.RegionID)
                .ForeignKey("dbo.Governorates", t => t.GovernorateID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .Index(t => t.ConsultantID)
                .Index(t => t.IncubationWorkshopModelID)
                .Index(t => t.IncubationtWorkshopStatusID)
                .Index(t => t.RegionID)
                .Index(t => t.GovernorateID)
                .Index(t => t.CityID)
                .Index(t => t.FundingSourceID);
            
            CreateTable(
                "dbo.Consultants",
                c => new
                    {
                        ConsultantID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 250),
                        POBox = c.String(nullable: false, maxLength: 50),
                        PostalCode = c.String(nullable: false, maxLength: 50),
                        OfficialMail = c.String(nullable: false, maxLength: 50),
                        MobileNumber = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        TelephoneNumber = c.String(nullable: false, maxLength: 50),
                        Extension = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ConsultantID);
            
            CreateTable(
                "dbo.ConsultantAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        ConsultantID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.Consultants", t => t.ConsultantID)
                .Index(t => t.ConsultantID);
            
            CreateTable(
                "dbo.Incubations",
                c => new
                    {
                        IncubationID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ProjectDurationInDays = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        ProjectDetails = c.String(nullable: false),
                        Targeted = c.String(nullable: false),
                        Notes = c.String(nullable: false),
                        IncubationModelID = c.Guid(nullable: false),
                        IncubationTypeID = c.Guid(nullable: false),
                        IncubationStatusID = c.Guid(nullable: false),
                        IncubationAdID = c.Guid(nullable: false),
                        ConsultantID = c.Guid(nullable: false),
                        TypeOfInterventionID = c.Guid(nullable: false),
                        SpecialistChargeOfIncubation = c.Guid(nullable: false),
                        IncubationProjectProposalID = c.Guid(nullable: false),
                        FrontendUser_UserID = c.Guid(),
                    })
                .PrimaryKey(t => t.IncubationID)
                .ForeignKey("dbo.BackendUsers", t => t.SpecialistChargeOfIncubation, cascadeDelete: true)
                .ForeignKey("dbo.IncubationModels", t => t.IncubationModelID)
                .ForeignKey("dbo.IncubationTypes", t => t.IncubationTypeID)
                .ForeignKey("dbo.IncubationAdvertisings", t => t.IncubationAdID)
                .ForeignKey("dbo.IncubationProjectProposals", t => t.IncubationProjectProposalID, cascadeDelete: true)
                .ForeignKey("dbo.IncubationStatus", t => t.IncubationStatusID)
                .ForeignKey("dbo.TypeOfIntervention", t => t.TypeOfInterventionID)
                .ForeignKey("dbo.Consultants", t => t.ConsultantID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUser_UserID)
                .Index(t => t.IncubationModelID)
                .Index(t => t.IncubationTypeID)
                .Index(t => t.IncubationStatusID)
                .Index(t => t.IncubationAdID)
                .Index(t => t.ConsultantID)
                .Index(t => t.TypeOfInterventionID)
                .Index(t => t.SpecialistChargeOfIncubation)
                .Index(t => t.IncubationProjectProposalID)
                .Index(t => t.FrontendUser_UserID);
            
            CreateTable(
                "dbo.IncubationAdvertisings",
                c => new
                    {
                        IncubationAdID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        AdvertisingDetails = c.String(nullable: false),
                        EntryRequirements = c.String(nullable: false),
                        IncubationTypeID = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ISPublic = c.Boolean(nullable: false),
                        FundingSourceID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.IncubationAdID)
                .ForeignKey("dbo.FundingSource", t => t.FundingSourceID)
                .ForeignKey("dbo.IncubationTypes", t => t.IncubationTypeID, cascadeDelete: true)
                .Index(t => t.IncubationTypeID)
                .Index(t => t.FundingSourceID);
            
            CreateTable(
                "dbo.FundingSource",
                c => new
                    {
                        FundingSourceID = c.Guid(nullable: false),
                        Nickname = c.String(nullable: false, maxLength: 10),
                        FundingSourceNameAR = c.String(nullable: false, maxLength: 50),
                        FundingSourceNameEN = c.String(nullable: false, maxLength: 50),
                        GrantLogoPic = c.String(),
                        GrantHeaderPic = c.String(),
                        GrantBackgroundPic = c.String(),
                        RegistrationBackgroundPic = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        UseCustomThemes = c.Boolean(nullable: false),
                        HideKKFLogo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FundingSourceID);
            
            CreateTable(
                "dbo.IncubationAdvertisingAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IncubationAdID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.IncubationAdvertisings", t => t.IncubationAdID)
                .Index(t => t.IncubationAdID);
            
            CreateTable(
                "dbo.IncubationAdvertisingModels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IncubationModelID = c.Guid(nullable: false),
                        IncubationAdID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.IncubationModels", t => t.IncubationModelID)
                .ForeignKey("dbo.IncubationAdvertisings", t => t.IncubationAdID)
                .Index(t => t.IncubationModelID)
                .Index(t => t.IncubationAdID);
            
            CreateTable(
                "dbo.IncubationModels",
                c => new
                    {
                        IncubationModelID = c.Guid(nullable: false),
                        IncubationTypeID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 100),
                        NameEN = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        FK_AspUserCreateModel = c.String(maxLength: 128),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IncubationModelID)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .ForeignKey("dbo.IncubationTypes", t => t.IncubationTypeID)
                .Index(t => t.IncubationTypeID)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.IncubationTypes",
                c => new
                    {
                        IncubationTypeID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IncubationTypeID);
            
            CreateTable(
                "dbo.IncubationFollowUpEvaluations",
                c => new
                    {
                        IncubationFollowUpEvaluationID = c.Guid(nullable: false),
                        FrontendUserID = c.Guid(nullable: false),
                        IncubationID = c.Guid(nullable: false),
                        IncubationTypeID = c.Guid(nullable: false),
                        Impact = c.String(),
                        SuccessStories = c.String(),
                        ProjectSuccessRate = c.Int(nullable: false),
                        Reasone = c.String(),
                    })
                .PrimaryKey(t => t.IncubationFollowUpEvaluationID)
                .ForeignKey("dbo.IncubationTypes", t => t.IncubationTypeID)
                .ForeignKey("dbo.Incubations", t => t.IncubationID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.FrontendUserID)
                .Index(t => t.IncubationID)
                .Index(t => t.IncubationTypeID);
            
            CreateTable(
                "dbo.IncubationFollowUpEvaluationAttchments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IncubationFollowUpEvaluationID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.IncubationFollowUpEvaluations", t => t.IncubationFollowUpEvaluationID)
                .Index(t => t.IncubationFollowUpEvaluationID);
            
            CreateTable(
                "dbo.IncubationFollowUpEvaluationLines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IncubationFollowUpEvaluationID = c.Guid(nullable: false),
                        Date = c.String(),
                        BackendUserID = c.Guid(nullable: false),
                        PhaseOutput = c.String(),
                        Notes = c.String(),
                        NextStep = c.String(),
                        FollowUpMethod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BackendUsers", t => t.BackendUserID, cascadeDelete: true)
                .ForeignKey("dbo.IncubationFollowUpEvaluations", t => t.IncubationFollowUpEvaluationID)
                .Index(t => t.IncubationFollowUpEvaluationID)
                .Index(t => t.BackendUserID);
            
            CreateTable(
                "dbo.IncubationPrivateInvitations",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        IncubationAdID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.IncubationAdvertisings", t => t.IncubationAdID)
                .Index(t => t.IncubationAdID);
            
            CreateTable(
                "dbo.IncubationProjectProposals",
                c => new
                    {
                        IncubationProjectProposalID = c.Guid(nullable: false),
                        IncubationProjectProposalStatusID = c.Guid(nullable: false),
                        IncubationAdID = c.Guid(nullable: false),
                        FrontendUserID = c.Guid(nullable: false),
                        submissionDate = c.DateTime(nullable: false, storeType: "date"),
                        Feadback = c.String(),
                    })
                .PrimaryKey(t => t.IncubationProjectProposalID)
                .ForeignKey("dbo.IncubationProjectProposalStatus", t => t.IncubationProjectProposalStatusID)
                .ForeignKey("dbo.IncubationAdvertisings", t => t.IncubationAdID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.IncubationProjectProposalStatusID)
                .Index(t => t.IncubationAdID)
                .Index(t => t.FrontendUserID);
            
            CreateTable(
                "dbo.IncubationProjectProposalAttachements",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IncubationProjectProposalID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.IncubationProjectProposals", t => t.IncubationProjectProposalID)
                .Index(t => t.IncubationProjectProposalID);
            
            CreateTable(
                "dbo.IncubationProjectProposalStatus",
                c => new
                    {
                        IncubationProjectProposalStatusID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IncubationProjectProposalStatusID);
            
            CreateTable(
                "dbo.IncubationAttchments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IncubationID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.Incubations", t => t.IncubationID)
                .Index(t => t.IncubationID);
            
            CreateTable(
                "dbo.IncubationStatus",
                c => new
                    {
                        IncubationStatusID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IncubationStatusID);
            
            CreateTable(
                "dbo.TypeOfIntervention",
                c => new
                    {
                        TypeOfInterventionID = c.Guid(nullable: false),
                        TypeOfInterventionNameAR = c.String(nullable: false, maxLength: 50),
                        TypeOfInterventionNameEN = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TypeOfInterventionID);
            
            CreateTable(
                "dbo.FollowUpProjectPlans",
                c => new
                    {
                        FollowUpProjectPlanId = c.Guid(nullable: false),
                        IncubationWorkshopID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Deadline = c.DateTime(nullable: false, storeType: "date"),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.FollowUpProjectPlanId)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .Index(t => t.IncubationWorkshopID);
            
            CreateTable(
                "dbo.FollowUpProjectPlanAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        FollowUpProjectPlanId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.FollowUpProjectPlans", t => t.FollowUpProjectPlanId)
                .Index(t => t.FollowUpProjectPlanId);
            
            CreateTable(
                "dbo.FollowUpProjectPlanRequests",
                c => new
                    {
                        FollowUpProjectPlanRequestId = c.Guid(nullable: false),
                        FollowUpProjectPlanId = c.Guid(nullable: false),
                        FrontendUserId = c.Guid(nullable: false),
                        FollowUpProjectPlanStatus = c.Int(nullable: false),
                        Notes = c.String(),
                        feedBack = c.String(),
                    })
                .PrimaryKey(t => t.FollowUpProjectPlanRequestId)
                .ForeignKey("dbo.FollowUpProjectPlans", t => t.FollowUpProjectPlanId)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserId)
                .Index(t => t.FollowUpProjectPlanId)
                .Index(t => t.FrontendUserId);
            
            CreateTable(
                "dbo.FollowUpProjectPlanRequestAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        FollowUpProjectPlanRequestId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.FollowUpProjectPlanRequests", t => t.FollowUpProjectPlanRequestId)
                .Index(t => t.FollowUpProjectPlanRequestId);
            
            CreateTable(
                "dbo.IncubationtWorkshopStatus",
                c => new
                    {
                        IncubationtWorkshopStatusID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IncubationtWorkshopStatusID);
            
            CreateTable(
                "dbo.IncubationWorkshopAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IncubationWorkshopID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .Index(t => t.IncubationWorkshopID);
            
            CreateTable(
                "dbo.IncubationWorkShopFollowUpEvaluations",
                c => new
                    {
                        IncubationWorkShopFollowUpEvaluationID = c.Guid(nullable: false),
                        FrontendUserID = c.Guid(nullable: false),
                        IncubationWorkshopID = c.Guid(nullable: false),
                        Impact = c.String(),
                        SuccessStories = c.String(),
                        ProjectSuccessRate = c.Int(nullable: false),
                        Reasone = c.String(),
                    })
                .PrimaryKey(t => t.IncubationWorkShopFollowUpEvaluationID)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.FrontendUserID)
                .Index(t => t.IncubationWorkshopID);
            
            CreateTable(
                "dbo.IncubationWorkShopFollowUpEvaluationAttchments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IncubationWorkShopFollowUpEvaluationID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.IncubationWorkShopFollowUpEvaluations", t => t.IncubationWorkShopFollowUpEvaluationID)
                .Index(t => t.IncubationWorkShopFollowUpEvaluationID);
            
            CreateTable(
                "dbo.IncubationWorkShopFollowUpEvaluationLines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IncubationWorkShopFollowUpEvaluationID = c.Guid(nullable: false),
                        Date = c.String(),
                        BackendUserID = c.Guid(nullable: false),
                        PhaseOutput = c.String(),
                        Notes = c.String(),
                        NextStep = c.String(),
                        FollowUpMethod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BackendUsers", t => t.BackendUserID, cascadeDelete: true)
                .ForeignKey("dbo.IncubationWorkShopFollowUpEvaluations", t => t.IncubationWorkShopFollowUpEvaluationID)
                .Index(t => t.IncubationWorkShopFollowUpEvaluationID)
                .Index(t => t.BackendUserID);
            
            CreateTable(
                "dbo.IncubationWorkshopModels",
                c => new
                    {
                        IncubationWorkshopModeID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        FK_AspUserCreateModel = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IncubationWorkshopModeID)
                .ForeignKey("dbo.AspNetUsers", t => t.FK_AspUserCreateModel)
                .Index(t => t.FK_AspUserCreateModel);
            
            CreateTable(
                "dbo.IncubationWorkshopRatings",
                c => new
                    {
                        IncubationWorkshopRatingId = c.Guid(nullable: false),
                        FrontendUserId = c.Guid(nullable: false),
                        IncubationWorkshopID = c.Guid(nullable: false),
                        EvaluationDate = c.DateTime(nullable: false, storeType: "date"),
                        TrainingWorkshop = c.Int(nullable: false),
                        ReasonForTrainingWorkshop = c.String(),
                        AchievingGoal = c.Int(nullable: false),
                        ReasonForAchievingGoals = c.String(),
                        MeetTheWorkRequirement = c.Int(nullable: false),
                        ReasonForMeetTheWorkRequirement = c.String(),
                        TrainingMaterial = c.Int(nullable: false),
                        ReasonForTrainingMaterial = c.String(),
                        PartcipationReaction = c.Int(nullable: false),
                        ReasonForPartcipationReaction = c.String(),
                        Weakness = c.String(),
                        Power = c.String(),
                        WorkshopClass = c.Int(nullable: false),
                        ReasonForWorkshopClass = c.String(),
                        Hosting = c.Int(nullable: false),
                        ReasonForHosting = c.String(),
                        TheAbilityToDeliverInformation = c.Int(nullable: false),
                        BodyLanguage = c.Int(nullable: false),
                        ClarityOfVoiceAndTone = c.Int(nullable: false),
                        MasteryOfTrainingMaterial = c.Int(nullable: false),
                        AbilityToManageDiscussionAndHandleQuestions = c.Int(nullable: false),
                        LinkingTheTrainingMaterialToReality = c.Int(nullable: false),
                        AbilityToAchieveAGoal = c.Int(nullable: false),
                        CommentsOnTrainer = c.String(),
                    })
                .PrimaryKey(t => t.IncubationWorkshopRatingId)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserId)
                .Index(t => t.FrontendUserId)
                .Index(t => t.IncubationWorkshopID);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        RegionID = c.Guid(nullable: false),
                        RegionNameAR = c.String(nullable: false, maxLength: 100),
                        RegionNameEN = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RegionID);
            
            CreateTable(
                "dbo.IndividualApplicationForm",
                c => new
                    {
                        IndividualApplicationFormID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        Gender = c.Boolean(nullable: false),
                        IdentityNumber = c.String(nullable: false, maxLength: 50),
                        CompanyName = c.String(maxLength: 50),
                        MobileNumber = c.String(nullable: false, maxLength: 50),
                        TelephoneNumber = c.String(nullable: false, maxLength: 50),
                        Extension = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false),
                        PostalCode = c.String(nullable: false, maxLength: 50),
                        POBox = c.String(nullable: false, maxLength: 50),
                        WorkStartDate = c.DateTime(nullable: false, storeType: "date"),
                        Position = c.String(maxLength: 50),
                        PositionDetails = c.String(),
                        Nationality = c.Guid(nullable: false),
                        RegionID = c.Guid(nullable: false),
                        GovernorateID = c.Guid(nullable: false),
                        CityID = c.Guid(),
                        ProgramID = c.Guid(nullable: false),
                        FrontendUserID = c.Guid(nullable: false),
                        Picture = c.Binary(storeType: "image"),
                    })
                .PrimaryKey(t => t.IndividualApplicationFormID)
                .ForeignKey("dbo.CountriesAndNationalities", t => t.Nationality)
                .ForeignKey("dbo.Programs", t => t.ProgramID)
                .ForeignKey("dbo.Regions", t => t.RegionID)
                .ForeignKey("dbo.Governorates", t => t.GovernorateID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.Nationality)
                .Index(t => t.RegionID)
                .Index(t => t.GovernorateID)
                .Index(t => t.CityID)
                .Index(t => t.ProgramID)
                .Index(t => t.FrontendUserID);
            
            CreateTable(
                "dbo.CountriesAndNationalities",
                c => new
                    {
                        CountriesAndNationalitiesID = c.Guid(nullable: false),
                        Abbreviation = c.String(nullable: false, maxLength: 2),
                        NameEN = c.String(nullable: false, maxLength: 50),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NationalityEN = c.String(nullable: false, maxLength: 50),
                        NationalityAR = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CountriesAndNationalitiesID);
            
            CreateTable(
                "dbo.EducationalQualifications",
                c => new
                    {
                        EducationalQualificationsID = c.Guid(nullable: false),
                        IndividualApplicationFormID = c.Guid(nullable: false),
                        TrainingName = c.String(nullable: false, maxLength: 100),
                        Organisers = c.String(nullable: false, maxLength: 100),
                        Representation = c.String(nullable: false, maxLength: 100),
                        RepresentationDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.EducationalQualificationsID)
                .ForeignKey("dbo.IndividualApplicationForm", t => t.IndividualApplicationFormID)
                .Index(t => t.IndividualApplicationFormID);
            
            CreateTable(
                "dbo.IndividualApplicantStatus",
                c => new
                    {
                        IndividualApplicationFormID = c.Guid(nullable: false),
                        ApplicantStatusID = c.Guid(nullable: false),
                        FeadBack = c.String(),
                    })
                .PrimaryKey(t => t.IndividualApplicationFormID)
                .ForeignKey("dbo.IndividualApplicationForm", t => t.IndividualApplicationFormID)
                .ForeignKey("dbo.ApplicantStatus", t => t.ApplicantStatusID)
                .Index(t => t.IndividualApplicationFormID)
                .Index(t => t.ApplicantStatusID);
            
            CreateTable(
                "dbo.IndividualApplicationFormAttachment",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        IndividualApplicationFormID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.IndividualApplicationForm", t => t.IndividualApplicationFormID)
                .Index(t => t.IndividualApplicationFormID);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        ProgramID = c.Guid(nullable: false),
                        ProgramName = c.String(nullable: false, maxLength: 50),
                        ProgramNameAR = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProgramID);
            
            CreateTable(
                "dbo.TrainingCourses",
                c => new
                    {
                        TrainingCoursesID = c.Guid(nullable: false),
                        IndividualApplicationFormID = c.Guid(nullable: false),
                        Qualification = c.String(nullable: false, maxLength: 100),
                        Specialization = c.String(nullable: false, maxLength: 100),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        IssuedFrom = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.TrainingCoursesID)
                .ForeignKey("dbo.IndividualApplicationForm", t => t.IndividualApplicationFormID)
                .Index(t => t.IndividualApplicationFormID);
            
            CreateTable(
                "dbo.WorkshopPrivateInvitations",
                c => new
                    {
                        WorkshopPrivateInvitationId = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        InvitationStatus = c.Int(nullable: false),
                        FrontendUserID = c.Guid(),
                        IncubationWorkshopID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.WorkshopPrivateInvitationId)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.FrontendUserID)
                .Index(t => t.IncubationWorkshopID);
            
            CreateTable(
                "dbo.WorkshopProjectProposals",
                c => new
                    {
                        WorkshopProjectProposalID = c.Guid(nullable: false),
                        WorkshopProjectProposalStatusID = c.Guid(nullable: false),
                        IncubationWorkshopID = c.Guid(nullable: false),
                        FrontendUserID = c.Guid(nullable: false),
                        WorkshopPP_InvitationStatus = c.Int(nullable: false),
                        Feedback = c.String(),
                    })
                .PrimaryKey(t => t.WorkshopProjectProposalID)
                .ForeignKey("dbo.WorkshopProjectProposalStatus", t => t.WorkshopProjectProposalStatusID)
                .ForeignKey("dbo.IncubationWorkshops", t => t.IncubationWorkshopID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.WorkshopProjectProposalStatusID)
                .Index(t => t.IncubationWorkshopID)
                .Index(t => t.FrontendUserID);
            
            CreateTable(
                "dbo.EmployeesAttendIncubationWorkShops",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        PositionTasks = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        EducationalQualificationAndSpecialization = c.String(nullable: false),
                        WorkshopProjectProposalID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkshopProjectProposals", t => t.WorkshopProjectProposalID)
                .Index(t => t.WorkshopProjectProposalID);
            
            CreateTable(
                "dbo.WorkshopProjectProposalAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        WorkshopProjectProposalID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.WorkshopProjectProposals", t => t.WorkshopProjectProposalID)
                .Index(t => t.WorkshopProjectProposalID);
            
            CreateTable(
                "dbo.WorkshopProjectProposalStatus",
                c => new
                    {
                        WorkshopProjectProposalStatusID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.WorkshopProjectProposalStatusID);
            
            CreateTable(
                "dbo.ClassificationSector",
                c => new
                    {
                        ClassificationSectorID = c.Guid(nullable: false),
                        CorporationsCategoryID = c.Guid(nullable: false),
                        ClassificationSectorNameAR = c.String(nullable: false, maxLength: 50),
                        ClassificationSectorNameEN = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClassificationSectorID)
                .ForeignKey("dbo.CorporationsCategory", t => t.CorporationsCategoryID)
                .Index(t => t.CorporationsCategoryID);
            
            CreateTable(
                "dbo.CorporationsCategory",
                c => new
                    {
                        CorporationsCategoryID = c.Guid(nullable: false),
                        CorporationsCategoryNameAR = c.String(nullable: false, maxLength: 50),
                        CorporationsCategoryNameEN = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CorporationsCategoryID);
            
            CreateTable(
                "dbo.CorporateApplicationFormAttachment",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        CorporateApplicationFormID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.CorporateApplicationForm", t => t.CorporateApplicationFormID)
                .Index(t => t.CorporateApplicationFormID);
            
            CreateTable(
                "dbo.CorporateFieldOfWork",
                c => new
                    {
                        CorporateFieldOfWorkID = c.Guid(nullable: false),
                        CorporateFieldOfWorkNameAR = c.String(nullable: false, maxLength: 50),
                        CorporateFieldOfWorkNameEN = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CorporateFieldOfWorkID);
            
            CreateTable(
                "dbo.FrontendGroups",
                c => new
                    {
                        GroupID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.FrontendRoles",
                c => new
                    {
                        RoleID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                        IsCreate = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsUpdate = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.IncubationBaselines",
                c => new
                    {
                        IncubationBaselineID = c.Guid(nullable: false),
                        IncubationBaselineStatusID = c.Guid(nullable: false),
                        FrontendUserID = c.Guid(nullable: false),
                        submissionDate = c.DateTime(storeType: "date"),
                        Feadback = c.String(),
                        ReasonEstablishingCorporate = c.String(),
                        History = c.String(),
                        Vision = c.String(),
                        Mission = c.String(),
                        Objectives = c.String(),
                        HowPlanMade = c.String(),
                        StrategicPlan = c.String(),
                        OperatingPlan = c.String(),
                        StrategicObjective = c.String(),
                        BoardDirectorsCount = c.Int(),
                        DateBoardApproval = c.DateTime(storeType: "date"),
                        GovernanceGuide = c.String(),
                        InternalRegulation = c.Boolean(nullable: false),
                        BoardMeetingNumber = c.Int(),
                        BoardDirectorRoles = c.String(),
                        FRDevSpecialistsCount = c.Int(),
                        CorporateRevenuesLastYear = c.Int(),
                        SupportersCountLastYear = c.Int(),
                        DonorInstitutions = c.Boolean(nullable: false),
                        GovernmentAgencies = c.Boolean(nullable: false),
                        PrivateSector = c.Boolean(nullable: false),
                        Individuals = c.Boolean(nullable: false),
                        Investments = c.Boolean(nullable: false),
                        Alms = c.Boolean(nullable: false),
                        IndicateNumberAndSupportAmount = c.String(),
                        CurrentCorporateBudget = c.Int(),
                        CharteredAccountantNotesCount = c.Int(),
                        CharteredAccountantNotes = c.String(),
                        specializedPandPDepartmentEmpCount = c.Int(),
                        PlanAccordingStrategicObjectives = c.Boolean(nullable: false),
                        PlanImplementationActivitiesParticipationClarify = c.String(),
                        IndicateDevelopmentProjectsWorkingOrganization = c.String(),
                        BudgetAllocatedProjects = c.Int(),
                        BudgetAllocatedOperation = c.Int(),
                        DepartmentFollowUpEvaluationEmpCount = c.Int(),
                        OrganizationFollowUpEvaluationPlan = c.Boolean(nullable: false),
                        OrganizationFollowUpEvaluationReports = c.Boolean(nullable: false),
                        AttachFollowUpandEvaluationForms = c.String(),
                        DepartmentCorporateCommunicationPublicRelationsEmpCount = c.Int(),
                        CorporateCommunicationPlan = c.Boolean(nullable: false),
                        SeparateItemGeneralBudget = c.Boolean(nullable: false),
                        BudgetAllocatedCorporateCommunication = c.Int(),
                        DepartmentHREmpCount = c.Int(),
                        FullTimeStaff = c.Int(),
                        PartTimeStaff = c.Int(),
                        VolunteerStaff = c.Int(),
                        AttractEmployees = c.String(),
                        StaffPerformanceMonitored = c.String(),
                        OrganizationalStructureUnderHRManagement = c.String(),
                        TrainingNeedsCorporateStaffIdentified = c.String(),
                    })
                .PrimaryKey(t => t.IncubationBaselineID)
                .ForeignKey("dbo.IncubationBaselineStatus", t => t.IncubationBaselineStatusID)
                .ForeignKey("dbo.FrontendUsers", t => t.FrontendUserID)
                .Index(t => t.IncubationBaselineStatusID)
                .Index(t => t.FrontendUserID);
            
            CreateTable(
                "dbo.IncubationBaselineStatus",
                c => new
                    {
                        IncubationBaselineStatusID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IncubationBaselineStatusID);
            
            CreateTable(
                "dbo.BackendUserPositions",
                c => new
                    {
                        BackendUserPositionID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BackendUserPositionID);
            
            CreateTable(
                "dbo.ReasonTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BackendGroups",
                c => new
                    {
                        GroupID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.BackendRoles",
                c => new
                    {
                        RoleID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                        IsCreate = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsUpdate = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.EmailAttachments",
                c => new
                    {
                        AttachmentID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ScreenName = c.String(nullable: false, maxLength: 150),
                        Type = c.String(nullable: false, maxLength: 150),
                        Size = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 250),
                        EmailID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.Email", t => t.EmailID)
                .Index(t => t.EmailID);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        EmailID = c.Guid(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 50),
                        From = c.String(nullable: false),
                        To = c.String(nullable: false),
                        CC = c.String(nullable: false),
                        BC = c.String(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.EmailID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Guid(nullable: false),
                        EventNameEN = c.String(nullable: false, maxLength: 50),
                        EventNameAR = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.FrontendUserTypes",
                c => new
                    {
                        UserTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UserTypeID);
            
            CreateTable(
                "dbo.IncubationWorkshopTypes",
                c => new
                    {
                        IncubationWorkshopTypeID = c.Guid(nullable: false),
                        NameAR = c.String(nullable: false, maxLength: 50),
                        NameEN = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IncubationWorkshopTypeID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FrontendGroupRoles",
                c => new
                    {
                        GroupID = c.Guid(nullable: false),
                        RoleID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupID, t.RoleID })
                .ForeignKey("dbo.FrontendGroups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.FrontendRoles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.GroupID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.FrontendUserGroups",
                c => new
                    {
                        GroupID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupID, t.UserID })
                .ForeignKey("dbo.FrontendGroups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.FrontendUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.GroupID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.BackendGroupRoles",
                c => new
                    {
                        GroupID = c.Guid(nullable: false),
                        RoleID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupID, t.RoleID })
                .ForeignKey("dbo.BackendGroups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.BackendRoles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.GroupID)
                .Index(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailAttachments", "EmailID", "dbo.Email");
            DropForeignKey("dbo.BackendGroupRoles", "RoleID", "dbo.BackendRoles");
            DropForeignKey("dbo.BackendGroupRoles", "GroupID", "dbo.BackendGroups");
            DropForeignKey("dbo.IndividualApplicantStatus", "ApplicantStatusID", "dbo.ApplicantStatus");
            DropForeignKey("dbo.CorporateApplicationStatus", "ApplicantStatusID", "dbo.ApplicantStatus");
            DropForeignKey("dbo.CorporateApplicationStatus", "ResonTypeID", "dbo.ReasonTypes");
            DropForeignKey("dbo.CorporateApplicationStatus", "Fk_BackEndMakeAction", "dbo.BackendUsers");
            DropForeignKey("dbo.BackendUsers", "BackEndPositionId", "dbo.BackendUserPositions");
            DropForeignKey("dbo.BackendUsers", "FK_AspUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.FrontendUsers", "FK_AspUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkshopProjectProposals", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.WorkshopPrivateInvitations", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.IndividualApplicationForm", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.IncubationWorkshopRatings", "FrontendUserId", "dbo.FrontendUsers");
            DropForeignKey("dbo.IncubationWorkShopFollowUpEvaluations", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.Incubations", "FrontendUser_UserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.IncubationProjectProposals", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.IncubationFollowUpEvaluations", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.IncubationBaselines", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.IncubationBaselines", "IncubationBaselineStatusID", "dbo.IncubationBaselineStatus");
            DropForeignKey("dbo.FrontendUsers", "ParentID", "dbo.FrontendUsers");
            DropForeignKey("dbo.FrontendUserGroups", "UserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.FrontendUserGroups", "GroupID", "dbo.FrontendGroups");
            DropForeignKey("dbo.FrontendGroupRoles", "RoleID", "dbo.FrontendRoles");
            DropForeignKey("dbo.FrontendGroupRoles", "GroupID", "dbo.FrontendGroups");
            DropForeignKey("dbo.FollowUpProjectPlanRequests", "FrontendUserId", "dbo.FrontendUsers");
            DropForeignKey("dbo.CorporateApplicationForm", "FrontendUserID", "dbo.FrontendUsers");
            DropForeignKey("dbo.CorporateApplicationForm", "CorporateFieldOfWorkID", "dbo.CorporateFieldOfWork");
            DropForeignKey("dbo.CorporateApplicationStatus", "CorporateApplicationFormID", "dbo.CorporateApplicationForm");
            DropForeignKey("dbo.CorporateApplicationFormAttachment", "CorporateApplicationFormID", "dbo.CorporateApplicationForm");
            DropForeignKey("dbo.CorporateApplicationForm", "CorporationsCategoryID", "dbo.CorporationsCategory");
            DropForeignKey("dbo.ClassificationSector", "CorporationsCategoryID", "dbo.CorporationsCategory");
            DropForeignKey("dbo.CorporateApplicationForm", "ClassificationSectorID", "dbo.ClassificationSector");
            DropForeignKey("dbo.IndividualApplicationForm", "CityID", "dbo.Cities");
            DropForeignKey("dbo.IncubationWorkshops", "CityID", "dbo.Cities");
            DropForeignKey("dbo.IndividualApplicationForm", "GovernorateID", "dbo.Governorates");
            DropForeignKey("dbo.IncubationWorkshops", "GovernorateID", "dbo.Governorates");
            DropForeignKey("dbo.WorkshopProjectProposals", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.WorkshopProjectProposals", "WorkshopProjectProposalStatusID", "dbo.WorkshopProjectProposalStatus");
            DropForeignKey("dbo.WorkshopProjectProposalAttachments", "WorkshopProjectProposalID", "dbo.WorkshopProjectProposals");
            DropForeignKey("dbo.EmployeesAttendIncubationWorkShops", "WorkshopProjectProposalID", "dbo.WorkshopProjectProposals");
            DropForeignKey("dbo.WorkshopPrivateInvitations", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.IndividualApplicationForm", "RegionID", "dbo.Regions");
            DropForeignKey("dbo.TrainingCourses", "IndividualApplicationFormID", "dbo.IndividualApplicationForm");
            DropForeignKey("dbo.IndividualApplicationForm", "ProgramID", "dbo.Programs");
            DropForeignKey("dbo.CorporateApplicationForm", "ProgramID", "dbo.Programs");
            DropForeignKey("dbo.IndividualApplicationFormAttachment", "IndividualApplicationFormID", "dbo.IndividualApplicationForm");
            DropForeignKey("dbo.IndividualApplicantStatus", "IndividualApplicationFormID", "dbo.IndividualApplicationForm");
            DropForeignKey("dbo.EducationalQualifications", "IndividualApplicationFormID", "dbo.IndividualApplicationForm");
            DropForeignKey("dbo.IndividualApplicationForm", "Nationality", "dbo.CountriesAndNationalities");
            DropForeignKey("dbo.IncubationWorkshops", "RegionID", "dbo.Regions");
            DropForeignKey("dbo.Governorates", "RegionID", "dbo.Regions");
            DropForeignKey("dbo.CorporateApplicationForm", "RegionID", "dbo.Regions");
            DropForeignKey("dbo.IncubationWorkshopRatings", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.IncubationWorkshops", "IncubationWorkshopModelID", "dbo.IncubationWorkshopModels");
            DropForeignKey("dbo.IncubationWorkshopModels", "FK_AspUserCreateModel", "dbo.AspNetUsers");
            DropForeignKey("dbo.IncubationWorkShopFollowUpEvaluations", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.IncubationWorkShopFollowUpEvaluationLines", "IncubationWorkShopFollowUpEvaluationID", "dbo.IncubationWorkShopFollowUpEvaluations");
            DropForeignKey("dbo.IncubationWorkShopFollowUpEvaluationLines", "BackendUserID", "dbo.BackendUsers");
            DropForeignKey("dbo.IncubationWorkShopFollowUpEvaluationAttchments", "IncubationWorkShopFollowUpEvaluationID", "dbo.IncubationWorkShopFollowUpEvaluations");
            DropForeignKey("dbo.IncubationWorkshopAttachments", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.IncubationWorkshops", "IncubationtWorkshopStatusID", "dbo.IncubationtWorkshopStatus");
            DropForeignKey("dbo.FollowUpProjectPlans", "IncubationWorkshopID", "dbo.IncubationWorkshops");
            DropForeignKey("dbo.FollowUpProjectPlanRequests", "FollowUpProjectPlanId", "dbo.FollowUpProjectPlans");
            DropForeignKey("dbo.FollowUpProjectPlanRequestAttachments", "FollowUpProjectPlanRequestId", "dbo.FollowUpProjectPlanRequests");
            DropForeignKey("dbo.FollowUpProjectPlanAttachments", "FollowUpProjectPlanId", "dbo.FollowUpProjectPlans");
            DropForeignKey("dbo.IncubationWorkshops", "ConsultantID", "dbo.Consultants");
            DropForeignKey("dbo.Incubations", "ConsultantID", "dbo.Consultants");
            DropForeignKey("dbo.Incubations", "TypeOfInterventionID", "dbo.TypeOfIntervention");
            DropForeignKey("dbo.Incubations", "IncubationStatusID", "dbo.IncubationStatus");
            DropForeignKey("dbo.Incubations", "IncubationProjectProposalID", "dbo.IncubationProjectProposals");
            DropForeignKey("dbo.IncubationFollowUpEvaluations", "IncubationID", "dbo.Incubations");
            DropForeignKey("dbo.IncubationAttchments", "IncubationID", "dbo.Incubations");
            DropForeignKey("dbo.IncubationAdvertisings", "IncubationTypeID", "dbo.IncubationTypes");
            DropForeignKey("dbo.Incubations", "IncubationAdID", "dbo.IncubationAdvertisings");
            DropForeignKey("dbo.IncubationProjectProposals", "IncubationAdID", "dbo.IncubationAdvertisings");
            DropForeignKey("dbo.IncubationProjectProposals", "IncubationProjectProposalStatusID", "dbo.IncubationProjectProposalStatus");
            DropForeignKey("dbo.IncubationProjectProposalAttachements", "IncubationProjectProposalID", "dbo.IncubationProjectProposals");
            DropForeignKey("dbo.IncubationPrivateInvitations", "IncubationAdID", "dbo.IncubationAdvertisings");
            DropForeignKey("dbo.IncubationAdvertisingModels", "IncubationAdID", "dbo.IncubationAdvertisings");
            DropForeignKey("dbo.Incubations", "IncubationTypeID", "dbo.IncubationTypes");
            DropForeignKey("dbo.IncubationModels", "IncubationTypeID", "dbo.IncubationTypes");
            DropForeignKey("dbo.IncubationFollowUpEvaluations", "IncubationTypeID", "dbo.IncubationTypes");
            DropForeignKey("dbo.IncubationFollowUpEvaluationLines", "IncubationFollowUpEvaluationID", "dbo.IncubationFollowUpEvaluations");
            DropForeignKey("dbo.IncubationFollowUpEvaluationLines", "BackendUserID", "dbo.BackendUsers");
            DropForeignKey("dbo.IncubationFollowUpEvaluationAttchments", "IncubationFollowUpEvaluationID", "dbo.IncubationFollowUpEvaluations");
            DropForeignKey("dbo.Incubations", "IncubationModelID", "dbo.IncubationModels");
            DropForeignKey("dbo.IncubationAdvertisingModels", "IncubationModelID", "dbo.IncubationModels");
            DropForeignKey("dbo.IncubationModels", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.IncubationAdvertisingAttachments", "IncubationAdID", "dbo.IncubationAdvertisings");
            DropForeignKey("dbo.IncubationWorkshops", "FundingSourceID", "dbo.FundingSource");
            DropForeignKey("dbo.IncubationAdvertisings", "FundingSourceID", "dbo.FundingSource");
            DropForeignKey("dbo.Incubations", "SpecialistChargeOfIncubation", "dbo.BackendUsers");
            DropForeignKey("dbo.ConsultantAttachments", "ConsultantID", "dbo.Consultants");
            DropForeignKey("dbo.CorporateApplicationForm", "GovernorateID", "dbo.Governorates");
            DropForeignKey("dbo.Cities", "GovernorateID", "dbo.Governorates");
            DropForeignKey("dbo.CorporateApplicationForm", "CityID", "dbo.Cities");
            DropForeignKey("dbo.CorporateApplicationForm", "AuthorizationAuthorityID", "dbo.AuthorizationAuthority");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.BackendGroupRoles", new[] { "RoleID" });
            DropIndex("dbo.BackendGroupRoles", new[] { "GroupID" });
            DropIndex("dbo.FrontendUserGroups", new[] { "UserID" });
            DropIndex("dbo.FrontendUserGroups", new[] { "GroupID" });
            DropIndex("dbo.FrontendGroupRoles", new[] { "RoleID" });
            DropIndex("dbo.FrontendGroupRoles", new[] { "GroupID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.EmailAttachments", new[] { "EmailID" });
            DropIndex("dbo.IncubationBaselines", new[] { "FrontendUserID" });
            DropIndex("dbo.IncubationBaselines", new[] { "IncubationBaselineStatusID" });
            DropIndex("dbo.CorporateApplicationFormAttachment", new[] { "CorporateApplicationFormID" });
            DropIndex("dbo.ClassificationSector", new[] { "CorporationsCategoryID" });
            DropIndex("dbo.WorkshopProjectProposalAttachments", new[] { "WorkshopProjectProposalID" });
            DropIndex("dbo.EmployeesAttendIncubationWorkShops", new[] { "WorkshopProjectProposalID" });
            DropIndex("dbo.WorkshopProjectProposals", new[] { "FrontendUserID" });
            DropIndex("dbo.WorkshopProjectProposals", new[] { "IncubationWorkshopID" });
            DropIndex("dbo.WorkshopProjectProposals", new[] { "WorkshopProjectProposalStatusID" });
            DropIndex("dbo.WorkshopPrivateInvitations", new[] { "IncubationWorkshopID" });
            DropIndex("dbo.WorkshopPrivateInvitations", new[] { "FrontendUserID" });
            DropIndex("dbo.TrainingCourses", new[] { "IndividualApplicationFormID" });
            DropIndex("dbo.IndividualApplicationFormAttachment", new[] { "IndividualApplicationFormID" });
            DropIndex("dbo.IndividualApplicantStatus", new[] { "ApplicantStatusID" });
            DropIndex("dbo.IndividualApplicantStatus", new[] { "IndividualApplicationFormID" });
            DropIndex("dbo.EducationalQualifications", new[] { "IndividualApplicationFormID" });
            DropIndex("dbo.IndividualApplicationForm", new[] { "FrontendUserID" });
            DropIndex("dbo.IndividualApplicationForm", new[] { "ProgramID" });
            DropIndex("dbo.IndividualApplicationForm", new[] { "CityID" });
            DropIndex("dbo.IndividualApplicationForm", new[] { "GovernorateID" });
            DropIndex("dbo.IndividualApplicationForm", new[] { "RegionID" });
            DropIndex("dbo.IndividualApplicationForm", new[] { "Nationality" });
            DropIndex("dbo.IncubationWorkshopRatings", new[] { "IncubationWorkshopID" });
            DropIndex("dbo.IncubationWorkshopRatings", new[] { "FrontendUserId" });
            DropIndex("dbo.IncubationWorkshopModels", new[] { "FK_AspUserCreateModel" });
            DropIndex("dbo.IncubationWorkShopFollowUpEvaluationLines", new[] { "BackendUserID" });
            DropIndex("dbo.IncubationWorkShopFollowUpEvaluationLines", new[] { "IncubationWorkShopFollowUpEvaluationID" });
            DropIndex("dbo.IncubationWorkShopFollowUpEvaluationAttchments", new[] { "IncubationWorkShopFollowUpEvaluationID" });
            DropIndex("dbo.IncubationWorkShopFollowUpEvaluations", new[] { "IncubationWorkshopID" });
            DropIndex("dbo.IncubationWorkShopFollowUpEvaluations", new[] { "FrontendUserID" });
            DropIndex("dbo.IncubationWorkshopAttachments", new[] { "IncubationWorkshopID" });
            DropIndex("dbo.FollowUpProjectPlanRequestAttachments", new[] { "FollowUpProjectPlanRequestId" });
            DropIndex("dbo.FollowUpProjectPlanRequests", new[] { "FrontendUserId" });
            DropIndex("dbo.FollowUpProjectPlanRequests", new[] { "FollowUpProjectPlanId" });
            DropIndex("dbo.FollowUpProjectPlanAttachments", new[] { "FollowUpProjectPlanId" });
            DropIndex("dbo.FollowUpProjectPlans", new[] { "IncubationWorkshopID" });
            DropIndex("dbo.IncubationAttchments", new[] { "IncubationID" });
            DropIndex("dbo.IncubationProjectProposalAttachements", new[] { "IncubationProjectProposalID" });
            DropIndex("dbo.IncubationProjectProposals", new[] { "FrontendUserID" });
            DropIndex("dbo.IncubationProjectProposals", new[] { "IncubationAdID" });
            DropIndex("dbo.IncubationProjectProposals", new[] { "IncubationProjectProposalStatusID" });
            DropIndex("dbo.IncubationPrivateInvitations", new[] { "IncubationAdID" });
            DropIndex("dbo.IncubationFollowUpEvaluationLines", new[] { "BackendUserID" });
            DropIndex("dbo.IncubationFollowUpEvaluationLines", new[] { "IncubationFollowUpEvaluationID" });
            DropIndex("dbo.IncubationFollowUpEvaluationAttchments", new[] { "IncubationFollowUpEvaluationID" });
            DropIndex("dbo.IncubationFollowUpEvaluations", new[] { "IncubationTypeID" });
            DropIndex("dbo.IncubationFollowUpEvaluations", new[] { "IncubationID" });
            DropIndex("dbo.IncubationFollowUpEvaluations", new[] { "FrontendUserID" });
            DropIndex("dbo.IncubationModels", new[] { "AspNetUser_Id" });
            DropIndex("dbo.IncubationModels", new[] { "IncubationTypeID" });
            DropIndex("dbo.IncubationAdvertisingModels", new[] { "IncubationAdID" });
            DropIndex("dbo.IncubationAdvertisingModels", new[] { "IncubationModelID" });
            DropIndex("dbo.IncubationAdvertisingAttachments", new[] { "IncubationAdID" });
            DropIndex("dbo.IncubationAdvertisings", new[] { "FundingSourceID" });
            DropIndex("dbo.IncubationAdvertisings", new[] { "IncubationTypeID" });
            DropIndex("dbo.Incubations", new[] { "FrontendUser_UserID" });
            DropIndex("dbo.Incubations", new[] { "IncubationProjectProposalID" });
            DropIndex("dbo.Incubations", new[] { "SpecialistChargeOfIncubation" });
            DropIndex("dbo.Incubations", new[] { "TypeOfInterventionID" });
            DropIndex("dbo.Incubations", new[] { "ConsultantID" });
            DropIndex("dbo.Incubations", new[] { "IncubationAdID" });
            DropIndex("dbo.Incubations", new[] { "IncubationStatusID" });
            DropIndex("dbo.Incubations", new[] { "IncubationTypeID" });
            DropIndex("dbo.Incubations", new[] { "IncubationModelID" });
            DropIndex("dbo.ConsultantAttachments", new[] { "ConsultantID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "FundingSourceID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "CityID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "GovernorateID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "RegionID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "IncubationtWorkshopStatusID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "IncubationWorkshopModelID" });
            DropIndex("dbo.IncubationWorkshops", new[] { "ConsultantID" });
            DropIndex("dbo.Governorates", new[] { "RegionID" });
            DropIndex("dbo.Cities", new[] { "GovernorateID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "FrontendUserID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "CityID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "GovernorateID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "RegionID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "ProgramID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "AuthorizationAuthorityID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "ClassificationSectorID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "CorporateFieldOfWorkID" });
            DropIndex("dbo.CorporateApplicationForm", new[] { "CorporationsCategoryID" });
            DropIndex("dbo.FrontendUsers", new[] { "FK_AspUser" });
            DropIndex("dbo.FrontendUsers", new[] { "ParentID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.BackendUsers", new[] { "BackEndPositionId" });
            DropIndex("dbo.BackendUsers", new[] { "FK_AspUser" });
            DropIndex("dbo.CorporateApplicationStatus", new[] { "Fk_BackEndMakeAction" });
            DropIndex("dbo.CorporateApplicationStatus", new[] { "ResonTypeID" });
            DropIndex("dbo.CorporateApplicationStatus", new[] { "ApplicantStatusID" });
            DropIndex("dbo.CorporateApplicationStatus", new[] { "CorporateApplicationFormID" });
            DropTable("dbo.BackendGroupRoles");
            DropTable("dbo.FrontendUserGroups");
            DropTable("dbo.FrontendGroupRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.IncubationWorkshopTypes");
            DropTable("dbo.FrontendUserTypes");
            DropTable("dbo.Events");
            DropTable("dbo.Email");
            DropTable("dbo.EmailAttachments");
            DropTable("dbo.BackendRoles");
            DropTable("dbo.BackendGroups");
            DropTable("dbo.ReasonTypes");
            DropTable("dbo.BackendUserPositions");
            DropTable("dbo.IncubationBaselineStatus");
            DropTable("dbo.IncubationBaselines");
            DropTable("dbo.FrontendRoles");
            DropTable("dbo.FrontendGroups");
            DropTable("dbo.CorporateFieldOfWork");
            DropTable("dbo.CorporateApplicationFormAttachment");
            DropTable("dbo.CorporationsCategory");
            DropTable("dbo.ClassificationSector");
            DropTable("dbo.WorkshopProjectProposalStatus");
            DropTable("dbo.WorkshopProjectProposalAttachments");
            DropTable("dbo.EmployeesAttendIncubationWorkShops");
            DropTable("dbo.WorkshopProjectProposals");
            DropTable("dbo.WorkshopPrivateInvitations");
            DropTable("dbo.TrainingCourses");
            DropTable("dbo.Programs");
            DropTable("dbo.IndividualApplicationFormAttachment");
            DropTable("dbo.IndividualApplicantStatus");
            DropTable("dbo.EducationalQualifications");
            DropTable("dbo.CountriesAndNationalities");
            DropTable("dbo.IndividualApplicationForm");
            DropTable("dbo.Regions");
            DropTable("dbo.IncubationWorkshopRatings");
            DropTable("dbo.IncubationWorkshopModels");
            DropTable("dbo.IncubationWorkShopFollowUpEvaluationLines");
            DropTable("dbo.IncubationWorkShopFollowUpEvaluationAttchments");
            DropTable("dbo.IncubationWorkShopFollowUpEvaluations");
            DropTable("dbo.IncubationWorkshopAttachments");
            DropTable("dbo.IncubationtWorkshopStatus");
            DropTable("dbo.FollowUpProjectPlanRequestAttachments");
            DropTable("dbo.FollowUpProjectPlanRequests");
            DropTable("dbo.FollowUpProjectPlanAttachments");
            DropTable("dbo.FollowUpProjectPlans");
            DropTable("dbo.TypeOfIntervention");
            DropTable("dbo.IncubationStatus");
            DropTable("dbo.IncubationAttchments");
            DropTable("dbo.IncubationProjectProposalStatus");
            DropTable("dbo.IncubationProjectProposalAttachements");
            DropTable("dbo.IncubationProjectProposals");
            DropTable("dbo.IncubationPrivateInvitations");
            DropTable("dbo.IncubationFollowUpEvaluationLines");
            DropTable("dbo.IncubationFollowUpEvaluationAttchments");
            DropTable("dbo.IncubationFollowUpEvaluations");
            DropTable("dbo.IncubationTypes");
            DropTable("dbo.IncubationModels");
            DropTable("dbo.IncubationAdvertisingModels");
            DropTable("dbo.IncubationAdvertisingAttachments");
            DropTable("dbo.FundingSource");
            DropTable("dbo.IncubationAdvertisings");
            DropTable("dbo.Incubations");
            DropTable("dbo.ConsultantAttachments");
            DropTable("dbo.Consultants");
            DropTable("dbo.IncubationWorkshops");
            DropTable("dbo.Governorates");
            DropTable("dbo.Cities");
            DropTable("dbo.AuthorizationAuthority");
            DropTable("dbo.CorporateApplicationForm");
            DropTable("dbo.FrontendUsers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.BackendUsers");
            DropTable("dbo.CorporateApplicationStatus");
            DropTable("dbo.ApplicantStatus");
        }
    }
}
