
/*---Convert english numbers to persian numbers---*/
export function persianNumberParse() {
    function traverse(el) {
        let persian = { 0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹' };
        if (el.nodeType === 3) {
            var list = el.data.match(/[0-9]/g);
            if (list !== null && list.length !== 0) {
                for (let i = 0; i < list.length; i++) {
                    el.data = el.data.replace(list[i], persian[list[i]]);
                }
            }
        }
        for (let i = 0; i < el.childNodes.length; i++) {
            traverse(el.childNodes[i]);
        }
    }
    //let digitFormat = document.querySelectorAll('.digit-format');
    //digitFormat.forEach(function (item) {
      
    //    traverse(item);
    //});

    window.addEventListener('resize', (e) => {
            let myUkSticky = document.querySelectorAll('.my-uk-sticky');
            myUkSticky.forEach(el => {
                if (window.innerWidth < 600) {
                    el.removeAttribute('uk-sticky');
                }
                else {
                    el.setAttribute('uk-sticky', 'bottom:true;offset:100');
                }
            });
        
    }, false);

    let myAllUkSticky = document.querySelectorAll('.my-uk-sticky');
    myAllUkSticky.forEach(el => {
        if (window.innerWidth < 600) {
            el.removeAttribute('uk-sticky');
        }
        else {
            el.setAttribute('uk-sticky', 'bottom:true;offset:100');
        }
    });
}

export function convertPersianNumbersToEnglish() {
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

    //change persian numbers to english numbers in login modal
    let loginModal = document.querySelector('#login--modal');
    if (loginModal) {
        let loginModalSubmitButton = loginModal.querySelector('.submit-button');
        if (loginModalSubmitButton) {
            loginModalSubmitButton.addEventListener('click', (e) => {
                let mobileNumberInput = loginModal.querySelector('input[name="Input.Mobile"]');
                let mobileNumberValue = mobileNumberInput.value;
                mobileNumberInput.value = changeNumbers(mobileNumberValue);
            }, false);

        }
    }
   

    //change persian numbers to english numbers in register modal
    let registerModal = document.querySelector('#register--modal');
    if (registerModal) {
        let registerModalSubmitButton = registerModal.querySelector('.submit-button');
        if (registerModalSubmitButton) {
            registerModalSubmitButton.addEventListener('click', (e) => {
                let mobileNumberInput = loginModal.querySelector('input[name="Input.Mobile"]');
                let mobileNumberValue = mobileNumberInput.value;
                mobileNumberInput.value = changeNumbers(mobileNumberValue);
            }, false);
        }
    }
    

     
}
/*----Top menu----*/
export async function topMenuFunction() {

    //toggle chevron-down | chevron-up animation on toggle dropdown
    let categoryDropdownTrigger = document.querySelector('#category-dropdown-content');
    function dropdownTrigger(elem) {
        let chevronDown = document.querySelector('#category-dropdown-trigger .chevron-down-icon');
        chevronDown.classList.toggle('open');
    }
    categoryDropdownTrigger.addEventListener('beforeshow', function () {
        dropdownTrigger(this);
    }, false);
    categoryDropdownTrigger.addEventListener('beforehide', function () {
        dropdownTrigger(this);
    }, false);

    //when scroll second section of menu be sticky and get shadow
    //desktop menu sticky
    let secondSection = document.querySelector('#menu-second-section');
    secondSection.addEventListener('active', e => {
        secondSection.classList.add('uk-box-shadow-small');
    }, false);
    secondSection.addEventListener('inactive', e => {
        secondSection.classList.remove('uk-box-shadow-small');
    }, false);
    //mobile menu sticky 
    let mobileSecondSection = document.querySelector('#mobile-menu-second-section');
    mobileSecondSection.addEventListener('active', e => {
        mobileSecondSection.classList.add('uk-box-shadow-small');
    }, false);
    mobileSecondSection.addEventListener('inactive', e => {
        mobileSecondSection.classList.remove('uk-box-shadow-small');
    }, false);
}
/*----Mobile menu (category navigation)----*/
export async function mobileMenuCategoryNavigation() {
    let categoryNavigation = document.querySelectorAll('.category-navigation');
    categoryNavigation.forEach(elem => {
        elem.addEventListener('click', e => {
            e.stopPropagation();
            e.preventDefault();
            let targetId = elem.getAttribute('href');
            let promise = new Promise((resolve) => {
                let subCategories = document.querySelectorAll('.mobile-sub-category-list-items');
                subCategories.forEach(el => {
                    el.classList.remove('active');

                });
                resolve();
            });
            promise.then(() => {
                let targetElem = document.querySelector(targetId);
                if (targetElem !== null) {
                    targetElem.classList.add('active');
                    document.querySelector('#main-mobile-menu').classList.remove('active');
                }
               
            });

        }, false);
    });
    //when click on back arrow in sub category list
    let backCatNavigation = document.querySelectorAll('.back-category-navigation');
    backCatNavigation.forEach(elem => {
        elem.addEventListener('click', e => {
            e.stopPropagation();
            e.preventDefault();
            let targetId = elem.getAttribute('href');
            let targetElem = document.querySelector(targetId);
            if (targetElem !== null) {
                targetElem.classList.add('active');
                elem.parentElement.classList.remove('active');
            } else {
                elem.parentElement.classList.remove('active');
                document.querySelector('#main-mobile-category-list').classList.add('active');
            }
        }, false);
    });
    //back to main mobile menu when click on back arrow in main category list
    let mainBackMobileMenu = document.querySelector('#main-back-mobile-menu');
    mainBackMobileMenu.addEventListener('click', e => {
        e.stopPropagation();
        e.preventDefault();
        document.querySelector('#main-mobile-category-list').classList.remove('active');
        document.querySelector('#main-mobile-menu').classList.add('active');
    }, false);

}


/*----Desktop menu (category navigation)----*/
export async function menuCategoryNavigation() {
    let categoryNavigation = document.querySelectorAll('.desktop-category-navigation');
    categoryNavigation.forEach(elem => {
        elem.addEventListener('click', e => {
            e.stopPropagation();
            e.preventDefault();
            let targetId = elem.getAttribute('href');
            let promise = new Promise((resolve) => {
                let subCategories = document.querySelectorAll('.sub-category-list');
                subCategories.forEach(el => {
                    el.classList.remove('active');
                });
                resolve();
            });
            promise.then(() => {
                let targetElem = document.querySelector(targetId);
                if (targetElem !== null) {
                    targetElem.classList.add('active');
                    document.querySelector('#main-category-list').classList.remove('active');
                }

            });

        }, false);
    });
    //when click on back arrow in sub category list
    let backCatNavigation = document.querySelectorAll('.desktop-back-category-navigation');
    backCatNavigation.forEach(elem => {
        elem.addEventListener('click', e => {
            e.stopPropagation();
            e.preventDefault();
            let targetId = elem.getAttribute('href');
            let targetElem = document.querySelector(targetId);
          
            if (targetElem !== null) {
                console.log('true');
                targetElem.classList.add('active');
                console.log(targetElem);
                elem.parentElement.classList.remove('active');
            } else {
                console.log('false');
                elem.parentElement.classList.remove('active');
                document.querySelector('#main-category-list').classList.add('active');
            }
        }, false);
    });
}


//Login/Register Modal
//show and hide password when click on visiblity icon
export async function passwordVisible() {
    let visibleIcons = document.querySelectorAll('.visible-password-icon');
    visibleIcons.forEach(item => {
        item.addEventListener('click', e => {
            e.stopPropagation();
            e.preventDefault();
            let myImage = item.querySelector('img');
            let myInput = item.nextElementSibling;
            let inputType = myInput.type;
            if (inputType === "password") {
                myInput.type = "text";
                let prevSrc = myImage.src;
                let nextSrc = myImage.getAttribute('data-second-src');
                myImage.src = nextSrc;
                myImage.setAttribute('data-second-src', prevSrc);
            } else {
                myInput.type = "password";
                let prevSrc = myImage.src;
                let nextSrc = myImage.getAttribute('data-second-src');
                myImage.src = nextSrc;
                myImage.setAttribute('data-second-src', prevSrc);
            }
        }, false);
    });
}

//utilities
export function setComma(mynumber){
    function isNumeric(val) {
        if (typeof val === 'number' && !isNaN(val)) {
            return true;
        }

        val = (val || '').toString().trim();
        return val && !isNaN(val);
    }

    function commafy(val) {
        if (typeof val === 'undefined' || val === null) {
            val = '';
        }

        val = val.toString();

        if (!isNumeric(val)) {
            return val;
        }

        var parts = val.split('.');

        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }

    return commafy(mynumber);
};

//ajax loader
export async function ajaxLoader(method, url, data, errorHandling,doneFunction, target, hasLoader) {

    if (hasLoader) {
        let loader = document.createElement("DIV");
        loader.className += "page-loading active";
        //loader text
        let loaderText = document.createElement("SPAN");
        loaderText.innerHTML = "Loading ....";
        loaderText.className += "dana-font white-text fontsize-20 mobile-fontsize-16 font-weight-600 margin-bottom-30";
        loader.setAttribute('dir', 'ltr');
        //loader animated balls
        let loaderAnimatedBalls = document.createElement("DIV");
        loaderAnimatedBalls.className += "page-loading-animation";
        //copy of loader animation balls for having 4 balls
        let loaderAnimatedBallsCopy = loaderAnimatedBalls.cloneNode(true);
        //append loader text
        loader.appendChild(loaderText);
        //append loaderAnimatedBalls
        //create loaderAnimatedBalls wrapper (flex mode)
        let loaderAnimatedBallsWrapper = document.createElement("DIV");
        loaderAnimatedBallsWrapper.className += "d-flex justify-content-center align-items-center";
        loaderAnimatedBallsWrapper.appendChild(loaderAnimatedBalls);
        loaderAnimatedBallsWrapper.appendChild(loaderAnimatedBallsCopy);
        loader.appendChild(loaderAnimatedBallsWrapper);
       
        document.querySelector('body').appendChild(loader);
        

    } else {
        //show loader before ajax fired
        let progress = document.createElement('DIV');
        let indeterminate = document.createElement('DIV');
        progress.classList.add('progress');
        indeterminate.classList.add('indeterminate');
        progress.appendChild(indeterminate);
        document.querySelector('body').appendChild(progress);
    }
    let promise = new Promise((resolve) => {
        let xhttp = new XMLHttpRequest();
      
        xhttp.open(method, url);
        xhttp.onerror = errorHandling;
        xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
       
        //on ready state change
        xhttp.onreadystatechange = function () {

            if (this.readyState == 4 && this.status == 200) {
                let result = this.responseText;
                persianNumberParse();
                if (hasLoader) {
                    let myloader = document.querySelector('.page-loading');
                    myloader.remove();
                    
                } else {
                    document.querySelector('.progress').remove();
                    
                }
                if (target) {
                    document.querySelector(target).innerHTML = result;
                    resolve();
                    
                } else {
                    resolve(result);
                }
                
            }
        };
        xhttp.send(data);
    });
    return promise.then((value) => {
        doneFunction(value);
        return value;
    });
  
}


//scroll to top
export function scrollToTop(duration) {
    // cancel if already on top
    if (document.scrollingElement.scrollTop === 0) return;

    const cosParameter = document.scrollingElement.scrollTop / 2;
    let scrollCount = 0, oldTimestamp = null;

    function step(newTimestamp) {
        if (oldTimestamp !== null) {
            // if duration is 0 scrollCount will be Infinity
            scrollCount += Math.PI * (newTimestamp - oldTimestamp) / duration;
            if (scrollCount >= Math.PI) return document.scrollingElement.scrollTop = 0;
            document.scrollingElement.scrollTop = cosParameter + cosParameter * Math.cos(scrollCount);
        }
        oldTimestamp = newTimestamp;
        window.requestAnimationFrame(step);
    }
    window.requestAnimationFrame(step);
}

//scroll to an element
export function scrollToElement(elem,target) {
    try {
        elem.scrollIntoView({ behavior: 'smooth', block: target });
    } catch (e) {
        if (target === 'start') {
            elem.scrollIntoView(true);
        } else {
            elem.scrollIntoView(false);
        }
        
    }
    
}
//image lazy loading
export function lazyLoading() {
    (async () => {if ('loading' in HTMLImageElement.prototype) {
            const images = document.querySelectorAll("img.lazyload");
        images.forEach(img => {
                img.src = img.dataset.src;
                let dataSrcSet = img.getAttribute('data-srcset');
            if (dataSrcSet) {
                img.setAttribute('srcset', dataSrcSet);
            }
                img.classList.remove('lazyload');
                img.classList.add('lazier');
                

            });
        } else {
            // Dynamically import the LazySizes library
            const script = document.createElement('script');
            script.src =
                '/js/lazysizes.min.js';
            document.body.appendChild(script);

        }
    })();
}

//click button effect
export async function clickButonEffect() {
    let buttons = document.querySelectorAll('.click-button-effect');
    if (!buttons) {
        return false;
    }
    buttons.forEach(elem => {
        elem.addEventListener('mousedown', e => {
            elem.classList.add('active-effect');

        }, false);
        elem.addEventListener('mouseup', e => {
            elem.classList.remove('active-effect');
        }, false);
        elem.addEventListener('touchstart', e => {
            elem.classList.add('active-effect');

        }, false);
        elem.addEventListener('touchend', e => {
            elem.classList.remove('active-effect');
        }, false);
    });
}

//prevent write string in input type number
export async function preventWriteString() {
    let numbersInput = document.querySelectorAll('input.type-number');
    if (!numbersInput) {
        return false;
    }
    numbersInput.forEach(elem => {
        elem.addEventListener('keydown', e => {
            let key = e.keyCode || e.which;
            if (key !== 9 && key !== 8 && ((key < 48 || key > 57) && !(key >= 96 && key <= 105))) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        }, false);
      
    });
}

//saveing product to favorits
export async function productSaving() {
    let errorHandling = (e) => { console.log(e); };
   //save product to favorits
    let saveProduct = async (el) => {
        let done = (result) => {
            if (result) {
                UIkit.notification({
                    message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Added To Your Favorites</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 3000
                });
                el.classList.add('saved-product');
            }
            return;
        };
        let productId = el.getAttribute('data-id');
        //set data for send by ajax
        let data = `productId=${productId}`;
        let result = await ajaxLoader("POST", "/FavoritProduct/AddToMyFavorit", data, errorHandling, done, null, false);
       
    };
    //unsave product to favorits
    let unsaveProduct = async (el) => {
        let done = (result) => {
            if (result) {
                UIkit.notification({
                    message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Removed From Your Favorites</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 3000
                });
                el.classList.remove('saved-product');
            }
            return;
        };
        let productId = el.getAttribute('data-id');
        //set data for send by ajax
        let data = `productId=${productId}`;
        await ajaxLoader("POST", "/FavoritProduct/RemoveFromMyFavorit", data, errorHandling, done, null, false);
       
    };
    let productSavingLinks = document.querySelectorAll('.product-saving');
    if (!productSavingLinks) {
        return false;
    }
    productSavingLinks.forEach(el => {
        el.addEventListener('click',async (e) => {
            e.stopPropagation();
            e.preventDefault();
            if (el.classList.contains('saved-product')) {
                await unsaveProduct(el);
            } else {
                await saveProduct(el);
            }

        }, false);
    });
}
