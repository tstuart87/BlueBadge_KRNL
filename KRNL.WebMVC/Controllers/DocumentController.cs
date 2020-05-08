using KRNL.Models;
using KRNL.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KRNL.WebMVC.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index(string searchString, string sortOrder)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            ViewBag.SortParm = sortOrder;
            ViewBag.SearchString = searchString;

            var service = new DocumentService(userId);
            var model = service.GetDocuments(userId);

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(e => e.SearchString.Contains(searchString.ToUpper()));
            }

            if (sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "Date":
                        model = model.OrderByDescending(s => s.DateCreated);
                        break;
                    case "LocID":
                        model = model.OrderBy(s => s.LocationCode);
                        break;
                    default:
                        model = model.OrderBy(s => s.LocationCode);
                        break;
                }
            }

            return View(model);
        }

        public ActionResult Create()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var locService = new LocationService(userId);
            ViewBag.locations = locService.GetLocations(userId);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentCreate model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var service = new DocumentService(Guid.Parse(User.Identity.GetUserId()));
                string path = "";
                string fileName = "";

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/Content/docs"), fileName);
                    file.SaveAs(path);
                }

                service.CreateDocument(model, path, fileName);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new DocumentService(userId);
            var model = svc.GetDocumentById(id, userId);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new DocumentService(userId);

            service.DeleteDocument(id, userId);

            return RedirectToAction("Index");
        }

        private DocumentService CreateDocumentService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new DocumentService(userId);
            return service;
        }
    }
}