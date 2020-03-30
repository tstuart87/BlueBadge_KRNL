using KRNL.Data;
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
        // GET: Message
        //public ActionResult Index()
        //{
        //    var service = new LocationService();
        //    var model = service.GetLocations();
        //    return View(model);
        //}

        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.LocationSortParm = sortOrder == "locationName_desc" ? "locationName" : "locationName_desc";
            ViewBag.GDUSortParm = sortOrder == "GDUs" ? "GDUs_desc" : "GDUs";
            ViewBag.LocIDSortParm = sortOrder == "LocID" ? "LocID_desc" : "LocID";
            var service = new LocationService();
            var model = service.GetLocations();

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(e => e.LocationCode.Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "locationName":
                    model = model.OrderBy(s => s.LocationName);
                    break;
                case "locationName_desc":
                    model = model.OrderByDescending(s => s.LocationName);
                    break;
                case "GDUs":
                    model = model.OrderByDescending(s => s.GDUs);
                    break;
                case "GDUs_desc":
                    model = model.OrderBy(s => s.GDUs);
                    break;
                case "LocID":
                    model = model.OrderBy(s => s.LocationCode);
                    break;
                case "LocID_desc":
                    model = model.OrderByDescending(s => s.LocationCode);
                    break;
                default:
                    model = model.OrderBy(s => s.LocationName);
                    break;
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
            var service = CreateLocationService();
            var model = service.GetLocationById(id);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var coopService = new CooperatorService();
            ViewBag.cooperators = coopService.GetCooperators();

            var service = CreateLocationService();
            var detail = service.GetLocationEditById(id);
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

        public ActionResult EditCoop(int id)
        {
            var coopService = new CooperatorService();
            ViewBag.cooperators = coopService.GetCooperators();

            var service = CreateLocationService();
            var detail = service.GetLocationEditById(id);
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
            var service = CreateLocationService();
            var model = service.GetLocationById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateLocationService();

            service.DeleteLocation(id);

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
