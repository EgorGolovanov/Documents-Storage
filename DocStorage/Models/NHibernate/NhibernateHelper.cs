using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DocStorage.Models;
using DocStorage.Models.NHibernate;


public static class NHibernateHelper
{
    public static ISession OpenSession()
    {
        ISessionFactory sessionFactory = Fluently.Configure()
        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(@"Server=DESKTOP-93DM52M\SQLEXPRESS; initial catalog= Documents; Integrated Security=SSPI;")
        .ShowSql()
        ) 
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DocStorage.Models.Document>())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DocStorage.Models.AccountModel>())
        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
        .BuildSessionFactory();

        return sessionFactory.OpenSession();
    }

    /// <summary>
    /// Выполнение хранимой процедуры по добавлению нового документа в базу данных
    /// </summary>
    /// <param name="session"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public static bool ExecuteStorageProcedure(ISession session, ProcedureQuery query)
    {
        using (var ts = session.BeginTransaction())
        {
            try
            {
                var command = new SqlCommand { Connection = (SqlConnection)session.Connection };
                ts.Enlist(command);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = query.Procedure;
                command.Parameters.AddRange(query.Parameters.ToArray());
                command.ExecuteNonQuery();
                ts.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Выполнение хранимой процедуры с возвратом результата
    /// </summary>
    /// <param name="session"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    public static IEnumerable<Document> ExecuteWithReturn(ISession session, ProcedureQuery query)
    {
        //обработать ошибки
        List<Document> documents = new List<Document>();
        using (var ts = session.BeginTransaction())
        {
            var command = new SqlCommand { Connection = (SqlConnection)session.Connection };
            ts.Enlist(command);

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = query.Procedure;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Document doc = new Document();
                    doc.name = reader.GetString(1);
                    doc.autor = reader.GetString(2);
                    doc.date = reader.GetDateTime(3);
                    doc.binaryFile = reader.GetString(4);
                    documents.Add(doc);
                }
            }
            ts.Commit();

            return Document.ConvertTo(documents);
        }
    }
}