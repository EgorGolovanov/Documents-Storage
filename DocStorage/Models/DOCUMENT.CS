using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.SqlClient;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace DocStorage.Models
{
    public class Document
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 1)]
        public virtual string NameDocument { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        public virtual int AuthorId { get; set; }

        public virtual User Author { get; set; }
        
        public virtual string BinaryFile { get; set; }

        public Document()
        {
            Date = System.DateTime.Now;

        }

        public static IEnumerable<Document> ConvertTo(List<Document> documents)
        {
            return (IEnumerable<Document>)documents;
        }
    }

    public class DocumentMap : ClassMapping<Document>
    {
        public DocumentMap()
        {
            Id(x => x.Id, m =>m.Generator(Generators.Guid));
            Property(x => x.NameDocument);
            Property(x => x.Date); 
            ManyToOne(x => x.Author, c =>
            {
                c.Cascade(Cascade.Persist);
                c.Column("AuthorId");
            });
            Property(x => x.BinaryFile);  
        }
    }
}