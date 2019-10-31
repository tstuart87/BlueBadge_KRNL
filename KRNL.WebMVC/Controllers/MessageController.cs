using KRNL.Models;
using KRNL.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KRNL.WebMVC.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            var service = new MessageService();
            var model = service.GetMessages();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageCreate model)
        {
            if (ModelState.IsValid)
            {
                var service = new MessageService(Guid.Parse(User.Identity.GetUserId()));
                var result = service.CreateMessage(model);
                TempData["SaveResult"] = "New comment successfully created.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateMessageService();
            var model = svc.GetMessageById(id);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateMessageService();
            var detail = service.GetMessageEditById(id);
            var model = new MessageEdit
            {
                MessageId = detail.MessageId,
                Comment = detail.Comment,
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

            var service = CreateMessageService();

            if (service.UpdateMessage(model))
            {
                TempData["SaveResult"] = "Your message was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your message could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateMessageService();
            var model = svc.GetMessageById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateMessageService();

            service.DeleteMessage(id);

            TempData["SaveResult"] = "Your message was deleted";

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