using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
//using AjaxControlToolkit;

public class ServerObject
{
    #region Data members
    string mLastError = string.Empty;
    DataBridge mDataBridge = null;
    string Plus = string.Empty;


    string mUser = string.Empty;
    #endregion Data members

    #region Properties
   
    public string LastError
    {
        get { return mLastError; }
    }
   
    #endregion Properties
    
    #region Construction and destruction
    public ServerObject()
    {   
        mDataBridge = new DataBridge();
        Plus = mDataBridge.ConcatinateSimbol + " ' ' " + mDataBridge.ConcatinateSimbol;
    }
  
    ~ServerObject()
    {
        mDataBridge = null;
    }
    #endregion Construction and destruction
        
    public DataTable ChekExistRegistredEmail(string login, string inputEmail)
    {
        DataTable result = new DataTable();
        mLastError = string.Empty;

        try
        {
            if (mDataBridge.Connect() == true)
            {
                string query = "select * from users where email= '" + inputEmail + "' and login = '" + login + "' ";

                result = mDataBridge.ExecuteQuery(query);
                mLastError = mDataBridge.LastError;
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }   
      
    #region Guarantors Region

    public DataTable GetGuarantorsById(string GuarantorsID)
    {
        DataTable resultDataTable = new DataTable();

        try
        {
            if (mDataBridge.Connect() == true)
            {
                string commandText = @"SELECT * "
                                     + " , (select Name From Classifiers Where Code = taraID) as tara_string "
                                     + " , (select Name From Classifiers Where Code = raionID) as raion_string "
                                     + " , (select Name From Classifiers Where Code = jobID) as job_string "
                                     + " , (select Name From Classifiers Where Code = surseVenitID) as sursevenit_string "
                                     + " FROM Guarantos "
                                     + " WHERE GuarantorsID = " + GuarantorsID;

                resultDataTable = mDataBridge.ExecuteQuery(commandText); // PG compliant
                mLastError = mDataBridge.LastError;
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return resultDataTable;
    }

    public DataTable GetGuarantorsList()
    {
        DataTable resultDataTable = new DataTable();

        try
        {
            if (mDataBridge.Connect() == true)
            {
                string commandText = @"SELECT * "                                     
                                     + " , (select Name From Classifiers Where Code = taraID) as tara_string "
                                     + " , (select Name From Classifiers Where Code = raionID) as raion_string "
                                     + " , (select Name From Classifiers Where Code = jobID) as job_string "
                                     + " , (select Name From Classifiers Where Code = surseVenitID) as sursevenit_string "
                                     + " FROM Guarantos "
                                     + " ORDER BY Name ";

                resultDataTable = mDataBridge.ExecuteQuery(commandText); // PG compliant
                mLastError = mDataBridge.LastError;
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return resultDataTable;
    }

    public bool AddGuarantors(string name, int taraID, int raionID, string localitate, string address, string phoneFix, string phoneGsm, int jobID, int surseVenitID, int venitLunar)
    {
        bool result = false;
        try
        {
            if (mDataBridge.Connect() == true)
            {
                string nonQuery = @"INSERT INTO Guarantos (name, taraID, raionID, localitate, address, phoneFix, phoneGsm, jobID, surseVenitID, venitLunar)"
                                       + " VALUES ( '" + name + "'," + taraID + "," + raionID + ",'" + localitate + "', '" + address + "', '" + phoneFix + "', '" + phoneGsm + "', " + jobID + ", " + surseVenitID + ", " + venitLunar + ")";

                result = mDataBridge.ExecuteNonQuery(nonQuery); // PG compliant
                mLastError = mDataBridge.LastError;
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public bool UpdateGuarantors(int GuarantorsID, string name, int taraID, int raionID, string localitate, string address, string phoneFix, string phoneGsm, int jobID, int surseVenitID, int venitLunar)
    {
        bool result = false;
        try
        {
            if (mDataBridge.Connect() == true)
            {
                string nonQuery = @"Update Guarantos "
                    + " SET Name = '" + name + "' "
                    + ", TaraID = " + taraID
                    + ", RaionID = " + raionID
                    + ", Localitate = '" + localitate + "' "
                    + ", Address = '" + address + "' "
                    + ", PhoneFix = '" + phoneFix + "' "
                    + ", PhoneGSM = '" + phoneGsm + "' "
                    + ", JobID = " + jobID
                    + ", SurseVenitID = " + surseVenitID 
                    + ", VenitLunar = " + venitLunar + " "
                    + " WHERE GuarantorsID = " + GuarantorsID;

                result = mDataBridge.ExecuteNonQuery(nonQuery); // PG compliant
                mLastError = mDataBridge.LastError;
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public bool DeleteGuarantors(int GuarantorsID)
    {
        bool result = false;
        try
        {
            if (mDataBridge.Connect() == true)
            {
                string nonQuery = @"Delete From Guarantos WHERE GuarantorsID = " + GuarantorsID;

                result = mDataBridge.ExecuteNonQuery(nonQuery); // PG compliant
                mLastError = mDataBridge.LastError;

            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    #endregion Guarantors Region
     
    #region For Universal SQL

    #region Insert
    
    public bool UniversalInsertFromValue(string listArgument, string insertTableName, string mNamePanel)
    {
        bool result = false;
        try
        {
            Hashtable insertHashTable = Utils.newSaveButton(listArgument, insertTableName, mNamePanel);

            if (!string.IsNullOrEmpty(insertTableName) && insertHashTable!=null)
            {
                if (mDataBridge.Connect() == true)
                {
                    ArrayList arrayKeyList = new ArrayList(insertHashTable.Keys);

                    string csvColumns = string.Empty;
                    string csvValues = string.Empty;

                    for (int indexList = 0; indexList < arrayKeyList.Count; indexList++)
                    {
                        if (indexList > 0)
                        {
                            csvColumns += ",";
                            csvValues += ", ";
                        }

                        csvColumns += arrayKeyList[indexList].ToString().Replace("@",string.Empty);
                        csvValues += arrayKeyList[indexList].ToString();
                    }

                    string nonQuery = @"INSERT INTO " + insertTableName + " ("+csvColumns+")"
                                           + " VALUES ( " + csvValues + ")";

                    result = mDataBridge.ExecuteNonQuery(nonQuery, insertHashTable); // PG compliant
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }
    
    public bool UniversalInsertFromValue(string insertTableName, Hashtable insertHashTable)
    {
        bool result = false;
        try
        {
            if (!string.IsNullOrEmpty(insertTableName) && insertHashTable != null)
            {
                if (mDataBridge.Connect() == true)
                {
                    string csvColumns = insertHashTable[Utils.hashtableKeyInsertColumns].ToString();
                    string csvValues = insertHashTable[Utils.hashtableKeyInsertValues].ToString();

                    string nonQuery = @"INSERT INTO " + insertTableName + " (" + csvColumns + ")"
                                           + " VALUES ( " + csvValues + ")";

                    Hashtable paramsHashTable = new Hashtable();

                    if (insertHashTable.Count == 3)
                    {
                        paramsHashTable = (Hashtable)insertHashTable[Utils.hashtableKeyInsertHashtable];
                    }

                    result = mDataBridge.ExecuteNonQuery(nonQuery, paramsHashTable); // PG compliant
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public bool UniversalInsertFromSelect() //todo
    {
        return true;
    }
    #endregion
    #region Update
    public bool UniversalUpdateFromValue(string updateTableName, string csvSet, string whereCondition)
    {
        bool result = false;
        mLastError = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(updateTableName) && !string.IsNullOrEmpty(csvSet))
            {
                if (mDataBridge.Connect() == true)
                {
                    string setColumnAndValue = csvSet;
                    string whereUpdate = string.Empty;
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        whereUpdate = " WHERE " + whereCondition;
                    }

                    string query = " UPDATE " + updateTableName + " SET "
                                    + setColumnAndValue
                                    + whereUpdate
                                    + " ";

                    result = mDataBridge.ExecuteNonQuery(query);
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public bool UniversalUpdateFromSelect() //todo
    {
        return true;
    }
    #endregion update
    #region Delete
    public bool UniversalMarkForDeleteFromValue(string deleteTableName, string whereCondition)
    {
        bool result = false;
        mLastError = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(deleteTableName))
            {
                if (mDataBridge.Connect() == true)
                {
                    string deleteWhereCondition = string.Empty;
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        deleteWhereCondition = " WHERE " + whereCondition;
                    }

                    string query = " Update " +deleteTableName+" set isB=1"
                    + deleteWhereCondition
                    + "" ;

                    result = mDataBridge.ExecuteNonQuery(query);
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public bool UniversalMarkForDeleteFromSelect() //todo
    {
        return true;
    }

    public bool UniversalDeleteFromValue(string deleteTableName, string whereCondition)
    {
        bool result = false;
        mLastError = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(deleteTableName))
            {
                if (mDataBridge.Connect() == true)
                {
                    string deleteWhereCondition = string.Empty;
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        deleteWhereCondition = " WHERE " + whereCondition;
                    }

                    string query = " DELETE FROM " + deleteTableName
                    + deleteWhereCondition
                    + "";

                    result = mDataBridge.ExecuteNonQuery(query);
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    #endregion delete
    #region Select
    public DataTable UniversalGetFromSQLQuery(string sqlQuery)
    {
        DataTable result = new DataTable();
        mLastError = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                if (mDataBridge.Connect() == true)
                {
                    result = mDataBridge.ExecuteQuery(sqlQuery);
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public DataTable UniversalGetFromSingleTable(string tableName, string csvColumns, string filterSelect)
    {
        DataTable result = new DataTable();
        mLastError = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(csvColumns))
            {
                if (mDataBridge.Connect() == true)
                {
                    string activeFilter = string.Empty;
                    if (!string.IsNullOrEmpty(filterSelect))
                    {
                        activeFilter = " WHERE " + filterSelect;
                    }

                    string query = " SELECT " + csvColumns
                                + " FROM " + tableName + " AS T0 "
                                + " " + activeFilter
                                + " ";

                    result = mDataBridge.ExecuteQuery(query);
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }

    public DataTable UniversalGetFromMultipleTable(string csvTableName,
        string csvJoin, string csvON, string csvColumns, string filterSelect)    //todo
    {
        DataTable result = new DataTable();
        mLastError = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(csvTableName) && !string.IsNullOrEmpty(csvColumns))
            {
                List<string> listTable = new List<string>(csvTableName.Split(','));
                List<string> listJoin = new List<string>(csvJoin.Split(','));
                List<string> listON = new List<string>(csvON.Split(','));
                string fromTables = string.Empty;

                if (listTable != null && listTable.Count > 0 
                    && listJoin!=null && listJoin.Count>0
                    && listON!=null && listON.Count>0)
                {
                    for (int indexList = 0; indexList < listTable.Count; indexList++)
                    {
                        if (indexList == 0)
                        {
                            fromTables = " FROM " + listTable[indexList];
                        }
                        else
                        {
                            fromTables += " " + listJoin[indexList-1] + " JOIN " + listTable[indexList];
                            fromTables += " ON " + listON[indexList - 1];
                        }
                    }
                }

                if (mDataBridge.Connect() == true)
                {
                    string activeFilter = string.Empty;
                    if (!string.IsNullOrEmpty(filterSelect))
                    {
                        activeFilter = " WHERE " + filterSelect;
                    }

                    string query = "SELECT " + csvColumns
                                + " " + fromTables
                                + " " + activeFilter
                                + " ";

                    result = mDataBridge.ExecuteQuery(query);
                    mLastError = mDataBridge.LastError;
                }
            }
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }
    #endregion select

    #region General Function

    public DataTable GetTableByTableName(string tableName)
    {
        DataTable sourceTable = new DataTable();
        mLastError = string.Empty;
        try
        {            
            string csvColumns = " * ";

            string filterSelect = " isB = 0 ";

            sourceTable = UniversalGetFromSingleTable(tableName, csvColumns, filterSelect);
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }

        return sourceTable;
    }

    public DataTable GetColumnsByTableName(string tableName)
    {
        DataTable sourceTable = new DataTable();
        mLastError = string.Empty;
        try
        {
            string csvTableName = "sys.tables AS t, sys.columns AS C";
            string csvJoin = "INNER";
            string csvON = "t.OBJECT_ID = c.OBJECT_ID";
            string csvColumns = "c.name AS column_name";
            string filterSelect = " t.name= '" + tableName + "' ";
            sourceTable = UniversalGetFromMultipleTable(csvTableName, csvJoin, csvON, csvColumns, filterSelect);
            
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }

        return sourceTable;
    }

    public DataTable GetConfigPanelByPanelName(string panelName)
    {
        DataTable sourceTable = new DataTable();
        mLastError = string.Empty;
        string mTableName = "ConfigPanels";
        try
        {
            string csvColumns = "*";
            string filterSelect = " isB=0 and PanelName='" + panelName + "' ";
            sourceTable = UniversalGetFromSingleTable(mTableName, csvColumns, filterSelect);
        }
        catch (Exception exception)
        {
            mLastError += "Error using DataBridge. " + exception.Message;
        }

        return sourceTable;
    }

    #endregion General Function

    #endregion For Universal SQL
    
    public bool DeletePaymentByList(int loanID, int iterationID)
    {
        bool result = false;
        try
        {
            if (Security.MainModule.DataBridge.Connect() == true)
            {
                string query = " UPDATE LoanOrders SET "
                            + " payedDate = @payedData "
                            + " , percentValue_Pays = @percentValue "
                            + " , mainRate_Pays = @mainRate "
                            + " , additionalPays = @additionalPays "
                            + " , additionalPaysComment = @additionalPaysComment "
                            + ", payedOrderNumber = @payedOrderNumber "
                            + " , input_Pays = @inputPays "
                            + " , diference_Pays = @diferencePays "
                            + " WHERE LoanID = " + loanID + " AND iterationID = " + (iterationID-1);

                Hashtable parameters = new Hashtable();
                parameters.Add("@payedData", DBNull.Value);
                parameters.Add("@percentValue", 0);
                parameters.Add("@mainRate", 0);
                parameters.Add("@additionalPays", 0);
                parameters.Add("@additionalPaysComment", string.Empty);
                parameters.Add("@payedOrderNumber", string.Empty);
                parameters.Add("@inputPays", 0);
                parameters.Add("@diferencePays", 0);

                result = Security.MainModule.DataBridge.ExecuteNonQuery(query, parameters);
                mLastError = Security.MainModule.DataBridge.LastError;

            }
        }
        finally
        {
            Security.MainModule.DataBridge.Disconnect();
        }

        return result;
    }

    public bool AddRefunds(int loanID, int iterationID, DateTime payDate, decimal main, decimal profit, decimal penalty, decimal currenty, decimal commission)
    {
        bool result = false;
        try
        {
            if (mDataBridge.Connect() == true)
            {

                string nonQuery = @"INSERT INTO LoanOrdersPays (LoanID, itertaionID, payDate, main, profit, penalty, currenty, commission)"
                                       + " VALUES ("+loanID+","+iterationID+","+payDate+","+main+","+profit+","+penalty+","+currenty+","+commission+" )";

                result = mDataBridge.ExecuteNonQuery(nonQuery); // PG compliant
                mLastError = mDataBridge.LastError;
            }
        }
        finally
        {
            mDataBridge.Disconnect();
        }

        return result;
    }
}