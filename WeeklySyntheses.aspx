<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeeklySyntheses.aspx.cs" Inherits="PFF.WeeklySyntheses" %>

<!DOCTYPE html>
<html lang="en">

<head>
	<meta charset="UTF-8" />
	<meta name="viewport" content=
		"width=device-width, initial-scale=1.0" />
	<link rel="stylesheet" href="style.css" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" integrity="sha512-SnH5WK+bZxgPHs44uWIX+LLJAJ9/2PkPKZ5QiAj6Ta86w+fsb2TkcmfRyVX3pBnMFcV7oQPJkl9QevSCWr3W6A==" crossorigin="anonymous" referrerpolicy="no-referrer" />	
<title>Weekly SYNTHESE</title>
</head></head>
<style>
body{

  font-family: Arial;
  font-size: 17px;
  padding: 8px;


background-image: linear-gradient(#6dd5ed,#1c92d2);
background-repeat: no-repeat;
  background-attachment: fixed;  
  background-size: cover;
}
table, th, td {
  border: 1px solid #dddddd; <!--light grey border -->;
  border-collapse: collapse;   
color :White;  
}

 /* Style for the dialog */
    #myDialog, #myDialog2, #myDialog3, #myDialog4, #myDialog5, #myDialog6, #myDialog7, #myDialog8, #myDialog9 {
	border:none;
    background-color: White;
width: 350px; /* Set the width */
        height: 400px; /* Set the height */    }
	
	  
	  
 /* Form styling CSS */



</style>



<style>



<style>
/* style.css */

	
	
#HideItem, #HideItem2, #HideItem3, #HideItem4, #HideItem5, #HideItem6, #HideItem7, #closeBtn{

display:none}

#sidebar-close {
  display: none;
  color: #333;
}
.sideMenu {
	height: 100%;
	width: 100px;
	position: fixed;
	margin-left = 10px;
	z-index: 1;
	top: 0;
	left: 0;
	overflow-x: hidden;
	transition: 0.2s;
	padding-top: 40px;
}

.mainMenu h2 {
	text-align: center;
	letter-spacing: 7px;
	color: #6AABD2;
	padding: 10px 0;
	
}

.sideMenu a {
	padding:  0px 0px 1px 32px;
  text-decoration: underline;
	color: #fff;
	display: block;
	transition: 0.3s;
	font-size: 18px;
	margin-bottom: 10px;
	text-transform: uppercase;
	font-weight: bold;
	}

.mainMenu a:hover {
	color: #fff;
		  text-decoration: underline;


}

.sideMenu .closeBtn {
	position: absolute;
	top: 0;
	right: 25px;
	font-size: 70px;
	margin-left: 50px;
	margin-top: -15px;
}

#contentArea {
	transition: margin-left 0.5s;
 margin-left: 100px; 
}

.contentText {
	padding: 100px 20px;
	text-align: center;
  margin-bottom: 0px;
  margin-right: 180px;
  margin-left: 80px; 
  margin-top: -140px;

}

.contentText h2 {
		border : none;

	display: inline-block;
	padding: 15px 10px;
	text-transform: uppercase;
	font-size: 24px;
	color: #fff;
}

.contentText h3 {
	text-transform: uppercase;
	font-size: 18px;
	margin: 0;
	letter-spacing: 3px;
}
/*@media screen and (min-width: 768px) {
	.contentText {
		padding: 100px 180px;
	}*/
	.contentText h2 {
		padding: 15px 65px;
		font-size: 50px;
	}
	.contentText h3 {
		font-size: 45px;
	}
	i{	font-size: 25px;}

}	.timeIcon{	font-size: 165px;}

}
}

</style>
<style>
body {
  font-family: Arial;
  font-size: 17px;
  padding: 8px;
  margin: 0;

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

.col-25 {
  -ms-flex: 25%; /* IE10 */
  flex: 25%;
}

.col-50 {
  -ms-flex: 500%; /* IE10 */
  flex: 50%;
}

.col-75 {
  -ms-flex: 70%; /* IE10 */
  flex: 70%;
}

.col-25,
.col-50,
.col-75 {
  padding: 0 16px;
}

.containermodal {
background-color: transparent; /* Makes the background color transparent */
    display: none; /* Hides the element from displaying on the webpage */
  background-color: display-none;
  padding: 5px 20px 15px 20px;
  border: 1px solid lightgrey;
  border-radius: 3px;
  width: 50%; margin:auto;
}

input[type=text],input[type=number] {
  width: 100%;
  margin-bottom: 10px;
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 1.5px;
}



.icon-containermodal {
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


span.price {
  float: right;
  color: grey;
}

/* Responsive layout - when the screen is less than 800px wide, make the two columns stack on top of each other instead of next to each other (also change the direction - make the "cart" column go on top) */
@media (max-width: 800px) {
  .row {
    flex-direction: column-reverse;
  }
  .col-25 {
    margin-bottom: 20px;
  }
}
</style>
<style>



 /* Style for the dialog */
    #myDialog, #myDialog2, #myDialog3, #myDialog4, #myDialog5, #myDialog6, #myDialog7, #myDialog8 {
	border:none;
    background-color: White;
width: 350px; /* Set the width */
        height: 400px; /* Set the height */    }
	
	  
	  
 /* Form styling CSS */



</style>




<style>


	
#HideItem, #HideItem2, #HideItem3, #HideItem4, #HideItem5, #HideItem6, #HideItem7, #closeBtn{

display:none}

#sidebar-close {
  display: none;
  color: #333;
}
.sideMenu {
	height: 100%;
	width: 100px;
	position: fixed;
	margin-left = 10px;
	z-index: 1;
	top: 0;
	left: 0;
	background: none;
	overflow-x: hidden;
	transition: 0.2s;
	padding-top: 40px;
}

.mainMenu h2 {
	text-align: center;
	letter-spacing: 7px;
	color: #6AABD2;
	border : solid 1px #f9f9f9;
	padding: 10px 0;
	
}

.sideMenu a {
	padding:  0px 0px 1px 32px;
	text-decoration: none;
	color: #fff;
	display: block;
	transition: 0.3s;
	font-size: 18px;
	margin-bottom: 10px;
	text-transform: uppercase;
	font-weight: bold;
	}

.mainMenu a:hover {
	color: #fff;

}

.sideMenu .closeBtn {
	position: absolute;
	top: 0;
	right: 25px;
	font-size: 70px;
	margin-left: 50px;
	margin-top: -15px;
}

#contentArea {
	transition: margin-left 0.5s;
 margin-left: 100px; 
}


.title_label{
 margin-right: 100px; /* Adjust as needed */
 
}

.contentText {
	padding: 100px 20px;
  margin-bottom: 0px;
  margin-right: 180px;
  margin-left: 80px; 
  margin-top: -140px;

}

.contentText h5 {
	padding: 10px 10px;
	text-transform: uppercase;
	font-size: 24px;
	color: #fff;
}

.contentText h3 {
	text-transform: uppercase;
	font-size: 18px;
	margin: 0;
	letter-spacing: 3px;
}
/*@media screen and (min-width: 768px) {
	.contentText {
		padding: 100px 180px;
	}*/
	.contentText h4 {
		padding: 15px 65px;
		font-size: 50px;
	}
	.contentText h3 {
		font-size: 45px;
	}
	i{	font-size: 25px;}

}	.timeIcon{	font-size: 165px;}

}
}

</style>
<style>




.row {
  display: -ms-flexbox; /* IE10 */
  display: flex;
  -ms-flex-wrap: wrap; /* IE10 */
  flex-wrap: wrap;
  margin: 0 -16px;
}

.col-25 {
  -ms-flex: 25%; /* IE10 */
  flex: 25%;
}

.col-50 {
  -ms-flex: 500%; /* IE10 */
  flex: 50%;
}

.col-75 {
  -ms-flex: 70%; /* IE10 */
  flex: 70%;
}

.col-25,
.col-50,
.col-75 {
  padding: 0 16px;
}

.containermodal {
background-color: transparent; /* Makes the background color transparent */
    display: none; /* Hides the element from displaying on the webpage */
  background-color: display-none;
  padding: 5px 20px 15px 20px;
  border: 1px solid lightgrey;
  border-radius: 3px;
  width: 50%; margin:auto;
}

input[type=text],input[type=number] {
  width: 100%;
  margin-bottom: 10px;
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 1.5px;
}



.icon-containermodal {
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


span.price {
  float: right;
  color: grey;
}

/* Responsive layout - when the screen is less than 800px wide, make the two columns stack on top of each other instead of next to each other (also change the direction - make the "cart" column go on top) */
@media (max-width: 800px) {
  .row {
    flex-direction: column-reverse;
  }
  .col-25 {
    margin-bottom: 20px;
  }
}
</style>
<body>









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
		</div>
	</div>
	

			<div id="contentArea"  >
			 <Tooltip title="Menu" arrow><span style="cursor: pointer; position: fixed; margin-top:  153px;"
			class="closeBtn" id="closeBtn" onclick="closeNav()"><i class="fa-solid fa-xmark" style="font-size: 47px; color: #ffffff;"></i></span></Tooltip>
			
			
		
		
		
		
		<div class="contentAreaCloseNav" onclick="closeNav()">
		<div class="contentText"> <h2 style="text-align: left; color:#6AABD2; background: White"> Weekly Synthesis </h2>						
			
			
		

<table style="width:60%;">
 <div class="th-color" >
  <tr>
 
    <th class="th-color">Milleur equipe <br> semaine 1</th>
    <th class="th-color">Equipe</th> 
    <th class="th-color">Lundi</th>
    <th class="th-color">Mardi</th>
    <th class="th-color">Mercredi</th>
    <th class="th-color">Jeudi</th>
    <th class="th-color">Vendredi</th>
    <th class="th-color">Samedi</th>
	
  </tr>
  	 </div>
  <tr>
       <td>A</td>
    <td>Equipe A</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
  </tr>
  <tr>
     <td>B</td>
    <td>Equipe B</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
  </tr>
  <tr>
    <td>C</td>
    <td>Equipe C</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
    <td>445</td>
  </tr>
  
</table>




		</div>
	</div>
	</div>
	<script>
    function openNav() {
	document.getElementById("sideMenu")
		.style.width = "300px";
	document.getElementById("contentArea")
		.style.marginLeft = "300px";
		document.getElementById("HideItem").style.display = 'unset ';
		document.getElementById("HideItem2").style.display = 'unset ';
		document.getElementById("HideItem3").style.display = 'unset';
		document.getElementById("HideItem4").style.display = 'unset';
		document.getElementById("HideItem5").style.display = 'unset';
		document.getElementById("HideItem6").style.display = 'unset';
		document.getElementById("closeBtn").style.display = 'unset';
		
}

function closeNav() {
	document.getElementById("sideMenu").style.width = "100px";
	document.getElementById("contentArea").style.marginLeft = "100px";
	document.getElementById("HideItem").style.display = 'none';
	document.getElementById("HideItem2").style.display = 'none';
	document.getElementById("HideItem3").style.display = 'none';
	document.getElementById("HideItem4").style.display = 'none';
	document.getElementById("HideItem5").style.display = 'none';
	document.getElementById("HideItem6").style.display = 'none';
	document.getElementById("closeBtn").style.display = 'none';
}

function showContent(content) {
	document.getElementById("contentTitle")
		.textContent = content + " page";
				

		
	closeNav();
}


    </script>
	
	
</body>

</html>
