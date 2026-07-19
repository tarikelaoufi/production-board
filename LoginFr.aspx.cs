using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFF
{
    public partial class LoginFr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1Fr_Click(object sender, EventArgs e)
        {
            Session["value1"] = DropDownListTeamsFr.Items[DropDownListTeamsFr.SelectedIndex].Text;
            Session["value2"] = DropDownListShiftFr.Items[DropDownListShiftFr.SelectedIndex].Text;
            Session["value3"] = DropDownListPLFr.Items[DropDownListPLFr.SelectedIndex].Text;
            Session["value4"] = todayDateFr.Value.ToString();

            if (DropDownListTeamsFr.Items[DropDownListTeamsFr.SelectedIndex].Text == "Team A" && verifyidfr.Text == "1111")
            {
                Response.Redirect("PB_page.aspx");

            }

            else if (DropDownListTeamsFr.Items[DropDownListTeamsFr.SelectedIndex].Text == "Team B" && verifyidfr.Text == "2222")
            {
                Response.Redirect("PB_page.aspx");

            }
            else if (DropDownListTeamsFr.Items[DropDownListTeamsFr.SelectedIndex].Text == "Team C" && verifyidfr.Text == "3333")
            {
                Response.Redirect("PB_page.aspx");

            }
            else { Response.Write("wrong Team and/or ID, Please check again."); }
        }
    }
}