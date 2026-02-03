// ~/wwwroot/js/memberpg/memberedit.js

// MOBILE NUMBER
document.addEventListener("DOMContentLoaded", function () {
    const contactInput = document.querySelector('input[name="ContactNo"]');
    if (!contactInput) return;

    contactInput.addEventListener("input", function () {
        // Delay ensures paste content is fully applied
        setTimeout(() => {
            // Keep only digits
            this.value = this.value.replace(/[^0-9]/g, "").slice(0, 11);
        }, 0);
    });
});
