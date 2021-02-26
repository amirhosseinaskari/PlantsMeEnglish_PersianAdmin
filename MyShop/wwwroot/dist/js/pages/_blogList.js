//======load layout javascript======
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.topMenuFunction();
    layout.mobileMenuCategoryNavigation();
    layout.menuCategoryNavigation();
    layout.passwordVisible();
    layout.convertPersianNumbersToEnglish();
})();
//commafy / ajaxLoader 
import { setComma, ajaxLoader, scrollToTop, lazyLoading } from './_layout.js';

setComma();
lazyLoading();
//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();
//desktop filter box
let desktopFilterBox = document.querySelector('#filters-container');
//mobile filter box
let mobileFilterBox = document.querySelector('#mobile-filters-container');
//mobile blog sort
let mobileBlogSort = document.getElementById('mobile-sortList-options');
let mobileSortListItems = mobileBlogSort.querySelectorAll('.sortList-options__list-item');
//desktop blog sort
let desktopBlogSort = document.getElementById('desktop-blogs-sort');
let desktopSortListItems = desktopBlogSort.querySelectorAll('.sortList-options__list-item');

function sortAjaxErrorHandling(e) {
    console.log(e);

}
//pagination buttons clicked
let paginationFire = async function () {
    let paginationButtons = document.querySelectorAll('.pagination--button');
    paginationButtons.forEach(el => {
        el.addEventListener('click', async (e) => {
            e.preventDefault();
            e.stopPropagation();
         
            //selected sort item
            let desktopSelectedSortItem = desktopBlogSort.querySelector('.sortList-options__list-item--selected');
            //set page index
            let myPageIndex = el.getAttribute('data-page-index');
            await blogSort(desktopSelectedSortItem, myPageIndex, desktopFilterBox, "Desktop");
        }, false);
    });
};
(async () => {
    await paginationFire();
})();

//blog sort function 
async function blogSort(elem, pageIndex, filterBox, device) {
    //get value of sort option selected
    let sort = elem.getAttribute('data-value');
    //category blog list element
    let categoryBlogListElem = document.querySelector('.category-detail__blog-list');

    //get search text
    let searchText = null;
    if (device === "Desktop") {
        searchText = document.querySelector('#desktop-blog-search-text').value;
    } else {
        searchText = document.querySelector('#mobile-blog-search-text').value;
    }
    //set sort value to category blog list element
    categoryBlogListElem.setAttribute('data-sort-value', sort);
    //get all cats
    let cats = filterBox.querySelectorAll('input[name="blogCategoryCheckbox"]');
    //get selected categories
    let selectedCats = filterBox.querySelectorAll('input[name="blogCategoryCheckbox"]:checked');
    //array of selected category ids
    let catIds = null;
    if (selectedCats.length > 0) {
        catIds = Array.prototype.map.call(selectedCats, currentCat => {
            return currentCat.getAttribute('data-id');
        });
    }
   
    //data of ajax request
    let mydata = null;
    if (device === "Desktop") {
       //set mobile values according to desktop values
        cats.forEach(current => {
            let dataId = current.getAttribute('data-id');
            mobileFilterBox.querySelector(`input[name="blogCategoryCheckbox"][data-id="${dataId}"]`).checked = current.checked;
        });


        let traditionalCatIds = null;
        if (catIds != null) {
            traditionalCatIds = "";
            catIds.forEach(currentId => {
                traditionalCatIds += `blogCatIds=${currentId}&`;
            });
        }
        //create data object for ajax
        mydata = `${traditionalCatIds}pageIndex=${pageIndex}
        &searchText=${searchText}
        &sort=${sort}`;
        console.log(mydata);
    } else {
       //set desktop values according to mobile values
        cats.forEach(current => {
            let dataId = current.getAttribute('data-id');
            desktopFilterBox.querySelector(`input[name="blogCategoryCheckbox"][data-id="${dataId}"]`).checked = current.checked;
        });
      
        //create data object for ajax
       
        let traditionalCatIds = null;
        if (catIds != null) {
            traditionalCatIds = "";
            catIds.forEach(currentId => {
                traditionalCatIds += `blogCatIds=${currentId}&`;
            });
        }
       
        mydata = `${traditionalCatIds}pageIndex=${pageIndex}
        &searchText=${searchText}
        &sort=${sort}`;
    }

    //********When Ajax Action Done***********
    //send ajax request and put result in category product list element
    let done = (currentElem = elem) => {
        //remove all selected class from desktop sort items
        desktopSortListItems.forEach(listItem => {
            listItem.classList.remove('sortList-options__list-item--selected');
        });
        //remove all selected class from mobile sort items
        mobileSortListItems.forEach(listItem => {
            listItem.classList.remove('sortList-options__list-item--selected');
        });
        //add selected class to this clicked element
        let myDataValue = currentElem.getAttribute('data-value');
        let targetElements = `.sortList-options__list-item[data-value="${myDataValue}"]`;
        let sameElementInMobileAndDesktop = document.querySelectorAll(targetElements);
        sameElementInMobileAndDesktop.forEach(listItem => {
            listItem.classList.add('sortList-options__list-item--selected');
            if (listItem.querySelector('input[type="radio"]')) {
                listItem.querySelector('input[type="radio"]').checked = true;
            }
        });
  
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
    };
    //ajax request 
    await ajaxLoader("POST", '/Blog/BlogListComponent', mydata, sortAjaxErrorHandling, done, '.category-detail__blog-list', true);

}
//call blog sort for desktop sort list
desktopSortListItems.forEach(elem => {
    elem.classList.remove('.sortList-options__list-item--selected');
    elem.addEventListener('click', async (e) => {
        await blogSort(elem, 1, desktopFilterBox, "Desktop");
    }, false);

});
desktopSortListItems[0].classList.add('.sortList-options__list-item--selected');
//call blog sort for mobile list
mobileSortListItems.forEach(elem => {
    elem.classList.remove('.sortList-options__list-item--selected');
    elem.addEventListener('click', async (e) => {
        await blogSort(elem, 1, mobileFilterBox, "Mobile");
    }, false);
});
mobileSortListItems[0].classList.add('.sortList-options__list-item--selected');


//when options changed in filter box 
let runFiltering = (async function () {
    //category blog list element
    let viewComponentElement = document.querySelector('.category-detail__blog-list');
  
    //desktop filter box 
    let desktopFilterBox = document.querySelector('#filters-container');
    //selected sort item
    let desktopSelectedSortItem = desktopBlogSort.querySelector('.sortList-options__list-item--selected');
    //categories in desktop filter box
    let desktopCategoiesFilter = desktopFilterBox.querySelectorAll('input[name="blogCategoryCheckbox"]');
    //when desktop categories changed
    desktopCategoiesFilter.forEach(elem => {
        elem.addEventListener('change', async (e) => {
            desktopSelectedSortItem = desktopBlogSort.querySelector('.sortList-options__list-item--selected');
            await blogSort(desktopSelectedSortItem, 1, desktopFilterBox, "Desktop");
        }, false);
    });
  
    //mobile filter box
    let mobileFilterBox = document.querySelector('#mobile-filters-container');
    //selected sort item
    let mobileSelectedSortItem = mobileBlogSort.querySelector('.sortList-options__list-item--selected');
  
    //categories in mobile filter box
    let mobileCategoiesFilter = mobileFilterBox.querySelectorAll('input[name="blogCategoryCheckbox"]');
   
  
    //when mobile filter box submit clicked
    let mobileFilterBoxSubmit = mobileFilterBox.querySelector('.filter-submit--button');

    mobileFilterBoxSubmit.addEventListener('click', async e => {
        await blogSort(mobileSelectedSortItem, 1, mobileFilterBox, "Mobile");
    }, false);
})();

//reset filters 
function resetFilters() {

    //category blog list element
    let viewComponentElement = document.querySelector('.category-detail__blog-list');
    //desktop filter box 
    let desktopFilterBox = document.querySelector('#filters-container');
    //categories in desktop filter box
    let desktopCategoiesFilter = desktopFilterBox.querySelectorAll('input[name="blogCategoryCheckbox"]');
    //when desktop categories changed
    desktopCategoiesFilter.forEach(elem => {
        elem.checked = false;
    });
   
    //===================================================
   
    //mobile filter box
    let mobileFilterBox = document.querySelector('#mobile-filters-container');
   
    //mobile categories reset
    let mobileCategoiesFilter = mobileFilterBox.querySelectorAll('input[name="categoryCheckbox"]');
    mobileCategoiesFilter.forEach(elem => {
        elem.checked = false;
    });
   
}

//desktop filter reset button
document.querySelector('#desktop-filter-reset').addEventListener('click', async e => {
    e.preventDefault();
    e.stopPropagation();
    resetFilters();
    //search text in desktop filter box
    document.querySelector('#desktop-blog-search-text').value = null;
    //selected sort item
    let desktopSelectedSortItem = desktopBlogSort.querySelector('.sortList-options__list-item--selected');
    await blogSort(desktopSelectedSortItem, 1, desktopFilterBox, "Desktop");
}, false);

//mobile filter reset button
document.querySelector('#mobile-filter-reset').addEventListener('click', async e => {
    e.preventDefault();
    e.stopPropagation();
    resetFilters();
    //search text in mobile filter box
    document.querySelector('#mobile-blog-search-text').value = null;
    //selected sort item
    let mobileSelectedSortItem = mobileBlogSort.querySelector('.sortList-options__list-item--selected');
    await blogSort(mobileSelectedSortItem, 1, mobileFilterBox, "Mobile");
}, false);


//when desktop blog search button clicked
let desktopBlogSearchBtn = document.querySelector('#desktop-blog-search');
desktopBlogSearchBtn.addEventListener('click', async e => {
    e.preventDefault();
    e.stopPropagation();
    resetFilters();
    //selected sort item
    let desktopSelectedSortItem = desktopBlogSort.querySelector('.sortList-options__list-item--selected');
    await blogSort(desktopSelectedSortItem, 1, desktopFilterBox, "Desktop");
}, false);

//when moble blog search button clicked
let mobileBlogSearchBtn = document.querySelector('#mobile-blog-search');
mobileBlogSearchBtn.addEventListener('click', async e => {
    e.preventDefault();
    e.stopPropagation();
    resetFilters();
    //selected sort item
    let mobileSelectedSortItem = mobileBlogSort.querySelector('.sortList-options__list-item--selected');
    await blogSort(mobileSelectedSortItem, 1, mobileFilterBox, "Mobile");
}, false);