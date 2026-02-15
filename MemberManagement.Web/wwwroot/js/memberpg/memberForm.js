document.addEventListener("DOMContentLoaded", function () {
    let isDirty = false;

    // 1. SELECTORS
    const contactInput = document.getElementById("ContactNo");
    const birthDateInput = document.querySelector('input[name="BirthDate"]');
    const ageBadge = document.getElementById('age-badge');
    const emailInput = document.querySelector('input[name="EmailAddress"]');
    const form = document.querySelector('form');
    const cancelLink = document.querySelector('a.btn-create-cancel');

    // 2. INITIALIZE UNOBTRUSIVE VALIDATION (Critical for Edit views)
    if (form) {
        // Force jQuery to re-parse the form attributes
        $.validator.unobtrusive.parse(form);

        // Ensure validation triggers as soon as user leaves a field
        const validator = $(form).validate();
        validator.settings.onfocusout = function (element) {
            $(element).valid();
        };
    }

    // 3. CONTACT NUMBER FORMATTING
    if (contactInput) {
        contactInput.addEventListener("input", function () {
            this.value = this.value.replace(/\D/g, "").slice(0, 11);
        });
    }

    // 4. AUTO-FORMAT NAMES
    document.querySelectorAll('input[name="FirstName"], input[name="LastName"]').forEach(input => {
        input.addEventListener("blur", function () {
            if (this.value) {
                this.value = this.value.trim();
                this.value = this.value.charAt(0).toUpperCase() + this.value.slice(1);
            }
        });
    });

    // 5. EMAIL FORMATTING & IMMEDIATE VALIDATION
    if (emailInput) {
        emailInput.addEventListener("blur", function () {
            this.value = this.value.trim().toLowerCase();
            // Trigger the red cloud immediately on blur
            if (typeof $(this).valid === "function") {
                $(this).valid();
            }
        });

        // If email is pre-filled (Edit View) and invalid, show error after a short delay
        if (emailInput.value && typeof $(emailInput).valid === "function") {
            setTimeout(() => { $(emailInput).valid(); }, 600);
        }
    }

    // 6. CANCEL BUTTON NAVIGATION
    if (cancelLink) {
        cancelLink.addEventListener("click", function () {
            isDirty = false;
        });
    }

    // 7. FORM SUBMISSION LOGIC
    if (form) {
        form.addEventListener("submit", function (e) {
            const submitBtn = this.querySelector('button[type="submit"]');

            if ($(this).valid()) {
                isDirty = false;
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fa-solid fa-circle-notch fa-spin"></i>&nbsp;&nbsp;Processing...';

                if (cancelLink) cancelLink.style.display = 'none';
                submitBtn.style.width = '30%';
            } else {
                // Restore button state if validation fails
                submitBtn.disabled = false;
                if (submitBtn.innerHTML.includes("Processing")) {
                    const headerText = document.querySelector('.card-header-name').innerText;
                    submitBtn.innerHTML = headerText.includes("Edit") ? 'Update Profile' : 'Register Member';
                }
                if (cancelLink) cancelLink.style.display = 'flex';
            }
        });
    }

    // 8. DIRTY CHECKING (Unsaved changes alert)
    document.querySelectorAll('input, select, textarea').forEach(el => {
        el.addEventListener('change', () => isDirty = true);
    });

    window.addEventListener('beforeunload', (e) => {
        if (isDirty) {
            e.preventDefault();
            e.returnValue = '';
        }
    });

    // 9. AGE CALCULATION
    if (birthDateInput && ageBadge) {
        const calculateAndDisplayAge = function () {
            if (!birthDateInput.value) {
                ageBadge.style.display = 'none';
                return;
            }

            const birthDate = new Date(birthDateInput.value);
            const today = new Date();
            let ageYears = today.getFullYear() - birthDate.getFullYear();
            let ageMonths = today.getMonth() - birthDate.getMonth();
            let ageDays = today.getDate() - birthDate.getDate();

            if (ageDays < 0) {
                ageMonths--;
                const prevMonth = new Date(today.getFullYear(), today.getMonth(), 0).getDate();
                ageDays += prevMonth;
            }
            if (ageMonths < 0) {
                ageYears--;
                ageMonths += 12;
            }

            ageBadge.style.display = 'inline-block';
            ageBadge.innerText = `Age: ${ageYears}y, ${ageMonths}m, ${ageDays}d`;

            const isTooYoung = ageYears < 18;
            const isTooOld = ageYears > 65 || (ageYears === 65 && ageMonths > 6) || (ageYears === 65 && ageMonths === 6 && ageDays > 1);

            ageBadge.className = (isTooYoung || isTooOld) ? "badge bg-danger mt-1" : "badge bg-success mt-1";

            if (typeof $(birthDateInput).valid === "function") {
                $(birthDateInput).valid();
            }
        };

        birthDateInput.addEventListener("input", calculateAndDisplayAge);
        calculateAndDisplayAge(); // Run once on load for Edit view
    }
});