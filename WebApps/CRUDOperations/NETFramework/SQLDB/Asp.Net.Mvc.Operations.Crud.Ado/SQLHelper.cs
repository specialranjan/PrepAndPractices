using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.Ajax.Utilities;

namespace Asp.Net.Mvc.Operations.Crud.Ado
{
    public class SqlHelper
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable table;
        public SqlHelper()
        {
            this.connection = new SqlConnection(ConfigurationManager.AppSettings["DbConnectionString"]);
            this.command = new SqlCommand();
            this.command.Connection = this.connection;
            this.command.CommandType = CommandType.Text;
            this.adapter = new SqlDataAdapter();
            table = new DataTable();
            this.connection.Open();
        }

        public DataTable GetTable(string queryText)
        {
            //this.connection.Open();
            this.command.CommandText = queryText;
            this.adapter.SelectCommand = this.command;
            this.adapter.Fill(this.table);
            //this.connection.Close();
            return this.table;
        }

        public DataTable GetTable(string queryText, SqlParameter sqlParameter)
        {
            //this.connection.Open();
            this.command.CommandText = queryText;
            this.command.Parameters.Add(sqlParameter);
            this.adapter.SelectCommand = this.command;
            this.adapter.Fill(this.table);
            //this.connection.Close();
            return this.table;
        }

        public DataTable GetTable(string queryText, SqlParameter[] sqlParameters)
        {
            //this.connection.Open();
            this.command.CommandText = queryText;
            this.command.Parameters.AddRange(sqlParameters);
            this.adapter.SelectCommand = this.command;
            this.adapter.Fill(this.table);
            //this.connection.Close();
            return this.table;
        }

        public int UpdateTable(string queryText, SqlParameter sqlParameter)
        {
            this.command.CommandText = queryText;
            this.command.Parameters.Add(sqlParameter);
            //this.connection.Close();
            return this.command.ExecuteNonQuery();
        }

        public int UpdateTable(string queryText, SqlParameter[] sqlParameters)
        {
            //this.connection.Open();
            this.command.CommandText = queryText;
            this.command.Parameters.AddRange(sqlParameters);
            //this.connection.Close();
            return this.command.ExecuteNonQuery();
        }

        ~SqlHelper()
        {
            this.connection.Close();
            if (this.connection != null)
                this.connection.Dispose();
            if (this.command != null)
                this.command.Dispose();
            if (this.adapter != null)
                this.adapter.Dispose();
        }
    }
}