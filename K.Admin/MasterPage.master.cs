using System;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage, IMasterItems
{
    public string navMainMenu = string.Empty;
    public string leftSubmenu = string.Empty;
    string mCurrentModule = string.Empty;

    #region IMaster Proprietes
    
    ServerObject IMasterItems.ServerObject
    {
        get
        {
            ServerObject mServerObject = (ServerObject)Session["ServerObject"];
            if (mServerObject == null) mServerObject = new ServerObject();
            return mServerObject;
        }
    }

    Credits.Module IMasterItems.ModuleCredits
    {
        get
        {
            Credits.Module mModuleCredits = (Credits.Module)Session["ModuleCredits"];
            if (mModuleCredits == null) Response.Redirect(Utils.GetApplicationPath(Request) + "/Default.aspx");
            return mModuleCredits;
        }
    }

    Accounting.Module IMasterItems.ModuleAccounting
    {
        get
        {
            Accounting.Module mModuleAccounting = (Accounting.Module)Session["ModuleAccounting"];
            if (mModuleAccounting == null) Response.Redirect(Utils.GetApplicationPath(Request) + "/Default.aspx");
            return mModuleAccounting;
        }
    }

    Security.Module IMasterItems.ModuleSecurity
    {
        get
        {
            Security.Module mModuleSecurity = (Security.Module)Session["ModuleSecurity"];
            if (mModuleSecurity == null) Response.Redirect(Utils.GetApplicationPath(Request) + "/Default.aspx");
            return mModuleSecurity;
        }
    }

    Security.User IMasterItems.UserObject
    {
        get
        {
            Security.User mUserObject = (Security.User)Session["UserObject"];
            if (mUserObject == null) Response.Redirect(Utils.GetApplicationPath(Request) + "/Default.aspx");
            return mUserObject;
        }
    }

    Security.MainModule IMasterItems.ModuleMain
    {
        get
        {
            Security.MainModule mModuleMain = (Security.MainModule)Session["ModuleMain"];
            if (mModuleMain == null) Response.Redirect(Utils.GetApplicationPath(Request) + "/Default.aspx");
            return mModuleMain;
        }
    }

    //Credits.Client IMasterItems.ClientObject
    //{
    //    get { return (Credits.Client)ViewState["ClientObject"]; }        
    //    set { ViewState["ClientObject"] = value; }
    //}

    bool IMasterItems.PermissionAllowed(string moduleName, string domainName, Constants.Constants.Classifiers permission)
    {
        bool result = false;

        Security.User user = ((IMasterItems)this).UserObject;
        if (user != null)
        {
            try
            {
                result = user.PermissionAllowed(moduleName, domainName, (int)permission);
            }
            catch (Exception excp)
            {
                Utils.InfoText(this.Page, "Atentie, Erroare in sistem!", excp.Message);
            }
        }

        return result;
    }

    bool IMasterItems.AutentificatedUser 
    {
        get
        {
            bool resulAutentification = false;
            Security.User userOBJ = (Security.User)Session["UserObject"];

            if (userOBJ != null && userOBJ.UserID != 0)
                resulAutentification = true;

            return resulAutentification;
        }    
    }

    #endregion IMaster Proprietes
    

    protected void Page_Load(object sender, EventArgs e)
    {        
        navMainMenu = Session["MenuObject"]!= null ? (string)Session["MenuObject"]:string.Empty;
    }  

    protected void LogInLogOutLinkButton_Load(object sender, EventArgs e)
    {
        if (Utils.GetMaster(this.Page).AutentificatedUser)
        {
            string firstName = Utils.GetMaster(this.Page).UserObject.FirstName;
            string lastName = Utils.GetMaster(this.Page).UserObject.LastName;
            LogInLogOutLinkButton.Text = firstName + " " + lastName + " | Log OUT";
            LogInLogOutLinkButton.NavigateUrl = "~/Default.aspx?action=logout";
        }
        else
        {
            LogInLogOutLinkButton.Text = "Log IN";
            LogInLogOutLinkButton.NavigateUrl = "~/Default.aspx?area=login";
        }
    }

    void IMasterItems.PerformPreloadActions(string currentModuleId, string pageName)
    {
        mCurrentModule = currentModuleId;

        string moduleDescription = Utils.GetMaster(this.Page).ModuleSecurity.GetModuleDescriptionById(mCurrentModule);
        string pageDescription = !string.IsNullOrEmpty(moduleDescription) ? moduleDescription : "Self-service platform";

        this.Page.Title = pageName + (!string.IsNullOrEmpty(pageDescription) && !string.IsNullOrEmpty(pageName) ? " - " : string.Empty) + pageDescription;
    }
    
    protected string WriteAppPath()
    {
        string appPath = Utils.GetApplicationPath(Request);
        return appPath;
    }
}
