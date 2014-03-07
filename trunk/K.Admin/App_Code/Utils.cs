using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using System.Collections;
using System.Globalization;
using System.Drawing;

public class Utils
{
    public static string arrayColumnName = "ColumnName";
    public static string arrayIsVisible = "isVisible";
    public static string arrayAlias = "Alias";
    public static string arrayColumnIndex = "ColumnIndex";
    public static string arrayObjects = "Object";
    
    public static string matrixColumnName = "ColumnName";
    public static string matrixColumnIndex = "ColumnIndex";
    public static string matrixObjectType = "ObjectType";
    public static string matrixAliasName = "Alias";
    public static string matrixIsVisible = "isVisible";
    public static string matrixTableName = "TableName";
    public static string matrixTableKeyID = "TableKeyID";
    public static string matrixTableColumnName = "TableColumnName";

    public static object hashtableKeyInsertColumns = "columnsKey";
    public static object hashtableKeyInsertValues = "valuesKey";
    public static object hashtableKeyInsertParamKeys = "paramsKeysKey";
    public static object hashtableKeyInsertParamValue = "paramsValueKey";
    public static object hashtableKeyInsertHashtable = "hashtableKey";

    public static char charObjDelimitier = char.Parse(Constants.Constants.objectDelimiter);

    public Utils()
    {
    }

    #region Email Settings

    public static string EmailTemplate(string toEmail, string[] replaceFields, string[] fieldsArray, string legalEntity, DataTable templateTable, int languageID, int event_type)
    {
        string response = string.Empty;
        try
        {
            if (templateTable != null && templateTable.Rows.Count == 1)
            {
                string emailSubject = templateTable.Rows[0]["subject"].ToString();
                string emailTemplate = templateTable.Rows[0]["text"].ToString();
                if (emailTemplate != null && emailTemplate.Length > 0)
                {
                    if (fieldsArray != null && fieldsArray.Length > 0)
                    {
                        for (int indexKey = 0; indexKey < fieldsArray.Length; indexKey++)
                        {
                            emailTemplate = emailTemplate.Replace("[" + fieldsArray[indexKey].Trim() + "]", replaceFields[indexKey]);
                        }
                    }
                }

                response = EmailSend(toEmail, emailTemplate, emailSubject);
            }
        }
        catch (Exception e)
        {
            response = "Error sending mail: Exception!!!" + e.Message;
        }
        return response;
    }

    public static string EmailSend(string toEmail, string emailTemplate, string emailSubject)
    {
        string response = string.Empty;

        try
        {
            string to = toEmail;
            string smtpHost = ConfigurationManager.AppSettings["smtpHost"];

            string emailFrom = ConfigurationManager.AppSettings["emailFrom"];

            if (!string.IsNullOrEmpty(emailFrom.Trim()))
            {

                string fromDisplay = ConfigurationManager.AppSettings["fromDisplay"];
                string smtpUser = ConfigurationManager.AppSettings["smtpUser"];
                string password = ConfigurationManager.AppSettings["smtpPassword"];

                int smtpPort = 25;
                int.TryParse(ConfigurationManager.AppSettings["smtpPort"], out smtpPort);
                if (smtpPort == 0)
                {
                    smtpPort = 25;
                }

                string body = @"<html><body><div style=""font-family:arial;font-size:12pt;"">";

                if (emailTemplate.Trim() != string.Empty)
                {
                    body += "<pre>";
                    body += emailTemplate;
                    body += "</pre>";
                    body += @"</div></body></html>";

                    // AuthTypes: "Basic", "NTLM", "Digest", "Kerberos", "Negotiate"
                    string authType = "Basic";
                    string bccTo = string.Empty;
                    string copyTo = string.Empty;

                    response = SendEmail(emailFrom, fromDisplay, to, emailFrom, emailFrom, copyTo, bccTo, emailSubject,
                        body, System.Text.Encoding.ASCII, System.Text.Encoding.UTF8,
                        true, smtpHost, smtpUser, password, smtpPort, authType);
                }
                else
                {
                    response = "Unable to send to an empty email address!";
                }
            }
            else
            {
                response = "Error. Empty address in from field.";
            }
        }
        catch (Exception e)
        {
            response = "Error sending mail: Exception!!!" + e.Message;
        }

        return response;
    }

    static string SendEmail(string from, string fromDisplay, string to, string sender,
       string replyTo, string copyTo, string bccTo, string subject, string body, System.Text.Encoding subjectEncoding,
       System.Text.Encoding bodyEncoding, bool isBodyHtml, string smtpHost, string smtpUser,
       string smtpPass, int smtpPort, string smtpAuthType)
    {
        string response = null;
        System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress(from, fromDisplay);
        System.Net.Mail.MailAddress toAddress = new System.Net.Mail.MailAddress(to);
        System.Net.Mail.MailAddress senderAddress = new System.Net.Mail.MailAddress(sender);
        System.Net.Mail.MailAddress replyToAddress = new System.Net.Mail.MailAddress(replyTo);
        System.Net.Mail.MailMessage emailMessage = new System.Net.Mail.MailMessage(fromAddress, toAddress);

        if (!copyTo.Equals(string.Empty))
        {
            System.Net.Mail.MailAddress copyToAddress = new System.Net.Mail.MailAddress(copyTo);
            emailMessage.CC.Add(copyToAddress);
        }

        if (!bccTo.Equals(string.Empty))
        {
            System.Net.Mail.MailAddress bccToAddress = new System.Net.Mail.MailAddress(bccTo);
            emailMessage.Bcc.Add(bccToAddress);
        }

        emailMessage.Body = body;
        emailMessage.Sender = senderAddress;
        emailMessage.ReplyTo = replyToAddress;
        emailMessage.SubjectEncoding = subjectEncoding;
        emailMessage.BodyEncoding = bodyEncoding;
        emailMessage.Subject = subject;
        emailMessage.IsBodyHtml = isBodyHtml;
        emailMessage.Priority = System.Net.Mail.MailPriority.High;

        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpHost);
        bool enableSsl = true;
        bool.TryParse(ConfigurationManager.AppSettings["EnableSsl"], out enableSsl);

        smtp.EnableSsl = enableSsl;
        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

        // Note: on GoDaddy host the creadentials are unnecessary
        bool withCredentials = false;
        bool.TryParse(ConfigurationManager.AppSettings["smtpUseCredentials"], out withCredentials);
        if (withCredentials)
        {
            smtp.UseDefaultCredentials = false;
            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(smtpUser, smtpPass);
            smtp.Credentials = (System.Net.ICredentialsByHost)networkCredential.GetCredential(smtpHost, smtpPort, smtpAuthType);
        }

        try
        {
            smtp.Send(emailMessage);
        }
        catch (System.Net.Mail.SmtpException smtpException)
        {
            response = smtpException.Message;
            response = response.Replace("\r\n", "");
            response = response.Replace("\"", "");
            response = response.Insert(0, "SMTP Agent Error: ");
        }
        catch (Exception ex)
        {
            response = ex.Message;
            response = response.Replace("\r\n", "");
            response = response.Replace("\"", "");
            response = response.Insert(0, "Error: ");
        }
        return response;
    }



    #endregion Email Settings

    public static TreeNode FindNode(TreeNode treenode, string name)
    {
        // Ищем в узлах первого уровня.
        foreach (TreeNode tn in treenode.ChildNodes)
        {
            // Если нашли,
            if (tn.Value == name) { return tn; }  // то возвращаем.

        }
        // Ищем в подузлах.
        TreeNode node;
        foreach (TreeNode tn in treenode.ChildNodes)
        {
            // Делаем поиск в узлах.
            node = FindNode(tn, name);
            // Если нашли,
            if (node != null) { return node; } // то возвращаем.
        }

        return null;
    }

    public static TreeNode FindNode(TreeView tv, string name)
    {
        // Ищем в узлах первого уровня.
        foreach (TreeNode tn in tv.Nodes)
        {
            if (tn.Value == name) { return tn; } // то возвращаем.
        }
        // Ищем в подузлах.
        TreeNode node;
        foreach (TreeNode tn in tv.Nodes)
        {
            // Делаем поиск в узлах.
            node = FindNode(tn, name);
            // Если нашли,
            if (node != null) { return node; } // то возвращаем.
        }

        return null;
    }

    public static void FillSelector(DropDownList destinationDDl, DataTable inputTable, string displayTextField, string valueField)
    {
        try
        {
            if (inputTable == null)
            {
                inputTable = new DataTable();
                inputTable.Columns.Add(valueField, typeof(string));
                inputTable.Columns.Add(displayTextField, typeof(string));

                inputTable.NewRow();
                inputTable.Rows.Add();
                inputTable.Rows[0][valueField] = "0";
                inputTable.Rows[0][displayTextField] = "**";
            }

            if (destinationDDl != null && inputTable != null)
            {
                destinationDDl.DataSource = null;
                destinationDDl.DataSource = inputTable;
                destinationDDl.DataValueField = valueField;
                destinationDDl.DataTextField = displayTextField;
                destinationDDl.DataBind();
            }
        }
        catch { }
    }

    public static IMasterItems GetMaster(System.Web.UI.Page page)
    {
        IMasterItems myMaster = (IMasterItems)page.Master;
        return myMaster;
    }

    public static void InfoText(System.Web.UI.Page inputPage, string messageTitle, string message)
    {
        Label infoWindowTitleLabel = (Label)inputPage.Master.FindControl("infoWindowTitleLabel");
        ToolkitScriptManager toolkitScriptManager1 = (ToolkitScriptManager)inputPage.Master.FindControl("ToolkitScriptManager1");

        if (infoWindowTitleLabel != null)
        {
            if (toolkitScriptManager1.IsInAsyncPostBack)
            {
                toolkitScriptManager1.RegisterDataItem(infoWindowTitleLabel, messageTitle);
            }
            else
            { infoWindowTitleLabel.Text = messageTitle; }
        }

        Label infoWindowMessageLabel = (Label)inputPage.Master.FindControl("infoWindowMessageLabel");
        if (infoWindowMessageLabel != null)
        {
            if (toolkitScriptManager1.IsInAsyncPostBack)
            {
                toolkitScriptManager1.RegisterDataItem(infoWindowMessageLabel, message);
            }
            else
            {
                infoWindowMessageLabel.Text = message;
            }
        }

        ModalPopupExtender infoLabelMaster = (ModalPopupExtender)inputPage.Master.FindControl("infoWindowPopupExtender");
        if (infoLabelMaster != null)
        {
            infoLabelMaster.Show();
        }
    }

    public static string GetApplicationPath(HttpRequest Request)
    {
        string appPath = Request.ApplicationPath;

        if (appPath.Length == 1 && appPath.Equals("/"))
        {
            appPath = string.Empty;
        }

        return appPath;
    }

    public static bool ContentTypeAllowed(string contentType)
    {
        bool result = false;

        contentType = contentType.ToLower();

        if (contentType.Equals(Constants.Constants.ContentType.DOC)
                        || contentType.Equals(Constants.Constants.ContentType.DOCX)
                        || contentType.Equals(Constants.Constants.ContentType.ODS)
                        || contentType.Equals(Constants.Constants.ContentType.ODT)
                        || contentType.Equals(Constants.Constants.ContentType.PDF)
                        || contentType.Equals(Constants.Constants.ContentType.RTF)
                        || contentType.Equals(Constants.Constants.ContentType.TXT)
                        || contentType.Equals(Constants.Constants.ContentType.CSV)
                        || contentType.Equals(Constants.Constants.ContentType.XLS)
                        || contentType.Equals(Constants.Constants.ContentType.XLSX)
                        || contentType.Equals(Constants.Constants.ContentType.ZIP)
                        || contentType.Equals(Constants.Constants.ContentType.ZIPX)
                        || contentType.Equals(Constants.Constants.ContentType.ZIPCompressed)
                        || contentType.Equals(Constants.Constants.ContentType.ZIPCompressedX)
                        || contentType.Equals(Constants.Constants.ContentType.ZIPMultipart)
                        || contentType.Equals(Constants.Constants.ContentType.ZIPMultipartX)
                        || contentType.Equals(Constants.Constants.ContentType.DownloadX)
                        || contentType.Equals(Constants.Constants.ContentType.Download)
                        || contentType.Equals(Constants.Constants.ContentType.PNG)
                        || contentType.Equals(Constants.Constants.ContentType.JPEGP)
                        || contentType.Equals(Constants.Constants.ContentType.JPG)
                        || contentType.Equals(Constants.Constants.ContentType.JPEG)
                            )
        {
            result = true;
        }

        return result;
    }

    #region ConfiPanel

    #region General Function

    public static void EnableButtons(DataTable configTable, Button buttonAdd, Button buttonEdit, Button buttonDelete, GridView sourceGridView)
    {
        if (configTable != null && configTable.Rows.Count == 1)
        {
            #region newButton
            if (configTable.Rows[0]["csvInsert"].ToString().Length > 0)
            {
                buttonAdd.Visible = true;
            }
            else
            {
                buttonAdd.Visible = false;
            }
            #endregion newButton

            DataTable sourceTable = (DataTable)sourceGridView.DataSource;
            if (sourceTable != null && sourceTable.Rows.Count > 0)
            {
                #region editButton
                if (configTable.Rows[0]["csvUpdate"].ToString().Length > 0)
                {
                    buttonEdit.Visible = true;
                }
                else
                {
                    buttonEdit.Visible = false;
                }
                #endregion editButton

                #region deleteButton
                if (configTable.Rows[0]["csvSelect"].ToString().Length > 0)
                {
                    buttonDelete.Visible = true;
                }
                else
                {
                    buttonDelete.Visible = false;
                }
                #endregion deleteButton
            }
            else
            {
                buttonEdit.Visible = false;
                buttonDelete.Visible = false;
            }
        }
        else
        {
            buttonAdd.Visible = false;
            buttonEdit.Visible = false;
            buttonDelete.Visible = false;
        }
    }

    public static DataTable GetArrayTable(string selectTableName, string namePanel, string operationName)
    {
        DataTable arrayTable = new DataTable();
        arrayTable.Columns.Add(arrayColumnName, typeof(string));
        arrayTable.Columns.Add(arrayIsVisible, typeof(bool));
        arrayTable.Columns.Add(arrayAlias, typeof(string));
        arrayTable.Columns.Add(arrayColumnIndex, typeof(int));
        arrayTable.Columns.Add(arrayObjects, typeof(string));
        arrayTable.NewRow();

        ServerObject mServerObject = new ServerObject();

        //get columns by TableName
        DataTable columnsList = mServerObject.GetColumnsByTableName(selectTableName);

        //get csvSelect from ConfigPanel by TableName
        DataTable configTable = mServerObject.GetConfigPanelByPanelName(namePanel);

        //get csvClassifier, csvAlias, csvSelect
        string csvObject = string.Empty;
        string csvAlias = string.Empty;
        string csvOperation = string.Empty;

        if (configTable != null && configTable.Rows.Count == 1)
        {
            csvObject = configTable.Rows[0][Constants.Constants.ConfigPanel.csvObjects].ToString();
            csvAlias = configTable.Rows[0][Constants.Constants.ConfigPanel.csvAlias].ToString();
            csvOperation = configTable.Rows[0][operationName].ToString();
        }

        List<string> operationList = new List<string>(csvOperation.Split(','));
        
        List<string> listObject = new List<string>(csvObject.Split('|'));
        
        for (int indexRow = 0; indexRow < columnsList.Rows.Count; indexRow++)
        {
            string columnName = columnsList.Rows[indexRow][Constants.Constants.ConfigPanel.column_name].ToString();

            bool isVisible = false;
            int indexColumn = 0;

            if (csvOperation.Contains(columnName))
            {
                isVisible = true;
                for (int indexList = 0; indexList < operationList.Count; indexList++)
                {
                    if (operationList[indexList].Equals(columnName))
                    {
                        indexColumn = (indexList + 1) * 10;
                    }
                }
            }

            string typeObject = "TextBox";
            #region for Table
            string tableName = string.Empty;
            string tableKeyID = string.Empty;
            string tableColumnName = string.Empty;
            #endregion

            #region for Classifier
            //only tableName exists
            #endregion for Classifier

            if (listObject.Contains(columnName))
            {
                for (int indexList = 0; indexList < listObject.Count; indexList++)
                {
                    List<string> subListObject = new List<string>(listObject[indexList].Split('='));
                    if (subListObject != null)
                    {
                        if (subListObject[0].Trim().Equals(columnName))
                        {
                            for (int indexSubList = 0; indexSubList < subListObject.Count; indexSubList++)
                            {
                                List<string> listToolBox = new List<string>(subListObject[indexSubList].Split(','));
                                if (listToolBox != null)
                                {
                                    typeObject = listToolBox[0];

                                    switch (typeObject)
                                    {
                                        case Constants.Constants.ToolBoxConst.tbTable:
                                            tableName = listToolBox[1];
                                            tableKeyID = listToolBox[2];
                                            tableColumnName = listToolBox[3];
                                            break;
                                        case Constants.Constants.ToolBoxConst.tbClassifiers:
                                            tableName = listToolBox[1];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            string aliasValue = string.Empty;
            List<string> listAlias = new List<string>(csvAlias.Split(','));
            if (!listAlias[indexRow].Trim().Equals(columnName))
            {
                aliasValue = listAlias[indexRow].Trim();
            }

            switch (operationName)
            {
                case Constants.Constants.ConfigPanel.csvSelect:
                    #region case_for_select
                    switch (typeObject)
                    {
                        case Constants.Constants.ToolBoxConst.tbClassifiers:
                            #region Classifiers
                            string columnNameForClassifiers = "(SELECT Name FROM Classifiers WHERE Code = " + tableName + ")";
                            if (!string.IsNullOrEmpty(aliasValue))
                            {
                                columnNameForClassifiers += " AS \"" + aliasValue + "\"";
                            }
                            else
                            {
                                aliasValue = columnName;
                            }
                            arrayTable.Rows.Add(columnName, false, columnName, indexColumn, typeObject);
                            arrayTable.Rows.Add(columnNameForClassifiers, isVisible, aliasValue, indexColumn + 1, typeObject);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbTextBox:
                            #region TextBox
                            if (!string.IsNullOrEmpty(aliasValue))
                            {
                                columnName += " AS \"" + aliasValue + "\"";
                            }
                            else
                            {
                                aliasValue = columnName;
                            }
                            arrayTable.Rows.Add(columnName, isVisible, aliasValue, indexColumn, typeObject);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbCheckBox:
                            #region CheckBox
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbDateTime:
                            #region DateTime
                            if (!string.IsNullOrEmpty(aliasValue))
                            {
                                columnName += " AS \"" + aliasValue + "\"";
                            }
                            else
                            {
                                aliasValue = columnName;
                            }
                            arrayTable.Rows.Add(columnName, isVisible, aliasValue, indexColumn, typeObject);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbTable:
                            #region Table
                            string columnNameForTable = "(SELECT "+tableColumnName+" FROM "+tableName+" WHERE "+tableKeyID+" = " + columnName + ")";
                            if (!string.IsNullOrEmpty(aliasValue))
                            {
                                columnNameForTable += " AS \"" + aliasValue + "\"";
                            }
                            else
                            {
                                aliasValue = columnName;
                            }
                            //arrayTable.Rows.Add(columnName, false, columnName, indexColumn, typeObject);
                            arrayTable.Rows.Add(columnNameForTable, isVisible, aliasValue, indexColumn + 1, typeObject);
                            
                            #endregion
                            break;

                        default:
                            #region default
                            if (!string.IsNullOrEmpty(aliasValue))
                            {
                                columnName += " AS \"" + aliasValue + "\"";
                            }
                            else
                            {
                                aliasValue = columnName;
                            }
                            arrayTable.Rows.Add(columnName, isVisible, aliasValue, indexColumn, typeObject);
                            #endregion
                            break;
                    }                    
                    #endregion case_for_select
                    break;

                case Constants.Constants.ConfigPanel.csvInsert:
                    #region case_for_insert
                    arrayTable.Rows.Add(columnName, isVisible, aliasValue, indexColumn, typeObject);
                    #endregion case_for_insert
                    break;
            }


        }

        arrayTable.DefaultView.Sort = arrayColumnIndex + " ASC";
        arrayTable.AcceptChanges();

        arrayTable = arrayTable.DefaultView.ToTable();

        return arrayTable;
    }

    public static DataTable GetArrayTableForSelectOperation(string selectTableName, string namePanel)
    {
        #region create result table
        DataTable arrayTable = new DataTable();
        arrayTable.Columns.Add(arrayColumnName, typeof(string));
        arrayTable.Columns.Add(arrayIsVisible, typeof(bool));
        arrayTable.Columns.Add(arrayAlias, typeof(string));
        arrayTable.Columns.Add(arrayColumnIndex, typeof(int));
        arrayTable.Columns.Add(arrayObjects, typeof(string));
        arrayTable.NewRow();
        #endregion create result table

        ServerObject mServerObject = new ServerObject();

        //get columns by TableName
        //DataTable columnsList = mServerObject.GetColumnsByTableName(selectTableName);

        //get csvSelect from ConfigPanel by TableName
        DataTable configTable = mServerObject.GetConfigPanelByPanelName(namePanel);
        
        if (configTable != null && configTable.Rows.Count == 1)
        {
            #region Build MatrixTable

            DataTable matrixTable = new DataTable();
            matrixTable.Columns.Add(matrixColumnName, typeof(string));
            matrixTable.Columns.Add(matrixObjectType, typeof(string));
            matrixTable.Columns.Add(matrixAliasName, typeof(string));
            matrixTable.Columns.Add(matrixIsVisible, typeof(bool));
            matrixTable.Columns.Add(matrixColumnIndex, typeof(int));
            matrixTable.Columns.Add(matrixTableName, typeof(string));
            matrixTable.Columns.Add(matrixTableKeyID, typeof(string));
            matrixTable.Columns.Add(matrixTableColumnName, typeof(string));
            matrixTable.NewRow();
            //[ColumnName]
            //[ObjectType]
            //[AliasName]
            //[isVisible]
            //[TableName]
            //[TableKeyID]
            //[TableColumnName]

            #endregion Build MatrixTable
       
            //get csvClassifier, csvAlias, csvSelect
            string csvObject = string.Empty;
            string csvAlias = string.Empty;
            string csvColumnForSelect = string.Empty;

            csvObject           = configTable.Rows[0][Constants.Constants.ConfigPanel.csvObjects].ToString();
            csvAlias            = configTable.Rows[0][Constants.Constants.ConfigPanel.csvAlias].ToString();
            csvColumnForSelect  = configTable.Rows[0][Constants.Constants.ConfigPanel.csvSelect].ToString();
            
            List<string> showColumnList = new List<string>(csvColumnForSelect.Split(',')); //csvShowColumn

            List<string> listAlias = new List<string>(csvAlias.Split(','));

            List<string> listObject = new List<string>(csvObject.Split('|'));

            for (int indexList = 0; indexList < listObject.Count; indexList++)
            {
                matrixTable.Rows.Add();

                List<string> subListObject = new List<string>(listObject[indexList].Split('='));
                if (subListObject != null)
                {
                    List<string> listToolBox = new List<string>(subListObject[1].Split(charObjDelimitier));
                    if (listToolBox != null)
                    {
                        string typeObject = listToolBox[0];

                        matrixTable.Rows[indexList][matrixColumnName] = subListObject[0].Trim();
                        matrixTable.Rows[indexList][matrixObjectType] = typeObject;
                        matrixTable.Rows[indexList][matrixAliasName] = listAlias[indexList].Trim();

                        if (showColumnList.Contains(subListObject[0].Trim()))
                        {
                            matrixTable.Rows[indexList][matrixIsVisible] = true;
                            int columnIndex = showColumnList.IndexOf(subListObject[0].Trim());
                            matrixTable.Rows[indexList][matrixColumnIndex] = (columnIndex + 1) * 10;// (indexList + 1) * 10;
                        }
                        else
                        {
                            matrixTable.Rows[indexList][matrixIsVisible] = false;
                        }

                        switch (typeObject)
                        {
                            case Constants.Constants.ToolBoxConst.tbSql:
                                matrixTable.Rows[indexList][matrixTableName] = listToolBox[1];
                                break;

                            case Constants.Constants.ToolBoxConst.tbTable:
                                matrixTable.Rows[indexList][matrixTableName] = listToolBox[1];
                                matrixTable.Rows[indexList][matrixTableKeyID] = listToolBox[2];
                                matrixTable.Rows[indexList][matrixTableColumnName] = listToolBox[3];
                                break;

                            case Constants.Constants.ToolBoxConst.tbClassifiers:
                                matrixTable.Rows[indexList][matrixTableName] = listToolBox[1];
                                break;

                            case Constants.Constants.ToolBoxConst.tbCheckBox:
                                break;

                            case Constants.Constants.ToolBoxConst.tbDateTime:
                                break;

                            case Constants.Constants.ToolBoxConst.tbTextBox:
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            for (int indexRow = 0; indexRow < matrixTable.Rows.Count; indexRow++)
            {

                string typeObject = matrixTable.Rows[indexRow][matrixObjectType].ToString(); // "TextBox";

                string vizibleValue = matrixTable.Rows[indexRow][matrixIsVisible].ToString();
                bool visibled = true;
                bool.TryParse(vizibleValue, out visibled);

                string indexValue = matrixTable.Rows[indexRow][matrixColumnIndex].ToString();
                int indexColumn = 0;
                int.TryParse(indexValue, out indexColumn);

                string aliasColumn = matrixTable.Rows[indexRow][matrixAliasName].ToString();

                #region case_for_select
                switch (typeObject)
                {
                    case Constants.Constants.ToolBoxConst.tbClassifiers:
                        #region Classifiers
                        string columnNameForClassifiers = "(SELECT Name FROM Classifiers WHERE Code = " + matrixTable.Rows[indexRow][matrixColumnName].ToString() + ")";
                        if (!string.IsNullOrEmpty(aliasColumn))
                        {
                            columnNameForClassifiers += " AS \"" + aliasColumn + "\"";
                        }
                        else
                        {
                            aliasColumn = matrixTable.Rows[indexRow][matrixColumnName].ToString();
                        }
                        arrayTable.Rows.Add(matrixTable.Rows[indexRow][matrixColumnName].ToString()
                            , false
                            , matrixTable.Rows[indexRow][matrixColumnName].ToString()
                            , indexColumn
                            , typeObject);

                        arrayTable.Rows.Add(columnNameForClassifiers
                            , visibled
                            , aliasColumn
                            , indexColumn + 1
                            , typeObject);
                        #endregion
                        break;

                    case Constants.Constants.ToolBoxConst.tbTextBox:
                        #region TextBox
                        if (!string.IsNullOrEmpty(aliasColumn))
                        {
                            matrixTable.Rows[indexRow][matrixColumnName] += " AS \"" + aliasColumn + "\"";
                        }
                        else
                        {
                            aliasColumn = matrixTable.Rows[indexRow][matrixColumnName].ToString();
                        }
                        arrayTable.Rows.Add(matrixTable.Rows[indexRow][matrixColumnName]
                            , visibled
                            , aliasColumn
                            , indexColumn
                            , typeObject);
                        #endregion
                        break;

                    case Constants.Constants.ToolBoxConst.tbCheckBox:
                        #region CheckBox
                        #endregion
                        break;

                    case Constants.Constants.ToolBoxConst.tbDateTime:
                        #region DateTime
                        if (!string.IsNullOrEmpty(aliasColumn))
                        {
                            matrixTable.Rows[indexRow][matrixColumnName] += " AS \"" + aliasColumn + "\"";
                        }
                        else
                        {
                            aliasColumn = matrixTable.Rows[indexRow][matrixColumnName].ToString();
                        }
                        arrayTable.Rows.Add(matrixTable.Rows[indexRow][matrixColumnName]
                            , visibled
                            , aliasColumn
                            , indexColumn
                            , typeObject);
                        #endregion
                        break;

                    case Constants.Constants.ToolBoxConst.tbTable:
                        #region Table
                        string columnNameForTable = "(SELECT T1." + matrixTable.Rows[indexRow][matrixTableColumnName] 
                            + " FROM " + matrixTable.Rows[indexRow][matrixTableName] + " AS T1 "
                            + " WHERE T1." + matrixTable.Rows[indexRow][matrixTableKeyID]
                            + " = T0." + matrixTable.Rows[indexRow][matrixColumnName] + ")";
                        if (!string.IsNullOrEmpty(matrixTable.Rows[indexRow][matrixAliasName].ToString()))
                        {
                            columnNameForTable += " AS \"" + aliasColumn + "\"";
                        }
                        else
                        {
                            aliasColumn = matrixTable.Rows[indexRow][matrixColumnName].ToString();
                        }
                        //arrayTable.Rows.Add(columnName, false, columnName, indexColumn, typeObject);
                        arrayTable.Rows.Add(columnNameForTable
                            , visibled
                            , aliasColumn
                            , indexColumn + 1
                            , typeObject);

                        #endregion
                        break;

                    case Constants.Constants.ToolBoxConst.tbSql:
                        #region SQL
                        string columnNameForSQL = "(SELECT cast(T1.Code as varchar) +' - '+T1.Name "
                            + " FROM (" + matrixTable.Rows[indexRow][matrixTableName]
                            + " WHERE Code=T0." + matrixTable.Rows[indexRow][matrixColumnName] + ") AS T1)";
                        if (!string.IsNullOrEmpty(matrixTable.Rows[indexRow][matrixAliasName].ToString()))
                        {
                            columnNameForSQL += " AS \"" + aliasColumn + "\"";
                        }
                        else
                        {
                            aliasColumn = matrixTable.Rows[indexRow][matrixColumnName].ToString();
                        }
                        arrayTable.Rows.Add(columnNameForSQL, visibled, aliasColumn, indexColumn+1, typeObject);
                        #endregion SQL
                        break;

                    default:
                        #region default
                        if (!string.IsNullOrEmpty(aliasColumn))
                        {
                            matrixTable.Rows[indexRow][matrixColumnName] += " AS \"" + aliasColumn + "\"";
                        }
                        else
                        {
                            aliasColumn = matrixTable.Rows[indexRow][matrixColumnName].ToString();
                        }
                        arrayTable.Rows.Add(matrixTable.Rows[indexRow][matrixColumnName]
                            , visibled
                            , aliasColumn
                            , indexColumn
                            , typeObject);
                        #endregion
                        break;
                }
                #endregion case_for_select

            }

            arrayTable.DefaultView.Sort = arrayColumnIndex + " ASC";
            arrayTable.AcceptChanges();

            arrayTable = arrayTable.DefaultView.ToTable();
        
        }
        
        return arrayTable;
    }

    public static DataTable GetArrayTableForInsertOperation(string selectTableName, string namePanel)
    {
        #region Build MatrixTable

        DataTable matrixTable = new DataTable();
        matrixTable.Columns.Add(matrixColumnName, typeof(string));
        matrixTable.Columns.Add(matrixObjectType, typeof(string));
        matrixTable.Columns.Add(matrixAliasName, typeof(string));
        matrixTable.Columns.Add(matrixIsVisible, typeof(bool));
        matrixTable.Columns.Add(matrixColumnIndex, typeof(int));
        matrixTable.Columns.Add(matrixTableName, typeof(string));
        matrixTable.Columns.Add(matrixTableKeyID, typeof(string));
        matrixTable.Columns.Add(matrixTableColumnName, typeof(string));
        matrixTable.NewRow();
        #endregion Build MatrixTable

        ServerObject mServerObject = new ServerObject();

        //get columns by TableName
        //DataTable columnsList = mServerObject.GetColumnsByTableName(selectTableName);

        //get csvSelect from ConfigPanel by TableName
        DataTable configTable = mServerObject.GetConfigPanelByPanelName(namePanel);

        if (configTable != null && configTable.Rows.Count == 1)
        {
            //get csvClassifier, csvAlias, csvSelect
            string csvObject = string.Empty;
            string csvAlias = string.Empty;
            string csvColumnForInsert = string.Empty;

            csvObject = configTable.Rows[0][Constants.Constants.ConfigPanel.csvObjects].ToString();
            csvAlias = configTable.Rows[0][Constants.Constants.ConfigPanel.csvAlias].ToString();
            csvColumnForInsert = configTable.Rows[0][Constants.Constants.ConfigPanel.csvInsert].ToString();

            List<string> showColumnList = new List<string>(csvColumnForInsert.Split(',')); //csvShowColumn

            List<string> listAlias = new List<string>(csvAlias.Split(','));

            List<string> listObject = new List<string>(csvObject.Split('|'));

            for (int indexList = 0; indexList < listObject.Count; indexList++)
            {
                matrixTable.Rows.Add();

                List<string> subListObject = new List<string>(listObject[indexList].Split('='));
                if (subListObject != null)
                {
                    List<string> listToolBox = new List<string>(subListObject[1].Split(charObjDelimitier));
                    if (listToolBox != null)
                    {
                        string typeObject = listToolBox[0];

                        matrixTable.Rows[indexList][matrixColumnName] = subListObject[0].Trim();
                        matrixTable.Rows[indexList][matrixObjectType] = typeObject;
                        matrixTable.Rows[indexList][matrixAliasName] = listAlias[indexList].Trim();

                        if (showColumnList.Contains(subListObject[0].Trim()))
                        {
                            matrixTable.Rows[indexList][matrixIsVisible] = true;
                            int columnIndex = showColumnList.IndexOf(subListObject[0].Trim());
                            matrixTable.Rows[indexList][matrixColumnIndex] = (columnIndex + 1) * 10;// (indexList + 1) * 10;
                        }
                        else
                        {
                            matrixTable.Rows[indexList][matrixIsVisible] = false;
                        }

                        switch (typeObject)
                        {
                            case Constants.Constants.ToolBoxConst.tbSql:
                                matrixTable.Rows[indexList][matrixTableName] = listToolBox[1];
                                break;
                            case Constants.Constants.ToolBoxConst.tbTable:
                                matrixTable.Rows[indexList][matrixTableName] = listToolBox[1];
                                matrixTable.Rows[indexList][matrixTableKeyID] = listToolBox[2];
                                matrixTable.Rows[indexList][matrixTableColumnName] = listToolBox[3];
                                break;
                            case Constants.Constants.ToolBoxConst.tbClassifiers:
                                matrixTable.Rows[indexList][matrixTableName] = listToolBox[1];
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            matrixTable.DefaultView.Sort = matrixColumnIndex + " ASC";
            matrixTable.AcceptChanges();

            matrixTable = matrixTable.DefaultView.ToTable();

        }

        return matrixTable;
    }

    #endregion General Function

    #region BuildGridView

    public static string GetCSVColumnFromArrayTable(DataTable arrayTable)
    {
        string csvColumns = string.Empty;
        for (int indexRow = 0; indexRow < arrayTable.Rows.Count; indexRow++)
        {
            string vizibleValue = arrayTable.Rows[indexRow][arrayIsVisible].ToString();
            bool isVizible = false;
            bool.TryParse(vizibleValue, out isVizible);
            if (isVizible)
            {
                if (csvColumns.Length > 0)
                {
                    csvColumns += ", ";
                }
                string columnName = arrayTable.Rows[indexRow][arrayColumnName].ToString();
                csvColumns += columnName;
            }
        }
        return csvColumns;
    }

    public static void GetColumnsForGridView(GridView sourceGridView, DataTable arrayTable)
    {
        for (int indexRowArray = 0; indexRowArray < arrayTable.Rows.Count; indexRowArray++)
        {
            string columnName = arrayTable.Rows[indexRowArray][arrayAlias].ToString();
            string arrayVisible = arrayTable.Rows[indexRowArray][arrayIsVisible].ToString();

            bool isVisible = false;
            bool.TryParse(arrayVisible, out isVisible);

            BoundField nameColumn = new BoundField();

            nameColumn.HeaderText = columnName;

            nameColumn.DataField = columnName;

            nameColumn.Visible = isVisible;

            if (!sourceGridView.Columns.Contains(nameColumn))
            {
                sourceGridView.Columns.Add(nameColumn);
            }
        }
    }

    public static void SetHiddenFieldValue(GridView sourceGridView, DataTable personsTable)
    {
        for (int indexRow = 0; indexRow < sourceGridView.Rows.Count; indexRow++)
        {
            HiddenField indexIDHiddenField = sourceGridView.Rows[indexRow].FindControl("indexIDHiddenField") as HiddenField;
            indexIDHiddenField.Value = personsTable.Rows[indexRow][0].ToString();
        }
    }

    #endregion BuildGridView

    #region General Buttons

    public static void deleteButton()
    {
    }

    public static Hashtable newSaveButton(string listArgument, string mTableName, string mNamePanel)
    {
        Hashtable result = new Hashtable();

        DataTable matrixTable = Utils.GetArrayTableForInsertOperation(mTableName, mNamePanel);

        List<string> argumentsList = new List<string>(listArgument.Split(','));
        List<string> columnList = new List<string>();
        List<string> objectList = new List<string>();
        
        for (int indexRow = 0; indexRow < matrixTable.Rows.Count; indexRow++)
        {
            bool isVisible = true;
            bool.TryParse(matrixTable.Rows[indexRow][Utils.matrixIsVisible].ToString(), out isVisible);
            if (isVisible)
            {
                columnList.Add(matrixTable.Rows[indexRow][Utils.matrixColumnName].ToString());

                objectList.Add(matrixTable.Rows[indexRow][Utils.matrixObjectType].ToString());

                result.Add("@"+matrixTable.Rows[indexRow][Utils.matrixColumnName].ToString(), "");
            }
        }

        for (int indexList = 0; indexList < objectList.Count; indexList++)
        {
            object hashKey = "@" + columnList[indexList];

            //result[hashKey] = argumentsList[indexList];

            string objectType = objectList[indexList];

            switch (objectType)
            {
                case Constants.Constants.ToolBoxConst.tbDateTime:
                    string strDate = argumentsList[indexList];               
                    DateTime objDate = Crypt.Utils.ConvertStringDateToDateTime(strDate);

                    result[hashKey] = objDate;

                    break;

                default:
                    result[hashKey] = argumentsList[indexList];
                    break;
            }
        }

        return result;
    }
     

    #endregion General Buttons

    #region BuildNewPanel

    public static Control GetNewPanel(string mTableName, string mNamePanel, Security.MainModule mainObject)
    {
        DataTable matrixTable = GetArrayTableForInsertOperation(mTableName, mNamePanel); // GetArrayTable(mTableName, mNamePanel, Constants.Constants.ConfigPanel.csvInsert);

        HtmlGenericControl divPanel = new HtmlGenericControl("div");

        if (matrixTable != null && matrixTable.Rows.Count > 0)
        {
            HtmlGenericControl divColumn = new HtmlGenericControl("div");
            divColumn.Attributes.Add("class", "leftColumn");

            HtmlGenericControl divForm = new HtmlGenericControl("div");
            divForm.Attributes.Add("class", "form");
            divForm.Attributes.Add("name", Constants.Constants.ConfigPanel.newFormContainer);
            divForm.Attributes.Add("id", Constants.Constants.ConfigPanel.newFormContainer);

            HtmlGenericControl myFieldList = new HtmlGenericControl("fieldset");
            HtmlGenericControl legendFieldList = new HtmlGenericControl("legend");
            legendFieldList.InnerText = "New";

            for (int indexRow = 0; indexRow < matrixTable.Rows.Count; indexRow++)
            {
                bool isVisible = true;
                bool.TryParse(matrixTable.Rows[indexRow][Utils.matrixIsVisible].ToString(), out isVisible);

                if (isVisible)
                {
                    string labelValue = matrixTable.Rows[indexRow][Utils.matrixAliasName].ToString();

                    if (string.IsNullOrEmpty(labelValue))
                    {
                        labelValue = matrixTable.Rows[indexRow][Utils.matrixColumnName].ToString();
                    }

                    HtmlGenericControl label = new HtmlGenericControl("label");
                    label.InnerText = labelValue + ":";

                    HtmlGenericControl paragrah = new HtmlGenericControl("p");
                    paragrah.Controls.Add(label);

                    string objectID = matrixTable.Rows[indexRow][Utils.matrixColumnName].ToString();
                    
                    string objectType = "TextBox";
                    string cssClassInputForm = "inputForm";

                    objectType = matrixTable.Rows[indexRow][Utils.matrixObjectType].ToString();

                    ServerObject mServerObject = new ServerObject();

                    switch (objectType)
                    {
                        case Constants.Constants.ToolBoxConst.tbCheckBox:
                            #region
                            HtmlInputCheckBox htmlCheckBox = new HtmlInputCheckBox();
                            htmlCheckBox.ID = objectID;
                            htmlCheckBox.Attributes.Add("class", cssClassInputForm);
                            paragrah.Controls.Add(htmlCheckBox);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbDateTime:
                            #region
                            TextBox dateTimeTextBox = new TextBox();
                            dateTimeTextBox.ID = objectID;
                            dateTimeTextBox.CssClass = cssClassInputForm;

                            CalendarExtender calendarExtender = new CalendarExtender();
                            calendarExtender.ID = "CalendarEntender" + objectID;
                            calendarExtender.TargetControlID = objectID;
                            calendarExtender.Format = "dd.MM.yyyy";
                            paragrah.Controls.Add(dateTimeTextBox);
                            paragrah.Controls.Add(calendarExtender);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbTextBox:
                            #region
                            TextBox textBox = new TextBox();
                            textBox.ID = objectID;
                            textBox.CssClass = cssClassInputForm;

                            paragrah.Controls.Add(textBox);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbClassifiers:
                            #region
                            DropDownList dropDownList = new DropDownList();
                            dropDownList.ID = objectID;
                            dropDownList.CssClass = cssClassInputForm;

                            int classID = 0;
                            string classValue = matrixTable.Rows[indexRow][matrixTableName].ToString();
                            int.TryParse(classValue, out classID);

                            DataTable sourceClassifier = mainObject.GetClassifierByTypeID(classID);
                            Utils.FillSelector(dropDownList, sourceClassifier, "Name", "Code");

                            paragrah.Controls.Add(dropDownList);
                            #endregion
                            break;

                       case Constants.Constants.ToolBoxConst.tbTable:
                            #region
                            DropDownList tableDropDownList = new DropDownList();
                            tableDropDownList.ID = objectID;
                            tableDropDownList.CssClass = cssClassInputForm;

                            string tableValue = matrixTable.Rows[indexRow][matrixTableName].ToString();
                            string displayTextField = matrixTable.Rows[indexRow][matrixTableColumnName].ToString().Trim();
                            string valueField = matrixTable.Rows[indexRow][matrixTableKeyID].ToString().Trim();  

                            DataTable sourceTable = mServerObject.GetTableByTableName(tableValue);
                            Utils.FillSelector(tableDropDownList, sourceTable, displayTextField, valueField);

                            paragrah.Controls.Add(tableDropDownList);
                            #endregion
                            break;

                        case Constants.Constants.ToolBoxConst.tbSql:
                            #region
                            DropDownList sqlDropDownList = new DropDownList();
                            sqlDropDownList.ID = objectID;
                            sqlDropDownList.CssClass = cssClassInputForm;

                            string sqlQuery = matrixTable.Rows[indexRow][matrixTableName].ToString();
                            string sqlDisplayTextField = "Name";
                            string sqlValueField = "Code";

                            DataTable sqlTable = mServerObject.UniversalGetFromSQLQuery(sqlQuery);
                            Utils.FillSelector(sqlDropDownList, sqlTable, sqlDisplayTextField, sqlValueField);

                            paragrah.Controls.Add(sqlDropDownList);

                            #endregion
                            break;

                        default:
                            break;
                    }

                    myFieldList.Controls.Add(legendFieldList);
                    myFieldList.Controls.Add(paragrah);
                }
            }

            divForm.Controls.Add(myFieldList);
            divColumn.Controls.Add(divForm);

            divPanel.Controls.Add(divColumn);

            HtmlGenericControl divClear = new HtmlGenericControl("div");
            divClear.Attributes.Add("class", "clear");

            HiddenField hiddenField = new HiddenField();
            hiddenField.ID = "addNewHiddenField";

            divClear.Controls.Add(hiddenField);

            divPanel.Controls.Add(divClear);

            HtmlGenericControl divButton = new HtmlGenericControl("div");
            divButton.Attributes.Add("class", "centerBox");
            divButton.Attributes.Add("style", " width: 150px");

            Button saveButton = GetButton("Save", "new");
            divButton.Controls.Add(saveButton);

            Button cancelButton = GetButton("Cancel", "new");
            divButton.Controls.Add(cancelButton);

            divPanel.Controls.Add(divButton);
        }

        return divPanel;
    }

    private static Button GetButton(string nameButton, string nameOperation)
    {
        Button resultButton = new Button();
        resultButton.ID = nameOperation + nameButton + "ButtonID";
        resultButton.Text = nameButton;
        resultButton.OnClientClick = nameButton.Equals("Save") ? "GetInputFromNewPanel()" : "CancelInputFromNewPanel()";//"doPost('" + nameOperation + nameButton + "','" + nameOperation + nameButton + "Button_Click')";

        return resultButton;
    }

    #endregion BuildNewPanel

    #endregion ConfigPanel
}