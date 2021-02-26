$(document).ready(function () {
    
    (function () {

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

        if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
            module.exports = commafy;
        } else {
            window.commafy = commafy;
        }

    })();
    var persian = { 0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹' };
    function traverse(el) {
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
    let promise = new Promise((resolve) => {
        traverse(document.body);
        resolve();
    });
    promise.then(() => {
        let videoElems = document.querySelectorAll('.video-content');
        videoElems.forEach(function (el) {
            let list = el.innerHTML.match(/[۰-۹]/g);
            console.log(list);
            if (list !== null && list.length > 0) {
                for (let i = 0; i < list.length; i++) {
                    let item = Object.keys(persian).filter(function (key) { return persian[key] === list[i] })[0];
                    el.innerHTML = el.innerHTML.replace(list[i], item);
                }
            }
        });
    });

    //******* Ajax Forms ********
    //Class for each ajax form
    class Ajax {
        constructor(dataObject, url, method, successMessage) {
            this.data = dataObject;
            this.url = url;
            this.method = method;
            this.success = function () {
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">'
                        + successMessage + '</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            };
        }

        ajaxFormSubmit(susscessFunction = this.success) {

            $.ajax({
                url: this.url,
                data: this.data,
                type: this.method,
                error: function (xhr) {
                    alert(xhr.responseText);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">عملیات با خطا مواجه شد</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                },
                success: function (data) {

                    let mainContentLength = $('.main-content:hidden').length;
                    if (mainContentLength > 0) {

                        if (data === "True") {
                            susscessFunction();
                        } else {
                            $('.main-content:eq(0)').html(data);
                            $('.customer-side-bar > div').removeClass('position-relative').attr('uk-sticky', 'offset: 140;bottom: #end');

                        }
                    } else {

                        if (data === "True") {
                            susscessFunction();
                        } else {
                            $('.main-content:eq(1)').html(data);

                        }
                    }
                    $('#customer-loader').addClass('d-none');
                    mylink.find('.ukspinner').addClass('d-none');
                    mylink.removeClass('uk-disabled');
                    const mdcButton01 = $('.mdc-button');
                    for (let i = 0; i < mdcButton01.length; i++) {
                        mdc.ripple.MDCRipple.attachTo(document.querySelectorAll('.mdc-button')[i]);
                    }

                    const mdcTextField01 = $('.mdc-text-field');
                    for (let i = 0; i < mdcTextField01.length; i++) {
                        mdc.textField.MDCTextField.attachTo(document.querySelectorAll('.mdc-text-field')[i]);
                    }
                    UIkit.sticky('.customer-side-bar > div');
                    traverse(document.body);
                }
            });


        }
    }

    //customer information class for implementation it's ajax form
    //Url: Customer/CustomerInformation
    //Data: Mobile, Email, FullName, CartBankNumber
    //Method: Post
    class CustomerInformation extends Ajax {
        constructor(fullName, mobile, email, cartBankNumber) {
            super(null, '/Customer/CustomerInformationOnPost', 'POST', 'تغییرات با موفقیت ذخیره شد');
            this.data = {
                FullName: fullName,
                Mobile: mobile,
                Email: email,
                CartBankNumber: cartBankNumber
            };

        }
    }
    //============Layout Page=================
    //menu category list
    $('.dropdown-list-container__list__link.category-has-subcat').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let id = $(this).attr('data-id');
        $.ajax({
            url: '/Category/MenuSubCategoryList',
            type: "POST",
            data: {
                id: id
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('.dropdown-list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //reset menu category list when close
    $('.top-menu__nav-list__main-category').click(function () {
        $.ajax({
            url: '/Category/MenuCategoryList',
            type: "GET",
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('.dropdown-list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //back to the parent category in category list menu
    $('.back-category--button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let id = $(this).attr('data-parent-id');
        $.ajax({
            url: '/Category/MenuSubCategoryList',
            type: "POST",
            data: {
                id: id
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('.dropdown-list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //mobile menu
   
    //show category list when click on category link
    $('.mobile-menu__main-category').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        $.ajax({
            url: "/Category/MobileMenuCategoryList",
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#mobile-menu__list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //when in mobile menu click on category that has subcategory
    $('.mobile-menu-list__link.category-has-subcat').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let id = $(this).attr('data-id');
        $.ajax({
            url: '/Category/MobileMenuSubCategoryList',
            type: "POST",
            data: {
                id: id
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#mobile-menu__list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //back to the parent category in mobile menu
    $('.mobile-back-category--button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let id = $(this).attr('data-parent-id');
        $.ajax({
            url: '/Category/MobileMenuSubCategoryList',
            type: "POST",
            data: {
                id: id
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#mobile-menu__list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //back to the main menu in mobile menu
    $('.mobile-back-to-main-menu').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        $.ajax({
            url: "/Category/BackToMainMobileMenu",
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#mobile-menu__list-container').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    $(window).resize(function () {
        $('#customer-page .main-content:eq(0)').load('/Customer/SideBar');
    });

    //===========Register/Login=================
    $('.visible-password-icon').click(function () {
        let input = $(this).siblings('input');
        let type = $(input).attr('type');
        if (type === "password") {
            $(input).attr('type', 'text');
        } else {
            $(input).attr('type', 'password');
        }
    });

    //============Product Pages=================
    //Magnifier: 1- when mouse hover on the product image, magnifying glass box will be visible and moving
    $(".product-image__image-container").mousemove(function (e) {
        let magnifierGlassBox = $(this).find('.magnifier-glass-box');
        var parentOffset = $(this).offset();
        var windowWidth = $(window).width() * 0.03;
        //or $(this).offset(); if you really just want the current element's offset
        var relX = e.pageX - parentOffset.left - windowWidth / 2;
        var relY = e.pageY - parentOffset.top - windowWidth / 2;

        $(magnifierGlassBox).css({
            "transform": "translate(" + relX + "px, " + relY + "px)"
        });
        $('.product-image__big-image-container').css({
            "background-position": ((-1) * relX * 2.65) + "px " + ((-1) * relY * 2.65) + "px"
        });
        let myurl = $(this).find('img').attr('data-url');
        $('.product-image__big-image-container').css({
            'display': 'block',
            'background-image': 'url(' + myurl + ')'
        });
    });

    $('.product-image__image-container').hover(function () {
        let myurl = $(this).find('img').attr('data-url');
        $('.product-image__big-image-container').css({
            'display': 'block',
            'background-image': 'url("' + myurl + '")'
        });
    }, function () {
        $('.product-image__big-image-container').fadeOut();
    });

    //Mobile => Click on large picture
    $('.product-image__image-container--mobile .default-image').click(function () {

        UIkit.lightbox('.product-image__other-images--mobile').show();
    });

    //Change product picture when click on another pictures of product
    $('.product-image__other-images a').click(function (e) {

        let myurl = $(this).attr('data-url');
        let mysrc = $(this).attr('data-src');
        let mysrcset = $(this).attr('data-srcset');
        $('.product-image__other-images a').removeClass('active').removeClass('uk-disabled');
        $(this).addClass('active').addClass('uk-disabled');
        $('.product-image__image-container img').attr('src', mysrc)
            .attr('data-src', mysrc)
            .attr('data-srcset', mysrcset);
        $('.product-image__image-container img').attr('data-url', myurl);
        $('.product-image__image-container--mobile > a > img').attr('src', mysrc)
            .attr('data-src', mysrc)
            .attr('data-srcset', mysrcset);
        $('.product-image__image-container--mobile > a > img').attr('data-url', myurl);
        (async () => {
            if ('loading' in HTMLImageElement.prototype) {
                const images = document.querySelectorAll("img.lazyload");
                images.forEach(img => {
                    img.src = img.dataset.src;
                });
            } else {
                // Dynamically import the LazySizes library
                const lazySizesLib = await import('/js/lazysizes.min.js');
                // Initiate LazySizes (reads data-src & class=lazyload)
                lazySizes.init(); // lazySizes works off a global.
            }
        })();
        e.stopPropagation();
        e.preventDefault();
    });

    //Switch between two tabs (comments and product-information) in product page
    //product-information
    $('#product-description-trigger').click(function (e) {

        e.preventDefault();
        let productId = $(this).attr('data-id');
        $.ajax({
            url: "/Products/ProductDescription",
            type: "POST",
            data: {
                productId: productId
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#product-main-content').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //product comments
    $('#product-comment-trigger').click(function (e) {

        e.preventDefault();
        let productId = $(this).attr('data-id');
        $.ajax({
            url: "/Products/ProductCommentList",
            type: "POST",
            data: {
                productId: productId
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#product-main-content').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //Change price with changing product variations
    try {
        const MDCProductVariation = mdc.select.MDCSelect;
        const productVariation = [].map.call(document.querySelectorAll('.product-variation'), function (el) {
            return new MDCProductVariation(el);
        });
        productVariation.forEach((item, i) => {
            item.listen('MDCSelect:change', function () {
                let elem = $('.product-variation:eq(' + i + ')')
                    .find('.mdc-list-item:eq(' + item.selectedIndex + ')');
                $('.addneworder-form__ids-container').html('');
                productVariation.forEach((pitem, index) => {
                    $('.addneworder-form__ids-container')
                        .append('<input type="hidden" class="d-none" name="ids" value="' + pitem.value + '"/>');
                });
                let subproductvariations = $('.sub-product-variation.mdc-list-item--selected');
                let discountPrice = Number($('.product-price').attr('data-discount-price'));
                let prices = [];

                subproductvariations.each(function () {
                    let price = $(this).attr('data-price');
                    let hasDifPrice = $(this).attr('data-has-difprice');
                    if (hasDifPrice.toLowerCase() === "true") {
                        prices.push(Number(price) - discountPrice);
                    }

                });

                if (prices.length > 0) {
                    $('.product-price').html(Math.max(...prices));
                    traverse(document.body);
                }
            });
        });

    } catch (e) {
        console.log(e);
    }
    //============Shopping Basket==============
    //remove an order
    $('.delete-order').click(function (e) {
        let id = $(this).attr('data-id');
        $('#delete--modal input[name="orderId"]').val(id);
    });

    //increase and decrease an order
    //increase an order
    $('.increase-order').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let id = $(this).attr('data-id');
        let parent = $(this).parents('.order-item');
        let orderCount = $(parent).find('.order-count');
        let numberOfOrderCount = Number($(orderCount).val());
        let orderPrices = $('.order-price');

        let totalPrice = 0;
        $.ajax({
            url: "/Shopping/IncreaseOrder",
            type: "POST",
            data: {
                orderId: id
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {

                let price = Number(data);
                $(parent).find('.order-price').html(commafy(price)).attr('data-price', data);
                $(orderCount).val(numberOfOrderCount + 1)
                    .attr('data-count', (numberOfOrderCount + 1));

            }
        }).done(function () {
            let ordersCount = 0;
            $('.order-count').each(function () {

                ordersCount += Number($(this).val());
            });
            $('.orders-count').html(`(${ordersCount})`);


            orderPrices.each(function () {
                totalPrice += Number($(this).attr('data-price'));
            });
            $('.total-price').html(commafy(totalPrice));
            $('#progress-loader').addClass('d-none');
            traverse(document.body);

        });
    });
    //decrease an order
    $('.decrease-order').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let id = $(this).attr('data-id');
        let parent = $(this).parents('.order-item');
        let orderCount = $(parent).find('.order-count');
        let numberOfOrderCount = Number($(orderCount).val());
        if (numberOfOrderCount < 2) {
            return false;
        }
        let orderPrices = $('.order-price');

        let totalPrice = 0;
        $.ajax({
            url: "/Shopping/DecreaseOrder",
            type: "POST",
            data: {
                orderId: id
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {

                let price = Number(data);
                $(parent).find('.order-price').html(commafy(price)).attr('data-price', data);
                $(orderCount).val(Math.abs(numberOfOrderCount - 1))
                    .attr('data-count', Math.abs(numberOfOrderCount - 1));

            }
        }).done(function () {
            let ordersCount = 0;
            $('.order-count').each(function () {

                ordersCount -= Number($(this).val());
            });
            $('.orders-count').html(`(${Math.abs(ordersCount)})`);


            orderPrices.each(function () {
                totalPrice -= Number($(this).attr('data-price'));
            });
            $('.total-price').html(commafy(Math.abs(totalPrice)));
            $('#progress-loader').addClass('d-none');
            traverse(document.body);

        });
    });
    //change order number
    
    $('.order-count').keyup(function (e) {
        let numberOfOrderCount = $(this).val();
        let id = $(this).attr('data-id');
        let parent = $(this).parents('.order-item');
        let thisElem = $(this);
        console.log("number:", numberOfOrderCount);
        if (numberOfOrderCount === null || $.trim(numberOfOrderCount) === "") {
            $(this).val(1);
            numberOfOrderCount = 1;
            console.log("number is null");
        }
        else if (Number(numberOfOrderCount) < 1) {
            $(this).val(1);
            numberOfOrderCount = 1;
            
        }
        let orderPrices = $('.order-price');
        let totalPrice = 0;
        $.ajax({
            url: "/Shopping/ChangeOrderNumber",
            type: "POST",
            data: {
                orderId: id,
                number: numberOfOrderCount
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {

                let price = Number(data);
                $(parent).find('.order-price').html(commafy(price)).attr('data-price', data);
                $(thisElem)
                    .attr('data-count', Math.abs(numberOfOrderCount));

            }
        }).done(function () {
            let ordersCount = 0;
            $('.order-count').each(function () {

                ordersCount -= Number($(this).val());
            });
            $('.orders-count').html(`(${Math.abs(ordersCount)})`);


            orderPrices.each(function () {
                totalPrice -= Number($(this).attr('data-price'));
            });
            $('.total-price').html(commafy(Math.abs(totalPrice)));
            $('#progress-loader').addClass('d-none');
            traverse(document.body);

        });
    });
    //============Address Information page===========
    try {
        const MDCCity = mdc.select.MDCSelect;
        const city = new MDCCity(document.querySelector('.city-of-address'));

        const MDCState = mdc.select.MDCSelect;
        const state = new MDCState(document.querySelector('.state-of-address'));
       
        state.listen('MDCSelect:change', function () {
            
            let val = state.value;
            console.log(val);
            $.ajax({
                url: "/Address/CityList",
                type: "POST",
                data: {
                    stateId: val
                },
                success: function (data) {
                    console.log(data);
                    $('.city-of-address__list-items').html(data);
                    city.selectedIndex = 0;
                    UIkit.modal('#add-address-form').show();
                }
            });
        });
        //add new address
        $('.add-address-form__submit-button').click(function (e) {
            let receiverName = $('#add-address-form input[name="ReceiverName"]').val();
            if (receiverName === null || $.trim(receiverName) === "") {
                $('#add-address-form .receivername-error').removeClass('d-none')
                    .html('وارد کردن نام گیرنده الزامی است');
                return false;
            }
            let receiverMobileNumber = $('#add-address-form input[name="ReceiverMobileNumber"]').val();
            if (receiverMobileNumber === null || $.trim(receiverMobileNumber) === "") {
                $('#add-address-form .receivermobilenumber-error').removeClass('d-none')
                    .html('وارد کردن شماره همراه گیرنده الزامی است');
                return false;
            }
            let postalCode = $('#add-address-form input[name="PostalCode"]').val();
            if (postalCode === null || $.trim(postalCode) === "") {
                $('#add-address-form .postalcode-error').removeClass('d-none')
                    .html('وارد کردن کد پستی الزامی است');
                return false;
            }
            let details = $('#add-address-form textarea[name="Details"]').val();
            if (details === null || $.trim(details) === "") {
                $('#add-address-form .details-error').removeClass('d-none')
                    .html('وارد کردن جزییات آدرس الزامی است');
                return false;
            }
            //get city id and city name
            let cityId = city.value;
            let citySelectedIndex = city.selectedIndex;
            let cityName = $('#add-address-form .city-of-address .city-item:eq(' + citySelectedIndex + ')')
                .attr('data-city-name');

            //get state id and state name
            let stateId = state.value;
            let stateSelectedIndex = state.selectedIndex;
            let stateName = $('#add-address-form .state-of-address .state-item:eq(' + stateSelectedIndex + ')')
                .attr('data-state-name');

            //put data on the form
            $('#add-address-form input[name="CityId"]').val(cityId);
            $('#add-address-form input[name="CityName"]').val(cityName);
            $('#add-address-form input[name="StateId"]').val(stateId);
            $('#add-address-form input[name="StateName"]').val(stateName);

        });
    } catch (e) {
        console.log(e);
    }

    //Edit Address
    try {
        const MDCEditCity = mdc.select.MDCSelect;
        const editCity = new MDCEditCity(document.querySelector('.edit-city-of-address'));

        const MDCEditState = mdc.select.MDCSelect;
        const editState = new MDCEditState(document.querySelector('.edit-state-of-address'));

        editState.listen('MDCSelect:change', function () {
            let parent = $(this).parents('.edit-address-modal');
            let val = editState.value;
            console.log(val);
            $.ajax({
                url: "/Address/CityList",
                type: "POST",
                data: {
                    stateId: val
                },
                success: function (data) {
                    console.log(data);
                    $('.edit-city-of-address__list-items').html(data);
                    UIkit.modal(parent).show();
                    editCity.selectedIndex = 0;
                }
            });
        });
        //add new address
        $('.edit-address-form__submit-button').click(function (e) {
            let parent = $(this).parents('.edit-address-form');
            let receiverName = $(parent).find('input[name="ReceiverName"]').val();
            if (receiverName === null || $.trim(receiverName) === "") {
                $(parent).find('.receivername-error').removeClass('d-none')
                    .html('وارد کردن نام گیرنده الزامی است');
                return false;
            }
            let receiverMobileNumber = $(parent).find('input[name="ReceiverMobileNumber"]').val();
            if (receiverMobileNumber === null || $.trim(receiverMobileNumber) === "") {
                $(parent).find('.receivermobilenumber-error').removeClass('d-none')
                    .html('وارد کردن شماره همراه گیرنده الزامی است');
                return false;
            }
            let postalCode = $(parent).find('input[name="PostalCode"]').val();
            if (postalCode === null || $.trim(postalCode) === "") {
                $(parent).find('.postalcode-error').removeClass('d-none')
                    .html('وارد کردن کد پستی الزامی است');
                return false;
            }
            let details = $(parent).find('textarea[name="Details"]').val();
            if (details === null || $.trim(details) === "") {
                $(parent).find('.details-error').removeClass('d-none')
                    .html('وارد کردن جزییات آدرس الزامی است');
                return false;
            }
            //get city id and city name
            let cityId = editCity.value;
            let citySelectedIndex = editCity.selectedIndex;
            let cityName = $(parent).find('.edit-city-of-address .city-item:eq(' + citySelectedIndex + ')')
                .attr('data-city-name');

            //get state id and state name
            let stateId = editState.value;
            let stateSelectedIndex = editState.selectedIndex;
            let stateName = $(parent).find('.edit-state-of-address .state-item:eq(' + stateSelectedIndex + ')')
                .attr('data-state-name');

            //put data on the form
            $(parent).find('input[name="CityId"]').val(cityId);
            $(parent).find('input[name="CityName"]').val(cityName);
            $(parent).find('input[name="StateId"]').val(stateId);
            $(parent).find('input[name="StateName"]').val(stateName);

        });
    } catch (e) {
        console.log(e);
    }
    //reset errors
    $('input[name="ReceiverName"]').focus(function () {
        $('.receivername-error').addClass('d-none').html('');
    });

    $('input[name="ReceiverMobileNumber"]').focus(function () {
        $('.receivermobilenumber-error').addClass('d-none').html('');
    });

    $('input[name="PostalCode"]').focus(function () {
        $('.postalcode-error').addClass('d-none').html('');
    });

    $('textarea[name="Details"]').focus(function () {
        $('.details-error').addClass('d-none').html('');
    });

    //delete an address
    $('.delete-address-link').click(function (e) {
        let addressId = $(this).attr('data-id');
        $('#delete-address--modal input[name="addressId"]').val(addressId);

    });

    //change selected address
    let onSelectedAddressChange = jQuery.Event('onSelectedAddressChange');
    $('input[name="IsSelectedAddress"]').on('onSelectedAddressChange', function () {
        let selectedAddressId = $(this).attr('data-id');
        console.log('selectedId:', selectedAddressId);
        $.ajax({
            url: "/Address/BillWithDeliveryPrice",
            type: "POST",
            data: {
                selectedAddressId: selectedAddressId
            },
            success: function (data) {
                $('.bill-with-delivery-price-container').html(data);
                traverse(document.body);
            }
        });

        $.ajax({
            url: "/Address/TotalPrice",
            type: "POST",
            data: {
                selectedAddressId: selectedAddressId
            },
            success: function (data) {
                $('.total-price-container').html(data);
                traverse(document.body);
            }
        });
    });
    $('input[name="IsSelectedAddress"]').change(function () {
        let selectedAddressId = $(this).attr('data-id');
        let thisElem = $(this);
        console.log('id:', selectedAddressId);
        $.ajax({
            url: "/Address/ChangeSelectedAddress",
            type: "POST",
            data: {
                selectedAddressId: selectedAddressId
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(thisElem).trigger(onSelectedAddressChange);

                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
            UIkit.notification({
                message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">آدرس انتخاب شده تغییر یافت</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 5000
            });
            traverse(document.body);
        });
    });
    //============Confirm and payment===========
    //change payment type

    $('input[name="IsLocalPayment"]').change(function () {
        let shoppingId = $(this).attr('data-id');
        let isLocalPayment = $(this).val();
        if ($(this).is(':checked')) {
            $(this).prop('checked', true);
            isLocalPayment = $(this).val();
            console.log(isLocalPayment);
        }
        $.ajax({
            url: "/Shopping/ChangeIsLocalPayment",
            type: "POST",
            data: {
                shoppingId: shoppingId,
                isLocalPayment: isLocalPayment
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">نحوه پرداخت تغییر کرد</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //using descount code 
    let onSetDiscountCode = jQuery.Event('onSetDiscountCode');
    //when discount code is exists, price should be changed
    $('.discount-submit').on('onSetDiscountCode', function () {
        let shoppingId = $(this).attr('data-id');
        $.ajax({
            url: "/Shopping/FinalBill",
            type: "POST",
            data: {
                shoppingId: shoppingId
            },
            success: function (data) {
                $('.final-bill-container').html(data);
                traverse(document.body);
            }
        });

        $.ajax({
            url: "/Shopping/TotalPrice",
            type: "POST",
            data: {
                shoppingId: shoppingId
            },
            success: function (data) {
                $('.total-price').html(commafy(Number(data)));
                traverse(document.body);
            }
        });
    });
    $('.discount-submit').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let discountCode = $('input[name="discountCode"]').val();
        let shoppingId = $(this).attr('data-id');
        $.ajax({
            url: "/Shopping/UsingDiscountCode",
            type: "POST",
            data: {
                discountCode: discountCode,
                shoppingId: shoppingId
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $('.discount-submit').trigger(onSetDiscountCode);
                    $('.error-discount-code').addClass('d-none').html('');
                    $('.success-discount-code').removeClass('d-none').html('کد تخفیف اعمال شد');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">کد تخفیف اعمال شد</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    $('.discount-submit').trigger(onSetDiscountCode);
                    $('.error-discount-code').removeClass('d-none').html('کد تخفیف فاقد اعتبار است');
                    $('.success-discount-code').addClass('d-none').html('');
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
            traverse(document.body);
        });
    });
    //============Customer Page=================
    //verify ticket befor sending
    $(document).on('click', '.send-ticket-submit-button', function (e) {
        let subject = $('input[name="ticketSubject"]').val();
        let description = $('textarea[name="ticketDescription"]').val();
        if ($.trim(subject) === null || $.trim(subject) === "") {
            $('.ticketSubject-error').removeClass('d-none').html('وارد کردن موضوع الزامی است');
            e.stopPropagation();
            e.preventDefault();
            return false;
        }
        if ($.trim(description) === null || $.trim(description) === "") {
            $('.ticketDescription-error').removeClass('d-none').html('وارد کردن توضیحات الزامی است');
            e.stopPropagation();
            e.preventDefault();
            return false;
        }
    });
    //reset errors
    $(document).on('focus', 'input[name="ticketSubject"]', function () {
        $('.ticketSubject-error').addClass('d-none').html('');
    });
    $(document).on('focus', 'textarea[name="ticketDescription"]', function () {
        $('.ticketDescription-error').addClass('d-none').html('');
    });
    //remove a ticket 
    $(document).on('click', '.delete-ticket-link', function (e) {
        let id = $(this).attr('data-id');
        $('#item-remove-modal #remove-item-form-on-modal input[name="id"]').val(id);
    });
    //When return-from-change-password button clicked
    $(document).on('click', '#return-from-change-password',function () {
        $('#change-password-link').removeClass('active-link');
        let returnURL = $(this).attr('data-target-url');
        partialLinkClick(this);
        $('.customer-side-bar__list li a[data-target-url="' + returnURL + '"]').parents('li').addClass('active');
    });

    //click on ticket pagination buttons
    $(document).on('click','#ticket-pagination-buttons-container .pagination--button',function (e) {
        let myIndex = $(this).attr('data-page-index');
        $.ajax({
            url: "/Customer/TicketList",
            type: "POST",
            data: {
                pageIndex: myIndex
            },
            success: function (data) {
                $('#ticket-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
                traverse(document.body);
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
    //====================Products (Filtering) =======================
    //when clicking on a filter, it's item will be open

    $('.filter__list-item').click(function (e) {
        let subFilter = $(this).find('.filter__list-item__sub-list');
        if (subFilter.hasClass('active')) {
            subFilter.removeClass('active');
            $(this).find('.arrow-icon-container i').first().html('keyboard_arrow_down');
            e.stopPropagation();
            e.preventDefault();
            return;
        }
        subFilter.addClass('active');
        $(this).find('.arrow-icon-container i').first().html('keyboard_arrow_up');
        e.stopPropagation();
        e.preventDefault();

        return;
    });
    $('.filter__list-item__sub-list__list-item').click(function (e) {

        e.stopPropagation();
    });

    try {
        const MDCProductSort = mdc.select.MDCSelect;
        const productSort = new MDCProductSort(document.querySelector('.category-detail__product-sort'));
        productSort.listen('MDCSelect:change', function () {
            let sort = productSort.value;
            let defaultCatId = $('.category-detail__product-list').attr('data-cat-id');
            let defaultBrandId = $('.category-detail__product-list').attr('data-brand-id');
            let searchText = $('.category-detail__product-list').attr('data-search-text');
           
            $('.category-detail__product-list').attr('data-sort-value', sort);
            let selectedCats = $('input[name="categoryCheckbox"]:checked');
            let catIds = [];
            selectedCats.each(function () {
                let myId = $(this).attr('data-id');
                catIds.push(myId);
            });

            let selectedBrands = $('input[name="brandCheckbox"]:checked');
            let brandIds = [];
            selectedBrands.each(function () {
                let myId = $(this).attr('data-id');
                brandIds.push(myId);
            });
            if (defaultCatId === null || $.trim(defaultCatId) === "") {
                defaultCatId = -1;
            }
            if (defaultBrandId === null || $.trim(defaultBrandId) === "") {
                defaultBrandId = -1;
            }
            if (searchText === null || $.trim(searchText) === "") {
                searchText = null;
            } 
            $.ajax({
                url: "/Category/CategoryProductList",
                type: "POST",
                data: {
                    defaultCategoryId: defaultCatId,
                    catIds: catIds,
                    defaultBrandId: defaultBrandId,
                    brandIds: brandIds,
                    sort: sort,
                    searchText: searchText
                },
                traditional: true,
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#product-list-loader').addClass('d-flex').removeClass('d-none');
                    
                },
                success: function (data) {
                    $('.category-detail__product-list').html(data);
                }
            }).done(function () {
                $('#product-list-loader').addClass('d-none').removeClass('d-flex');
            });
        });

        //Select category or categories in filter settings
        $('.desktop-filter-box input.filter-checkbox').change(function (e) {
            productSort.selectedIndex = 0;
            let defaultCatId = $('.category-detail__product-list').attr('data-cat-id');
            let defaultBrandId = $('.category-detail__product-list').attr('data-brand-id');
            let searchText = $('.category-detail__product-list').attr('data-search-text');
            let selectedCats = $('.desktop-filter-box input[name="categoryCheckbox"]:checked');
            let catIds = [];
            selectedCats.each(function () {
                let myId = $(this).attr('data-id');
                catIds.push(myId);
            });

            let selectedBrands = $('.desktop-filter-box input[name="brandCheckbox"]:checked');
            let brandIds = [];
            selectedBrands.each(function () {
                let myId = $(this).attr('data-id');
                brandIds.push(myId);
            });
            if (defaultCatId === null || $.trim(defaultCatId) === "") {
                defaultCatId = -1;
            }
            if (defaultBrandId === null || $.trim(defaultBrandId) === "") {
                defaultBrandId = -1;
            }
            if (searchText === null || $.trim(searchText) === "") {
                searchText = null;
            }
            $.ajax({
                url: "/Category/CategoryProductList",
                type: "POST",
                data: {
                    defaultCategoryId: defaultCatId,
                    catIds: catIds,
                    defaultBrandId: defaultBrandId,
                    brandIds: brandIds,
                    searchText: searchText
                },
                traditional: true,
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#product-list-loader').addClass('d-flex').removeClass('d-none');
                },
                success: function (data) {
                    $('.category-detail__product-list').html(data);
                }
            }).done(function () {
                $('#product-list-loader').addClass('d-none').removeClass('d-flex');
            });
        });

        //select category in mobile filter box
        $('.mobile-filter-box__button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            UIkit.modal('#filter-box--modal').hide();

            productSort.selectedIndex = 0;
            let defaultCatId = $('.category-detail__product-list').attr('data-cat-id');
            let defaultBrandId = $('.category-detail__product-list').attr('data-brand-id');
            let searchText = $('.category-detail__product-list').attr('data-search-text');
            let selectedCats = $('.mobile-filter-box input[name="categoryCheckbox"]:checked');
            let catIds = [];
            selectedCats.each(function () {
                let myId = $(this).attr('data-id');
                catIds.push(myId);
            });
            console.log(catIds);
            let selectedBrands = $('.mobile-filter-box input[name="brandCheckbox"]:checked');
            let brandIds = [];
            selectedBrands.each(function () {
                let myId = $(this).attr('data-id');
                brandIds.push(myId);
            });
            if (defaultCatId === null || $.trim(defaultCatId) === "") {
                defaultCatId = -1;
            }
            if (defaultBrandId === null || $.trim(defaultBrandId) === "") {
                defaultBrandId = -1;
            }
            if (searchText === null || $.trim(searchText) === "") {
                searchText = null;
            }
            $.ajax({
                url: "/Category/CategoryProductList",
                type: "POST",
                data: {
                    defaultCategoryId: defaultCatId,
                    catIds: catIds,
                    defaultBrandId: defaultBrandId,
                    brandIds: brandIds,
                    searchText: searchText
                },
                traditional: true,
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#product-list-loader').addClass('d-flex').removeClass('d-none');
                },
                success: function (data) {
                    $('.category-detail__product-list').html(data);
                }
            }).done(function () {
                $('#product-list-loader').addClass('d-none').removeClass('d-flex');
            });
        });

      
    } catch (e) {
        console.log(e);
    }

    try {

        //Product paginations
        $('#product-list__pageination-buttons-container .pagination--button').click(function (e) {
            console.log('hello');
            e.stopPropagation();
            e.preventDefault();
            let pageIndex = $(this).attr('data-page-index');
            //  let sort = productSort02.value;
            let defaultCatId = $('.category-detail__product-list').attr('data-cat-id');
            let defaultBrandId = $('.category-detail__product-list').attr('data-brand-id');
            let searchText = $('.category-detail__product-list').attr('data-search-text');
            let sort = $('.category-detail__product-list').attr('data-sort-value');
            let selectedCats = $('input[name="categoryCheckbox"]:checked');
            let catIds = [];
            selectedCats.each(function () {
                let myId = $(this).attr('data-id');
                catIds.push(myId);
            });

            let selectedBrands = $('input[name="brandCheckbox"]:checked');
            let brandIds = [];
            selectedBrands.each(function () {
                let myId = $(this).attr('data-id');
                brandIds.push(myId);
            });
            if (defaultCatId === null || $.trim(defaultCatId) === "") {
                defaultCatId = -1;
            }
            if (defaultBrandId === null || $.trim(defaultBrandId) === "") {
                defaultBrandId = -1;
            }
            if (searchText === null || $.trim(searchText) === "") {
                searchText = null;
            }
            $.ajax({
                url: "/Category/CategoryProductList",
                type: "POST",
                data: {
                    defaultCategoryId: defaultCatId,
                    catIds: catIds,
                    defaultBrandId: defaultBrandId,
                    brandIds: brandIds,
                    sort: sort,
                    pageIndex: pageIndex,
                    searchText: searchText
                },
                traditional: true,
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#product-list-loader').addClass('d-flex').removeClass('d-none');
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                },
                success: function (data) {
                    $('.category-detail__product-list').html(data);
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                }
            }).done(function () {
                $('#product-list-loader').addClass('d-none').removeClass('d-flex');
            });
        });
    } catch (e) {
        console.log(e);
    }

    //================Blog page==========================
    console.log('hello');
    //rating number
    try {
        
        $('.blog-stars-container input[name="star"]').change(function () {
            console.log('hello');
            $('.rate-result').html($(this).val());
            $('.blog-stars-container').attr('data-rate-number', $(this).val());
        });

        //add new comment
        $('.blog-submit-comment--button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let blogId = $(this).attr('data-id');
            let description = $('textarea[name="description"]').val();
            let rate = $('.blog-stars-container').attr('data-rate-number');
            if (description === null || description.trim() === "") {
                $('.error-comment-description')
                    .removeClass('d-none')
                    .html('برای ثبت نظر، پر کردن این فیلد الزامی است');
                return false;
            }
            $.ajax({
                url: "/BlogComment/AddNewComment",
                type: "POST",
                data: {
                    blogId: blogId,
                    description: description,
                    rate: rate
                },
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');
                    $('.blog-submit-comment--button').attr('disabled', 'disabled');
                },
                success: function (data) {
                    if (parseInt(data) === 101) {
                        UIkit.modal('#comment-add-modal').hide();
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">نظر شما با موفقیت ثبت شد</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 3000
                        });
                    } else if (parseInt(data) === 102) {
                        UIkit.modal('#comment-add-modal').hide();
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">نظر شما برای این مقاله قبلا ثبت شده است</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 3000
                        });
                    }
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }
    $('textarea[name="description"]').focus(function () {
        $('.error-comment-description').addClass('d-none').html('');
    });
    $('textarea[name="replyDescription"]').focus(function () {
        $('.error-reply-description').addClass('d-none').html('');
    });
    //Reply to comment
    $('.blog-reply-comment-trigger').click(function (e) {
        let commentId = $(this).attr('data-commentId');
        $('.blog-submit-reply-comment--button').attr('data-commentId', commentId);
    });
    $('.blog-submit-reply-comment--button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let description = $('textarea[name="replyDescription"]').val();
        if (description === null || $.trim(description) === "") {
            $('.error-reply-description')
                .removeClass('d-none')
                .html('برای ثبت وارد کردن نظر الزامی است');
            return false;
        }
        let commentId = $(this).attr('data-commentId');
        $.ajax({
            url: "/BlogComment/AddReplyComment",
            type: "POST",
            data: {
                description: description,
                commentId: commentId
            },
            error: function (xhr) { console.log(xhr.responseText); },
            beforeSend: function () {
                $('#progress-loader').removeClass('d-none');
                $('.blog-submit-reply-comment--button').attr('disabled', 'disabled');
            },
            success: function (data) {
                if (parseInt(data) === 101) {
                    UIkit.modal('#reply-comment-add-modal').hide();
                    $('.blog-submit-reply-comment--button').removeAttr('disabled');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="rtl">نظر شما با موفقیت ثبت شد</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });

    });
    try {
        $('#blog-comment-list__pageination-buttons-container .pagination--button').click(function (e) {
            console.log('hello');
            e.stopPropagation();
            e.preventDefault();
            let blogId = $('#blog-comment-list__pageination-buttons-container').attr('data-blog-id');

            let pageIndex = $(this).attr('data-page-index');
            $.ajax({
                url: "/Blog/BlogCommentList",
                type: "POST",
                data: {
                    blogId: blogId,
                    pageIndex: pageIndex
                },
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');

                },
                success: function (data) {
                    $('#blog-comment').html(data);
                    $('html,body').animate({ scrollTop: ($('#blog-comment').offset().top - 130) }, 'slow');
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }

    try {
        $('#blog-comment-list__pageination-buttons-container .pagination--button').click(function (e) {
            console.log('hello');
            e.stopPropagation();
            e.preventDefault();
            let blogId = $('#blog-comment-list__pageination-buttons-container').attr('data-blog-id');

            let pageIndex = $(this).attr('data-page-index');
            $.ajax({
                url: "/Blog/BlogCommentList",
                type: "POST",
                data: {
                    blogId: blogId,
                    pageIndex: pageIndex
                },
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');

                },
                success: function (data) {
                    $('#blog-comment').html(data);
                    $('html,body').animate({ scrollTop: ($('#blog-comment').offset().top - 130) }, 'slow');
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }
    try {
        
        //Blog paginations
        $('#blog-list__pageination-buttons-container .pagination--button').click(function (e) {
           
            e.stopPropagation();
            e.preventDefault();
            let pageIndex = $(this).attr('data-page-index');
            
            $.ajax({
                url: "/Blog/BlogListComponent",
                type: "POST",
                data: {
                    pageIndex: pageIndex
                },
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                },
                success: function (data) {
                    $('#blog-list-container').html(data);
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }

   
    //================Shopping Page=========================
   

    //==============Special Discount Page=================
    try {

        //Special Discount paginations
        $('#specialDiscount-list__pageination-buttons-container .pagination--button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let pageIndex = $(this).attr('data-page-index');

            $.ajax({
                url: "/SpecialDiscount/AllSpecialDiscountList",
                type: "POST",
                data: {

                    pageIndex: pageIndex

                },
                error: function (xhr) { console.log(xhr.responseText); },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                },
                success: function (data) {
                    $('#all-special-discount-list').html(data);
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }


    //==============Mobile Verification Page===============
    var verifyWrapper = $('#verify-wrapper');

    function goToNextInput(e) {

        var key = e.which || e.keyCode,
            t = $(e.target),
            sib = t.next('input');

        if (key !== 9 && ((key < 48 || key > 57) && !(key >= 96 && key <= 105))) {
            e.preventDefault();
            return false;
        }

        if (key === 9) {
            return true;
        }

        if (!sib || !sib.length) {
            $(this).select().focus();
            return;
        }
        sib.select().focus();
    }

    function onKeyDown(e) {
        var key = e.which || e.keyCode;

        if (key === 9 || ((key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
            return true;
        }

        e.preventDefault();
        return false;
    }

    function onFocus(e) {
        $(e.target).select();

    }



    verifyWrapper.on('keyup', 'input', goToNextInput);
    verifyWrapper.on('keydown', 'input', onKeyDown);
    verifyWrapper.on('click', 'input', onFocus);

    $('#verify-submit-button').click(function () {
        let tokenCode = "";
        $('input.verify-input-digit').each(function () {
            tokenCode += $(this).val();

        });
        $('input#Input_Token').val(tokenCode);

    });


    //=============Input Validation=================
    $('input.input-validation-error').change(function () {
        $(this).removeClass('input-validation-error');
    });
    ////============Google material design=================
    //Google material design button
    try {
        const mdcButton = $('.mdc-button');
        for (let i = 0; i < mdcButton.length; i++) {
            mdc.ripple.MDCRipple.attachTo(document.querySelectorAll('.mdc-button')[i]);
        }

        //mdc.ripple.MDCRipple.attachTo(document.querySelector('.mdc-fab'));



        const mdcTextField = $('.mdc-text-field');
        for (let i = 0; i < mdcTextField.length; i++) {
            mdc.textField.MDCTextField.attachTo(document.querySelectorAll('.mdc-text-field')[i]);
        }
        // mdc.tabBar.MDCTabBar.attachTo(document.querySelector('.my-tab-list'));
        const MDCTABLE = $('.mdc-data-table');
        for (let i = 0; i < MDCTABLE.length; i++) {
            mdc.dataTable.MDCDataTable.attachTo(document.querySelectorAll('.mdc-data-table')[i]);
        }

    } catch (e) {
        console.log(e);
    }
    

    

    function googleMaterialDesignInstall() {
        try {
            const mdcButton = $('.mdc-button');
            for (let i = 0; i < mdcButton.length; i++) {
                mdc.ripple.MDCRipple.attachTo(document.querySelectorAll('.mdc-button')[i]);
            }

            const mdcTextField = $('.mdc-text-field');
            for (let i = 0; i < mdcTextField.length; i++) {
                mdc.textField.MDCTextField.attachTo(document.querySelectorAll('.mdc-text-field')[i]);
            }
            //mdc.tabBar.MDCTabBar.attachTo(document.querySelector('.my-tab-list'));
            const MDCTABLE = $('.mdc-data-table');
            for (let i = 0; i < MDCTABLE.length; i++) {
                mdc.dataTable.MDCDataTable.attachTo(document.querySelectorAll('.mdc-data-table')[i]);
            }
        } catch (e) {
            console.log(e);
        }
       
    }

});