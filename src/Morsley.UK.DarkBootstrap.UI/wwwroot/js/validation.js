(function () {

    'use strict';

    const nameInputId = 'Name';
    const emailInputId = 'EmailAddress';
    const selectedCountryCodeInputId = 'SelectedCountryCode';
    const mobileInputId = 'MobileNumber';
    const messageInputId = 'Message';
    
    $(document).ready(function () {

        let nameInput = $('#' + nameInputId);
        $(nameInput).on('input', function () {
            clearValidation();
        });

        let emailInput = $('#' + emailInputId);
        $(emailInput).on('input', function () {
            clearValidation();            
        });

        let countryCodeSelect = $('#SelectedCountryCode');
        let detectedCountryCode = $('#DetectedCountryCode').val();
        if (detectedCountryCode) {
            countryCodeSelect.find('option').each(function () {
                const value = $(this).val();
                if (value === detectedCountryCode) {
                    $(this).prop('selected', true);
                    return false;
                }
            });
        }
       
        let selectedCountryCodeInput = $('#' + selectedCountryCodeInputId);
        $(selectedCountryCodeInput).on('input', function () {
            clearValidation();
        });

        let mobileInput = $('#' + mobileInputId);        
        $(mobileInput).on('input', function () {
            clearValidation();
        });

        let messageInput = $('#' + messageInputId);
        $(messageInput).on('input', function () {
            clearValidation();
        });

    });

    function clearValidation() {
        let nameInput = $('#' + nameInputId);
        let emailInput = $('#' + emailInputId);        
        let selectedCountryCodeInput = $('#' + selectedCountryCodeInputId);
        let mobileInput = $('#' + mobileInputId);
        let messageInput = $('#' + messageInputId);
        $(nameInput).removeClass("is-valid").removeClass("is-invalid");
        $(emailInput).removeClass("is-valid").removeClass("is-invalid");
        $(selectedCountryCodeInput).removeClass("is-valid").removeClass("is-invalid");
        $(mobileInput).removeClass("is-valid").removeClass("is-invalid");
        $(messageInput).removeClass("is-valid").removeClass("is-invalid");
    }

})();