<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PB_page.aspx.cs" Inherits="PFF.PB_page" %> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <title>Production BOARD</title>
    <link href="stylesheet.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</head>
	
 <!-- Link to Font Awesome CSS -->
    <!-- Link to your external CSS -->


 <body>
     
<!-- modal de 1er heure-->

     <form id="form1" runat="server">
   
<input type="checkbox" id="openDialog" style="display: none"/>

<dialog id="myDialog" >
   <div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H1</h3>
            <label>Quantité totale des pièces réels:</label>
              <asp:TextBox id="r1" runat="server"></asp:TextBox>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment" required="required" />
            </div></div>              
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue() "  value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog()"  value="Cancel" class="btn"/>  </div>
  </div></div>
    
</dialog>
<!-- modal de 2er heure-->
<input type="checkbox" id="openDialog2" style="display: none"/>
<dialog id="myDialog2" >
    <div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H2</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue2" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut2" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment2" required="required" />
            </div></div>
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue2()" value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog2()"  value="Cancel" class="btn"/>  </div>
  </div></div>
    
</dialog>
<!-- modal de 3eme heure-->
<input type="checkbox" id="openDialog3" style="display: none"/>
<dialog id="myDialog3" >
    <div><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H3</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue3" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut3" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment3" required="required" />
            </div></div>        
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue3()" value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog3()" value="Cancel" class="btn"/>  </div>
  </div></div>
    </div>
</dialog>
<!-- modal de 4eme heure-->
<input type="checkbox" id="openDialog4" style="display: none"/>
<dialog id="myDialog4" >
    <div><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H4</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue4" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut4" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment4" required="required" />
            </div></div>        
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue4()" value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog4()" value="Cancel" class="btn"/>  </div>
  </div></div>
    </div>
</dialog>
<!-- modal de 5eme heure-->
<input type="checkbox" id="openDialog5" style="display: none"/>
<dialog id="myDialog5" >
    <div><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H5</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue5" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut5" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment5" required="required" />
            </div></div>        
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue5()"  value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog5()" value="Cancel" class="btn"/>  </div>
  </div></div>
    </div>
</dialog>
<!-- modal de 6eme heure-->
<input type="checkbox" id="openDialog6" style="display: none"/>
<dialog id="myDialog6" >
    <div><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H6</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue6" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut6" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment6" required="required" />
            </div></div>        
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue6()" value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog6()" value="Cancel" class="btn"/>  </div>
  </div></div>
    </div>
</dialog>
<!-- modal de 7eme heure-->
<input type="checkbox" id="openDialog7" style="display: none"/>
<dialog id="myDialog7" >
    <div><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H7</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue7" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut7" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment7" required="required" />
            </div></div>        
               <input formmethod="dialog" type="submit"  onclick="updateLabelValue7()"  value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog7()" value="Cancel" class="btn"/>  </div>
  </div></div>
    </div>
</dialog>
<!-- modal de 8eme heure-->
<input type="checkbox" id="openDialog8" style="display: none"/>
<dialog id="myDialog8" >
    <div><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row">
          <div class="col-50">
            <h3>Production Hourly update: H8</h3>
            <label>Quantité totale des pièces réels:</label>
            <input type="number" placeholder="Quantité totale des pièces réels"  id="inputValue8" required="required" />
            <label>Quantité totale des pièces rebuts:</label>
            <input type="number" placeholder="Quantité totale des pièces rebuts" id="inputrubut8" required="required" />
            <label for="adr">Commentaires</label>
            <input type="text" placeholder="Commentaires" id="inputcomment8" required="required" />
            </div></div>        
        <input formmethod="dialog" type="submit"  onclick="updateLabelValue8()" value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog8()"  value="Cancel" class="btn"/></div>
  </div></div>
    </div>
</dialog>
<!-- modal pour definir la langue-->
<input type="checkbox" id="openDialog9" style="display: none"/>
<dialog id="myDialog9" >
    <div ><div class="row">
  <div class="col-75">
    <div class="containerModal">
        <div class="row" >
          <div class="col-50">
            <h3>Change language</h3>
            <label>Select a language:</label>
         <%--     <asp:DropDownList ID="DropDownListLanguages" class="inputdropdown" AppendDataBoundItems="true" runat="server" Width="153px">
     <asp:ListItem Text="English" Value="EN" />
     <asp:ListItem Text="French" Value="FR" />
</asp:DropDownList>--%>
            </div></div>        
        <input formmethod="dialog" type="submit"  onclick="updateLabelValue8()" value="Save" class="btn"/>
        <input formmethod="dialog" type="button" onclick="cancelDialog9()"  value="Cancel" class="btn"/></div>
  </div></div>
    </div>
</dialog>


	<div id="sideMenu" class="sideMenu" onclick="openNav()">
	
	<Tooltip title="Menu" arrow><span style="  color: white; cursor: pointer;  margin-top:  0px;"
			onclick="openNav()"><i style="font-size: 40px; margin-top:  -120px;  margin-left:  30px;" class="fa-solid fa-bars"></i></span></Tooltip>
	

		<div class="mainMenu" onclick="openNav()" style=" padding-top:75px;" >
			
			
			<a href="PB_page.aspx"
				onclick="showContent('Production Board')" ><i class="fa-solid fa-marker">&nbsp;</i><div id="HideItem">Production Board</div> </a>
			<a href="WeeklySyntheses.aspx"
				onclick="showContent('WEEKLY Synthese</')">  <i class="fa-solid fa-calendar-week">&nbsp; </i><div id="HideItem2">WEEKLY Synthese </div></a>
			<a href="FindBoard.aspx"
				onclick="showContent('Find a board')"> <i class="fa-solid fa-upload">&nbsp;</i><div id="HideItem3">Find a board</div></a>			
			<a href="javascript:void(0)" onclick="showContent('Language')" 
				for="openDialog9"> <i class="fa-solid fa-language" for="openDialog9">&nbsp;</i><label style="cursor: pointer;"id="HideItem4" for="openDialog9">Language </label></a>

            <a href="javascript:void(0)"
				onclick="location.reload()"> <i class="fa-solid fa-rotate">&nbsp;</i><div id="HideItem5">REFRECH</div></a>

			<a href="Login.aspx" 
                onclick="ChangeMode()"> <i id="signOut"  class="fa-solid fa-person-through-window">&nbsp;</i><label id="HideItem6" >Sign out</label></a>
            <a href="javascript:void(0)" 
                onclick="ChangeMode()"> <i id="updateelement"  class="fa-solid fa-person-through-window">&nbsp;</i></a></div>
	</div>
	
	<div id="contentArea"  >
		
			 <Tooltip title="Menu" arrow><span style="cursor: pointer; position: fixed; margin-top:  153px;"
			class="closeBtn" id="closeBtn" onclick="closeNav()"><i class="fa-solid fa-xmark" style="font-size: 47px; color: #ffffff;"></i></span></Tooltip>
			
			
		
		
		
		
		<div class="contentAreaCloseNav" onclick="closeNav()">
		<div class="contentText">
		<h5>
		<div>
		<table style="width:90%;" runat="server">
  <tr>
    <td class="auto-style1">
        <asp:Label ID="TeamLabel" runat="server" Text="Team"></asp:Label>

    </td>  
      <td class="auto-style1">
        <asp:Label ID="ProductLabel" runat="server" Text="Product Name"></asp:Label>

    </td>
    <td class="auto-style1">
        <asp:Label ID="PLLabel" runat="server" Text="Production Line"></asp:Label>

    </td>
  </tr>
  <tr>
    <td>
        <asp:Label ID="ShiftLabel" runat="server" Text="Shift"></asp:Label></td>
    <td>
        <asp:Label ID="DateLabel" runat="server" Text="Date"></asp:Label></td>
      <td> 
          <asp:Label ID="lblCOUCOU" runat="server" Text="coucou"></asp:Label>&nbsp;
          <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update btn" />
          <asp:Button ID="BTN2" runat="server" Text="Button 2" OnClick="BTN2_Click" />
      </td>
  </tr>    
</table>

</div>
		
		
		</h5>
	
			<table style="width:90%; margin-top:  -25px;  ">
        <tr>
            <th style="width:0%; background-color: none;color:white">
                <div class="timeIcon">
                   <i class="fa-solid fa-clock-rotate-left" alt="3Shifts" class="center"></i> 
                </div>
            </th>

            <th class="custom-row" style="width:0%;background-color: #6AABD2; text-align: center; background: linear-gradient(to top right, transparent calc(50% - 1px), White, transparent calc(50% + 1px)); background-color: #6AABD2;">
                <div class="dbox">
                    <div class="dheading_tr" style="color:white;top: -6px;">Objet</div>
                    <div class="dheading_bl"style="color:white; bottom: -6px;">Cumul</div>
                </div>
            </th>

            <th class="custom-row" style="width:0%;background-color: #6AABD2; text-align: center; background: linear-gradient(to top right, transparent calc(50% - 1px), White, transparent calc(50% + 1px)); background-color: #6AABD2;">
                <div class="dbox">
                    <div class="dheading_tr"style="color:white; top: -6px;">Reel</div>
                    <div class="dheading_bl"style="color:white; bottom: -6px;">Cumul</div>
                </div>
            </th>
            <th class="custom-row" style="width:0%;background-color: #6AABD2; text-align: center; background: linear-gradient(to top right, transparent calc(50% - 1px), White, transparent calc(50% + 1px)); background-color: #6AABD2;">

                <div class="dbox">
				
                    <div class="dheading_tr"style="color:white; top: -6px;">Rebut</div>
                    <div class="dheading_bl"style="color:white; bottom: -6px;">Cumul</div>
                </div>
            </th>
            <th style="width:80%; height:50px; background-color: #6AABD2;color:white" >Commentaires</th>
        </tr>
  
  <tr>
<td style="height:10px">
    <div class="round" >
        <asp:Label ID="h1Label" runat="server" Text="1st h"></asp:Label>
         </div>
</td>
	
	
         <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr">
        <asp:Label ID="h1Object" runat="server" Text="Obj1"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML1" runat="server" Text="Cml1"></asp:Label></div>
</div>
  </td>
  
         <td style="width:0%">
    <div class="dbox">
    
    <div class="dheading_tr" > <asp:Label runat="server" id="reel_h1" for="openDialog" Text="&emsp;"></asp:Label>
    </div>
    <div class="dheading_bl"><label id= "cumul_h1" for="openDialog">&emsp;</label></div>
</div>
  </td>
  
         <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "rubut_h1" for="openDialog">0</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h1"for="openDialog" >0 </label></div>
</div>
  </td>
  
  
   <td class= "Commentaire"><label id= "Commentaire_h1" for="openDialog" >&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  <tr>
    <td ><div class="round" >
        <asp:Label ID="h2Label" runat="server" Text="2nd h"></asp:Label>
    </div></td>
    
	 <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h2Object" runat="server" Text="Obj2"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML2" runat="server" Text="Cml2"></asp:Label></div>
</div>
  </td>
	
     <td style="width:0%"> 
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h2" for="openDialog2">00 </label> </div>
    <div class="dheading_bl"><label id= "cumul_h2" for="openDialog2"> 00 </label></div>
</div>
  </td>
    
	 <td style="width:0%">
    <div class="dbox">
        <div class="dheading_tr"><label id= "rubut_h2" for="openDialog2">00</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h2" for="openDialog2" > 00 </label></div>
</div>
  </td>
	
     <td class= "Commentaire"><label id= "Commentaire_h2" for="openDialog2">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  <tr>
    <td ><div class="round" >
                <asp:Label ID="h3Label" runat="server" Text="3rd h"></asp:Label>

    </div></td>
	
	
    <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h3Object" runat="server" Text="Obj3"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML3" runat="server" Text="Cml3"></asp:Label></div>
</div>
  </td>
	
	
     <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h3" for="openDialog3">0</label> </div>
    <div class="dheading_bl"><label id= "cumul_h3" for="openDialog3"  >0</label></div>
</div>
  </td>
	
	
     <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "rubut_h3" for="openDialog3">0</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h3" for="openDialog3"> 0</label></div>
</div>
  </td>
	
	
    <td class= "Commentaire"><label id= "Commentaire_h3" for="openDialog3">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  
  
  <tr>
    <td ><div class="round" >
    <asp:Label ID="h4Label" runat="server" Text="4th h"></asp:Label>

    </div></td>
 <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h4Object" runat="server" Text="Obj4"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML4" runat="server" Text="Cml4"></asp:Label></div>
</div>
  </td>

  
   <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h4" for="openDialog4">00</label> </div>
    <div class="dheading_bl"><label id= "cumul_h4" for="openDialog4">00</label></div>
</div>
  </td>
  
   <td style="width:0%">
    <div class="dbox">
        <div class="dheading_tr"><label id= "rubut_h4" for="openDialog4">00</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h4" for="openDialog4"> 00 </label></div>
</div>
  </td>
  
    <td class= "Commentaire"><label id= "Commentaire_h4" for="openDialog4">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  <tr>
    <td ><div class="round" >
    <asp:Label ID="h5Label" runat="server" Text="5th h"></asp:Label>

    </div></td>
	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h5Object" runat="server" Text="Obj5"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML5" runat="server" Text="Cml5"></asp:Label></div>
</div>
  </td>
	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h5" for="openDialog5">00</label> </div>
    <div class="dheading_bl"><label id= "cumul_h5" for="openDialog5">00</label></div>
</div>
  </td>
	
    <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "rubut_h5" for="openDialog5">00</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h5" for="openDialog5"> 00 </label></div>
</div>
  </td>
	
       <td class= "Commentaire"><label id= "Commentaire_h5" for="openDialog5">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  <tr>
<td ><div class="round" >
    <asp:Label ID="h6Label" runat="server" Text="6th h"></asp:Label>
    </div></td>	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h6Object" runat="server" Text="Obj6"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML6" runat="server" Text="Cml6"></asp:Label></div>
</div>
  </td>
	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h6" for="openDialog6">00</label> </div>
    <div class="dheading_bl"><label id= "cumul_h6" for="openDialog6">00</label></div>
</div>
  </td>
	
    <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "rubut_h6" for="openDialog6">00</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h6" for="openDialog6"> 00 </label></div>
</div>
  </td>
	
    <td class= "Commentaire"><label id= "Commentaire_h6" for="openDialog6">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  
<tr>
<td ><div class="round" >
    <asp:Label ID="h7Label" runat="server" Text="7th h"></asp:Label>
    </div></td>	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h7Object" runat="server" Text="Obj7"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML7" runat="server" Text="Cml7"></asp:Label></div>
</div>
  </td>
	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h7" for="openDialog7">00</label> </div>
    <div class="dheading_bl"><label id= "cumul_h7" for="openDialog7">00</label></div>
</div>
  </td>
	
    <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "rubut_h7" for="openDialog7">00</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h7" for="openDialog7"> 00 </label></div>
</div>
  </td>
	
       <td class= "Commentaire"><label id= "Commentaire_h7" for="openDialog7">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
  <tr>
<td ><div class="round" >
    <asp:Label ID="h8Label" runat="server" Text="8th h"></asp:Label>
    </div></td>	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><asp:Label ID="h8Object" runat="server" Text="Obj8"></asp:Label></div>
    <div class="dheading_bl"><asp:Label ID="OBJ_CML8" runat="server" Text="Cml8"></asp:Label></div>
</div>
  </td>
	
	<td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "reel_h8" for="openDialog8">0</label> </div>
    <div class="dheading_bl"><label id= "cumul_h8" for="openDialog8" >0</label></div>
</div>
  </td>
	
    <td style="width:0%">
    <div class="dbox">
    <div class="dheading_tr"><label id= "rubut_h8" for="openDialog8">0</label></div>
    <div class="dheading_bl"><label id= "cumulrubut_h8" for="openDialog8"> 0 </label></div>
</div>
  </td>
	
        <td class= "Commentaire"><label id= "Commentaire_h8" for="openDialog8">&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
   &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</label></td>
  </tr>
</table>




		</div>
	</div>
	</div>
    </form>

     <script src="JavaScript.js"></script>
</body>
</html>
