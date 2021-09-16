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

        public CooperatorService ()
        {

        }

        public bool CreateCooperator(CooperatorCreate model)
        {
            var entity = new Cooperator()
            {
                FirstName = model.FirstName[0].ToString().ToUpper() + model.FirstName.Substring(1).ToLower(),
                LastName = model.LastName.ToUpper(),
                OwnerId = _userId,
                FullName = model.LastName.ToUpper() + ", " + model.FirstName[0].ToString().ToUpper() + model.FirstName.Substring(1).ToLower(),
                Email = model.Email.ToLower(),
                AreaCode = model.AreaCode,
                PhoneFirst = model.PhoneFirst,
                PhoneSecond = model.PhoneSecond,
                ContactType = model.ContactType,
                Phone = "(" + model.AreaCode + ") " + model.PhoneFirst + "-" + model.PhoneSecond
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Cooperators.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CooperatorListItem> GetCooperators(Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Cooperators
                        .Where(e => e.IsDeleted == false && e.OwnerId == userId)
                        .Select(
                            e =>
                                new CooperatorListItem
                                {
                                    CooperatorId = e.CooperatorId,
                                    FirstName = e.FirstName,
                                    LastName = e.LastName,
                                    Phone = e.Phone,
                                    Email = e.Email,
                                    ContactType = e.ContactType,
                                    SearchString = (e.ContactType.ToString() + e.Email.ToString() + e.FullName).ToUpper(),
                                    FullName = e.FullName
                                }
                                );
                return query.ToArray().OrderBy(e => e.FullName);
            }
        }

        public bool UpdateCooperator(CooperatorEdit model, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Cooperators.Single(e => e.CooperatorId == model.CooperatorId && e.OwnerId == userId);

                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.OwnerId = model.OwnerId;
                entity.ContactType = model.ContactType;
                entity.CooperatorId = model.CooperatorId;

                return ctx.SaveChanges() == 1;
            }
        }

        public CooperatorDetail GetCooperatorById(int id, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Cooperators
                        .Single(e => e.CooperatorId == id && e.IsDeleted == false && e.OwnerId == userId);
                return
                    new CooperatorDetail
                    {
                        CooperatorId = entity.CooperatorId,
                        OwnerId = entity.OwnerId,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Phone = entity.Phone,
                        ContactType = entity.ContactType,
                        Email = entity.Email
                    };
            }
        }

        public CooperatorEdit GetCooperatorEditById(int id, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Cooperators
                        .Single(e => e.CooperatorId == id && e.IsDeleted == false && e.OwnerId == userId);
                return
                    new CooperatorEdit
                    {
                        CooperatorId = entity.CooperatorId,
                        OwnerId = entity.OwnerId,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Phone = entity.Phone,
                        ContactType = entity.ContactType,
                        Email = entity.Email
                    };
            }
        }

        public int GetFirstEmployeeId(Guid userId)
        {
            int coopId = 0;

            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Cooperators
                        .Where(e => e.ContactType == contact.Employee && e.IsDeleted == false && e.OwnerId == userId)
                        .FirstOrDefault();

                coopId = entity.CooperatorId;
            }
            return coopId;
        }

        public string GetFullName(int coopId, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Cooperators
                        .Where(e => e.CooperatorId == coopId && e.OwnerId == userId)
                        .FirstOrDefault();

                string FullName = entity.FullName;

                return FullName;
            }
        }

        public bool DeleteCooperator(int cooperatorId, Guid userId)
        {

            var locService = new LocationService();
            var messageService = new MessageService();

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Cooperators.Single(e => e.CooperatorId == cooperatorId && e.OwnerId == userId);
                entity.IsDeleted = true;

                locService.SetCooperatorToNull(entity.CooperatorId);
                messageService.SetEmployeeToNull(entity.CooperatorId, userId);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
