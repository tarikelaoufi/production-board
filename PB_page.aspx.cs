using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PFF
{
    public partial class PB_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                //lbltest.Text = "message from load";
                TeamLabel.Text = (string)Session["value1"];
                ShiftLabel.Text = "Shift: " + (string)Session["value2"];
                PLLabel.Text = (string)Session["value3"];
                //DateLabel.Text = (string)Session["value4"];
                DateLabel.Text = Session["value4"].ToString();
                //string d = "2024-05-24";
                //DateTime.Parse(d);
                //DateLabel.Text= d.ToString();


                ProductLabel.Text = (string)Session["value5"];

                //int labelValue = Convert.ToInt32(reel_h1);
                //int number = int.Parse(labelValue);
                //string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=productionboard_db;Integrated Security=True";
                //string insertQuery = "UPDATE productionBoard SET reel_h1 = @reel1 WHERE teamName= @teamName and shiftName=@shiftName";
                //SqlConnection connection = new SqlConnection(connectionString);
                //connection.Open();
                //SqlCommand command = new SqlCommand(insertQuery, connection);
                //command.Parameters.AddWithValue("@teamName", (string)Session["value1"]);
                //command.Parameters.AddWithValue("@shiftName", (string)Session["value2"]);
                //command.Parameters.AddWithValue("@shiftName", Convert.ToDateTime((string)Session["value4"]));
                //command.Parameters.AddWithValue("@reel1", 111);
                //command.Parameters.AddWithValue("@reel_h1", Convert.ToInt32(reel_h1.Text));

                //command.ExecuteNonQuery();

                //string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=productionboard_db;Integrated Security=True";
                //string insertQuery = "insert into productionBoard (teamName, LineName,boardDate, shiftName,productName) values " +
                //    "                                              (@teamName,@LineName,@boardDate, @shiftName,@productName)";
                //SqlConnection connection = new SqlConnection(connectionString);
                //connection.Open();
                //SqlCommand command = new SqlCommand(insertQuery, connection);
                //command.Parameters.AddWithValue("@teamName", (string)Session["value1"]);
                //command.Parameters.AddWithValue("@shiftName", (string)Session["value2"]);
                //command.Parameters.AddWithValue("@LineName", (string)Session["value3"]);
                //command.Parameters.AddWithValue("@boardDate", Convert.ToDateTime((string)Session["value4"]));
                //command.Parameters.AddWithValue("@productName", (string)Session["value5"]);

                //command.ExecuteNonQuery();





                if ((string)Session["value2"] == "Morning" || (string)Session["value2"] == "Matin")
                {
                    h1Label.Text = "07:00";
                    h2Label.Text = "08:00";
                    h3Label.Text = "09:00";
                    h4Label.Text = "10:00";
                    h5Label.Text = "11:00";
                    h6Label.Text = "12:00";
                    h7Label.Text = "13:00";
                    h8Label.Text = "14:00";
                }
                else if ((string)Session["value2"] == "Afternoon" || (string)Session["value2"] == "Après-midi")
                {
                    h1Label.Text = "15:00";
                    h2Label.Text = "16:00";
                    h3Label.Text = "17:00";
                    h4Label.Text = "18:00";
                    h5Label.Text = "19:00";
                    h6Label.Text = "20:00";
                    h7Label.Text = "21:00";
                    h8Label.Text = "22:00";
                }
                else if ((string)Session["value2"] == "Night" || (string)Session["value2"] == "Nuit")
                {
                    h1Label.Text = "23:00";
                    h2Label.Text = "00:00";
                    h3Label.Text = "01:00";
                    h4Label.Text = "02:00";
                    h5Label.Text = "03:00";
                    h6Label.Text = "04:00";
                    h7Label.Text = "05:00";
                    h8Label.Text = "06:00";
                }
                if ((string)Session["value3"] == "Production Line 1" || (string)Session["value3"] == "Ligne de production 1")
                {
                    h1Object.Text = OBJ_CML1.Text = "55";
                    h2Object.Text = h3Object.Text = h5Object.Text = h6Object.Text = h7Object.Text = "60";
                    h4Object.Text = "40";
                    h8Object.Text = "50";

                    OBJ_CML2.Text = "115";
                    OBJ_CML3.Text = "175";
                    OBJ_CML4.Text = "215";
                    OBJ_CML5.Text = "275";
                    OBJ_CML6.Text = "335";
                    OBJ_CML7.Text = "395";
                    OBJ_CML8.Text = "445";
                }
                else if ((string)Session["value3"] == "Production Line 2" || (string)Session["value3"] == "Ligne de production 2")
                {
                    h1Object.Text = OBJ_CML1.Text = "110";
                    h2Object.Text = h3Object.Text = h5Object.Text = h6Object.Text = h7Object.Text = "120";
                    h4Object.Text = "80";
                    h8Object.Text = "100";


                    OBJ_CML2.Text = "230";
                    OBJ_CML3.Text = "350";
                    OBJ_CML4.Text = "430";
                    OBJ_CML5.Text = "550";
                    OBJ_CML6.Text = "670";
                    OBJ_CML7.Text = "790";
                    OBJ_CML8.Text = "890";
                }
                else if ((string)Session["value3"] == "Production Line 3" || (string)Session["value3"] == "Ligne de production 3")
                {
                    h1Object.Text = OBJ_CML1.Text = "220";
                    h2Object.Text = h3Object.Text = h5Object.Text = h6Object.Text = h7Object.Text = "240";
                    h4Object.Text = "160";
                    h8Object.Text = "200";

                    OBJ_CML2.Text = "460";
                    OBJ_CML3.Text = "700";
                    OBJ_CML4.Text = "860";
                    OBJ_CML5.Text = "1100";
                    OBJ_CML6.Text = "1340";
                    OBJ_CML7.Text = "1580";
                    OBJ_CML8.Text = "1780";
                }





            }

        }


        [WebMethod]
        public static string ChangeLabel()
        {
           
            return "Label text changed from server-side method.";
        }
        void change_label()
        {
            lblCOUCOU.Text = "BTN CLICKED";

        }

        public void Update_reel_h1()
        {
            //int labelValue = Convert.ToInt32(reel_h1);
            //int number = int.Parse(labelValue);
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=productionboard_db;Integrated Security=True";
            string insertQuery = "UPDATE productionBoard SET reel_h1 = @reel1 WHERE teamName= @teamName and shiftName=@shiftName";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@teamName", (string)Session["value1"]);
            command.Parameters.AddWithValue("@shiftName", (string)Session["value2"]);
            command.Parameters.AddWithValue("@reel1", 889);
            //command.Parameters.AddWithValue("@reel_h1", Convert.ToInt32(reel_h1.Text));

            command.ExecuteNonQuery();


        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblCOUCOU.Text = "label changed from code behind";
            change_label();
        }

        protected void BTN2_Click(object sender, EventArgs e)
        {
            lblCOUCOU.Text = "";
        }





        //protected void btnupdate_Click(object sender, EventArgs e)
        //{
        //    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=productionboard_db;Integrated Security=True";
        //    string insertQuery = "UPDATE productionBoard SET reel_h1 = @reel1 WHERE teamName= @teamName and shiftName=@shiftName";
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = new SqlCommand(insertQuery, connection);
        //    command.Parameters.AddWithValue("@teamName", (string)Session["value1"]);
        //    command.Parameters.AddWithValue("@shiftName", (string)Session["value2"]);
        //    command.Parameters.AddWithValue("@reel1", 888);
        //    //command.Parameters.AddWithValue("@reel_h1", Convert.ToInt32(reel_h1.Text));

        //}





    }
}
    
