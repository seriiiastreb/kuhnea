using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Data;
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page
{
    private readonly string mCurrentModule = Security.MainModule.ID;
    private readonly string mPageName = "Pagina de start";
    private string appPath = string.Empty;
    
    private void ShowPanel(string panelName)
    {
        #region Hide panels
        loginPanel.Visible = false;
        mainPagePanel.Visible = false;
        emptyPanel.Visible = false;
        #endregion Hide panels

        try
        {
            switch (panelName)
            {
                #region Login Panel

                case "loginPanel":
                    loginPanel.Visible = true;
                    break;

                case "mainPagePanel":
                    mainPagePanel.Visible = true;

                    break;

                #endregion Login Panel

                default:
                    emptyPanel.Visible = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            Utils.InfoText(this, "Atentie! Eroare in sistem!", ex.Message);
        }
    }
    
    protected string WriteAppPath()
    {
        string appPath = Utils.GetApplicationPath(Request);
        return appPath;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        appPath = Utils.GetApplicationPath(Request);

        #region LogOut Action
        if (Request["action"] != null)
        {
            string action = Request["action"].ToString();

            switch (action)
            {
                case "logout":
                    Session["UserObject"] = null;
                    Session["ModuleCredits"] = null;
                    Session["ServerObject"] = null;
                    Response.Redirect(appPath + "/Default.aspx", true);

                    Session["MenuObject"] = GenerateNavigationMenu(false);

                    break;
                default:
                    break;
            }
        }
        #endregion LogOut ACtion
        
        string areaGUID = string.Empty;
        if (Request["area"] != null)
        {
            areaGUID = Request["area"].ToString();
        }

        #region Display Panels depending of area
                
        if (!IsPostBack)
        {
            string areaName = areaGUID;

            switch (areaName)
            {
                case "login":
                    ShowPanel(loginPanel.ID);
                    Session["MenuObject"] = GenerateNavigationMenu(false);

                    break;

                default:

                    if (Utils.GetMaster(this).AutentificatedUser)
                    {   
                        Credits.Module moduleCredits = Utils.GetMaster(this.Page).ModuleCredits;

                        if (moduleCredits != null)
                        {
                            DataTable clientsDT = moduleCredits.GetClientlist();
                            clientFilter.DisplayValueField = "Client Full Name";
                            clientFilter.DataValueFiled = "clientID";
                            clientFilter.DataSource = clientsDT;
                            clientFilter.DataBind();

                            DataTable paysDT = moduleCredits.GetLoansListForFilter();
                            paysFilterControl.DisplayValueField = "clentName";
                            paysFilterControl.DataValueFiled = "loanID";
                            paysFilterControl.DataSource = paysDT;
                            paysFilterControl.DataBind();
                        }    

                        newClientHyperLink.NavigateUrl = appPath + "/ModuleCredits/Client.aspx?clid=n";
                        newTrainingHyperLink.NavigateUrl = appPath + "/ModuleCredits/Credits.aspx?cd=n";
                        ShowPanel(mainPagePanel.ID);

                        Session["MenuObject"] = GenerateNavigationMenu(true);

                        FillRaportDatorii();

                        changeRateDateLabel.Text = DateTime.Now.ToString("dd.MM.yyyy");

                        Utils.GetMaster(this).ModuleCredits.CheckChangeRateForAllDays();

                        cursEUROTextBox.Text = Utils.GetMaster(this).ModuleCredits.GetCurrencyRateByDate(DateTime.Now.Date, (int)Constants.Constants.CurrencyList.EURO).ToString();
                        cursUSDTextBox.Text = Utils.GetMaster(this).ModuleCredits.GetCurrencyRateByDate(DateTime.Now.Date, (int)Constants.Constants.CurrencyList.USD).ToString();
                    }
                    else
                    {

                        Session["MenuObject"] = GenerateNavigationMenu(false);
                        ShowPanel(emptyPanel.ID);
                    }
                    break;
            }
        }

        if (Utils.GetMaster(this).AutentificatedUser)
        {
            string parReport = Server.UrlEncode(Crypt.Module.CreateEncodedString(Constants.Constants.ReportNames.ParReportName));
            RPT_PAR_HyperLink.NavigateUrl = appPath + "/ModuleCredits/Reports.aspx?rpt=" + parReport;

            string lopipReport = Server.UrlEncode(Crypt.Module.CreateEncodedString(Constants.Constants.ReportNames.ListOfPaymentsInSelectedPeriodReportName));
            RPTListOfPaymentsInPeriodHyperLink.NavigateUrl = appPath + "/ModuleCredits/Reports.aspx?rpt=" + lopipReport;

            string lpoclReport = Server.UrlEncode(Crypt.Module.CreateEncodedString(Constants.Constants.ReportNames.LoanPartOfCreditsReportName));
            RPTLoansPartOfCreditLinesHyperLink.NavigateUrl = appPath + "/ModuleCredits/Reports.aspx?rpt=" + lpoclReport;

            string persReport = Server.UrlEncode(Crypt.Module.CreateEncodedString(Constants.Constants.ReportNames.ClientPersonalDataReportName));
            RPTClientPersonalReportHyperLink.NavigateUrl = appPath + "/ModuleCredits/Reports.aspx?rpt=" + persReport;

            string credtAcord = Server.UrlEncode(Crypt.Module.CreateEncodedString(Constants.Constants.ReportNames.ImprumuturiAcordateReportName));
            RPTListaCreditelorAcordateHyperLink.NavigateUrl = appPath + "/ModuleCredits/Reports.aspx?rpt=" + credtAcord;

            string consultEval = Server.UrlEncode(Crypt.Module.CreateEncodedString(Constants.Constants.ReportNames.ConsultariEvaluariReportName));
            RPTConsulEvalHyperLink.NavigateUrl = appPath + "/ModuleCredits/Reports.aspx?rpt=" + consultEval;
        }

        #endregion Display Panels depending of area
    }

    private void FillRaportDatorii()
    {
        int countDebit = 0;
        DataTable clientsDT = Utils.GetMaster(this.Page).ModuleCredits.GetLoansListDebits();
        if (clientsDT != null)
        {
            for (int i = 0; i < clientsDT.Rows.Count; i++)
            {
                int dayDelay = (int)clientsDT.Rows[i]["DaysDelay"];
                if (dayDelay >= 0)
                {
                    clientsDT.Rows[i].Delete();
                }
            }
            clientsDT.AcceptChanges();
            countDebit = clientsDT.Rows.Count;
        }
        countDebitLabel.Text = countDebit.ToString();
        creditListGridView.DataSource = clientsDT;
        creditListGridView.DataBind();
    }

    protected void creditListGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = creditListGridView.SelectedRow;
        Response.Redirect(appPath + "/ModuleCredits/refunds.aspx?loanID=" + row.Cells[0].Text);
    }

    protected void creditListGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            for (int i = 0; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.creditListGridView, "Select$" + e.Row.RowIndex);
            }
        }
    }

    protected void ClientSelecetd_Event(object sender, FilterWindow.FilterWindowEventsArg e)
    {
        string selectedClietID = e.SelectedItems.Count == 1 ? e.SelectedItems[0] : string.Empty;
        Response.Redirect(appPath + "/ModuleCredits/Client.aspx?clid=" + selectedClietID);
    }

    protected void loanSelectionDialog_OnClientSelected(object sender, LoanSelectionDialog.FilterWindowEventsArg e)
    {
        int selectedLoanID = e.SelectedItem != 0 ? e.SelectedItem : 0;
        Response.Redirect(appPath + "/ModuleCredits/Credits.aspx?cd=" + selectedLoanID);
    }

    protected void loanSelectionButton_Click(object sender, EventArgs e)
    {
        loanSelectionDialog.BindDataSource();
        loanSelectionDialog.Show();
    }

    protected void paysSelected_Event(object sender, FilterWindow.FilterWindowEventsArg e)
    {
        string selectedClietID = e.SelectedItems.Count == 1 ? e.SelectedItems[0] : string.Empty;
        Response.Redirect(appPath + "/ModuleCredits/refunds.aspx?loanID=" + selectedClietID);  
    }

    protected void currencyRateUpdateButton_Click(object sender, EventArgs e)
    {
        try
        {
            bool allowHere = Utils.GetMaster(this).PermissionAllowed(mCurrentModule, Security.Domains.BasicProgramAdministration.Name, Constants.Constants.Classifiers.Permissions_Edit);
            if (allowHere)
            {
                DateTime date = DateTime.Now.Date;
                decimal euroRate = Crypt.Utils.MyDecimalParce(cursEUROTextBox.Text.Trim());
                decimal usdRate = Crypt.Utils.MyDecimalParce(cursUSDTextBox.Text.Trim());

                if (!Utils.GetMaster(this).ModuleCredits.UpdateCurrencyRate(date, (int)Constants.Constants.CurrencyList.EURO, euroRate))
                { Utils.InfoText(this, "Eroare de salvare!", "Rata de schimb pentru EURO nu a fost salvata."); }

                if (!Utils.GetMaster(this).ModuleCredits.UpdateCurrencyRate(date, (int)Constants.Constants.CurrencyList.USD, usdRate))
                { Utils.InfoText(this, "Eroare de salvare!", "Rata de schimb pentru EURO nu a fost salvata."); }
            }
        }
        catch (Exception ex)
        {
            Utils.InfoText(this, "Atentie! Eroare in sistem!", ex.Message);
        }
    }

    protected void login_Ok_Button_Click(object sender, EventArgs e)
    {
        try
        {
            string username = userNameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            Security.User mUserObject = Security.User.Login(username, password);
            
            if (mUserObject != null && mUserObject.UserID != 0)
            {            
                Session["UserObject"] = mUserObject;
                Session["ModuleCredits"] = new Credits.Module();
                Session["ServerObject"] = new ServerObject();
                Session["ModuleSecurity"] = new Security.Module();
                Session["ModuleMain"] = new Security.MainModule();
                Session["ModuleAccounting"] = new Accounting.Module();
                Session["MenuObject"] = GenerateNavigationMenu(true);

                Response.Redirect(appPath + "/Default.aspx", false);  
            }
            else
            {
                Session["MenuObject"] = GenerateNavigationMenu(false);
                Utils.InfoText(this, "USERNAME sau PAROLA nu este corecta", "In Baza de date nu s-a gasit astfel de USERNAME, sau dumneavoastra ati gresit parola. Va rog verificati corectitudinea datelor introduse!");
            }
        }
        catch (Exception ex)
        {
            Utils.InfoText(this, "Atentie! Eroare in sistem!", ex.Message);
        }
    }

    protected void login_Cancel_Button_Click(object sender, EventArgs e)
    {
        ShowPanel(emptyPanel.ID);
    }

    public string GenerateNavigationMenu(bool autentificatedUser)
    {
        string resultMenu = string.Empty;

        if (autentificatedUser)
        {
            resultMenu = GetMenuObject();
        }
        else
        {
            DataTable submenu = new DataTable();
            submenu.Columns.Add("Link", typeof(string));
            submenu.Columns.Add("Name", typeof(string));

            submenu.Clear();
            submenu.AcceptChanges();
            submenu.Rows.Add(appPath + "/default.aspx?area=contacts", "Contacte");
            submenu.Rows.Add(appPath + "/default.aspx?area=history", "Istoria Companiei");
            submenu.Rows.Add(appPath + "/default.aspx?area=job", "Cariera Personala");

            resultMenu += "<li>";
            resultMenu += "<a>DESPRE COMPANIE</a>";
            resultMenu += GenerateSubmenu(submenu);
            resultMenu += "</li>";

            resultMenu += "<li>";
            resultMenu += "<a href=\"" + appPath + "/ModuleMain/RecoveryPWD.aspx\">RESTABILIREA PAROLIE</a>";
            resultMenu += "</li>";
        }

        return resultMenu;
    }

    private string GetMenuObject()
    {
        string resultMenu = string.Empty;

        ServerObject mServerObject = new ServerObject();

        DataTable sourceTable = mServerObject.UniversalGetFromSingleTable("MenuList", "*", " menuID >1 and isB=0 and enabled = 'true'");

        resultMenu += "<li>";
        resultMenu += "<a href=\"" + appPath + "/default.aspx\">HOME</a>";
        resultMenu += "</li>";

        if (sourceTable != null && sourceTable.Rows.Count > 0)
        {
            resultMenu += GenerateSubmenuByParentID(sourceTable, 1);
        }

        return resultMenu;
    }

    private string GenerateSubmenuByParentID(DataTable inputTable, int parentID)
    {
        string result = string.Empty;

        string link = string.Empty;
        ServerObject mServerObject = new ServerObject();
        string moduleDirectory = string.Empty;

        string linkEx = ".aspx";
        string linkNote = string.Empty;
        string linkToken = string.Empty;

        DataRow[] selectedRows = inputTable.Select("parentID=" + parentID);

        for (int indexRow = 0; indexRow < selectedRows.Length; indexRow++)
        {
            moduleDirectory = mServerObject.UniversalGetFromSingleTable("Classifiers", " Name ", " code = " + selectedRows[indexRow]["ClassifierID_46"].ToString()).Rows[0][0].ToString();
            if (moduleDirectory.Equals("root"))
            {
                moduleDirectory = string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(moduleDirectory))
                {
                    moduleDirectory = moduleDirectory + "/";
                }
            }

            link = selectedRows[indexRow]["link"].ToString();
            if (!link.Equals("#"))
            {
                link = link + linkEx;
            }

            linkToken = selectedRows[indexRow]["token"].ToString();
            if (!string.IsNullOrEmpty(linkToken))
            {
                link = link + "?area=" + linkToken;
            }

            linkNote = selectedRows[indexRow]["Note"].ToString();
            if (!string.IsNullOrEmpty(linkNote))
            {
                linkNote = " title =\"" + linkNote + "\"";
            }

            string subItem = GenerateSubmenuByParentID(inputTable, int.Parse(selectedRows[indexRow]["menuid"].ToString()));

            result += "<li>";

            result += "<a href=\"" + appPath + "/" + moduleDirectory + link + "\" " + linkNote + ">";

            result += selectedRows[indexRow]["Name"].ToString();

            result += "</a>";

            if (!string.IsNullOrEmpty(subItem))
            {
                subItem = "<ul>" + subItem + "</ul>";
            }

            result += subItem + "</li>";
        }
        return result;
    }

    private string GenerateSubmenu(DataTable inputTable)
    {
        string result = string.Empty;

        result = "<ul>";

        if (inputTable != null)
        {
            for (int i = 0; i < inputTable.Rows.Count; i++)
            {
                result += "<li> " + "<a href=\"" + inputTable.Rows[i]["Link"].ToString() + "\">" + inputTable.Rows[i]["Name"].ToString() + "</a> </li>";
            }
        }

        result += "</ul>";

        return result;
    }


}
