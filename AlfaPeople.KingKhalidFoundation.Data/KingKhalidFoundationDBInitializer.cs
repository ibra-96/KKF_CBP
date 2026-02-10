using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Data
{
    public class KingKhalidFoundationDBInitializer : CreateDatabaseIfNotExists<KingkhaledFoundationDB>
    {
        protected override void Seed(KingkhaledFoundationDB context)
        {
            #region List of Incubation Workshop Controls
            List<IncubationWorkshopControls> LstIWSControls = new List<IncubationWorkshopControls>  {
                new IncubationWorkshopControls() { ControlsID = Guid.NewGuid(), ControlsName = "TextBox" },
                new IncubationWorkshopControls() { ControlsID = Guid.NewGuid(), ControlsName = "Textarea" },
                new IncubationWorkshopControls() { ControlsID = Guid.NewGuid(), ControlsName = "Checkbox" },
                new IncubationWorkshopControls() { ControlsID = Guid.NewGuid(), ControlsName = "Attachments" }
            };
            #endregion
            context.IncubationWorkshopControls.AddRange(LstIWSControls);

            context.IncubationWorkshopControlsType.AddRange(new List<IncubationWorkshopControlsType> {
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "TextBox").ControlsID, ControlTypeName = "text" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "TextBox").ControlsID, ControlTypeName = "number" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "TextBox").ControlsID, ControlTypeName = "date" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "TextBox").ControlsID, ControlTypeName = "datetime-local" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "Textarea").ControlsID, ControlTypeName = "textarea" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "Checkbox").ControlsID, ControlTypeName = "checkbox" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "Attachments").ControlsID, ControlTypeName = "single" },
                new IncubationWorkshopControlsType() { ControlTypeID = Guid.NewGuid(), ControlsID = LstIWSControls.FirstOrDefault(i => i.ControlsName == "Attachments").ControlsID, ControlTypeName = "multi" }
            });

            context.IncubationWorkshopBLTransValStatus.AddRange(new List<IncubationWorkshopBLTransValStatus>
            {
                new IncubationWorkshopBLTransValStatus () {IncubationWorkshopBLTransValStatusID = Guid.NewGuid(), NameAR = "مقبول", NameEN = "Accepted" },
                new IncubationWorkshopBLTransValStatus () {IncubationWorkshopBLTransValStatusID = Guid.NewGuid(), NameAR = "مرفوض", NameEN = "Rejected" },
                new IncubationWorkshopBLTransValStatus () {IncubationWorkshopBLTransValStatusID = Guid.NewGuid(), NameAR = "معلق", NameEN = "Pending" },
                new IncubationWorkshopBLTransValStatus () {IncubationWorkshopBLTransValStatusID = Guid.NewGuid(), NameAR = "مسودة", NameEN = "Draft" },
                new IncubationWorkshopBLTransValStatus () {IncubationWorkshopBLTransValStatusID = Guid.NewGuid(), NameAR = "تحديث نموذج الطلب الأساسي", NameEN = "Update Baseline Application Form" },
                new IncubationWorkshopBLTransValStatus () {IncubationWorkshopBLTransValStatusID = Guid.NewGuid(), NameAR = "تم تحديث نموذج الطلب الأساسي", NameEN = "Baseline Application Form Updated" }
            });

            #region List of Positions
            List<BackendUserPosition> LstPost = new List<BackendUserPosition>  {
                new BackendUserPosition() { BackendUserPositionID = Guid.NewGuid(), IsActive = true, NameAR = "Administration", NameEN = "Administration" },
                new BackendUserPosition() { BackendUserPositionID = Guid.NewGuid(), IsActive = true, NameAR = "CB Manager", NameEN = "CB Manager" },
                new BackendUserPosition() { BackendUserPositionID = Guid.NewGuid(), IsActive = true, NameAR = "CB Supervisor", NameEN = "CB Supervisor" },
                new BackendUserPosition() { BackendUserPositionID = Guid.NewGuid(), IsActive = true, NameAR = "CB Analyst", NameEN = "CB Analyst" }
            };
            #endregion
            context.BackendUserPositions.AddRange(LstPost);

            context.WorkshopProjectProposalStatus.AddRange(new List<WorkshopProjectProposalStatu>
            {
                new WorkshopProjectProposalStatu () {WorkshopProjectProposalStatusID=Guid.NewGuid(),NameAR = "معلق",NameEN ="Pending" },
                new WorkshopProjectProposalStatu () {WorkshopProjectProposalStatusID=Guid.NewGuid(),NameAR = "مرفوض",NameEN ="Rejected" },
                new WorkshopProjectProposalStatu () {WorkshopProjectProposalStatusID=Guid.NewGuid(),NameAR = "غائب",NameEN ="Absent" },
                new WorkshopProjectProposalStatu () {WorkshopProjectProposalStatusID=Guid.NewGuid(),NameAR = "تحديث بيانات", NameEN = "Update Project Proposal" },
                new WorkshopProjectProposalStatu () {WorkshopProjectProposalStatusID=Guid.NewGuid(),NameAR = "تم تحديث البيانات", NameEN = "Project Proposal Updated" },
                new WorkshopProjectProposalStatu () {WorkshopProjectProposalStatusID=Guid.NewGuid(),NameAR = "مقبول",NameEN ="Accepted" }
            });

            context.IncubationBaselineStatus.AddRange(new List<IncubationBaselineStatus>
            {
                new IncubationBaselineStatus () {IncubationBaselineStatusID = Guid.NewGuid(), NameAR = "مقبول", NameEN = "Accepted" },
                new IncubationBaselineStatus () {IncubationBaselineStatusID = Guid.NewGuid(), NameAR = "مرفوض", NameEN = "Rejected" },
                new IncubationBaselineStatus () {IncubationBaselineStatusID = Guid.NewGuid(), NameAR = "معلق", NameEN = "Pending" },
                new IncubationBaselineStatus () {IncubationBaselineStatusID = Guid.NewGuid(), NameAR = "مسودة", NameEN = "Draft" },
                new IncubationBaselineStatus () {IncubationBaselineStatusID = Guid.NewGuid(), NameAR = "تحديث نموذج الطلب الأساسي", NameEN = "Update Baseline Application Form" },
                new IncubationBaselineStatus () {IncubationBaselineStatusID = Guid.NewGuid(), NameAR = "تم تحديث نموذج الطلب الأساسي", NameEN = "Baseline Application Form Updated" }
            });

            context.ReasonType.AddRange(new List<ReasonType>
            {
                new ReasonType() {Id=Guid.NewGuid(),Name="Missing Data" },
                new ReasonType() {Id=Guid.NewGuid(),Name="Bad Request" },
                new ReasonType() {Id=Guid.NewGuid(),Name="recheck" }
            });

            context.IncubationProjectProposalStatus.AddRange(new List<IncubationProjectProposalStatu>
            {
                new IncubationProjectProposalStatu () {IncubationProjectProposalStatusID=Guid.NewGuid(),NameAR = "معلق",NameEN ="Pending" },
                new IncubationProjectProposalStatu () {IncubationProjectProposalStatusID=Guid.NewGuid(),NameAR = "مرفوض",NameEN ="Rejected" },
                new IncubationProjectProposalStatu () {IncubationProjectProposalStatusID=Guid.NewGuid(),NameAR = "تحديث بيانات",NameEN ="Update Project Proposal" },
                new IncubationProjectProposalStatu () {IncubationProjectProposalStatusID=Guid.NewGuid(),NameAR = "تم تحديث البيانات",NameEN ="Project Proposal Updated" },
                new IncubationProjectProposalStatu () {IncubationProjectProposalStatusID=Guid.NewGuid(),NameAR = "مقبول",NameEN ="Accepted" },
                new IncubationProjectProposalStatu () {IncubationProjectProposalStatusID=Guid.NewGuid(),NameAR = "مسودة",NameEN ="Draft" }
            });

            #region List Of Asp Net Role
            List<AspNetRole> aspNetRoleList = new List<AspNetRole>() {
                new AspNetRole() { Id = "1", Name = "Admin" },
                new AspNetRole() { Id = "2", Name = "CB Manager" },
                new AspNetRole() { Id = "3", Name = "CB Supervisor" },
                new AspNetRole() { Id = "4", Name = "CB Analyst" },
                new AspNetRole() { Id = "8", Name = "Corporation" }
            };
            #endregion
            context.AspNetRoles.AddRange(aspNetRoleList);

            context.Programs.AddRange(new List<Program> {
                new Program() { ProgramID = Guid.NewGuid(), IsActive = true, ProgramName = "Capacity Building" , ProgramNameAR ="بناء القدرات" }
            });

            context.AuthorizationAuthorities.AddRange(new List<AuthorizationAuthority> {
                new AuthorizationAuthority() { AuthorizationAuthorityID = Guid.NewGuid(), IsActive = true, AuthorizationAuthorityNameAR = "وزارة الموارد البشرية والتنمية الاجتماعية", AuthorizationAuthorityNameEN = "Ministry of Human Resources and Social Development" },
                new AuthorizationAuthority() { AuthorizationAuthorityID = Guid.NewGuid(), IsActive = true, AuthorizationAuthorityNameAR = "وزارة التجارة والاستثمار", AuthorizationAuthorityNameEN = "Ministry of Commerce and Investment" }
            });

            context.ApplicantStatus.AddRange(new List<ApplicantStatu> {
                new ApplicantStatu() { ApplicantStatusID = Guid.NewGuid(), ApplicantStatusName = "Accepted" },
                new ApplicantStatu() { ApplicantStatusID = Guid.NewGuid(), ApplicantStatusName = "Rejected" },
                new ApplicantStatu() { ApplicantStatusID = Guid.NewGuid(), ApplicantStatusName = "Pending" }
            });

            #region List Of Incubation Types
            List<IncubationType> incubationTypeList = new List<IncubationType>(){
                new IncubationType () { IncubationTypeID = Guid.NewGuid(), NameAR = "الاحتضان الكلي", NameEN = "Incubation" },
                new IncubationType () { IncubationTypeID = Guid.NewGuid(), NameAR = "الاحتضان الجزئي", NameEN = "Acceleration" }
            };
            #endregion
            context.IncubationTypes.AddRange(incubationTypeList);

            context.IncubationModels.AddRange(new List<IncubationModel> {
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "حوكمة مجلس الإدارة", NameEN = "Board Governance" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "التخطيط الاستراتيجي", NameEN = "Strategic Planning" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "الموارد البشرية", NameEN = "Human Resurses" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "تنمية الموارد المالية", NameEN = "Fundraising" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "إدارة المشاريع", NameEN = "Programs/Projects Management" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "الاتصال المؤسسي", NameEN = "Corporate Communications" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Acceleration").IncubationTypeID, NameAR = "الإدارة المالية", NameEN = "Finance Management" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "التقييم المؤسسي", NameEN = "Needs Assessment" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "حوكمة مجلس الإدارة", NameEN = "Board Governance" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "التخطيط الاستراتيجي", NameEN = "Strategic Planning" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "الموارد البشرية", NameEN = "Human Resurses" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "تنمية الموارد المالية", NameEN = "Fundraising" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "إدارة المشاريع", NameEN = "Programs/Projects Management" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "الاتصال المؤسسي", NameEN = "Corporate Communications" },
                new IncubationModel() { IncubationModelID = Guid.NewGuid(), IsActive = true, IncubationTypeID = incubationTypeList.FirstOrDefault(i => i.NameEN == "Incubation").IncubationTypeID, NameAR = "الإدارة المالية", NameEN = "Finance Management" }
            });

            context.IncubationStatus.AddRange(new List<IncubationStatus> {
                new IncubationStatus() { IncubationStatusID = Guid.NewGuid(), NameAR = "فعال", NameEN = "Active" },
                new IncubationStatus() { IncubationStatusID = Guid.NewGuid(), NameAR = "غير فعال", NameEN = "InActive" }
            });

            context.IncubationWorkshopModels.AddRange(new List<IncubationWorkshopModel> {
                new IncubationWorkshopModel() { IncubationWorkshopModeID = Guid.NewGuid(), IsActive = true, NameAR = "تمكين المدراء التنفيذين", NameEN = "Leadership Training " },
                new IncubationWorkshopModel() { IncubationWorkshopModeID = Guid.NewGuid(), IsActive = true, NameAR = "تنمية الموارد المالية", NameEN = "Fundraising" },
                new IncubationWorkshopModel() { IncubationWorkshopModeID = Guid.NewGuid(), IsActive = true, NameAR = "الاتصال المؤسسي", NameEN = "Corporate Communication" },
                new IncubationWorkshopModel() { IncubationWorkshopModeID = Guid.NewGuid(), IsActive = true, NameAR = "المتابعة والتقييم للبرامج والمشاريع", NameEN = "Monitoring and Evaluation" }
            });

            context.IncubationWorkshopTypes.AddRange(new List<IncubationWorkshopType> {
                new IncubationWorkshopType() {  IncubationWorkshopTypeID = Guid.NewGuid(), NameAR = "الإرشاد", NameEN = "Mentorship" },
                new IncubationWorkshopType() {  IncubationWorkshopTypeID = Guid.NewGuid(), NameAR = "ورشة تعريفية", NameEN = "Induction Workshop" },
                new IncubationWorkshopType() {  IncubationWorkshopTypeID = Guid.NewGuid(), NameAR = "ريادة الاعمال", NameEN = "Social Entrepreneurship" },
                new IncubationWorkshopType() {  IncubationWorkshopTypeID = Guid.NewGuid(), NameAR = "ورشة تطوير النماذج", NameEN = "Model Development Workshop" }
            });

            context.IncubationtWorkshopStatus.AddRange(new List<IncubationtWorkshopStatu> {
                new IncubationtWorkshopStatu() { IncubationtWorkshopStatusID = Guid.NewGuid(), NameAR = "فعال", NameEN = "Active" },
                new IncubationtWorkshopStatu() { IncubationtWorkshopStatusID = Guid.NewGuid(), NameAR = "غير فعال", NameEN = "InActive" }
            });

            #region List of Corporations Category
            List<CorporationsCategory> CorpCategoryList = new List<CorporationsCategory>() {
                new CorporationsCategory(){ CorporationsCategoryID = Guid.NewGuid(), IsActive = true, CorporationsCategoryNameAR = "القطاع الحكومي", CorporationsCategoryNameEN = "Public Sector" },
                new CorporationsCategory(){ CorporationsCategoryID = Guid.NewGuid(), IsActive = true, CorporationsCategoryNameAR = "القطاع الخاص", CorporationsCategoryNameEN = "Private Sector" },
                new CorporationsCategory(){ CorporationsCategoryID = Guid.NewGuid(), IsActive = true, CorporationsCategoryNameAR = "القطاع غير ربحي", CorporationsCategoryNameEN = "Non Profit Sector" }
            };
            #endregion
            context.CorporationsCategories.AddRange(CorpCategoryList);

            context.ClassificationSectors.AddRange(new List<ClassificationSector> {
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Public Sector").CorporationsCategoryID, ClassificationSectorNameAR = "مجالس تنسيقية", ClassificationSectorNameEN = "Coordinating Councils" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Private Sector").CorporationsCategoryID, ClassificationSectorNameAR = "شركة مؤسسة ربحية", ClassificationSectorNameEN = "Profit Foundation/Company" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Private Sector").CorporationsCategoryID, ClassificationSectorNameAR = "شركة غير ربحية", ClassificationSectorNameEN = "Non-Profit Company" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Non Profit Sector").CorporationsCategoryID, ClassificationSectorNameAR = "مؤسسة خيرية", ClassificationSectorNameEN = "Charitable Institution" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Non Profit Sector").CorporationsCategoryID, ClassificationSectorNameAR = "مركز تنمية أسرية", ClassificationSectorNameEN = "Family Development Center" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Non Profit Sector").CorporationsCategoryID, ClassificationSectorNameAR = "لجنة تنمية اجتماعية", ClassificationSectorNameEN = "Social Development Committee" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Non Profit Sector").CorporationsCategoryID, ClassificationSectorNameAR = "جمعية خيرية", ClassificationSectorNameEN = "Charitable Society" },
                new ClassificationSector() { ClassificationSectorID = Guid.NewGuid(), IsActive = true, CorporationsCategoryID = CorpCategoryList.FirstOrDefault(c => c.CorporationsCategoryNameEN == "Non Profit Sector").CorporationsCategoryID, ClassificationSectorNameAR = "وقف خيري", ClassificationSectorNameEN = "Charitable Endowment" }
            });

            context.CorporateFieldOfWork.AddRange(new List<CorporateFieldOfWork> {
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "تعليمي", CorporateFieldOfWorkNameEN = "educational", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "ثقافي", CorporateFieldOfWorkNameEN = "cultural", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "صحي", CorporateFieldOfWorkNameEN = "healthy", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "بيئي", CorporateFieldOfWorkNameEN = "environmental", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "توعوي", CorporateFieldOfWorkNameEN = "Awareness", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "أسر منتجة", CorporateFieldOfWorkNameEN = "Productive families", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "تنمية أسرية", CorporateFieldOfWorkNameEN = "Family development", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "تربوي", CorporateFieldOfWorkNameEN = "educational", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "حقوق إنسان", CorporateFieldOfWorkNameEN = "human rights", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "تطوع", CorporateFieldOfWorkNameEN = "volunteer", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "هندسية", CorporateFieldOfWorkNameEN = "Geometric", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "رعاية سجناء", CorporateFieldOfWorkNameEN = "Take care of prisoners", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "تمكين المرأة", CorporateFieldOfWorkNameEN = "Women's Empowerment", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "تمكين الطفل", CorporateFieldOfWorkNameEN = "Child Empowerment", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "شبابي", CorporateFieldOfWorkNameEN = "My youth", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "ذوي الاحتياجات الخاصة", CorporateFieldOfWorkNameEN = "People with special needs", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "أيتام", CorporateFieldOfWorkNameEN = "Orphans", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "مهنية", CorporateFieldOfWorkNameEN = "Vocational", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "ريادة أعمال", CorporateFieldOfWorkNameEN = "Entrepreneurship", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "رعوي (جمعيات البر)", CorporateFieldOfWorkNameEN = "Pastoral", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "رعاية كبار السن", CorporateFieldOfWorkNameEN = "Care for the elderly", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "حفظ الطعام", CorporateFieldOfWorkNameEN = "Save food", IsActive = true },
                new CorporateFieldOfWork() { CorporateFieldOfWorkID = Guid.NewGuid(), CorporateFieldOfWorkNameAR = "حفظ التراث", CorporateFieldOfWorkNameEN = "Conservation of heritage", IsActive = true }
            });

            context.FundingSources.AddRange(new List<FundingSource> {
                new FundingSource() { FundingSourceID = Guid.NewGuid(), FundingSourceNameAR = "صندوق الأميرة صيتة", FundingSourceNameEN = "Princess Seeta Fund", Nickname = "Seeta", UseCustomThemes = false, HideKKFLogo = false, IsActive = true },
                new FundingSource() { FundingSourceID = Guid.NewGuid(), FundingSourceNameAR = "صندوق الأميرة مشاعل بنت خالد", FundingSourceNameEN = "Princess Mashael bint Khalid Fund", Nickname = "Mashael", UseCustomThemes = false, HideKKFLogo = false,  IsActive = true },
                new FundingSource() { FundingSourceID = Guid.NewGuid(), FundingSourceNameAR = "صندوق الأميرة موضي بنت خالد", FundingSourceNameEN = "Princess Moudhi bint Khalid Fund", Nickname = "Moudhi", UseCustomThemes = false, HideKKFLogo = false,  IsActive = true },
                new FundingSource() { FundingSourceID = Guid.NewGuid(), FundingSourceNameAR = "مؤسسة الملك خالد", FundingSourceNameEN = "King Khalid Foundation (KKF)", Nickname = "KKF", UseCustomThemes = false, HideKKFLogo = false,  IsActive = true }
            });

            context.TypeOfInterventions.AddRange(new List<TypeOfIntervention> {
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "volunteer",TypeOfInterventionNameAR = "تطوع", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Training",TypeOfInterventionNameAR = "تدريب", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Awareness",TypeOfInterventionNameAR = "توعية", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Enable",TypeOfInterventionNameAR = "تمكين", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Qualifying",TypeOfInterventionNameAR = "تأهيل", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Relief",TypeOfInterventionNameAR = "إغاثة", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Capacity building",TypeOfInterventionNameAR = "بناء قدرات", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Institutional building",TypeOfInterventionNameAR = "بناء مؤسسي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Educate",TypeOfInterventionNameAR = "تثقيف", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Strengthen",TypeOfInterventionNameAR = "تعزيز", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Development",TypeOfInterventionNameAR = "تطوير", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "rationing",TypeOfInterventionNameAR = "تقنين", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Activation",TypeOfInterventionNameAR = "تفعيل", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Recruit",TypeOfInterventionNameAR = "توظيف", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Policy",TypeOfInterventionNameAR = "سياسات ", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "A comparative study",TypeOfInterventionNameAR = "دراسة مقارنة", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "educational",TypeOfInterventionNameAR = "تعليمي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "cultural",TypeOfInterventionNameAR = "ثقافي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "healthy",TypeOfInterventionNameAR = "صحي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "environmental",TypeOfInterventionNameAR = "بيئي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Awareness",TypeOfInterventionNameAR = "توعوي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Productive families",TypeOfInterventionNameAR = "أسر منتجة", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Family development",TypeOfInterventionNameAR = "تنمية أسرية", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "educational",TypeOfInterventionNameAR = "تربوي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "human rights",TypeOfInterventionNameAR = "حقوق إنسان", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "volunteer",TypeOfInterventionNameAR = "تطوع", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Geometric",TypeOfInterventionNameAR = "هندسية", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Take care of prisoners",TypeOfInterventionNameAR = "رعاية سجناء", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Women's Empowerment",TypeOfInterventionNameAR = "تمكين المرأة", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Child Empowerment",TypeOfInterventionNameAR = "تمكين الطفل", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Youth",TypeOfInterventionNameAR = "شبابي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Obstruction",TypeOfInterventionNameAR = "إعاقة", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Orphans",TypeOfInterventionNameAR = "أيتام", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Vocational",TypeOfInterventionNameAR = "مهنية", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "rationing",TypeOfInterventionNameAR = "تقنين", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Training",TypeOfInterventionNameAR = "تدريب", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Activation",TypeOfInterventionNameAR = "تفعيل", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Enable",TypeOfInterventionNameAR = "تمكين", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Qualifying",TypeOfInterventionNameAR = "تأهيل", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Relief",TypeOfInterventionNameAR = "إغاثة", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Capacity building",TypeOfInterventionNameAR = "بناء قدرات", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Institutional development",TypeOfInterventionNameAR = "تطوير مؤسسي", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Educate",TypeOfInterventionNameAR = "تثقيف", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Strengthen",TypeOfInterventionNameAR = "تعزيز", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Development",TypeOfInterventionNameAR = "تطوير", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "building abilities",TypeOfInterventionNameAR = "بناء القدرات", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Create a job",TypeOfInterventionNameAR = "خلق فرصة عمل", IsActive = true },
                new TypeOfIntervention() { TypeOfInterventionID = Guid.NewGuid(), TypeOfInterventionNameEN = "Increase income",TypeOfInterventionNameAR = "زيادة دخل", IsActive = true }
            });

            #region List Of Regions
            List<Region> regList = new List<Region> {
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "الرياض", RegionNameEN = "Riyadh" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "مكة المكرمة", RegionNameEN = "Makkah" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "المدينة المنورة", RegionNameEN = "Al Madinah" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "القصيم", RegionNameEN = "Qassim" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "الشرقية", RegionNameEN = "Eastern Province" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "عسير", RegionNameEN = "Asir" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "تبوك", RegionNameEN = "Tabuk" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "حائل", RegionNameEN = "Hail" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "الحدود الشمالية", RegionNameEN = "Northern Borders" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "جازان", RegionNameEN = "Jizan" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "نجران", RegionNameEN = "Najran" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "الباحة", RegionNameEN = "Bahah" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "الجوف", RegionNameEN = "Jawf" },
                new Region() { RegionID = Guid.NewGuid(), IsActive = true, RegionNameAR = "KSA", RegionNameEN = "KSA" }
        };
            #endregion
            context.Regions.AddRange(regList);

            #region List Of Governorates
            List<Governorate> govList = new List<Governorate> {
                #region Riyadh
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الرياض", GovernorateEN = "Riyadh" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الدرعية", GovernorateEN = "Al Diriyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الخرج", GovernorateEN = "Al Kharj" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الدوادمي", GovernorateEN = "Al Duwadimi" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "المجمعة", GovernorateEN = "Al Majmaah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "القويعية", GovernorateEN = "Al Quway'iyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "وادي الدواسر", GovernorateEN = "Wadi Al Dawasir" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الأفلاج", GovernorateEN = "Al Aflaj" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الزلفي", GovernorateEN = "Al Zulfi" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "شقراء", GovernorateEN = "Shaqra" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "حوطة بني تميم", GovernorateEN = "Howtat Bani Tamim" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "عفيف", GovernorateEN = "Afif" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "السليل", GovernorateEN = "Al Sulayyil" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "ضرما", GovernorateEN = "Dhurma" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "المزاحمية", GovernorateEN = "Al Muzahimiyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "رماح", GovernorateEN = "Rumah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "ثادق", GovernorateEN = "Thadiq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "حريملاء", GovernorateEN = "Huraymila" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الحريق", GovernorateEN = "Al Hariq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الغاط", GovernorateEN = "Al Ghat" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "مرات", GovernorateEN = "Marrat" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Riyadh").RegionID, GovernorateAR = "الرين", GovernorateEN = "Al riyn" },
                #endregion
                #region Makkah
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "مكة المكرمة", GovernorateEN = "Makkah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "جدة", GovernorateEN = "Jeddah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "الطائف", GovernorateEN = "Al Taif" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "القنفذة", GovernorateEN = "Al Qunfudhah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "الليث", GovernorateEN = "Al Lith" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "رابغ", GovernorateEN = "Rabigh" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "خليص", GovernorateEN = "Khulays" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "الخرمة", GovernorateEN = "Al Khurmah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "رنية", GovernorateEN = "Ranyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "تربة", GovernorateEN = "Turubah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "الجموم", GovernorateEN = "Al Jumum" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "الكامل", GovernorateEN = "Al Kamil" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "المويه", GovernorateEN = "Al Mawiyuh" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "ميسان", GovernorateEN = "Maysan" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "أضم", GovernorateEN = "Aduma" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "العرضيات", GovernorateEN = "Al Erdiat" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Makkah").RegionID, GovernorateAR = "بحرة", GovernorateEN = "Bahra" },
                #endregion
                #region Al Madinah
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "المدينة المنورة", GovernorateEN = "Al Madinah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "ينبع", GovernorateEN = "Yanbu" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "العلا", GovernorateEN = "Al Ula" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "مهد الذهب", GovernorateEN = "Mahd adh Dhahab" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "الحناكية", GovernorateEN = "Al Henakiyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "بدر", GovernorateEN = "Badr" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "خيبر", GovernorateEN = "Khaybar" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "العيص", GovernorateEN = "Al Eays" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Al Madinah").RegionID, GovernorateAR = "وادي الفرع", GovernorateEN = "Wadi Alfare" },
                #endregion
                #region Qassim
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "بريدة", GovernorateEN = "Buraydah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "عنيزة", GovernorateEN = "Unaizah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "الرس", GovernorateEN = "Al Rass" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "المذنب", GovernorateEN = "Al Mithnab" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "البكيرية", GovernorateEN = "Al Bukayriyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "البدائع", GovernorateEN = "Al Badayea" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "الأسياح", GovernorateEN = "Al Asyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "النبهانية", GovernorateEN = "Al Nabhaniyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "الشماسية", GovernorateEN = "Al Shimasiyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "عيون الجواء", GovernorateEN = "Uyun Al Jiwa" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "رياض الخبراء", GovernorateEN = "Riyadh Al Khabra" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "عقلة الصقور", GovernorateEN = "Euqlat Alsuqur" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Qassim").RegionID, GovernorateAR = "ضرية", GovernorateEN = "Duriya" },
                #endregion
                #region Eastern Province
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "الدمام", GovernorateEN = "Al Dammam" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "الأحساء", GovernorateEN = "Al Ahsa" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "حفر الباطن", GovernorateEN = "Hafar Al Batin" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "الجبيل", GovernorateEN = "Jubail" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "القطيف", GovernorateEN = "Al Qatif" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "الخبر", GovernorateEN = "Al Khobar" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "الخفجي", GovernorateEN = "Al Khafji" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "رأس تنورة", GovernorateEN = "Ras Tanurah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "بقيق", GovernorateEN = "Buqayq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "النعيرية", GovernorateEN = "Al Nairyah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "قرية العليا", GovernorateEN = "Qaryat Al-Ulya" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Eastern Province").RegionID, GovernorateAR = "العديد", GovernorateEN = "Aledyd" },
                #endregion
                #region Asir
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "أبها", GovernorateEN = "Abha" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "خميس مشيط", GovernorateEN = "Khamis Mushait" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "بيشة", GovernorateEN = "Bisha" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "النماص", GovernorateEN = "Al Namas" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "محايل عسير", GovernorateEN = "Muhayil Asir" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "ظهران الجنوب", GovernorateEN = "Dhahran Al Janub" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "تثليث", GovernorateEN = "Tathlith" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "سراة عبيدة", GovernorateEN = "Sarat Abidah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "رجال ألمع", GovernorateEN = "Rojal" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "بلقرن", GovernorateEN = "Balqarn" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "أحد رفيدة", GovernorateEN = "Ahad Rafidah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "المجاردة", GovernorateEN = "Al Majaridah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "البرك", GovernorateEN = "Samira" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "بارق", GovernorateEN = "Bariq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "تنومة", GovernorateEN = "Tanawma" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "طريب", GovernorateEN = "Trib" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Asir").RegionID, GovernorateAR = "الحرجة", GovernorateEN = "Alharija" },
                #endregion
                #region Tabuk
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "تبوك", GovernorateEN = "Tabuk" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "الوجه", GovernorateEN = "Al Wajh" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "ضبا", GovernorateEN = "Duba" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "تيماء", GovernorateEN = "Tayma" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "أملج", GovernorateEN = "Umluj" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "حقل", GovernorateEN = "Haql" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Tabuk").RegionID, GovernorateAR = "البدع", GovernorateEN = "Al Badae" },
                #endregion
                #region Hail
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "حائل", GovernorateEN = "Hail" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "بقعاء", GovernorateEN = "Baqaa" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "الغزالة", GovernorateEN = "Al Khazaiah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "الشنان", GovernorateEN = "Al Shinan" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "الحائط", GovernorateEN = "Alhayit" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "السليمي", GovernorateEN = "Alsalimi" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "الشملي", GovernorateEN = "Alshamli" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "موقق", GovernorateEN = "Mawqiq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Hail").RegionID, GovernorateAR = "سميراء", GovernorateEN = "Samira" },
                #endregion
                #region Northern Borders
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Northern Borders").RegionID, GovernorateAR = "عرعر", GovernorateEN = "Arar" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Northern Borders").RegionID, GovernorateAR = "رفحاء", GovernorateEN = "Rafha" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Northern Borders").RegionID, GovernorateAR = "طريف", GovernorateEN = "Turaif" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Northern Borders").RegionID, GovernorateAR = "العويقيلة", GovernorateEN = "Al Ewyqila" },
                #endregion
                #region Jizan
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "جازان", GovernorateEN = "Jizan" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "صبيا", GovernorateEN = "Sabya" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "أبو عريش", GovernorateEN = "Abu Arish" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "صامطة", GovernorateEN = "Samtah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "بيش", GovernorateEN = "Baish" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "الدرب", GovernorateEN = "Al Darb" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "الحرث", GovernorateEN = "Al Harth" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "ضمد", GovernorateEN = "Damad" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "الريث", GovernorateEN = "Al Reeth" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "جزر فرسان", GovernorateEN = "Juzur Farasan" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "الدائر", GovernorateEN = "Al Dayer" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "العارضة", GovernorateEN = "Al Aridhah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "أحد المسارحة", GovernorateEN = "Ahad Al Masarihah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "العيدابي", GovernorateEN = "Al Edabi" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "فيفاء", GovernorateEN = "Fayfa" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "الطوال", GovernorateEN = "Al Twal" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jizan").RegionID, GovernorateAR = "هروب", GovernorateEN = "Harub" },
                #endregion
                #region Najran
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "نجران", GovernorateEN = "Najran" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "شرورة", GovernorateEN = "Sharurah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "حبونا", GovernorateEN = "Hubuna" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "بدر الجنوب", GovernorateEN = "Badr Al Janub" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "يدمه", GovernorateEN = "Yadamah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "ثار", GovernorateEN = "Thar" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "خباش", GovernorateEN = "Khubash" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Najran").RegionID, GovernorateAR = "الخرخير", GovernorateEN = "Al Kharkhir" },
                #endregion
                #region Bahah
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "الباحة", GovernorateEN = "Al Bahah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "بلجرشي", GovernorateEN = "Baljurashi" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "المندق", GovernorateEN = "Al Mandaq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "المخواة", GovernorateEN = "Al Makhwah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "قلوة", GovernorateEN = "Qilwah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "العقيق", GovernorateEN = "Al Aqiq" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "القرى", GovernorateEN = "Al Qara" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "غامد الزناد", GovernorateEN = "Ghamid Alzinad" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "الحجرة", GovernorateEN = "Alhajra" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Bahah").RegionID, GovernorateAR = "بني حسن", GovernorateEN = "Bani Hasan" },
                #endregion
                #region Jawf
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jawf").RegionID, GovernorateAR = "سكاكا", GovernorateEN = "Sakakah" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jawf").RegionID, GovernorateAR = "القريات", GovernorateEN = "Qurayyat" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jawf").RegionID, GovernorateAR = "دومة الجندل	", GovernorateEN = "Dumat Al-Jandal" },
                new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "Jawf").RegionID, GovernorateAR = "طبرجل", GovernorateEN = "Tabirajl" },
                #endregion
                #region KSA
		        new Governorate() { GovernorateID = Guid.NewGuid(), IsActive = true, RegionID = regList.FirstOrDefault(r=> r.RegionNameEN == "KSA").RegionID, GovernorateAR = "Online", GovernorateEN = "Online" }
                #endregion
                };
            #endregion
            context.Governorates.AddRange(govList);

            context.Cities.AddRange(new List<City>{       
                #region Al Ahsa
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Ahsa").GovernorateID, CityNameAR = "المجابل", CityNameEN = "Al Majabil" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Ahsa").GovernorateID, CityNameAR = "النعاثل", CityNameEN = "Al Naeathul" },
                #endregion
                #region Yanbu
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Yanbu").GovernorateID, CityNameAR = "الشروق", CityNameEN = "Al Shuruq" },
                #endregion
                #region Jeddah
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "الحرازات", CityNameEN = "Al Harazat" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "جدة التاريخية", CityNameEN = "Jidat Alttarikhia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "الصفا", CityNameEN = "Alsafa" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "العزيزية", CityNameEN = "Aleazizi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "النسيم", CityNameEN = "Alnasim" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "بني مالك", CityNameEN = "Bani Malik" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Jeddah").GovernorateID, CityNameAR = "قويزة", CityNameEN = "Quiza" },
                #endregion
                #region Al Khobar
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Khobar").GovernorateID, CityNameAR = "الثقبة", CityNameEN = "Althaqaba" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Khobar").GovernorateID, CityNameAR = "الخبر الشمالية", CityNameEN = "Alkhabar Alshamalia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Khobar").GovernorateID, CityNameAR = "حارات الخبر", CityNameEN = "Harat Alkhabar" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Khobar").GovernorateID, CityNameAR = "الجسر", CityNameEN = "Aljisr" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Khobar").GovernorateID, CityNameAR = "العقربية", CityNameEN = "Aleaqrabia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Khobar").GovernorateID, CityNameAR = "العنزي", CityNameEN = "Aleanzi" },
                #endregion
                #region Al Dammam
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "الحمراء", CityNameEN = "Alhamra" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "الشاطئ", CityNameEN = "Alshshati" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "الشاطئ الشرقي", CityNameEN = "Alshshati Alsharqiu" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "العنزي", CityNameEN = "Al-Anzi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "بدر", CityNameEN = "Badr" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "عبد الله فؤاد", CityNameEN = "Abdullah Fouad" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "الشاطئ الغربي", CityNameEN = "Alshshati Algharbiu" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "العدامة", CityNameEN = "Aleaddama" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "فريج العنود وبورشيد", CityNameEN = "Farij al-Anoud and Burshid" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Dammam").GovernorateID, CityNameAR = "واجهة الدمام البحرية", CityNameEN = "Wajihat Al-Dammam Al-Bahria" },
                #endregion
                #region Riyadh
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "السفارات", CityNameEN = "Alsiffarat" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "العريجاء الغربي", CityNameEN = "Aleryja' Algharbiu" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "العزيزية", CityNameEN = "Aleazizi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "أم سليم", CityNameEN = "Aum Salim" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الازدهار", CityNameEN = "Alaizdihar" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "البديعة", CityNameEN = "Albadiea" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "التعاون", CityNameEN = "Altaeawun" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الجزيرة", CityNameEN = "Aljazira" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الخزامى", CityNameEN = "Alkhazamaa" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "السليمانية", CityNameEN = "Alsulaymania" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "السويدي", CityNameEN = "Alsuwidi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الشعلان", CityNameEN = "Alshaelan" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الصحافة", CityNameEN = "Alsahafa" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الطريف", CityNameEN = "Altarif" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "العليا", CityNameEN = "Aleulya" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الفلاح", CityNameEN = "Alfalah" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المرسلات", CityNameEN = "Almursalat" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المروج", CityNameEN = "Almuruj" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المصانع", CityNameEN = "Almasanie" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المصيف", CityNameEN = "Almasif" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المعيزيلة", CityNameEN = "Almueayzila" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المغرزات", CityNameEN = "Almughrizat" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الملز", CityNameEN = "Almlzu" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الملك فهد", CityNameEN = "Almalik Fahd" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الملك فيصل", CityNameEN = "Almulk Faysal" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "النزهة", CityNameEN = "Alnuzha" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "النسيم الشرقي", CityNameEN = "Alnasim Alsharqiu" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "النظيم", CityNameEN = "Alnazim" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الواحة", CityNameEN = "Alwaha" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الوادي", CityNameEN = "Alwadi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الورود", CityNameEN = "Alwurud" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "شبرا", CityNameEN = "Shubaraan" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "صلاح الدين", CityNameEN = "Salah Aldiyn" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "طويق", CityNameEN = "Tawiq" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "ظهرة البديعة", CityNameEN = "Zahrat Albdye" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "غرناطة", CityNameEN = "Gharnata" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "منفوحة", CityNameEN = "Manfuha" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "الريان", CityNameEN = "Alrayan" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "المعذر الشمالي", CityNameEN = "Northern Maathar" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Riyadh").GovernorateID, CityNameAR = "ام الحمام", CityNameEN = "Umm Al-Hamam" },
                #endregion
                #region Dhahran Al Janub
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Dhahran Al Janub").GovernorateID, CityNameAR = "الرحيب", CityNameEN = "Alrahib" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Dhahran Al Janub").GovernorateID, CityNameAR = "العطف", CityNameEN = "Aleutf" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Dhahran Al Janub").GovernorateID, CityNameAR = "النسيم ظهران الجنوب", CityNameEN = "Alnasim Zahran Aljanub" },
                #endregion
                #region Al Madinah
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "أبواب المدينة المنورة", CityNameEN = "Abwab Al-madinat Al-munawara" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "أحواش المدينة المنورة", CityNameEN = "Ahwash Al-madinat Al-munawara" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "أحياء المدينة المنورة", CityNameEN = "Ahya Al-madinat Al-munawara" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الأغوات", CityNameEN = "Al Aghawat" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الجرف", CityNameEN = "Aljirf" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الدويمة", CityNameEN = "Aldawima" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "العزيزية", CityNameEN = "Aleazizi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الحرة الغربية", CityNameEN = "Alhurat Algharbia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "المنطقة المركزية", CityNameEN = "Almintaqat Almarkazia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "باب الشامي", CityNameEN = "Bab Alshamy" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "باب المجيدي", CityNameEN = "Bab Almujidi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "التاجوري", CityNameEN = "Altajuri" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الحرة الشرقية", CityNameEN = "Alhurat Alsharqia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "حمراء الأسد", CityNameEN = "Hamra' Alasad" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "البحر", CityNameEN = "Albahr" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الجبور", CityNameEN = "Aljubur" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "المستراح", CityNameEN = "Almustarah" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "زقاق الطيار", CityNameEN = "Auqaq Altayar" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "الساحة", CityNameEN = "Alssaha" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "سيد الشهداء", CityNameEN = "Sayd Alshuhada'" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "العنابس", CityNameEN = "Aleanabis" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "العنبرية", CityNameEN = "Aleanbaria" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "العوالي", CityNameEN = "Aleawali" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "العيون", CityNameEN = "Aleuyun" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "قباء", CityNameEN = "Qaba" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "مديكيك", CityNameEN = "Mudikik" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Al Madinah").GovernorateID, CityNameAR = "المناخة", CityNameEN = "Almanakha" },
                #endregion
                #region Makkah
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "أجياد", CityNameEN = "Ajyad" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "أحياء مكة المكرمة", CityNameEN = "Ahya Makat Al-Mukarama" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "الزاهر", CityNameEN = "alzzahir" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "الشامية", CityNameEN = "Alshaamia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "الشبيكة", CityNameEN = "Alshabika" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "العزيزية", CityNameEN = "Aleazizi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "القشاشية", CityNameEN = "Alqushashia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "الكعكية", CityNameEN = "Alkiekia" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "المسفلة", CityNameEN = "Almusfala" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "المعابدة", CityNameEN = "Almueabada" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "النزهة", CityNameEN = "Alnuzha" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "بطحاء قريش", CityNameEN = "Bataha' Quraysh" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "جرول", CityNameEN = "Groll" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "حارة الباب", CityNameEN = "Albab" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "حارة الملاوي", CityNameEN = "Almlawi" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "الشرائع", CityNameEN = "Alsharayie" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "الهجرة", CityNameEN = "Alhijra" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "كدي", CityNameEN = "Kudiy" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "ريع بخش", CityNameEN = "Raye Bikhsh" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "شعب عامر", CityNameEN = "Shaeb Eamir" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "معلاة", CityNameEN = "Miela" },
                new City() { CityID = Guid.NewGuid(), IsActive = true, GovernorateID = govList.FirstOrDefault(g => g.GovernorateEN == "Makkah").GovernorateID, CityNameAR = "منى", CityNameEN = "Mona" }
                
                #endregion

            });

            context.CountriesAndNationalities.AddRange(new List<CountriesAndNationality> {
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AF", NameAR = "أفغانستان", NameEN = "Afghanistan", NationalityAR = "أفغانستاني", NationalityEN = "Afghan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AX", NameAR = "جزر آلاند", NameEN = "Aland Islands", NationalityAR = "آلاندي", NationalityEN = "Aland Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AL", NameAR = "ألبانيا", NameEN = "Albania", NationalityAR = "ألباني", NationalityEN = "Albanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "DZ", NameAR = "الجزائر", NameEN = "Algeria", NationalityAR = "جزائري", NationalityEN = "Algerian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AS", NameAR = "ساموا-الأمريكي", NameEN = "American Samoa", NationalityAR = "أمريكي سامواني", NationalityEN = "American Samoan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AD", NameAR = "أندورا", NameEN = "Andorra", NationalityAR = "أندوري", NationalityEN = "Andorran" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AO", NameAR = "أنغولا", NameEN = "Angola", NationalityAR = "أنقولي", NationalityEN = "Angolan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AI", NameAR = "أنغويلا", NameEN = "Anguilla", NationalityAR = "أنغويلي", NationalityEN = "Anguillan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AQ", NameAR = "أنتاركتيكا", NameEN = "Antarctica", NationalityAR = "أنتاركتيكي", NationalityEN = "Antarctican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AG", NameAR = "أنتيغوا وبربودا", NameEN = "Antigua and Barbuda", NationalityAR = "بربودي", NationalityEN = "Antiguan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AR", NameAR = "الأرجنتين", NameEN = "Argentina", NationalityAR = "أرجنتيني", NationalityEN = "Argentinian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AM", NameAR = "أرمينيا", NameEN = "Armenia", NationalityAR = "أرميني", NationalityEN = "Armenian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AW", NameAR = "أروبه", NameEN = "Aruba", NationalityAR = "أوروبهيني", NationalityEN = "Aruban" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AU", NameAR = "أستراليا", NameEN = "Australia", NationalityAR = "أسترالي", NationalityEN = "Australian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AT", NameAR = "النمسا", NameEN = "Austria", NationalityAR = "نمساوي", NationalityEN = "Austrian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AZ", NameAR = "أذربيجان", NameEN = "Azerbaijan", NationalityAR = "أذربيجاني", NationalityEN = "Azerbaijani" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BS", NameAR = "الباهاماس", NameEN = "Bahamas", NationalityAR = "باهاميسي", NationalityEN = "Bahamian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BH", NameAR = "البحرين", NameEN = "Bahrain", NationalityAR = "بحريني", NationalityEN = "Bahraini" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BD", NameAR = "بنغلاديش", NameEN = "Bangladesh", NationalityAR = "بنغلاديشي", NationalityEN = "Bangladeshi" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BB", NameAR = "بربادوس", NameEN = "Barbados", NationalityAR = "بربادوسي", NationalityEN = "Barbadian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BY", NameAR = "روسيا البيضاء", NameEN = "Belarus", NationalityAR = "روسي", NationalityEN = "Belarusian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BE", NameAR = "بلجيكا", NameEN = "Belgium", NationalityAR = "بلجيكي", NationalityEN = "Belgian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BZ", NameAR = "بيليز", NameEN = "Belize", NationalityAR = "بيليزي", NationalityEN = "Belizean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BJ", NameAR = "بنين", NameEN = "Benin", NationalityAR = "بنيني", NationalityEN = "Beninese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BM", NameAR = "جزر برمودا", NameEN = "Bermuda", NationalityAR = "برمودي", NationalityEN = "Bermudan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BT", NameAR = "بوتان", NameEN = "Bhutan", NationalityAR = "بوتاني", NationalityEN = "Bhutanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BO", NameAR = "بوليفيا", NameEN = "Bolivia", NationalityAR = "بوليفي", NationalityEN = "Bolivian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BA", NameAR = "البوسنة و الهرسك", NameEN = "Bosnia and Herzegovina", NationalityAR = "بوسني/هرسكي", NationalityEN = "Bosnian / Herzegovinian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BW", NameAR = "بوتسوانا", NameEN = "Botswana", NationalityAR = "بوتسواني", NationalityEN = "Botswanan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BV", NameAR = "جزيرة بوفيه", NameEN = "Bouvet Island", NationalityAR = "بوفيهي", NationalityEN = "Bouvetian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BR", NameAR = "البرازيل", NameEN = "Brazil", NationalityAR = "برازيلي", NationalityEN = "Brazilian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IO", NameAR = "إقليم المحيط الهندي البريطاني", NameEN = "British Indian Ocean Territory", NationalityAR = "إقليم المحيط الهندي البريطاني", NationalityEN = "British Indian Ocean Territory" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BN", NameAR = "بروني", NameEN = "Brunei Darussalam", NationalityAR = "بروني", NationalityEN = "Bruneian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BG", NameAR = "بلغاريا", NameEN = "Bulgaria", NationalityAR = "بلغاري", NationalityEN = "Bulgarian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BF", NameAR = "بوركينا فاسو", NameEN = "Burkina Faso", NationalityAR = "بوركيني", NationalityEN = "Burkinabe" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BI", NameAR = "بوروندي", NameEN = "Burundi", NationalityAR = "بورونيدي", NationalityEN = "Burundian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KH", NameAR = "كمبوديا", NameEN = "Cambodia", NationalityAR = "كمبودي", NationalityEN = "Cambodian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CM", NameAR = "كاميرون", NameEN = "Cameroon", NationalityAR = "كاميروني", NationalityEN = "Cameroonian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CA", NameAR = "كندا", NameEN = "Canada", NationalityAR = "كندي", NationalityEN = "Canadian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CV", NameAR = "الرأس الأخضر", NameEN = "Cape Verde", NationalityAR = "الرأس الأخضر", NationalityEN = "Cape Verdean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KY", NameAR = "جزر كايمان", NameEN = "Cayman Islands", NationalityAR = "كايماني", NationalityEN = "Caymanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CF", NameAR = "جمهورية أفريقيا الوسطى", NameEN = "Central African Republic", NationalityAR = "أفريقي", NationalityEN = "Central African" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TD", NameAR = "تشاد", NameEN = "Chad", NationalityAR = "تشادي", NationalityEN = "Chadian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CL", NameAR = "شيلي", NameEN = "Chile", NationalityAR = "شيلي", NationalityEN = "Chilean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CN", NameAR = "الصين", NameEN = "China", NationalityAR = "صيني", NationalityEN = "Chinese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CX", NameAR = "جزيرة عيد الميلاد", NameEN = "Christmas Island", NationalityAR = "جزيرة عيد الميلاد", NationalityEN = "Christmas Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CC", NameAR = "جزر كوكوس", NameEN = "Cocos (Keeling) Islands", NationalityAR = "جزر كوكوس", NationalityEN = "Cocos Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CO", NameAR = "كولومبيا", NameEN = "Colombia", NationalityAR = "كولومبي", NationalityEN = "Colombian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KM", NameAR = "جزر القمر", NameEN = "Comoros", NationalityAR = "جزر القمر", NationalityEN = "Comorian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CG", NameAR = "الكونغو", NameEN = "Congo", NationalityAR = "كونغي", NationalityEN = "Congolese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CK", NameAR = "جزر كوك", NameEN = "Cook Islands", NationalityAR = "جزر كوك", NationalityEN = "Cook Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CR", NameAR = "كوستاريكا", NameEN = "Costa Rica", NationalityAR = "كوستاريكي", NationalityEN = "Costa Rican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "HR", NameAR = "كرواتيا", NameEN = "Croatia", NationalityAR = "كوراتي", NationalityEN = "Croatian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CU", NameAR = "كوبا", NameEN = "Cuba", NationalityAR = "كوبي", NationalityEN = "Cuban" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CW", NameAR = "كوراساو", NameEN = "Curaçao", NationalityAR = "كوراساوي", NationalityEN = "Curacian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CY", NameAR = "قبرص", NameEN = "Cyprus", NationalityAR = "قبرصي", NationalityEN = "Cypriot" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CZ", NameAR = "الجمهورية التشيكية", NameEN = "Czech Republic", NationalityAR = "تشيكي", NationalityEN = "Czech" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "DK", NameAR = "الدانمارك", NameEN = "Denmark", NationalityAR = "دنماركي", NationalityEN = "Danish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "DJ", NameAR = "جيبوتي", NameEN = "Djibouti", NationalityAR = "جيبوتي", NationalityEN = "Djiboutian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "DM", NameAR = "دومينيكا", NameEN = "Dominica", NationalityAR = "دومينيكي", NationalityEN = "Dominican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "DO", NameAR = "الجمهورية الدومينيكية", NameEN = "Dominican Republic", NationalityAR = "دومينيكي", NationalityEN = "Dominican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "EC", NameAR = "إكوادور", NameEN = "Ecuador", NationalityAR = "إكوادوري", NationalityEN = "Ecuadorian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "EG", NameAR = "مصر", NameEN = "Egypt", NationalityAR = "مصري", NationalityEN = "Egyptian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SV", NameAR = "إلسلفادور", NameEN = "El Salvador", NationalityAR = "سلفادوري", NationalityEN = "Salvadoran" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GQ", NameAR = "غينيا الاستوائي", NameEN = "Equatorial Guinea", NationalityAR = "غيني", NationalityEN = "Equatorial Guinean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ER", NameAR = "إريتريا", NameEN = "Eritrea", NationalityAR = "إريتيري", NationalityEN = "Eritrean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "EE", NameAR = "استونيا", NameEN = "Estonia", NationalityAR = "استوني", NationalityEN = "Estonian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ET", NameAR = "أثيوبيا", NameEN = "Ethiopia", NationalityAR = "أثيوبي", NationalityEN = "Ethiopian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "FK", NameAR = "جزر فوكلاند", NameEN = "Falkland Islands (Malvinas)", NationalityAR = "فوكلاندي", NationalityEN = "Falkland Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "FO", NameAR = "جزر فارو", NameEN = "Faroe Islands", NationalityAR = "جزر فارو", NationalityEN = "Faroese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "FJ", NameAR = "فيجي", NameEN = "Fiji", NationalityAR = "فيجي", NationalityEN = "Fijian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "FI", NameAR = "فنلندا", NameEN = "Finland", NationalityAR = "فنلندي", NationalityEN = "Finnish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "FR", NameAR = "فرنسا", NameEN = "France", NationalityAR = "فرنسي", NationalityEN = "French" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GF", NameAR = "غويانا الفرنسية", NameEN = "French Guiana", NationalityAR = "غويانا الفرنسية", NationalityEN = "French Guianese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PF", NameAR = "بولينيزيا الفرنسية", NameEN = "French Polynesia", NationalityAR = "بولينيزيي", NationalityEN = "French Polynesian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TF", NameAR = "أراض فرنسية جنوبية وأنتارتيكية", NameEN = "French Southern and Antarctic Lands", NationalityAR = "أراض فرنسية جنوبية وأنتارتيكية", NationalityEN = "French" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GA", NameAR = "الغابون", NameEN = "Gabon", NationalityAR = "غابوني", NationalityEN = "Gabonese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GM", NameAR = "غامبيا", NameEN = "Gambia", NationalityAR = "غامبي", NationalityEN = "Gambian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GE", NameAR = "جيورجيا", NameEN = "Georgia", NationalityAR = "جيورجي", NationalityEN = "Georgian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "DE", NameAR = "ألمانيا", NameEN = "Germany", NationalityAR = "ألماني", NationalityEN = "German" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GH", NameAR = "غانا", NameEN = "Ghana", NationalityAR = "غاني", NationalityEN = "Ghanaian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GI", NameAR = "جبل طارق", NameEN = "Gibraltar", NationalityAR = "جبل طارق", NationalityEN = "Gibraltar" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GR", NameAR = "اليونان", NameEN = "Greece", NationalityAR = "يوناني", NationalityEN = "Greek" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GL", NameAR = "جرينلاند", NameEN = "Greenland", NationalityAR = "جرينلاندي", NationalityEN = "Greenlandic" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GD", NameAR = "غرينادا", NameEN = "Grenada", NationalityAR = "غرينادي", NationalityEN = "Grenadian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GP", NameAR = "جزر جوادلوب", NameEN = "Guadeloupe", NationalityAR = "جزر جوادلوب", NationalityEN = "Guadeloupe" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GU", NameAR = "جوام", NameEN = "Guam", NationalityAR = "جوامي", NationalityEN = "Guamanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GT", NameAR = "غواتيمال", NameEN = "Guatemala", NationalityAR = "غواتيمالي", NationalityEN = "Guatemalan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GG", NameAR = "غيرنزي", NameEN = "Guernsey", NationalityAR = "غيرنزي", NationalityEN = "Guernsian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GN", NameAR = "غينيا", NameEN = "Guinea", NationalityAR = "غيني", NationalityEN = "Guinean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GW", NameAR = "غينيا-بيساو", NameEN = "Guinea-Bissau", NationalityAR = "غيني", NationalityEN = "Guinea-Bissauan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GY", NameAR = "غيانا", NameEN = "Guyana", NationalityAR = "غياني", NationalityEN = "Guyanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "HT", NameAR = "هايتي", NameEN = "Haiti", NationalityAR = "هايتي", NationalityEN = "Haitian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "HM", NameAR = "جزيرة هيرد وجزر ماكدونالد", NameEN = "Heard and Mc Donald Islands", NationalityAR = "جزيرة هيرد وجزر ماكدونالد", NationalityEN = "Heard and Mc Donald Islanders" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "HN", NameAR = "هندوراس", NameEN = "Honduras", NationalityAR = "هندوراسي", NationalityEN = "Honduran" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "HK", NameAR = "هونغ كونغ", NameEN = "Hong Kong", NationalityAR = "هونغ كونغي", NationalityEN = "Hongkongese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "HU", NameAR = "المجر", NameEN = "Hungary", NationalityAR = "مجري", NationalityEN = "Hungarian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IS", NameAR = "آيسلندا", NameEN = "Iceland", NationalityAR = "آيسلندي", NationalityEN = "Icelandic" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IN", NameAR = "الهند", NameEN = "India", NationalityAR = "هندي", NationalityEN = "Indian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ID", NameAR = "أندونيسيا", NameEN = "Indonesia", NationalityAR = "أندونيسيي", NationalityEN = "Indonesian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IR", NameAR = "إيران", NameEN = "Iran", NationalityAR = "إيراني", NationalityEN = "Iranian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IQ", NameAR = "العراق", NameEN = "Iraq", NationalityAR = "عراقي", NationalityEN = "Iraqi" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IE", NameAR = "إيرلندا", NameEN = "Ireland", NationalityAR = "إيرلندي", NationalityEN = "Irish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IM", NameAR = "جزيرة مان", NameEN = "Isle of Man", NationalityAR = "ماني", NationalityEN = "Manx" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IL", NameAR = "إسرائيل", NameEN = "Israel", NationalityAR = "إسرائيلي", NationalityEN = "Israeli" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "IT", NameAR = "إيطاليا", NameEN = "Italy", NationalityAR = "إيطالي", NationalityEN = "Italian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CI", NameAR = "ساحل العاج", NameEN = "Ivory Coast", NationalityAR = "ساحل العاج", NationalityEN = "Ivory Coastian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "JM", NameAR = "جمايكا", NameEN = "Jamaica", NationalityAR = "جمايكي", NationalityEN = "Jamaican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "JP", NameAR = "اليابان", NameEN = "Japan", NationalityAR = "ياباني", NationalityEN = "Japanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "JE", NameAR = "جيرزي", NameEN = "Jersey", NationalityAR = "جيرزي", NationalityEN = "Jersian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "JO", NameAR = "الأردن", NameEN = "Jordan", NationalityAR = "أردني", NationalityEN = "Jordanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KZ", NameAR = "كازاخستان", NameEN = "Kazakhstan", NationalityAR = "كازاخستاني", NationalityEN = "Kazakh" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KE", NameAR = "كينيا", NameEN = "Kenya", NationalityAR = "كيني", NationalityEN = "Kenyan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KI", NameAR = "كيريباتي", NameEN = "Kiribati", NationalityAR = "كيريباتي", NationalityEN = "I-Kiribati" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KP", NameAR = "كوريا الشمالية", NameEN = "Korea(North Korea)", NationalityAR = "كوري", NationalityEN = "North Korean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KR", NameAR = "كوريا الجنوبية", NameEN = "Korea(South Korea)", NationalityAR = "كوري", NationalityEN = "South Korean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "XK", NameAR = "كوسوفو", NameEN = "Kosovo", NationalityAR = "كوسيفي", NationalityEN = "Kosovar" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KW", NameAR = "الكويت", NameEN = "Kuwait", NationalityAR = "كويتي", NationalityEN = "Kuwaiti" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KG", NameAR = "قيرغيزستان", NameEN = "Kyrgyzstan", NationalityAR = "قيرغيزستاني", NationalityEN = "Kyrgyzstani" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LA", NameAR = "لاوس", NameEN = "Lao PDR", NationalityAR = "لاوسي", NationalityEN = "Laotian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LV", NameAR = "لاتفيا", NameEN = "Latvia", NationalityAR = "لاتيفي", NationalityEN = "Latvian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LB", NameAR = "لبنان", NameEN = "Lebanon", NationalityAR = "لبناني", NationalityEN = "Lebanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LS", NameAR = "ليسوتو", NameEN = "Lesotho", NationalityAR = "ليوسيتي", NationalityEN = "Basotho" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LR", NameAR = "ليبيريا", NameEN = "Liberia", NationalityAR = "ليبيري", NationalityEN = "Liberian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LY", NameAR = "ليبيا", NameEN = "Libya", NationalityAR = "ليبي", NationalityEN = "Libyan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LI", NameAR = "ليختنشتين", NameEN = "Liechtenstein", NationalityAR = "ليختنشتيني", NationalityEN = "Liechtenstein" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LT", NameAR = "لتوانيا", NameEN = "Lithuania", NationalityAR = "لتوانيي", NationalityEN = "Lithuanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LU", NameAR = "لوكسمبورغ", NameEN = "Luxembourg", NationalityAR = "لوكسمبورغي", NationalityEN = "Luxembourger" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MO", NameAR = "ماكاو", NameEN = "Macau", NationalityAR = "ماكاوي", NationalityEN = "Macanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MK", NameAR = "مقدونيا", NameEN = "Macedonia", NationalityAR = "مقدوني", NationalityEN = "Macedonian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MG", NameAR = "مدغشقر", NameEN = "Madagascar", NationalityAR = "مدغشقري", NationalityEN = "Malagasy" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MW", NameAR = "مالاوي", NameEN = "Malawi", NationalityAR = "مالاوي", NationalityEN = "Malawian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MY", NameAR = "ماليزيا", NameEN = "Malaysia", NationalityAR = "ماليزي", NationalityEN = "Malaysian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MV", NameAR = "المالديف", NameEN = "Maldives", NationalityAR = "مالديفي", NationalityEN = "Maldivian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ML", NameAR = "مالي", NameEN = "Mali", NationalityAR = "مالي", NationalityEN = "Malian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MT", NameAR = "مالطا", NameEN = "Malta", NationalityAR = "مالطي", NationalityEN = "Maltese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MH", NameAR = "جزر مارشال", NameEN = "Marshall Islands", NationalityAR = "مارشالي", NationalityEN = "Marshallese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MQ", NameAR = "مارتينيك", NameEN = "Martinique", NationalityAR = "مارتينيكي", NationalityEN = "Martiniquais" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MR", NameAR = "موريتانيا", NameEN = "Mauritania", NationalityAR = "موريتانيي", NationalityEN = "Mauritanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MU", NameAR = "موريشيوس", NameEN = "Mauritius", NationalityAR = "موريشيوسي", NationalityEN = "Mauritian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "YT", NameAR = "مايوت", NameEN = "Mayotte", NationalityAR = "مايوتي", NationalityEN = "Mahoran" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MX", NameAR = "المكسيك", NameEN = "Mexico", NationalityAR = "مكسيكي", NationalityEN = "Mexican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "FM", NameAR = "مايكرونيزيا", NameEN = "Micronesia", NationalityAR = "مايكرونيزيي", NationalityEN = "Micronesian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MD", NameAR = "مولدافيا", NameEN = "Moldova", NationalityAR = "مولديفي", NationalityEN = "Moldovan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MC", NameAR = "موناكو", NameEN = "Monaco", NationalityAR = "مونيكي", NationalityEN = "Monacan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MN", NameAR = "منغوليا", NameEN = "Mongolia", NationalityAR = "منغولي", NationalityEN = "Mongolian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ME", NameAR = "الجبل الأسود", NameEN = "Montenegro", NationalityAR = "الجبل الأسود", NationalityEN = "Montenegrin" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MS", NameAR = "مونتسيرات", NameEN = "Montserrat", NationalityAR = "مونتسيراتي", NationalityEN = "Montserratian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MA", NameAR = "المغرب", NameEN = "Morocco", NationalityAR = "مغربي", NationalityEN = "Moroccan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MZ", NameAR = "موزمبيق", NameEN = "Mozambique", NationalityAR = "موزمبيقي", NationalityEN = "Mozambican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MM", NameAR = "ميانمار", NameEN = "Myanmar", NationalityAR = "ميانماري", NationalityEN = "Myanmarian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NA", NameAR = "ناميبيا", NameEN = "Namibia", NationalityAR = "ناميبي", NationalityEN = "Namibian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NR", NameAR = "نورو", NameEN = "Nauru", NationalityAR = "نوري", NationalityEN = "Nauruan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NP", NameAR = "نيبال", NameEN = "Nepal", NationalityAR = "نيبالي", NationalityEN = "Nepalese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NL", NameAR = "هولندا", NameEN = "Netherlands", NationalityAR = "هولندي", NationalityEN = "Dutch" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AN", NameAR = "جزر الأنتيل الهولندي", NameEN = "Netherlands Antilles", NationalityAR = "هولندي", NationalityEN = "Dutch Antilier" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NC", NameAR = "كاليدونيا الجديدة", NameEN = "New Caledonia", NationalityAR = "كاليدوني", NationalityEN = "New Caledonian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NZ", NameAR = "نيوزيلندا", NameEN = "New Zealand", NationalityAR = "نيوزيلندي", NationalityEN = "New Zealander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NI", NameAR = "نيكاراجوا", NameEN = "Nicaragua", NationalityAR = "نيكاراجوي", NationalityEN = "Nicaraguan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NE", NameAR = "النيجر", NameEN = "Niger", NationalityAR = "نيجيري", NationalityEN = "Nigerien" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NG", NameAR = "نيجيريا", NameEN = "Nigeria", NationalityAR = "نيجيري", NationalityEN = "Nigerian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NU", NameAR = "ني", NameEN = "Niue", NationalityAR = "ني", NationalityEN = "Niuean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NF", NameAR = "جزيرة نورفولك", NameEN = "Norfolk Island", NationalityAR = "نورفوليكي", NationalityEN = "Norfolk Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MP", NameAR = "جزر ماريانا الشمالية", NameEN = "Northern Mariana Islands", NationalityAR = "ماريني", NationalityEN = "Northern Marianan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "NO", NameAR = "النرويج", NameEN = "Norway", NationalityAR = "نرويجي", NationalityEN = "Norwegian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "OM", NameAR = "عمان", NameEN = "Oman", NationalityAR = "عماني", NationalityEN = "Omani" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PK", NameAR = "باكستان", NameEN = "Pakistan", NationalityAR = "باكستاني", NationalityEN = "Pakistani" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PW", NameAR = "بالاو", NameEN = "Palau", NationalityAR = "بالاوي", NationalityEN = "Palauan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PS", NameAR = "فلسطين", NameEN = "Palestine", NationalityAR = "فلسطيني", NationalityEN = "Palestinian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PA", NameAR = "بنما", NameEN = "Panama", NationalityAR = "بنمي", NationalityEN = "Panamanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PG", NameAR = "بابوا غينيا الجديدة", NameEN = "Papua New Guinea", NationalityAR = "بابوي", NationalityEN = "Papua New Guinean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PY", NameAR = "باراغواي", NameEN = "Paraguay", NationalityAR = "بارغاوي", NationalityEN = "Paraguayan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PE", NameAR = "بيرو", NameEN = "Peru", NationalityAR = "بيري", NationalityEN = "Peruvian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PH", NameAR = "الفليبين", NameEN = "Philippines", NationalityAR = "فلبيني", NationalityEN = "Filipino" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PN", NameAR = "بيتكيرن", NameEN = "Pitcairn", NationalityAR = "بيتكيرني", NationalityEN = "Pitcairn Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PL", NameAR = "بولونيا", NameEN = "Poland", NationalityAR = "بوليني", NationalityEN = "Polish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PT", NameAR = "البرتغال", NameEN = "Portugal", NationalityAR = "برتغالي", NationalityEN = "Portuguese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "PR", NameAR = "بورتو ريكو", NameEN = "Puerto Rico", NationalityAR = "بورتي", NationalityEN = "Puerto Rican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "QA", NameAR = "قطر", NameEN = "Qatar", NationalityAR = "قطري", NationalityEN = "Qatari" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "RE", NameAR = "ريونيون", NameEN = "Reunion Island", NationalityAR = "ريونيوني", NationalityEN = "Reunionese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "RO", NameAR = "رومانيا", NameEN = "Romania", NationalityAR = "روماني", NationalityEN = "Romanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "RU", NameAR = "روسيا", NameEN = "Russian", NationalityAR = "روسي", NationalityEN = "Russian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "RW", NameAR = "رواندا", NameEN = "Rwanda", NationalityAR = "رواندا", NationalityEN = "Rwandan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "BL", NameAR = "سان بارتيلمي", NameEN = "Saint Barthelemy", NationalityAR = "سان بارتيلمي", NationalityEN = "Saint Barthelmian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SH", NameAR = "سانت هيلانة", NameEN = "Saint Helena", NationalityAR = "هيلاني", NationalityEN = "St. Helenian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "KN", NameAR = "سانت كيتس ونيفس,", NameEN = "Saint Kitts and Nevis", NationalityAR = "سانت كيتس ونيفس", NationalityEN = "Kittitian/Nevisian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "MF", NameAR = "ساينت مارتن فرنسي", NameEN = "Saint Martin (French part)", NationalityAR = "ساينت مارتني فرنسي", NationalityEN = "St. Martian(French)" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LC", NameAR = "سان بيير وميكلون", NameEN = "Saint Pierre and Miquelon", NationalityAR = "سان بيير وميكلوني", NationalityEN = "St. Pierre and Miquelon" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "VC", NameAR = "سانت فنسنت وجزر غرينادين", NameEN = "Saint Vincent and the Grenadines", NationalityAR = "سانت فنسنت وجزر غرينادين", NationalityEN = "Saint Vincent and the Grenadines" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "WS", NameAR = "ساموا", NameEN = "Samoa", NationalityAR = "ساموي", NationalityEN = "Samoan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SM", NameAR = "سان مارينو", NameEN = "San Marino", NationalityAR = "ماريني", NationalityEN = "Sammarinese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ST", NameAR = "ساو تومي وبرينسيبي", NameEN = "Sao Tome and Principe", NationalityAR = "ساو تومي وبرينسيبي", NationalityEN = "Sao Tomean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SA", NameAR = "المملكة العربية السعودية", NameEN = "Saudi Arabia", NationalityAR = "سعودي", NationalityEN = "Saudi Arabian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SN", NameAR = "السنغال", NameEN = "Senegal", NationalityAR = "سنغالي", NationalityEN = "Senegalese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "RS", NameAR = "صربيا", NameEN = "Serbia", NationalityAR = "صربي", NationalityEN = "Serbian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SC", NameAR = "سيشيل", NameEN = "Seychelles", NationalityAR = "سيشيلي", NationalityEN = "Seychellois" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SL", NameAR = "سيراليون", NameEN = "Sierra Leone", NationalityAR = "سيراليوني", NationalityEN = "Sierra Leonean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SG", NameAR = "سنغافورة", NameEN = "Singapore", NationalityAR = "سنغافوري", NationalityEN = "Singaporean" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SX", NameAR = "ساينت مارتن هولندي", NameEN = "Sint Maarten (Dutch part)", NationalityAR = "ساينت مارتني هولندي", NationalityEN = "St. Martian(Dutch)" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SK", NameAR = "سلوفاكيا", NameEN = "Slovakia", NationalityAR = "سولفاكي", NationalityEN = "Slovak" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SI", NameAR = "سلوفينيا", NameEN = "Slovenia", NationalityAR = "سولفيني", NationalityEN = "Slovenian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SB", NameAR = "جزر سليمان", NameEN = "Solomon Islands", NationalityAR = "جزر سليمان", NationalityEN = "Solomon Island" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SO", NameAR = "الصومال", NameEN = "Somalia", NationalityAR = "صومالي", NationalityEN = "Somali" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ZA", NameAR = "جنوب أفريقيا", NameEN = "South Africa", NationalityAR = "أفريقي", NationalityEN = "South African" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GS", NameAR = "المنطقة القطبية الجنوبية", NameEN = "South Georgia and the South Sandwich", NationalityAR = "لمنطقة القطبية الجنوبية", NationalityEN = "South Georgia and the South Sandwich" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SS", NameAR = "السودان الجنوبي", NameEN = "South Sudan", NationalityAR = "سوادني جنوبي", NationalityEN = "South Sudanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ES", NameAR = "إسبانيا", NameEN = "Spain", NationalityAR = "إسباني", NationalityEN = "Spanish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "LK", NameAR = "سريلانكا", NameEN = "Sri Lanka", NationalityAR = "سريلانكي", NationalityEN = "Sri Lankian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SD", NameAR = "السودان", NameEN = "Sudan", NationalityAR = "سوداني", NationalityEN = "Sudanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SR", NameAR = "سورينام", NameEN = "Suriname", NationalityAR = "سورينامي", NationalityEN = "Surinamese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SJ", NameAR = "سفالبارد ويان ماين", NameEN = "Svalbard and Jan Mayen", NationalityAR = "سفالبارد ويان ماين", NationalityEN = "Svalbardian/Jan Mayenian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SZ", NameAR = "سوازيلند", NameEN = "Swaziland", NationalityAR = "سوازيلندي", NationalityEN = "Swazi" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SE", NameAR = "السويد", NameEN = "Sweden", NationalityAR = "سويدي", NationalityEN = "Swedish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "CH", NameAR = "سويسرا", NameEN = "Switzerland", NationalityAR = "سويسري", NationalityEN = "Swiss" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "SY", NameAR = "سوريا", NameEN = "Syria", NationalityAR = "سوري", NationalityEN = "Syrian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TW", NameAR = "تايوان", NameEN = "Taiwan", NationalityAR = "تايواني", NationalityEN = "Taiwanese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TJ", NameAR = "طاجيكستان", NameEN = "Tajikistan", NationalityAR = "طاجيكستاني", NationalityEN = "Tajikistani" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TZ", NameAR = "تنزانيا", NameEN = "Tanzania", NationalityAR = "تنزانيي", NationalityEN = "Tanzanian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TH", NameAR = "تايلندا", NameEN = "Thailand", NationalityAR = "تايلندي", NationalityEN = "Thai" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TL", NameAR = "تيمور الشرقية", NameEN = "Timor-Leste", NationalityAR = "تيموري", NationalityEN = "Timor-Lestian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TG", NameAR = "توغو", NameEN = "Togo", NationalityAR = "توغي", NationalityEN = "Togolese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TK", NameAR = "توكيلاو", NameEN = "Tokelau", NationalityAR = "توكيلاوي", NationalityEN = "Tokelaian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TO", NameAR = "تونغا", NameEN = "Tonga", NationalityAR = "تونغي", NationalityEN = "Tongan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TT", NameAR = "ترينيداد وتوباغو", NameEN = "Trinidad and Tobago", NationalityAR = "ترينيداد وتوباغو", NationalityEN = "Trinidadian/Tobagonian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TN", NameAR = "تونس", NameEN = "Tunisia", NationalityAR = "تونسي", NationalityEN = "Tunisian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TR", NameAR = "تركيا", NameEN = "Turkey", NationalityAR = "تركي", NationalityEN = "Turkish" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TM", NameAR = "تركمانستان", NameEN = "Turkmenistan", NationalityAR = "تركمانستاني", NationalityEN = "Turkmen" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TC", NameAR = "جزر توركس وكايكوس", NameEN = "Turks and Caicos Islands", NationalityAR = "جزر توركس وكايكوس", NationalityEN = "Turks and Caicos Islands" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "TV", NameAR = "توفالو", NameEN = "Tuvalu", NationalityAR = "توفالي", NationalityEN = "Tuvaluan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "UG", NameAR = "أوغندا", NameEN = "Uganda", NationalityAR = "أوغندي", NationalityEN = "Ugandan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "UA", NameAR = "أوكرانيا", NameEN = "Ukraine", NationalityAR = "أوكراني", NationalityEN = "Ukrainian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "AE", NameAR = "الإمارات العربية المتحدة", NameEN = "United Arab Emirates", NationalityAR = "إماراتي", NationalityEN = "Emirati" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "GB", NameAR = "المملكة المتحدة", NameEN = "United Kingdom", NationalityAR = "بريطاني", NationalityEN = "British" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "US", NameAR = "الولايات المتحدة", NameEN = "United States", NationalityAR = "أمريكي", NationalityEN = "American" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "UY", NameAR = "أورغواي", NameEN = "Uruguay", NationalityAR = "أورغواي", NationalityEN = "Uruguayan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "UM", NameAR = "قائمة الولايات والمناطق الأمريكية", NameEN = "US Minor Outlying Islands", NationalityAR = "أمريكي", NationalityEN = "US Minor Outlying Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "UZ", NameAR = "أوزباكستان", NameEN = "Uzbekistan", NationalityAR = "أوزباكستاني", NationalityEN = "Uzbek" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "VU", NameAR = "فانواتو", NameEN = "Vanuatu", NationalityAR = "فانواتي", NationalityEN = "Vanuatuan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "VA", NameAR = "فنزويلا", NameEN = "Vatican City", NationalityAR = "فاتيكاني", NationalityEN = "Vatican" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "VE", NameAR = "فنزويلا", NameEN = "Venezuela", NationalityAR = "فنزويلي", NationalityEN = "Venezuelan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "VN", NameAR = "فيتنام", NameEN = "Vietnam", NationalityAR = "فيتنامي", NationalityEN = "Vietnamese" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "VI", NameAR = "الجزر العذراء الأمريكي", NameEN = "Virgin Islands (U.S.)", NationalityAR = "أمريكي", NationalityEN = "American Virgin Islander" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "WF", NameAR = "والس وفوتونا", NameEN = "Wallis and Futuna Islands", NationalityAR = "فوتوني", NationalityEN = "Wallisian/Futunan" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "EH", NameAR = "الصحراء الغربية", NameEN = "Western Sahara", NationalityAR = "صحراوي", NationalityEN = "Sahrawian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "YE", NameAR = "اليمن", NameEN = "Yemen", NationalityAR = "يمني", NationalityEN = "Yemeni" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ZM", NameAR = "زامبيا", NameEN = "Zambia", NationalityAR = "زامبياني", NationalityEN = "Zambian" },
                new CountriesAndNationality() { CountriesAndNationalitiesID = Guid.NewGuid(), Abbreviation = "ZW", NameAR = "زمبابوي", NameEN = "Zimbabwe", NationalityAR = "زمبابوي", NationalityEN = "Zimbabwean" }
            });

            context.AspNetUsers.AddRange(new List<AspNetUser> {
                new AspNetUser { Id = "fe998381-379b-4bfb-9a33-7fe62eaa928d", Email = "Admin@kkf.com", EmailConfirmed = false, PasswordHash = "ANC6FgFdU4Ubqp7NvB1qAXyJGvjcoQWVXV6d97HcyolT65YICYLSMFH+V1EtVYlXug==", SecurityStamp = "cfe4402b-2008-4ed1-9561-bb250a18662f", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = true, AccessFailedCount = 0, UserName = "Admin@kkf.com", AspNetRoles = aspNetRoleList.Where(r => r.Name == "Admin").ToList() },

                new AspNetUser { Id = "55b79071-41e3-4a02-be05-aca4a7dd5055", Email = "e.alsanie@kkf.org.sa", EmailConfirmed = false, PasswordHash = "AOLVhwRW8g4mglq+aF/ZvpxwOvJu3FY+CS6wGVkc1qsrDLRM2dCOW4qYR8IH5KTkxA==", SecurityStamp = "2141758b-1a98-4c4a-ba82-3c410877e9c5", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = true, AccessFailedCount = 0, UserName = "e.alsanie@kkf.org.sa", AspNetRoles = aspNetRoleList.Where(r => r.Name == "CB Manager").ToList() },
                new AspNetUser { Id = "9355973a-7397-4acf-9cc9-fa4d3651aed5", Email = "a.alkhaledi@kkf.org.sa", EmailConfirmed = false, PasswordHash = "AOLVhwRW8g4mglq+aF/ZvpxwOvJu3FY+CS6wGVkc1qsrDLRM2dCOW4qYR8IH5KTkxA==", SecurityStamp = "2141758b-1a98-4c4a-ba82-3c410877e9c5", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = true, AccessFailedCount = 0, UserName = "a.alkhaledi@kkf.org.sa", AspNetRoles = aspNetRoleList.Where(r => r.Name == "CB Analyst").ToList() },
                new AspNetUser { Id = "78f4bdd4-024b-4291-96c1-178218c9231f", Email = "a.alsaidan@kkf.org.sa", EmailConfirmed = false, PasswordHash = "ADttLCvznWm+yk4jey06kfyqpcoah1WkW6n8HP2Z6nexSCmK/7ANqkN2V0phJvVQyA==", SecurityStamp = "def9820a-863c-4068-ae20-9c0e88c048b2", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = true, AccessFailedCount = 0, UserName = "a.alsaidan@kkf.org.sa", AspNetRoles = aspNetRoleList.Where(r => r.Name == "CB Supervisor").ToList() },
                new AspNetUser { Id = "ebc1db0c-de24-42fc-a4f0-4e635f160213", Email = "h.alshafi@kkf.org.sa", EmailConfirmed = false, PasswordHash = "ADEmFfNQPwjeOf8ZlwCOKlY3GeR2wfJMAzWMfSvCAsVK+qhN6pGqZRR5gL46v4rw+w==", SecurityStamp = "37608a3c-68a3-4cf6-a810-3bd8d4aab971", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = true, AccessFailedCount = 0, UserName = "h.alshafi@kkf.org.sa", AspNetRoles = aspNetRoleList.Where(r => r.Name == "CB Analyst").ToList() }
            });

            context.BackendUsers.AddRange(new List<BackendUser> {
                new BackendUser { BackendUserID = Guid.Parse("fe998381-379b-4bfb-9a33-7fe62eaa928d"), FK_AspUser = "fe998381-379b-4bfb-9a33-7fe62eaa928d", UserName = "Admin", Password = "P@ssw0rd!@#", BackEndPositionId = LstPost.Where(f => f.NameEN == "Administration").FirstOrDefault().BackendUserPositionID, IsActive = true, CreateDate = DateTime.Now },

                new BackendUser { BackendUserID = Guid.Parse("55b79071-41e3-4a02-be05-aca4a7dd5055"), FK_AspUser = "55b79071-41e3-4a02-be05-aca4a7dd5055", UserName = "Elham Alsanie", Password = "Cbp-2019", BackEndPositionId = LstPost.Where(f => f.NameEN == "CB Manager").FirstOrDefault().BackendUserPositionID, IsActive = true, CreateDate = DateTime.Now },
                new BackendUser { BackendUserID = Guid.Parse("9355973a-7397-4acf-9cc9-fa4d3651aed5"), FK_AspUser = "9355973a-7397-4acf-9cc9-fa4d3651aed5", UserName = "Ashwaq Alkhaledi", Password = "Cbp-2019", BackEndPositionId = LstPost.Where(f => f.NameEN == "CB Analyst").FirstOrDefault().BackendUserPositionID, IsActive = true, CreateDate = DateTime.Now },
                new BackendUser { BackendUserID = Guid.Parse("78f4bdd4-024b-4291-96c1-178218c9231f"), FK_AspUser = "78f4bdd4-024b-4291-96c1-178218c9231f", UserName = "Ahmed Alsaidan", Password = "October2019", BackEndPositionId = LstPost.Where(f => f.NameEN == "CB Supervisor").FirstOrDefault().BackendUserPositionID, IsActive = true, CreateDate = DateTime.Now },
                new BackendUser { BackendUserID = Guid.Parse("ebc1db0c-de24-42fc-a4f0-4e635f160213"), FK_AspUser = "ebc1db0c-de24-42fc-a4f0-4e635f160213", UserName = "Hayam AlShafi", Password = "Cbp-2019", BackEndPositionId = LstPost.Where(f => f.NameEN == "CB Analyst").FirstOrDefault().BackendUserPositionID, IsActive = true, CreateDate = DateTime.Now }
            });
            base.Seed(context);
        }
    }
}