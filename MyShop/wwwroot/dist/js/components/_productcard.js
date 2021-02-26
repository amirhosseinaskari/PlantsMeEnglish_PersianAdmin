

//show second image in product card when hover on it
 function showSecondImage(e) {
    e.preventDefault();
    let elem = e.target;
    let image = elem.querySelector('img');
    let secondImageSrc = image.getAttribute('data-second-image');

    if (secondImageSrc !== null && secondImageSrc !== "") {

        image.src = secondImageSrc;
        e.stopPropagation();
    }

}

 function showFirstImage(e) {
    e.preventDefault();
    let elem = e.target;
    let image = elem.querySelector('img');
    let firstImageSrc = image.getAttribute('data-src');

    if (firstImageSrc !== null && firstImageSrc !== "") {

        image.src = firstImageSrc;
        e.stopPropagation();

    }
}
 let productCardImages = document.querySelectorAll('li.product-card-container');
productCardImages.forEach(function (elem) {
    elem.addEventListener('mouseenter', showSecondImage, false);
    elem.addEventListener('mouseleave', showFirstImage, false);
});

export default {
    methods: [showSecondImage, showFirstImage],
    elems: productCardImages,
    return: productCardImages.forEach(function (elem) {
        elem.addEventListener('mouseenter', showSecondImage, false);
        elem.addEventListener('mouseleave', showFirstImage, false);
    }),
    inAjaxMode: () => {
        let myProCardImages = document.querySelectorAll('li.product-card-container');
        myProCardImages.forEach(function (elem) {
            elem.addEventListener('mouseenter', showSecondImage , false);
            elem.addEventListener('mouseleave', showFirstImage, false);
        });

    }
};