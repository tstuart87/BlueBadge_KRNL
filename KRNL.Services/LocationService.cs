using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRNL.Data;
using KRNL.Models;
using KRNL.WebMVC.Data;

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
                Zip = model.Zip,
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
                                    LocationCode = e.LocationCode,
                                    IsStaked = e.IsStaked,
                                    GDUs = e.GDUs,
                                    Latitude = e.Latitude,
                                    Longitude = e.Longitude,
                                    Zip = e.Zip,
                                    MapLink = "https://www.google.com/maps/dir/?api=1&destination="+e.Latitude+","+e.Longitude
                                }
                        );

                return query.ToArray();
            }
        }

        public bool UpdateLocation(LocationEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Locations.Single(e => e.LocationId == model.LocationId);

                entity.LocationName = model.LocationName;
                entity.LocationCode = model.LocationCode;
                entity.Longitude = model.Longitude;
                entity.Latitude = model.Latitude;
                entity.NumberOfPlots = model.NumberOfPlots;
                entity.Length = model.Length;
                entity.Width = model.Width;
                entity.DatePlanted = model.DatePlanted;
                entity.IsStaked = model.IsStaked;
                entity.Year = model.Year;
                entity.CooperatorId = model.CooperatorId;
                entity.Zip = model.Zip;


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

                if(entity.CooperatorId != null)
                {
                    return
                        new LocationDetail
                        {
                            LocationId = entity.LocationId,
                            LocationName = entity.LocationName,
                            LocationCode = entity.LocationCode,
                            GDUs = entity.GDUs,
                            DatePlanted = entity.DatePlanted,
                            IsStaked = entity.IsStaked,
                            CooperatorId = entity.CooperatorId,
                            FullName = entity.Cooperators.FirstName + " " + entity.Cooperators.LastName,
                            Zip = entity.Zip,
                            MapLink = "https://www.google.com/maps/dir/?api=1&destination=" + entity.Latitude + "," + entity.Longitude
                        };
                }

                return
                    new LocationDetail
                    {
                        LocationId = entity.LocationId,
                        LocationName = entity.LocationName,
                        LocationCode = entity.LocationCode,
                        GDUs = entity.GDUs,
                        DatePlanted = entity.DatePlanted,
                        IsStaked = entity.IsStaked,
                        CooperatorId = entity.CooperatorId,
                        FullName = null,
                        Zip = entity.Zip,
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

            return
                new LocationEdit
                {
                    LocationId = entity.LocationId,
                    LocationName = entity.LocationName,
                    LocationCode = entity.LocationCode,
                    Longitude = entity.Longitude,
                    Latitude = entity.Latitude,
                    Length = entity.Length,
                    Width = entity.Width,
                    DatePlanted = entity.DatePlanted,
                    IsStaked = entity.IsStaked,
                    Zip = entity.Zip
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
}
}
