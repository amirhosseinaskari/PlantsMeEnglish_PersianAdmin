//load layout javascript
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.topMenuFunction();
    layout.mobileMenuCategoryNavigation();
    layout.menuCategoryNavigation();
    layout.passwordVisible();
    layout.convertPersianNumbersToEnglish();
})();
//commafy / ajaxLoader / persian numbers parse
import { setComma, ajaxLoader, persianNumberParse, lazyLoading, clickButonEffect, preventWriteString } from './_layout.js';
persianNumberParse();
lazyLoading();
clickButonEffect();
preventWriteString();
//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();


//remove an order
(async function deleteOrder() {
    let deleteOrder = document.querySelectorAll('.delete-order');
    if (!deleteOrder) {
        return false;
    }
    deleteOrder.forEach(elem => {
        elem.addEventListener('click', e => {
            let id = elem.getAttribute('data-id');
            document.querySelector('#delete--modal').querySelector('input[name="orderId"]').value = id;
        }, false);
    });
})();


//==============Shopping basket==============
//increase and decrease an order
let isDoneCounterChanged = true;
function errorHandling(e) {
    console.log(e);
    isDoneCounterChanged = true;
}
//increase an order
(function increaseOrder() {
    //increase order buttons
    let increaseOrderBtns = document.querySelectorAll('.increase-order');
    if (!increaseOrderBtns) {
        return false;
    }
    
    //change order number when an increase button clicked
    increaseOrderBtns.forEach(elem => {
        elem.addEventListener('click', (e) => {
            console.log(isDoneCounterChanged);
            e.stopPropagation();
            e.preventDefault();
            if (!isDoneCounterChanged) {
                return false;
            }
            isDoneCounterChanged = false;
            //get id of product
            let id = elem.getAttribute('data-id');
            //find parent of this button
            let parent = elem.parentElement;
            //get current order count
            let orderCount = parent.querySelector('.order-count');
            let numberOfOrderCount = Number(orderCount.value);
            //order prices 
            let orderPrices = document.querySelectorAll('.order-price');
            let totalPrice = 0;
            //done ajax function
            let doneAjax = function (result) {
                if (result === -2) {
                    UIkit.notification({
                        message: '<span class= "margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"><span class="dana-font fontsize-14" dir="rtl">Sorry, there is not any more in stock</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 3000
                    });
                    return;
                }
                persianNumberParse();
                //increase number of order (change value of input)
                orderCount.value = numberOfOrderCount + 1;
                orderCount.setAttribute('data-count', (numberOfOrderCount + 1));
                //find order price text element
                let orderPrice = document.querySelector(`.order-price[data-id="${id}"]`);
                //change old price text to new price
                orderPrice.innerHTML = setComma(result);
                orderPrice.setAttribute('data-price', result);

                //change all order count elemnts
                let orderCountElements = document.querySelectorAll('.order-count');
                let defaultCount = 0;
                orderCountElements.forEach(elem => {
                    defaultCount += Number(elem.value);
                });
                //change text of orders count in summary price box (first row)
                document.querySelector('.orders-count').innerHTML = `(${defaultCount})`;
                //change price text of order-prices elements
                orderPrices.forEach(elem => {
                    totalPrice += Number(elem.getAttribute('data-price'));
                });
                //change html of total price
                let totalPriceElements = document.querySelectorAll('.total-price');
                totalPriceElements.forEach(elem => {
                    elem.innerHTML = setComma(totalPrice);
                });
                persianNumberParse();
                isDoneCounterChanged = true;
                return true;
            };
            //preparing data for ajax request
            let data = `orderId=${id}`;
            //send request by ajax and get result (new price of this order)
            ajaxLoader('POST', '/Shopping/IncreaseOrder', data, errorHandling, doneAjax, null, true);
            
        }, false);
    });
})();

//decrease an order
(function decreaseOrder() {
    //decrease order buttons
    let decreaseOrderBtns = document.querySelectorAll('.decrease-order');
    if (!decreaseOrderBtns) {
        return false;
    }
    
    //change order number when an increase button clicked
    decreaseOrderBtns.forEach(elem => {
        elem.addEventListener('click', (e) => {
            console.log(isDoneCounterChanged);
            e.stopPropagation();
            e.preventDefault();
            if (!isDoneCounterChanged) {
                return false;
            }
            isDoneCounterChanged = false;
            //get id of product
            let id = elem.getAttribute('data-id');
            //find parent of this button
            let parent = elem.parentElement;
            //get current order count
            let orderCount = parent.querySelector('.order-count');
            let numberOfOrderCount = Number(orderCount.value);
            if (numberOfOrderCount < 2) {
                isDoneCounterChanged = true;
                return false;
            }
            //order prices 
            let orderPrices = document.querySelectorAll('.order-price');
            let totalPrice = 0;
            //done ajax function
            let doneAjax = function (result) {

                //decrease number of order (change value of input)
                orderCount.value = numberOfOrderCount - 1;
                orderCount.setAttribute('data-count', (numberOfOrderCount - 1));
                //find order price text element
                let orderPrice = document.querySelector(`.order-price[data-id="${id}"]`);
                //change old price text to new price
                orderPrice.innerHTML = setComma(result);
                orderPrice.setAttribute('data-price', result);

                //change all order count elemnts
                let orderCountElements = document.querySelectorAll('.order-count');
                let defaultCount = 0;
                orderCountElements.forEach(elem => {
                    defaultCount += Number(elem.value);
                });
                //change text of orders count in summary price box (first row)
                document.querySelector('.orders-count').innerHTML = `(${Math.abs(defaultCount)})`;
                //change price text of order-prices elements
                orderPrices.forEach(elem => {
                    totalPrice += Number(elem.getAttribute('data-price'));
                });
                //change html of total price
                let totalPriceElements = document.querySelectorAll('.total-price');
                totalPriceElements.forEach(elem => {
                    elem.innerHTML = setComma(Math.abs(totalPrice));
                });
                persianNumberParse();
                isDoneCounterChanged = true;
                return true;
            };
            //preparing data for ajax request
            let data = `orderId=${id}`;

            //send request by ajax and get result (new price of this order)
            ajaxLoader('POST', '/Shopping/DecreaseOrder', data, errorHandling, doneAjax, null, true);



        }, false);
    });
})();

(function manualChangeOrderNumber() {
    let orderCountInputs = document.querySelectorAll('input.order-count');
    if (!orderCountInputs) {
        return false;
    }
    orderCountInputs.forEach(elem => {
        elem.addEventListener('keydown', (e) => {
            console.log(isDoneCounterChanged);
            if (!isDoneCounterChanged) {
                e.stopPropagation().
                    e.preventDefault();
                return false;
            }
           
        }, false);

        elem.addEventListener('keyup', (e) => {
            console.log(isDoneCounterChanged);
            if (!isDoneCounterChanged) {
                e.stopPropagation().
                    e.preventDefault();
                return false;
            }
            isDoneCounterChanged = false;
            //get id of product
            let id = elem.getAttribute('data-id'); 
           
            let numberOfOrderCount = Number(elem.value);
            if (numberOfOrderCount < 2) {
                isDoneCounterChanged = true;
                elem.value = elem.getAttribute('data-count');
                return false;
               
            }
            //order prices 
            let orderPrices = document.querySelectorAll('.order-price');
            let totalPrice = 0;
            //done ajax function
            let doneAjax = function (result) {
                if (result === -2) {
                    UIkit.notification({
                        message: '<span class= "margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"><span class="dana-font fontsize-14" dir="rtl">Sorry, there is not any more in stock</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 3000
                    });
                    return;
                }
                elem.setAttribute('data-count', (numberOfOrderCount));
                //find order price text element
                let orderPrice = document.querySelector(`.order-price[data-id="${id}"]`);
                //change old price text to new price
                orderPrice.innerHTML = setComma(result);
                orderPrice.setAttribute('data-price', result);

                //change all order count elemnts
                let orderCountElements = document.querySelectorAll('.order-count');
                let defaultCount = 0;
                orderCountElements.forEach(elem => {
                    defaultCount += Number(elem.value);
                });
                //change text of orders count in summary price box (first row)
                document.querySelector('.orders-count').innerHTML = `(${Math.abs(defaultCount)})`;
                //change price text of order-prices elements
                orderPrices.forEach(elem => {
                    totalPrice += Number(elem.getAttribute('data-price'));
                });
                //change html of total price
                let totalPriceElements = document.querySelectorAll('.total-price');
                totalPriceElements.forEach(elem => {
                    elem.innerHTML = setComma(Math.abs(totalPrice));
                });
                persianNumberParse();
                isDoneCounterChanged = true;
                return true;
            };
            //preparing data for ajax request
            let data = `orderId=${id}&number=${numberOfOrderCount}`;

            //send request by ajax and get result (new price of this order)
            ajaxLoader('POST', '/Shopping/ChangeOrderNumber', data, errorHandling, doneAjax, null, true);

        }, false);
    });

     
})();
//=========Address information==========
(async () => {
    //add modal address
    let addressFormModals = document.querySelectorAll('.address-form-modal');
    if (!addressFormModals) {
        return false;
    }
    addressFormModals.forEach(elem => {
        //state select box
        let stateSelectBox = elem.querySelector('select[name="StateId"]');
        //city select box
        let citySelectBox = elem.querySelector('select[name="CityId"]');
        //city select box button
        let citySelectBoxBtn = elem.querySelector('.city-selectbox-button');
        //change city value when state changed
        stateSelectBox.addEventListener('change', e => {
            //disable state select box until ajax be done
            stateSelectBox.setAttribute('disabled', 'disabled');
            citySelectBox.setAttribute('disabled', 'disabled');
            citySelectBoxBtn.innerHTML = 'Loading ...';
            //data for send by ajax
            let data = `stateId=${stateSelectBox.value}`;
            let done = (result) => {
                citySelectBox.innerHTML = result;
                stateSelectBox.removeAttribute('disabled');
                citySelectBox.removeAttribute('disabled');
                citySelectBoxBtn.innerHTML = `<span>${citySelectBox.querySelector('option:checked').innerHTML}</span><span uk-icon="icon: chevron-down"></span>`;
                return true;
            };
            //send request by ajax and get result (get city list of slected state)
            ajaxLoader('POST', '/Address/CityList', data, errorHandling, done, null, true);
        }, false);
        //when modal address form submit button clicked
        let eventFunction = (e) => {
            //get receiver name
            let receiverName = elem.querySelector('input[name="ReceiverName"]').value;
            if (receiverName !== null) {
                receiverName = receiverName.trim();

            }
           
            //show error message if receiver name is empty
            if (receiverName === null || receiverName === "") {
                let receiverNameError = elem.querySelector('.receivername-error');
                receiverNameError.classList.remove('d-none');
                receiverNameError.innerHTML = "Please enter your full name";
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
            console.log(receiverName);
            //get receiver mobile number
            let receiverMobileNumber = elem.querySelector('input[name="ReceiverMobileNumber"]').value;
            if (receiverMobileNumber !== null) {
                receiverMobileNumber = receiverMobileNumber.trim();
            }
            //show error message if receiver mobile number is empty
            if (receiverMobileNumber === null || receiverMobileNumber === "") {
                let receiverMobileNumber = elem.querySelector('.receivermobilenumber-error');
                receiverMobileNumber.innerHTML = "Please enter your mobile number";
                receiverMobileNumber.classList.remove('d-none');
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
            console.log(receiverMobileNumber);
            //get postal code number
            let postalCodeNumber = elem.querySelector('input[name="PostalCode"]').value;
            if (postalCodeNumber !== null) {
                postalCodeNumber = postalCodeNumber.trim();
            }

            //show error message if postal code number is empty
            if (postalCodeNumber === null || postalCodeNumber === "") {
               
                let errorPostalCodeNumber = elem.querySelector('.postalcode-error');
                errorPostalCodeNumber.innerHTML = "Please enter your postal code";
                errorPostalCodeNumber.classList.remove('d-none');
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
            console.log(postalCodeNumber);
            //get detail of address
            let detailOfAddress = elem.querySelector('textarea[name="Details"]').value;
            if (detailOfAddress !== null) {
                detailOfAddress = detailOfAddress.trim();
            }
            //show error message if details is empty
            if (detailOfAddress === null || detailOfAddress === "") {
                let errorDetails = elem.querySelector('.details-error');
                errorDetails.innerHTML = "Please enter your address's details";
                errorDetails.classList.remove('d-none');
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
            console.log(detailOfAddress);
            //get city name and state name
            let stateName = stateSelectBox.options[stateSelectBox.selectedIndex].text;
            let cityName = citySelectBox.options[citySelectBox.selectedIndex].text;
            elem.querySelector('input[name="StateName"]').value = stateName;
            elem.querySelector('input[name="CityName"]').value = cityName;
        };
        //when add-address submit button clicked
        let addAddressSbmitButton = document.querySelector('.add-address-form__submit-button');
        if (!addAddressSbmitButton) {
            return false;
        }
        addAddressSbmitButton.addEventListener('click', eventFunction, false);

        //when edit-address submit button clicked
        let editAddressSubmitButton = elem.querySelector('.edit-address-form__submit-button');
        if (!editAddressSubmitButton) {
            return false;
        }
        editAddressSubmitButton.addEventListener('click', eventFunction, false);

        //reset inputs errors
        let resetInputsError = (errorElem) => {
            errorElem.classList.add('d-none');
            errorElem.innerHTML = "";
        };
        let myReceiverNameInput = elem.querySelector('input[name="ReceiverName"]');
        let myReceiverNameError = elem.querySelector('.receivername-error');
        myReceiverNameInput.addEventListener('focus', e => {
            resetInputsError(myReceiverNameError);
        }, false);
        let myReceiverMobileNumber = elem.querySelector('input[name="ReceiverMobileNumber"]');
        let myReceiverMobileNumberError = elem.querySelector('.receivermobilenumber-error');
        myReceiverMobileNumber.addEventListener('focus', e => {
            resetInputsError(myReceiverMobileNumberError);
        }, false);
        let myPostalCode = elem.querySelector('input[name="PostalCode"]');
        let myPostalCodeError = elem.querySelector('.postalcode-error');
        myPostalCode.addEventListener('focus', e => {
            resetInputsError(myPostalCodeError);
        }, false);
        let myDetails = elem.querySelector('textarea[name="Details"]');
        let myDetailsError = elem.querySelector('.details-error');
        myDetails.addEventListener('focus', e => {
            resetInputsError(myDetailsError);
        }, false);

    });

    //delete an address
    let deleteAddressLinks = document.querySelectorAll('.delete-address-link');
    let deleteAddressModal = document.querySelector('#delete-address--modal');
    deleteAddressLinks.forEach(d => {
        d.addEventListener('click', (e) => {
            let addressId = d.getAttribute('data-id');
            deleteAddressModal.querySelector('input[name="addressId"]').value = addressId;
        }, false);
    });

    //change price when selected address changed
    let onSelectedAddressChange = async (i) => {
        
        let selectedAddressId = i.getAttribute('data-id');
        let done = (result) => {
            let billWithDelivery = document.querySelector('.bill-with-delivery-price-container');
            billWithDelivery.innerHTML = result;
            persianNumberParse();
            return true;
        };
        //set data for send by ajax
        let data = `selectedAddressId=${selectedAddressId}`;
        await ajaxLoader("POST", "/Address/BillWithDeliveryPrice", data, errorHandling, done, null, false);

        let done02 = (result) => {
            let totalPriceContainer = document.querySelectorAll('.total-price-container');
            totalPriceContainer.forEach(el => {
                el.innerHTML = result;
                persianNumberParse();
                return true;
            });
        };
        await ajaxLoader("POST", "/Address/TotalPrice", data, errorHandling, done02, null, false);
        
    };
   //when IsSelected address changed
    let isSelectedAddress = document.querySelectorAll('input[name="IsSelectedAddress"]');
    if (!isSelectedAddress) {
        return false;
    }
    isSelectedAddress.forEach(c => {
        c.addEventListener('change', (e) => {
            let selectedAddressId = c.getAttribute('data-id');
            let mydata = `selectedAddressId=${selectedAddressId}`;
            let done03 = (result) => {
                onSelectedAddressChange(c);
                persianNumberParse();
            };
            ajaxLoader("POST", "/Address/ChangeSelectedAddress", mydata, errorHandling, done03, null, false);
        }, false);
    });
   
   
})();

//===============Confirm and payment=========
(async () => {
    //local payment radio button inputs
    let isLocalPaymentInput = document.querySelectorAll('input[name="IsLocalPayment"]');
    if (!isLocalPaymentInput) {
        return false;
    }
    //when IsLocalPayment input changed
    isLocalPaymentInput.forEach(c => {
        c.addEventListener('change', e => {
            let shoppingId = c.getAttribute('data-id');
            let isLocalPayment = c.value;
            let done = (result) => {
                if (result) {
                    UIkit.notification({
                        message: '<span class= "margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span ><span class="dana-font fontsize-14" dir="ltr">Payment Method Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 3000
                    });
                }
            };
            //set data for send by ajax
            let data = `shoppingId=${shoppingId}&isLocalPayment=${isLocalPayment}`;
            ajaxLoader("POST", "/Shopping/ChangeIsLocalPayment", data, errorHandling, done, null, false);
        }, false);
      

    });

    //discount code
    let discountSubmitButton = document.querySelector('.discount-submit');
    if (!discountSubmitButton) {
        return false;
    }
    let onSetDiscountCode = (el) => {
        let shoppingId = el.getAttribute('data-id');
        let done01 = (result) => {
            let priceSummary = document.querySelector('#price-summary-container');
            priceSummary.innerHTML = result;
            persianNumberParse();
        };
        //set data for send by ajax
        let data01 = `shoppingId=${shoppingId}`;
        ajaxLoader("POST", "/Shopping/FinalBill", data01, errorHandling, done01, null, false);
        let done02 = (result) => {
            let totalPrices = document.querySelectorAll('.total-price');
            totalPrices.forEach(t => {
                t.innerHTML = setComma(result);
                persianNumberParse();
            });
        };
        ajaxLoader("POST", "/Shopping/TotalPrice", data01, errorHandling, done02, null, false);
    };
    //when discount submit clicked
    discountSubmitButton.addEventListener('click', (e) => {
        e.stopPropagation();
        e.preventDefault();
        let discountCode = document.querySelector('input[name="discountCode"]').value;
        let shoppingId = discountSubmitButton.getAttribute('data-id');
        let done = (result) => {
            console.log(result);
            if (result==='true') {
                onSetDiscountCode(discountSubmitButton);
                let errorDiscountCode = document.querySelector('.error-discount-code');
                errorDiscountCode.classList.add('d-none');
                errorDiscountCode.innerHTML = "";
                let successDiscountCode = document.querySelector('.success-discount-code');
                successDiscountCode.classList.remove('d-none');
                successDiscountCode.innerHTML = "Discount Code Applied";
                UIkit.notification({
                    message: '<span class= "margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"><span class="dana-font fontsize-14" dir="rtl">Discount Code Applied</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 3000
                });
            } else {
                onSetDiscountCode(discountSubmitButton);
                let errorDiscountCode = document.querySelector('.error-discount-code');
                errorDiscountCode.classList.remove('d-none');
                errorDiscountCode.innerHTML = "Discount code is not valid";
                let successDiscountCode = document.querySelector('.success-discount-code');
                successDiscountCode.classList.add('d-none');
                successDiscountCode.innerHTML = "";
            }
        };
        let data = `discountCode=${discountCode}&shoppingId=${shoppingId}`;
        ajaxLoader("POST", "/Shopping/UsingDiscountCode", data, errorHandling, done, null, false);
        
    }, false);
})();



