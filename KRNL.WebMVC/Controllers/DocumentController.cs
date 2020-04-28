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
        public ActionResult Index()
        {
            var service = new DocumentService();
            var model = service.GetDocuments();

            return View(model);
        }

        public ActionResult Create()
        {
            var locService = new LocationService();
            ViewBag.locations = locService.GetLocations();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentCreate model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var service = new DocumentService(Guid.Parse(User.Identity.GetUserId()));
                string pathString = "";

                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Docs/uploads"), fileName);
                    file.SaveAs(path);
                    pathString = path.ToString();
                }

                service.CreateDocument(model, pathString);


                TempData["SaveResult"] = "New document successfully uploaded.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        private DocumentService CreateDocumentService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new DocumentService(userId);
            return service;
        }
    }
}