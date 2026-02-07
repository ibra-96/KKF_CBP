using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using ClosedXML.Excel;
using System.Data;
using System.IO;
namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class CorporationRegisteredController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public CorporationRegisteredController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<CorporateApplicationFormAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: CorporationRegistered
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;

            string ProgramType = "";
            if (User.IsInRole("CB Manager") || User.IsInRole("CB Supervisor") || User.IsInRole("CB Analyst"))
            {
                ProgramType = "Capacity Building";
            }
            var LstFrontendUserNotApproved = (User.IsInRole("Admin") && string.IsNullOrWhiteSpace(ProgramType)) ?
                repository.Find<CorporateApplicationForm>(f => f.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName != "Pending").ToList() :
                repository.Find<CorporateApplicationForm>(f => (f.Program.ProgramName == ProgramType) && f.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName != "Pending").ToList();
            //return View(LstFrontendUserNotApproved.OrderByDescending(o => o.CorporateApplicationStatu.DateTimeMakeAction));
            return View(LstFrontendUserNotApproved
            .Where(o => o.CorporateApplicationStatu != null) 
                   .OrderByDescending(o => o.CorporateApplicationStatu.DateTimeMakeAction));

        }
        //23-1-2025
        public ActionResult ExportToExcel()
        {
      
            var data = (from caf in repository.GetAll<CorporateApplicationForm>()
                        where !caf.IsDraft // فقط النماذج غير المسودة
                        join cas in repository.GetAll<CorporateApplicationStatu>() on caf.CorporateApplicationFormID equals cas.CorporateApplicationFormID into casGroup
                        from cas in casGroup.DefaultIfEmpty()
                        join as1 in repository.GetAll<ApplicantStatu>() 
                        on cas.ApplicantStatusID equals as1.ApplicantStatusID into statusGroup
                        from as1 in statusGroup.DefaultIfEmpty()
                        join u in repository.GetAll<AspNetUser>() on caf.FrontendUserID.ToString() equals u.Id into userGroup
                        from u in userGroup.DefaultIfEmpty()
                        join c in repository.GetAll<City>() on caf.CityID equals c.CityID into cityGroup
                        from c in cityGroup.DefaultIfEmpty() // معالجة القيم الفارغة
                        join g in repository.GetAll<Governorate>() on (c != null ? c.GovernorateID : Guid.Empty) equals g.GovernorateID into governorateGroup
                        from g in governorateGroup.DefaultIfEmpty() // معالجة القيم الفارغة
                        join r in repository.GetAll<Region>() on (g!=null ? g.RegionID: Guid.Empty)  equals r.RegionID into regionGroup
                        from r in regionGroup.DefaultIfEmpty() // معالجة القيم الفارغة
                        join cc in repository.GetAll<CorporationsCategory>() on caf.CorporationsCategoryID equals cc.CorporationsCategoryID into categoryGroup
                        from cc in categoryGroup.DefaultIfEmpty()
                        join cf in repository.GetAll<CorporateFieldOfWork>() on caf.CorporateFieldOfWorkID equals cf.CorporateFieldOfWorkID into fieldGroup
                        from cf in fieldGroup.DefaultIfEmpty()
                        join cs in repository.GetAll<ClassificationSector>() on caf.ClassificationSectorID equals cs.ClassificationSectorID into sectorGroup
                        from cs in sectorGroup.DefaultIfEmpty()
                        join aa in repository.GetAll<AuthorizationAuthority>() on caf.AuthorizationAuthorityID equals aa.AuthorizationAuthorityID into authorityGroup
                        from aa in authorityGroup.DefaultIfEmpty()
                        select new
                        {
                            OrganizationName = caf.Name ?? "N/A",
                            FoundedYear = caf.FoundedYear,
                            AuthorizationAuthority = aa?.AuthorizationAuthorityNameAR ?? "N/A",
                            CorporationCategory = cc?.CorporationsCategoryNameAR ?? "N/A",
                            CorporateFieldOfWork = cf?.CorporateFieldOfWorkNameAR ?? "N/A",
                            ClassificationSector = cs?.ClassificationSectorNameAR ?? "N/A",
                            CityName = c != null ? c.CityNameAR : "N/A",
                            GovernorateName = g != null ? g.GovernorateAR : "N/A",
                            RegionName = r != null ? r.RegionNameAR : "N/A",
                            CorporateAdministratorName = caf.CorporateAdministratorName ?? "N/A",
                            CorporateAdministratorMobileNumber = caf.CorporateAdministratorMobileNumber ?? "N/A",
                            CorporateAdministratorTelephoneNumber = caf.CorporateAdministratorTelephoneNumber ?? "N/A",
                            CorporateAdministratorExtension = caf.CorporateAdministratorExtension ?? "N/A",
                            OfficialEmail = caf.OfficialEmail ?? "N/A",
                            UserEmail = u?.Email ?? "N/A",
                            AdministratorEmail = caf.AdministratorEmail ?? "N/A",
                            Website = caf.Website ?? "N/A",
                            UserName = u?.UserName ?? "N/A",
                            TelephoneNumber = caf.TelephoneNumber ?? "N/A",
                            ApplicantStatus = as1?.ApplicantStatusName ?? "N/A"
                        }).ToList();

            // التحقق من وجود بيانات
            if (!data.Any())
            {
                TempData["ErrorMessage"] = "No data available for export.";
                return RedirectToAction("Index");
            }

            // تحويل البيانات إلى DataTable بنفس الترتيب
            DataTable dt = new DataTable("ExportedData");
            dt.Columns.AddRange(new DataColumn[]
            {
        new DataColumn("اسم المنظمة/ المؤسسة"),
        new DataColumn("سنة التاسيس"),
        new DataColumn("جهة التسجيل"),
        new DataColumn("القطاع"),
        new DataColumn("مجال العمل"),
        new DataColumn("تصنيف القطاع"),
        new DataColumn("المنطقة"),
        new DataColumn("المحافظة"),
        new DataColumn("الحي"),
        new DataColumn("اسم الرئيس التنفيذي/مدير المنظمة"),
        new DataColumn("الجوال"),
        new DataColumn("رقم الهاتف"),
        new DataColumn("التحويلة"),
        new DataColumn("البريد الإلكتروني الرسمي"),
        new DataColumn("البريد الإلكتروني المسجل عند إنشاء الحساب"),
        new DataColumn("البريد الإلكتروني لمدير المنظمة"),
        new DataColumn("الموقع الإلكتروني"),
        new DataColumn("اسم المستخدم"),
        new DataColumn("جوال المنظمة"),
        new DataColumn("الموقع الالكتروني")
            });

            foreach (var item in data)
            {
                dt.Rows.Add(item.OrganizationName, item.FoundedYear, item.AuthorizationAuthority, item.CorporationCategory,
                            item.CorporateFieldOfWork, item.ClassificationSector, item.CityName, item.GovernorateName,
                            item.RegionName, item.CorporateAdministratorName, item.CorporateAdministratorMobileNumber,
                            item.CorporateAdministratorTelephoneNumber, item.CorporateAdministratorExtension,
                            item.OfficialEmail, item.UserEmail, item.AdministratorEmail, item.Website,
                            item.UserName, item.TelephoneNumber, item.ApplicantStatus);
            }

            // إنشاء ملف Excel باستخدام ClosedXML
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Data");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    string fileName = $"Corporations_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

    }





}