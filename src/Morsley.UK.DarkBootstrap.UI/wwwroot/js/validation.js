(function () {

    'use strict';
    
    // Email validation via AJAX
    function validateEmailViaAjax(email, callback) {
        fetch('/Message/ValidateEmail?email=' + encodeURIComponent(email))
            .then(response => response.json())
            .then(data => callback(data.isValid))
            .catch(error => {
                console.error('Email validation error:', error);
                callback(false);
            });
    }
    
    // Validation function called when send button is clicked
    function validateForm() {

        console.log('Validate form called');

        debugger;

        var form = $('.needs-validation')[0];
        var emailInput = $('#email');
        var email = emailInput.val().trim();
        
        // Check HTML5 validation first
        if (!form.checkValidity()) {
            $(form).addClass('was-validated');
            return false;
        }
        
        // If email is empty, allow submit
        if (email === '') {
            console.log('No email provided, submitting form');
            return true;
        }
        
        // Validate email via AJAX
        console.log('Validating email:', email);
        validateEmailViaAjax(email, function(isValid) {
            console.log('Email validation result:', isValid);
            
            if (isValid) {
                emailInput[0].setCustomValidity('');
                emailInput.removeClass('is-invalid').addClass('is-valid');
                $(form).addClass('was-validated');
                form.submit();
            } else {
                emailInput[0].setCustomValidity('Please enter a valid email address');
                emailInput.removeClass('is-valid').addClass('is-invalid');
                $(form).addClass('was-validated');
            }
        });
        
        return false; // Prevent default submit while AJAX is processing
    }
    
    // Intercept send button click
    $(document).ready(function () {

        console.log('jQuery ready - setting up send button handler');
        
        $('button[type="submit"]').on('click', function(e) {
            console.log('Send button clicked');
            e.preventDefault();
            validateForm();
        });
        
        // Also intercept form submit event
        $('.needs-validation').on('submit', function(e) {
            console.log('Form submit event');
            e.preventDefault();
            validateForm();
        });

    });

})();