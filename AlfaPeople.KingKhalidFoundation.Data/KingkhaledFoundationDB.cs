using System;
using System.Data.Entity;
using AlfaPeople.KingKhalidFoundation.Data.Model;


namespace AlfaPeople.KingKhalidFoundation.Data
{
    public partial class KingkhaledFoundationDB : DbContext
    {
        public KingkhaledFoundationDB()
            : base("name=KKF_CBP_DB")
        {
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new KingKhalidFoundationDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicantStatu>()
                .HasMany(e => e.CorporateApplicationStatus)
                .WithRequired(e => e.ApplicantStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicantStatu>()
                .HasMany(e => e.IndividualApplicantStatus)
                .WithRequired(e => e.ApplicantStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.FrontendUsers)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.FK_AspUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AuthorizationAuthority>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithRequired(e => e.AuthorizationAuthority)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BackendGroup>()
                .HasMany(e => e.BackendRoles)
                .WithMany(e => e.BackendGroups)
                .Map(m => m.ToTable("BackendGroupRoles").MapLeftKey("GroupID").MapRightKey("RoleID"));

            modelBuilder.Entity<BackendUserPosition>()
                .HasMany(e => e.BackendUsers)
                .WithRequired(e => e.BackendUserPositions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithOptional(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.IncubationWorkshops)
                .WithOptional(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.IndividualApplicationForms)
                .WithOptional(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Consultant>()
                .HasMany(e => e.ConsultantAttachments)
                .WithRequired(e => e.Consultant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Consultant>()
                .HasMany(e => e.Incubations)
                .WithRequired(e => e.Consultant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Consultant>()
                .HasMany(e => e.IncubationWorkshops)
                .WithRequired(e => e.Consultant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CorporateApplicationForm>()
                .HasMany(e => e.CorporateApplicationFormAttachments)
                .WithRequired(e => e.CorporateApplicationForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CorporateApplicationForm>()
                .HasOptional(e => e.CorporateApplicationStatu)
                .WithRequired(e => e.CorporateApplicationForm);

            modelBuilder.Entity<CorporationsCategory>()
                .HasMany(e => e.ClassificationSectors)
                .WithRequired(e => e.CorporationsCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CorporationsCategory>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithRequired(e => e.CorporationsCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CorporateFieldOfWork>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithOptional(e => e.CorporateFieldOfWork)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CountriesAndNationality>()
                .HasMany(e => e.IndividualApplicationForms)
                .WithRequired(e => e.CountriesAndNationality)
                .HasForeignKey(e => e.Nationality)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Email>()
                .HasMany(e => e.EmailAttachments)
                .WithRequired(e => e.Email)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendGroup>()
                .HasMany(e => e.FrontendRoles)
                .WithMany(e => e.FrontendGroups)
                .Map(m => m.ToTable("FrontendGroupRoles").MapLeftKey("GroupID").MapRightKey("RoleID"));

            modelBuilder.Entity<FrontendGroup>()
                .HasMany(e => e.FrontendUsers)
                .WithMany(e => e.FrontendGroups)
                .Map(m => m.ToTable("FrontendUserGroups").MapLeftKey("GroupID").MapRightKey("UserID"));

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.IncubationProjectProposals)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationFollowUpEvaluation>()
                .HasMany(e => e.IncubationFollowUpEvaluationLines)
                .WithRequired(e => e.IncubationFollowUpEvaluation)
                .HasForeignKey(e => e.IncubationFollowUpEvaluationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationFollowUpEvaluation>()
                .HasMany(e => e.IncubationFollowUpEvaluationAttchments)
                .WithRequired(e => e.IncubationFollowUpEvaluation)
                .HasForeignKey(e => e.IncubationFollowUpEvaluationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkShopFollowUpEvaluation>()
                .HasMany(e => e.IncubationWorkShopFollowUpEvaluationLines)
                .WithRequired(e => e.IncubationWorkShopFollowUpEvaluation)
                .HasForeignKey(e => e.IncubationWorkShopFollowUpEvaluationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkShopFollowUpEvaluation>()
                .HasMany(e => e.IncubationWorkShopFollowUpEvaluationAttchments)
                .WithRequired(e => e.IncubationWorkShopFollowUpEvaluation)
                .HasForeignKey(e => e.IncubationWorkShopFollowUpEvaluationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.WorkshopPrivateInvitations)
                .WithOptional(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.WorkshopProjectProposals)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.IndividualApplicationForms)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.FrontendUsers1)
                .WithOptional(e => e.FrontendUser1)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.IncubationBaselines)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationBaselineStatus>()
                .HasMany(e => e.IncubationBaselines)
                .WithRequired(e => e.IncubationBaselineStatus)
                .HasForeignKey(e => e.IncubationBaselineStatusID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Governorate>()
                .HasMany(e => e.Cities)
                .WithRequired(e => e.Governorate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Governorate>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithRequired(e => e.Governorate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Governorate>()
                .HasMany(e => e.IncubationWorkshops)
                .WithRequired(e => e.Governorate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Governorate>()
                .HasMany(e => e.IndividualApplicationForms)
                .WithRequired(e => e.Governorate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationModel>()
                .HasMany(e => e.Incubations)
                .WithRequired(e => e.IncubationModel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationProjectProposal>()
                .HasMany(e => e.IncubationProjectProposalAttachements)
                .WithRequired(e => e.IncubationProjectProposal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationProjectProposalStatu>()
                .HasMany(e => e.IncubationProjectProposals)
                .WithRequired(e => e.IncubationProjectProposalStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkshopProjectProposalStatu>()
                .HasMany(e => e.WorkshopProjectProposals)
                .WithRequired(e => e.WorkshopProjectProposalStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkshopProjectProposal>()
                .HasMany(e => e.WorkshopProjectProposalAttachments)
                .WithRequired(e => e.WorkshopProjectProposal)
                .HasForeignKey(e => e.WorkshopProjectProposalID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Incubation>()
                .HasMany(e => e.IncubationAttchments)
                .WithRequired(e => e.Incubation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FundingSource>()
                .HasMany(e => e.IncubationAdvertisings)
                .WithRequired(e => e.FundingSource)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FundingSource>()
                .HasMany(e => e.IncubationWorkshops)
                .WithRequired(e => e.FundingSource)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeOfIntervention>()
                .HasMany(e => e.Incubations)
                .WithRequired(e => e.TypeOfIntervention)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshop>()
                .HasMany(e => e.WorkshopProjectProposals)
                .WithRequired(e => e.IncubationWorkshop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshop>()
                .HasMany(e => e.WorkshopPrivateInvitations)
                .WithRequired(e => e.IncubationWorkshop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkshopProjectProposal>()
                .HasMany(e => e.EmployeesAttendIncubationWorkShops)
                .WithRequired(e => e.WorkshopProjectProposal)
                .HasForeignKey(e => e.WorkshopProjectProposalID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
               .HasMany(e => e.IncubationWorkshopRatings)
               .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshop>()
               .HasMany(e => e.IncubationWorkshopRatings)
               .WithRequired(e => e.IncubationWorkshop)
                .HasForeignKey(e => e.IncubationWorkshopID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationModel>()
                .HasMany(e => e.IncubationAdvertisingModels)
                .WithRequired(e => e.IncubationModel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationAdvertising>()
                .HasMany(e => e.IncubationAdvertisingModels)
                .WithRequired(e => e.IncubationAdvertising)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationAdvertising>()
                .HasMany(e => e.Incubations)
                .WithRequired(e => e.IncubationAdvertising)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationAdvertising>()
                .HasMany(e => e.IncubationAdvertisingAttachments)
                .WithRequired(e => e.IncubationAdvertising)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationAdvertising>()
                .HasMany(e => e.IncubationPrivateInvitations)
                .WithRequired(e => e.IncubationAdvertising)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationAdvertising>()
                .HasMany(e => e.IncubationProjectProposals)
                .WithRequired(e => e.IncubationAdvertising)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.IncubationFollowUpEvaluations)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
               .HasMany(e => e.IncubationWorkShopFollowUpEvaluations)
               .WithRequired(e => e.FrontendUser)
               .HasForeignKey(e => e.FrontendUserID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshop>()
               .HasMany(e => e.IncubationWorkShopFollowUpEvaluations)
               .WithRequired(e => e.IncubationWorkshop)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Incubation>()
               .HasMany(e => e.IncubationFollowUpEvaluations)
               .WithRequired(e => e.Incubation)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationStatus>()
                .HasMany(e => e.Incubations)
                .WithRequired(e => e.IncubationStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationtWorkshopStatu>()
                .HasMany(e => e.IncubationWorkshops)
                .WithRequired(e => e.IncubationtWorkshopStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationType>()
                .HasMany(e => e.Incubations)
                .WithRequired(e => e.IncubationType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationType>()
                .HasMany(e => e.IncubationFollowUpEvaluations)
                .WithRequired(e => e.IncubationType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationType>()
                .HasMany(e => e.IncubationModels)
                .WithRequired(e => e.IncubationType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopModel>()
                .HasMany(e => e.IncubationWorkshops)
                .WithRequired(e => e.IncubationWorkshopModel)
                .HasForeignKey(e => e.IncubationWorkshopModelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshop>()
                .HasMany(e => e.IncubationWorkshopAttachments)
                .WithRequired(e => e.IncubationWorkshop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndividualApplicationForm>()
                .HasMany(e => e.EducationalQualifications)
                .WithRequired(e => e.IndividualApplicationForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndividualApplicationForm>()
                .HasOptional(e => e.IndividualApplicantStatu)
                .WithRequired(e => e.IndividualApplicationForm);

            modelBuilder.Entity<IndividualApplicationForm>()
                .HasMany(e => e.IndividualApplicationFormAttachments)
                .WithRequired(e => e.IndividualApplicationForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IndividualApplicationForm>()
                .HasMany(e => e.TrainingCourses)
                .WithRequired(e => e.IndividualApplicationForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Program>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithRequired(e => e.Program)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Program>()
                .HasMany(e => e.IndividualApplicationForms)
                .WithRequired(e => e.Program)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.CorporateApplicationForms)
                .WithRequired(e => e.Region)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.Governorates)
                .WithRequired(e => e.Region)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.IncubationWorkshops)
                .WithRequired(e => e.Region)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.IndividualApplicationForms)
                .WithRequired(e => e.Region)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FollowUpProjectPlan>()
                .HasMany(e => e.FollowUpProjectPlanAttachments)
                .WithRequired(e => e.FollowUpProjectPlan)
                .HasForeignKey(e => e.FollowUpProjectPlanId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FollowUpProjectPlan>()
                .HasMany(e => e.FollowUpProjectPlanRequests)
                .WithRequired(e => e.FollowUpProjectPlan)
                .HasForeignKey(e => e.FollowUpProjectPlanId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshop>()
                .HasMany(e => e.FollowUpProjectPlans)
                .WithRequired(e => e.IncubationWorkshop)
                .HasForeignKey(e => e.IncubationWorkshopID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FollowUpProjectPlanRequest>()
                .HasMany(e => e.FollowUpProjectPlanRequestAttachments)
                .WithRequired(e => e.FollowUpProjectPlanRequest)
                .HasForeignKey(e => e.FollowUpProjectPlanRequestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.FollowUpProjectPlanRequests)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopControls>()
                .HasMany(e => e.IncubationWorkshopControlsTypes)
                .WithRequired(e => e.IncubationWorkshopControl)
                .HasForeignKey(e => e.ControlsID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopControlsType>()
                .HasMany(e => e.IncubationWorkshopBLTrans)
                .WithRequired(e => e.IncubationWSControlsType)
                .HasForeignKey(e => e.ControlTypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopBLTransactionsType>()
                .HasMany(e => e.IncubationWorkshopBLTrans)
                .WithRequired(e => e.IncubationWorkshopBLTransType)
                .HasForeignKey(e => e.TransTypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopBLTransactions>()
                .HasMany(e => e.IncubationWSBLTransactionsValue)
                .WithRequired(e => e.IncubationWorkshopBLTrans)
                .HasForeignKey(e => e.TransID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopBLTransactionsValue>()
                .HasMany(e => e.IncubationWSBLTransValueAttachment)
                .WithRequired(e => e.IncubationWSBLTransValue)
                .HasForeignKey(e => e.TransValueID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FrontendUser>()
                .HasMany(e => e.IncubationWSBLTransactionsValue)
                .WithRequired(e => e.FrontendUser)
                .HasForeignKey(e => e.FrontendUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IncubationWorkshopBLTransValStatus>()
                .HasMany(e => e.IncubationWSBLTransactionsValue)
                .WithRequired(e => e.IncubationWorkshopBLTransValStatus)
                .HasForeignKey(e => e.IncubationWorkshopBLTransValStatusID)
                .WillCascadeOnDelete(false);
            //shadia
            // تكوين العلاقة بين IncubationWorkshopControlOptions و IncubationWorkshopOptionValues
            modelBuilder.Entity<IncubationWorkshopOptionValues>()
                .HasRequired(o => o.Option)
                .WithMany(opt => opt.OptionValues)
                .HasForeignKey(o => o.OptionID)
                .WillCascadeOnDelete(false);


            // إزالة العلاقة الإضافية لمنع تكرار المفتاح TransValueID
            modelBuilder.Entity<IncubationWorkshopOptionValues>()
                .HasRequired(v => v.TransactionsValue)
                .WithMany(t => t.OptionValues)
                .HasForeignKey(v => v.TransValueID)
                .WillCascadeOnDelete(true);


            modelBuilder.Entity<IncubationWorkshopBLTransactions>()
     .HasRequired(t => t.IncubationWorkshop)        // العلاقة المطلوبة
     .WithMany(w => w.IncubationWorkshopBLTransactions)  // العلاقة العكسية
     .HasForeignKey(t => t.IncubationWorkshopID)             // المفتاح الأجنبي
     .WillCascadeOnDelete(true);                   // الحذف المتتابع (اختياري)



        }
        //shadia
        public virtual DbSet<IncubationWorkshopControlOptions> IncubationWorkshopControlOptions { get; set; }
        public virtual DbSet<IncubationWorkshopOptionValues> IncubationWorkshopOptionValues { get; set; }

        //
        public virtual DbSet<ApplicantStatu> ApplicantStatus { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AuthorizationAuthority> AuthorizationAuthorities { get; set; }
        public virtual DbSet<BackendGroup> BackendGroups { get; set; }
        public virtual DbSet<BackendRole> BackendRoles { get; set; }
        public virtual DbSet<BackendUserPosition> BackendUserPositions { get; set; }
        public virtual DbSet<BackendUser> BackendUsers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ClassificationSector> ClassificationSectors { get; set; }
        public virtual DbSet<ConsultantAttachment> ConsultantAttachments { get; set; }
        public virtual DbSet<Consultant> Consultants { get; set; }
        public virtual DbSet<CorporateApplicationForm> CorporateApplicationForms { get; set; }
        public virtual DbSet<CorporateApplicationFormAttachment> CorporateApplicationFormAttachments { get; set; }
        public virtual DbSet<CorporateApplicationStatu> CorporateApplicationStatus { get; set; }
        public virtual DbSet<CorporationsCategory> CorporationsCategories { get; set; }
        public virtual DbSet<CorporateFieldOfWork> CorporateFieldOfWork { get; set; }
        public virtual DbSet<CountriesAndNationality> CountriesAndNationalities { get; set; }
        public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<EmailAttachment> EmailAttachments { get; set; }
        public virtual DbSet<FrontendGroup> FrontendGroups { get; set; }
        public virtual DbSet<FrontendRole> FrontendRoles { get; set; }
        public virtual DbSet<FrontendUser> FrontendUsers { get; set; }
        public virtual DbSet<FrontendUserType> FrontendUserTypes { get; set; }
        public virtual DbSet<Governorate> Governorates { get; set; }
        public virtual DbSet<FundingSource> FundingSources { get; set; }
        public virtual DbSet<TypeOfIntervention> TypeOfInterventions { get; set; }
        public virtual DbSet<IncubationAttchment> IncubationAttchments { get; set; }
        public virtual DbSet<IncubationModel> IncubationModels { get; set; }
        public virtual DbSet<IncubationProjectProposalAttachement> IncubationProjectProposalAttachements { get; set; }
        public virtual DbSet<IncubationProjectProposal> IncubationProjectProposals { get; set; }
        public virtual DbSet<WorkshopProjectProposal> WorkshopProjectProposals { get; set; }
        public virtual DbSet<IncubationProjectProposalStatu> IncubationProjectProposalStatus { get; set; }
        public virtual DbSet<WorkshopProjectProposalStatu> WorkshopProjectProposalStatus { get; set; }
        public virtual DbSet<WorkshopProjectProposalAttachment> WorkshopProjectProposalAttachments { get; set; }
        public virtual DbSet<Incubation> Incubations { get; set; }
        public virtual DbSet<IncubationStatus> IncubationStatus { get; set; }
        public virtual DbSet<IncubationtWorkshopStatu> IncubationtWorkshopStatus { get; set; }
        public virtual DbSet<WorkshopPrivateInvitation> WorkshopPrivateInvitations { get; set; }
        public virtual DbSet<IncubationPrivateInvitation> IncubationPrivateInvitations { get; set; }
        public virtual DbSet<IncubationType> IncubationTypes { get; set; }
        public virtual DbSet<IncubationWorkshopAttachment> IncubationWorkshopAttachments { get; set; }
        public virtual DbSet<IncubationWorkshopModel> IncubationWorkshopModels { get; set; }
        public virtual DbSet<IncubationWorkshop> IncubationWorkshops { get; set; }
        public virtual DbSet<IncubationWorkshopType> IncubationWorkshopTypes { get; set; }
        public virtual DbSet<IndividualApplicantStatu> IndividualApplicantStatus { get; set; }
        public virtual DbSet<IndividualApplicationForm> IndividualApplicationForms { get; set; }
        public virtual DbSet<IndividualApplicationFormAttachment> IndividualApplicationFormAttachments { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<ReasonType> ReasonType { get; set; }
        public virtual DbSet<TrainingCours> TrainingCourses { get; set; }
        public virtual DbSet<IncubationAdvertising> IncubationAdvertisings { get; set; }
        public virtual DbSet<IncubationAdvertisingAttachment> IncubationAdvertisingAttachments { get; set; }
        public virtual DbSet<IncubationAdvertisingModel> IncubationAdvertisingModels { get; set; }
        public virtual DbSet<IncubationFollowUpEvaluation> IncubationFollowUpEvaluations { get; set; }
        public virtual DbSet<IncubationFollowUpEvaluationAttchment> IncubationFollowUpEvaluationAttchments { get; set; }
        public virtual DbSet<IncubationFollowUpEvaluationLines> IncubationFollowUpEvaluationLines { get; set; }
        public virtual DbSet<IncubationWorkShopFollowUpEvaluation> IncubationWorkShopFollowUpEvaluations { get; set; }
        public virtual DbSet<IncubationWorkShopFollowUpEvaluationAttchment> IncubationWorkShopFollowUpEvaluationAttchments { get; set; }
        public virtual DbSet<IncubationWorkShopFollowUpEvaluationLines> IncubationWorkShopFollowUpEvaluationLines { get; set; }
        public virtual DbSet<IncubationWorkshopRating> IncubationWorkshopRating { get; set; }
        public virtual DbSet<EmployeesAttendIncubationWorkShop> EmployeesAttendIncubationWorkShops { get; set; }
        public virtual DbSet<FollowUpProjectPlan> FollowUpProjectPlans { get; set; }
        public virtual DbSet<FollowUpProjectPlanAttachment> FollowUpProjectPlanAttachments { get; set; }
        public virtual DbSet<FollowUpProjectPlanRequest> FollowUpProjectPlanRequests { get; set; }
        public virtual DbSet<FollowUpProjectPlanRequestAttachment> FollowUpProjectPlanRequestAttachments { get; set; }
        public virtual DbSet<IncubationBaseline> IncubationBaselines { get; set; }
        public virtual DbSet<IncubationBaselineStatus> IncubationBaselineStatus { get; set; }
        public virtual DbSet<IncubationWorkshopControls> IncubationWorkshopControls { get; set; }
        public virtual DbSet<IncubationWorkshopControlsType> IncubationWorkshopControlsType { get; set; }
        public virtual DbSet<IncubationWorkshopBLTransactionsType> IncubationWorkshopBLTransactionsType { get; set; }
        public virtual DbSet<IncubationWorkshopBLTransactions> IncubationWorkshopBLTransactions { get; set; }
        public virtual DbSet<IncubationWorkshopBLTransactionsValue> IncubationWorkshopBLTransactionsValue { get; set; }
        public virtual DbSet<IncubationWorkshopBLTransValStatus> IncubationWorkshopBLTransValStatus { get; set; }
        public virtual DbSet<IncubationWorkshopBLTransValueAttachment> IncubationWorkshopBLTransValueAttachment { get; set; }

        public static KingkhaledFoundationDB Create()
        {
            return new KingkhaledFoundationDB();
        }
    }
}