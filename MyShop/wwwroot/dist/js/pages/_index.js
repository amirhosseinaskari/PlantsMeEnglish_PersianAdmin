
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


//commafy / ajaxLoader / persian numbers parse
import {lazyLoading } from './_layout.js';
lazyLoading();

let videos = document.querySelectorAll('video');
if (videos.length > 0) {
    videos.forEach(el => {
        el.play();
    });
}