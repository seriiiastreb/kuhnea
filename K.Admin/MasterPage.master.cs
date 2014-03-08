using System;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage, IMasterItems
{
    public string navMainMenu = string.Empty;
    public string leftSubmenu = string.Empty;
    string mCurrentModule = string.Empty;

    #region IMaster Proprietes  
   
    #endregion IMaster Proprietes
    

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LogInLogOutLinkButton_Load(object sender, EventArgs e)
    {

    }


   
}
