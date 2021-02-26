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


//add new blog comment
(async function addNewBlogComment() {
    let blogStarsContainer = document.querySelector('.blog-stars-container');
    if (!blogStarsContainer) {
        return false;
    }
    let starsInput = blogStarsContainer.querySelectorAll('input[name="star"]');
    starsInput.forEach(elem => {
        elem.addEventListener('change', (e) => {
            document.querySelector('.rate-result').innerHTML = elem.value;
           
            blogStarsContainer.setAttribute('data-rate-number', elem.value);
        }, false);
    });
    //when blog submit comment clicked
    let blogSubmitButton = document.querySelector('.blog-submit-comment--button');
    blogSubmitButton.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        //disable blog submit button after click until ajax response
        blogSubmitButton.setAttribute('disabled', 'disabled');
        //get blog id
        let blogId = blogSubmitButton.getAttribute('data-id');
        //get comment description
        let description = document.querySelector('textarea[name="description"]').value;
        //get rate number
        let rate = blogStarsContainer.getAttribute('data-rate-number');
        //show error message if comment description is null
        if (description === null || description.trim() === "") {
            let errorCommentDescription = document.querySelector('.error-comment-description');
            errorCommentDescription.classList.remove('d-none');
            errorCommentDescription.innerHTML = 'Please fill this field';
            return false;
        }
        //after success ajax response
        let doneAjax = function () {
            //hide comment modal
            UIkit.modal('#comment-add-modal').hide();
            //remove blog submit button disabled attribute
            blogSubmitButton.removeAttribute('disabled');

            return true;
        };
        //ajax error handling function
        let errorHandling = (e) => {
            console.log(e);
        };
        //data for sending by ajax
        let mydata = `blogId=${blogId}&description=${description}&rate=${rate}`;
        //send data by ajax to /BlogComment/AddNewComment
        let result = await ajaxLoader('POST', '/BlogComment/AddNewComment', mydata, errorHandling, doneAjax, null, false);
        if (result === '101') {
            //show notification for success response after ajax done
            UIkit.notification({
                message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Your comment submited</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 3000
            });
        } else if (result === '102') {

            UIkit.notification({
                message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Your comment has submited</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 3000
            });
        }
    }, false);
    //remove disabled when click on add new comment
    document.querySelector('.add-new-comment').addEventListener('click', (e) => {
        blogSubmitButton.removeAttribute('disabled');
        document.querySelector('textarea[name="description"]').value = null;

    }, false);
    //reset error for comment description when focused
    document.querySelector('textarea[name="description"]').addEventListener('focus', (e) => {
        let errorCommentDescription = document.querySelector('.error-comment-description');
        errorCommentDescription.classList.add('d-none');
        errorCommentDescription.innerHTML = '';
        blogSubmitButton.removeAttribute('disabled');
    }, false);
})();

//add new reply blog comment
(async function addNewReplyComment() {
    //Reply to comment
    let blogReplyCommentTrigger = document.querySelector('.blog-reply-comment-trigger');
    let blogReplyCommentSubmitButton = document.querySelector('.blog-submit-reply-comment--button');
    if (!blogReplyCommentTrigger || !blogReplyCommentSubmitButton) {
        return false;
    }
    //when comment reply button clicked
    blogReplyCommentTrigger.addEventListener('click', (e) => {
        let commentId = blogReplyCommentTrigger.getAttribute('data-commentId');
        blogReplyCommentSubmitButton.setAttribute('data-commentId', commentId);
    }, false);
    //when reply comment submited
    blogReplyCommentSubmitButton.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        blogReplyCommentSubmitButton.setAttribute('disabled', 'disabled');
        //get reply comment description
        let description = document.querySelector('textarea[name="replyDescription"]').value;
        //show error message if description is null
        if (description === null || description.trim() === "") {
            let errorReplyDescription = document.querySelector('.error-reply-description');
            errorReplyDescription.classList.remove('d-none');
            errorReplyDescription.innerHTML = "Please fill this field";
            return false;
        }
        //get comment id
        let commentId = blogReplyCommentSubmitButton.getAttribute('data-commentId');
        //data for sending by ajax
        let mydata = `description=${description}&commentId=${commentId}`;
        //after success ajax response
        let doneAjax = function () {
            //hide comment modal
            UIkit.modal('#reply-comment-add-modal').hide();
            //remove blog submit button disabled attribute
            blogReplyCommentSubmitButton.removeAttribute('disabled');

            return true;
        };
        //ajax error handling function
        let errorHandling = (e) => {
            console.log(e);
        };
        //send data by ajax to /BlogComment/AddReplyComment
        let result = await ajaxLoader('POST', '/BlogComment/AddReplyComment', mydata, errorHandling, doneAjax, null, false);
        if (result === '101') {
            //show notification for success response after ajax done
            UIkit.notification({
                message: '<span class="margin-right-10 position-relative" uk-icon="icon:bell;ratio:1.2"></span><span class="dana-font fontsize-14" dir="ltr">Your comment submited</span>',
                status: 'primary',
                pos: 'top-center',
                timeout: 3000
            });
        }
    }, false);
    //remove disabled attribute when click on add new reply comment
    blogReplyCommentTrigger.addEventListener('click', (e) => {
        blogReplyCommentSubmitButton.removeAttribute('disabled');
        document.querySelector('textarea[name="replyDescription"]').value = null;
    }, false);
    //reset error for comment description when focused
    document.querySelector('textarea[name="replyDescription"]').addEventListener('focus', (e) => {
        let errorCommentDescription = document.querySelector('.error-reply-description');
        errorCommentDescription.classList.add('d-none');
        errorCommentDescription.innerHTML = '';
        blogReplyCommentSubmitButton.removeAttribute('disabled');
    }, false);
})();

//blog comments (pagination buttons)
(function blogCommentsPagination() {
    let commentPaginationContainer = document.querySelector('#blog-comment-list__pageination-buttons-container');
    if (!commentPaginationContainer) {
        return false;
    }
    let commentPaginationButtons = commentPaginationContainer.querySelectorAll('.pagination--button');
    let doneAjax = function () {
        window.scrollTo({
            top: document.querySelector('#blog-comments-container').offsetTop - 120,
            behavior: 'smooth'
        });
        blogCommentsPagination();
   
        addNewBlogComment();
        addNewReplyComment();
        return true;
    };
    //ajax error handling function
    let errorHandling = (e) => {
        console.log(e);
    };
    commentPaginationButtons.forEach(elem => {
        elem.addEventListener('click', async (e) => {
            e.preventDefault();
            e.stopPropagation();
            let blogId = commentPaginationContainer.getAttribute('data-blog-id');
            let pageIndex = elem.getAttribute('data-page-index');
            let data = `blogId=${blogId}&pageIndex=${pageIndex}`;
            await ajaxLoader('POST', '/Blog/BlogCommentList', data, errorHandling, doneAjax, '#blog-comments-container', true);
        }, false);
    });

})();
