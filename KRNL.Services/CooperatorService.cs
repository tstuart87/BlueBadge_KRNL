using KRNL.Data;
using KRNL.Models;
using KRNL.WebMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Services
{
    public class CooperatorService
    {
        private readonly Guid _userId;

        public CooperatorService(Guid userId)
        {
            _userId = userId;
        }

        public CooperatorService()
        {

        }

        public bool CreateCooperator(CooperatorCreate model)
        {
            var entity = new Cooperator()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                OwnerId = _userId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Cooperators.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CooperatorListItem> GetCooperators()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Cooperators
                        .Select(
                            e =>
                                new CooperatorListItem
                                {
                                    CooperatorId = e.CooperatorId,
                                    FirstName = e.FirstName,
                                    LastName = e.LastName,
                                    Phone = e.Phone,
                                    Email = e.Email,
                                    City = e.City,
                                    State = e.State,
                                    Zip = e.Zip,
                                    FullName = e.FirstName + " " + e.LastName
                                }
                                );
                return query.ToArray();
            }
        }

        public bool UpdateCooperator(CooperatorEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Cooperators.Single(e => e.CooperatorId == model.CooperatorId);

                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.StreetAddress = model.StreetAddress;
                entity.City = model.City;
                entity.State = model.State;
                entity.Zip = model.Zip;
                entity.OwnerId = model.OwnerId;
                entity.CooperatorId = model.CooperatorId;

                return ctx.SaveChanges() == 1;
            }
        }

        public CooperatorDetail GetCooperatorById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Cooperators
                        .Single(e => e.CooperatorId == id);
                return
                    new CooperatorDetail
                    {
                        CooperatorId = entity.CooperatorId,
                        OwnerId = entity.OwnerId,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Phone = entity.Phone,
                        Email = entity.Email,
                        StreetAddress = entity.StreetAddress,
                        City = entity.City,
                        State = entity.State,
                        Zip = entity.Zip
                    };
            }
        }

        public CooperatorEdit GetCooperatorEditById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Cooperators
                        .Single(e => e.CooperatorId == id);
                return
                    new CooperatorEdit
                    {
                        CooperatorId = entity.CooperatorId,
                        OwnerId = entity.OwnerId,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Phone = entity.Phone,
                        Email = entity.Email,
                        StreetAddress = entity.StreetAddress,
                        City = entity.City,
                        State = entity.State,
                        Zip = entity.Zip
                    };
            }
        }

        public bool DeleteCooperator(int cooperatorId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                       .Cooperators
                       .Single(e => e.CooperatorId == cooperatorId);

                ctx.Cooperators.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
