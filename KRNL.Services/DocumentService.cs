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
    public class DocumentService
    {
        private readonly Guid _userId;

        public DocumentService(Guid userId)
        {
            _userId = userId;
        }

        public DocumentService()
        {

        }

        public bool CreateDocument(DocumentCreate model, string pathString, string fileName)
        {
            string docString = "/Content/docs/";
            string fullFileName = fileName;

            if (fileName.Length > 15)
            {
                fileName = fileName.Substring(0, 10) + "..." + fileName.Substring(fileName.Length - 4, 4);
            }

            var entity = new Document()
            {
                OwnerId = _userId,
                DocName = fileName,
                DocString = docString + fullFileName,
                DocType = model.DocType,
                DateCreated = DateTimeOffset.Now,
                LocationId = model.LocationId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Documents.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public int CreateDocumentFromJob(MessageCreate model, string fileName)
        {
            new DocumentService();

            string docString = "/Content/docs/";
            string fullFileName = fileName;
            fileName = fileName.Substring(fileName.Length - 8, 8);

            var entity = new Document()
            {
                OwnerId = _userId,
                DocName = fileName,
                DocString = docString + fullFileName,
                DocType = docType.Image,
                DateCreated = DateTimeOffset.Now,
                LocationId = model.LocationId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Documents.Add(entity);
                ctx.SaveChanges();
            }

            return entity.DocumentId;
        }

        public IEnumerable<DocumentListItem> GetDocuments(Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Documents
                        .Where(e => e.IsDeleted == false && e.OwnerId == userId)
                        .Select(
                            e =>
                                new DocumentListItem
                                {
                                    DocumentId = e.DocumentId,
                                    DocName = e.DocName,
                                    DocString = e.DocString,
                                    DocType = e.DocType,
                                    LocationId = e.LocationId,
                                    DateCreated = e.DateCreated,
                                    SearchString = (e.Locations.LocationCode.ToString() + e.DocName + e.DocType.ToString()).ToUpper(),
                                    LocationCode = e.Locations.LocationCode
                                }
                        );

                return query.ToList().OrderByDescending(e => e.DateCreated);
            }
        }

        public IEnumerable<DocumentListItem> GetDocuments(int locId, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Documents
                        .Where(e => e.LocationId == locId && e.IsDeleted == false && e.OwnerId == userId)
                        .Select(
                            e =>
                                new DocumentListItem
                                {
                                    DocumentId = e.DocumentId,
                                    DocName = e.DocName,
                                    DocString = e.DocString,
                                    DocType = e.DocType,
                                    LocationId = e.LocationId,
                                    DateCreated = e.DateCreated,
                                    LocationCode = e.Locations.LocationCode
                                }
                        );

                return query.ToArray().OrderByDescending(e => e.DateCreated);
            }
        }

        public DocumentDetail GetDocumentById(int id, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Documents
                        .Single(e => e.DocumentId == id && e.OwnerId == userId);
                return
                    new DocumentDetail
                    {
                        DocumentId = entity.DocumentId,
                        OwnerId = entity.OwnerId,
                        DocName = entity.DocName,
                        DocString = entity.DocString,
                        DocType = entity.DocType,
                        LocationId = entity.LocationId,
                        LocationCode = entity.Locations.LocationCode,
                        IsDeleted = entity.IsDeleted
                    };
            }
        }

        public bool DeleteDocument(int docId, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Documents.Single(e => e.DocumentId == docId && e.OwnerId == userId);
                entity.IsDeleted = true;

                return ctx.SaveChanges() == 1;
            }
        }

        public string GetDocStringForLocation(int locId, Guid userId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Documents
                        .FirstOrDefault(e => e.LocationId == locId && e.DocType == docType.Map && e.IsDeleted == false && e.OwnerId == userId);

                if (query == null)
                {
                    return null;
                }
                else
                {
                    return query.DocString;
                }
            }
        }
    }
}
