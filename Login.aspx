<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PFF.Login" %>

<!DOCTYPE html>
<html>
<head>
<meta name="viewport" content="width=device-width, initial-scale=1">
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" integrity="sha512-SnH5WK+bZxgPHs44uWIX+LLJAJ9/2PkPKZ5QiAj6Ta86w+fsb2TkcmfRyVX3pBnMFcV7oQPJkl9QevSCWr3W6A==" crossorigin="anonymous" referrerpolicy="no-referrer" />
<style>
body {
  font-family: Arial;
  font-size: 17px;
  padding: 8px;


}

* {
  box-sizing: border-box;
}

.row {
  display: -ms-flexbox; /* IE10 */
  display: flex;
  -ms-flex-wrap: wrap; /* IE10 */
  flex-wrap: wrap;
  margin: 0 -16px;
}

.col-25https://accounts.google.com/b/0/AddMailService {
  -ms-flex: 25%; /* IE10 */
  flex: 25%;
}

.col-50 {
  -ms-flex: 50%; /* IE10 */
  flex: 50%;
}

.col-75 {
  -ms-flex: 75%; /* IE10 */
  flex: 75%;
}

.col-25,
.col-50,
.col-75 {
  padding: 0 16px;
}

.container {
  background-color: #f2f2f2;
  padding: 5px 20px 15px 20px;
  border: 1px solid lightgrey;
  border-radius: 3px;
   background-color: #f2f2f2;
  padding: 5px 20px 15px 20px;
  border: 1px solid lightgrey;
  border-radius: 3px;
  width: 50%; /* Adjust the width as needed */
  margin: auto; /* Center the container horizontally */
    
  
}

input[type=text],input[type=date],select {
  width: 100%;
  margin-bottom: 20px;
  padding: 12px;
  border: 1px solid #ccc;
  border-radius: 3px;
}

label {
  margin-bottom: 10px;
  display: block;
  COLOR: #6AABD2;
}

.icon-container {
  margin-bottom: 20px;
  padding: 7px 0;
  font-size: 24px;
}

.btn {
  background-color: #6AABD2;
  color: white;
  padding: 12px;
  margin: 10px 0;
  border: none;
  width: 100%;
  border-radius: 3px;
  cursor: pointer;
  font-size: 17px;
}

.btn:hover {
  background-color: #87B8D6;
}

}


a {
  color: #2196F3;
}

hr {
  border: 1px solid lightgrey;
}

span.price {
  float: right;
  color: grey;
}


@media (max-width: 800px) {
  .row {
    flex-direction: column-reverse;
  }
  .col-25 {
    margin-bottom: 20px;
  }
}
</style>
</head>
<body>
    <nav>
<div >
<a href="Login.aspx">En</a> -
<a href="LoginFr.aspx">Fr</a>
</div>
</nav>

<h2 style="color:#6AABD2;"><center>Sign up to Production Board</center></h2>

<div class="row">
  <div class="col-75">
    <div class="container">
      <form runat="server">
      
        <div class="row">
          <div class="col-50">
            <br>
            <label for="teams"><i class="fa-solid fa-people-group"></i> Select your Team</label>
              <asp:DropDownList ID="DropDownListTeams" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Team A" Value="TeamA" />
     <asp:ListItem Text="Team B" Value="TeamB" />
     <asp:ListItem Text="Team C" Value="TeamC" />
</asp:DropDownList>





      
            <label for="shifts"><i class="fa-solid fa-traffic-light"></i> Select your Shift</label>
                   <asp:DropDownList ID="DropDownListShift" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Morning" Value="morning" />
     <asp:ListItem Text="Night" Value="night" />
     <asp:ListItem Text="Afternoon" Value="afternoon" />
</asp:DropDownList>
  
   
              <div class="row">
              <div class="col-50">
           <label for="pl"><i class="fa-solid fa-box-open"></i> Select Product</label>
                     <asp:DropDownList ID="DropDownListProducts" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Product 1" Value="Product1" />
     <asp:ListItem Text="Product 2" Value="Product2" />
     <asp:ListItem Text="Product 3" Value="Product3" />
</asp:DropDownList>
				
              </div>
			  </div> 
              
            
            <div class="row">
              <div class="col-50">
                
              </div>
              <div class="col-50">
                
              </div>
            </div>
          </div>

          <div class="col-50">
            <h3></h3>
               <label for="pl"><i class="fa-brands fa-font-awesome"></i> Select your Production Line</label>
                     <asp:DropDownList ID="DropDownListPL" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Production Line 1" Value="pl1" />
     <asp:ListItem Text="Production Line 2" Value="pl2" />
     <asp:ListItem Text="Production Line 3" Value="pl3" />
</asp:DropDownList>
               <label for="date"><i class="fa-solid fa-calendar"></i> Ajust Date</label>
                <input type="date" runat="server" id="todayDate" name="sel_date"  />

                               <label for="verifyid"><i class="fa-solid fa-user-check"></i> Verify ID</label>
              <asp:TextBox ID="verifyid" runat="server" placeholder="50001234"></asp:TextBox>


 <script>
     document.getElementById("todayDate").valueAsDate = new Date();
 </script>

           
           </div>
  </div>

         <asp:Button ID="Button1" runat="server" Text="Continue to Production BOARD" type="submit"  class="btn" OnClick="Button1_Click" />
    

      </form>
   
  </div>


</body>
</html>

