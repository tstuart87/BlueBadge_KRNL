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
                                    GrowthStage = e.GrowthStage,
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

                DateTimeOffset date = DateTimeOffset.Parse("1/1/0001");

                if (model.DayOfPlanting > 1 && model.YearOfPlanting > 2)
                {
                    date = DateTimeOffset.Parse(((((int)model.MonthOfPlanting) + "/" + model.DayOfPlanting + "/" + (model.YearOfPlanting.ToString())).ToString()));
                }

                entity.DatePlanted = date;
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

                if (entity.CooperatorId == null)
                {
                    return
                        new LocationDetail
                        {
                            LocationId = entity.LocationId,
                            LocationName = entity.LocationName,
                            State = entity.State,
                            LocationCode = entity.LocationCode,
                            GDUs = entity.GDUs,
                            GrowthStage = entity.GrowthStage,
                            DatePlanted = entity.DatePlanted,
                            IsStaked = entity.IsStaked,
                            //CooperatorId = entity.CooperatorId,
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
                        GrowthStage = entity.GrowthStage,
                        DatePlanted = entity.DatePlanted,
                        IsStaked = entity.IsStaked,
                        CooperatorId = entity.CooperatorId,
                        FullName = entity.Cooperators.FullName,
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

                if (entity.CooperatorId == null)
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
                            //CooperatorId = entity.CooperatorId,
                            //FullName = entity.Cooperators.FullName,
                            MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + entity.Latitude + "," + entity.Longitude
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
                        CooperatorId = entity.CooperatorId,
                        FullName = entity.Cooperators.FullName,
                        MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + entity.Latitude + "," + entity.Longitude
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

        public bool SetGdusAndCumulativePrecipAndGrowthStages()
        {
            using (var ctx = new ApplicationDbContext())
            {
                foreach (Location x in ctx.Locations)
                {
                    if (x.DayOfPlanting > 0 && x.YearOfPlanting > 2)
                    {
                        x.GDUs = SetGDUsForLocation(x).ToString();
                        x.CumulativePrecip = SetCumulativePrecipForLocation(x).ToString();
                    }
                }

                CalculateGrowthStage();

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
                    endMonth.SendKeys("07");
                    endDay.SendKeys("27");
                    endYear.SendKeys("2019");

                    calculateButton.Click();

                    WaitForPageToLoadgdu(driver);

                    var result = (driver.FindElementByXPath("//div[@class='growing_degree_days']/h2").Text).ToString();

                    return Convert.ToInt32(result);
                }
            }
        }

        public void WaitForPageToLoadgdu(ChromeDriver driver)
        {
            while ((driver.FindElementsByXPath("//div[@class='growing_degree_days']/h2")).Count() == 0)
            {
                WaitForPageToLoadgdu(driver);
            }
        }

        public decimal SetCumulativePrecipForLocation(Location model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                using (var driver = new ChromeDriver())
                {
                    driver.Navigate().GoToUrl("https://nutrien-ekonomics.com/tools-to-calculate-fertilizer-needs/calculators/rainfall/");

                    var zipCode = driver.FindElementByName("zipcode");
                    var startMonth = driver.FindElementByName("date_start_mm");
                    var startDay = driver.FindElementByName("date_start_dd");
                    var startYear = driver.FindElementByName("date_start_yyyy");
                    var endMonth = driver.FindElementByName("date_end_mm");
                    var endDay = driver.FindElementByName("date_end_ddd");
                    var endYear = driver.FindElementByName("date_end_yyyy");

                    var calculateButton = driver.FindElementByClassName("btn");

                    zipCode.SendKeys((model.LocationName.ToString() + ", " + model.State).ToString());
                    startMonth.SendKeys((Convert.ToInt32(model.MonthOfPlanting)).ToString());
                    startDay.SendKeys(model.DayOfPlanting.ToString());
                    startYear.SendKeys(model.YearOfPlanting.ToString());
                    endMonth.SendKeys("07");
                    endDay.SendKeys("27");
                    endYear.SendKeys("2019");

                    calculateButton.Click();

                    WaitForPageToLoadprecip(driver);
                    int index = ((driver.FindElementsByXPath("//div[@class='stats']/table/tbody/tr/td[@class='cumulative']")).Count()) - 1;

                    var result = (driver.FindElementsByXPath("//div[@class='stats']/table/tbody/tr/td[@class='cumulative']")[index].Text).ToString();

                    return Decimal.Parse(result);
                }
            }
        }

        public void WaitForPageToLoadprecip(ChromeDriver driver)
        {
            while ((driver.FindElementsByXPath("//div[@class='stats']/table/tbody/tr/td[@class='cumulative']")).Count() == 0)
            {
                WaitForPageToLoadprecip(driver);
            }
        }

        public bool CalculateGrowthStage()
        {
            using (var ctx = new ApplicationDbContext())
            {
                foreach (Location x in ctx.Locations)
                {
                    double daysSincePlanting = Convert.ToDouble(GetDaysSincePlantingForLocation(x));
                    double cumulativePrecip = Convert.ToDouble(x.CumulativePrecip);
                    double averageDailyPrecip = GetAverageDailyPrecipForLocation(cumulativePrecip, daysSincePlanting);

                    double low = 0.09; //These are daily average precipitation ranges - these can be changed to tweak the algorithm.
                    double high = 0.17;

                    int gdu = Convert.ToInt32(x.GDUs);

                    if (daysSincePlanting == 0)
                    {
                        x.GrowthStage = null;
                    }
                    else
                    {
                        if (averageDailyPrecip >= low && averageDailyPrecip < high) //good precip range
                        {
                            if (gdu <= 150)
                            {
                                x.GrowthStage = "V0";
                            }

                            else if (gdu >= 151 && gdu <= 220)
                            {
                                x.GrowthStage = "VE";
                            }

                            else if (gdu >= 221 && gdu <= 288)
                            {
                                x.GrowthStage = "V1";
                            }

                            else if (gdu >= 289 && gdu <= 352)
                            {
                                x.GrowthStage = "V2";
                            }

                            else if (gdu >= 353 && gdu <= 420)
                            {
                                x.GrowthStage = "V3";
                            }

                            else if (gdu >= 421 && gdu <= 484)
                            {
                                x.GrowthStage = "V4";
                            }

                            else if (gdu >= 485 && gdu <= 552)
                            {
                                x.GrowthStage = "V5";
                            }

                            else if (gdu >= 553 && gdu <= 620)
                            {
                                x.GrowthStage = "V6";
                            }

                            else if (gdu >= 621 && gdu <= 684)
                            {
                                x.GrowthStage = "V7";
                            }

                            else if (gdu >= 685 && gdu <= 752)
                            {
                                x.GrowthStage = "V8";
                            }

                            else if (gdu >= 753 && gdu <= 818)
                            {
                                x.GrowthStage = "V9";
                            }

                            else if (gdu >= 819 && gdu <= 884)
                            {
                                x.GrowthStage = "V10";
                            }

                            else if (gdu >= 885 && gdu <= 950)
                            {
                                x.GrowthStage = "V11";
                            }

                            else if (gdu >= 951 && gdu <= 1016)
                            {
                                x.GrowthStage = "V12";
                            }

                            else if (gdu >= 1017 && gdu <= 1082)
                            {
                                x.GrowthStage = "V13";
                            }

                            else if (gdu >= 1083 && gdu <= 1140)
                            {
                                x.GrowthStage = "V14";
                            }

                            else if (gdu >= 1141 && gdu <= 1200)
                            {
                                x.GrowthStage = "V15";
                            }

                            else if (gdu >= 1201 && gdu <= 1600)
                            {
                                x.GrowthStage = "V16";
                            }

                            else if (gdu >= 1601 && gdu <= 1800)
                            {
                                x.GrowthStage = "R1";
                            }

                            else if (gdu >= 1801 && gdu <= 2000)
                            {
                                x.GrowthStage = "R2";
                            }

                            else if (gdu >= 2001 && gdu <= 2300)
                            {
                                x.GrowthStage = "R3";
                            }

                            else if (gdu >= 2301 && gdu <= 2500)
                            {
                                x.GrowthStage = "R4";
                            }

                            else if (gdu >= 2501 && gdu <= 2700)
                            {
                                x.GrowthStage = "R5";
                            }

                            else if (gdu <= 2701)
                            {
                                x.GrowthStage = "R6";
                            }
                        }

                        else //too much precip or too little precip range

                        {
                            if (gdu <= 160)
                            {
                                x.GrowthStage = "V0";
                            }

                            else if (gdu >= 161 && gdu <= 230)
                            {
                                x.GrowthStage = "VE";
                            }

                            else if (gdu >= 231 && gdu <= 298)
                            {
                                x.GrowthStage = "V1";
                            }

                            else if (gdu >= 299 && gdu <= 362)
                            {
                                x.GrowthStage = "V2";
                            }

                            else if (gdu >= 363 && gdu <= 430)
                            {
                                x.GrowthStage = "V3";
                            }

                            else if (gdu >= 431 && gdu <= 494)
                            {
                                x.GrowthStage = "V4";
                            }

                            else if (gdu >= 495 && gdu <= 562)
                            {
                                x.GrowthStage = "V5";
                            }

                            else if (gdu >= 563 && gdu <= 630)
                            {
                                x.GrowthStage = "V6";
                            }

                            else if (gdu >= 631 && gdu <= 694)
                            {
                                x.GrowthStage = "V7";
                            }

                            else if (gdu >= 695 && gdu <= 762)
                            {
                                x.GrowthStage = "V8";
                            }

                            else if (gdu >= 763 && gdu <= 828)
                            {
                                x.GrowthStage = "V9";
                            }

                            else if (gdu >= 829 && gdu <= 894)
                            {
                                x.GrowthStage = "V10";
                            }

                            else if (gdu >= 895 && gdu <= 960)
                            {
                                x.GrowthStage = "V11";
                            }

                            else if (gdu >= 961 && gdu <= 1026)
                            {
                                x.GrowthStage = "V12";
                            }

                            else if (gdu >= 1027 && gdu <= 1092)
                            {
                                x.GrowthStage = "V13";
                            }

                            else if (gdu >= 1093 && gdu <= 1150)
                            {
                                x.GrowthStage = "V14";
                            }

                            else if (gdu >= 1151 && gdu <= 1210)
                            {
                                x.GrowthStage = "V15";
                            }

                            else if (gdu >= 1211 && gdu <= 1610)
                            {
                                x.GrowthStage = "V16";
                            }

                            else if (gdu >= 1611 && gdu <= 1810)
                            {
                                x.GrowthStage = "R1";
                            }

                            else if (gdu >= 1811 && gdu <= 2010)
                            {
                                x.GrowthStage = "R2";
                            }

                            else if (gdu >= 2011 && gdu <= 2310)
                            {
                                x.GrowthStage = "R3";
                            }

                            else if (gdu >= 2311 && gdu <= 2510)
                            {
                                x.GrowthStage = "R4";
                            }

                            else if (gdu >= 2511 && gdu <= 2710)
                            {
                                x.GrowthStage = "R5";
                            }

                            else if (gdu <= 2711)
                            {
                                x.GrowthStage = "R6";
                            }
                        }
                    }
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public int GetDaysSincePlantingForLocation(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (x.DayOfPlanting > 0 && x.YearOfPlanting > 0)
                {
                    DateTimeOffset Today = DateTimeOffset.Now;
                    double daysSincePlanting = (Today - x.DatePlanted).TotalDays;

                    daysSincePlanting = daysSincePlanting - 246;

                    return Convert.ToInt32(daysSincePlanting);
                }
                return 0;
            }
        }

        public double GetAverageDailyPrecipForLocation(double cumulativePrecip, double daysSincePlanting)
        {
            return cumulativePrecip / daysSincePlanting;
        }
    }
}
