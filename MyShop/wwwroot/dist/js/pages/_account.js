
//load layout javascript
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.persianNumberParse();
    layout.passwordVisible();
    layout.preventWriteString();
})();

//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();

//when write mobile verification code
(async () => {
    let verifyInputs = document.querySelectorAll('.verify-input-digit');
    if (!verifyInputs) {
        return;
    }
    verifyInputs.forEach((elem, index) => {

        elem.addEventListener('keydown', e => {
            let key = e.keyCode || e.which;
            if (key !== 9 && key !== 8 && ((key < 48 || key > 57) && !(key >= 96 && key <= 105))) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        }, false);
        elem.addEventListener('keyup', e => {
            let key = e.keyCode || e.which;
            if (key !== 9 && key !== 8 && ((key < 48 || key > 57) && !(key >= 96 && key <= 105))) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            if ((verifyInputs.length - 1) > index) {
                verifyInputs[index + 1].focus();
            } else {
                let submitButton = document.querySelector('#verify-submit-button');
                submitButton.focus();
            }
        }, false);
    });
    let verifySubmitButton = document.querySelector('#verify-submit-button');
    verifySubmitButton.addEventListener('click', e => {
        let tokenCode = "";
        verifyInputs.forEach(elem => {
            tokenCode += elem.value;
        });
        let inputToken = document.querySelector('#Input_Token');
        inputToken.value = tokenCode;

    }, false);
})();



//change persian number of mobile number input to english number
(async () => {
    let changeNumbers = (text) => {
        if (!text) {
            return false;
        }
        let persian = { '۰': 0, '۱': 1, '۲': 2, '۳': 3, '۴': 4, '۵': 5, '۶': 6, '۷': 7, '۸': 8, '۹': 9 };
        let list = text.match(/[۰۱۲۳۴۵۶۷۸۹]/g)
        if (list !== null && list.length !== 0) {
            for (let i = 0; i < list.length; i++) {
                text = text.replace(list[i], persian[list[i]]);
            }
        }
        return text;
    };
    //login form
    let loginForm = document.querySelector('.login-form');
    if (loginForm) {
        let submitButton = loginForm.querySelector('.submit-button');
        if (submitButton) {
            submitButton.addEventListener('click', (e) => {
                let mobileInput = loginForm.querySelector('input[name="Input.Mobile"]');
                let mobileInputValue = mobileInput.value;
                mobileInput.value = changeNumbers(mobileInputValue);
            }, false);
        }
    }

    //register form
    let registerForm = document.querySelector('.register-form');
    if (registerForm) {
        let submitButton = registerForm.querySelector('.submit-button');
        if (submitButton) {
            submitButton.addEventListener('click', (e) => {
                let mobileInput = registerForm.querySelector('input[name="Input.Mobile"]');
                let mobileInputValue = mobileInput.value;
                mobileInput.value = changeNumbers(mobileInputValue);
            }, false);
        }
    }
})();