
//load layout javascript
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.topMenuFunction();
    layout.persianNumberParse();
    layout.mobileMenuCategoryNavigation();
    layout.menuCategoryNavigation();
    layout.passwordVisible();
    layout.convertPersianNumbersToEnglish();
})();

//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();



//commafy / ajaxLoader / persian numbers parse
import { persianNumberParse, scrollToTop, lazyLoading, ajaxLoader} from './_layout.js';
lazyLoading();
//check message validate in send new message
function validateSendNewMessage() {
    let customerForm = document.querySelector('.customer-form');
    if (!customerForm) {
        return false;
    }
    let title = customerForm.querySelector('input[name="ticketSubject"]');
    let description = customerForm.querySelector('textarea[name="ticketDescription"]');
    let submitButton = customerForm.querySelector('.send-ticket-submit-button');
    if (submitButton) {
        submitButton.addEventListener('click', (e) => {

            if (title.value === null || title.value.trim() === "") {
                e.stopPropagation();
                e.preventDefault();
                let errorTitle = customerForm.querySelector('.ticketSubject-error');
                errorTitle.classList.remove('d-none');
                errorTitle.innerHTML = "پر کردن این فیلد الزامی است";
            }

            if (description.value === null || description.value.trim() === "") {
                e.stopPropagation();
                e.preventDefault();
                let errorDescription = customerForm.querySelector('.ticketDescription-error');
                errorDescription.classList.remove('d-none');
                errorDescription.innerHTML = "پر کردن این فیلد الزامی است";
            }
        }, false);
    }
    //reset errors
    title.addEventListener('focus', e => {
        let errorTitle = customerForm.querySelector('.ticketSubject-error');
        errorTitle.classList.add('d-none');
        errorTitle.innerHTML = "";
    }, false);

    description.addEventListener('focus', e => {
        let errorDescription = customerForm.querySelector('.ticketDescription-error');
        errorDescription.classList.add('d-none');
        errorDescription.innerHTML = "";
    }, false);
}
function errorHandling(e) {
    console.log(e);
}
//remove ticket
function removeTicket() {
    let removeLinks = document.querySelectorAll('.delete-ticket-link');
    if (!removeLinks) {
        return false;
    }
    removeLinks.forEach(el => {
        el.addEventListener('click', (e) => {
            let myId = el.getAttribute('data-id');
            let removeModal = document.querySelector('#item-remove-modal');
            if (!removeModal) {
                return false;
            }
            let inputId = removeModal.querySelector('input[name="id"]');
            inputId.value = myId;
        }, false);
    });
}
removeTicket();
function partialLinkClick(elem){
    let targetURL = elem.getAttribute('data-target-url');
    let done = (result) => {
        let mainContentContainer = document.querySelector('#main-content-container');
        mainContentContainer.innerHTML = result;
        persianNumberParse();
        validateSendNewMessage();
        paginationFire();
        let partialLinks = document.querySelectorAll('.partial-top-link');
        if (!partialLinks) {
            return false;
        }
        partialLinks.forEach((elem) => {
            elem.addEventListener('click', (e) => {
                partialLinkClick(elem);
            }, false);
        });
    };
    ajaxLoader("GET", targetURL, null, errorHandling, done, null, false);
};

async function partialLinkTrigger() {
    let partialLinks = document.querySelectorAll('.partial-top-link');
    if (!partialLinks) {
        return false;
    }
    partialLinks.forEach((elem) => {
        elem.addEventListener('click', (e) => {
            partialLinkClick(elem);
        }, false);
    });
}
partialLinkTrigger();

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
    let customerForm = document.querySelector('.customer-form');
    if (!customerForm) {
        return false;
    }
    let submitButton = customerForm.querySelector('.submit-form');
    if (!submitButton) {
        return false;
    }
    submitButton.addEventListener('click', (e) => {
        let mobileInput = customerForm.querySelector('input[name="Mobile"]');
        let mobileInputValue = mobileInput.value;
        mobileInput.value = changeNumbers(mobileInputValue);
    }, false);
})();


//pagination buttons clicked
async function paginationFire () {
    let paginationButtons = document.querySelectorAll('.pagination--button');
    if (!paginationButtons) {
        return false;
    }
    let done = (result) => {
        let ticketListContainer = document.querySelector('#ticket-list-container');
        if (ticketListContainer) {
            ticketListContainer.innerHTML = result;
            removeTicket();
            paginationFire();
            partialLinkTrigger();
        }
    };
    paginationButtons.forEach(el => {
        el.addEventListener('click', async (e) => {
            e.preventDefault();
            e.stopPropagation();
            //set page index
            let myPageIndex = el.getAttribute('data-page-index');
            let data = `pageIndex=${myPageIndex}`;
            await ajaxLoader("POST", "/Customer/TicketList", data, errorHandling, done, null, false);
        }, false);
    });
};
(async () => {
    await paginationFire();
})();