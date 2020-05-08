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
    public class MessageService
    {
        private readonly Guid _userId;

        public MessageService(Guid userId)
        {
            _userId = userId;
        }

        public MessageService ()
        {

        }

        public bool CreateMessage(MessageCreate model, int? docId)
        {
            var locationService = new LocationService();

            var entity = new Message()
            {
                Comment = model.Comment,
                OwnerId = _userId,
                DateCreated = DateTimeOffset.Now,
                LocationId = model.LocationId,
                HumanGrowthStage = model.HumanGrowthStage,
                IsRequest = noYes.No,
                JobOne = model.JobOne,
                JobTwo = model.JobTwo,
                JobThree = model.JobThree,
                CooperatorId = model.CooperatorId,
                FullName = model.FullName,
                DocumentId = docId,
                Rating = model.Rating
                //LocationCode = model.LocationCode,
                //PredictedGrowthStage = model.PredictedGrowthStage
            };

            if (model.Rating != rating.NoRating)
            {
                locationService.SetLocationRating(model.LocationId, model.Rating);
            }

            if (model.JobOne == job.Planting || model.JobTwo == job.Planting || model.JobThree == job.Planting)
            {
                locationService.SetLocationIsPlantedToYes(model.LocationId);
            }

            if (model.JobOne == job.Staking || model.JobTwo == job.Staking || model.JobThree == job.Staking)
            {
                locationService.SetLocationIsStakedToYes(model.LocationId);
            }

            if (model.JobOne == job.Rowbanding || model.JobTwo == job.Rowbanding || model.JobThree == job.Rowbanding)
            {
                locationService.SetLocationIsRowbandedToYes(model.LocationId);
            }

            if (model.JobOne == job.Harvesting || model.JobTwo == job.Harvesting || model.JobThree == job.Harvesting)
            {
                locationService.SetLocationIsHarvestedToYes(model.LocationId);
            }

            locationService.SetLastVisitor(model.LocationId, model.FullName);

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Messages.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool CreateRequest(MessageCreate model)
        {
            var coopService = new CooperatorService();
            var locService = new LocationService();

            var entity = new Message()
            {
                OwnerId = _userId,
                DateCreated = DateTimeOffset.Now,
                LocationId = model.LocationId,
                HumanGrowthStage = model.HumanGrowthStage,
                IsRequest = noYes.Yes,
                JobOne = model.JobOne,
                JobTwo = model.JobTwo,
                JobThree = model.JobThree,
                CooperatorId = model.CooperatorId,
                FullName = model.FullName,
                Rating = model.Rating,
                Comment = "Requested by: " + coopService.GetFullName(model.CooperatorId, _userId) + " - " + DateTimeOffset.Now.Month.ToString() + "/" + DateTimeOffset.Now.Day.ToString() + "/" + DateTimeOffset.Now.Year.ToString() + " " + " " + " " + model.Comment

                //LocationCode = model.LocationCode,
                //PredictedGrowthStage = model.PredictedGrowthStage
            };

            if (model.Rating != rating.NoRating)
            {
                locService.SetLocationRating(model.LocationId, model.Rating);
            }

            locService.SetLastVisitor(model.LocationId, model.FullName);
            locService.AddOneToRequestCount(model.LocationId);

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Messages.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<MessageListItem> GetMessages(Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Messages
                        .Where(e => e.IsDeleted == noYes.No && e.OwnerId == userId)
                        .Select(
                            e =>
                                new MessageListItem
                                {
                                    MessageId = e.MessageId,
                                    OwnerId = e.OwnerId,
                                    LocationId = e.LocationId,
                                    LocationCode = e.Locations.LocationCode,
                                    DateCreated = e.DateCreated,
                                    PredictedGrowthStage = e.Locations.GrowthStage,
                                    HumanGrowthStage = e.HumanGrowthStage,
                                    CooperatorId = e.CooperatorId,
                                    FullName = e.Cooperators.FullName,
                                    JobOne = e.JobOne,
                                    JobTwo = e.JobTwo,
                                    JobThree = e.JobThree,
                                    Rating = e.Rating,
                                    IsRequest = e.IsRequest,
                                    DocString = e.Documents.DocString,
                                    SearchString = (e.JobOne.ToString() + e.JobTwo.ToString() + e.JobThree.ToString() + e.Cooperators.FullName + e.Locations.LocationCode + e.Locations.LocationCode + e.HumanGrowthStage.ToString()).ToUpper(),
                                    Comment = e.Comment
                                }
                        );
                return query.ToArray().OrderByDescending(e => e.DateCreated);
            }
        }

        public IEnumerable<MessageListItem> GetMessages(int locId, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Messages
                        .Where(e => e.LocationId == locId && e.IsDeleted == noYes.No && e.OwnerId == userId)
                        .Select(
                            e =>
                                new MessageListItem
                                {
                                    MessageId = e.MessageId,
                                    OwnerId = e.OwnerId,
                                    LocationId = e.LocationId,
                                    LocationCode = e.Locations.LocationCode,
                                    DateCreated = e.DateCreated,
                                    PredictedGrowthStage = e.Locations.GrowthStage,
                                    HumanGrowthStage = e.HumanGrowthStage,
                                    Rating = e.Rating,
                                    CooperatorId = e.CooperatorId,
                                    FullName = e.Cooperators.FullName,
                                    JobOne = e.JobOne,
                                    JobTwo = e.JobTwo,
                                    JobThree = e.JobThree,
                                    IsRequest = e.IsRequest,
                                    DocString = e.Documents.DocString,
                                    Comment = e.Comment
                                }
                        );
                return query.ToArray().OrderByDescending(e => e.DateCreated);
            }
        }

        public bool UpdateMessage(MessageEdit model, Guid userId)
        {
            var locationService = new LocationService();

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Messages.Single(e => e.MessageId == model.MessageId && e.OwnerId == userId);

                entity.MessageId = model.MessageId;
                entity.Comment = model.Comment;
                entity.MessageId = model.MessageId;
                entity.LocationId = model.LocationId;
                entity.Comment = model.Comment;
                entity.CooperatorId = model.CooperatorId;
                entity.DateCreated = model.DateCreated;
                entity.OwnerId = model.OwnerId;
                entity.JobOne = model.JobOne;
                entity.JobTwo = model.JobTwo;
                entity.JobThree = model.JobThree;
                entity.HumanGrowthStage = model.HumanGrowthStage;
                entity.Rating = model.Rating;
                entity.IsRequest = model.IsRequest;

                if (model.Rating != rating.NoRating)
                {
                    locationService.SetLocationRating(model.LocationId, model.Rating);
                }

                if (model.JobOne == job.Planting || model.JobTwo == job.Planting || model.JobThree == job.Planting)
                {
                    locationService.EditLocationIsPlantedToYes(model.LocationId, entity.DateCreated);
                }

                if (model.JobOne == job.Staking || model.JobTwo == job.Staking || model.JobThree == job.Staking)
                {
                    locationService.SetLocationIsStakedToYes(model.LocationId);
                }

                if (model.JobOne == job.Rowbanding || model.JobTwo == job.Rowbanding || model.JobThree == job.Rowbanding)
                {
                    locationService.SetLocationIsRowbandedToYes(model.LocationId);
                }

                if (model.JobOne == job.Harvesting || model.JobTwo == job.Harvesting || model.JobThree == job.Harvesting)
                {
                    locationService.EditLocationIsHarvestedToYes(model.LocationId, entity.DateCreated);
                }

                locationService.SetLastVisitor(model.LocationId, entity.FullName);

                return ctx.SaveChanges() == 1;
            }
        }

        public MessageDetail GetMessageById(int id, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Messages
                        .Single(e => e.MessageId == id && e.OwnerId == userId);
                return
                    new MessageDetail
                    {
                        MessageId = entity.MessageId,
                        OwnerId = entity.OwnerId,
                        Comment = entity.Comment,
                        CooperatorId = entity.CooperatorId,
                        FullName = entity.Cooperators.FullName,
                        DateCreated = entity.DateCreated,
                        LocationId = entity.LocationId,
                        LocationCode = entity.Locations.LocationCode,
                        IsDeleted = entity.IsDeleted,
                        JobOne = entity.JobOne,
                        JobTwo = entity.JobTwo,
                        JobThree = entity.JobThree,
                        IsRequest = entity.IsRequest,
                        Rating = entity.Rating
                    };
            }
        }

        public MessageEdit GetMessageEditById(int id, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Messages
                        .Single(e => e.MessageId == id && e.OwnerId == userId);

                if (entity.CooperatorId == null)
                {
                    return
                        new MessageEdit
                        {
                            MessageId = entity.MessageId,
                            Comment = entity.Comment,
                            OwnerId = entity.OwnerId,
                            DateCreated = entity.DateCreated,
                            LocationId = entity.LocationId,
                            LocationCode = entity.Locations.LocationCode,
                            HumanGrowthStage = entity.HumanGrowthStage,
                            JobOne = entity.JobOne,
                            JobTwo = entity.JobTwo,
                            JobThree = entity.JobThree,
                            CooperatorId = entity.CooperatorId,
                            Rating = entity.Rating,
                            IsRequest = entity.IsRequest
                        };
                }
                else
                {
                    return
                    new MessageEdit
                    {
                        MessageId = entity.MessageId,
                        Comment = entity.Comment,
                        FullName = entity.Cooperators.FullName,
                        OwnerId = entity.OwnerId,
                        DateCreated = entity.DateCreated,
                        LocationId = entity.LocationId,
                        LocationCode = entity.Locations.LocationCode,
                        HumanGrowthStage = entity.HumanGrowthStage,
                        JobOne = entity.JobOne,
                        JobTwo = entity.JobTwo,
                        JobThree = entity.JobThree,
                        CooperatorId = entity.CooperatorId,
                        Rating = entity.Rating,
                        IsRequest = entity.IsRequest
                    };
                };
            }
        }

        public bool SetIsRequestToNo(int messageId, int locId, DateTimeOffset? dateCreated, Guid userId)
        {
            var locService = new LocationService();

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Messages.Single(e => e.MessageId == messageId && e.OwnerId == userId);
                entity.IsRequest = noYes.No;
                entity.DateCreated = DateTime.Now;

                if (entity.JobOne == job.Planting || entity.JobTwo == job.Planting || entity.JobThree == job.Planting)
                {
                    var locationService = new LocationService();
                    locationService.EditLocationIsPlantedToYes(locId, dateCreated);
                }

                if (entity.JobOne == job.Staking || entity.JobTwo == job.Staking || entity.JobThree == job.Staking)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsStakedToYes(locId);
                }

                if (entity.JobOne == job.Rowbanding || entity.JobTwo == job.Rowbanding || entity.JobThree == job.Rowbanding)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsRowbandedToYes(locId);
                }

                if (entity.JobOne == job.Harvesting || entity.JobTwo == job.Harvesting || entity.JobThree == job.Harvesting)
                {
                    var locationService = new LocationService();
                    locationService.EditLocationIsHarvestedToYes(locId, dateCreated);
                }

                locService.SubtractOneFromRequestCount(entity.LocationId);

                return ctx.SaveChanges() == 1;
            }
        }

        public bool SetEmployeeToNull(int coopId, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                foreach (Message x in ctx.Messages.Where(e => e.CooperatorId == coopId && e.OwnerId == userId))
                {
                    x.CooperatorId = null;
                }

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteMessage(int messageId, Guid userId)
        {
            var locationService = new LocationService();

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Messages.Single(e => e.MessageId == messageId && e.OwnerId == userId);
                entity.IsDeleted = noYes.Yes;

                //if (entity.Rating != rating.NoRating)                    
                //{
                //    var locationService = new LocationService();
                //    locationService.SetLocationRating(entity.LocationId);
                //}

                if (entity.JobOne == job.Planting || entity.JobTwo == job.Planting || entity.JobThree == job.Planting)
                {
                    locationService.SetLocationIsPlantedToNo(entity.LocationId);
                }

                if (entity.JobOne == job.Staking || entity.JobTwo == job.Staking || entity.JobThree == job.Staking)
                {
                    locationService.SetLocationIsStakedToNo(entity.LocationId);
                }

                if (entity.JobOne == job.Rowbanding || entity.JobTwo == job.Rowbanding || entity.JobThree == job.Rowbanding)
                {
                    locationService.SetLocationIsRowbandedToNo(entity.LocationId);
                }

                if (entity.JobOne == job.Harvesting || entity.JobTwo == job.Harvesting || entity.JobThree == job.Harvesting)
                {
                    locationService.SetLocationIsHarvestedToNo(entity.LocationId);
                }

                if (entity.IsRequest == noYes.Yes)
                {
                    locationService.SubtractOneFromRequestCount(entity.LocationId);
                }

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
