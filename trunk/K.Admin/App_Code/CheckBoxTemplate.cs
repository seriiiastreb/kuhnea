using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public class CheckBoxTemplate : System.Web.UI.ITemplate, INamingContainer
{
    System.Web.UI.WebControls.ListItemType _type;

    public CheckBoxTemplate(System.Web.UI.WebControls.ListItemType type)
	{
        _type = type;
	}

    public void InstantiateIn(System.Web.UI.Control container)
    {
        CheckBox chk = new CheckBox();
        chk.ID = "chkSelect";
        switch (_type)
        {
            case System.Web.UI.WebControls.ListItemType.Item:
                chk.Attributes.Add("onclick", "SelectRow()");
                container.Controls.Add(chk);
                break;
            case System.Web.UI.WebControls.ListItemType.Header:
                chk.ID = "chkAll";
                chk.Attributes.Add("onclick", "SelectAllRows()");
                container.Controls.Add(chk);
                break;
            case System.Web.UI.WebControls.ListItemType.EditItem:
                Button btn = new Button();
                btn.CommandName = "Delete";
                btn.Text = "Delete";
                container.Controls.Add(btn);

                break;
        }
    }
}