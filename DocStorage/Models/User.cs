using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DocStorage.Models
{
    public class User
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 2)]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, MinimumLength = 2)]
        public virtual string Password { get; set; }

        private IList<Document> documents;
        public virtual IList<Document> Documents
        {
            get
            {
                return documents ?? (documents = new List<Document>());
            }
            set { documents = value; }
        }
    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Name);
            Property(x => x.Password);
            Bag(x => x.Documents,
                    c => { c.Key(k => k.Column("AuthorId")); c.Inverse(true); },
                    r => r.OneToMany());
        }
    }
}