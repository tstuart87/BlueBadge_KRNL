﻿using KRNL.Data;
using KRNL.Models;
using KRNL.Services;
using KRNL.WebMVC.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KRNL.WebMVC.Controllers
{
    [Authorize]
    public class CooperatorController : Controller
    {
        // GET: Cooperator
        public ActionResult Index(string searchString, string toggleView)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);
            var model = service.GetCooperators(userId);
            model.OrderBy(s => s.LastName);

            ViewBag.ToggleView = "viewOne";
            ViewBag.SearchString = searchString;

            if (toggleView != null)
            {
                ViewBag.ToggleView = toggleView;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(e => e.SearchString.Contains(searchString.ToUpper()));
            }

            return View(model.OrderBy(e => e.FullName));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CooperatorCreate model)
        {
            if (ModelState.IsValid)
            {
                var service = new CooperatorService(Guid.Parse(User.Identity.GetUserId()));
                var result = service.CreateCooperator(model);
                TempData["SaveResult"] = "New cooperator successfully created.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);
            var model = service.GetCooperatorById(id, userId);

            return View(model);
        }

        public ActionResult Edit (int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);
            var detail = service.GetCooperatorById(id, userId);
            var model = new CooperatorEdit
            {
                CooperatorId = detail.CooperatorId,
                FirstName = detail.FirstName,
                LastName = detail.LastName,
                Phone = detail.Phone,
                Email = detail.Email,
                ContactType = detail.ContactType,
                OwnerId = detail.OwnerId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CooperatorEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CooperatorId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);

            if (service.UpdateCooperator(model, userId))
            {
                TempData["SaveResult"] = "Your cooperator was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your cooperator could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);
            var model = service.GetCooperatorById(id, userId);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);

            service.DeleteCooperator(id, userId);

            return RedirectToAction("Index");
        }

        private CooperatorService CreateCooperatorService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CooperatorService(userId);
            return service;
        }
    }
}