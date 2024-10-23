document.addEventListener("DOMContentLoaded", function () {
    const successMessage = '@TempData["SuccessMessage"]';
    const errorMessage = '@TempData["ErrorMessage"]';

    if (successMessage) {
        alert(successMessage);
    } else if (errorMessage) {
        alert(errorMessage);
    }
});