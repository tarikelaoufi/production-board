
        window.onbeforeunload = function () {
            return "Data will be lost if you refrech the page, are you sure?";
        };


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


    document.getElementById("openDialog").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside);
        }
    });

    document.getElementById("openDialog2").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog2");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside2);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside2);
        }
    });

    document.getElementById("openDialog3").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog3");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside3);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside3);
        }
    });
    document.getElementById("openDialog4").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog4");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside4);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside4);
        }
    });
    document.getElementById("openDialog5").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog5");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside5);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside5);
        }
    });
    document.getElementById("openDialog6").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog6");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside6);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside6);
        }
    });
    document.getElementById("openDialog7").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog7");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside7);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside7);
        }
    });
    document.getElementById("openDialog8").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog8");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside8);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside8);
        }
    });
    // Language Modal
    document.getElementById("openDialog9").addEventListener("change", function () {
        var dialog = document.getElementById("myDialog9");

        if (this.checked) {
            dialog.showModal();
            document.addEventListener("click", closeDialogOutside9);
        } else {
            dialog.close();
            document.removeEventListener("click", closeDialogOutside9);
        }
    });


    function cancelDialog() { var dialog = document.getElementById("myDialog"); if (dialog) { dialog.close(); } }
    function cancelDialog2() { var dialog2 = document.getElementById("myDialog2"); if (dialog2) { dialog2.close(); } }
    function cancelDialog3() { var dialog3 = document.getElementById("myDialog3"); if (dialog3) { dialog3.close(); } }
    function cancelDialog4() { var dialog4 = document.getElementById("myDialog4"); if (dialog4) { dialog4.close(); } }
    function cancelDialog5() { var dialog5 = document.getElementById("myDialog5"); if (dialog5) { dialog5.close(); } }
    function cancelDialog6() { var dialog6 = document.getElementById("myDialog6"); if (dialog6) { dialog6.close(); } }
    function cancelDialog7() { var dialog7 = document.getElementById("myDialog7"); if (dialog7) { dialog7.close(); } }
    function cancelDialog8() { var dialog8 = document.getElementById("myDialog8"); if (dialog8) { dialog8.close(); } }
    function cancelDialog9() { var dialog9 = document.getElementById("myDialog9"); if (dialog9) { dialog9.close(); } }

    function closeDialogOutside(e) {
        var dialog = document.getElementById("myDialog");
        var dialogDimensions = dialog.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions.left ||
            e.clientX > dialogDimensions.right ||
            e.clientY < dialogDimensions.top ||
            e.clientY > dialogDimensions.bottom
        ) {
            dialog.close();
        }
    } function closeDialogOutside2(e) {
        var dialog2 = document.getElementById("myDialog2");
        var dialogDimensions2 = dialog2.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions2.left ||
            e.clientX > dialogDimensions2.right ||
            e.clientY < dialogDimensions2.top ||
            e.clientY > dialogDimensions2.bottom
        ) {
            dialog2.close();
        }
    }
    function closeDialogOutside3(e) {
        var dialog3 = document.getElementById("myDialog3");
        var dialogDimensions3 = dialog3.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions3.left ||
            e.clientX > dialogDimensions3.right ||
            e.clientY < dialogDimensions3.top ||
            e.clientY > dialogDimensions3.bottom
        ) {
            dialog3.close();
        }
    }
    function closeDialogOutside4(e) {
        var dialog4 = document.getElementById("myDialog4");
        var dialogDimensions4 = dialog4.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions4.left ||
            e.clientX > dialogDimensions4.right ||
            e.clientY < dialogDimensions4.top ||
            e.clientY > dialogDimensions4.bottom
        ) {
            dialog4.close();
        }
    }
    function closeDialogOutside5(e) {
        var dialog5 = document.getElementById("myDialog5");
        var dialogDimensions5 = dialog5.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions5.left ||
            e.clientX > dialogDimensions5.right ||
            e.clientY < dialogDimensions5.top ||
            e.clientY > dialogDimensions5.bottom
        ) {
            dialog5.close();
        }
    }
    function closeDialogOutside6(e) {
        var dialog6 = document.getElementById("myDialog6");
        var dialogDimensions6 = dialog6.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions6.left ||
            e.clientX > dialogDimensions6.right ||
            e.clientY < dialogDimensions6.top ||
            e.clientY > dialogDimensions6.bottom
        ) {
            dialog6.close();
        }
    }
    function closeDialogOutside7(e) {
        var dialog7 = document.getElementById("myDialog7");
        var dialogDimensions7 = dialog7.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions7.left ||
            e.clientX > dialogDimensions7.right ||
            e.clientY < dialogDimensions7.top ||
            e.clientY > dialogDimensions7.bottom
        ) {
            dialog7.close();
        }
    }

    function closeDialogOutside8(e) {
        var dialog8 = document.getElementById("myDialog8");
        var dialogDimensions8 = dialog8.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions8.left ||
            e.clientX > dialogDimensions8.right ||
            e.clientY < dialogDimensions8.top ||
            e.clientY > dialogDimensions8.bottom
        ) {
            dialog8.close();
        }
    }

    function closeDialogOutside9(e) {
        var dialog9 = document.getElementById("myDialog9");
        var dialogDimensions9 = dialog9.getBoundingClientRect();

        if (
            e.clientX < dialogDimensions9.left ||
            e.clientX > dialogDimensions9.right ||
            e.clientY < dialogDimensions9.top ||
            e.clientY > dialogDimensions9.bottom
        ) {
            dialog9.close();
        }
    }


    function updateLabelValue() {
        var inputValue = document.getElementById("inputValue").value;
        var inputrubut = document.getElementById("inputrubut").value;
        var inputcomment = document.getElementById("inputcomment").value;

        var label = document.querySelector('label[for="openDialog"]');

        reel_h1.textContent = Number(inputValue);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);

        rubut_h1.textContent = Number(inputrubut);

        cumulrubut_h1.textContent = Number(inputrubut);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h1.textContent = inputcomment;



        var dialog = document.getElementById("myDialog");

        dialog.close();



    }

    function updateLabelValue2() {
        var inputValue2 = document.getElementById("inputValue2").value;
        var inputrubut2 = document.getElementById("inputrubut2").value;
        var inputcomment2 = document.getElementById("inputcomment2").value;
        var label2 = document.querySelector('label2[for="openDialog2"]');

        reel_h2.textContent = Number(inputValue2);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h2.textContent = Number(inputrubut2);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h2.textContent = inputcomment2;


        var dialog2 = document.getElementById("myDialog2");
        dialog2.close();
    }

    function updateLabelValue3() {
        var inputValue3 = document.getElementById("inputValue3").value;
        var inputrubut3 = document.getElementById("inputrubut3").value;
        var inputcomment3 = document.getElementById("inputcomment3").value;

        var label3 = document.querySelector('label3[for="openDialog3"]');



        reel_h3.textContent = Number(inputValue3);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h3.textContent = Number(inputrubut3);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h3.textContent = inputcomment3;


        var dialog3 = document.getElementById("myDialog3");
        dialog3.close();
    }
    function updateLabelValue4() {
        var inputValue4 = document.getElementById("inputValue4").value;
        var inputrubut4 = document.getElementById("inputrubut4").value;
        var inputcomment4 = document.getElementById("inputcomment4").value;


        var label4 = document.querySelector('label4[for="openDialog4"]');

        reel_h4.textContent = Number(inputValue4);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h4.textContent = Number(inputrubut4);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h4.textContent = inputcomment4;


        var dialog4 = document.getElementById("myDialog4");
        dialog4.close();
    }
    function updateLabelValue5() {
        var inputValue5 = document.getElementById("inputValue5").value;
        var inputrubut5 = document.getElementById("inputrubut5").value;
        var inputcomment5 = document.getElementById("inputcomment5").value;


        var label5 = document.querySelector('label5[for="openDialog5"]');


        reel_h5.textContent = Number(inputValue5);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h5.textContent = Number(inputrubut5);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h5.textContent = inputcomment5;


        var dialog5 = document.getElementById("myDialog5");
        dialog5.close();
    }
    function updateLabelValue6() {
        var inputValue6 = document.getElementById("inputValue6").value;
        var inputrubut6 = document.getElementById("inputrubut6").value;
        var inputcomment6 = document.getElementById("inputcomment6").value;
        var label6 = document.querySelector('label6[for="openDialog6"]');

        reel_h6.textContent = Number(inputValue6);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h6.textContent = Number(inputrubut6);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h6.textContent = inputcomment6;


        var dialog6 = document.getElementById("myDialog6");
        dialog6.close();
    }
    function updateLabelValue7() {
        var inputValue7 = document.getElementById("inputValue7").value;
        var inputrubut7 = document.getElementById("inputrubut7").value;
        var inputcomment7 = document.getElementById("inputcomment7").value;

        var label7 = document.querySelector('label7[for="openDialog7"]');

        reel_h7.textContent = Number(inputValue7);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h7.textContent = Number(inputrubut7);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h7.textContent = inputcomment7;


        var dialog7 = document.getElementById("myDialog7");
        dialog7.close();
    }
    function updateLabelValue8() {
        var inputValue8 = document.getElementById("inputValue8").value;
        var inputrubut8 = document.getElementById("inputrubut8").value;
        var inputcomment8 = document.getElementById("inputcomment8").value;
        var label8 = document.querySelector('label8[for="openDialog8"]');



        reel_h8.textContent = Number(inputValue8);

        cumul_h1.textContent = Number(reel_h1.textContent);
        cumul_h2.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent);
        cumul_h3.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent);
        cumul_h4.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent);
        cumul_h5.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent);
        cumul_h6.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent);
        cumul_h7.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent);
        cumul_h8.textContent = Number(reel_h1.textContent) + Number(reel_h2.textContent) + Number(reel_h3.textContent) + Number(reel_h4.textContent) + Number(reel_h5.textContent) + Number(reel_h6.textContent) + Number(reel_h7.textContent) + Number(reel_h8.textContent);


        rubut_h8.textContent = Number(inputrubut8);

        cumulrubut_h1.textContent = Number(rubut_h1.textContent);
        cumulrubut_h2.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent);
        cumulrubut_h3.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent);
        cumulrubut_h4.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent);
        cumulrubut_h5.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent);
        cumulrubut_h6.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent);
        cumulrubut_h7.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent);
        cumulrubut_h8.textContent = Number(rubut_h1.textContent) + Number(rubut_h2.textContent) + Number(rubut_h3.textContent) + Number(rubut_h4.textContent) + Number(rubut_h5.textContent) + Number(rubut_h6.textContent) + Number(rubut_h7.textContent) + Number(rubut_h8.textContent);


        Commentaire_h8.textContent = inputcomment8;


        var dialog8 = document.getElementById("myDialog8");
        dialog8.close();
    }




        function compareLabels() {
            var obj1 = document.getElementById("h1Object").textContent;
            var reel1 = document.getElementById("reel_h1").textContent;
           

        if (obj1 === reel1) {
            document.getElementById("reel_h1").style.color = "green"; // Set color to green if labels are equal
            document.getElementById("cumul_h1").style.color = "green"; // Set color to green if labels are equal
    } else {
            document.getElementById("reel_h1").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h1").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h2").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h3").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h4").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h5").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h6").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h7").style.color = "red"; // Set color to red if labels are not equal
            document.getElementById("cumul_h8").style.color = "red"; // Set color to red if labels are not equal
    }
}








