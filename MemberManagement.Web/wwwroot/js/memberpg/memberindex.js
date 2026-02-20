// ~/wwwroot/js/memberpg/memberindex.js

// DELETE MODAL DYNAMIC DATA
var deleteModal = document.getElementById('deleteModal');
if (deleteModal) {
    deleteModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        if (!button) return;

        var memberId = button.getAttribute('data-id') ?? '';
        var memberName = button.getAttribute('data-name') ?? '';

        deleteModal.querySelector('#memberName').textContent = memberName;
        deleteModal.querySelector('#deleteMemberId').value = memberId;
        deleteModal.querySelector('#deleteForm').action = '/Members/Delete';
    });
}

// RESTORE MODAL DYNAMIC DATA
var restoreModal = document.getElementById('restoreModal');
if (restoreModal) {
    restoreModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        if (!button) return;

        var memberId = button.getAttribute('data-id') ?? '';
        var memberName = button.getAttribute('data-name') ?? '';

        var displayName = restoreModal.querySelector('#restoreMemberName');
        var inputId = restoreModal.querySelector('#restoreMemberId');

        if (displayName) displayName.textContent = memberName;
        if (inputId) inputId.value = memberId;
    });
}

// EDIT MODAL CHANGE DETECTION
var editModal = document.getElementById('editModal');
if (editModal) {
    let originalValues = {};
    editModal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        if (!button) return;

        var memberId = button.getAttribute('data-id') ?? '';
        var firstName = button.getAttribute('data-firstname') ?? '';
        var lastName = button.getAttribute('data-lastname') ?? '';
        var branch = button.getAttribute('data-branch') ?? '';
        var email = button.getAttribute('data-email') ?? '';
        var contact = button.getAttribute('data-contact') ?? '';

        document.getElementById('editMemberId').value = memberId;
        document.getElementById('editFirstName').value = firstName;
        document.getElementById('editLastName').value = lastName;
        document.getElementById('editBranch').value = branch;
        document.getElementById('editEmail').value = email;
        document.getElementById('editContact').value = contact;

        originalValues = { firstName, lastName, branch, email, contact };
    });

    const editForm = editModal.querySelector('form');
    if (editForm) {
        editForm.addEventListener('submit', function (e) {
            const currentValues = {
                firstName: document.getElementById('editFirstName').value.trim(),
                lastName: document.getElementById('editLastName').value.trim(),
                branch: document.getElementById('editBranch').value.trim(),
                email: document.getElementById('editEmail').value.trim(),
                contact: document.getElementById('editContact').value.trim()
            };

            const noChanges = Object.keys(originalValues).every(
                key => originalValues[key] === currentValues[key]
            );

            if (noChanges) {
                e.preventDefault();
                const modalInstance = bootstrap.Modal.getInstance(editModal);
                if (modalInstance) modalInstance.hide();

                const alertDiv = document.createElement('div');
                alertDiv.id = 'successAlert';
                alertDiv.className = 'alert alert-success text-center operation-alert';
                alertDiv.innerHTML = '<i class="fa-solid fa-circle-check"></i> No changes detected';
                document.body.appendChild(alertDiv);

                setTimeout(() => {
                    alertDiv.style.animation = 'fadeOut 0.5s forwards';
                    setTimeout(() => alertDiv.remove(), 500);
                }, 3000);
            }
        });
    }
}

// FLOATING ALERT AUTO-REMOVE
window.addEventListener('DOMContentLoaded', (event) => {
    document.querySelectorAll('#successAlert, #errorAlert').forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});

// COPY TO CLIPBOARD FUNCTIONALITY
function copyToClipboard(text, element) {
    if (!text || text.trim() === "") return;

    if (navigator.clipboard && window.isSecureContext) {
        navigator.clipboard.writeText(text).then(() => showCopyFeedback(element));
    } else {
        let textArea = document.createElement("textarea");
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        try {
            document.execCommand('copy');
            showCopyFeedback(element);
        } catch (err) {
            console.error('Fallback copy failed', err);
        }
        document.body.removeChild(textArea);
    }
}

function showCopyFeedback(element) {
    const icon = element.querySelector('.copy-icon');
    if (icon) {
        icon.className = 'bi bi-check-lg text-success ms-2 copy-icon';
        icon.style.opacity = '1';

        setTimeout(() => {
            icon.className = 'bi bi-copy text-primary ms-2 copy-icon';
            icon.style.opacity = '';
        }, 1500);
    }
}

// UI INTERACTION HANDLERS
document.addEventListener("DOMContentLoaded", function () {
    // 1. Handle Row Click for Details
    const rows = document.querySelectorAll(".clickable-row");
    rows.forEach(row => {
        row.addEventListener("click", function (e) {
            if (e.target.closest('.copy-cell') ||
                e.target.closest('button') ||
                e.target.closest('.edit-trigger-cell')) {
                return;
            }
            const detailsUrl = this.getAttribute("data-details-url");
            if (detailsUrl) window.location.href = detailsUrl;
        });
    });

    // 2. Handle ID Cell Click for Edit
    const editCells = document.querySelectorAll(".edit-trigger-cell");
    editCells.forEach(cell => {
        cell.addEventListener("click", function (e) {
            e.stopPropagation();
            const editUrl = this.getAttribute("data-edit-url");
            if (editUrl) window.location.href = editUrl;
        });
    });

    // 3. Handle Status Button Text Toggle (ACTIVE -> DEACTIVATE)
    const statusButtons = document.querySelectorAll(".btn-status-active");
    statusButtons.forEach(btn => {
        btn.addEventListener("mouseenter", function () {
            this.textContent = "DEACTIVATE";
        });
        btn.addEventListener("mouseleave", function () {
            this.textContent = "ACTIVE";
        });
    });

    // Change "INACTIVE" to "ACTIVATE" on hover
    document.querySelectorAll('.btn-status-deactivate').forEach(button => {
        button.addEventListener('mouseenter', function () {
            this.textContent = 'ACTIVATE';
        });
        button.addEventListener('mouseleave', function () {
            this.textContent = 'INACTIVE';
        });
    });
});