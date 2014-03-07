using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public class FormattedTemplate : System.Web.UI.ITemplate, INamingContainer
{
    System.Web.UI.WebControls.ListItemType _type;
    DataColumn _dc;

    public FormattedTemplate(System.Web.UI.WebControls.ListItemType type, DataColumn dc)
	{
        _type = type;
        _dc = dc;
	}

    public void InstantiateIn(System.Web.UI.Control container)
    {
        TextBox txt = null;
        Label lbl = null;
        RegularExpressionValidator rexpval = null;
        RangeValidator rangeVal = null;
        switch (_type)
        {
            case System.Web.UI.WebControls.ListItemType.Item:
                switch (_dc.DataType.ToString())
                {
                    case "System.Decimal":
                        txt = new TextBox();
                        if (_dc.DefaultValue.Equals(DBNull.Value)) txt.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                        if (!_dc.Expression.Equals(string.Empty)) txt.ToolTip += ", a calculated field based on " + _dc.Expression.ToString();

                        txt.ID = "txt" + _dc.ColumnName;
                        container.Controls.Add(txt);
                        txt.MaxLength = 19;
                        txt.Width = Unit.Pixel(60);
                        rexpval = new RegularExpressionValidator();
                        rexpval.ValidationGroup = rexpval.ClientID + "_group";
                        rexpval.ValidationExpression = @"^\d*.?\d{0,2}$";
                        rexpval.Display = ValidatorDisplay.Dynamic;
                        rexpval.Text = "*";
                        rexpval.ErrorMessage = "Entered value for " + _dc.ColumnName + " is not valid for Decimal type";
                        rexpval.ControlToValidate = txt.ID;
                        container.Controls.Add(rexpval);
                        //I am adding this segment only as a demonstration of the use of Validation 
                        //Group to specify the target of the validating in case there were several
                        //controls rendered on the GridView that would require validation.
                        Button btnValidate = new Button();
                        container.Controls.Add(btnValidate);
                        btnValidate.Text = "Validate";
                        btnValidate.ValidationGroup = rexpval.ClientID + "_group";

                        txt.DataBinding += new EventHandler(txt_DataBinding);
                        break;
                    default:

                        //if datacolumn is readonly and unique then it is a primary key
                        //render it as label a distinctive style
                        lbl = new Label();
                        if (_dc.ReadOnly && _dc.Unique)
                        {
                            lbl.CssClass = "PrimaryKeyCell";
                        }
                        //compose a tooltip from the column attributes
                        lbl.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                        if (_dc.ReadOnly) lbl.ToolTip += ", Readonly";
                        if (_dc.Unique) lbl.ToolTip += ", Unique";
                        if (_dc.DefaultValue.Equals(DBNull.Value)) lbl.ToolTip += ", Default value is DBNull";
                        if (!_dc.Expression.Equals(string.Empty)) lbl.ToolTip += ", a calculated field based on " + _dc.Expression.ToString();

                        container.Controls.Add(lbl);
                        lbl.DataBinding += new EventHandler(lbl_DataBinding);

                        break;
                }
                break;
            case System.Web.UI.WebControls.ListItemType.EditItem:

                if (_dc.ReadOnly || (_dc.ReadOnly && _dc.Unique))
                {


                    lbl = new Label();
                    lbl.CssClass = "PrimaryKeyCell";
                    //compose a tooltip from the column attributes
                    lbl.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                    if (_dc.ReadOnly) lbl.ToolTip += ", Readonly";
                    if (_dc.Unique) lbl.ToolTip += ", Unique";
                    if (_dc.DefaultValue.Equals(DBNull.Value)) lbl.ToolTip += ", Default value is DBNull";
                    container.Controls.Add(lbl);
                    lbl.DataBinding += new EventHandler(lbl_DataBinding);

                }

                else
                {

                    switch (_dc.DataType.ToString())
                    {
                        case "System.Boolean":
                            RadioButtonList rbl = new RadioButtonList();
                            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Yes", "True");
                            rbl.Items.Add(li);
                            li = new System.Web.UI.WebControls.ListItem("No", "False");
                            rbl.Items.Add(li);
                            rbl.DataBound += new EventHandler(rbl_DataBound);
                            container.Controls.Add(rbl);
                            break;
                        case "System.String":
                            txt = new TextBox();
                            txt.MaxLength = _dc.MaxLength;
                            txt.Width = Unit.Pixel(_dc.MaxLength * 6);
                            container.Controls.Add(txt);
                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;
                        case "System.Int16":
                            txt = new TextBox();
                            if (_dc.DefaultValue.Equals(DBNull.Value)) txt.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                            if (!_dc.Expression.Equals(string.Empty)) txt.ToolTip += ", a calculated field based on " + _dc.Expression.ToString();

                            txt.ID = "txt" + _dc.ColumnName;
                            container.Controls.Add(txt);
                            txt.MaxLength = 5;
                            txt.Width = Unit.Pixel(40);
                            rangeVal = new RangeValidator();
                            rangeVal.ControlToValidate = txt.ID;
                            rangeVal.MaximumValue = "32767";
                            rangeVal.MinimumValue = "-32767";
                            rangeVal.Display = ValidatorDisplay.Dynamic;
                            rangeVal.Text = "*";
                            rangeVal.ErrorMessage = "Entered value for " + _dc.ColumnName + " is not valid for an int16 type";

                            rangeVal.Type = ValidationDataType.Integer;
                            container.Controls.Add(rangeVal);

                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;
                        case "System.Int32":
                            txt = new TextBox();
                            if (_dc.DefaultValue.Equals(DBNull.Value)) txt.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                            if (!_dc.Expression.Equals(string.Empty)) txt.ToolTip += ", a calculated field based on " + _dc.Expression.ToString();

                            txt.ID = "txt" + _dc.ColumnName;
                            container.Controls.Add(txt);
                            txt.MaxLength = 10;
                            txt.Width = Unit.Pixel(60);
                            rangeVal = new RangeValidator();
                            rangeVal.ControlToValidate = txt.ID;
                            rangeVal.MaximumValue = "2147483647";
                            rangeVal.MinimumValue = "-2147483648";
                            rangeVal.Display = ValidatorDisplay.Dynamic;
                            rangeVal.Text = "*";
                            rangeVal.ErrorMessage = "Entered value for " + _dc.ColumnName + " is not valid for an int32 type";
                            rangeVal.Type = ValidationDataType.Integer;
                            container.Controls.Add(rangeVal);
                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;
                        case "System.Int64":
                            txt = new TextBox();
                            if (_dc.DefaultValue.Equals(DBNull.Value)) txt.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                            if (!_dc.Expression.Equals(string.Empty)) txt.ToolTip += ", a calculated field based on " + _dc.Expression.ToString();

                            txt.ID = "txt" + _dc.ColumnName;
                            container.Controls.Add(txt);
                            txt.MaxLength = 19;
                            txt.Width = Unit.Pixel(60);
                            rangeVal = new RangeValidator();
                            rangeVal.ControlToValidate = txt.ID;
                            rangeVal.MaximumValue = "9223372036854775807";
                            rangeVal.MinimumValue = "-9223372036854775808";
                            rangeVal.Display = ValidatorDisplay.Dynamic;
                            rangeVal.Text = "*";
                            rangeVal.ErrorMessage = "Entered value for " + _dc.ColumnName + " is not valid for an int64 type";
                            rangeVal.Type = ValidationDataType.Integer;
                            container.Controls.Add(rangeVal);
                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;
                        case "System.Decimal":
                            txt = new TextBox();
                            if (_dc.DefaultValue.Equals(DBNull.Value)) txt.ToolTip = _dc.ColumnName + " is of type " + _dc.DataType.ToString();
                            if (!_dc.Expression.Equals(string.Empty)) txt.ToolTip += ", a calculated field based on " + _dc.Expression.ToString();

                            txt.ID = "txt" + _dc.ColumnName;
                            container.Controls.Add(txt);
                            txt.MaxLength = 19;
                            txt.Width = Unit.Pixel(60);
                            rexpval = new RegularExpressionValidator();
                            rexpval.ValidationExpression = @"^\d*.?\d{0,2}$";
                            rexpval.Display = ValidatorDisplay.Dynamic;
                            rexpval.Text = "*";
                            rexpval.ErrorMessage = "Entered value for " + _dc.ColumnName + " is not valid for Decimal type";
                            rexpval.ControlToValidate = txt.ID;
                            container.Controls.Add(rexpval);
                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;

                        case "System.DateTime":
                            txt = new TextBox();
                            txt.ID = "txt" + _dc.ColumnName;
                            txt.MaxLength = 10;
                            txt.Width = Unit.Pixel(80);
                            container.Controls.Add(txt);
                            rexpval = new RegularExpressionValidator();
                            rexpval.ValidationExpression = @"^\(?\d{3}[\)\-\s]?\d{3}[-\s]?\d{4}$";
                            rexpval.ControlToValidate = txt.ID;
                            rexpval.Display = ValidatorDisplay.Dynamic;
                            rexpval.Text = "*";
                            rexpval.ErrorMessage = "Entered value for " + _dc.ColumnName + " is not a valid date entry";
                            container.Controls.Add(rexpval);
                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;
                        default:
                            txt = new TextBox();
                            txt.ID = "txt" + _dc.ColumnName;
                            container.Controls.Add(txt);
                            if (txt.MaxLength > 0)
                            {
                                txt.MaxLength = _dc.MaxLength;
                                txt.Width = Unit.Pixel(_dc.MaxLength * 6);
                            }
                            txt.DataBinding += new EventHandler(txt_DataBinding);
                            break;
                    }

                }
                break;
        }
    }


    void txt_DataBinding(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        DataRowView drv = null;
        if (txt.NamingContainer is GridViewRow)
        {
            drv = (DataRowView)((GridViewRow)txt.NamingContainer).DataItem;
        }
        else if (txt.NamingContainer is FormView)
        {
            drv = (DataRowView)((FormView)txt.NamingContainer).DataItem;
        }
        switch (_dc.DataType.ToString())
        {

            case "System.String":
                txt.Text = drv[_dc.ColumnName].ToString();
                break;
            case "System.Int16":
            case "System.Int32":
            case "System.Int64":
            case "int":
                if (Convert.ToInt64(drv[_dc.ColumnName]) < 0) txt.CssClass = "NegativeNumber";
                txt.Text = Convert.ToInt64(drv[_dc.ColumnName]).ToString("#0");
                break;
            case "System.Decimal":
                if (Convert.ToDecimal(drv[_dc.ColumnName]) < 0) txt.CssClass = "NegativeNumber";
                txt.Text = Convert.ToDecimal(drv[_dc.ColumnName]).ToString("#0.00");
                break;
            case "System.DateTime":
                if (Convert.ToDateTime(drv[_dc.ColumnName]) > DateTime.Now) txt.CssClass = "LateDate";
                txt.Text = Convert.ToDateTime(drv[_dc.ColumnName]).ToString("MM/dd/yyyy");
                break;

            default:
                txt.Text = drv[_dc.ColumnName].ToString();
                break;
        }
    }

    void rbl_DataBound(object sender, EventArgs e)
    {
        RadioButtonList rbl = ((RadioButtonList)sender);
        DataRowView drv = null;
        if (rbl.NamingContainer is GridViewRow)
        {
            drv = (DataRowView)((GridViewRow)rbl.NamingContainer).DataItem;
        }
        else if (rbl.NamingContainer is FormView)
        {
            drv = (DataRowView)((FormView)rbl.NamingContainer).DataItem;
        }
        rbl.SelectedValue = drv[_dc.ColumnName].ToString();

    }
    void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = ((Label)sender);
        DataRowView drv = null;
        if (lbl.NamingContainer is GridViewRow)
        {
            drv = (DataRowView)((GridViewRow)lbl.NamingContainer).DataItem;
        }
        else if (lbl.NamingContainer is FormView)
        {
            drv = (DataRowView)((FormView)lbl.NamingContainer).DataItem;
        }
        //change the data format based on the data if the datacolumn is not a primary key field
        if (_dc.ReadOnly && _dc.Unique)
        {
            lbl.Text = drv[_dc.ColumnName].ToString();
        }
        else
        {
            switch (_dc.DataType.ToString())
            {
                case "System.DateTime":
                    lbl.Text = ((DateTime)drv[_dc.ColumnName]).ToString("ddd, MMM dd, yyyy");
                    if ((DateTime)drv[_dc.ColumnName] > DateTime.Now) lbl.CssClass = "LateDate";
                    break;
                case "System.Decimal":
                    lbl.Text = ((Decimal)drv[_dc.ColumnName]).ToString("#0.00");
                    if ((Decimal)drv[_dc.ColumnName] < 0) lbl.CssClass = "NegativeNumber";
                    break;
                case "System.Boolean":
                    lbl.Text = Convert.ToBoolean(drv[_dc.ColumnName]) ? "Yes" : "No";
                    break;
                default:
                    lbl.Text = drv[_dc.ColumnName].ToString();
                    break;

            }
        }


    }


}