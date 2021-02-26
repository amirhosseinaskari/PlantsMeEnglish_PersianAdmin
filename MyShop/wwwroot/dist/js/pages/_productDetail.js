//load layout javascript
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.topMenuFunction();
    layout.mobileMenuCategoryNavigation();
    layout.menuCategoryNavigation();
    layout.passwordVisible();
    layout.productSaving();
    layout.convertPersianNumbersToEnglish();
})();
//commafy / ajaxLoader / persian numbers parse
import { setComma, ajaxLoader, persianNumberParse, lazyLoading } from './_layout.js';

lazyLoading();
//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();

//load product card component
(async function loadProductCardComponent() {
    let pComponent = await import('../components/_productcard.js');
    pComponent.default.return;

})();

//load fade component
import { fadeIn, fadeOut } from '../components/_fade.js';
//Magnifier: 1- when mouse hover on the product image, magnifying glass box will be visible and moving
let productImageContainer = document.querySelector('.product-image__image-container');
productImageContainer.addEventListener('mousemove', e => {
    let magnifierGlassBox = productImageContainer.querySelector('.magnifier-glass-box');
    let rect = productImageContainer.getBoundingClientRect();
    let windowWidth = window.innerWidth * 0.03;
    let relX = e.clientX - rect.left - windowWidth / 2;
    let relY = e.clientY - rect.top - windowWidth / 2;
    magnifierGlassBox.style.cssText = `transform:translate(${relX}px,${relY}px);display:unset`;
    let bigImageContainer = document.querySelector('.product-image__big-image-container');
    let myurl = productImageContainer.querySelector('img').src;
    bigImageContainer.style.cssText = `display:block;background-image:url('${myurl}');background-position:${((-1) * relX * 2.70)}px ${((-1) * relY * 2.70)}px`;
    console.log(myurl);

}, false);
//visible big image box when mouse enter
productImageContainer.addEventListener('mouseenter', e => {
    let myurl = productImageContainer.querySelector('img').getAttribute('data-url');
    let bigImageContainer = document.querySelector('.product-image__big-image-container');
    bigImageContainer.classList.remove('fadeout');
    bigImageContainer.style.cssText = `display:block;background-image:url('${myurl}')`;

}, false);
//hide big image box when mouse leave
productImageContainer.addEventListener('mouseleave', e => {
    let magnifierGlassBox = productImageContainer.querySelector('.magnifier-glass-box');
    magnifierGlassBox.style.cssText = 'display:none';
    let bigImageContainer = document.querySelector('.product-image__big-image-container');
    bigImageContainer.classList.add('fadeout');
    fadeOut(bigImageContainer);

}, false);

//Change product picture when click on another pictures of product
let otherProductImages = document.querySelector('.product-image__other-images').querySelectorAll('a:not(.uk-slidenav)');

otherProductImages.forEach(item => {
    item.addEventListener('click', e => {
        e.stopPropagation();
        e.preventDefault();
        let myurl = item.getAttribute('data-url');
        let mysrc = item.getAttribute('data-src');
        let mysrcset = item.getAttribute('data-srcset');
        let image = productImageContainer.querySelector('img');
        image.src = mysrc;
        image.setAttribute('src', mysrc);
        image.setAttribute('srcset', mysrcset);
        image.setAttribute('data-url', myurl);
       
        let otherImages = document.querySelector('.product-image__other-images').querySelectorAll('a');
        otherImages.forEach(el => {
            el.classList.remove('active');
            el.classList.remove('uk-disabled');
        });
        item.classList.add('active');
        item.classList.add('uk-disabled');
        

    }, false);
});

//product image lightbox trigger when click on big image on mobile
let mobileProductImageContainer = document.querySelector('.product-image__image-container--mobile');
let mobileImage = mobileProductImageContainer.querySelector('a');
mobileImage.addEventListener('click', e => {
    e.stopPropagation();
    e.preventDefault();
    let lightBoxTrigger = document.querySelector('.product-image__other-images--mobile')
        .querySelector('a:not(.active)');
    lightBoxTrigger.click();
}, false);
//change product variation value and change product price after new variation selected
function updateProductPrice() {
    let productVariationList = document.querySelector('.product-variation-list');
    if (productVariationList === null) {
        return false;
    }
    let subProductVariationInputs = productVariationList.querySelectorAll('input[type="radio"]:checked');
    let idsContainer = document.querySelector('.addneworder-form__ids-container');
    idsContainer.innerHTML = "";
    let productPrice = document.querySelector('.product-price');
    let discountPrice = Number(productPrice.getAttribute('data-discount-price'));
    let prices = [];
    subProductVariationInputs.forEach(el => {
        let myId = el.value;
        let newInput = document.createElement('INPUT');
        newInput.classList.add('d-none');
        newInput.setAttribute('name', 'ids');
        newInput.value = myId;
        newInput.setAttribute('type', 'hidden');
        idsContainer.appendChild(newInput);
        let myPrice = el.getAttribute('data-price');
        let hasDifPrice = el.getAttribute('data-has-difprice');
        if (hasDifPrice.toLowerCase() === "true") {
            prices.push(Number(myPrice) - discountPrice);
        }
    });
    console.log(prices);
    if (prices.length > 0) {
        productPrice.innerHTML = setComma(Math.max(...prices));
        
    }
}
updateProductPrice();
(async function productVariationChanged() {
    let productVariationList = document.querySelector('.product-variation-list');
    if (productVariationList === null) {
        return false;
    }
    let subProductVariationInputs = productVariationList.querySelectorAll('input[type="radio"]');
    subProductVariationInputs.forEach(item => {
        item.addEventListener('change', e => {

            subProductVariationInputs.forEach(el => {
                let myparent = el.parentElement;

                if (el.checked) {
                    myparent.classList.add('sub-product-variation--selected');
                } else {
                    myparent.classList.remove('sub-product-variation--selected');
                }
            });

            updateProductPrice();

        }, false);
    });
})();


//product comments (pagination buttons)
function productCommentsPagination() {
    let commentPaginationContainer = document.querySelector('#product-comment-list__pageination-buttons-container');
    if (commentPaginationContainer) {
        let commentPaginationButtons = commentPaginationContainer.querySelectorAll('.pagination--button');
        let doneAjax = function () {
            window.scrollTo({
                top: document.querySelector('#comment-and-details').offsetTop - 120,
                behavior: 'smooth'
            });
            productCommentsPagination();
       
            addNewProductComment();
            addNewReplyComment();
            return true;
        };
        commentPaginationButtons.forEach(elem => {
            elem.addEventListener('click', async (e) => {
                e.preventDefault();
                e.stopPropagation();
                let productId = commentPaginationContainer.getAttribute('data-product-id');
                let pageIndex = elem.getAttribute('data-page-index');
                let data = `productId=${productId}&pageIndex=${pageIndex}`;
                await ajaxLoader('POST', '/Products/ProductCommentList', data, errorHandling, doneAjax, '#product-main-content', true);
            }, false);
        });
    }


}
//add new product comment
async function addNewProductComment() {
    let productStarsContainer = document.querySelector('.product-stars-container');
    if (!productStarsContainer) {
        return false;
    }
    let starsInput = productStarsContainer.querySelectorAll('input[name="star"]');
    starsInput.forEach(elem => {
        elem.addEventListener('change', (e) => {
            document.querySelector('.rate-result').innerHTML = elem.value;
       
            productStarsContainer.setAttribute('data-rate-number', elem.value);
        }, false);
    });
    //when product submit comment clicked
    let productSubmitButton = document.querySelector('.product-submit-comment--button');
    productSubmitButton.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        //disable product submit button after click until ajax response
        productSubmitButton.setAttribute('disabled', 'disabled');
        //get product id
        let productId = productSubmitButton.getAttribute('data-id');
        //get comment description
        let description = document.querySelector('textarea[name="description"]').value;
        //get rate number
        let rate = productStarsContainer.getAttribute('data-rate-number');
        //show error message if comment description is null
        if (description === null || description.trim() === "") {
            let errorCommentDescription = document.querySelector('.error-comment-description');
            errorCommentDescription.classList.remove('d-none');
            errorCommentDescription.innerHTML = 'Fill out this field';
            return false;
        }
        //after success ajax response
        let doneAjax = function () {
            //scroll to comment section after ajax done
            window.scrollTo({
                top: document.querySelector('#comment-and-details').offsetTop - 120,
                behavior: 'smooth'
            });
            //hide comment modal
            UIkit.modal('#comment-add-modal').hide();
            //remove product submit button disabled attribute
            productSubmitButton.removeAttribute('disabled');
           
            return true;
        };
        //data for sending by ajax
        let mydata = `productId=${productId}&description=${description}&rate=${rate}`;
        //send data by ajax to /Comment/AddNewComment
        let result = await ajaxLoader('POST', '/Comment/AddNewComment', mydata, errorHandling, doneAjax, null, false);
        if (result === '101') {
            //show notification for success response after ajax done
            UIkit.notification({
                message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Your comment submited</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 5000
            });
        } else if(result==='102') {
          
            UIkit.notification({
                message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Your review sumbited before</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 5000
            });
        }
    }, false);
    //remove disabled when click on add new comment
    document.querySelector('.add-new-comment').addEventListener('click', (e) => {
        productSubmitButton.removeAttribute('disabled');
        document.querySelector('textarea[name="description"]').value = null;

    }, false);
    //reset error for comment description when focused
    document.querySelector('textarea[name="description"]').addEventListener('focus', (e) => {
        let errorCommentDescription = document.querySelector('.error-comment-description');
        errorCommentDescription.classList.add('d-none');
        errorCommentDescription.innerHTML = '';
        productSubmitButton.removeAttribute('disabled');
    }, false);
}

//add new reply product comment
async function addNewReplyComment() {
    //Reply to comment
    let productReplyCommentTrigger = document.querySelector('.product-reply-comment-trigger');
    let productReplyCommentSubmitButton = document.querySelector('.product-submit-reply-comment--button');
    if (!productReplyCommentTrigger || !productReplyCommentSubmitButton) {
        return false;
    }
    //when comment reply button clicked
    productReplyCommentTrigger.addEventListener('click', (e) => {
        let commentId = productReplyCommentTrigger.getAttribute('data-commentId');
        productReplyCommentSubmitButton.setAttribute('data-commentId', commentId);
    }, false);
    //when reply comment submited
    productReplyCommentSubmitButton.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        productReplyCommentSubmitButton.setAttribute('disabled','disabled');
      //get reply comment description
        let description = document.querySelector('textarea[name="replyDescription"]').value;
          //show error message if description is null
        if (description === null || description.trim() === "") {
            let errorReplyDescription = document.querySelector('.error-reply-description');
            errorReplyDescription.classList.remove('d-none');
            errorReplyDescription.innerHTML = "Fill out this field";
            return false;
        }
        //get comment id
        let commentId = productReplyCommentSubmitButton.getAttribute('data-commentId');
        //data for sending by ajax
        let mydata = `description=${description}&commentId=${commentId}`;
        //after success ajax response
        let doneAjax = function () {
            //scroll to comment section after ajax done
            window.scrollTo({
                top: document.querySelector('#comment-and-details').offsetTop - 120,
                behavior: 'smooth'
            });
            //hide comment modal
            UIkit.modal('#reply-comment-add-modal').hide();
            //remove product submit button disabled attribute
            productReplyCommentSubmitButton.removeAttribute('disabled');

            return true;
        };
        //send data by ajax to /Comment/AddReplyComment
        let result = await ajaxLoader('POST', '/Comment/AddReplyComment', mydata, errorHandling, doneAjax, null, false);
        if (result === '101') {
            //show notification for success response after ajax done
            UIkit.notification({
                message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Your comment submited</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 5000
            });
        } 
    }, false);
    //remove disabled attribute when click on add new reply comment
    productReplyCommentTrigger.addEventListener('click', (e) => {
        productReplyCommentSubmitButton.removeAttribute('disabled');
        document.querySelector('textarea[name="replyDescription"]').value = null;
    }, false);
    //reset error for comment description when focused
    document.querySelector('textarea[name="replyDescription"]').addEventListener('focus', (e) => {
        let errorCommentDescription = document.querySelector('.error-reply-description');
        errorCommentDescription.classList.add('d-none');
        errorCommentDescription.innerHTML = '';
        productReplyCommentSubmitButton.removeAttribute('disabled');
    }, false);
}
//Switch between tabs (comments and product-information and product details) in product page
//product-information
let productDescriptionTrigger = document.querySelector('#product-description-trigger');
productDescriptionTrigger.addEventListener('click', async (e) => {
    e.preventDefault();
    let productId = productDescriptionTrigger.getAttribute('data-id');
    let data = `productId=${productId}`;
    //ajax error handling function
    let errorHandling = (e) => {
        console.log(e);
    };
    //ajax done handling function
    let done = () => {
     
        lazyLoading();
        return true;
    };
    await ajaxLoader('POST', '/Products/ProductDescription', data, errorHandling, done, '#product-main-content', false);


}, false);
//product-options
let productDetailsTrigger = document.querySelector('#product-options-trigger');
productDetailsTrigger.addEventListener('click', async (e) => {
    e.preventDefault();
    let productId = productDescriptionTrigger.getAttribute('data-id');
    //data for send by ajax
    let data = `productId=${productId}`;
    //ajax error handling function
    let errorHandling = (e) => {
        console.log(e);
    };
    //ajax done handling function
    let done = () => {
      
        return true;
    };
    await ajaxLoader('POST', '/Products/ProductOptions', data, errorHandling, done, '#product-main-content', false);


}, false);
//product comments 
let productCommentsTrigger = document.querySelector('#product-comment-trigger');
productCommentsTrigger.addEventListener('click', async (e) => {
    e.preventDefault();

    let productId = productDescriptionTrigger.getAttribute('data-id');
    let data = `productId=${productId}`;
    //ajax error handling function
    let errorHandling = (e) => {
        console.log(e);
    };
    //ajax done handling function
    let done = () => {
    
        productCommentsPagination();
        addNewProductComment();
        addNewReplyComment();
        return true;
    };
    await ajaxLoader('POST', '/Products/ProductCommentList', data, errorHandling, done, '#product-main-content', false);

}, false);

//scroll to submit comment
(function scrollToComment() {
    let goToComment = document.querySelector('.go-to-comment');
    let commentTab = document.querySelector('#comment-and-details');
    goToComment.addEventListener('click', (e) => {
        e.stopPropagation();
        e.preventDefault();
        window.scrollTo({
            top: document.querySelector('#comment-and-details').offsetTop - 160,
            behavior: 'smooth'
        });
    }, false);
})();

//share button
(async function shareButon() {
    let shareButtonTrigger = document.querySelector('.product-share-button-trigger');

    shareButtonTrigger.addEventListener('click', (e) => {
        e.preventDefault();
        e.stopPropagation();
        let shareButtons = document.querySelectorAll('.product-share-button');
        shareButtons.forEach(elem => {
            if (elem.classList.contains('product-share-button-active')) {
                elem.classList.add('product-share-button-disactive');
                elem.classList.remove('product-share-button-active');
                setTimeout(() => {
                    elem.classList.remove('product-share-button-active');
                    elem.classList.remove('product-share-button-disactive');
                }, 500);
            } else {
                elem.classList.add('product-share-button-active');
            }
        });
    }, false);

    //copy url link
    let copyURLButton = document.querySelector('.copy-url-link');
    copyURLButton.addEventListener('click', (e) => {
        e.preventDefault();
        e.stopPropagation();
        /* Get the text field */
        let urlText = copyURLButton.getAttribute('data-url');
        let tempInputText = document.createElement('INPUT');
        tempInputText.value = urlText;
        tempInputText.style.opacity = 0;
        document.body.appendChild(tempInputText);
        /* Select the text field */
        tempInputText.select();
        tempInputText.setSelectionRange(0, 99999); /*For mobile devices*/

        /* Copy the text inside the text field */
        document.execCommand("copy");
        tempInputText.remove();


    }, false);
    copyURLButton.addEventListener('mouseup', (e) => {
        copyURLButton.setAttribute('uk-tooltip', 'title:Link Copied;pos:right');
        setTimeout(() => {
            UIkit.tooltip(copyURLButton).show();

        }, 200);

    }, false);
    copyURLButton.addEventListener('mouseenter', (e) => {
        copyURLButton.setAttribute('uk-tooltip', 'title:Copy Link;pos:right');
        UIkit.tooltip(copyURLButton).show();
    }, false);


})();
function errorHandling(e) {
    console.log(e);
}

