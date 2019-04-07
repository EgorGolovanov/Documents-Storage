using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace DocStorage.Models
{
    public class Document
    {
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 1)]
        public virtual string name { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        [DataType(DataType.Date)]
        public virtual DateTime date { get; set; }

        public virtual string autor { get; set; }
        
        public virtual string binaryFile { get; set; }

        public Document()
        {
            date = System.DateTime.Now;
        }

        public static IEnumerable<Document> ConvertTo(List<Document> documents)
        {
            return (IEnumerable<Document>)documents;
        }
    }
}