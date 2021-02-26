function fadeOut(elem,time) {
    elem.classList.add('fadeout');
    setTimeout(() => {
        elem.style.cssText = `visibility:hidden`;
    }, time);
}

function fadeIn(elem, time) {
    elem.style.cssText = `visibility:visible`;
    elem.classList.add('fadein');
}

export { fadeOut, fadeIn };