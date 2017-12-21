$(document).ready(function () {
    $('.popUpForm').hide();
    $('#loginToggle').click(function(){
        $('.popUpForm').fadeToggle(500);
    });
});