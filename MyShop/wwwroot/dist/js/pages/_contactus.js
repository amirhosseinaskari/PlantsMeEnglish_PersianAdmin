//load layout javascript
(async function loadLayout() {
    let layout = await import('./_layout.js');
    layout.topMenuFunction();
    layout.mobileMenuCategoryNavigation();
    layout.menuCategoryNavigation();
    layout.passwordVisible();
    layout.convertPersianNumbersToEnglish();
})();
//lazyloading / persian numbers parse
import { persianNumberParse, lazyLoading } from './_layout.js';
persianNumberParse();
lazyLoading();
//load wave button component
(async function loadButtonComponent() {
    let bComponent = await import('../components/_wavebutton.js');
    bComponent.waveButton();
    bComponent.ripple();
})();

