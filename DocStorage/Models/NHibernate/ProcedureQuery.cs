using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DocStorage.Models.NHibernate
{
    public class ProcedureQuery
    {
        public ProcedureQuery(string procedure)
        {
            Procedure = procedure;
            Parameters = new List<SqlParameter>();
        }

        public string Procedure { get; private set; }
        public List<SqlParameter> Parameters { get; private set; }

        public void AddInt32(string name, int value)
        {
            AddParameter(name, SqlDbType.Int, value);
        }

        public void AddString(string name, string value)
        {
            AddParameter(name, SqlDbType.VarChar, value);
        }

        public void AddDateTime(string name, DateTime value)
        {
            AddParameter(name, SqlDbType.DateTime, value);
        }

        private void AddParameter(string name, SqlDbType type, object value)
        {
            var parameter = new SqlParameter("@" + name, type)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
            Parameters.Add(parameter);
        }

    }
}