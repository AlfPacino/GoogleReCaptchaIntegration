// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function googleCaptchaV2Loaded() {
    console.log('v2 loaded');
}

function googleCaptchaV3Loaded() {
    console.log('v3 loaded');
}

function execV3Check() {
    grecaptcha.execute('6LexQtsUAAAAAPjsw_jBTa1P8htzKIgBn1Bgx0kz', { action: 'homepage' }).then(function (token) {
        $.ajax({
            async: false,
            method: 'POST',
            url: 'Captcha/Home/ValidateV3',
            data: { token: token }
        }).done((res) => {
            if (res.Success === true) {
                // ...
            }
        });
    });
}

$(function () {
    grecaptcha.ready(execV3Check);

    $('#submit_form_btn_v2').click((e) => {
        e.preventDefault();
        $.ajax({
            async: false,
            method: 'POST',
            url: 'Captcha/Home/ValidateV2',
            data: { token: grecaptcha.getResponse() }
        }).done((res) => {
            if (res.Success === true) {
                // submit form ... proceed ... etc
            }
        });
    });

    $('#submit_form_btn_v3').click((e) => {
        e.preventDefault();
        execV3Check();
    });
});