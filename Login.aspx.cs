using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PFF
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["value1"] = DropDownListTeams.Items[DropDownListTeams.SelectedIndex].Text;
            Session["value2"] = DropDownListShift.Items[DropDownListShift.SelectedIndex].Text;
            Session["value3"] = DropDownListPL.Items[DropDownListPL.SelectedIndex].Text;
            Session["value4"] = todayDate.Value;
            Session["value5"] = DropDownListProducts.Items[DropDownListPL.SelectedIndex].Text;
            if (DropDownListTeams.Items[DropDownListTeams.SelectedIndex].Text == "Team A" && verifyid.Text == "1111")
            {
                Response.Redirect("PB_page.aspx");

            }

            else if (DropDownListTeams.Items[DropDownListTeams.SelectedIndex].Text == "Team B" && verifyid.Text == "2222")
            {
                Response.Redirect("PB_page.aspx");

            }
            else if (DropDownListTeams.Items[DropDownListTeams.SelectedIndex].Text == "Team C" && verifyid.Text == "3333")
            {
                Response.Redirect("PB_page.aspx");

            }
            else { Response.Write("wrong Team and/or ID, Please check again."); }
        }
    }
}