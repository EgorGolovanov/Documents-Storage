using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using DocStorage.Models;
using DocStorage.Models.NHibernate;
using NHibernate;
using NHibernate.Linq;

namespace DocStorage.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            User account = new User();
            return View("ViewAccount", account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User account)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var user = session.Query<User>().Where(a => a.Name.Equals(account.Name) && a.Password.Equals(account.Password)).FirstOrDefault();
                if (user == null)
                {
                    Session["checkCorrect"] = false;
                    return View("ViewAccount");
                }
                else
                {
                    Session["id"] = user.Id;
                    Session["userName"] = user.Name;
                    Session["checkCorrect"] = true;
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateUsers()
        {         
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    string scriptSql = System.IO.File.ReadAllText(Server.MapPath("~\\Insert_User.sql"));
                    var query = session.CreateSQLQuery(scriptSql);
                    query.ExecuteUpdate();
                    Session["CreateUsers"] = true;
                }
                catch (NHibernate.Exceptions.GenericADOException e) 
                { 
                    Session["CreateUsers"] = false; 
                }
            }
            return RedirectToAction("Index");
        }

    }
}
