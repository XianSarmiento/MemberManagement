document.addEventListener("DOMContentLoaded", function () {
    let isDirty = false;

    // 1. SELECTORS
    const contactInput = document.getElementById("ContactNo");
    const birthDateInput = document.querySelector('input[name="BirthDate"]');
    const ageBadge = document.getElementById('age-badge');
    const emailInput = document.querySelector('input[name="EmailAddress"]');
    const form = document.querySelector('form');
    const cancelLink = document.querySelector('a.btn-create-cancel');

    // 2. INITIALIZE UNOBTRUSIVE VALIDATION
    if (form) {
        $.validator.unobtrusive.parse(form);
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
            let val = this.value.trim().toLowerCase();

            // Automatic @gmail.com logic
            if (val && !val.includes("@")) {
                val += "@gmail.com";
            }

            this.value = val;

            if (typeof $(this).valid === "function") {
                $(this).valid();
            }
        });

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
                submitBtn.disabled = false;
                // If validation fails, don't clear the errors
                if (submitBtn.innerHTML.includes("Processing")) {
                    const headerText = document.querySelector('.card-header-name').innerText;
                    submitBtn.innerHTML = headerText.includes("Edit") ? 'Update Profile' : 'Register Member';
                }
                if (cancelLink) cancelLink.style.display = 'flex';
            }
        });
    }

    // 8. DIRTY CHECKING
    document.querySelectorAll('input, select, textarea').forEach(el => {
        el.addEventListener('change', () => isDirty = true);
    });

    window.addEventListener('beforeunload', (e) => {
        if (isDirty) {
            e.preventDefault();
            e.returnValue = '';
        }
    });

    // 9. AGE CALCULATION & STICKY VALIDATION
    if (birthDateInput && ageBadge) {
        const calculateAndDisplayAge = function (triggerValidation = false) {
            if (!birthDateInput.value) {
                ageBadge.style.display = 'none';
                return;
            }

            const birthDate = new Date(birthDateInput.value);
            const today = new Date();
            if (isNaN(birthDate.getTime())) return;

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

            if (triggerValidation && typeof $(birthDateInput).valid === "function") {
                $(birthDateInput).valid();
            }
        };

        birthDateInput.addEventListener("input", () => calculateAndDisplayAge(false));
        birthDateInput.addEventListener("blur", () => calculateAndDisplayAge(true));

        if (birthDateInput.value) {
            calculateAndDisplayAge(false);
        }
    }
});