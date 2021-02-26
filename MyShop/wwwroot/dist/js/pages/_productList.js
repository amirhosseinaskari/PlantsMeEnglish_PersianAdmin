//======load layout javascript======
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
import { setComma, ajaxLoader, scrollToTop, lazyLoading } from './_layout.js';

setComma();
lazyLoading();
//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();

//load product card component
import { default as productCardComponent } from '../components/_productcard.js';
productCardComponent.return;

//======range slider======
//range slider in desktop 
let desktopFilterBox = document.querySelector('#filters-container');
//range slider in mobile
let mobileFilterBox = document.querySelector('#mobile-filters-container');
//activation range slider in desktop mode
rangeSlider(desktopFilterBox);
//activation range slider in mobile mode
rangeSlider(mobileFilterBox);
function rangeSlider(elem) {
    let inputLeft = elem.querySelector(".input-left");
    let inputRight = elem.querySelector(".input-right");
    let range = elem.querySelector(".slider > .range");
    let priceFrom = elem.querySelector(".price-from");
    let priceTo = elem.querySelector(".price-to");

    function setLeftValue() {
        let _this = inputLeft,
            min = parseInt(_this.min),
            max = parseInt(_this.max);

        _this.value = Math.min(
            parseInt(_this.value),
            parseInt(inputRight.value) - 1
        );
        priceFrom.textContent = setComma(_this.value);
        
        let percent = ((_this.value - min) / (max - min)) * 100;

        range.style.left = percent + "%";
    }

    setLeftValue();

    function setRightValue() {
        let _this = inputRight,
            min = parseInt(_this.min),
            max = parseInt(_this.max);

        _this.value = Math.max(parseInt(_this.value), parseInt(inputLeft.value) + 1);
        priceTo.textContent = setComma(_this.value);
       
        let percent = ((_this.value - min) / (max - min)) * 100;

        range.style.right = 100 - percent + "%";
    }

    setRightValue();

    inputLeft.addEventListener("input", setLeftValue);
    inputRight.addEventListener("input", setRightValue);

    inputLeft.addEventListener("mouseover", (e) => {
        inputLeft.classList.add("hover");
    });
    inputLeft.addEventListener("mouseout", (e) => {
        inputLeft.classList.remove("hover");
    });
    inputLeft.addEventListener("mousedown", (e) => {
        inputLeft.classList.add("active");
    });
    inputLeft.addEventListener("mouseup", (e) => {
        inputLeft.classList.remove("active");
    });
    inputLeft.addEventListener("touchstart", (e) => {
        inputLeft.classList.add("active");
    });
    inputLeft.addEventListener("touchend", (e) => {
        inputLeft.classList.remove("active");
    });

    inputRight.addEventListener("mouseover", (e) => {
        inputRight.classList.add("hover");
    });
    inputRight.addEventListener("mouseout", (e) => {
        inputRight.classList.remove("hover");
    });
    inputRight.addEventListener("mousedown", (e) => {
        inputRight.classList.add("active");
    });
    inputRight.addEventListener("mouseup", (e) => {
        inputRight.classList.remove("active");
    });
    inputRight.addEventListener("touchstart", (e) => {
        inputRight.classList.add("active");
    });
    inputRight.addEventListener("touchend", (e) => {
        inputRight.classList.remove("active");
    });
}
//======send request for get products by ajax======

function sortAjaxErrorHandling(e) {
    console.log(e);

}

//mobile product sort
let mobileProductSort = document.getElementById('mobile-sortList-options');
let mobileSortListItems = mobileProductSort.querySelectorAll('.sortList-options__list-item');
//desktop product sort
let desktopProductSort = document.getElementById('desktop-products-sort');
let desktopSortListItems = desktopProductSort.querySelectorAll('.sortList-options__list-item');

//pagination buttons clicked
let paginationFire = async function () {
    let paginationButtons = document.querySelectorAll('.pagination--button');
    if (!paginationButtons) {
        return false;
    }
    paginationButtons.forEach(el => {
        el.addEventListener('click', async (e) => {
            e.preventDefault();
            e.stopPropagation();
            rangeSlider(mobileFilterBox);
            rangeSlider(desktopFilterBox);
            //selected sort item
            let desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
            //set page index
            let myPageIndex = el.getAttribute('data-page-index');
            await productSort(desktopSelectedSortItem, myPageIndex, desktopFilterBox, "Desktop");
        }, false);
    });
};
(async () => {
    await paginationFire();
})();
//product sort function 
async function productSort(elem,pageIndex,filterBox, device) {
    //get value of sort option selected
    let sort = elem.getAttribute('data-value');
    //category product list element
    let categoryProductListElem = document.querySelector('.category-detail__product-list');
    //get default category id
    let defaultCatId = categoryProductListElem.getAttribute('data-cat-id');
    //get default brand id
    let defaultBrandId = categoryProductListElem.getAttribute('data-brand-id');
    //get search text
    let searchText = categoryProductListElem.getAttribute('data-search-text');
    if (searchText !== null) {
        searchText = searchText.trim();
    }
    //set sort value to category product list element
    categoryProductListElem.setAttribute('data-sort-value', sort);
    //get all cats
    let cats = filterBox.querySelectorAll('input[name="categoryCheckbox"]');
    //get selected categories
    let selectedCats = filterBox.querySelectorAll('input[name="categoryCheckbox"]:checked');
    //array of selected category ids
    let catIds = null;
    if (selectedCats.length > 0) {
        catIds = Array.prototype.map.call(selectedCats, currentCat => {
            return currentCat.getAttribute('data-id');
        });
    }
    //get all brands
    let brands = filterBox.querySelectorAll('input[name="brandCheckbox"]');
    //get selected brands
    let selectedBrands = filterBox.querySelectorAll('input[name="brandCheckbox"]:checked');
    //array of selected brand ids
    let brandIds = null;
    if (selectedBrands.length > 0) {
        brandIds = Array.prototype.map.call(selectedBrands, currentBrand => {
            return currentBrand.getAttribute('data-id');
        });
    }

    //set default category id | brand id | search text to -1 if they are empty
    defaultCatId = -1;
    defaultBrandId = -1;
    if (searchText === null) {
        searchText = "";
    } else if (searchText.trim() === "") {
        searchText = "";
    }
    //get range price values on desktop 
    let filtersContainer = document.getElementById('filters-container');
    let mobileFiltersContainer = document.getElementById('mobile-filters-container');
    //price from value
    let priceFromValue = filtersContainer.querySelector('input[name="input-left"]').value;
    //price to value
    let priceToValue = filtersContainer.querySelector('input[name="input-right"]').value;
    //mobile price from value 
    let mobilePriceFromValue = mobileFiltersContainer.querySelector('input[name="input-left"]').value;
    //mobile price to value
    let mobilePriceToValue = mobileFiltersContainer.querySelector('input[name="input-right"]').value;
    //get has free delivery option on desktop
    let hasFreeDelivery = filtersContainer.querySelector('input[name="has_free_delivery"]').checked;
    //get has free delivery option on mobile
    let mobileHasFreeDelivery = mobileFiltersContainer.querySelector('input[name="has_free_delivery"]').checked;
    //get has selling stock on desktop
    let hasSellingStock = filtersContainer.querySelector('input[name="has_selling_stock"]').checked;
    //get has selling stock on mobile
    let mobileHasSellingStock = mobileFiltersContainer.querySelector('input[name="has_selling_stock"]').checked;
    //data of ajax request
    let mydata = null;
    if (device === "Desktop") {
        //set mobile values according to desktop values
        mobileFiltersContainer.querySelector('input[name="input-left"]').value = priceFromValue;
        mobileFiltersContainer.querySelector('input[name="input-right"]').value = priceToValue;
        mobileFiltersContainer.querySelector('input[name="has_free_delivery"]').checked = hasFreeDelivery;
        mobileFiltersContainer.querySelector('input[name="has_selling_stock"]').checked = hasSellingStock;
        rangeSlider(mobileFilterBox);
        cats.forEach(current => {
            let dataId = current.getAttribute('data-id');
            mobileFilterBox.querySelector(`input[name="categoryCheckbox"][data-id="${dataId}"]`).checked = current.checked;
        });
        brands.forEach(current => {
            let dataId = current.getAttribute('data-id');
            mobileFilterBox.querySelector(`input[name="brandCheckbox"][data-id="${dataId}"]`).checked = current.checked;
        });

        let traditionalCatIds = null;
        if (catIds != null) {
            traditionalCatIds = "";
            catIds.forEach(currentId => {
                traditionalCatIds += `catIds=${currentId}&`;
            });
        }
        //create data object for ajax
        mydata = `defaultCategoryId=${defaultCatId}&pageIndex=${pageIndex}
        &defaultBrandId=${defaultBrandId}&brandIds=${brandIds}&searchText=${searchText}
        &sort=${sort}&minValuePrice=${priceFromValue}&maxValuePrice=${priceToValue}
        &hasFreeDelivery=${hasFreeDelivery}&hasSellingStock=${hasSellingStock}&${traditionalCatIds}`;
      
    } else {
        //set desktop values according to mobile values
        filtersContainer.querySelector('input[name="input-left"]').value = mobilePriceFromValue;
        filtersContainer.querySelector('input[name="input-right"]').value = mobilePriceToValue;
        filtersContainer.querySelector('input[name="has_free_delivery"]').checked = mobileHasFreeDelivery;
        filtersContainer.querySelector('input[name="has_selling_stock"]').checked = mobileHasSellingStock;
        rangeSlider(desktopFilterBox);
        cats.forEach(current => {
            let dataId = current.getAttribute('data-id');
            desktopFilterBox.querySelector(`input[name="categoryCheckbox"][data-id="${dataId}"]`).checked = current.checked;
        });
        brands.forEach(current => {
            let dataId = current.getAttribute('data-id');
            desktopFilterBox.querySelector(`input[name="brandCheckbox"][data-id="${dataId}"]`).checked = current.checked;
        });
        let traditionalCatIds = null;
        if (catIds != null) {
            traditionalCatIds = "";
            catIds.forEach(currentId => {
                traditionalCatIds += `catIds=${currentId}&`;
            });
        }
       
        
        //create data object for ajax
        mydata = `defaultCategoryId=${defaultCatId}&pageIndex=${pageIndex}
        &defaultBrandId=${defaultBrandId}&brandIds=${brandIds}&searchText=${searchText}
        &sort=${sort}&minValuePrice=${mobilePriceFromValue}&maxValuePrice=${mobilePriceToValue}
        &hasFreeDelivery=${mobileHasFreeDelivery}&hasSellingStock=${mobileHasSellingStock}&${traditionalCatIds}`;
       
    }

    //********When Ajax Action Done***********
    //send ajax request and put result in category product list element
    let done = () => {
        //remove all selected class from desktop sort items
        desktopSortListItems.forEach(listItem => {
            listItem.classList.remove('sortList-options__list-item--selected');
        });
        //remove all selected class from mobile sort items
        mobileSortListItems.forEach(listItem => {
            listItem.classList.remove('sortList-options__list-item--selected');
        });
        //add selected class to this clicked element
        let myDataValue = elem.getAttribute('data-value');
        let targetElements = `.sortList-options__list-item[data-value="${myDataValue}"]`;
        let sameElementInMobileAndDesktop = document.querySelectorAll(targetElements);
        sameElementInMobileAndDesktop.forEach(listItem => {
            listItem.classList.add('sortList-options__list-item--selected');
            if (listItem.querySelector('input[type="radio"]')) {
                listItem.querySelector('input[type="radio"]').checked = true;
            }
        });
        //product card effect (show second image)
        productCardComponent.inAjaxMode();
        //close mobile filter box modal
        let mobileFilterBoxModal = document.querySelector('#mobile-filter-box__modal');
        UIkit.modal(mobileFilterBoxModal).hide();
        //fire pagination buttons
        (async () => {
            await paginationFire();
        })();
        //scroll to top
        scrollToTop(600);
        //loading images in lazy loading mode
        lazyLoading();
        persianNumberParse();
    };
    //ajax request 
    await ajaxLoader("POST", '/Category/CategoryProductList', mydata, sortAjaxErrorHandling, done, '.category-detail__product-list', true);

}
//call product sort for desktop sort list
desktopSortListItems.forEach(elem => {
    elem.classList.remove('.sortList-options__list-item--selected');
    elem.addEventListener('click', async (e) => {
        await productSort(elem,1, desktopFilterBox, "Desktop");
    }, false);

});
desktopSortListItems[0].classList.add('.sortList-options__list-item--selected');
//call product sort for mobile list
mobileSortListItems.forEach(elem => {
    elem.classList.remove('.sortList-options__list-item--selected');
    elem.addEventListener('click', async (e) => {
        await productSort(elem,1, mobileFilterBox, "Mobile");
    }, false);
});
mobileSortListItems[0].classList.add('.sortList-options__list-item--selected');
//when options changed in filter box 
let runFiltering = (async function () {
    //category product list element
    let viewComponentElement = document.querySelector('.category-detail__product-list');
    //default cat id
    let default_cat_id = viewComponentElement.getAttribute('data-cat-id');
    //default brand id
    let default_brand_id = viewComponentElement.getAttribute('data-brand-id');
    //desktop filter box 
    let desktopFilterBox = document.querySelector('#filters-container');
    //selected sort item
    let desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
    //categories in desktop filter box
    let desktopCategoiesFilter = desktopFilterBox.querySelectorAll('input[name="categoryCheckbox"]');
    //when desktop categories changed
    desktopCategoiesFilter.forEach(elem => {
        elem.checked = false;
        let myCatId = elem.getAttribute('data-id');
        if (myCatId === default_cat_id) {
            elem.checked = true;
        }
        elem.addEventListener('change', async (e) => {
            desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
            await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
        }, false);
    });
    //brands in desktop filter box
    let desktopBrandsFilter = desktopFilterBox.querySelectorAll('input[name="brandCheckbox"]');
    //when desktop brands changed
    desktopBrandsFilter.forEach(elem => {
        elem.checked = false;
        let myBrandId = elem.getAttribute('data-id');
        if (myBrandId === default_brand_id) {
            elem.checked = true;
        }
        elem.addEventListener('change', async (e) => {
            desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
            await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
        }, false);
    });

    //price range changed
    let priceFromInput = desktopFilterBox.querySelector('input[name="input-left"]');
    let priceToInput = desktopFilterBox.querySelector('input[name="input-right"]');
    priceFromInput.value = 0;
    priceToInput.value = viewComponentElement.getAttribute('data-max-value-price');
    
    priceFromInput.addEventListener('change', async e => {
        desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
        await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
    }, false);
    priceToInput.addEventListener('change', async e => {
        desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
        await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
    }, false);
    //has free delivery option 
    let hasDeliveryCheckbox = desktopFilterBox.querySelector('input[name="has_free_delivery"]');
    hasDeliveryCheckbox.checked = false;
    hasDeliveryCheckbox.addEventListener('change', async e => {
        desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
        await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
    }, false);

    //has stock selling option
    let hasStockSellingCheckbox = desktopFilterBox.querySelector('input[name="has_selling_stock"]');
    hasStockSellingCheckbox.checked = false;
    hasStockSellingCheckbox.addEventListener('change', async e => {
        desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
        await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
    }, false);

    //mobile filter box
    let mobileFilterBox = document.querySelector('#mobile-filters-container');
    //selected sort item
    let mobileSelectedSortItem = mobileProductSort.querySelector('.sortList-options__list-item--selected');
    //reset mobile has free delivery
    mobileFilterBox.querySelector('input[name="has_free_delivery"]').checked = false;
    //reset mobile has selling stock 
    mobileFilterBox.querySelector('input[name="has_selling_stock"]').checked = false;
    //reset mobile price range
    mobileFilterBox.querySelector('input[name="input-left"]').value = 0;
    mobileFilterBox.querySelector('input[name="input-right"]').value = viewComponentElement.getAttribute('data-max-value-price');
    //categories in mobile filter box
    let mobileCategoiesFilter = mobileFilterBox.querySelectorAll('input[name="categoryCheckbox"]');
    //mobile categories reset
    mobileCategoiesFilter.forEach(elem => {
        elem.checked = false;
        let myCatId = elem.getAttribute('data-id');
        if (myCatId === default_cat_id) {
            elem.checked = true;
        }
    });
    //reset brands in mobile filter box
    let mobileBrandsFilter = mobileFilterBox.querySelectorAll('input[name="brandCheckbox"]');
    //when desktop brands changed
    mobileBrandsFilter.forEach(elem => {
        elem.checked = false;
        let myCatId = elem.getAttribute('data-id');
        if (myCatId === default_brand_id) {
            elem.checked = true;
        }
    });

    //when mobile filter box submit clicked
    let mobileFilterBoxSubmit = mobileFilterBox.querySelector('.filter-submit--button');
    
    mobileFilterBoxSubmit.addEventListener('click',async e => {
        await productSort(mobileSelectedSortItem,1, mobileFilterBox, "Mobile");
    }, false);
})();


//reset filters 
function resetFilters() {
   
    //category product list element
    let viewComponentElement = document.querySelector('.category-detail__product-list');
    //desktop filter box 
    let desktopFilterBox = document.querySelector('#filters-container');
    //categories in desktop filter box
    let desktopCategoiesFilter = desktopFilterBox.querySelectorAll('input[name="categoryCheckbox"]');
    //when desktop categories changed
    desktopCategoiesFilter.forEach(elem => {
        elem.checked = false;
    });
    //brands in desktop filter box
    let desktopBrandsFilter = desktopFilterBox.querySelectorAll('input[name="brandCheckbox"]');
    //when desktop brands changed
    desktopBrandsFilter.forEach(elem => {
        elem.checked = false;
    });
    //price range changed
    let priceFromInput = desktopFilterBox.querySelector('input[name="input-left"]');
    let priceToInput = desktopFilterBox.querySelector('input[name="input-right"]');
    priceFromInput.value = 0;
    priceToInput.value = viewComponentElement.getAttribute('data-max-value-price');
    //has free delivery option 
    let hasDeliveryCheckbox = desktopFilterBox.querySelector('input[name="has_free_delivery"]');
    hasDeliveryCheckbox.checked = false;
    //has stock selling option
    let hasStockSellingCheckbox = desktopFilterBox.querySelector('input[name="has_selling_stock"]');
    hasStockSellingCheckbox.checked = false;

    //===================================================
  
    //mobile filter box
    let mobileFilterBox = document.querySelector('#mobile-filters-container');
    //reset mobile has free delivery
    mobileFilterBox.querySelector('input[name="has_free_delivery"]').checked = false;
    //reset mobile has selling stock 
    mobileFilterBox.querySelector('input[name="has_selling_stock"]').checked = false;
    //reset mobile price range
    mobileFilterBox.querySelector('input[name="input-left"]').value = 0;
    mobileFilterBox.querySelector('input[name="input-right"]').value = viewComponentElement.getAttribute('data-max-value-price');
   //mobile categories reset
    let mobileCategoiesFilter = mobileFilterBox.querySelectorAll('input[name="categoryCheckbox"]');
    mobileCategoiesFilter.forEach(elem => {
        elem.checked = false;
    });
    //reset brands in mobile filter box
    let mobileBrandsFilter = mobileFilterBox.querySelectorAll('input[name="brandCheckbox"]');
    mobileBrandsFilter.forEach(elem => {
        elem.checked = false;
    });
}

//desktop filter reset button
document.querySelector('#desktop-filter-reset').addEventListener('click',async e => {
    e.preventDefault();
    e.stopPropagation();
    resetFilters();
    rangeSlider(mobileFilterBox);
    rangeSlider(desktopFilterBox);
    //selected sort item
    let desktopSelectedSortItem = desktopProductSort.querySelector('.sortList-options__list-item--selected');
    await productSort(desktopSelectedSortItem,1, desktopFilterBox, "Desktop");
}, false);

//mobile filter reset button
document.querySelector('#mobile-filter-reset').addEventListener('click',async e => {
    e.preventDefault();
    e.stopPropagation();
    resetFilters();
    rangeSlider(mobileFilterBox);
    rangeSlider(desktopFilterBox);
    //selected sort item
    let mobileSelectedSortItem = mobileProductSort.querySelector('.sortList-options__list-item--selected');
    await productSort(mobileSelectedSortItem,1, mobileFilterBox, "Mobile");
}, false);

