using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Data;
using K.DAL;
using System.Collections;
using K.Crypt;


[WebService(Namespace = "http://kService.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService] 
public class KService : System.Web.Services.WebService
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ATTENTION! All methods not related to authentication should call in first line the CheckAuthentication() method //
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Data members
    private readonly string sDbConnectionStringKey = "DBConnectionString";
    DataBridge mDataBridge = null;
    string mLastError = string.Empty;

    #region Logger Setup
    protected static readonly log4net.ILog msLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion Logger Setup
    #endregion Data members

    #region Construction and destruction
    public KService()
    {
        mDataBridge = new DataBridge(System.Configuration.ConfigurationManager.ConnectionStrings[sDbConnectionStringKey].ConnectionString, System.Configuration.ConfigurationManager.ConnectionStrings[sDbConnectionStringKey].ProviderName);
    }
    #endregion Construction and destruction

    #region Helpers
   
    [WebMethod(EnableSession = true)]
    public string LastError()
    {
        CheckAuthentication();
        return mLastError;
    }
    #endregion Helpers

    #region Authentication
    private int UserId
    {
        get
        {
            int userId = (int)Session[SessionKeys.LoggedInUserId];
            return userId;
        }
    }

    [WebMethod(EnableSession = true)]
    public bool IsUserAuthenticated() {
        bool loggedIn = false;
        if (Session[SessionKeys.LoggedIn] != null)
        {            
            loggedIn = (bool)Session[SessionKeys.LoggedIn];
        }

        return loggedIn;
    }

    [WebMethod(EnableSession = true)]
    public bool Login(string login, string password)
    {
        if (login.Contains("'") || login.Contains(" ") || password.Contains("'") || password.Contains(" "))
        {
            throw new Exception("Invalid characters in login or password");
        }

        string passwordEncrypted =  K.Crypt.CryptorEngine.GetMd5Hash(password);

        DataTable result = mDataBridge.ExecuteQuery("SELECT userid FROM users WHERE login = '" + login + "' AND password = '" + passwordEncrypted + "' AND PasswordStatus = " + (int)K.Constants.Classifiers.PasswordStatusActive);

        bool loggedIn = false;
        if (result != null && result.Rows.Count == 1)
        {
            loggedIn = true;
            K.DataObjects.User logetUser = new K.DataObjects.User(result.Rows[0]);
            //Session[SessionKeys.LoggedInUser] = logetUser;
        }
        else
        {
            if (result != null && result.Rows.Count > 1)
            {
                throw new Exception("FATAL Failure. MULTIPLE ROWS RETURNED.");
            }
        }

        if (!string.IsNullOrEmpty(mDataBridge.LastError))
        {
            msLogger.Debug(mDataBridge.LastError);
        }
        
        return loggedIn;
    }

    [WebMethod(EnableSession = true)]
    public bool Logout()
    {
        Session[SessionKeys.LoggedIn] = null;
        Session[SessionKeys.LoggedInUserId] = null;
        return true;
    }

    [WebMethod(EnableSession = true)]
    private void CheckAuthentication()
    {
        if (!IsUserAuthenticated()) 
        {
            throw new Exception("Not autentificated exception");
        }
    }

    [WebMethod(EnableSession = true)]
    public DataTable GetCurrentUserData()
    {
        CheckAuthentication();

        DataTable userData = null;
        int userId = 0;
        
        userId = (int)Session[SessionKeys.LoggedInUserId];
        userData = mDataBridge.ExecuteQuery(
                        @"SELECT users.userid 
                        , users.firstname 
                        , users.lastname 
                        , users.login 
                        , users.password 
                        , users.passwordstatus 
                        , classifierspasswordstatus.name as passworstatusname 
                        , users.recordstatus 
                        , classifiersrecordstatus.name as recordstatusname 
                        , users.edituserid
                        , EditUser.login as edituserlogin 
                        , EditUser.firstname as edituserfirstname 
                        , EditUser.lastname as edituserlastname 
                        , users.editdate 
                        , users.legal_entity_id 
                        , users.main_empl_id 
                        , users.email_address 
                        , users.sysadmin 
                        FROM users as users 
                        LEFT JOIN users AS EditUser ON users.edituserid = EditUser.userid
                        LEFT JOIN classifiers as classifierspasswordstatus on users.passwordstatus = classifierspasswordstatus.code and classifierspasswordstatus.typeid = 45 
                        LEFT JOIN classifiers as classifiersrecordstatus on users.recordstatus = classifiersrecordstatus.code and classifiersrecordstatus.typeid = 8 
                        WHERE users.userid =  " + userId + " "
                    );

        if (userData != null)
        {
            userData.TableName = "User Data";
        }

        return userData;
    }
    #endregion Authentication

    #region Users
    [WebMethod(EnableSession = true)]
    public bool UserEmailExists(string email)
    {
        CheckAuthentication();

        bool result = false;
        DataTable userData = mDataBridge.ExecuteQuery("SELECT * FROM users as users WHERE users.email_address =  '" + email + "' ");
        //result = HasOnlyOneEntry(userData);

        return result;
    }

    [WebMethod(EnableSession = true)]
    public bool LoginExists(string login)
    {
        CheckAuthentication();

        bool result = false;
        DataTable userData = mDataBridge.ExecuteQuery("SELECT * FROM users as users WHERE users.login =  '" + login + "' ");
        if (userData != null && userData.Rows.Count > 0)
        {
            result = true;
        }

        return result;
    }

    [WebMethod(EnableSession = true)]
    public bool BoundUserExists( string legalentityID, string employeeID)
    {
        CheckAuthentication();

        bool result = false;

        DataTable userData = mDataBridge.ExecuteQuery("SELECT * FROM users as users WHERE users.legal_entity_id =  '" + legalentityID + "' AND users.main_empl_id = '" + employeeID + "' ");
        if (userData != null && userData.Rows.Count > 0)
        {
            result = true;
        }

        return result;
    }

    [WebMethod(EnableSession = true)]
    public bool BoundUserExistsExcluding(int excludeUserId, string legalentityID, string employeeID)
    {
        CheckAuthentication();

        bool result = false;

        DataTable userData = mDataBridge.ExecuteQuery("SELECT * FROM users as users WHERE users.legal_entity_id =  '" + legalentityID + "' AND users.main_empl_id = '" + employeeID + "' AND userid <> " + excludeUserId);
        if (userData != null && userData.Rows.Count > 0)
        {
            result = true;
        }

        return result;
    }

    [WebMethod(EnableSession = true)]
    public DataTable GetUserDataById(int userId)
    {
        CheckAuthentication();

        DataTable userData = null;
        
        userData = mDataBridge.ExecuteQuery(
                            @"SELECT users.userid 
                            , users.firstname 
                            , users.lastname 
                            , users.login 
                            , users.password 
                            , users.passwordstatus 
                            , classifierspasswordstatus.name as passworstatusname 
                            , users.recordstatus 
                            , classifiersrecordstatus.name as recordstatusname 
                            , users.edituserid
                            , EditUser.login as edituserlogin 
                            , EditUser.firstname as edituserfirstname 
                            , EditUser.lastname as edituserlastname 
                            , users.editdate 
                            , users.legal_entity_id 
                            , users.main_empl_id 
                            , users.email_address 
                            , users.sysadmin 
                            FROM users as users 
                            LEFT JOIN users AS EditUser ON users.edituserid = EditUser.userid
                            LEFT JOIN classifiers as classifierspasswordstatus on users.passwordstatus = classifierspasswordstatus.code and classifierspasswordstatus.typeid = 45 
                            LEFT JOIN classifiers as classifiersrecordstatus on users.recordstatus = classifiersrecordstatus.code and classifiersrecordstatus.typeid = 8 
                            WHERE users.userid =  " + userId + " "
                    );
        if (userData != null)
        {
            userData.TableName = "User Data";
        }

        return userData;
    }

    [WebMethod(EnableSession = true)]
    public DataTable GetUserDataByCredentials(string login, string password, bool encryptedPassword)
    {
        CheckAuthentication();

        string passwordEncrypted = string.Empty;

        passwordEncrypted = K.Crypt.CryptorEngine.GetMd5Hash(password);

        DataTable userData = null;

        userData = mDataBridge.ExecuteQuery(
                        @"SELECT users.userid 
                        , users.firstname 
                        , users.lastname 
                        , users.login 
                        , users.password 
                        , users.passwordstatus 
                        , classifierspasswordstatus.name as passworstatusname 
                        , users.recordstatus 
                        , classifiersrecordstatus.name as recordstatusname 
                        , users.edituserid
                        , EditUser.login as edituserlogin 
                        , EditUser.firstname as edituserfirstname 
                        , EditUser.lastname as edituserlastname 
                        , users.editdate 
                        , users.legal_entity_id 
                        , users.main_empl_id 
                        , users.email_address 
                        , users.sysadmin 
                        FROM users as users 
                        LEFT JOIN users AS EditUser ON users.edituserid = EditUser.userid
                        LEFT JOIN classifiers as classifierspasswordstatus on users.passwordstatus = classifierspasswordstatus.code and classifierspasswordstatus.typeid = 45 
                        LEFT JOIN classifiers as classifiersrecordstatus on users.recordstatus = classifiersrecordstatus.code and classifiersrecordstatus.typeid = 8 
                        WHERE users.login =  '" + login + "' "
                        + "AND users.password = '" + ( encryptedPassword ? password : passwordEncrypted )+ "' "
                    );
        if (userData != null)
        {
            userData.TableName = "User Data";
        }

        return userData;
    }

    [WebMethod(EnableSession = true)]
    public int AddUser(string userLogin, string firstName, string lastName, string password, int passwordStatus, int recordStatus, string legalEntityID, string mainEmployeeID, string email)
    {
        CheckAuthentication();

        int result = 0;
        string lastError = string.Empty;

        if (userLogin.Contains("'") || userLogin.Contains(" ") || password.Contains("'") || password.Contains(" "))
        {
            throw new Exception("Invalid characters in login or password");
        }

        string encriptedPassword = K.Crypt.CryptorEngine.GetMd5Hash(password);
        int userId = UserId;

        try
        {
            if (mDataBridge.Connect() == true)
            {
                firstName = firstName.Replace("'", "`").Trim();
                lastName = lastName.Replace("'", "`").Trim();                

                string commandText = " INSERT INTO Users (userid, firstname,        lastname,           login,              passwordstatus,             password,               recordstatus,           edituserid,         editdate,       legal_entity_id,          main_empl_id,         email_address,  sysadmin, roletype) "
                                    + " VALUES ( (coalesce( (Select MAX(userid) FROM USERS), 0) + 1), '" + firstName + "', '" + lastName + "', '" + userLogin + "', " + passwordStatus + ", '" + encriptedPassword + "', " + recordStatus + " , " + userId + " , @editDate, '" + legalEntityID + "', '" + mainEmployeeID + "', '" + email + "', @isSysAdmin, 0 ) ";

                Hashtable parameters = new Hashtable();
                parameters.Add("@editDate", DateTime.Now);
                parameters.Add("@isSysAdmin", false);

                bool added = mDataBridge.ExecuteNonQuery(commandText, parameters); // PG compliant
                lastError = mDataBridge.LastError;

                if (added && string.IsNullOrEmpty(lastError))
                {
                    DataTable newUserData = GetUserDataByCredentials(userLogin, encriptedPassword, true);

                    if (newUserData != null && newUserData.Rows.Count == 1)
                    {
                        result = newUserData.Rows[0]["userid"] != DBNull.Value ? (int)newUserData.Rows[0]["userid"] : 0;
                    }
                }
            }
            else
            {
                throw new Exception(mDataBridge.LastError);
            }
        }
        catch (Exception e)
        {
            lastError = e.Message;
        }
        finally
        {
        }

        return result;
    }

    [WebMethod(EnableSession = true)]
    public bool UpdateUser(int userID, string userLogin, string firstName, string lastName, bool resetPassword, string password, int passwordStatus, int recordStatus, string legalEntityID, string mainEmployeeID, string email)
    {
        CheckAuthentication();

        bool result = false;
        string lastError = string.Empty;

        if (userLogin.Contains("'") || userLogin.Contains(" ") || password.Contains("'") || password.Contains(" "))
        {
            throw new Exception("Invalid characters in login or password");
        }

        string encriptedPassword = K.Crypt.CryptorEngine.GetMd5Hash(password);
        int editUserId = UserId;

        try
        {
            if (mDataBridge.Connect() == true)
            {
                firstName = firstName.Replace("'", "`").Trim();
                lastName = lastName.Replace("'", "`").Trim(); 

                string commandText = " UPDATE Users SET "
                                    + " firstname = '" + firstName + "' "
                                    + " , lastname = '" + lastName + "' "
                                    + " , login = '" + userLogin + "' "
                                    + " , passwordstatus = " + passwordStatus + " "
                                    + (resetPassword ? " , password = '" + encriptedPassword + "' " : string.Empty)
                                    + " , recordstatus = " + recordStatus + " "
                                    + " , edituserid = " + editUserId
                                    + " , editdate = @editDate "
                                    + " , legal_entity_id = '" + legalEntityID + "' "
                                    + " , main_empl_id = '" + mainEmployeeID + "' "
                                    + " , email_address = '" + email + "' "
                                    + " WHERE UserID = " + userID + " ";

                Hashtable parameters = new Hashtable();
                parameters.Add("@editDate", DateTime.Now);

                result = mDataBridge.ExecuteNonQuery(commandText, parameters); // PG compliant
                lastError = mDataBridge.LastError;
            }
            else
            {
                throw new Exception(mDataBridge.LastError);
            }
        }
        catch (Exception e)
        {
            lastError = e.Message;
        }
        finally
        {
        }

        return result;
    }

    #endregion Users   
}
