
//load layout javascript
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.topMenuFunction();
    layout.persianNumberParse();
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
import { persianNumberParse, scrollToTop, lazyLoading} from './_layout.js';
lazyLoading();