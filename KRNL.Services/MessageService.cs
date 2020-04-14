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

            if (model.JobOne == job.Staking || model.JobTwo == job.Staking || model.JobThree == job.Staking)
            {
                var locationService = new LocationService();
                locationService.SetLocationIsStakedToYes(model.LocationId);
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
                        .Where(e => e.LocationId == locID)
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
                        Comment = entity.Comment,
                        DateCreated = entity.DateCreated,
                        LocationCode = entity.Locations.LocationCode
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
                return
                    new MessageEdit
                    {
                        MessageId = entity.MessageId,
                        Comment = entity.Comment,
                    };
            }
        }

        public bool DeleteMessage(int messageId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Messages
                        .Single(e => e.MessageId == messageId);

                ctx.Messages.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
