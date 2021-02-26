
$(document).ready(function () {
    
    //-------Ajax Function------------
    function runAjax(urlTarget, dataValue, method, replaceTargetId) {
        $.ajax({
            url: urlTarget,
            method: method,
            data: dataValue,
            success: function (data) {
                $(replaceTargetId).html(data);
                googleMaterialDesignInstall();
            }
        });
    }

    //-----------Search Ajax Input------------
    $('input.search-ajax-input').focus(function () {
        let target = $(this).attr('data-target');
        $(target).fadeIn();
    });
    $('input.search-ajax-input').blur(function () {
        let target = $(this).attr('data-target');
        $(target).fadeOut();
    });
    $('.search-result li').click(function () {
        let text = $(this).text();

        let parent = $(this).parents('.search-result');
        let inputName = $(parent).attr('data-name');
        $('input[name="' + inputName + '"]').val(text);
        $(parent).fadeOut();
    });

    //============ Charts ======================
    //***** income-time chart in index page *******
    function chartJs(ctx, myData, borderColor, backColor, myLabels) {
        let myFontSize = 13;
        if ($(window).width() < 800) {
            myFontSize = 9;
        } else {
            myFontSize = 13;
        }
        let myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: myLabels,

                datasets: [{
                    data: myData,
                    borderWidth: 2,
                    borderColor: borderColor,
                    backgroundColor: backColor
                }]
            },
            options: {
                legend: {
                    display: false

                },
                tooltips: {
                    titleFontFamily: 'IRANSans',
                    displayColors: false,
                    bodyAlign: 'center'
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            display: true

                        },
                        ticks: {
                            fontFamily: 'IRANSans',
                            fontSize: myFontSize
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            display: true
                        },
                        ticks: {
                            beginAtZero: false
                        }
                    }]
                }
            }
        });
        return myChart;
    }

    //==============Z-Index Controlling of select box============
    $(document).on('click', '.filter-mdc-select--container', function () {
        $('.filter-mdc-select--container').css('z-index', '2');
        $(this).css('z-index', '5');
    });
    //==============Top Menu Controller================
    //Search Form On Mobile:
    //open search form
    $('#search-button').click(function () {
        let targetId = $(this).attr('data-target');
        $(targetId).addClass('active');
    });
    //close search form
    $('#close-search-form-mobile').click(function () {
        let targetId = $(this).attr('data-target');
        $(targetId).removeClass('active');
    });

    //==============Side Menu Controller===============
    $('#side-menu--controller').click(function (e) {
        $('#totop-button').toggleClass('close');
        $('#side-menu').toggleClass('opened');
        $('.back-fade').toggleClass('opened');
        $('#side-menu-container').toggleClass('opened').toggleClass('opened-mobile');
        $('#back-of-sidemenu').toggleClass('back-show');
        if ($(window).width() < 1050) {
            if (!$('#side-menu-container').hasClass('opened')) {
                $('#side-menu').removeAttr('uk-sticky');
                $('html, body').scrollTop(0); 
                $('html').removeClass('offcanvas-html');
                $('body').removeClass('offcanvas-body');


            } else {
                $('#side-menu').attr('uk-sticky', '');
                $('html').addClass('offcanvas-html');
                $('body').addClass('offcanvas-body');
            }
        }

    });

    if ($(window).width() < 1050) {
        $('#side-menu').removeClass('opened');
        $('#side-menu-container').removeClass('opened').removeClass('opened-mobile');
        $('#back-of-sidemenu').removeClass('back-show');
        $('#top-menu__title-container').removeClass('opened');

    } else {
        $('#side-menu').addClass('opened');
        $('#side-menu-container').addClass('opened').addClass('opened-mobile');
        $('#back-of-sidemenu').addClass('back-show');
        $('#top-menu__title-container').addClass('opened');

    }

    $(window).resize(function () {
        if ($(window).width() < 1050) {
            $('#side-menu').removeClass('opened');
            $('#side-menu-container').removeClass('opened').removeClass('opened-mobile');
            $('#back-of-sidemenu').removeClass('back-show');
            $('#top-menu__title-container').removeClass('opened');
            if (!$('#side-menu-container').hasClass('opened')) {
                $('#side-menu').removeAttr('uk-sticky');
               
                $('html').removeClass('offcanvas-html');
                $('body').removeClass('offcanvas-body');

            } else {
                $('#side-menu').attr('uk-sticky', '');
                $('html').addClass('offcanvas-html');
                $('body').addClass('offcanvas-body');
            }
        } else {
            $('#side-menu').addClass('opened');
            $('#side-menu-container').addClass('opened').addClass('opened-mobile');
            $('#back-of-sidemenu').addClass('back-show');
            $('#top-menu__title-container').addClass('opened');
            $('#side-menu').attr('uk-sticky', '');
            $('html').removeClass('offcanvas-html');
            $('body').removeClass('offcanvas-body');
        }
    });



    $('.back-fade').click(function () {
        $('#side-menu--controller').trigger('click');
    });

    //===============Orders page=========================
    /*-----Show Detail And Edit Page When Thats Link Clicked-----*/
    $('#order-detail-edit--link').click(function (e) {

        //orderId => the id of the order
        var orderId = $(this).attr('data-id');
        $.ajax({
            url: '/Admin/Orders/DetailAndEdit',
            data: { id: orderId },
            method: "GET",
            beforeSend: function () {

            },
            error: function (jqXHR, ex) {
                console.log(jqXHR.statusText);
            },
            success: function (data) {
                $('#main-container').html(data);
                googleMaterialDesignInstall();
            }
        });
        e.preventDefault();
    });
    //==============Brands==============================
    $('.create-brand__submit-button').click(function () {
        let brandDescription = $('.text-editor').html();
        let isEmpty = $.trim($('.text-editor p:eq(0)').text()) === "";
        if (!isEmpty) {
            $('input[name="Input.Description"]').val(brandDescription);
        } else {
            $('input[name="Input.Description"]').val(null);
        }
    });
    $('.edit-brand__submit-button').click(function () {
        let brandDescription = $('.text-editor').html();
        let isEmpty = $.trim($('.text-editor p:eq(0)').text()) === "";
        if (!isEmpty) {
            $('input[name="Description"]').val(brandDescription);
        } else {
            $('input[name="Description"]').val(null);
        }
    });
    //==============Categories==========================
    //----Category List----

    $('.action-items-container, .action-items-container--mobile').click(function (e) {

        e.stopPropagation();
    });

    //Create Category (select parent of category)
    try {
        const MDCParentOfCategory = mdc.select.MDCSelect;
        const parentOfCategory = new MDCParentOfCategory(document.querySelector('.parent-of-category'));
        parentOfCategory.listen('MDCSelect:change', function () {
            $('.parent-of-category').attr('data-value', parentOfCategory.value);
        });
        $('.create-category__submit-button').click(function () {
            let myVal = parentOfCategory.value;
            $('input[name="Input.ParentId"]').val(myVal);
            let categoryDescription = $('.text-editor .ql-editor:eq(0)').html();
            let isEmpty = $.trim($('.text-editor p').text()) === "";
            if (!isEmpty) {
                $('input[name="Input.Description"]').val(categoryDescription);
            } else {
                $('input[name="Input.Description"]').val(null);
            }
           
        });
    } catch (e) {
        console.log(e);
    }
    $('.edit-category-submit-button').click(function (e) {
        let parentId = $('.parent-of-category').attr('data-value');
        $('input[name="ParentId"]').val(parentId);
        let categoryDescription = $('.text-editor .ql-editor:eq(0)').html();
        let isEmpty = $.trim($('.text-editor p').text()) === "";
        if (!isEmpty) {
            $('input[name="Description"]').val(categoryDescription);
        } else {
            $('input[name="Description"]').val(null);
        }
    });
    //When category order Or brand order changed
    $(document).on('moved', '[uk-sortable]', function (e) {
        let myIds = $('.uk-sortable-item');
        let list = [];
        myIds.each(function () {
            let id = $(this).attr('data-my-id');
            list.push(id);
            
        });
        let myurl = $(this).attr('data-url');
        let successMessage = $(this).attr('data-message');
        let myIdList = $.makeArray(list);
        $.ajax({
            url: myurl,
            type: "POST",
            data: {
                myIds: myIdList
            },
            traditional: true,
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }

        });
    });

    //Publish and UnPublish Category/Brand
    console.log('before event');
    $('.publish-icon').click(function (e) {
       
        let state = $(this).attr('data-state');
        let myid = $(this).attr('data-my-id');
        console.log(myid);
        let myUrl = $(this).attr('data-url');
        let successMessage = $(this).attr('data-message');
        let changedUrl = $(this).attr('data-changed-url');
        let changedSuccessMessage = $(this).attr('data-changed-message');
        if (state === "publish") {
            $.ajax({
                url: myUrl,
                type: "POST",
                data: {
                    id: myid
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        let elems = $('.publish-icon[data-my-id="' + myid + '"]');
                        $(elems).addClass('uk-text-muted')
                            .attr('data-state', 'unpublish')
                            .attr('data-url', changedUrl)
                            .attr('data-changed-url', myUrl)
                            .attr('data-message', changedSuccessMessage)
                            .attr('data-changed-message', successMessage);
                        $(elems).find('i').html('visibility_off');
                        $(elems).attr('uk-tooltip', 'title: publish');
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }

            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        } else {
            $.ajax({
                url: myUrl,
                type: "POST",
                data: {
                    id: myid
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        let elems = $('.publish-icon[data-my-id="' + myid + '"]');
                        $(elems).removeClass('uk-text-muted')
                            .attr('data-state', 'publish')
                            .attr('data-url', changedUrl)
                            .attr('data-changed-url', myUrl)
                            .attr('data-message', changedSuccessMessage)
                            .attr('data-changed-message', successMessage);
                        $(elems).find('i').html('visibility');
                        $(elems).attr('uk-tooltip', 'title: unpublish');
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }

            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        }

        e.stopPropagation();
        e.preventDefault();
    });
   

    //Delete Category
    $(document).on('click', '.delete-icon', function (e) {
        
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#remove-item-form-on-modal .item-id-container').html(input);
        UIkit.modal('#item-remove-modal').show();
        e.preventDefault();
        e.stopPropagation();
    });
    $('.delete-icon').click(function (e) {
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#remove-item-form-on-modal .item-id-container').html(input);
        UIkit.modal('#item-remove-modal').show();
        e.preventDefault();
        e.stopPropagation();
    });

    //mobile delete icon
    $(document).on('click', '.mobile-delete-icon', function (e) {

        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="mobileid" id="mobileid" value="' + myId + '" />';
        $('#remove-mobile-item-form-on-modal .item-id-container').html(input);
        UIkit.modal('#item-mobile-remove-modal').show();

    });
    //==============Products============================

    try {
        const MDCCategoryTitle = mdc.select.MDCSelect;
        const categoryTitle = new MDCCategoryTitle(document.querySelector('.product-category-title'));

        const MDCBrandTitle = mdc.select.MDCSelect;
        const brandTitle = new MDCBrandTitle(document.querySelector('.product-brand-title'));
        $('.product-totalInformation__submit-button').click(function () {
            let myCat = categoryTitle.value;
            $('input[name="CategoryId"]').val(myCat);
            let myBrand = brandTitle.value;
            $('input[name="BrandId"]').val(myBrand);
        });
    } catch (e) {
        console.log(e);
    }

    //------Product Pagination--------------
    //when click on pagination buttons on product list page
    $(document).on('click','#product-list__pageination-buttons-container .pagination--button',function (e) {
        let myIndex = $(this).attr('data-page-index');
        let sortType = $('#product-list__pageination-buttons-container').attr('data-sortType');
        let catId = $('#product-list__pageination-buttons-container').attr('data-catId');
        let text = $('#product-list__pageination-buttons-container').attr('data-searchText');
        $.ajax({
            url: "/En/Admin/Products/ProductList",
            type: "POST",
            data: {
                pageIndex: myIndex,
                sortType: sortType,
                catId: catId,
                text: text
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#datatable-product-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });

    

    //------Add new Product------------
    //Validate form of add product:
    try {
        const MDCCategoryOfProduct = mdc.select.MDCSelect;
        const categoryOfProduct = new MDCCategoryOfProduct(document.querySelector('.category-of-product'));
        $('#product-add-modal__form button[type="submit"]').click(function (e) {
            let catId = categoryOfProduct.value;
            if (catId === -1 || catId === "-1") {
                e.stopPropagation();
                e.preventDefault();
                $('.error-select-category').removeClass('d-none')
                    .html('please select a category');
                return false;
            }
            $('input[name="CategoryId"]').val(catId);
            let myTitle = $('#product-add-modal__form input[name="Title"]');
            if ($(myTitle).val().trim() === "" || $(myTitle).val() === null) {
                $(this).addClass('uk-disabled');
                $(myTitle).css('border-color', 'red');
                $('#product-add-modal__form span.error-title')
                    .html('please enter the product name')
                    .removeClass('d-none');
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
        });
    } catch (e) {
        console.log(e);
    }


    //remove error validation when focus on title input
    $('#product-add-modal__form input[name="Title"]').focus(function () {
        $('#product-add-modal__form button[type="submit"]').removeClass('uk-disabled');
        $(this).css('border-color', 'unset');
        $('#product-add-modal__form span.error-title').html("").addClass('d-none');
    });
    //------Select All Product From the Table-------
    $(document).on('change', 'input[name="main-product-checkbox"]', function (e) {
        let isChecked = $(this).is(':checked');
        if (isChecked) {
            $('.product-checkbox').prop('checked', true);
        } else {
            $('.product-checkbox').prop('checked', false);
        }
        if ($('.product-checkbox:checked').length > 0) {
            $('#all-product-actions-control').addClass('active');
        } else {
            $('#all-product-actions-control').removeClass('active');
        }
    });
    $(document).on('change', 'input.product-checkbox', function (e) {
        if ($('.product-checkbox:checked').length > 0) {
            $('#all-product-actions-control').addClass('active');
        } else {
            $('#all-product-actions-control').removeClass('active');
        }
    });

    //-----------Delete a group of products in product list page-----------
    $(document).on('click', '.group-delete-icon', function (e) {
        let selectedProducts = $('.product-checkbox:checked');
        let idContainer = $('#group-remove-modal .group-id-container');

        selectedProducts.each(function () {
            let id = $(this).attr('data-my-id');
            $(idContainer).append('<input name="ids" type="hidden" value="' + id + '" />');
        });
        UIkit.modal('#group-remove-modal').show();
        e.stopPropagation();
        e.preventDefault();
    });

//---Publish a group of products in product list page-----------
    $(document).on('click', '.group-publish-icon', function (e) {

        let selectedProducts = $('.product-checkbox:checked');
        let myIds = [];
        selectedProducts.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/Products/PublishGroupProduct",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {

                        let targetProducts = $('.publish-icon[data-my-id="' + item + '"]');
                        $(targetProducts).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Products Published</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 3000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });
   
    //---------UnPublish a group of products in product list page-----------
    $(document).on('click', '.group-unpublish-icon', function (e) {
        let selectedProducts = $('.product-checkbox:checked');
        let myIds = [];
        selectedProducts.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/Products/UnPublishGroupProduct",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetProducts = $('.publish-icon[data-my-id="' + item + '"]');
                        $(targetProducts).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility_off</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Product Unpublished</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 3000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });
   
    //Change Alt of images
    $('.alt-form-button').click(function (e) {
        let myform = $(this).parents('.alt-form');
        let altInput = $(myform).find('input[name="altText"]');
        let myId = $(altInput).attr('data-imageId');
        let mytext = $(altInput).val();
        $.ajax({
            url: "/Admin/Products/ImageAltText",
            type: "POST",
            data: {
                imageId: myId,
                altText: mytext
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Alt Edited</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });


    //Change Features:
    //1- Free Delivery
    $('#HasFreeDelivery').change(function () {
        let isChecked = $(this).is(':checked');
        let productId = $(this).attr('data-productId');
        let result = false;
        let successMessage = "Item Changed";
        let errorMessage = "Error";
        if (isChecked) {
            $(this).prop('checked', true);
            result = true;
            successMessage = "Item Changed";
        } else {
            $(this).prop('checkec', false);
            result = false;
            successMessage = "Item Changed";
        }

        $.ajax({
            url: "/Admin/Products/EditHasFreeDelivery",
            type: "POST",
            data: {
                id: productId,
                result: result
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + errorMessage + '</span>',
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

    //Change Features
    //2- Authority Guarantee
    $('#AuthotityGuarantee').change(function () {
        let isChecked = $(this).is(':checked');
        let productId = $(this).attr('data-productId');
        let result = false;
        let successMessage = "Item Changed";
        let errorMessage = "Error";
        if (isChecked) {
            $(this).prop('checked', true);
            result = true;
            successMessage = "Item Changed";
        } else {
            $(this).prop('checkec', false);
            result = false;
            successMessage = "Error";
        }

        $.ajax({
            url: "/Admin/Products/EditAuthotityGuarantee",
            type: "POST",
            data: {
                id: productId,
                result: result
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + errorMessage + '</span>',
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

    //Change Features
    //3- Local Payment
    $('#LocalPayment').change(function () {
        let isChecked = $(this).is(':checked');
        let productId = $(this).attr('data-productId');
        let result = false;
        let successMessage = "Item Changed";
        let errorMessage = "Error";
        if (isChecked) {
            $(this).prop('checked', true);
            result = true;
            successMessage = "Item Changed";
        } else {
            $(this).prop('checkec', false);
            result = false;
            successMessage = "Item Changed";
        }

        $.ajax({
            url: "/Admin/Products/EditLocalPayment",
            type: "POST",
            data: {
                id: productId,
                result: result
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + errorMessage + '</span>',
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

    //Change Features
    //4- Return Money
    $('#ReturnMonyGuarantee').change(function () {
        let isChecked = $(this).is(':checked');
        let productId = $(this).attr('data-productId');
        let result = false;
        let successMessage = "Item Changed";
        let errorMessage = "Error";
        if (isChecked) {
            $(this).prop('checked', true);
            result = true;
            successMessage = "Item Changed";
        } else {
            $(this).prop('checkec', false);
            result = false;
            successMessage = "Error";
        }

        $.ajax({
            url: "/Admin/Products/EditReturnMonyGuarantee",
            type: "POST",
            data: {
                id: productId,
                result: result
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + errorMessage + '</span>',
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
    //==============Product search and sort in product list (data table page)===========
    try {
        const MDCProductSort = mdc.select.MDCSelect;
        const productSort = new MDCProductSort(document.querySelector('.product-sort'));
        const MDCCategorySort = mdc.select.MDCSelect;
        const categorySort = new MDCCategorySort(document.querySelector('.category-sort'));
        
        //----Search product title or category name of product-------
        $('input[name="productSearch"]').keyup(function (e) {
            productSort.selectedIndex = 0;
            categorySort.selectedIndex = 0;
            let myval = $(this).val();
            $('#product-list__pageination-buttons-container').attr('data-sortType', 0);
            $('#product-list__pageination-buttons-container').attr('data-catId', -1);
            $('#product-list__pageination-buttons-container').attr('data-searchText', myval);
            $.ajax({
                url: "/Admin/Products/ProductList",
                type: "POST",
                data: {
                    text: myval
                },

                success: function (data) {

                    $('#datatable-product-list-container').html(data);
                }
            });
            e.stopPropagation();
            e.preventDefault();
        });

        //------Sort products based on create date, top sell, and most view-----

        productSort.listen('MDCSelect:change', function () {
            let myVal = productSort.value;
            let catId = categorySort.value;
            $('input[name="productSearch"]').val('');
            $('#product-list__pageination-buttons-container').attr('data-sortType', myVal);
            $('#product-list__pageination-buttons-container').attr('data-catId', catId);
            $('#product-list__pageination-buttons-container').attr('data-searchText', "");
            $.ajax({
                url: "/Admin/Products/ProductList",
                type: "POST",
                data: {
                    sortType: myVal,
                    catId: catId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    $('#datatable-product-list-container').html(data);
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });


        //------Filter products by category-------------

        categorySort.listen('MDCSelect:change', function () {
            
            let sorttype = productSort.value;
            let catId = categorySort.value;
            $('#product-list__pageination-buttons-container').attr('data-sortType', sorttype);
            $('#product-list__pageination-buttons-container').attr('data-catId', catId);
            $('#product-list__pageination-buttons-container').attr('data-searchText', "");
            $.ajax({
                url: "/En/Admin/Products/ProductList",
                type: "POST",
                data: {
                    sortType: sorttype,
                    catId: catId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    $('#datatable-product-list-container').html(data);
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }
   
    //==============Product Related Category========
    //Add new related category for this product
    try {
        const MDCRelatedCategory = mdc.select.MDCSelect;
        const relatedCategory = new MDCRelatedCategory(document.querySelector('.all-categories'));
        
        $(document).on('click', '.relatedCategory__submit-button', function (e) {
            
            e.preventDefault();
            e.stopPropagation();
            //get product id
            let productId = $(this).attr('data-productId');
            //get category id
            let selectedCategoryId = relatedCategory.value;

            $.ajax({
                url: "/En/Admin/RelatedCategory/AddToRelatedCategory",
                type: "POST",
                data: {
                    productId: productId,
                    categoryId: selectedCategoryId
                },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        $('#related-categories-container').append(data);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Category Added</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 4000
                        });
                    } else {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 4000
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

    //Delete related category
    $(document).on('click', '.delete-related-category-icon', function (e) {
        let thisElem = $(this).parents('.added-related-category-chip');
        //get category id
        let catgoryId = $(this).attr('data-category-id');
        //get product id 
        let productId = $(this).attr('data-product-id');
        $.ajax({
            url: "/Admin/RelatedCategory/DeleteRelatedCategory",
            type: "POST",
            data: {
                productId: productId,
                categoryId: catgoryId
            },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(thisElem).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Category Deleted</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 4000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 4000
                    });
                }

            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //==============Product Description===========
    $(document).on('click','.product-description-form__submit-button',function (e) {
        e.stopPropagation();
        e.preventDefault();
        let parent = $(this).parents('.product-description-form');
        let text = $(parent).find('#summary-description .ql-editor').html();
        let myId = $(parent).attr('data-productId');
        $.ajax({
            url: "/Admin/Products/ProductDescription",
            type: "POST",
            data: {
                id: myId,
                text: text
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
    });
    //==============Product Options===========================
    //add new option
    $(document).on('click', '.product-option-form__submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let optionTitle = $('input[name="proOptionTitle"]').val();
        let optionValue = $('input[name="proOptionValue"]').val();
        if (optionTitle === null || $.trim(optionTitle) === "") {
            $('#productOption-title-error').removeClass('d-none')
                .html('please fill this field');
            return false;
        }
        if (optionValue === null || $.trim(optionValue) === "") {
            $('#productOption-value-error').removeClass('d-none')
                .html('please fill this field');
            return false;
        }
        let productId = $(this).attr('data-productId');
        $.ajax({
            url: "/En/Admin/ProductOption/AddProductOption",
            type: "POST",
            data: {
                productId: productId,
                title: optionTitle,
                value: optionValue
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('.product-options-table__body').append(data);
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Added</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //reset error of title and value 
    $('input[name="proOptionTitle"]').focus(function (e) {
        $('#productOption-title-error').addClass('d-none')
            .html('');
    });
    $('input[name="proOptionValue"]').focus(function (e) {
        $('#productOption-value-error').addClass('d-none')
            .html('');
    });

    //delete a product option
    $(document).on('click', '.delete-productOptions', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let productOptionId = $(this).attr('data-my-id');
        let trParent = $(this).parents('tr');
        $.ajax({
            url: "/Admin/ProductOption/DeleteProductOption",
            type: "POST",
            data: {
                productOptionId: productOptionId
            },

            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    trParent.remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
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

    //==============Product Technical Content=================
    //Delete Content
    $(document).on('click','.delete-content-icon',function (e) {
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#remove-content-form-on-modal .item-id-container').html(input);
        UIkit.modal('#content-remove-modal').show();
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit Text
    $(document).on('click','.product-content-text__submit-button',function (e) {
        let parent = $(this).parents('li.content-list__list-item');
        let myId = $(this).attr('data-my-id');
        let myText = $(parent).find('.ql-editor').html();
        $.ajax({
            url: "/Admin/ProductTechnicalContent/EditText",
            type: "POST",
            data: {
                id: myId,
                text: myText
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit image (edit alt text)
    $(document).on('click','.product-content-image__submit-button',function () {
        let parent = $(this).parents('li.content-list__list-item');
        let myId = $(this).attr('data-my-id');
        let myAltText = $(parent).find('input[name="imageAlt"]').val();
        $.ajax({
            url: "/Admin/ProductTechnicalContent/EditImageAltText",
            type: "POST",
            data: {
                id: myId,
                altText: myAltText
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
    });
  
    //==============Product Tags====================
    //Add new product tag
    $(document).on('click','.product-tag-form__submit-button',function (e) {
        let parent = $(this).parents('.product-tag-form');
        let myProductId = $(this).attr('data-pro-id');
        let myText = $(parent).find('input[name="text"]').val();
        if (myText === null) {
            $(parent).find('.error-tag-title').removeClass('d-none')
                .html('please fill this field');
            $(parent).find('input[name="text"]').css('border-color', 'red');
            return false;
        } else if (myText.trim() === "") {
            $(parent).find('.error-tag-title').removeClass('d-none')
                .html('please fill this field');
            $(parent).find('input[name="text"]').css('border-color', 'red');
            return false;
        }
        $.ajax({
            url: "/En/Admin/ProductTag/CreateTag",
            type: "POST",
            data: {
                id: myProductId,
                text: myText
            },
            success: function (data) {
                $('#product-tag-list-container').html(data);
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Added</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            }
        });
        e.preventDefault();
        e.stopPropagation();
    });
    //reset error of input type text (title tag) when focus on it
    $('.product-tag-form input[name="text"]').focus(function (e) {
        let parent = $(this).parents('.product-tag-form');
        $(this).css('border-color', 'unset');
        $(parent).find('.error-tag-title').addClass('d-none').html("");
    });

    //Delete a product tag
    $(document).on('click','.delete-tag-icon',function (e) {
        let myId = $(this).attr('data-my-id');
        let parent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/ProductTag/DeleteTag",
            type: "POST",
            data: {
                id: myId
            },
            success: function (data) {
                if (data) {
                    $(parent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }

        });
        e.preventDefault();
        e.stopPropagation();
    });

    //==============Product SEO Setting===========
    $(document).on('click','.seo-setting-form__submit-button',function (e) {
        let parent = $(this).parents('.seo-setting-form');
        let myId = $(this).attr('data-pro-id');
        let title = $(parent).find('input[name="TitlePage"]').val();
        let metaDes = $(parent).find('textarea[name="MetaDescription"]').val();
        let redirectURL = $(parent).find('input[name="RedirectURL"]').val();
        $.ajax({
            url: "/Admin/Products/SEO",
            type: "POST",
            data: {
                id: myId,
                titlePage: title,
                metaDescription: metaDes,
                redirectURL: redirectURL
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }

        });
    });

    //==============Product Publishment===========
    $('input.product-publishment').change(function (e) {

        let state = $(this).is(':checked');
        let publishment = false;
        let proId = $(this).attr('data-pro-id');
        let message = "";
        if (state) {
            $(this).prop('checked', true);
            publishment = true;
            message = "Product Published";
        } else {
            $(this).prop('checked', false);
            publishment = false;
            message = "Product Unpublished";
        }

        $.ajax({
            url: "/Admin/Products/Publishment",
            type: "POST",
            data: {
                id: proId,
                publish: publishment
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + message + '</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
    });
    //==============Product Variation=============
    try {
        const MDCProductVariationHasDifPrice = mdc.select.MDCSelect;
        const productVariationHasDifPrice = new MDCProductVariationHasDifPrice(document.querySelector('.product-variation-hasDifPrice'));
        $(document).on('click','.product-variation-form__submit-button',function (e) {
            let myvarTitle = $('#product-variation-form input[name="varTitle"]').val().trim();
            if (myvarTitle === "") {
                $('#var-title-error').removeClass('d-none').html('please fill this field');
                $('#product-variation-form input[name="varTitle"]').css('border-color', 'red');
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
            let proId = $('#product-variation-form').attr('data-productId');
            let hasDifferentPrice = productVariationHasDifPrice.value;
            let successMessage = "Item Added";
            let errorMessage = "Error";
            $.ajax({
                url: "/En/Admin/ProductVariation/CreateProductVariation",
                type: "POST",
                data: {
                    productId: proId,
                    varTitle: myvarTitle,
                    hasDifPrice: hasDifferentPrice
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data === "False") {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + errorMessage + '</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 3000
                        });
                    } else {
                        $('#created-variations-list').html(data);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">' + successMessage + '</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 3000
                        });
                    }
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
            e.preventDefault();
            e.stopPropagation();
        });
        //reset error var title when focus on it
        $('#product-variation-form input[name="varTitle"]').focus(function () {
            $(this).css('border-color', 'unset');
            $('#var-title-error').addClass('d-none').html("");
        });
    } catch (e) {
        console.log(e);
    }

    //Delete Product Variation
    $(document).on('click','.delete-product-variation',function (e) {
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#product-variation-remove-modal .item-id-container').html(input);
        UIkit.modal('#product-variation-remove-modal').show();
        e.stopPropagation();
        e.preventDefault();
    });

    //==============SubProductVariation============
    //Delete subproduct variation
    $(document).on('click','.delete-subProductVariation',function (e) {
        let myId = $(this).attr('data-my-id');
        let proVarId = $(this).attr('data-pro-var-id');
        let parent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/SubProductVariation/Delete",
            type: "POST",
            data: {
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(parent).remove();
                    $('.provar-table-row[data-my-id="' + myId + '"]').remove();
                    let selectBox = $('.sub-product-variation-price-select[data-my-id="' + proVarId + '"]');
                    $(selectBox).find('option[value="' + myId + '"]').remove();
                    if ($(selectBox).find('option').length < 1) {
                        $(selectBox).parents('.set-price-for-subProductVariation').remove();
                    }

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 3000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });

    //Add new subproduct variation

    $('.sub-product-variation-form__submit-button').click(function (e) {
        let subProductVariationForm = $(this).parents('.sub-product-variation-form');
        let title = $(subProductVariationForm).find('input[name="subProVarTitle"]').val();

        if (title.trim() === "" || title === null) {
            $(subProductVariationForm).find('.error-subprovar').removeClass('d-none').html('please fill this field');
            return false;
        }
        let provarId = $(subProductVariationForm).attr('data-productVariationId');
        $.ajax({
            url: "/En/Admin/SubProductVariation/Create",
            type: "POST",
            data: {
                productVariationId: provarId,
                proVarTitle: title
            },
          
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {

                $('.sub-product-variation-container[data-my-id="' + provarId + '"]').html(data);
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Added</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 3000
                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.preventDefault();
        e.stopPropagation();
        return false;
    });

    //Define different price for subproduct variation
    try {
        $(document).on('click','.set-price-for-subProductVariation__Submit-button',function (e) {
            e.stopPropagation();
            e.preventDefault();
            let parent = $(this).parents('.set-price-for-subProductVariation');
            let provarid = $(parent).attr('data-id');
            let mySubProId = $(parent).find('.sub-product-variation-price-select').val();
            let price = $(parent).find('input[name="subproduct-var-price"]').val();
            if (price === null || price.trim() === "") {
                $(parent).find('.error-subProductVar-price').removeClass('d-none').html('please fill the price field');
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            $.ajax({
                url: "/En/Admin/SubProductionVariation/DefinePrice",
                type: "POST",
                data: {
                    id: mySubProId,
                    price: price,
                    productVariationId: provarid
                },
                success: function (data) {
                    $(parent).find('.subProductVariation-listPrice-container').html(data);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                    e.stopPropagation();
                    e.preventDefault();
                }
            });
        });
    } catch (e) {
        console.log(e);
    }

    $('input[name="subproduct-var-price"]').focus(function () {
        let parent = $(this).parents('.set-price-for-subProductVariation');
        let error = $(parent).find('.error-subProductVar-price').addClass('d-none').html("");
    });

    //Delete Price of SubProductVariation
    $(document).on('click','.delete-subProductVariationPrice',function (e) {
        e.preventDefault();
        e.stopPropagation();
        let myId = $(this).attr('data-my-id');
        let parent = $(this).parents('.provar-table-row');
        $.ajax({
            url: "/Admin/SubProductVariation/DeletePrice",
            type: "POST",
            data: {
                id: myId
            },
            success: function (data) {
                if (data) {
                    $(parent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }

        });
    });

    //==============Delivery Page==============
    //Create events => on can sending to all city be false and on true
    let onCanSendToAllCityFalse = jQuery.Event('onCanSendToAllCityFalse');
    $('input[name="CanSendingToAllCity"]').on('onCanSendToAllCityFalse', function (e) {

        $.ajax({
            url: "/Admin/Delivery/CityTableOfDeliveryZone",
            type: "POST",
            error: function (xhr) {
                alert(xhr.responseText);
            },

            success: function (data) {

                $('#cities-of-deliver-zone').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //Edit Can Send To All City:
    $('input[name="CanSendingToAllCity"]').change(function () {
        let val = $(this).val();
        $.ajax({
            url: "/Admin/Delivery/EditCanSendToAllCity",
            type: "POST",
            data: {
                canSendToAllCity: val
            }, beforeSend: function () {
                $('#progress-loader').removeClass('d-none');
            },
            success: function (data) {
                if (data) {
                    if (val === "true") {
                        $('#cities-of-deliver-zone > div').remove();
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                        $('#progress-loader').addClass('d-none');
                    } else {
                        $('input[name="CanSendingToAllCity"]').trigger(onCanSendToAllCityFalse);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }

                }
            }
        });
    });

    //state and city list
    //change city list select box when state changed
    try {
        const MDCStateOfDeliveryZone = mdc.select.MDCSelect;
        const stateOfDeliveryZone = new MDCStateOfDeliveryZone(document.querySelector('.state-of-delivery-zone'));
        const MDCCityOfDeliveryZone = mdc.select.MDCSelect;
        const cityOfDeliveryZone = new MDCCityOfDeliveryZone(document.querySelector('.city-of-delivery-zone'));
        stateOfDeliveryZone.listen('MDCSelect:change', function () {
            let val = stateOfDeliveryZone.value;
            let parent = $('.state-of-delivery-zone').parents('.city-state--container');
            $.ajax({
                url: "/En/Admin/Delivery/CityList",
                type: "POST",
                data: {
                    stateId: val
                },
                success: function (data) {
                    $(parent).find('.city-of-delivery-zone__options').html(data);


                }
            }).done(function () {
                let firstCityListText = $('.city-of-delivery-zone li').first().html();
                $('.city-of-delivery-zone .mdc-select__selected-text').html(firstCityListText);
                $('.city-of-delivery-zone .mdc-list-item').on('click', function () {
                    $('.city-of-delivery-zone .mdc-list-item').removeClass('mdc-list-item--selected');
                    $(this).addClass('mdc-list-item--selected');
                });
            });
        });

        //Add new city to delivery zone
        $('.add-city-to-delivery-zone').click(function (e) {

            e.stopPropagation();
            e.preventDefault();
            let val = cityOfDeliveryZone.value;
            let myState = stateOfDeliveryZone.value;

            $.ajax({
                url: "/En/Admin/Deliver/AddCityToDeliverZone",
                type: "POST",
                data: {
                    cityId: val,
                    stateId: myState

                },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');
                },
                success: function (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Added</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                    $('#city-table-container').html(data);
                }
            }).done(function () { $('#progress-loader').addClass('d-none'); });
        });

    } catch (e) {
        console.log(e);
    }

    //state and city list in set price for each delivery zone
    //change city list select box when state changed
    try {
        const MDCStateOfDeliverySetPrice = mdc.select.MDCSelect;
        const stateOfDeliverySetPrice = new MDCStateOfDeliverySetPrice(document.querySelector('.state-of-delivery-set-price'));
        const MDCCityOfDeliverySetPrice = mdc.select.MDCSelect;
        const cityOfDeliverySetPrice = new MDCCityOfDeliverySetPrice(document.querySelector('.city-of-delivery-set-price'));
        stateOfDeliverySetPrice.listen('MDCSelect:change', function () {
            let val = stateOfDeliverySetPrice.value;
            let parent = $('.state-of-delivery-set-price').parents('.city-state--container');
            $.ajax({
                url: "/En/Admin/Delivery/CityList",
                type: "POST",
                data: {
                    stateId: val
                },
                success: function (data) {
                    $(parent).find('.city-of-delivery-set-price__options').html(data);

                }
            });
        });

        //add delivery price for a city
        $(document).on('click', '.add-city-delivery-price--button', function (e) {
            e.stopPropagation();
            e.preventDefault();
            let parent = $(this).parents('.city-state--container');
            let price = $(parent).find('input[name="priceOfCityDelivery"]').val();
            if (price === null || price.trim() === "") {
                $(parent).find('.error-delivery-price').removeClass('d-none').html('please fill the price field');
                $(parent).find('input[name="priceOfCityDelivery"]').css('border-color', 'red');
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            let city = cityOfDeliverySetPrice.value;
            let state = stateOfDeliverySetPrice.value;
            $.ajax({
                url: "/En/Admin/Delivery/AddPriceForACity",
                type: "POST",
                data: {
                    cityId: city,
                    stateId: state,
                    price: price
                },
                beforeSend: function () {
                    $('#progress-loader').removeClass('d-none');
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {
                    $('#city-price-delivery--table').html(data);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });

    } catch (e) {
        console.log(e);
    }
    //Define event for Different Delivery Price For Each City
    let OnDifferentDeliveryPriceForEachCity = jQuery.Event('OnDifferentDeliveryPriceForEachCity');
    $('input[name="CityPriceStatus"]').on('OnDifferentDeliveryPriceForEachCity', function () {
        $.ajax({
            url: "/Admin/Delivery/OnDifferentPriceForEachCity",
            type: "POST",
            success: function (data) {
                $('#set-price-for-each-city--container').html(data);
            }
        });
    });
    //Edit city price status
    $('input[name="CityPriceStatus"]').change(function () {

        let val = $(this).val();

        $.ajax({
            url: "/Admin/Delivery/EditCityPriceStatus",
            type: "POST",
            data: {
                cityPriceStatus: val
            },
            beforeSend: function () {
                $('#progress-loader').removeClass('d-none');
            },
            success: function (data) {
                if (data) {
                    if (val === "DeifferentForEachCity") {
                        $('input[name="CityPriceStatus"]').trigger(OnDifferentDeliveryPriceForEachCity);
                    } else {
                        $('#set-price-for-each-city--container').html('');
                    }
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () { $('#progress-loader').addClass('d-none'); });
    });


    //reset error message for price of delivery
    $(document).on('focus', 'input[name="priceOfCityDelivery"]', function () {
        let parent = $(this).parents('.city-state--container');
        $(parent).find('.error-delivery-price').addClass('d-none').html('');
        $(this).css('border-color', 'unset');
    });
    //Delete price of a city delivery

    $(document).on('click', '.delete-city-price-delivery-icon', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        $.ajax({
            url: "/En/Admin/Delivery/DeletePriceCityDelivery",
            type: "POST",
            data: {
                id: myId
            }
            , beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#city-price-delivery--table').html(data);
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });

    });
    //Edit HasMinAmountForFreeDelivery option
    $('input[name="HasMinAmountForFreeDelivery"]').change(function () {
        let val = $(this).val();

        $.ajax({
            url: "/En/Admin/Delivery/EditHasMinAmountForFreeDelivery",
            type: "POST",
            data: {
                hasMinAmountForFreeDelivery: val
            },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (val === "true") {
                    $('#set-min-amount-for-free-delivery--container').html(data);
                } else {
                    $('#set-min-amount-for-free-delivery--container').html('');
                }
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //Set min amount for free delivery
    $(document).on('click', '.set-min-amount-for-free-delivery--button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let amount = $('input[name="MinAmountForFreeDelivery"]').val();
        if (amount === null || amount.trim() === "") {
            $('.error-delivery-amount').removeClass('d-none').html('please set the price');
            $('input[name="MinAmountForFreeDelivery"]').css('border-color', 'red');
            return false;
        }
        $.ajax({
            url: "/Admin/Delivery/SetMinAmountForFreeDelivery",
            type: "POST",
            data: {
                amount: amount
            },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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

    //reset error-amount for min amount for free delivery 
    $(document).on('click', 'input[name="MinAmountForFreeDelivery"]', function () {
        $('.error-delivery-amount').addClass('d-none');
        $(this).css('border-color', 'unset');
    });

    //Edit is delivery different price for each product
    $('input[name="IsFreeForBuyValue"]').change(function () {
        let val = $(this).val();
        $.ajax({
            url: "/En/Admin/Delivery/EditIsDifferentForEachProduct",
            type: "POST",
            data: {
                isDifferentForEachProduct: val
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (val === "true") {
                    $('#set-price-delivery-for-product--container').html(data);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    $('#set-price-delivery-for-product--container').html('');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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
    //==============Discount Page==============
    //Add new discount modal
    $('#discount-add-modal button[type="submit"]').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let form = $(this).parents('form');
        let code = $('input[name="Code"]').val();

        if (code === null || code.trim() === "") {
            $('.error-code').removeClass('d-none').html('please fill this field');
            $('input[name="Code"]').css('border-color', 'red');
            e.stopPropagation();
            e.preventDefault();
            return false;
        }
        let number = $('input[name="Number"]').val();
        if (number === null || number.trim() === "") {
            $('.error-number').removeClass('d-none').html('please fill this field');
            $('input[name="Number"]').css('border-color', 'red');
            e.stopPropagation();
            e.preventDefault();
            return false;
        }
        let percent = $('input[name="Percent"]').val();
        if (percent === null || percent.trim() === "" || percent.trim() === 0) {
            $('.error-percent').removeClass('d-none').html('please fill this field');
            $('input[name="Percent"]').css('border-color', 'red');
            e.stopPropagation();
            e.preventDefault();
            return false;
        }
        //Check code is unique
        $.ajax({
            url: "/Admin/Discount/CheckUniqueDiscountCode",
            type: "POST",
            data: {
                code: code
            },
            success: function (data) {
                if (!data) {
                    $('.error-code').removeClass('d-none').html('this code created before');
                    $('input[name="Code"]').css('border-color', 'red');
                    e.stopPropagation();
                    e.preventDefault();
                    return false;
                } else {
                    $(form).submit();
                }
            }
        });
    });
    //reset error-code
    $('input[name="Code"]').focus(function (e) {
        $('.error-code').addClass('d-none').html('');
        $(this).css('border-color', 'unset');
    });
    //reset error-number
    $('input[name="Number"]').focus(function (e) {
        $('.error-number').addClass('d-none').html('');
        $(this).css('border-color', 'unset');
    });
    //reset error-percent
    $('input[name="Percent"]').focus(function (e) {
        $('.error-percent').addClass('d-none').html('');
        $(this).css('border-color', 'unset');
    });

    //Edit Total Information
    $('.total-information__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let code = $('input[name="Code"]').val();
        if (code === null || code.trim() === "") {
            $('.error-code').removeClass('d-none').html('please fill this field');
            $('input[name="Code"]').css('border-color', 'red');

            return false;
        }
        let number = $('input[name="Number"]').val();
        if (number === null || number.trim() === "") {
            $('.error-number').removeClass('d-none').html('please fill this field');
            $('input[name="Number"]').css('border-color', 'red');

            return false;
        }
        let percent = $('input[name="Percent"]').val();
        if (percent === null || percent.trim() === "" || percent.trim() === 0) {
            $('.error-percent').removeClass('d-none').html('please fill this field');
            $('input[name="Percent"]').css('border-color', 'red');

            return false;
        }
        let myid = $(this).attr('data-id');
        $.ajax({
            url: "/Admin/Discount/EditTotalInformation",
            type: "POST",
            data: {
                code: code,
                number: number,
                percent: percent,
                id: myid
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
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
    //Define events for selected discount target:

    //1- event for some product
    let someProduct = jQuery.Event('onSomeProduct');
    $('input[name="DiscountTarget"]').on('onSomeProduct', function () {
        let myid = $(this).attr('data-id');
        $.ajax({
            url: "/En/Admin/Discount/ChooseProductsForDiscountTarget",
            type: "POST",
            data: {
                id: myid
            },
            error: function (xhr) { alert(xhr.responseText); },
            success: function (data) {
                $('#discount-target-details').html(data);
            }

        });
    });

    //2- event for some category
    let someCategory = jQuery.Event('onSomeCategory');
    $('input[name="DiscountTarget"]').on('onSomeCategory', function () {
        let myid = $(this).attr('data-id');
        $.ajax({
            url: "/En/Admin/Discount/ChooseCategoriesForDiscountTarget",
            type: "POST",
            data: {
                id: myid
            },
            error: function (xhr) { alert(xhr.responseText); },
            success: function (data) {
                $('#discount-target-details').html(data);
            }

        });
    });

    //3- event for some brand
    let someBrand = jQuery.Event('onSomeBrand');
    $('input[name="DiscountTarget"]').on('onSomeBrand', function () {
        let myid = $(this).attr('data-id');
        $.ajax({
            url: "/En/Admin/Discount/ChooseBrandsForDiscountTarget",
            type: "POST",
            data: {
                id: myid
            },
            error: function (xhr) { alert(xhr.responseText); },
            success: function (data) {
                $('#discount-target-details').html(data);
            }

        });
    });
    //Discount Target 
    $('input[name="DiscountTarget"]').change(function () {
        let val = $(this).val();
        let myid = $(this).attr('data-id');

        $.ajax({
            url: "/Admin/Discount/DiscountTarget",
            type: "POST",
            data: {
                discountTarget: val,
                id: myid
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (val === "SomeProducts") {

                        $('input[name="DiscountTarget"]').trigger(someProduct);
                    } else if (val === "SomeCategories") {
                        $('input[name="DiscountTarget"]').trigger(someCategory);
                    } else if (val === "SomeBrands") {
                        $('input[name="DiscountTarget"]').trigger(someBrand);
                    } else {
                        $('#discount-target-details').html("");
                    }
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //Add product of discount target (some products)
    try {
        const MDCProductDiscount = mdc.select.MDCSelect;
        const productDiscount = new MDCProductDiscount(document.querySelector('.product-of-discount'));
        $('.add-discounttarget-product__submit-button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let proId = productDiscount.value;
            let disId = $(this).attr('data-id');
            if (proId === -1) {
                $('.error-product-discount-target').removeClass('d-none').html('please select a product');

                return false;
            }
            $.ajax({
                url: "/En/Admin/Discount/AddProductDiscountTarget",
                type: "POST",
                data: {
                    productId: proId,
                    discountId: disId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        $('#discount-target__added-products').html(data);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Product Added</span>',
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

        //reset error-product-discount-target
        productDiscount.listen('MDCSelect:focus', function () {
            $('.error-product-discount-target').addClass('d-none').html('');

        });
    } catch (e) {
        console.log(e);
    }


    //Delete product of discount target
    $('.delete-discount-target__product').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let myParent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/Discount/DeleteProductDiscountTarget",
            type: "POST",
            data: {
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(myParent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
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
    //Add category of discount target (some categories)
    try {
        const MDCCategoryDiscount = mdc.select.MDCSelect;
        const categoryDiscount = new MDCCategoryDiscount(document.querySelector('.category-of-discount'));
        $('.add-discounttarget-category__submit-button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let catId = categoryDiscount.value;
            let disId = $(this).attr('data-id');
            if (catId === -1) {
                $('.error-category-discount-target').removeClass('d-none').html('please select a category');

                return false;
            }
            $.ajax({
                url: "/En/Admin/Discount/AddCategoryDiscountTarget",
                type: "POST",
                data: {
                    categoryId: catId,
                    discountId: disId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        $('#discount-target__added-categories').html(data);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Category Added</span>',
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

        //reset error-category-discount-target
        categoryDiscount.listen('MDCSelect:focus', function () {
            $('.error-category-discount-target').addClass('d-none').html('');

        });
    } catch (e) {
        console.log(e);
    }


    //Delete category of discount target
    $('.delete-discount-target__category').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let myParent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/Discount/DeleteCategoryDiscountTarget",
            type: "POST",
            data: {
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(myParent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
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
    //Add brand of discount target (some brands)
    try {
        const MDCBrandDiscount = mdc.select.MDCSelect;
        const brandDiscount = new MDCBrandDiscount(document.querySelector('.brand-of-discount'));
        $('.add-discounttarget-brand__submit-button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let brandId = brandDiscount.value;
            let disId = $(this).attr('data-id');

            if (brandId === "-1") {
                $('.error-brand-discount-target').removeClass('d-none').html('please select a brand');


                return false;
            }
            $.ajax({
                url: "/En/Admin/Discount/AddBrandDiscountTarget",
                type: "POST",
                data: {
                    brandId: brandId,
                    discountId: disId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        $('#discount-target__added-brands').html(data);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Brand Added</span>',
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

        //reset error-brand-discount-target
        brandDiscount.listen('MDCSelect:focus', function () {
            $('.error-brand-discount-target').addClass('d-none').html('');

        });
    } catch (e) {
        console.log(e);
    }

    //Delete brand of discount target
    $('.delete-discount-target__brand').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let myParent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/Discount/DeleteBrandDiscountTarget",
            type: "POST",
            data: {
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(myParent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
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
    //-----Active min value and min number condition-----------
    $('input[name="IsForMinBuyValue"]').change(function () {
        let disId = $(this).attr('data-id');
        let myval = $(this).val();
        let isChecked = $(this).is(':checked');
        if (isChecked) {
            $(this).prop('checked', true);
            myval = true;
        } else {
            $(this).prop('checked', false);
            myval = false;
        }

        $.ajax({
            url: "/Admin/Discount/ChangeIsForMinValueCondition",
            type: "POST",
            data: {
                discountId: disId,
                isForMinBuyValue: myval
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (isChecked) {
                        $('input[name="MinBuyValue"]').val('0');
                    }
                    else {
                        $('input[name="MinBuyValue"]').val('');
                    }
                    $('input[name="MinBuyValue"]').toggleClass('uk-disabled');
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    $('input[name="IsForMinBuyNumber"]').change(function () {
        let disId = $(this).attr('data-id');
        let myval = $(this).val();
        let isChecked = $(this).is(':checked');
        if (isChecked) {
            $(this).prop('checked', true);
            myval = true;
        } else {
            $(this).prop('checked', false);
            myval = false;
        }

        $.ajax({
            url: "/Admin/Discount/ChangeIsForMinBuyNumberCondition",
            type: "POST",
            data: {
                discountId: disId,
                isForMinBuyNumber: myval
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (isChecked) {
                        $('input[name="MinBuyNumber"]').val('0');
                    }
                    else {
                        $('input[name="MinBuyNumber"]').val('');
                    }
                    $('input[name="MinBuyNumber"]').toggleClass('uk-disabled');
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });


    //Add min buy value and min buy number
    $('.discount-conditions__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let disId = $(this).attr('data-id');
        let minbuyval = $('input[name="MinBuyValue"]').val();
        let minbuynumber = $('input[name="MinBuyNumber"]').val();
        $.ajax({
            url: "/Admin/Discount/EditMinValue",
            type: "POST",
            data: {
                discountId: disId,
                minBuyValue: minbuyval,
                minBuyNumber: minbuynumber
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //Edit customer of discount

    $('input[name="IsForAllCustomers"]').change(function (e) {
        let val = $(this).val();
        let disId = $(this).attr('data-id');

        $.ajax({
            url: "/En/Admin/Discount/EditCustomerDiscount",
            type: "POST",
            data: {
                isForAllCustomer: val,
                discountId: disId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (val === "true") {
                        $('#discount-customer-container').html('');
                    } else {
                        $('#discount-customer-container').html(data);
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //Add discount code for a customer
    $('.add-customer-discount__button').click(function (e) {
        let disId = $(this).attr('data-id');
        let userName = $('select[name="discountCustomer"]').val();
        $.ajax({
            url: "/En/Admin/Discount/AddDiscountCodeForCustomer",
            type: "POST",
            data: {
                discountId: disId,
                name: userName
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#discount-target__added-customers').html(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //Delete customer of discount target
    $('.delete-discount-target__customer').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let myParent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/Discount/DeleteCustomerDiscountTarget",
            type: "POST",
            data: {
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(myParent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
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

    //Set Activation and Expiration Date
    $('.discount-date__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let disId = $(this).attr('data-id');
        let activationDate = $('input[name="ActivationDate"]').val();
        let expirationDate = $('input[name="ExpirationDate"]').val();
        let from = activationDate.split("/");
        
        let to = expirationDate.split("/");
        
        if (from[0] > to[0]) {
            $('.error-discount-date')
                .removeClass('d-none')
                .html('expiration date should be after activation date');
            return false;
        } else if (from[0] === to[0] && from[1] > to[1]) {
            $('.error-discount-date')
                .removeClass('d-none')
                .html('expiration date should be after activation date');
            return false;
        } else if (from[0] === to[0] && from[1] === to[1] && from[2] > to[2]) {
            $('.error-discount-date')
                .removeClass('d-none')
                .html('expiration date should be after activation date');
            return false;
        }
        if (activationDate === null || activationDate.trim() === "") {
            $('.error-discount-date')
                .removeClass('d-none')
                .html('please set the date');
            return false;
        }
        if (expirationDate === null || expirationDate.trim() === "") {
            $('.error-discount-date')
                .removeClass('d-none')
                .html('please set the date');
            return false;
        }
        $.ajax({
            url: "/Admin/Discount/SetDiscountDate",
            type: "POST",
            data: {
                activationDate: activationDate,
                expirationDate: expirationDate,
                discountId: disId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $('.error-discount-date').addClass('d-none').html('');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Date Changed</span>',
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
    //Discount Publishment
    $('input.discount-publishment').change(function () {
        let isChecked = $(this).is(':checked');
        let val = $(this).val();
        if (isChecked) {
            $(this).prop('checked', true);
            val = true;
        } else {
            $(this).prop('checked', false);
            val = false;
        }

        let disId = $(this).attr('data-id');
        $.ajax({
            url: "/Admin/Discount/DiscountPublishment",
            type: "POST",
            data: {
                isPublished: val,
                discountId: disId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (val) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Discount Published</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    } else {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Discount Unpublished</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }

                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //------Discount Pagination--------------
    //when click on pagination buttons on discount list page
    $(document).on('click','#discount-list__pageination-buttons-container .pagination--button',function (e) {
        let myIndex = $(this).attr('data-page-index');
        let sortType = $('#discount-list__pageination-buttons-container').attr('data-sortType');
        let text = $('#discount-list__pageination-buttons-container').attr('data-searchText');
        $.ajax({
            url: "/En/Admin/Discount/DiscountList",
            type: "POST",
            data: {
                pageIndex: myIndex,
                sortType: sortType,
                text: text
            },
            success: function (data) {
                $('#datatable-discount-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });

    
    //------Select All Discount From the Table-------

    $('input[name="main-discount-checkbox"]').change(function () {
        let isChecked = $(this).is(':checked');
        if (isChecked) {
            $('.discount-checkbox').prop('checked', true);
        } else {
            $('.discount-checkbox').prop('checked', false);
        }
        if ($('.discount-checkbox:checked').length > 0) {
            $('#all-discount-actions-control').addClass('active');
        } else {
            $('#all-discount-actions-control').removeClass('active');
        }
    });

    $('input.discount-checkbox').change(function () {
        if ($('.discount-checkbox:checked').length > 0) {
            $('#all-discount-actions-control').addClass('active');
        } else {
            $('#all-discount-actions-control').removeClass('active');
        }
    });
    //-----------Delete a group of discounts in discount list page-----------
    $(document).on('click', '.group-discount-delete-icon', function (e) {
        let selectedDiscounts = $('.discount-checkbox:checked');
        let idContainer = $('#group-remove-modal .group-id-container');

        selectedDiscounts.each(function () {
            let id = $(this).attr('data-my-id');
            $(idContainer).append('<input name="ids" type="hidden" value="' + id + '" />');
        });
        UIkit.modal('#group-remove-modal').show();
        e.stopPropagation();
        e.preventDefault();
    });
   
    //---------Publish a group of discounts in discount list page-----------
    $(document).on('click', '.group-discount-publish-icon', function (e) {
        let selectedDiscounts = $('.discount-checkbox:checked');
        let myIds = [];
        selectedDiscounts.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/Discount/PublishGroupDiscount",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetDiscounts = $('.publish-icon[data-my-id="' + item + '"]');
                        $(targetDiscounts).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Discounts Published</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
   

    //---------UnPublish a group of discount in discount list page-----------
    $(document).on('click', '.group-discount-unpublish-icon', function (e) {
        let selectedDiscounts = $('.discount-checkbox:checked');
        let myIds = [];
        selectedDiscounts.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/Discount/UnPublishGroupDiscount",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetDiscounts = $('.publish-icon[data-my-id="' + item + '"]');
                        $(targetDiscounts).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility_off</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Discounts Unpublished</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
   
    //-----------------Discount Searcn And Filter--------------------


    //------Sort discounts based on activation date, active, and expired-----
    try {
        const MDCDiscountSort = mdc.select.MDCSelect;
        const discountSort = new MDCDiscountSort(document.querySelector('.discount-sort'));
        discountSort.listen('MDCSelect:change', function () {
            let myVal = discountSort.value;
            $('#discount-list__pageination-buttons-container').attr('data-sortType', myVal);
            $('#discount-list__pageination-buttons-container').attr('data-searchText',"");
            $.ajax({
                url: "/En/Admin/Discount/DiscountList",
                type: "POST",
                data: {
                    sortType: myVal
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {

                    $('#datatable-discount-list-container').html(data);
                }
            });
        });
        //----Search discount code-------
        $('input[name="discountSearch"]').keyup(function (e) {
            discountSort.selectedIndex = 0;
          
            let myval = $(this).val();
            $('#discount-list__pageination-buttons-container').attr('data-sortType', '');
            $('#discount-list__pageination-buttons-container').attr('data-searchText', myval);
            $.ajax({
                url: "/En/Admin/Discount/DiscountList",
                type: "POST",
                data: {
                    text: myval
                },
                success: function (data) {

                    $('#datatable-discount-list-container').html(data);
                }
            });
            e.stopPropagation();
            e.preventDefault();
        });
    } catch (e) {
        console.log(e);
    }

    //=============Special Discount Page===========
    //------Special Discount Pagination--------------
    //when click on pagination buttons on special discount list page
    $('#special-discount-list__pageination-buttons-container .pagination--button').click(function (e) {
        let myIndex = $(this).attr('data-page-index');
        $.ajax({
            url: "/En/Admin/SpecialDiscount/SpecialDiscountList",
            type: "POST",
            data: {
                pageIndex: myIndex
            },
            success: function (data) {
                $('#datatable-special-discount-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });

    //when click on pagination buttons on special discont sorted and filtered list page
    $('#special-discount-sortandfilter__pageination-buttons-container .pagination--button').click(function (e) {
        let myIndex = $(this).attr('data-page-index');
        let myVal = $('select[name="specialDiscountSort"]').val();

        $.ajax({
            url: "/En/Admin/Search/SpecialDiscountSortAndFilter",
            type: "POST",
            data: {
                sortType: myVal,
                pageIndex: myIndex
            },
            success: function (data) {
                $('#datatable-special-discount-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
    //when click on pagination buttons on special discount searched list page
    $('#special-discount-search__pageination-buttons-container .pagination--button').click(function (e) {
        let myIndex = $(this).attr('data-page-index');
        let myVal = $('input[name="discountSearch"]').val();
        $('select[name="specialDiscountSort"]').val('0').change();
        $('select[name="specialDiscountSort"]').siblings('button').find('.selected-text-container').html('نزدیک ترین');

        $.ajax({
            url: "/En/Admin/Search/SpecialDiscountSearchText",
            type: "POST",
            data: {
                text: myVal,
                pageIndex: myIndex
            },
            success: function (data) {
                $('#datatable-special-discount-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
    //------Select All Discount From the Table-------

    $('input[name="main-special-discount-checkbox"]').change(function () {
        let isChecked = $(this).is(':checked');
        if (isChecked) {
            $('.special-discount-checkbox').prop('checked', true);
        } else {
            $('.special-discount-checkbox').prop('checked', false);
        }
        if ($('.special-discount-checkbox:checked').length > 0) {
            $('#all-special-discount-actions-control').addClass('active');
        } else {
            $('#all-special-discount-actions-control').removeClass('active');
        }
    });

    $('input.special-discount-checkbox').change(function () {
        if ($('.special-discount-checkbox:checked').length > 0) {
            $('#all-special-discount-actions-control').addClass('active');
        } else {
            $('#all-special-discount-actions-control').removeClass('active');
        }
    });
    //-----------Delete a group of special discounts in special discount list page-----------
    $(document).on('click', '.group-special-discount-delete-icon', function (e) {
        let selectedDiscounts = $('.special-discount-checkbox:checked');
        let idContainer = $('#group-remove-modal .group-id-container');

        selectedDiscounts.each(function () {
            let id = $(this).attr('data-my-id');
            $(idContainer).append('<input name="ids" type="hidden" value="' + id + '" />');
        });
        UIkit.modal('#group-remove-modal').show();
        e.stopPropagation();
        e.preventDefault();
    });
    
    //---------Publish a group of special discounts in special discount list page-----------
    $(document).on('click', '.group-special-discount-publish-icon', function (e) {
        let selectedDiscounts = $('.special-discount-checkbox:checked');
        let myIds = [];
        selectedDiscounts.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/SpecialDiscount/PublishGroupSpecialDiscount",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetDiscounts = $('.publish-icon[data-my-id="' + item + '"]');
                        $(targetDiscounts).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Special Offers Published</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
    

    //---------UnPublish a group of special discount in special discount list page-----------
    $(document).on('click', '.group-special-discount-unpublish-icon', function (e) {
        let selectedDiscounts = $('.special-discount-checkbox:checked');
        let myIds = [];
        selectedDiscounts.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/SpecialDiscount/UnPublishGroupSpecialDiscount",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetDiscounts = $('.publish-icon[data-my-id="' + item + '"]');
                        $(targetDiscounts).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility_off</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Special Discounts Unpublished</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });
   
    //-----------------Special Discount Searcn And Filter--------------------
    //----Search special discount product title-------
    try {
        const MDCSpecialDiscountSort = mdc.select.MDCSelect;
        const specialDiscountSort = new MDCSpecialDiscountSort(document.querySelector('.special-discount-sort'));
        $('input[name="specialDiscountSearch"]').keyup(function (e) {
            specialDiscountSort.selectedIndex = 0;
            let myval = $(this).val();
            $.ajax({
                url: "/En/Admin/Search/SpecialDiscountSearchText",
                type: "POST",
                data: {
                    text: myval
                },
                success: function (data) {

                    $('#datatable-special-discount-list-container').html(data);
                }
            });
            e.stopPropagation();
            e.preventDefault();
        });

        //------Sort special discounts based on activation date, active, and expired-----
        specialDiscountSort.listen('MDCSelect:change', function () {
            let myVal = specialDiscountSort.value;

            $.ajax({
                url: "/En/Admin/Search/SpecialDiscountSortAndFilter",
                type: "POST",
                data: {
                    sortType: myVal
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {

                    $('#datatable-special-discount-list-container').html(data);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }

    //edit special discount
    try {
        const MDCSpecialDiscountProduct = mdc.select.MDCSelect;
        const specialDiscountProduct = new MDCSpecialDiscountProduct(document.querySelector('.product-of-special-discount'));
        $('.special-discount-edit__submit-button').click(function () {
            let myVal = specialDiscountProduct.value;
            $('input[name="ProductId"]').val(myVal);
        });
    } catch (e) {
        console.log(e);
    }

    //============Customer Page================
    $(document).on('click', '.customer-publishment-icon', function (e) {

        e.stopPropagation();
        e.preventDefault();
        let elem = $(this);
        let status = $(this).attr('data-status');
        if (status.toLowerCase() === "true") {
            status = false;
        } else {
            status = true;
        }
        let id = $(this).attr('data-my-id');
        console.log(id);
        $.ajax({
            url: "/Admin/Customer/EditCustomerStatus",
            type: "POST",
            data: {
                id: id,
                status: status
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (status) {
                        $(elem).attr('data-status', true);
                        $(elem).find('i').html('visibility');
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Account Became Active</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    } else {
                        $(elem).attr('data-status', false);
                        $(elem).find('i').html('visibility_off');
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Account Became Inactive</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    
    $('.customer-totalInformation__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let fName = $('input[name="FullName"]').val();
        let mobile = $('input[name="Mobile"]').val();
        let mail = $('input[name="Email"]').val();
        let cardNumber = $('input[name="CartBankNumber"]').val();
        if (fName === null || fName.trim() === "") {
            $('.error-total-information__fullname').removeClass('d-none')
                .html('please fill the name');
            $('input[name="FullName"]').css('border-color', 'red');
            return false;
        }
        else if (mobile === null || mobile.trim() === "") {
            $('.error-total-information__mobile').removeClass('d-none')
                .html('please fill the mobile number');
            $('input[name="Mobile"]').css('border-color', 'red');
            return false;
        }
        $.ajax({
            url: "/Admin/Customer/EditTotalInformation",
            type: "POST",
            data: {
                id: myId,
                fullName: fName,
                mobileNumber: mobile,
                email: mail,
                cardNumber: cardNumber
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data === 101) {
                    $('.error-total-information__mobile').addClass('d-none').html('');
                    $('input[name="Mobile"]').css('border-color', 'unset');

                    $('.error-total-information__fullname').addClass('d-none')
                        .html('');
                    $('input[name="FullName"]').css('border-color', 'unset');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else if (data === 102) {
                    $('input[name="Mobile"]').css('border-color', 'red');
                    $('.error-total-information__mobile').removeClass('d-none')
                        .html('this mobile number used before');
                } else if (data === 103) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
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

    //=============Edit Customer Page=============

    //Edit Customer Role
    try {
        const MDCCustomerRole = mdc.select.MDCSelect;
        const customerRole = new MDCCustomerRole(document.querySelector('.customer-role'));
        customerRole.listen('MDCSelect:change', function () {
            let val = customerRole.value;
            let userId = $('.customer-role').attr('data-id');
            $.ajax({
                url: "/Admin/Customer/EditCustomerRole",
                type: "POST",
                data: {
                    role: val,
                    id: userId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Role Changed</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
            return false;
        });

        //Edit Customer Status
        const MDCCustomerStatus = mdc.select.MDCSelect;
        const customerStatus = new MDCCustomerStatus(document.querySelector('.customer-status'));
        customerStatus.listen('MDCSelect:change', function () {
            let val = customerStatus.value;
            let userId = $('.customer-status').attr('data-id');
            $.ajax({
                url: "/Admin/Customer/EditCustomerStatus",
                type: "POST",
                data: {
                    status: val,
                    id: userId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        if (val === "true") {
                            UIkit.notification({
                                message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Account Became Active</span>',
                                status: 'primary',
                                pos: 'top-center',
                                timeout: 5000
                            });
                        } else {
                            UIkit.notification({
                                message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Account Became Inactive</span>',
                                status: 'primary',
                                pos: 'top-center',
                                timeout: 5000
                            });
                        }
                    }
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
            return false;
        });
    } catch (e) {
        console.log(e);
    }
    //============Blog Category================
    //add blog category
    $('.blog-category__submit-button').click(function (e) {
       
        let blogCategoryTitle = $('input[name="blogCategoryTitle"]').val();
        if (blogCategoryTitle === null) {
            $('.error-blog-category-title').removeClass('d-none').html('please fill the title');
            $('input[name="blogCategoryTitle"]').css('border-color', 'red');
            e.stopPropagation();
            e.preventDefault();
            return false;
        } else if (blogCategoryTitle.trim() === "") {
            $('.error-blog-category-title').removeClass('d-none').html('please fill the title');
            $('input[name="blogCategoryTitle"]').css('border-color', 'red');
            e.stopPropagation();
            e.preventDefault();
            return false;
        }
       
    });

    //reset blog category title error
    $('input[name="blogCategoryTitle"]').focus(function (e) {
        $(this).css('border-color', 'unset');
        $('.error-blog-category-title').addClass('d-none').html('');
    });

    //Delete a blog category
    $(document).on('click', '.delete-blog-category', function (e) {
        let myId = $(this).attr('data-my-id');
        $('#blogCategory-remove-form-on-modal input[name="id"]').val(myId);
    });

   
    //=============Blog Page===================
    try {
        const MDCBlogCategoryTitle = mdc.select.MDCSelect;
        const BlogCategoryTitle = new MDCBlogCategoryTitle(document.querySelector('.blog-category-title'));
        //Add new blog
        $('.add-blog__submit-button').click(function (e) {
            $('input[name="BlogCategoryId"]').val(BlogCategoryTitle.value);
            let title = $('input[name="Title"]').val();
            if (title === null) {
                e.stopPropagation();
                e.preventDefault();
                $('.error-blog-title').removeClass('d-none').html('please fill the title');
                $('input[name="Title"]').css('border-color', 'red');
                return false;
            } else if (title.trim() === "") {
                e.stopPropagation();
                e.preventDefault();
                $('.error-blog-title').removeClass('d-none').html('please fill the title');
                $('input[name="Title"]').css('border-color', 'red');
                return false;
            }
        });
    } catch(e){
        console.log(e);
    }
   
    //reset blog title error
    $('input[name="Title"]').focus(function () {
        $('.error-blog-title').addClass('d-none').html('');
        $(this).css('border-color', 'unset');
    });

    
    //publish blog in blog list page
    $('.publish-blog-icon').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let elem = $(this);
        let myicon = $(this).find('.my-icon');
        let id = $(this).attr('data-id');
        let value = $(this).attr('data-val');
        
        $.ajax({
            url: "/Admin/Blog/IsPublished",
            type: "POST",
            data: {
                id: id,
                isPublished: value
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data && (value === "true")) {
                    $(elem).attr('data-val', false);
                    $(myicon).html('visibility');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Blog Published</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else if (data && (value ==="false")) {
                    $(elem).attr('data-val', true);
                    $(myicon).html('visibility_off');
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Blog Unpublished</span>',
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

    //------Sort blogs based on date, publishment, top view, top rate-----
    try {
        const MDCBlogSort = mdc.select.MDCSelect;
        const blogSort = new MDCBlogSort(document.querySelector('.blog-sort'));
        blogSort.listen('MDCSelect:change', function () {
            let myVal = blogSort.value;

            $.ajax({
                url: "/En/Admin/Search/BlogSortAndFilter",
                type: "POST",
                data: {
                    sortType: myVal
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {

                    $('#blog-list-container').html(data);
                }
            });
        });
        //----Search blog title-------
        $('input[name="blogSearch"]').keyup(function (e) {
            blogSort.selectedIndex = 0;

            let myval = $(this).val();
            $.ajax({
                url: "/En/Admin/Search/BlogSearchText",
                type: "POST",
                data: {
                    text: myval
                },
                success: function (data) {

                    $('#blog-list-container').html(data);
                }
            });
            e.stopPropagation();
            e.preventDefault();
        });
    } catch (e) {
        console.log(e);
    }

    //============Edit Blog Page===============
    //edit blog total information
    try {
        const MDCEditBlogCategoryTitle = mdc.select.MDCSelect;
        const EditBlogCategoryTitle = new MDCEditBlogCategoryTitle(document.querySelector('.blog-category-title'));
        $('.blog-totalInformation-edit__submit-button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let myTitle = $('input[name="Title"]').val();
            let mySummaryDescription = $('textarea[name="SummaryDescription"]').val();
            let myId = $(this).attr('data-id');
            let blogCatId = EditBlogCategoryTitle.value;
            $.ajax({
                url: "/Admin/Blog/EditTotalInformation",
                type: "POST",
                data: {
                    id: myId,
                    title: myTitle,
                    summaryDescription: mySummaryDescription,
                    blogCategoryId: blogCatId
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    if (data) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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
    } catch (e) {

    }
  

    //add new blog tag
    $('.blog-tags__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let myTagTitle = $('input[name="blogTags"]').val();
        if (myTagTitle === null) {
            $('.error-blog-tag-title').removeClass('d-none').html('plesae fill the title');
            $('input[name="blogTags"]').css('border-color', 'red');
            return false;
        } else if (myTagTitle.trim() === "") {
            $('.error-blog-tag-title').removeClass('d-none').html('please fill the title');
            $('input[name="blogTags"]').css('border-color', 'red');
            return false;
        }
        $.ajax({
            url: "/En/Admin/Blog/AddNewTag",
            type: "POST",
            data: {
                id: myId,
                tagTitle: myTagTitle
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#blog-tag-list-container').html(data);
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Tag Added</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000

                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //reset blog title tag error
    $('input[name="blogTags"]').focus(function (e) {
        $(this).css('border-color', 'unset');
        $('.error-blog-tag-title').addClass('d-none').html('');
    });

    //Delete a blog tag
    $(document).on('click','.delete-blog-tag-icon',function (e) {
        let myId = $(this).attr('data-my-id');
        let parent = $(this).parents('.admin-chip-tags');
        $.ajax({
            url: "/Admin/Blog/DeleteTag",
            type: "POST",
            data: {
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(parent).remove();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Deleted</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }

        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit SEO settings
    $('.blog-seo-settings__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let title = $('input[name="TitlePage"]').val();
        let description = $('textarea[name="MetaDescription"]').val();
        let redirectURL = $('input[name="RedirectURL"]').val();
        $.ajax({
            url: "/Admin/Blog/EditSEOSetting",
            type: "POST",
            data: {
                id:myId,
                titlePage: title,
                metaDescription: description,
                redirectURL: redirectURL
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //Edit publishment
    $('.blog-ispublished').change(function (e) {
        let isChecked = $(this).is(':checked');
        let value = false;
        let myId = $(this).attr('data-id');
        if (isChecked) {
            $(this).prop('checked', true);
            value = true;
        } else {
            $(this).prop('checked', false);
            value = false;
        }
        $.ajax({
            url: "/Admin/Blog/IsPublished",
            type: "POST",
            data: {
                isPublished: value,
                id: myId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (value) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Blog Publishd</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    } else {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Blog Unpublished</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //=============Blog Content================
    //Delete Blog Content
    $('.delete-blog-content-icon').click(function (e) {
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#remove-blog-content-form-on-modal .item-id-container').html(input);
        UIkit.modal('#blog-content-remove-modal').show();
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit Text
    $('.blog-content-text__submit-button').click(function (e) {
        let parent = $(this).parents('li.blog-content-list__list-item');
        let myId = $(this).attr('data-my-id');
        let myText = $(parent).find('.ql-editor').html();
        $.ajax({
            url: "/Admin/BlogContent/EditText",
            type: "POST",
            data: {
                id: myId,
                text: myText
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit image (edit alt text)
    $('.blog-content-image__submit-button').click(function () {
        let parent = $(this).parents('li.blog-content-list__list-item');
        let myId = $(this).attr('data-my-id');
        let myAltText = $(parent).find('input[name="imageAlt"]').val();
        $.ajax({
            url: "/Admin/BlogContent/EditImageAltText",
            type: "POST",
            data: {
                id: myId,
                altText: myAltText
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
    });

    //============Home Page===================
    //Edit total information of home page
    $('.homepage-totalinformation__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myTitle = $('input[name="Title"]').val();
        $.ajax({
            url: "/Admin/HomePage/EditTotalInformation",
            type: "POST",
            data: {
                title: myTitle
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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
    //Edit Instagram API code of home page
    $('.homepage-instagramapi__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myInstagramAPI = $('input[name="InstagramAPI"]').val();
      
        $.ajax({
            url: "/Admin/HomePage/EditInstagramAPI",
            type: "POST",
            data: {
                instagramAPI: myInstagramAPI
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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
    //Edit Website Features
    $('input[name="Has24Support"],input[name="HasFastDeliveryOption"],input[name="HasOriginalWarranty"],input[name="HasLocalPaymentOption"]').change(function (e) {
       
        let has24support = $('input[name="Has24Support"]').is(':checked');
       
        let hasfastdelivery = $('input[name="HasFastDeliveryOption"]').is(':checked');
        let hasoriginalwarranty = $('input[name="HasOriginalWarranty"]').is(':checked');
        let haslocalpayment = $('input[name="HasLocalPaymentOption"]').is(':checked');
       
        $.ajax({
            url: "/Admin/HomePage/EditFeatures",
            type: "POST",
            data: {
                has24Support: has24support,
                hasFastDelivery: hasfastdelivery,
                hasLocalPaymentOption: haslocalpayment,
                hasOriginalWarranty: hasoriginalwarranty
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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

    //Edit home description
    $('.edit-home-description__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let description = $('#home-description .ql-editor').html();
        $.ajax({
            url: "/Admin/HomePage/EditHomeDescription",
            type: "POST",
            data: {
                description: description
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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

    //Edit footer description
    $('.edit-footer-description__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let description = $('#footer-description .ql-editor').html();
        $.ajax({
            url: "/Admin/HomePage/EditFooterDescription",
            type: "POST",
            data: {
                description: description
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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
    //SEO Setting
    $('.homepage-seosetting__submit-button').click(function (e) {
        let myTitle = $('input[name="TitlePage"]').val();
        let myMetaDescription = $('textarea[name="MetaDescription"]').val();
        let redirectURL = $('input[name="RedirectURL"]').val();
        $.ajax({
            url: "/Admin/HomePage/SEO",
            type: "POST",
            data: {
                titlePage: myTitle,
                metaDescription: myMetaDescription,
                redirectURL: redirectURL
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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

   

    //============Contact Us Page==============
    //Edit Total Information
    $('.contactus-totalInformation__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let email = $('input[name="Email"]').val();
        let phone = $('input[name="Phone"]').val();
        let mobile = $('input[name="Mobile"]').val();
        let whatsapp = $('input[name="WhatsAppNumber"]').val();
        let address = $('textarea[name="Address"]').val();
        let googleMap = $('input[name="GoogleMap"]').val();
        let googleMapLink = $('input[name="GoogleMapLink"]').val();
        $.ajax({
            url: "/Admin/ContactUs/EditTotalInformation",
            type: "POST",
            data: {
                email: email,
                phone: phone,
                mobile: mobile,
                address: address,
                whatsapp: whatsapp,
                googleMap: googleMap,
                googleMapLink: googleMapLink
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //Edit social networks
    $('.contactus-social__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let instagram = $('input[name="Instagram"]').val();
        let facebook = $('input[name="Facebook"]').val();
        let aparat = $('input[name="Aparat"]').val();
        let youtube = $('input[name="Youtube"]').val();
        let twitter = $('input[name="Twitter"]').val();
        let telegram = $('input[name="Telegram"]').val();
        $.ajax({
            url: "/Admin/ContactUs/SocialNetworks",
            type: "POST",
            data: {
                instagram: instagram,
                twitter: twitter,
                telegram: telegram,
                facebook: facebook,
                youtube: youtube,
                aparat: aparat
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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

    //Edit SEO settings
    $('.contactus-seosetting__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let titlePage = $('input[name="TitlePage"]').val();
        let metaDescription = $('textarea[name="MetaDescription"]').val();
        let redirectURL = $('input[name="RedirectURL"]').val();
        $.ajax({
            url: "/Admin/ContactUs/SEO",
            type: "POST",
            data: {
                titlePage: titlePage,
                metaDescription: metaDescription,
                redirectURL: redirectURL
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
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

    //============About Us Page================
    //Delete About Content
    $('.delete-about-content-icon').click(function (e) {
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#remove-about-content-form-on-modal .item-id-container').html(input);
        UIkit.modal('#about-content-remove-modal').show();
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit Text
    $('.about-content-text__submit-button').click(function (e) {
        let parent = $(this).parents('li.about-content-list__list-item');
        let myId = $(this).attr('data-my-id');
        let myText = $(parent).find('.ql-editor').html();
        $.ajax({
            url: "/Admin/AboutUs/EditText",
            type: "POST",
            data: {
                id: myId,
                text: myText
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
        e.preventDefault();
        e.stopPropagation();
    });

    //Edit image (edit alt text)
    $('.about-content-image__submit-button').click(function () {
        let parent = $(this).parents('li.about-content-list__list-item');
        let myId = $(this).attr('data-my-id');
        let myAltText = $(parent).find('input[name="imageAlt"]').val();
        $.ajax({
            url: "/Admin/AboutUs/EditImageAltText",
            type: "POST",
            data: {
                id: myId,
                altText: myAltText
            },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        });
    });
    //Edit SEO settings
    $('.about-seo-settings__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-id');
        let title = $('input[name="TitlePage"]').val();
        let description = $('textarea[name="MetaDescription"]').val();
        let redirectURL = $('input[name="RedirectURL"]').val();
        $.ajax({
            url: "/Admin/AboutUs/EditSEOSetting",
            type: "POST",
            data: {
                id: myId,
                titlePage: title,
                metaDescription: description,
                redirectURL: redirectURL
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //=============FAQ Page====================
    //Add new faq
    $('.add-new-faq__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let question = $('input[name="Question"]').val();
        let answer = $('#Answer p:eq(0)').html();
        $.ajax({
            url: "/En/Admin/FAQ/AddFAQ",
            type: "POST",
            data: {
                question: question,
                answer: answer
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#faq-list-container').html(data);
                UIkit.notification({
                    message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Added</span>',
                    status: 'primary',
                    pos: 'top-center',
                    timeout: 5000
                });
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //=============Terms Page=================
    $('.edit-terms__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let content = $('.ql-editor').html();
        $.ajax({
            url: "/Admin/Terms/EditTerms",
            type: "POST",
            data: {
                content: content
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //SEO Settings
    $('.terms-seo-settings__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let titlePage = $('input[name="TitlePage"]').val();
        let metaDescription = $('textarea[name="MetaDescription"]').val();
        let redirectURL = $('input[name="RedirectURL"]').val();
        $.ajax({
            url: "/Admin/Terms/SEO",
            type: "POST",
            data: {
                titlePage: titlePage,
                metaDescription: metaDescription,
                redirectURL: redirectURL
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //============Orders Page==================
    try {
        const MDCOrderSort = mdc.select.MDCSelect;
        const orderSort = new MDCOrderSort(document.querySelector('.order-sort'));
        orderSort.listen('MDCSelect:change', function () {
            let sort = orderSort.value;
            $('.order-sort').attr('data-order-sort', sort);
            $.ajax({
                url: "/En/Admin/Orders/OrderListComponent",
                type: "POST",
                data: {
                    sort: sort
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    $('#order-list-dataTable').html(data);
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });

        $('input[name="myOrderSearch"]').keyup(function (e) {
            
            let text = $(this).val();
            $.ajax({
                url: "/En/Admin/Orders/OrderListComponent",
                type: "POST",
                data: {
                    searchText: $.trim(text)
                },
                error: function (xhr) { alert(xhr.responseText); },
                
                success: function (data) {
                    $('#order-list-dataTable').html(data);
                   
                }
            });

        });

        //order pagination buttons
        $('#order-pagination-buttons-container .pagination--button').click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            let pageIndex = $(this).attr('data-page-index');
            let sort = orderSort.value;
            let searchText = $('input[name="myOrderSearch"]').val();
            if ($.trim(searchText) === null || $.trim(searchText) === "") {
                searchText = null;
            }
            $.ajax({
                url: "/En/Admin/Orders/OrderListComponent",
                type: "POST",
                data: {
                    searchText: $.trim(searchText),
                    pageIndex: pageIndex,
                    sort: sort
                },
                error: function (xhr) { console.log(xhr.responseText); },

                success: function (data) {
                    $('#order-list-dataTable').html(data);
                    $('html,body').animate({ scrollTop: 0 }, 'slow');
                }
            });
        });
    } catch (e) {
        console.log(e);
    }

    //change order status in Shopping Detail Page
    $('input[name="Status"]').change(function () {
        let status = $(this).val();
        let shoppingId = $(this).attr('data-id');
        $.ajax({
            url: "/Admin/Orders/ChangeShoppingStatus",
            type: "POST",
            data: {
                status: status,
                shoppingId:shoppingId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //Edit shopping description
    $('.shopping-description--submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let description = $('textarea[name="shoppingDescription"]').val();
        let shoppingId = $(this).attr('data-id');
        $.ajax({
            url: "/Admin/Orders/EditDescription",
            type: "POST",
            data: {
                description: description,
                shoppingId: shoppingId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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
    //=============Comments Page===============
    $(document).on('click', '.comment-list-items__link', function () {
        $('#comment-list-contents').toggleClass('active');
    });
    $(document).on('click', '#comment-back-button', function () {
        $('#comment-list-contents').toggleClass('active');
    });
    

    //switch on comment tab (comments of product / comments of blog)
    $(document).on('click', '#product-comment-tab', function (e) {
        e.stopPropagation();
        e.preventDefault();
        $.ajax({
            url: "/Admin/Comments/ProductCommentTab",
            type: "POST",
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#comments-container').html(data);
                $('#comment-list-contents').animate({ scrollTop: $('.commens-container-of-this-product').height() }, 300);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    $(document).on('click', '#blog-comment-tab', function (e) {
        e.stopPropagation();
        e.preventDefault();
        $.ajax({
            url: "/En/Admin/Comments/BlogCommentTab",
            type: "POST",
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#comments-container').html(data);
                $('#comment-list-contents').animate({ scrollTop: $('.commens-container-of-this-blog').height() }, 300);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //Product Comments:
    //Add reply to a comment
    $(document).on('click', '.reply-to-comment-button',function () {
        let fullname = $(this).attr('data-fullname');
        let description = $(this).attr('data-description');
        let commentId = $(this).attr('data-id');
        if (description.length > 30) {
            description = description.substr(0, 30) + "....";
        }
        $('#reply-comment-modal .target-comment-fullname').html(fullname);
        $('#reply-comment-modal .target-comment-description').html(description);
        $('#reply-comment-modal .reply-comment-submit-button').attr('data-commentId', commentId);

    });

    $(document).on('click', '#reply-comment-modal .reply-comment-submit-button',function (e) {
        e.stopPropagation();
        e.preventDefault();
        let replyDescription = $('textarea[name="replyDescription"]').val();
        if ($.trim(replyDescription) === null || $.trim(replyDescription) === "") {
            $('.error-reply-description').removeClass('d-none').html('please filled this field');
            return false;
        }
        let commentId = $(this).attr('data-commentId');
        $.ajax({
            url: "/Admin/Comments/AddReplyCommentFromAdmin",
            type: "POST",
            data: {
                commentId: commentId,
                replyDescription: replyDescription
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                UIkit.modal('#reply-comment-modal').hide();
                $('.my-reply-comments[data-id="' + commentId + '"]').append(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    $('#comment-list-contents').animate({ scrollTop: $('.commens-container-of-this-product').height() }, 300);
    //change comment list when click on another product
    $(document).on('click', '.comment-list-items__link', function (e) {
        e.stopPropagation();
        e.preventDefault();
        $('.product-comment-list-items').removeClass('active');
        $(this).parents('li').addClass('active');
        let productId = $(this).attr('data-id');
        $.ajax({
            url: "/En/Admin/Comments/CommentsOfThisProduct",
            type: "POST",
            data: {
                productId: productId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('.commens-container-of-this-product').html(data);
                $('#comment-list-contents').animate({ scrollTop: $('.commens-container-of-this-product').height() }, 300);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //publish a comment
    $(document).on('click', '.comment-publish-icon', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let elem = $(this);
        let isPublished = $(this).attr('data-publish');
        if (isPublished.toLowerCase() === "true") {
            isPublished = true;
        } else {
            isPublished = false;
        }
        let commentId = $(this).attr('data-id');
        let myIcon = $(this).find('i');
        $.ajax({
            url: "/Admin/Comments/PublishComment",
            type: "POST",
            data: {
                isPublished: !isPublished,
                commentId: commentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(elem).attr('data-publish', !isPublished);
                    if (isPublished) {
                        $(myIcon).html('visibility_off');
                       
                    } else {
                        $(myIcon).html('visibility');
                    }
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Published</span>',
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

    //delete a comment
    $(document).on('click', '.comment-delete', function () {
        let commentId = $(this).attr('data-id');
        $('#delete-comment-modal .delete-comment-submit-button').attr('data-id', commentId);
    });

    $(document).on('click', '#delete-comment-modal .delete-comment-submit-button', function () {
        let commentId = $(this).attr('data-id');
        let mainParent = $('li[data-id="' + commentId + '"]');
        $.ajax({
            url: "/Admin/Comments/DeleteComment",
            type: "POST",
            data: {
                commentId: commentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(mainParent).remove();
                    UIkit.modal('#delete-comment-modal').hide();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Deleted</span>',
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

    //delete a reply comment
    $(document).on('click', '.delete-reply-comment', function () {
        let replyCommentId = $(this).attr('data-id');
        $('#delete-reply-comment-modal .delete-reply-comment-submit-button').attr('data-id', replyCommentId);
    });

    $(document).on('click', '#delete-reply-comment-modal .delete-reply-comment-submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let replyCommentId = $(this).attr('data-id');
        $.ajax({
            url: "/Admin/Comments/DeleteReplyComment",
            type: "POST",
            data: {
                replyCommentId: replyCommentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $('.reply-comment-box[data-id="' + replyCommentId + '"]').remove();
                    UIkit.modal('#delete-reply-comment-modal').hide();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Deleted</span>',
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

    //publish a reply comment
    $(document).on('click', '.reply-comment-publish', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let elem = $(this);
        let replyCommentId = $(this).attr('data-id');
        let isPublished = $(this).attr('data-publish');
        if (isPublished.toLowerCase() === "true") {
            isPublished = true;
        } else {
            isPublished = false;
        }
        let myIcon = $(this).find('i');
        $.ajax({
            url: "/Admin/Comments/PublishReplyComment",
            type: "POST",
            data: {
                isPublished: !isPublished,
                replyCommentId: replyCommentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(elem).attr('data-publish', !isPublished);
                    if (isPublished) {
                        $(myIcon).html('visibility_off');
                    } else {
                        $(myIcon).html('visibility');
                    }
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Published</span>',
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

    //Blog Comments:
    //Add reply to a blog comment
    $(document).on('click', '.blog-reply-to-comment-button', function () {
        let fullname = $(this).attr('data-fullname');
        let description = $(this).attr('data-description');
        let commentId = $(this).attr('data-id');
        if (description.length > 30) {
            description = description.substr(0, 30) + "....";
        }
        $('#blog-reply-comment-modal .target-comment-fullname').html(fullname);
        $('#blog-reply-comment-modal .target-comment-description').html(description);
        $('#blog-reply-comment-modal .reply-comment-submit-button').attr('data-commentId', commentId);

    });

    $(document).on('click', '#blog-reply-comment-modal .reply-comment-submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let replyDescription = $('textarea[name="replyDescription"]').val();
        if ($.trim(replyDescription) === null || $.trim(replyDescription) === "") {
            $('.error-reply-description').removeClass('d-none').html('please fill this field');
            return false;
        }
        let commentId = $(this).attr('data-commentId');
        $.ajax({
            url: "/En/Admin/Comments/AddBlogReplyCommentFromAdmin",
            type: "POST",
            data: {
                commentId: commentId,
                replyDescription: replyDescription
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                UIkit.modal('#blog-reply-comment-modal').hide();
                $('.my-reply-comments[data-id="' + commentId + '"]').append(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    $('#comment-list-contents').animate({ scrollTop: $('.commens-container-of-this-blog').height() }, 300);
    //change comment list when click on another product
    $(document).on('click', '.comment-list-items__link', function (e) {
        e.stopPropagation();
        e.preventDefault();
        $('.blog-comment-list-items').removeClass('active');
        $(this).parents('li').addClass('active');
        let blogId = $(this).attr('data-id');
        $.ajax({
            url: "/En/Admin/Comments/CommentsOfThisBlog",
            type: "POST",
            data: {
                blogId: blogId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('.commens-container-of-this-blog').html(data);
                $('#comment-list-contents').animate({ scrollTop: $('.commens-container-of-this-blog').height() }, 300);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //publish a comment
    $(document).on('click', '.blog-comment-publish-icon', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let elem = $(this);
        let isPublished = $(this).attr('data-publish');
        if (isPublished.toLowerCase() === "true") {
            isPublished = true;
        } else {
            isPublished = false;
        }
        let commentId = $(this).attr('data-id');
        let myIcon = $(this).find('i');
        $.ajax({
            url: "/Admin/Comments/PublishBlogComment",
            type: "POST",
            data: {
                isPublished: !isPublished,
                commentId: commentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(elem).attr('data-publish', !isPublished);
                    if (isPublished) {
                        $(myIcon).html('visibility_off');

                    } else {
                        $(myIcon).html('visibility');
                    }
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Published</span>',
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

    //delete a comment
    $(document).on('click', '.blog-comment-delete', function () {
        let commentId = $(this).attr('data-id');
        $('#blog-delete-comment-modal .delete-comment-submit-button').attr('data-id', commentId);
    });

    $(document).on('click', '#blog-delete-comment-modal .delete-comment-submit-button', function () {
        let commentId = $(this).attr('data-id');
        let mainParent = $('li[data-id="' + commentId + '"]');
        $.ajax({
            url: "/Admin/Comments/DeleteBlogComment",
            type: "POST",
            data: {
                commentId: commentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(mainParent).remove();
                    UIkit.modal('#blog-delete-comment-modal').hide();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Deleted</span>',
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

    //delete a reply comment
    $(document).on('click', '.blog-delete-reply-comment', function () {
        let replyCommentId = $(this).attr('data-id');
        $('#blog-delete-reply-comment-modal .delete-reply-comment-submit-button').attr('data-id', replyCommentId);
    });

    $(document).on('click', '#blog-delete-reply-comment-modal .delete-reply-comment-submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let replyCommentId = $(this).attr('data-id');
        $.ajax({
            url: "/Admin/Comments/DeleteBlogReplyComment",
            type: "POST",
            data: {
                replyCommentId: replyCommentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $('.reply-comment-box[data-id="' + replyCommentId + '"]').remove();
                    UIkit.modal('#blog-delete-reply-comment-modal').hide();
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Deleted</span>',
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

    //publish a reply comment
    $(document).on('click', '.blog-reply-comment-publish', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let elem = $(this);
        let replyCommentId = $(this).attr('data-id');
        let isPublished = $(this).attr('data-publish');
        if (isPublished.toLowerCase() === "true") {
            isPublished = true;
        } else {
            isPublished = false;
        }
        let myIcon = $(this).find('i');
        $.ajax({
            url: "/Admin/Comments/PublishBlogReplyComment",
            type: "POST",
            data: {
                isPublished: !isPublished,
                replyCommentId: replyCommentId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    $(elem).attr('data-publish', !isPublished);
                    if (isPublished) {
                        $(myIcon).html('visibility_off');
                    } else {
                        $(myIcon).html('visibility');
                    }
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Comment Published</span>',
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
    //============ Messages (Tickets) ==============
    //when message sort select box changed
    try {
        const MDCMessageSort = mdc.select.MDCSelect;
        const messageSort = new MDCMessageSort(document.querySelector('.message-sort'));
        messageSort.listen('MDCSelect:change', function () {
            let sort = messageSort.value;
            $('#message-list-container').attr('data-sort', sort);
            $.ajax({
                url: "/En/Admin/Messages/TicketList",
                type: "POST",
                data: {
                    sort: sort
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    $('#message-list-container').html(data);
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
        //when typing on message search input for search a message
        $('input[name="messageSearch"]').keyup(function () {
            messageSort.selectedIndex = 0;
            let searchText = $(this).val();
            console.log(searchText);
            $('#message-list-container').attr('data-sort', 0).attr('data-searchText', searchText);

            $.ajax({
                url: "/En/Admin/Messages/TicketList",
                type: "POST",
                data: {
                    searchText: searchText
                },
                success: function (data) {
                    $('#message-list-container').html(data);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }
   
    //click on ticket pagination buttons
    $(document).on('click', '#message-pagination-buttons-container .pagination--button', function (e) {
        let myIndex = $(this).attr('data-page-index');
        let sort = $('#message-list-container').attr('data-sort');
        let searchText = $('#message-list-container').attr('data-searchText');
        $.ajax({
            url: "/En/Admin/Messages/TicketList",
            type: "POST",
            data: {
                pageIndex: myIndex,
                sort: sort,
                searchText: searchText
            },
            success: function (data) {
                $('#message-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });

    //Delete reply ticket in ticket detail page
    $(document).on('click', '.reply-ticket-delete', function (e) {
        let myId = $(this).attr('data-my-id');
        let input = '<input type="hidden" name="id" id="id" value="' + myId + '" />';
        $('#reply-ticket-remove-modal .item-id-container').html(input);
        UIkit.modal('#reply-ticket-remove-modal').show();
        e.preventDefault();
        e.stopPropagation();
    });

    //add new reply ticket in ticket detail page
    $(document).on('click', '.reply-ticket__submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let replyDescription = $('.replyTicketDescription .ql-editor').html();
        let ticketId = $(this).attr('data-id');
        if ($.trim(replyDescription) === null || $.trim(replyDescription) === "") {
            $('.error-replyTicketDescription').removeClass('d-none').html('please fill this field');
            return false;
        }
        $.ajax({
            url: "/En/Admin/Messages/AddReplyTicket",
            type: "POST",
            data: {
                replyDescription: replyDescription,
                ticketId: ticketId
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                $('#reply-tickets-container').append(data);
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //============Notification======================
    //edit notification
    $('.notification__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let mainText = $('input[name="MainText"]').val();
        let linkText = $('input[name="LinkText"]').val();
        if ($.trim(mainText) === null || $.trim(mainText)==="") {
            $('.error-mainText').removeClass('d-none').html('please fill this field');
            return false;
        }
        if ($.trim(linkText) === null || $.trim(linkText) === "") {
            $('.error-linkText').removeClass('d-none').html('please fill this field');
            return false;
        }

        $.ajax({
            url: "/Admin/Notification/EditNotification",
            type: "POST",
            data: {
                mainText: mainText,
                linkText: linkText
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //edit notification publishment
    $('input.web-notification').change(function () {
        let isPublished = $(this).is(':checked');
        if (isPublished) {
            $(this).prop('checked', true);
        } else {
            $(this).prop('checked', false);
        }
        $.ajax({
            url: "/Admin/Notification/Publishment",
            type: "POST",
            data: {
                isPublished: isPublished
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (isPublished) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Notification Bar Published</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    } else {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Notification Bar Unpublished</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }
                
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //============Admin Index Page==================
    try {
        //Income-Time Chart
        let ctx01 = $('#income-time-chart').get(0).getContext('2d');
        let sellNumbers = $('#income-time-chart').attr('data-sellnumbers').split(',').map(c=> +c);
        let timeColumns = $('#income-time-chart').attr('data-timecolumns').split(',');
        console.log(sellNumbers);
        console.log(timeColumns);
        let incomeTimeCahrt = chartJs(ctx01, sellNumbers, 'blue', 'rgba(158, 181, 252, 0.38)', timeColumns);

        //Register-Time Chart
        let ctx02 = $('#register-time-chart').get(0).getContext('2d');
        let registerNumbers = $('#register-time-chart').attr('data-registernumbers').split(',').map(c => +c);
        let timeColumns02 = $('#register-time-chart').attr('data-timecolumns').split(',');
        let registerTimeChart = chartJs(ctx02, registerNumbers, 'rgb(9, 186, 94)', 'rgba(0, 255, 159, 0.38)', timeColumns02);
        $(window).resize(function () {
            let myFontSize = 13;
            if ($(window).width() < 800) {
                myFontSize = 9;
            } else {
                myFontSize = 13;
            }
            incomeTimeCahrt.options.scales.xAxes[0].ticks.fontSize = myFontSize;
            registerTimeChart.options.scales.xAxes[0].ticks.fontSize = myFontSize;
        });
    } catch (e) {
        console.log(e.message);
    }
    //select range of income chart
    try {
        const MDCIncomeRangeDate = mdc.select.MDCSelect;
        const incomeRangeDate = new MDCIncomeRangeDate(document.querySelector('.icome-chart-date-range'));
        incomeRangeDate.listen('MDCSelect:change', function () {
            let dateRange = incomeRangeDate.value;
            $.ajax({
                url: "/En/Admin/IncomeChart",
                type: "POST",
                data: {
                    dateRange: dateRange
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    $('#income-chart-container').html(data);
                    try {
                        //Income-Time Chart
                        let ctx01 = $('#income-time-chart').get(0).getContext('2d');
                        let sellNumbers = $('#income-time-chart').attr('data-sellnumbers').split(',').map(c => +c);
                        let timeColumns = $('#income-time-chart').attr('data-timecolumns').split(',');
                        console.log(sellNumbers);
                        console.log(timeColumns);
                        let incomeTimeCahrt = chartJs(ctx01, sellNumbers, 'blue', 'rgba(158, 181, 252, 0.38)', timeColumns);
                    } catch (e) {
                        console.log(e);
                    }
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }

    //select range of register chart
    try {
        const MDCRegisterRangeDate = mdc.select.MDCSelect;
        const registerRangeDate = new MDCRegisterRangeDate(document.querySelector('.register-chart-date-range'));
        registerRangeDate.listen('MDCSelect:change', function () {
            let dateRange = registerRangeDate.value;
            $.ajax({
                url: "/En/Admin/RegisterChart",
                type: "POST",
                data: {
                    dateRange: dateRange
                },
                error: function (xhr) { alert(xhr.responseText); },
                beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
                success: function (data) {
                    $('#register-chart-container').html(data);
                    try {
                        //Income-Time Chart
                        let ctx02 = $('#register-time-chart').get(0).getContext('2d');
                        let registerNumbers = $('#register-time-chart').attr('data-registernumbers').split(',').map(c => +c);
                        let timeColumns02 = $('#register-time-chart').attr('data-timecolumns').split(',');
                        
                        let registerTimeCahrt = chartJs(ctx02, registerNumbers, 'rgb(9, 186, 94)', 'rgba(0, 255, 159, 0.38)', timeColumns02);
                    } catch (e) {
                        console.log(e);
                    }
                }
            }).done(function () {
                $('#progress-loader').addClass('d-none');
            });
        });
    } catch (e) {
        console.log(e);
    }

    //=============SEO Settings=====================
    //Edit seo block
    $('input[name="IsBlock"]').change(function () {
        let isBlock = $(this).is(':checked');
        console.log(isBlock);
        $.ajax({
            url: "/Admin/SEO/IsBlock",
            type: "POST",
            data: {
                isBlock: isBlock
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (isBlock) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    } else {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //================Customer=====================
    try {
        const MDCCustomerSort = mdc.select.MDCSelect;
        const customerSort = new MDCCustomerSort(document.querySelector('.customer-sort'));
        const MDCCustomerRoleSort = mdc.select.MDCSelect;
        const customerRoleSort = new MDCCustomerRoleSort(document.querySelector('.customer-role-sort'));
        //----Search customer by name-------
        $('input[name="customerSearch"]').keyup(function (e) {
            customerSort.selectedIndex = 0;
            customerRoleSort.selectedIndex = 0;
            let customerSearch = $(this).val();
            console.log(customerSearch);
            $('#datatable-customer-list-container').attr('data-sort', 0);
            $('#datatable-customer-list-container').attr('data-role-sort', 0);
            $('#datatable-customer-list-container').attr('data-search-text', customerSearch);
            $.ajax({
                url: "/En/Admin/Customer/CustomerList",
                type: "POST",
                data: {
                    customerSearch: customerSearch
                },
                success: function (data) {

                    $('#datatable-customer-list-container').html(data);
                }
            });
            e.stopPropagation();
            e.preventDefault();
        });

        //------Sort customers based on sort and role sort-----

        customerSort.listen('MDCSelect:change', function () {
            let sort = customerSort.value;
            let roleSort = customerRoleSort.value;
            $('#datatable-customer-list-container').attr('data-sort', sort);
            $('#datatable-customer-list-container').attr('data-role-sort', roleSort);
            $('#datatable-customer-list-container').attr('data-search-text', "");
            $.ajax({
                url: "/En/Admin/Customer/CustomerList",
                type: "POST",
                data: {
                    sort: sort,
                    roleSort: roleSort
                },
                success: function (data) {
                    $('#datatable-customer-list-container').html(data);
                }
            });
        });


        //------Filter customers by roles-------------

        customerRoleSort.listen('MDCSelect:change', function () {
            let sort = customerSort.value;
            let roleSort = customerRoleSort.value;
            $('#datatable-customer-list-container').attr('data-sort', sort);
            $('#datatable-customer-list-container').attr('data-role-sort', roleSort);
            $('#datatable-customer-list-container').attr('data-search-text', "");
            
            $.ajax({
                url: "/En/Admin/Customer/CustomerList",
                type: "POST",
                data: {
                    sort: sort,
                    roleSort: roleSort
                },
                success: function (data) {
                    $('#datatable-customer-list-container').html(data);
                }
            });
        });
    } catch (e) {
        console.log(e);
    }
    //------Customer Pagination--------------
    //when click on pagination buttons on customer list page
    $(document).on('click','#customer-list__pageination-buttons-container .pagination--button',function (e) {
        let myIndex = $(this).attr('data-page-index');
        let customerSearch = $('#datatable-customer-list-container').attr('data-search-text');
        let sort = $('#datatable-customer-list-container').attr('data-sort');
        let roleSort = $('#datatable-customer-list-container').attr('data-role-sort');
        $.ajax({
            url: "/En/Admin/Customer/CustomerList",
            type: "POST",
            data: {
                pageIndex: myIndex,
                customerSearch: customerSearch,
                sort: sort,
                roleSort: roleSort
            },
            success: function (data) {
                $('#datatable-customer-list-container').html(data);
                $('html,body').animate({ scrollTop: 0 }, 'slow');
            }
        });
        e.stopPropagation();
        e.preventDefault();
    });

   
   
    //------Select All Customer From the Table-------

    $(document).on('change','input[name="main-customer-checkbox"]',function () {
        let isChecked = $(this).is(':checked');
        if (isChecked) {
            $('.customer-checkbox').prop('checked', true);
        } else {
            $('.customer-checkbox').prop('checked', false);
        }
        if ($('.customer-checkbox:checked').length > 0) {
            $('#all-customer-actions-control').addClass('active');
        } else {
            $('#all-customer-actions-control').removeClass('active');
        }
    });

    $('input.customer-checkbox').change(function () {
        if ($('.customer-checkbox:checked').length > 0) {
            $('#all-customer-actions-control').addClass('active');
        } else {
            $('#all-customer-actions-control').removeClass('active');
        }
    });
    //-----------Delete a group of customers in customer list page-----------
    $(document).on('click','.group-customer-delete-icon',function (e) {
        let selectedCustomers = $('.customer-checkbox:checked');
        let idContainer = $('#group-remove-modal .group-id-container');

        selectedCustomers.each(function () {
            let id = $(this).attr('data-my-id');
            $(idContainer).append('<input name="ids" type="hidden" value="' + id + '" />');
        });
        UIkit.modal('#group-remove-modal').show();
        e.stopPropagation();
        e.preventDefault();
    });
    //---------Publish a group of customers in customer list page-----------
    $(document).on('click','.group-customer-publish-icon',function (e) {
        let selectedCustomers = $('.customer-checkbox:checked');
        let myIds = [];
        selectedCustomers.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        console.log(myIds);
        $.ajax({
            url: "/Admin/Customer/ActiveGroupCustomer",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetCustomers = $('.customer-publishment-icon[data-my-id="' + item + '"]');
                        $(targetCustomers).attr('data-status', true);
                        $(targetCustomers).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Accounts Became Active</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });

    //---------UnPublish a group of customer in customer list page-----------
    $(document).on('click', '.group-customer-unpublish-icon', function (e) {
        let selectedCustomers = $('.customer-checkbox:checked');
        let myIds = [];
        selectedCustomers.each(function () {
            let id = $(this).attr('data-my-id');
            myIds.push(id);
        });
        $.ajax({
            url: "/Admin/Customer/DisActiveGroupCustomer",
            type: "POST",
            data: {
                ids: myIds
            },
            traditional: true,
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    myIds.forEach(function (item) {
                        let targetCustomers = $('.customer-publishment-icon[data-my-id="' + item + '"]');
                        $(targetCustomers).attr('data-status', false);
                        $(targetCustomers).html('<i class="material-icons-outlined fontsize-18 uk-text-muted">visibility_off</i>');
                    });

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Accounts Became Inactive</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });
    
    
    //=============Notification=====================
    //Edit link text and main text of notification
    $('.notification__submit-button').click(function (e) {
        e.stopPropagation();
        e.preventDefault();
        let linkText = $('input[name="LinkText"]').val();
        let mainText = $('input[name="MainText"]').val();
        $.ajax({
            url: "/Admin/Notification/EditNotification",
            type: "POST",
            data: {
                mainText: mainText,
                linkText: linkText
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });
    //=============Input Validation=================
    $('input.input-validation-error').keyup(function () {

        $(this).removeClass('input-validation-error');
    });
    $('input#Input_TitlePage,input#TitlePage').keyup(function () {

        let myval = $(this).val();
        $('#title-page-preview').html(myval);
        if (!myval.length > 0) {
            $('#title-page-preview').html("Title");
        }
    });

    $('textarea#Input_MetaDescription,textarea#MetaDescription').keyup(function () {
        let myval = $(this).val();
        $('#metadescription-page-preview').html(myval);
        if (!myval.length > 0) {
            $('#metadescription-page-preview').html('If leave empty, it automatically will generate by search engine');
        }
    });

    //input type numbers 
    $(document).on('keydown','input.number-type',function (e) {
        let key = e.keyCode || e.which;
        if (!((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105)) && key !== 8) {
            e.preventDefault();
            return false;
        }
    });
    //==============Image Uploading============
    //Function for show preview of file before uploading
    function readURL(input, fileid) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(fileid).attr('src', e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
    $('#Input_Image').change(function () {
        readURL(this, '#image-result');
        $('#image-result').removeClass('d-none');
        $('#brand-image-container .image-upload--placeholder').remove();
    });
    $('#Image').change(function () {
        readURL(this, '#image-result');
        $('#image-result').removeClass('d-none');
        $('#brand-image-container .image-upload--placeholder').remove();
    });
    $('#Input_CategoryImage,#CategoryImage').change(function () {
        readURL(this, '#image-result');
        $('#image-result').removeClass('d-none');
        $('#category-image-container .image-upload--placeholder').remove();
    });
  
    $('input[name="CoverImage"]').change(function () {
        $('#progress-loader').removeClass('d-none');
        readURL(this, '#blog-cover-image');
        let myId = $(this).attr('data-id');
        let fileInput = document.getElementById("CoverImage");
        let files = fileInput.files;
        const formData = new FormData();
        for (var i = 0; i !== files.length; i++) {
            formData.append("Images", files[i]);
        }
        formData.append("id", myId);
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();

        xhr.open('POST', '/Admin/Blog/UploadCoverImage');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                $('#progress-loader').addClass('d-none');
                if (xhr.responseText === "true") {

                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Item Changed</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        };

    });

    //product images
    $('input[name="Images"]').change(function () {
        let productId = $('#productId').val();
        let fileInput = document.getElementById("Images");
        let files = fileInput.files;
        const formData = new FormData();
        for (var i = 0; i !== files.length; i++) {
            formData.append("Images", files[i]);
        }
        formData.append("productId", productId);
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", Progress, false);
        xhr.open('POST', '/En/Admin/Products/Upload');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                if (xhr.responseText === "true") {
                    $('#upload-progress').attr('value', 0);
                    $.ajax({
                        url: "/En/Admin/Products/ImageList/",
                        type: "POST",
                        data: {
                            proId: productId
                        },
                        success: function (data) {
                            $('#productImage-list-container').html(data);
                            UIkit.notification({
                                message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Images Uploaded</span>',
                                status: 'primary',
                                pos: 'top-center',
                                timeout: 5000
                            });
                        }
                    });

                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }

            }
        };
    });
    function Progress() {

        var percent = (event.loaded / event.total) * 100;
        $('#upload-progress').attr('value', percent);
    }
    //site logo image
    $(document).on('change', 'input[name="siteLogo"]', function (e) {
        let myimage = $('#siteLogoPreview');
        let mySpinner = $(this).siblings('.my-spinner');
        $(mySpinner).removeClass('d-none');
        $(myimage).css('opacity', .6);
        let fileInput = document.getElementById(e.target.id);
        let files = fileInput.files;
        const formData = new FormData();
        formData.append('siteLogo', files[0]);
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Admin/HomePage/EditLogo');
        xhr.send(formData);
        xhr.onreadystatechange = function () {

            if (xhr.readyState === 4 && xhr.status === 200) {

                if (xhr.responseText === "true") {
                    $(mySpinner).addClass('d-none');
                    var reader = new FileReader();
                    reader.onload = function (event) {
                        $(myimage).attr('src', event.target.result);
                    };
                    reader.readAsDataURL(files[0]);
                    $(myimage).css('opacity', 1);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Image Uploaded</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });

                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }

            }
        }
    });
    //home slider images
    $('input[name="HomeSliderImages"]').change(function () {
        let myid = $(this).attr('data-id');
        let fileInput = document.getElementById("HomeSliderImages");
        let files = fileInput.files;
        const formData = new FormData();
        for (var i = 0; i !== files.length; i++) {
            formData.append("HomeSliderImages", files[i]);
        }
        formData.append("homeId", myid);
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", Progress, false);
        xhr.open('POST', '/Admin/HomePage/UploadImageSlider');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                $('#upload-progress').attr('value', 0);
                if (xhr.responseText === "true") {
                    $.ajax({
                        url: "/En/Admin/HomePage/SliderImageList/",
                        type: "POST",
                        data: {
                            homeId: myid
                        },
                        error: function (xhr) {
                            alert(xhr.responseText);
                        },
                        success: function (data) {
                            
                            $('#homeSliderImage-list-container').html(data);
                            
                            UIkit.notification({
                                message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Images Uploaded</span>',
                                status: 'primary',
                                pos: 'top-center',
                                timeout: 5000
                            });
                        }
                    });

                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }

            }
        };
    });

    function MobileProgress() {

        var percent = (event.loaded / event.total) * 100;
        $('#mobile-upload-progress').attr('value', percent);
    }
    //home slider images
    $('input[name="MobileHomeSliderImages"]').change(function () {
        let myid = $(this).attr('data-id');
        let fileInput = document.getElementById("MobileHomeSliderImages");
        let files = fileInput.files;
        const formData = new FormData();
        for (var i = 0; i !== files.length; i++) {
            formData.append("HomeSliderImages", files[i]);
        }
        formData.append("homeId", myid);
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", MobileProgress, false);
        xhr.open('POST', '/Admin/HomePage/UploadMobileImageSlider');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                $('#mobile-upload-progress').attr('value', 0);
                if (xhr.responseText === "true") {
                    $.ajax({
                        url: "/En/Admin/HomePage/MobileSliderImageList/",
                        type: "POST",
                        data: {
                            homeId: myid
                        },
                        error: function (xhr) {
                            alert(xhr.responseText);
                        },
                        success: function (data) {

                            $('#mobileHomeSliderImage-list-container').html(data);

                            UIkit.notification({
                                message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Images Uploaded</span>',
                                status: 'primary',
                                pos: 'top-center',
                                timeout: 5000
                            });
                        }
                    });

                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }

            }
        };
    });

    //Change Alt of images
    $(document).on('click', '.alt-imageslider-form-button', function (e) {
        let myform = $(this).parents('.alt-form');
        let altInput = $(myform).find('input[name="altText"]');
        let myId = $(altInput).attr('data-imageId');
        let mytext = $(altInput).val();
        let myUrl = $(this).attr('data-url');
        $.ajax({
            url: myUrl,
            type: "POST",
            data: {
                imageId: myId,
                altText: mytext
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });

    //Change target link of images
    $(document).on('click', '.targetLink-imageslider-form-button', function (e) {
        let myform = $(this).parents('.targetLink-form');
        let targetLinkInput = $(myform).find('input[name="targetLink"]');
        let myId = $(targetLinkInput).attr('data-imageId');
        let targetLink = $(targetLinkInput).val();
        let myUrl = $(this).attr('data-url');

        $.ajax({
            url: myUrl,
            type: "POST",
            data: {
                imageId: myId,
                targetLink: targetLink
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
        e.stopPropagation();
        e.preventDefault();
    });
    //=============Banner Management================
    //when image of home banner changed
    $('.inputOfImageBanner').change(function (e) {
        let myId = $(this).attr('data-id');
        let myimage = $(this).siblings('img');
        let mySpinner = $(this).siblings('.my-spinner');
        $(mySpinner).removeClass('d-none');
        $(myimage).css('opacity', .6);
        let fileInput = document.getElementById(e.target.id);
        let files = fileInput.files;
        const formData = new FormData();
        for (var i = 0; i !== files.length; i++) {
            formData.append('image', files[i]);
        }
        formData.append("homeBannerId", myId);
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();

        xhr.open('POST', '/Admin/BannerManagement/ChangeHomeBannerImage');
        xhr.send(formData);
        xhr.onreadystatechange = function () {

            if (xhr.readyState === 4 && xhr.status === 200) {

                if (xhr.responseText === "true") {
                    $(mySpinner).addClass('d-none');
                    var reader = new FileReader();
                    reader.onload = function (event) {
                        $(myimage).attr('src', event.target.result);
                    };
                    reader.readAsDataURL(files[0]);
                    $(myimage).css('opacity', 1);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Image Uploaded</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });

                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }

            }
        };

    });

    //change home banner information (title, alt and target link)
    $(document).on('click', '.home-banner-information__submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myparent = $(this).parents('.home-banner-information');
        let title = $(myparent).find('input[name="title"]').val();
        let alt = $(myparent).find('input[name="alt"]').val();
        let targetLink = $(myparent).find('input[name="targetLink"]').val();
        let myId = $(this).attr('data-my-id');
        $.ajax({
            url: "/Admin/BannerManagement/ChnageHomeBannerInformation",
            type: "POST",
            data: {
                homeBannerId: myId,
                title: title,
                alt: alt,
                targetLink: targetLink
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    if (data) {
                        UIkit.notification({
                            message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
                            status: 'primary',
                            pos: 'top-center',
                            timeout: 5000
                        });
                    }

                 
                }
            }
        }).done(function () {
            $('#progress-loader').addClass('d-none');
        });
    });

    //upload banners of blog page
    $(document).on('change', 'input[name="blogPageImage"]', function (e) {
        let fileInput = document.getElementById(e.target.id);
        let files = fileInput.files;
        const formData = new FormData();
        for (var i = 0; i !== files.length; i++) {
            formData.append("images", files[i]);
        }
        //Creating an XMLHttpRequest and sending
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", Progress, false);
        xhr.open('POST', '/Admin/BannerManagement/AddNewBlogPageBanner');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
              
                if (xhr.responseText) {
                    console.log(xhr.responseText);
                    $('#upload-progress').attr('value', 0);
                    $('#blogPage-bannerImage-list').append(xhr.responseText);
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Images Uploaded</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });

                } else {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Error</span>',
                        status: 'primary',
                        pos: 'top-center',
                        timeout: 5000
                    });
                }

            }
        };
    });

    //save blog page banner information changing
    $(document).on('click', '.blogPage-bannerIfnormation__submit-button', function (e) {
        e.stopPropagation();
        e.preventDefault();
        let myId = $(this).attr('data-my-id');
        let myParent = $(this).parents('.blog-page-information__formbody');
        let title = $(myParent).find('input[name="title"]').val();
        let alt = $(myParent).find('input[name="alt"]').val();
        let targetLink = $(myParent).find('input[name="targetLink"]').val();
        $.ajax({
            url: "/Admin/BannerManagement/ChangeBlogPageBannerInformation",
            type: "POST",
            data: {
                id: myId,
                title: title,
                alt: alt,
                targetLink: targetLink
            },
            error: function (xhr) { alert(xhr.responseText); },
            beforeSend: function () { $('#progress-loader').removeClass('d-none'); },
            success: function (data) {
                if (data) {
                    UIkit.notification({
                        message: '<span class="material-icons-outlined red-lighten-7 mx-2 position-relative" style="top:6px">notifications_active</span><span class="IRANSans_Light fontsize-14" dir="ltr">Changes Saved</span>',
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

    //=============Instagram Posts=============
    
    //==============Text Editor================
    function quillInitialize(elem) {
        let toolbarOptions = [
            ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
            ['blockquote', 'code-block'],               // custom button values
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
            [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
            [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
            [{ 'direction': 'ltr' }],                         // text direction
            [{ 'link': 'https://quilljs.com' }],

            [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
            [{ 'align': ['right', 'center', '', 'justify'] }],

            ['clean']                                         // remove formatting button
        ];

        let quill = new Quill(elem, {
            modules: {
                toolbar: toolbarOptions

            },
            theme: 'snow'
        });
        $(elem).click(function () {
           
            quill.format('direction', 'ltr');
        });

    }
    $('.text-editor').each(function () {
        quillInitialize(this);
    });

    ////============Google material design=================
    //Google material design button
    function googleMaterialDesignInstall() {
        const mdcButton = $('.mdc-button');
        for (let i = 0; i < mdcButton.length; i++) {
            mdc.ripple.MDCRipple.attachTo(document.querySelectorAll('.mdc-button')[i]);
        }
        mdc.ripple.MDCRipple.attachTo(document.querySelector('.mdc-fab'));
       
        const mdcTextField = $('.mdc-text-field');
        for (let i = 0; i < mdcTextField.length; i++) {
            mdc.textField.MDCTextField.attachTo(document.querySelectorAll('.mdc-text-field')[i]);
        }
        mdc.tabBar.MDCTabBar.attachTo(document.querySelector('.my-tab-list'));
        const MDCTABLE = $('.mdc-data-table');
        for (let i = 0; i < MDCTABLE.length; i++) {
            mdc.dataTable.MDCDataTable.attachTo(document.querySelectorAll('.mdc-data-table')[i]);
        }
    }
});