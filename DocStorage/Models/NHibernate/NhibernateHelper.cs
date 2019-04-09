using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using System.Data;
using System.Data.SqlClient;
using DocStorage.Models.NHibernate;
using System.Reflection;


public static class NHibernateHelper
{

    public static ISession OpenSession()
    {
        var cfg = new Configuration()
        .DataBaseIntegration(db => {
            db.ConnectionString = @"Server=DESKTOP-93DM52M\SQLEXPRESS;initial catalog=MyDocumentsDB;Integrated Security=SSPI;";
            db.Dialect<MsSql2012Dialect>();
        });
        var mapper = new ModelMapper();
        mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
        HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        cfg.AddMapping(mapping);
        new SchemaUpdate(cfg).Execute(true, true);
        ISessionFactory sessionFactory = cfg.BuildSessionFactory();
        return sessionFactory.OpenSession();
    }

/// <summary>
/// Выполнение хранимой процедуры по добавлению нового документа в базу данных
/// </summary>
/// <param Name="session"></param>
/// <param Name="query"></param>
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
            catch (SqlException e)
            {
                return false;
            }
        }
    }
}