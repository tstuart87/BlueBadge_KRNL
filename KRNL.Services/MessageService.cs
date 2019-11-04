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
                MessageId = model.MessageId,
                Comment = model.Comment,
                OwnerId = _userId,
                DateCreated = DateTimeOffset.Now,
                LocationId = model.LocationId
            };

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
                                    Comment = e.Comment,
                                    DateCreated = e.DateCreated,
                                    LocationCode = e.Locations.LocationCode
                                }
                        );

                return query.ToArray();
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
