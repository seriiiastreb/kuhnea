using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace K.DAL
{
    public class DataBridge
    {
        private System.Data.Common.DbConnection mSqlConnection = null;
        private string mLastError = string.Empty;
        private string mProvider = "MSSQL";
        private string mConnectionString = string.Empty;
        private string mLimitStringMSSQL = string.Empty;
        private string mLimitStringPGSQL = string.Empty;

        public const string MSSQLProvider = "MSSQL";
        public const string PGSQLProvider = "PGSQL";

        static int SqlCommandTimeoutSeconds = 300;

        public DataBridge(string connectionString, string provider)
        {
            mConnectionString = connectionString;

            mProvider = provider;
            switch (mProvider)
            {
                case "System.Data.SqlClient":
                    mProvider = MSSQLProvider;
                    break;
                case "Npgsql":
                    mProvider = PGSQLProvider;
                    break;
                default:
                    mProvider = MSSQLProvider;
                    break;
            }


            mLimitStringMSSQL = " TOP 1 ";
            mLimitStringPGSQL = string.Empty;

            if (mProvider.Equals("PGSQL"))
            {
                mLimitStringMSSQL = string.Empty;
                mLimitStringPGSQL = " LIMIT 1 ";
            }
        }

        public string LimitStringMSSQL
        {
            get { return mLimitStringMSSQL; }
        }

        public string LimitStringPGSQL
        {
            get { return mLimitStringPGSQL; }
        }

        public string LastError
        {
            get
            {
                return mLastError;
            }

            set
            {
                mLastError = value;
            }
        }

        public string Provider
        {
            get
            {
                return mProvider;
            }
        }

        public string ConcatinateSimbol
        {
            get
            {
                string concatSimbol = string.Empty;

                switch (mProvider)
                {
                    case MSSQLProvider:
                        concatSimbol = " + ";
                        break;

                    case PGSQLProvider:
                        concatSimbol = " || ";
                        break;
                    default:
                        concatSimbol = " + ";
                        break;
                }

                return concatSimbol;
            }

        }
      
        public bool Connect()
        {
            mLastError = string.Empty;

            bool result = false;

            try
            {
                if (mSqlConnection == null)
                {
                    switch (mProvider)
                    {
                        case "MSSQL":
                            mSqlConnection = new System.Data.SqlClient.SqlConnection(mConnectionString);
                            break;

                        case "PGSQL":
                            mSqlConnection = new Npgsql.NpgsqlConnection(mConnectionString);
                            break;

                        default:
                            mSqlConnection = new System.Data.SqlClient.SqlConnection(mConnectionString);
                            break;

                    }
                }

                if (mSqlConnection.State != System.Data.ConnectionState.Open)
                {
                    mSqlConnection.Open();
                }

                if (mSqlConnection.State == System.Data.ConnectionState.Open)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                mLastError = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ss") + " : Exception in Connect(). " + e.Message;
                if (mSqlConnection.State == System.Data.ConnectionState.Open)
                {
                    mSqlConnection.Close();
                }
            }

            return result;
        }

        public void Disconnect()
        {
            if (mSqlConnection.State == System.Data.ConnectionState.Open)
            {
                mSqlConnection.Close();
            }
        }

        private System.Data.Common.DbDataAdapter CreateAdapter(string query)
        {
            System.Data.Common.DbDataAdapter adapter = null;

            switch (mProvider)
            {
                case "MSSQL":
                    adapter = new System.Data.SqlClient.SqlDataAdapter(query, (SqlConnection)mSqlConnection);
                    break;

                case "PGSQL":
                    adapter = new Npgsql.NpgsqlDataAdapter(query, (Npgsql.NpgsqlConnection)mSqlConnection);
                    break;
                default:
                    adapter = new System.Data.SqlClient.SqlDataAdapter(query, (SqlConnection)mSqlConnection);
                    break;
            }

            if (adapter.InsertCommand != null)
            {
                adapter.InsertCommand.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
            }

            if (adapter.UpdateCommand != null)
            {
                adapter.UpdateCommand.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
            }

            if (adapter.SelectCommand != null)
            {
                adapter.SelectCommand.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
            }

            if (adapter.DeleteCommand != null)
            {
                adapter.DeleteCommand.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
            }

            return adapter;
        }

        /// <summary>
        /// Creates adapter with a select command.
        /// </summary>
        /// <param name="selectCommand"></param>
        /// <returns></returns>
        private System.Data.Common.DbDataAdapter CreateSelectAdapter(System.Data.Common.DbCommand selectCommand)
        {
            System.Data.Common.DbDataAdapter adapter = null;

            switch (mProvider)
            {
                case "MSSQL":
                    adapter = new System.Data.SqlClient.SqlDataAdapter();
                    adapter.SelectCommand = selectCommand;
                    break;

                case "PGSQL":
                    adapter = new Npgsql.NpgsqlDataAdapter();
                    adapter.SelectCommand = selectCommand;
                    break;
                default:
                    adapter = new System.Data.SqlClient.SqlDataAdapter();
                    adapter.SelectCommand = selectCommand;
                    break;
            }

            adapter.SelectCommand.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;

            return adapter;
        }

        public System.Data.DataTable ExecuteQuery(string queryString)
        {
            Connect();
            CheckQueryCompliance(queryString);
            mLastError = string.Empty;

            System.Data.DataTable toReturn = new System.Data.DataTable();

            try
            {
                System.Data.Common.DbDataAdapter adapter = this.CreateAdapter(queryString);
                adapter.Fill(toReturn);
            }
            catch (Exception e)
            {
                toReturn = null;
                mLastError = "Query exception. " + e.Message;
            }

            Disconnect();
            return toReturn;
        }

        public System.Data.DataTable ExecuteQuery(string queryString, Hashtable parameters)
        {
            Connect();
            CheckQueryCompliance(queryString);
            mLastError = string.Empty;

            System.Data.DataTable toReturn = new System.Data.DataTable();

            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;

                command.CommandText = queryString;

                foreach (string parameterName in parameters.Keys)
                {
                    object parameterValue = parameters[parameterName];
                    System.Data.Common.DbParameter aParameter = command.CreateParameter();
                    aParameter.ParameterName = parameterName;
                    aParameter.Value = parameterValue;
                    command.Parameters.Add(aParameter);
                }

                System.Data.Common.DbDataAdapter adapter = this.CreateSelectAdapter(command);
                adapter.Fill(toReturn);
            }
            catch (Exception e)
            {
                toReturn = null;
                mLastError = "Query exception. " + e.Message;
            }

            Disconnect();
            return toReturn;
        }

        private bool CheckQueryCompliance(string queryString)
        {
            bool result = false;

            switch (mProvider)
            {
                case "MSSQL":
                    result = true;
                    break;

                case "PGSQL":

                    if (!queryString.ToLower().Contains("isnull"))
                    {
                        result = true;
                    }
                    else
                    {
                        K.Platform.Logger.LogFatal("Non-compliant query. " + queryString);
                        throw new Exception("Non-compliant query.");
                    }

                    break;
                default:
                    result = true;
                    break;
            }

            return result;
        }

        public object ExecuteScalarQuery(string scalarQuery)
        {
            Connect();
            CheckQueryCompliance(scalarQuery);

            mLastError = string.Empty;

            object result = null;
            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
                command.CommandText = scalarQuery;
                result = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                mLastError = "Scalar query exception. " + e.Message;
            }

            Disconnect();
            return result;
        }

        public object ExecuteScalarQuery(string scalarQuery, Hashtable parameters)
        {
            object result = null;

            Connect();
            CheckQueryCompliance(scalarQuery);

            mLastError = string.Empty;

            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
                command.CommandText = scalarQuery;

                foreach (string parameterName in parameters.Keys)
                {
                    object parameterValue = parameters[parameterName];
                    System.Data.Common.DbParameter aParameter = command.CreateParameter();
                    aParameter.ParameterName = parameterName;
                    aParameter.Value = parameterValue;
                    command.Parameters.Add(aParameter);
                }

                result = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                mLastError = "Scalar query exception. " + e.Message;
            }

            Disconnect();
            return result;
        }

        public bool ExecuteNonQuery(string nonQuery)
        {
            Connect();
            CheckQueryCompliance(nonQuery);

            mLastError = string.Empty;

            bool result = false;

            System.Data.Common.DbTransaction transaction = mSqlConnection.BeginTransaction();
            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
                command.Transaction = transaction;

                command.CommandText = nonQuery;

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    result = true;
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                mLastError = "Non-query exception. " + e.Message;
                transaction.Rollback();
            }

            Disconnect();

            return result;
        }

        public bool ExecuteNonQuery(string nonQuery, Hashtable parameters)
        {
            Connect();
            CheckQueryCompliance(nonQuery);

            mLastError = string.Empty;

            bool result = false;

            System.Data.Common.DbTransaction transaction = mSqlConnection.BeginTransaction();
            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;
                command.Transaction = transaction;

                command.CommandText = nonQuery;

                foreach (string parameterName in parameters.Keys)
                {
                    object parameterValue = parameters[parameterName];
                    System.Data.Common.DbParameter aParameter = command.CreateParameter();
                    aParameter.ParameterName = parameterName;
                    aParameter.Value = parameterValue;
                    command.Parameters.Add(aParameter);
                }

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    result = true;
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                mLastError = "Non-query with parameters execution exception. " + e.Message;
                transaction.Rollback();
            }

            Disconnect();

            return result;
        }

        /// <summary>
        /// Executes a batch of queries. All the queries are executed within one (the same) transaction which is commited 
        /// after last query is executed. If an exception is caught during execution, it is reported in mLast error, 
        /// and the transaction is rolled back. 
        /// </summary>
        /// <param name="insertQueryBatch">
        /// An array of queries. 
        /// </param>
        /// <returns>
        /// Total number of affected rows.
        /// </returns>
        public int ExecuteNonQueryBatch(string[] insertQueryBatch)
        {
            Connect();
            int rowsAffected = 0;

            mLastError = string.Empty;

            System.Data.Common.DbTransaction transaction = mSqlConnection.BeginTransaction();
            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;

                command.Transaction = transaction;

                for (int i = 0; i < insertQueryBatch.Length; i++)
                {
                    string nonQuery = insertQueryBatch[i];

                    if (!string.IsNullOrEmpty(nonQuery))
                    {
                        CheckQueryCompliance(nonQuery);

                        command.CommandText = nonQuery;
                        rowsAffected += command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                mLastError = "Non-query batch execution exception. " + e.Message;
                transaction.Rollback();
                rowsAffected = 0;
            }

            Disconnect();

            return rowsAffected;
        }


        /// <summary>
        /// Executes a batch of queries. All the queries are executed within one (the same) transaction which is commited 
        /// after last query is executed. If an exception is caught during execution, it is reported in mLast error, 
        /// and the transaction is rolled back. 
        /// </summary>
        /// <param name="insertQueryBatch">
        /// An array of queries. 
        /// </param>
        /// <param name="parametersArray">
        /// Ann array of parameters. Each index paratemers hashtable correcponds to same index query.
        /// </param>
        /// <returns>
        /// Total number of affected rows.
        /// <returns></returns>
        public int ExecuteNonQueryBatch(string[] insertQueryBatch, Hashtable[] parametersArray)
        {
            Connect();
            int rowsAffected = 0;

            mLastError = string.Empty;

            System.Data.Common.DbTransaction transaction = mSqlConnection.BeginTransaction();
            try
            {
                System.Data.Common.DbCommand command = mSqlConnection.CreateCommand();
                command.CommandTimeout = DataBridge.SqlCommandTimeoutSeconds;

                command.Transaction = transaction;

                for (int i = 0; i < insertQueryBatch.Length; i++)
                {
                    string nonQuery = insertQueryBatch[i];

                    if (!string.IsNullOrEmpty(nonQuery))
                    {
                        CheckQueryCompliance(nonQuery);

                        command.CommandText = nonQuery;
                        if (parametersArray != null && parametersArray.Length > i)
                        {
                            Hashtable parameters = parametersArray[i];
                            if (parameters != null)
                            {
                                foreach (string parameterName in parameters.Keys)
                                {
                                    object parameterValue = parameters[parameterName];
                                    System.Data.Common.DbParameter aParameter = command.CreateParameter();
                                    aParameter.ParameterName = parameterName;
                                    aParameter.Value = parameterValue;
                                    command.Parameters.Add(aParameter);
                                }
                            }
                        }

                        rowsAffected += command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                mLastError = "Non-query batch with parameters execution exception. " + e.Message;
                transaction.Rollback();
                rowsAffected = 0;
            }

            Disconnect();

            return rowsAffected;
        }
    }
}
