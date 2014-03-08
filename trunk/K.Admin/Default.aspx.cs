using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Data;
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
   
    protected void login_Ok_Button_Click(object sender, EventArgs e)
    {
        try
        {
            string username = userNameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            Service.KService ServiceBus = new Service.KService();
            Service.User userObject = ServiceBus.Login(username, password);

            if (userObject != null && userObject.UserID != 0)
            {
                Session["UserObject"] = userObject;
                Session["ServiceBus"] = ServiceBus;

            //    Session["ModuleCredits"] = new Credits.Module();
            //    Session["ServerObject"] = new ServerObject();
            //    Session["ModuleSecurity"] = new Security.Module();
            //    Session["ModuleMain"] = new Security.MainModule();
            //    Session["ModuleAccounting"] = new Accounting.Module();
            //    Session["MenuObject"] = GenerateNavigationMenu(true);

            //    Response.Redirect(appPath + "/Default.aspx", false);  
            }
            else
            {
            //    Session["MenuObject"] = GenerateNavigationMenu(false);
            //    Utils.InfoText(this, "USERNAME sau PAROLA nu este corecta", "In Baza de date nu s-a gasit astfel de USERNAME, sau dumneavoastra ati gresit parola. Va rog verificati corectitudinea datelor introduse!");
            }
        }
        catch (Exception ex)
        {
            //Utils.InfoText(this, "Atentie! Eroare in sistem!", ex.Message);
        }
    }

    protected void login_Cancel_Button_Click(object sender, EventArgs e)
    {
    }  

}
