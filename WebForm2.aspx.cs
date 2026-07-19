using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFF
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = "  Label1 = ";
            Label3.Text = "  Label2 = ";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            Label1.Text = "this message from btn click";


            //string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=productionboard_db;Integrated Security=True";
            //string insertQuery = "UPDATE productionBoard SET reel_h1 = @reel1 WHERE teamName= @teamName and shiftName=@shiftName";
            //SqlConnection connection = new SqlConnection(connectionString);
            //connection.Open();
            //SqlCommand command = new SqlCommand(insertQuery, connection);
            //command.Parameters.AddWithValue("@teamName", "Team A");
            //command.Parameters.AddWithValue("@shiftName", "Morning");
            //command.Parameters.AddWithValue("@reel1", 11);
            ////command.Parameters.AddWithValue("@reel_h1", Convert.ToInt32(reel_h1.Text));

            //command.ExecuteNonQuery();

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Label1.Text = "  Label2 !=";
            Label3.Text = "  Label1 !=";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Text = " Label1 =";
            Label3.Text = " Label2 =";
        }
    }
}