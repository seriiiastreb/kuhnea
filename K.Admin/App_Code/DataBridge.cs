using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

public class DataBridge
{
	private SqlConnection mSqlConnection = null;
    private string mLastError = string.Empty;
    private string mConnectionString = string.Empty;
    private string mProvider = "MSSQL";
    public const string MSSQLProvider = "MSSQL";
    public const string PGSQLProvider = "PGSQL";

    public DataBridge()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["mainDBConnectionString"].ToString();
        mConnectionString = connectionString;
    }

    public DataBridge(string connectionString)
    {
        mConnectionString = connectionString;
    }

    public string LastError
    {
        get { return mLastError; }
        set { mLastError = value; }
    }

    public bool Connect()
    {
        mLastError = string.Empty;

        bool result = false;

        mSqlConnection = new SqlConnection(mConnectionString);

        try
        {
            if (mSqlConnection.State != ConnectionState.Open)
            {
                mSqlConnection.Open();
            }
            result = true;
        }
        catch (Exception e)
        {
            mLastError = DateTime.Now.ToString() + " : " + e.Message;
            if (mSqlConnection.State == ConnectionState.Open)
            {
                mSqlConnection.Close();
            }
        }

        return result;
    }

    public void Disconnect()
    {
        if (mSqlConnection.State == ConnectionState.Open)
        {
            mSqlConnection.Close();
        }

        mLastError = string.Empty;
    }
    
    public object ExecuteScalarQuery(string scalarQuery)
    {
        object result = null;
        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();
            command.CommandText = scalarQuery;
            result = command.ExecuteScalar();
        }
        catch (Exception e)
        {
            mLastError = e.Message;
        }

        return result;
    }

    public object ExecuteScalarQuery(string scalarQuery, Hashtable parameters)
    {
        object result = null;
        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();
            command.CommandText = scalarQuery;

            foreach (string parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];
                SqlParameter aParameter = command.CreateParameter();
                aParameter.ParameterName = parameterName;
                aParameter.Value = parameterValue;
                command.Parameters.Add(aParameter);
            }

            result = command.ExecuteScalar();
        }
        catch (Exception e)
        {
            mLastError = e.Message;
        }

        return result;
    }

    public System.Data.DataTable ExecuteQuery(string queryString)
    {
        System.Data.DataTable toReturn = new System.Data.DataTable();

        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();
            command.CommandText = queryString;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(toReturn);

        }
        catch (Exception e)
        {
            toReturn = null;
            mLastError = e.Message;
        }

        return toReturn;
    }

    public System.Data.DataTable ExecuteQuery(string queryString, Hashtable parameters)
    {
        mLastError = string.Empty;

        System.Data.DataTable toReturn = new System.Data.DataTable();

        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();
            command.CommandText = queryString;

            foreach (string parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];
                SqlParameter aParameter = command.CreateParameter();
                aParameter.ParameterName = parameterName;
                aParameter.Value = parameterValue;
                command.Parameters.Add(aParameter);
            }

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(toReturn);
        }
        catch (Exception e)
        {
            toReturn = null;
            mLastError = "Query exception. " + e.Message;
        }

        return toReturn;
    }

    public bool ExecuteNonQuery(string nonQuery)
    {
        bool result = false;
        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();
            command.CommandText = nonQuery;

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                result = true;
            }
        }
        catch (Exception e)
        {
            mLastError = e.Message;
        }

        return result;
    }

    public bool ExecuteNonQuery(string nonQuery, Hashtable parameters)
    {
        bool result = false;

        SqlTransaction transaction = mSqlConnection.BeginTransaction();
        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();
            command.Transaction = transaction;

            command.CommandText = nonQuery;

            foreach (string parameterName in parameters.Keys)
            {
                object parameterValue = parameters[parameterName];
                SqlParameter aParameter = command.CreateParameter();
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

        return result;
    }

    internal bool ExecuteNonQueryBatch(string[] insertQueryBatch)
    {
        bool result = true;
        SqlTransaction transaction = mSqlConnection.BeginTransaction();
        try
        {
            SqlCommand command = mSqlConnection.CreateCommand();

            command.Transaction = transaction;

            for (int i = 0; i < insertQueryBatch.Length; i++)
            {
                string nonQuery = insertQueryBatch[i];

                command.CommandText = nonQuery;
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    result &= true;
                }
                else
                {
                    result &= false;
                    mLastError = "Failed to insert data. Statement:\r\n" + nonQuery + "\r\n";
                }
            }

            if (result)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }
        }
        catch (Exception e)
        {
            mLastError = e.Message;
            transaction.Rollback();
        }

        return result;
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

    public string Provider
    {
        get
        {
            return mProvider;
        }
    }
}