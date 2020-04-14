using System;
using System.Collections;
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
                CRM = model.CRM,
                OwnerId = _userId,
                SearchString = model.LocationName.ToUpper() + ", " + model.LocationCode.ToUpper() + ", " + model.State.ToString().ToUpper()
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
                                    MonthOfPlanting = e.MonthOfPlanting,
                                    DayOfPlanting = e.DayOfPlanting,
                                    YearOfPlanting = e.YearOfPlanting,
                                    SearchString = e.SearchString,
                                    IsHarvested = e.IsHarvested,
                                    MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + e.Latitude + "," + e.Longitude
                                }
                        );

                return query.ToList();
            }
        }



        //public bool UpdateLocationTags()
        //{
        //    return false;
        //}

        //public bool SetIsSelectedToTrue(int id)
        //{
        //    return false;
        //}

        //public bool SetIsSelectedToFalseForAllLocations()
        //{
        //    return false;
        //}

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

                if (model.DayOfPlanting > 0 && model.YearOfPlanting > 1)
                {
                    date = DateTimeOffset.Parse(((((int)model.MonthOfPlanting) + "/" + model.DayOfPlanting + "/" + (model.YearOfPlanting.ToString())).ToString()));
                }

                var tag = "";

                if (model.Tag != null)
                {
                    tag = model.Tag;
                    entity.SearchString = entity.SearchString + ", " + model.Tag.ToUpper();
                }

                entity.DatePlanted = date;
                entity.IsStaked = model.IsStaked;
                entity.CRM = model.CRM;
                entity.IsHarvested = model.IsHarvested;
                entity.CooperatorId = model.CooperatorId;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetLocationIsStakedToYes(int locId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Locations.Single(e => e.LocationId == locId);
                entity.IsStaked = stake.Yes;

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
                            CRM = entity.CRM,
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
                        CRM = entity.CRM,
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
                var messageService = new MessageService();
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
                            IsHarvested = entity.IsHarvested,
                            CRM = entity.CRM,
                            Messages = messageService.GetMessages(entity.LocationId),
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
                        CRM = entity.CRM,
                        CooperatorId = entity.CooperatorId,
                        FullName = entity.Cooperators.FullName,
                        Messages = messageService.GetMessages(entity.LocationId),
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
                    endMonth.SendKeys("08");
                    endDay.SendKeys("01");
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
                    endMonth.SendKeys("08");
                    endDay.SendKeys("01");
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
                        switch (x.CRM)
                        {
                            case crm.CRM85_90:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood85_90(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad85_90(x);
                                    }
                                    break;
                                }
                            case crm.CRM91_95:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood91_95(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad91_95(x);
                                    }
                                    break;
                                }
                            case crm.CRM96_100:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood96_100(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad96_100(x);
                                    }
                                    break;
                                }
                            case crm.CRM101_105:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood101_105(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad101_105(x);
                                    }
                                    break;
                                }
                            case crm.CRM106_110:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood106_110(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad106_110(x);
                                    }
                                    break;
                                }
                            case crm.CRM111_115:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood111_115(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad111_115(x);
                                    }
                                    break;
                                }
                            case crm.CRM116_120:
                                {
                                    if (cumulativePrecip >= low && cumulativePrecip <= high)
                                    {
                                        SetGrowthStageGood116_120(x);
                                    }
                                    else
                                    {
                                        SetGrowthStageBad116_120(x);
                                    }
                                    break;
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

                    daysSincePlanting = daysSincePlanting - 250; //Just for testing purposes delete this when this is working live. 4/7/2019 = 250

                    return Convert.ToInt32(daysSincePlanting);
                }
                return 0;
            }
        }

        public double GetAverageDailyPrecipForLocation(double cumulativePrecip, double daysSincePlanting)
        {
            return cumulativePrecip / daysSincePlanting;
        }

        public bool SetGrowthStageGood85_90(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 250 && gdu <= 345)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 346 && gdu <= 469)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 470 && gdu <= 569)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 570 && gdu <= 630)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 631 && gdu <= 690)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 691 && gdu <= 764)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 765 && gdu <= 808)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 809 && gdu <= 852)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 853 && gdu <= 896)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 897 && gdu <= 940)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 941 && gdu <= 984)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 985 && gdu <= 1028)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1029 && gdu <= 1074)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1075 && gdu <= 1294)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1295 && gdu <= 1469)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1470 && gdu <= 1579)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1580 && gdu <= 1774)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 1775 && gdu <= 1959)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 1960 && gdu <= 2144)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2145)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad85_90(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 365)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 366 && gdu <= 489)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 490 && gdu <= 589)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 590 && gdu <= 650)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 651 && gdu <= 710)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 711 && gdu <= 784)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 784 && gdu <= 828)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 829 && gdu <= 872)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 873 && gdu <= 916)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 917 && gdu <= 960)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 961 && gdu <= 1004)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1005 && gdu <= 1048)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1049 && gdu <= 1094)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1095 && gdu <= 1314)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1315 && gdu <= 1489)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1490 && gdu <= 1599)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1600 && gdu <= 1794)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 1795 && gdu <= 1979)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 1980 && gdu <= 2164)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2165)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageGood91_95(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 251 && gdu <= 350)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 351 && gdu <= 482)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 483 && gdu <= 582)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 583 && gdu <= 643)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 644 && gdu <= 712)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 713 && gdu <= 782)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 783 && gdu <= 830)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 831 && gdu <= 878)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 879 && gdu <= 926)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 927 && gdu <= 974)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 975 && gdu <= 1022)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1023 && gdu <= 1070)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1071 && gdu <= 1124)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1125 && gdu <= 1367)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1368 && gdu <= 1552)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1553 && gdu <= 1669)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1670 && gdu <= 1877)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 1878 && gdu <= 2072)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2073 && gdu <= 2267)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2268)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad91_95(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 370)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 371 && gdu <= 502)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 503 && gdu <= 602)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 603 && gdu <= 663)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 664 && gdu <= 732)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 733 && gdu <= 802)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 803 && gdu <= 850)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 851 && gdu <= 898)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 899 && gdu <= 946)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 947 && gdu <= 994)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 995 && gdu <= 1042)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1043 && gdu <= 1090)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1091 && gdu <= 1144)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1145 && gdu <= 1387)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1388 && gdu <= 1572)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1573 && gdu <= 1689)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1690 && gdu <= 1897)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 1898 && gdu <= 2092)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2093 && gdu <= 2287)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2288)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageGood96_100(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 251 && gdu <= 355)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 356 && gdu <= 494)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 495 && gdu <= 594)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 595 && gdu <= 655)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 656 && gdu <= 727)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 728 && gdu <= 799)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 800 && gdu <= 852)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 853 && gdu <= 905)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 906 && gdu <= 958)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 959 && gdu <= 1011)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1012 && gdu <= 1064)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1065 && gdu <= 1117)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1118 && gdu <= 1174)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1175 && gdu <= 1439)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1440 && gdu <= 1634)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1635 && gdu <= 1759)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1760 && gdu <= 1979)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 1980 && gdu <= 2184)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2185 && gdu <= 2389)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2390)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad96_100(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 375)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 376 && gdu <= 514)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 515 && gdu <= 614)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 615 && gdu <= 675)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 676 && gdu <= 747)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 748 && gdu <= 819)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 820 && gdu <= 872)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 873 && gdu <= 925)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 926 && gdu <= 978)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 979 && gdu <= 1031)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1032 && gdu <= 1084)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1085 && gdu <= 1137)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1138 && gdu <= 1194)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1195 && gdu <= 1459)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1460 && gdu <= 1654)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1655 && gdu <= 1779)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1780 && gdu <= 1999)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2000 && gdu <= 2204)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2205 && gdu <= 2409)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2410)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageGood101_105(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 251 && gdu <= 360)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 361 && gdu <= 507)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 508 && gdu <= 607)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 608 && gdu <= 668)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 669 && gdu <= 742)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 743 && gdu <= 817)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 818 && gdu <= 875)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 876 && gdu <= 933)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 934 && gdu <= 991)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 992 && gdu <= 1049)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1050 && gdu <= 1107)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1108 && gdu <= 1165)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1166 && gdu <= 1227)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1228 && gdu <= 1512)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1513 && gdu <= 1717)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1718 && gdu <= 1849)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1850 && gdu <= 2079)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2080 && gdu <= 2297)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2298 && gdu <= 2514)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2515)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad101_105(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 380)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 381 && gdu <= 527)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 528 && gdu <= 627)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 628 && gdu <= 688)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 689 && gdu <= 762)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 763 && gdu <= 837)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 838 && gdu <= 895)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 896 && gdu <= 953)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 954 && gdu <= 1011)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1012 && gdu <= 1069)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1070 && gdu <= 1127)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1128 && gdu <= 1185)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1186 && gdu <= 1247)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1248 && gdu <= 1532)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1533 && gdu <= 1737)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1738 && gdu <= 1869)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1870 && gdu <= 2099)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2100 && gdu <= 2317)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2318 && gdu <= 2534)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2535)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageGood106_110(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 251 && gdu <= 365)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 366 && gdu <= 519)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 520 && gdu <= 619)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 620 && gdu <= 680)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 681 && gdu <= 757)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 758 && gdu <= 834)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 835 && gdu <= 897)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 898 && gdu <= 960)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 961 && gdu <= 1023)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1024 && gdu <= 1086)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1087 && gdu <= 1149)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1150 && gdu <= 1212)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1213 && gdu <= 1279)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1280 && gdu <= 1584)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1585 && gdu <= 1799)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1800 && gdu <= 1939)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1940 && gdu <= 2179)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2180 && gdu <= 2409)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2410 && gdu <= 2639)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2640)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad106_110(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 385)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 386 && gdu <= 539)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 540 && gdu <= 639)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 640 && gdu <= 700)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 701 && gdu <= 777)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 778 && gdu <= 854)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 855 && gdu <= 917)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 918 && gdu <= 980)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 981 && gdu <= 1043)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1044 && gdu <= 1106)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1107 && gdu <= 1169)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1170 && gdu <= 1232)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1233 && gdu <= 1299)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1300 && gdu <= 1604)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1605 && gdu <= 1819)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1820 && gdu <= 1959)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 1960 && gdu <= 2199)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2200 && gdu <= 2429)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2430 && gdu <= 2659)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2660)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageGood111_115(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 251 && gdu <= 370)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 371 && gdu <= 524)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 525 && gdu <= 624)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 625 && gdu <= 685)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 686 && gdu <= 761)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 762 && gdu <= 837)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 838 && gdu <= 910)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 911 && gdu <= 983)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 984 && gdu <= 1056)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1057 && gdu <= 1129)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1130 && gdu <= 1202)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1203 && gdu <= 1275)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1276 && gdu <= 1349)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1350 && gdu <= 1654)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1655 && gdu <= 1882)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1883 && gdu <= 2029)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 2030 && gdu <= 2282)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2283 && gdu <= 2524)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2525 && gdu <= 2764)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2765)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad111_115(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 390)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 391 && gdu <= 544)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 545 && gdu <= 644)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 645 && gdu <= 705)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 706 && gdu <= 781)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 782 && gdu <= 857)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 858 && gdu <= 930)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 931 && gdu <= 1003)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 1004 && gdu <= 1076)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1077 && gdu <= 1149)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1150 && gdu <= 1222)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1223 && gdu <= 1295)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1296 && gdu <= 1369)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1370 && gdu <= 1674)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1675 && gdu <= 1902)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1903 && gdu <= 2049)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 2050 && gdu <= 2302)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2303 && gdu <= 2544)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2545 && gdu <= 2784)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2785)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageGood116_120(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 100)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 101 && gdu <= 120)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 121 && gdu <= 200)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 201 && gdu <= 250)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 251 && gdu <= 375)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 376 && gdu <= 529)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 530 && gdu <= 629)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 630 && gdu <= 690)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 691 && gdu <= 764)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 765 && gdu <= 839)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 840 && gdu <= 921)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 922 && gdu <= 1003)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 1004 && gdu <= 1085)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1086 && gdu <= 1167)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1168 && gdu <= 1249)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1250 && gdu <= 1331)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1332 && gdu <= 1419)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1420 && gdu <= 1724)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1725 && gdu <= 1964)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1965 && gdu <= 2119)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 2120 && gdu <= 2384)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2385 && gdu <= 2639)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2640 && gdu <= 2889)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2890)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetGrowthStageBad116_120(Location x)
        {
            using (var ctx = new ApplicationDbContext())
            {
                int gdu = Convert.ToInt32(x.GDUs);

                if (gdu <= 120)
                {
                    x.GrowthStage = "V0";
                }

                else if (gdu >= 121 && gdu <= 140)
                {
                    x.GrowthStage = "VE";
                }

                else if (gdu >= 141 && gdu <= 220)
                {
                    x.GrowthStage = "V1";
                }

                else if (gdu >= 221 && gdu <= 270)
                {
                    x.GrowthStage = "V2";
                }

                else if (gdu >= 271 && gdu <= 395)
                {
                    x.GrowthStage = "V3";
                }

                else if (gdu >= 396 && gdu <= 549)
                {
                    x.GrowthStage = "V4";
                }

                else if (gdu >= 550 && gdu <= 649)
                {
                    x.GrowthStage = "V5";
                }

                else if (gdu >= 650 && gdu <= 710)
                {
                    x.GrowthStage = "V6";
                }

                else if (gdu >= 711 && gdu <= 784)
                {
                    x.GrowthStage = "V7";
                }

                else if (gdu >= 785 && gdu <= 859)
                {
                    x.GrowthStage = "V8";
                }

                else if (gdu >= 860 && gdu <= 941)
                {
                    x.GrowthStage = "V9";
                }

                else if (gdu >= 942 && gdu <= 1023)
                {
                    x.GrowthStage = "V10";
                }

                else if (gdu >= 1024 && gdu <= 1105)
                {
                    x.GrowthStage = "V11";
                }

                else if (gdu >= 1106 && gdu <= 1187)
                {
                    x.GrowthStage = "V12";
                }

                else if (gdu >= 1188 && gdu <= 1269)
                {
                    x.GrowthStage = "V13";
                }

                else if (gdu >= 1270 && gdu <= 1351)
                {
                    x.GrowthStage = "V14";
                }

                else if (gdu >= 1352 && gdu <= 1439)
                {
                    x.GrowthStage = "V15";
                }

                else if (gdu >= 1440 && gdu <= 1744)
                {
                    x.GrowthStage = "VT";
                }

                else if (gdu >= 1745 && gdu <= 1984)
                {
                    x.GrowthStage = "R1";
                }

                else if (gdu >= 1985 && gdu <= 2139)
                {
                    x.GrowthStage = "R2";
                }

                else if (gdu >= 2140 && gdu <= 2404)
                {
                    x.GrowthStage = "R3";
                }

                else if (gdu >= 2405 && gdu <= 2659)
                {
                    x.GrowthStage = "R4";
                }

                else if (gdu >= 2660 && gdu <= 2909)
                {
                    x.GrowthStage = "R5";
                }

                else if (gdu >= 2910)
                {
                    x.GrowthStage = "R6";
                }
                return ctx.SaveChanges() == 1;
            }
        }
    }
}

