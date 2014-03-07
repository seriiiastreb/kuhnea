using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccessDenied : System.Web.UI.Page
{
    private readonly string mCurrentModule = string.Empty;
    private readonly string mPageName = "Security restrictions applied";

    protected void Page_Load(object sender, EventArgs e)
    {
        Utils.GetMaster(this).PerformPreloadActions(mCurrentModule, mPageName);

        //Utils.InfoText(this,"","Access denied. Not enough priviledges to access the required resource.");
    }
}