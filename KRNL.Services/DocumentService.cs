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

        public bool CreateDocument(DocumentCreate model, string pathString)
        {
            var entity = new Document()
            {
                OwnerId = _userId,
                DocName = pathString.Substring((pathString.LastIndexOf("uploads") + 8), pathString.Length - (pathString.LastIndexOf("uploads") + 8)),
                DocString = pathString,
                DocType = model.DocType,
                LocationId = model.LocationId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Documents.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<DocumentListItem> GetDocuments()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Documents
                        .Where(e => e.IsDeleted == noYes.No)
                        .Select(
                            e =>
                                new DocumentListItem
                                {
                                    DocumentId = e.DocumentId,
                                    DocName = e.DocName,
                                    DocString = e.DocString,
                                    DocType = e.DocType,
                                    LocationId = e.LocationId,
                                    LocationCode = e.Locations.LocationCode
                                }
                        );

                return query.ToList();
            }
        }

        public IEnumerable<DocumentListItem> GetDocuments(int locId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Documents
                        .Where(e => e.LocationId == locId && e.IsDeleted == noYes.No)
                        .Select(
                            e =>
                                new DocumentListItem
                                {
                                    DocumentId = e.DocumentId,
                                    DocName = e.DocName,
                                    DocString = e.DocString,
                                    DocType = e.DocType,
                                    LocationId = e.LocationId,
                                    LocationCode = e.Locations.LocationCode
                                }
                        );

                return query.ToArray();
            }
        }

        public DocumentDetail GetDocumentById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Documents
                        .Single(e => e.DocumentId == id);
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

        public bool DeleteDocument(int docId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Documents.Single(e => e.DocumentId == docId);
                entity.IsDeleted = noYes.Yes;

                return ctx.SaveChanges() == 1;
            }
        }

        public string GetDocStringForLocation(int locId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Documents
                        .FirstOrDefault(e => e.LocationId == locId && e.DocType == docType.Map);

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
