using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using DocStorage.Models;
using DocStorage.Models.NHibernate;
using NHibernate;

namespace DocStorage.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            AccountModel account = new AccountModel();
            return View("AccountView", account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AccountModel account)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var user = session.CreateSQLQuery(String.Format("select Users.name from Users where Users.name='{0}' and Users.password='{1}'", account.name, account.password))
                    .SetMaxResults(1)
                    .SetCacheable(true)
                    .UniqueResult();
                if (user == null)
                {
                    Session["checkCorrect"] = false;
                    return View("AccountView");
                }
                else
                {
                    Session["userName"] = user.ToString();
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
