$(document).ready(function () {
    $(function () {
        $("#txtFromDate").datepicker({
            dateFormat: 'dd-mm-yy',
            minDate: 0,
            buttonImage: '../../../images/calenderIcon.png',
            showOn: 'button'

        });
    });
    $(function () {

        $("#txtDoB").datepicker({
            dateFormat: 'dd-mm-yy',
            buttonImage: '../../../images/calenderIcon.png',
            showOn: 'button'
        });

    });
});

function setMinDate() {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd;
    /*document.getElementById('<%= txtFromDate.ClientID %>').setAttribute('min', today);*/

};

//email regular val

$(document).ready(function () {
    $("#txtEmail").on("change", function () {
        const emailInput = $(this).val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (emailRegex.test(emailInput)) {
            $("#error").text("")
        } else {
            $("#error").text("Please enter a valid email address.").css("color", "red");
        }
    });
});

$(document).ready(function () {
    $("#txtMobile").on("change", function () {
        const mobileInput = $(this).val();
        const mobileRegex = /^[0-9]{10}$/; 
        if (mobileRegex.test(mobileInput)) {
            $("#mobileError").text(""); 
        } else {
            $("#mobileError").text("Please enter a valid 10-digit mobile number.").css("color", "red");
        }
    });
});


