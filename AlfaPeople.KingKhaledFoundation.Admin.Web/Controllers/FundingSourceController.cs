using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class FundingSourceController : BaseController
    {
        private readonly IRepository repository;

        public FundingSourceController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult Index()
        {
            return View(repository.GetAll<FundingSource>().ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FundingSourceVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.FundingSource.UseCustomThemes)
                {
                    if (repository.Get<FundingSource>(f => f.Nickname == model.FundingSource.Nickname).Count() == 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                model.FundingSource.FundingSourceID = Guid.NewGuid();

                                if ((model.LogoPic != null && model.LogoPic.ContentLength > 0) &&
                                    (model.LogoPic.ContentType == "image/jpeg" || model.LogoPic.ContentType == "image/png" || model.LogoPic.ContentType == "image/gif" || model.LogoPic.ContentType == "image/x-icon"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.LogoPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.GrantLogoPic = string.Format("data:{0};base64,{1}", model.LogoPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadLogo;
                                    return View(model);
                                }

                                if ((model.HeaderPic != null && model.HeaderPic.ContentLength > 0) &&
                                    (model.HeaderPic.ContentType == "image/jpeg" || model.HeaderPic.ContentType == "image/png" || model.HeaderPic.ContentType == "image/gif"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.HeaderPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.GrantHeaderPic = string.Format("data:{0};base64,{1}", model.HeaderPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadHeader;
                                    return View(model);
                                }

                                if ((model.GBackgroundPic != null && model.GBackgroundPic.ContentLength > 0) &&
                                    (model.GBackgroundPic.ContentType == "image/jpeg" || model.GBackgroundPic.ContentType == "image/png" || model.GBackgroundPic.ContentType == "image/gif"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.GBackgroundPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.GrantBackgroundPic = string.Format("data:{0};base64,{1}", model.RBackgroundPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadGBackground;
                                    return View(model);
                                }

                                if ((model.RBackgroundPic != null && model.RBackgroundPic.ContentLength > 0) &&
                                    (model.RBackgroundPic.ContentType == "image/jpeg" || model.RBackgroundPic.ContentType == "image/png" || model.RBackgroundPic.ContentType == "image/gif"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.RBackgroundPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.RegistrationBackgroundPic = string.Format("data:{0};base64,{1}", model.RBackgroundPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadRBackground;
                                    return View(model);
                                }

                                repository.Add(model.FundingSource);
                                repository.UnitOfWork.SaveChanges();
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                ViewBag.Message = ex.Message;
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Message = App_GlobalResources.FundingSource.MsgAlreadyExist;
                }
                else
                {
                    if (repository.Get<FundingSource>(f => f.Nickname == model.FundingSource.Nickname).Count() == 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                repository.Add(model.FundingSource);
                                repository.UnitOfWork.SaveChanges();
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                ViewBag.Message = ex.Message;
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Message = App_GlobalResources.FundingSource.MsgAlreadyExist;
                }
            }
            else
                ViewBag.Message = App_GlobalResources.FundingSource.MsgFillData;

            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Create");

            var fundingSourceVM = new FundingSourceVM()
            {
                FundingSource = repository.GetByKey<FundingSource>(id),
                LogoPic = null,
                HeaderPic = null,
                GBackgroundPic = null,
                RBackgroundPic = null
            };

            if (fundingSourceVM.FundingSource == null)
                return RedirectToAction("Create");

            return View(fundingSourceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FundingSourceVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.FundingSource.UseCustomThemes)
                {
                    if (repository.Get<FundingSource>(f => f.Nickname == model.FundingSource.Nickname && f.FundingSourceID != model.FundingSource.FundingSourceID).Count() == 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                if ((model.LogoPic != null && model.LogoPic.ContentLength > 0) &&
                                    (model.LogoPic.ContentType == "image/jpeg" || model.LogoPic.ContentType == "image/png" || model.LogoPic.ContentType == "image/gif" || model.LogoPic.ContentType == "image/x-icon"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.LogoPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.GrantLogoPic = string.Format("data:{0};base64,{1}", model.LogoPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else if (model.LogoPic == null && string.IsNullOrWhiteSpace(model.FundingSource.GrantLogoPic))
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadLogo;
                                    return View(model);
                                }

                                if ((model.HeaderPic != null && model.HeaderPic.ContentLength > 0) &&
                                    (model.HeaderPic.ContentType == "image/jpeg" || model.HeaderPic.ContentType == "image/png" || model.HeaderPic.ContentType == "image/gif"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.HeaderPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.GrantHeaderPic = string.Format("data:{0};base64,{1}", model.HeaderPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else if (model.HeaderPic == null && string.IsNullOrWhiteSpace(model.FundingSource.GrantHeaderPic))
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadHeader;
                                    return View(model);
                                }

                                if ((model.GBackgroundPic != null && model.GBackgroundPic.ContentLength > 0) &&
                                    (model.GBackgroundPic.ContentType == "image/jpeg" || model.GBackgroundPic.ContentType == "image/png" || model.GBackgroundPic.ContentType == "image/gif"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.GBackgroundPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.GrantBackgroundPic = string.Format("data:{0};base64,{1}", model.RBackgroundPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else if (model.GBackgroundPic == null && string.IsNullOrWhiteSpace(model.FundingSource.GrantBackgroundPic))
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadGBackground;
                                    return View(model);
                                }

                                if ((model.RBackgroundPic != null && model.RBackgroundPic.ContentLength > 0) &&
                                    (model.RBackgroundPic.ContentType == "image/jpeg" || model.RBackgroundPic.ContentType == "image/png" || model.RBackgroundPic.ContentType == "image/gif"))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        model.RBackgroundPic.InputStream.CopyTo(memoryStream);
                                        model.FundingSource.RegistrationBackgroundPic = string.Format("data:{0};base64,{1}", model.RBackgroundPic.ContentType, Convert.ToBase64String(memoryStream.GetBuffer()));
                                    }
                                }
                                else if (model.RBackgroundPic == null && string.IsNullOrWhiteSpace(model.FundingSource.RegistrationBackgroundPic))
                                {
                                    ViewBag.Message = App_GlobalResources.FundingSource.MsgUploadRBackground;
                                    return View(model);
                                }

                                repository.Update(model.FundingSource);
                                repository.UnitOfWork.SaveChanges();
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                ViewBag.Message = ex.Message;
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Message = App_GlobalResources.FundingSource.MsgAlreadyExist;
                }
                else
                {
                    if (repository.Get<FundingSource>(f => f.Nickname == model.FundingSource.Nickname && f.FundingSourceID != model.FundingSource.FundingSourceID).Count() == 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                model.FundingSource.GrantLogoPic = null;
                                model.FundingSource.GrantHeaderPic = null;
                                model.FundingSource.GrantBackgroundPic = null;
                                model.FundingSource.RegistrationBackgroundPic = null;

                                repository.Update(model.FundingSource);
                                repository.UnitOfWork.SaveChanges();
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                ViewBag.Message = ex.Message;
                                return View(model);
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Message = App_GlobalResources.FundingSource.MsgAlreadyExist;
                }
            }
            else
                ViewBag.Message = App_GlobalResources.FundingSource.MsgFillData;

            return View(model);
        }
    }
}