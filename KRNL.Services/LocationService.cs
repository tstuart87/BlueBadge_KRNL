using System;
using System.Collections.Generic;
using System.Linq;
using KRNL.Data;
using KRNL.Models;
using KRNL.WebMVC.Data;
using OpenQA.Selenium.Chrome;


namespace KRNL.Services
{
    public class LocationService
    {
        private readonly Guid _userId;

        public LocationService(Guid userId)
        {
            _userId = userId;
        }

        public LocationService()
        {

        }

        public bool CreateLocation(LocationCreate model)
        {
            var entity = new Location()
            {
                LocationName = model.LocationName,
                LocationCode = model.LocationCode,
                State = model.State,
                OwnerId = _userId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Locations.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<LocationListItem> GetLocations()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Locations
                        .Select(
                            e =>
                                new LocationListItem
                                {
                                    LocationId = e.LocationId,
                                    LocationName = e.LocationName,
                                    State = e.State,
                                    LocationCode = e.LocationCode,
                                    IsStaked = e.IsStaked,
                                    GDUs = e.GDUs,
                                    Latitude = e.Latitude,
                                    Longitude = e.Longitude,
                                    MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + e.Latitude + "," + e.Longitude
                                }
                        );

                //return query.ToArray();
                return query.ToList();
            }
        }

        public bool UpdateLocation(LocationEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Locations.Single(e => e.LocationId == model.LocationId);

                entity.LocationName = model.LocationName;
                entity.LocationCode = model.LocationCode;
                entity.State = model.State;
                entity.Longitude = model.Longitude;
                entity.Latitude = model.Latitude;
                entity.MonthOfPlanting = model.MonthOfPlanting;
                entity.DayOfPlanting = model.DayOfPlanting;
                entity.YearOfPlanting = model.YearOfPlanting;
                entity.IsStaked = model.IsStaked;
                entity.CooperatorId = model.CooperatorId;

                return ctx.SaveChanges() == 1;
            }
        }

        public LocationDetail GetLocationById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Locations
                        .Single(e => e.LocationId == id);

                if (entity.CooperatorId != null)
                {
                    return
                        new LocationDetail
                        {
                            LocationId = entity.LocationId,
                            LocationName = entity.LocationName,
                            State = entity.State,
                            LocationCode = entity.LocationCode,
                            GDUs = entity.GDUs,
                            DatePlanted = entity.DatePlanted,
                            IsStaked = entity.IsStaked,
                            CooperatorId = entity.CooperatorId,
                            FullName = entity.Cooperators.FirstName + " " + entity.Cooperators.LastName,
                            MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + entity.Latitude + "," + entity.Longitude
                        };
                }

                return
                    new LocationDetail
                    {
                        LocationId = entity.LocationId,
                        LocationName = entity.LocationName,
                        State = entity.State,
                        LocationCode = entity.LocationCode,
                        GDUs = entity.GDUs,
                        DatePlanted = entity.DatePlanted,
                        IsStaked = entity.IsStaked,
                        CooperatorId = entity.CooperatorId,
                        FullName = null,
                        MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + entity.Latitude + "," + entity.Longitude
                    };
            }
        }

        public LocationEdit GetLocationEditById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Locations
                        .Single(e => e.LocationId == id);
                if (entity.CooperatorId != null)
                {
                    return
                        new LocationEdit
                        {
                            LocationId = entity.LocationId,
                            LocationName = entity.LocationName,
                            State = entity.State,
                            LocationCode = entity.LocationCode,
                            Longitude = entity.Longitude,
                            Latitude = entity.Latitude,
                            MonthOfPlanting = entity.MonthOfPlanting,
                            DayOfPlanting = entity.DayOfPlanting,
                            YearOfPlanting = entity.YearOfPlanting,
                            IsStaked = entity.IsStaked,
                            FullName = entity.Cooperators.FirstName + " " + entity.Cooperators.LastName
                        };
                }

                return
                    new LocationEdit
                    {
                        LocationId = entity.LocationId,
                        LocationName = entity.LocationName,
                        State = entity.State,
                        LocationCode = entity.LocationCode,
                        Longitude = entity.Longitude,
                        Latitude = entity.Latitude,
                        MonthOfPlanting = entity.MonthOfPlanting,
                        DayOfPlanting = entity.DayOfPlanting,
                        YearOfPlanting = entity.YearOfPlanting,
                        IsStaked = entity.IsStaked,
                        FullName = null
                    };
            }
        }

        public bool DeleteLocation(int locationId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Locations
                        .Single(e => e.LocationId == locationId);

                ctx.Locations.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGdus()
        {
            using (var ctx = new ApplicationDbContext())
            {
                foreach (Location x in ctx.Locations)
                {
                    x.GDUs = SetGDUsForLocation(x).ToString();
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public int SetGDUsForLocation(Location model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                using (var driver = new ChromeDriver())
                {
                    driver.Navigate().GoToUrl("https://nutrien-ekonomics.com/tools-to-calculate-fertilizer-needs/calculators/gdd/");

                    var zipCode = driver.FindElementByName("zipcode");
                    var cropType = driver.FindElementByClassName("corn");
                    var startMonth = driver.FindElementByName("date_start_mm");
                    var startDay = driver.FindElementByName("date_start_dd");
                    var startYear = driver.FindElementByName("date_start_yyyy");
                    var endMonth = driver.FindElementByName("date_end_mm");
                    var endDay = driver.FindElementByName("date_end_ddd");
                    var endYear = driver.FindElementByName("date_end_yyyy");

                    var calculateButton = driver.FindElementByClassName("btn");

                    zipCode.SendKeys((model.LocationName.ToString() + ", " + model.State).ToString());
                    cropType.Click();
                    startMonth.SendKeys((Convert.ToInt32(model.MonthOfPlanting)).ToString());
                    startDay.SendKeys(model.DayOfPlanting.ToString());
                    startYear.SendKeys(model.YearOfPlanting.ToString());
                    endMonth.SendKeys("03");
                    endDay.SendKeys("26");
                    endYear.SendKeys("2020");

                    calculateButton.Click();

                    WaitForPageToLoad(driver);

                    var result = (driver.FindElementByXPath("//div[@class='growing_degree_days']/h2").Text).ToString();

                    return Convert.ToInt32(result);
                }
            }
        }

        public void WaitForPageToLoad(ChromeDriver driver)
        {
            while ((driver.FindElementsByXPath("//div[@class='growing_degree_days']/h2")).Count() == 0)
            {
                WaitForPageToLoad(driver);
            }
        }
    }
}
