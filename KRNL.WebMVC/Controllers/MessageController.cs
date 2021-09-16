using KRNL.Data;
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
    [Authorize]
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index(string searchString, string toggleView, string sortOrder)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MessageService(userId);

            var model = service.GetMessages(userId);

            ViewBag.ToggleRequestJob = "jobView";
            ViewBag.SearchString = null;
            ViewBag.SortParm = sortOrder;

            if (searchString != null)
            {
                ViewBag.SearchString = searchString;
            }

            if (toggleView != null)
            {
                ViewBag.ToggleRequestJob = toggleView;
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
                        model = model.OrderByDescending(s => s.DateCreated);
                        break;
                }
            }

            switch (ViewBag.ToggleRequestJob)
            {
                case "jobView":
                    model = model.Where(e => e.IsRequest == false);
                    break;
                case "requestView":
                    model = model.Where(e => e.IsRequest == true);
                    break;
                default:
                    model = model.Where(e => e.IsRequest == false);
                    break;
            }

            if (!String.IsNullOrEmpty(ViewBag.SearchString))
            {
                model = model.Where(e => e.SearchString.Contains(searchString.ToUpper()));
            }

            return View(model);
        }

        public ActionResult Create()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var locService = new LocationService(userId);
            ViewBag.locations = locService.GetLocations(userId);

            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId).Where(e => e.ContactType == contact.Employee);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageCreate model, HttpPostedFileBase file)
        {
            //var coopService = new CooperatorService();
            //ViewBag.cooperators = coopService.GetCooperators().Where(e => e.ContactType == contact.Employee).OrderBy(e => e.FullName);

            //var locService = new LocationService();                    I think this is copypasta accident ?
            //ViewBag.locations = locService.GetLocations().OrderBy(e => e.LocationCode);

            var userId = Guid.Parse(User.Identity.GetUserId());

            var docService = new DocumentService(userId);

            string path = "";
            string fileName = "";

            if (ModelState.IsValid)
            {
                var service = new MessageService(Guid.Parse(User.Identity.GetUserId()));
                int? docId = null;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/Content/docs"), fileName);
                    file.SaveAs(path);

                    docId = docService.CreateDocumentFromJob(model, fileName);
                }

                service.CreateMessage(model, docId);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult CreateRequest()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var locService = new LocationService(userId);
            ViewBag.locations = locService.GetLocations(userId);

            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId).Where(e => e.ContactType == contact.Employee);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRequest(MessageCreate model)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId).Where(e => e.ContactType == contact.Employee);

            var locService = new LocationService(userId);
            ViewBag.locations = locService.GetLocations(userId);

            if (ModelState.IsValid)
            {
                var service = new MessageService(Guid.Parse(User.Identity.GetUserId()));
                var result = service.CreateRequest(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new MessageService(userId);
            var model = svc.GetMessageById(id, userId);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId).Where(e => e.ContactType == contact.Employee);

            var locService = new LocationService(userId);
            ViewBag.locations = locService.GetLocations(userId);

            var service = CreateMessageService();
            var detail = service.GetMessageEditById(id, userId);
            var model = new MessageEdit
            {
                MessageId = detail.MessageId,
                LocationId = detail.LocationId,
                Comment = detail.Comment,
                CooperatorId = detail.CooperatorId,
                DateCreated = detail.DateCreated,
                OwnerId = detail.OwnerId,
                JobOne = detail.JobOne,
                JobTwo = detail.JobTwo,
                JobThree = detail.JobThree,
                IsRequest = detail.IsRequest,
                HumanGrowthStage = detail.HumanGrowthStage,
                Rating = detail.Rating
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MessageEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.MessageId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MessageService(userId);

            if (service.UpdateMessage(model, userId))
            {
                TempData["SaveResult"] = "Your message was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your message could not be updated.");
            return View(model);
        }

        public ActionResult CompleteRequest(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId).Where(e => e.ContactType == contact.Employee);

            var locService = new LocationService(userId);
            ViewBag.locations = locService.GetLocations(userId);

            var service = CreateMessageService();
            var detail = service.GetMessageEditById(id, userId);
            var model = new MessageEdit
            {
                MessageId = detail.MessageId,
                LocationId = detail.LocationId,
                LocationCode = detail.LocationCode,
                Comment = detail.Comment,
                CooperatorId = detail.CooperatorId,
                DateCreated = detail.DateCreated,
                OwnerId = detail.OwnerId,
                JobOne = detail.JobOne,
                JobTwo = detail.JobTwo,
                JobThree = detail.JobThree,
                IsRequest = detail.IsRequest,
                HumanGrowthStage = detail.HumanGrowthStage,
                Rating = detail.Rating
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteRequest(int id, MessageEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.MessageId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MessageService(userId);

            if (service.UpdateMessage(model, userId))
            {
                service.SetIsRequestToNo(model.MessageId, model.LocationId, model.DateCreated, userId);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your message could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var svc = new MessageService(userId);
            var model = svc.GetMessageById(id, userId);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MessageService(userId);

            service.DeleteMessage(id, userId);

            return RedirectToAction("Index");
        }

        private MessageService CreateMessageService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new MessageService(userId);
            return service;
        }
    }
}