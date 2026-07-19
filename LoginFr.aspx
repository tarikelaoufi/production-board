<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginFr.aspx.cs" Inherits="PFF.LoginFr" %>

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
  width: 51%; /* Adjust the width as needed */
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
<h2 style="color:#6AABD2;"><center>S'initialiser au Tableau de Production</center></h2>

<div class="row">
  <div class="col-75">
    <div class="container">
      <form runat="server">
      
        <div class="row">
          <div class="col-50">
            <br>
            <label for="teams"><i class="fa-solid fa-people-group"></i>Sélectionnez votre équipe</label>
              <asp:DropDownList ID="DropDownListTeamsFr" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Équipe A" Value="TeamA" />
     <asp:ListItem Text="Équipe B" Value="TeamB" />
     <asp:ListItem Text="Équipe C" Value="TeamC" />
</asp:DropDownList>





      
            <label for="shifts"><i class="fa-solid fa-traffic-light"></i>Sélectionnez votre shift</label>
                   <asp:DropDownList ID="DropDownListShiftFr" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Matin" Value="morning" />
     <asp:ListItem Text="Nuit" Value="night" />
     <asp:ListItem Text="Après-midi" Value="afternoon" />
</asp:DropDownList>
  
   
              <div class="row">
              <div class="col-50">
                 <label for="shifts"><i class="fa-solid fa-box-open"></i>Sélectionnez le Produit</label>
                   <asp:DropDownList ID="DropDownList1" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Produit 1" Value="produit1" />
     <asp:ListItem Text="Produit 2" Value="produit2" />
     <asp:ListItem Text="Produit 3" Value="produit3" />
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
               <label for="pl"><i class="fa-brands fa-font-awesome"></i>Sélectionnez la ligne de production</label>
                     <asp:DropDownList ID="DropDownListPLFr" AppendDataBoundItems="true" runat="server">
     <asp:ListItem Text="Ligne de production 1" Value="pl1" />
     <asp:ListItem Text="Ligne de production 2" Value="pl2" />
     <asp:ListItem Text="Ligne de production 3" Value="pl3" />
</asp:DropDownList>
               <label for="date"><i class="fa-solid fa-calendar"></i> Ajust Date</label>
                <input type="date" runat="server" id="todayDateFr" name="sel_date"  />

                   <label for="verifyidFr"><i class="fa-solid fa-user-check"></i>Ajuster la Date</label>
              <asp:TextBox ID="verifyidfr" runat="server" placeholder="50001234"></asp:TextBox>

 <script>
     document.getElementById("todayDateFr").valueAsDate = new Date();
 </script>

           
           </div>
  </div>    
          <asp:Button ID="Button1Fr" runat="server" Text="Continuer au Tableau de Production" type="submit"  class="btn" OnClick="Button1Fr_Click" />
      </form>
   
  </div>


</body>
</html>

