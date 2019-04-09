using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using System.Data.SqlClient;
using System.IO;
using DocStorage.Models;
using DocStorage.Models.NHibernate;
using System.Diagnostics;

namespace DocStorage.Controllers
{
    public class HomeController : Controller
    {
        IEnumerable<Document> documents;

        public ActionResult Index()
        {
            return View("Home");
        }

        [HttpGet]
        public ActionResult Create()
        {
            Document doc = new Document();
            return View(doc);
        }

        /// <summary>
        /// POST метод создания документа. Сохраняет документ в базу данных посредством хранимой процедуры,
        /// файл сохраняет в поддиректорию приложения (папка "Files")
        /// </summary>
        /// <param Name="doc">Модель документа</param>
        /// <param Name="fileupload">Перечислитель выбранных пользователем файлов</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Document doc, IEnumerable<HttpPostedFileBase> fileupload)
        {
            doc.AuthorId = int.Parse(Session["id"].ToString());
            using (ISession session = NHibernateHelper.OpenSession())
            {
                doc.Id = session.Query<Document>().ToList().Count + 1;
                foreach (var file in fileupload)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var extention = System.IO.Path.GetExtension(file.FileName);
                        var path = System.IO.Path.Combine(Server.MapPath("~/Content/Files/"), doc.NameDocument + extention);
                        file.SaveAs(path);

                        ProcedureQuery query = new ProcedureQuery("sp_InsertDocument");
                        query.AddInt32("id", doc.Id);
                        query.AddString("name", doc.NameDocument);
                        query.AddInt32("authorId", doc.AuthorId);
                        query.AddDateTime("date", doc.Date);
                        query.AddString("binaryFile", path);


                        if (!NHibernateHelper.ExecuteStorageProcedure(session, query))
                            return View("Create");
                    }
                    else 
                        return View("Create");
                }   
            }
   
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Выводит список всех документов, осуществляет поиск по документам и их сортировку
        /// </summary>
        /// <param Name="sortOrder"> Возвращает строку, которая выполняет нужную сортировку</param>
        /// <param Name="searchString"> Возвращает строку, введенную в поиске для поиска документов</param>
        /// <returns></returns>
        public ActionResult Search(string sortOrder, string searchString)
        {
            if (documents == null)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    documents = session.Query<Document>().ToList();
                    var user = session.Query<User>().ToList();
                    if (documents == null)
                        return RedirectToAction("Create", "Home");
                }
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AutorSortParm = sortOrder == "Autor" ? "autor_desc" : "Autor";

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.NameDocument.Contains(searchString)
                                       || s.Author.Name.Contains(searchString) 
                                       || s.Date.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    documents = documents.OrderByDescending(s => s.NameDocument);
                    break;
                case "Date":
                    documents = documents.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    documents = documents.OrderByDescending(s => s.Date);
                    break;
                case "autor_desc":
                    documents = documents.OrderByDescending(s => s.Author.Name);
                    break;
                case "Autor":
                    documents = documents.OrderBy(s => s.Author.Name);
                    break;
                default:
                    documents = documents.OrderBy(s => s.Date);
                    break;
            }
            return View(documents);
        }

        public ActionResult OpenFile(string path)
        {
            Process.Start(path);
            return RedirectToAction("Search");
        }
    }
}
