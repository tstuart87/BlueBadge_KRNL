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

        public MessageService()
        {

        }

        public bool CreateMessage(MessageCreate model)
        {
            var entity = new Message()
            {
                Comment = model.Comment,
                OwnerId = _userId,
                DateCreated = DateTimeOffset.Now,
                LocationId = model.LocationId,
                HumanGrowthStage = model.HumanGrowthStage,
                JobOne = model.JobOne,
                JobTwo = model.JobTwo,
                JobThree = model.JobThree,
                CooperatorId = model.CooperatorId,
                FullName = model.FullName,
                Rating = model.Rating

                //LocationCode = model.LocationCode,
                //PredictedGrowthStage = model.PredictedGrowthStage
            };

            if (model.Rating != rating.NoRating)
            {
                var locationService = new LocationService();
                locationService.SetLocationRating(model.LocationId, model.Rating);
            }

            if (model.JobOne == job.Planting || model.JobTwo == job.Planting || model.JobThree == job.Planting)
            {
                var locationService = new LocationService();
                locationService.SetLocationIsPlantedToYes(model.LocationId);
            }

            if (model.JobOne == job.Staking || model.JobTwo == job.Staking || model.JobThree == job.Staking)
            {
                var locationService = new LocationService();
                locationService.SetLocationIsStakedToYes(model.LocationId);
            }

            if (model.JobOne == job.Rowbanding || model.JobTwo == job.Rowbanding || model.JobThree == job.Rowbanding)
            {
                var locationService = new LocationService();
                locationService.SetLocationIsRowbandedToYes(model.LocationId);
            }

            if (model.JobOne == job.Harvesting || model.JobTwo == job.Harvesting || model.JobThree == job.Harvesting)
            {
                var locationService = new LocationService();
                locationService.SetLocationIsHarvestedToYes(model.LocationId);
            }

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Messages.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<MessageListItem> GetMessages()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Messages
                        .Where(e => e.IsDeleted == noYes.No)
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
                                    Comment = e.Comment
                                }
                        );
                return query.ToArray();
            }
        }

        public IEnumerable<MessageListItem> GetMessages(int locID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Messages
                        .Where(e => e.LocationId == locID && e.IsDeleted == noYes.No)
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
                                    Comment = e.Comment
                                }
                        );
                return query.ToArray().OrderByDescending(e => e.DateCreated);
            }
        }

        public bool UpdateMessage(MessageEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Messages.Single(e => e.MessageId == model.MessageId);

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

                if (model.Rating != rating.NoRating)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationRating(model.LocationId, model.Rating);
                }

                if (model.JobOne == job.Planting || model.JobTwo == job.Planting || model.JobThree == job.Planting)
                {
                    var locationService = new LocationService();
                    locationService.EditLocationIsPlantedToYes(model.LocationId, entity.DateCreated);
                }

                if (model.JobOne == job.Staking || model.JobTwo == job.Staking || model.JobThree == job.Staking)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsStakedToYes(model.LocationId);
                }

                if (model.JobOne == job.Rowbanding || model.JobTwo == job.Rowbanding || model.JobThree == job.Rowbanding)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsRowbandedToYes(model.LocationId);
                }

                if (model.JobOne == job.Harvesting || model.JobTwo == job.Harvesting || model.JobThree == job.Harvesting)
                {
                    var locationService = new LocationService();
                    locationService.EditLocationIsHarvestedToYes(model.LocationId, entity.DateCreated);
                }

                return ctx.SaveChanges() == 1;
            }
        }

        public MessageDetail GetMessageById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Messages
                        .Single(e => e.MessageId == id);
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
                        Rating = entity.Rating
                    };
            }
        }

        public MessageEdit GetMessageEditById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Messages
                        .Single(e => e.MessageId == id);

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
                            HumanGrowthStage = entity.HumanGrowthStage,
                            JobOne = entity.JobOne,
                            JobTwo = entity.JobTwo,
                            JobThree = entity.JobThree,
                            CooperatorId = entity.CooperatorId,
                            Rating = entity.Rating
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
                        HumanGrowthStage = entity.HumanGrowthStage,
                        JobOne = entity.JobOne,
                        JobTwo = entity.JobTwo,
                        JobThree = entity.JobThree,
                        CooperatorId = entity.CooperatorId,
                        Rating = entity.Rating
                    };
                };
            }
        }

        public bool SetEmployeeToNull(int coopId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                foreach (Message x in ctx.Messages.Where(e => e.CooperatorId == coopId))
                {
                    x.CooperatorId = null;
                }

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteMessage(int messageId)
        {
            var locService = new LocationService();

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Messages.Single(e => e.MessageId == messageId);
                entity.IsDeleted = noYes.Yes;

                //if (entity.Rating != rating.NoRating)                    
                //{
                //    var locationService = new LocationService();
                //    locationService.SetLocationRating(entity.LocationId);
                //}

                if (entity.JobOne == job.Planting || entity.JobTwo == job.Planting || entity.JobThree == job.Planting)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsPlantedToNo(entity.LocationId);
                }

                if (entity.JobOne == job.Staking || entity.JobTwo == job.Staking || entity.JobThree == job.Staking)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsStakedToNo(entity.LocationId);
                }

                if (entity.JobOne == job.Rowbanding || entity.JobTwo == job.Rowbanding || entity.JobThree == job.Rowbanding)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsRowbandedToNo(entity.LocationId);
                }

                if (entity.JobOne == job.Harvesting || entity.JobTwo == job.Harvesting || entity.JobThree == job.Harvesting)
                {
                    var locationService = new LocationService();
                    locationService.SetLocationIsHarvestedToNo(entity.LocationId);
                }

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
