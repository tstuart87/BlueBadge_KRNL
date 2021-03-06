﻿using KRNL.Data;
using KRNL.Models;
using KRNL.Services;
using KRNL.WebMVC.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KRNL.WebMVC.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        public ActionResult Index(string sortOrder, string searchString, string toggleView)
        {
            ViewBag.SortParm = sortOrder;

            ViewBag.ToggleView = "viewOne";

            if (toggleView != null)
            {
                ViewBag.ToggleView = toggleView;
            }

            ViewBag.SearchString = searchString;

            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LocationService(userId);

            var model = service.GetLocations(userId);

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(e => e.SearchString.Contains(searchString.ToUpper()));
            }

            if (sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "GDUs":
                        model = model.OrderByDescending(s => Convert.ToInt32(s.GDUs));
                        break;
                    case "GDUs_desc":
                        model = model.OrderBy(s => Convert.ToInt32(s.GDUs));
                        break;
                    case "LocID":
                        model = model.OrderBy(s => s.LocationCode);
                        break;
                    case "LocID_desc":
                        model = model.OrderByDescending(s => s.LocationCode);
                        break;
                    case "GrowthStage":
                        model = model.OrderBy(s => s.GrowthStage);
                        break;
                    case "GrowthStage_desc":
                        model = model.OrderByDescending(s => s.GrowthStage);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationCreate model)
        {
            if (ModelState.IsValid)
            {
                var service = new LocationService(Guid.Parse(User.Identity.GetUserId()));
                var result = service.CreateLocation(model);
                TempData["SaveResult"] = "New location successfully created.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            var service = CreateLocationService();
            var model = service.GetLocationById(id, userId);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId);

            var messageService = new MessageService(userId);
            ViewBag.messages = messageService.GetMessages(id, userId);

            var documentService = new DocumentService(userId);
            ViewBag.documents = documentService.GetDocuments(id, userId);

            var service = CreateLocationService();
            var detail = service.GetLocationEditById(id, userId);
            var model = new LocationEdit
            {
                LocationId = detail.LocationId,
                LocationName = detail.LocationName,
                State = detail.State,
                LocationCode = detail.LocationCode,
                Longitude = detail.Longitude,
                Latitude = detail.Latitude,
                MonthOfPlanting = detail.MonthOfPlanting,
                DayOfPlanting = detail.DayOfPlanting,
                YearOfPlanting = detail.YearOfPlanting,
                CooperatorId = detail.CooperatorId,
                FullName = detail.FullName,
                CRM = detail.CRM,
                Messages = detail.Messages,
                Documents = detail.Documents,
                Tag = detail.Tag,
                SearchString = detail.SearchString,
                DatePlanted = detail.DatePlanted,
                DateHarvested = detail.DateHarvested,
                IsPlanted = detail.IsPlanted,
                IsRowbanded = detail.IsRowbanded,
                IsStaked = detail.IsStaked,
                IsHarvested = detail.IsHarvested,
                DocString = detail.DocString,
                Rating = detail.Rating,
                MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + detail.Latitude + "," + detail.Longitude
            };
            return View(model);
        }


        public ActionResult EditCoop(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var coopService = new CooperatorService(userId);
            ViewBag.cooperators = coopService.GetCooperators(userId).Where(e => e.ContactType == contact.Cooperator);

            var service = CreateLocationService();
            var detail = service.GetLocationEditById(id, userId);
            var model = new LocationEdit
            {
                LocationId = detail.LocationId,
                LocationName = detail.LocationName,
                State = detail.State,
                LocationCode = detail.LocationCode,
                Longitude = detail.Longitude,
                Latitude = detail.Latitude,
                MonthOfPlanting = detail.MonthOfPlanting,
                DayOfPlanting = detail.DayOfPlanting,
                YearOfPlanting = detail.YearOfPlanting,
                IsStaked = detail.IsStaked,
                CooperatorId = detail.CooperatorId,
                FullName = detail.FullName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocationEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.LocationId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateLocationService();

            if (service.UpdateLocation(model))
            {
                TempData["SaveResult"] = "Your location was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your location could not be updated.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCoop(int id, LocationEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.LocationId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateLocationService();

            if (service.UpdateLocation(model))
            {
                TempData["SaveResult"] = "Your location was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your location could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = CreateLocationService();
            var model = service.GetLocationById(id, userId);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LocationService(userId);

            service.DeleteLocation(id, userId);

            TempData["SaveResult"] = "Your location was deleted";

            return RedirectToAction("Index");
        }

        private LocationService CreateLocationService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new LocationService(userId);
            return service;
        }
    }
}
