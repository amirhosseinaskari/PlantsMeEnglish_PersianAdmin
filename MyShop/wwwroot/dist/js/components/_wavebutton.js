
//wave button
export async function waveButton() {
    let waveButtons = document.querySelectorAll('.wave-button');
    waveButtons.forEach(item => {
        item.addEventListener('click', e => {
            let myWaveButton = item.querySelector('.riggle');
            if (myWaveButton !== null) {
                myWaveButton.remove();
            }
            let newWaveBtn = document.createElement('DIV');
            newWaveBtn.classList.add('riggle');
            let rect = item.getBoundingClientRect();
            let parentOffsetTop = rect.top;
            let parentOffsetLeft = rect.left;
            let relX = e.clientX - parentOffsetLeft;
            let relY = e.clientY - parentOffsetTop;
            item.appendChild(newWaveBtn);
            newWaveBtn.style.top = relY + "px";
            newWaveBtn.style.left = relX + "px";
            
        }, false);
    });
}

//ripple
export async function ripple() {
    let rippleElems = document.querySelectorAll('.ripple-input');
    rippleElems.forEach(item => {
        item.addEventListener('click', e => {
            let myRipple = item.querySelector('.ripple');
            if (myRipple !== null) {
                myRipple.remove();
            }
            let newRipple = document.createElement('DIV');
            newRipple.classList.add('ripple');
            item.appendChild(newRipple);
        }, false);
    });

    let formModals = document.querySelectorAll('.form-modal');
    formModals.forEach(item => {
        item.addEventListener('hidden', e => {
            let myripple = item.querySelector('.ripple');
            if (myripple !== null) {
                myripple.remove();
            }
        }, false);
    });
}