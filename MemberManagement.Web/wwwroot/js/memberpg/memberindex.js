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

        // Match the IDs from your HTML: restoreMemberName and restoreMemberId
        var displayName = restoreModal.querySelector('#restoreMemberName');
        var inputId = restoreModal.querySelector('#restoreMemberId');

        if (displayName) displayName.textContent = memberName;
        if (inputId) inputId.value = memberId;
    });
}

// FLOATING ALERT AUTO-REMOVE
window.addEventListener('DOMContentLoaded', (event) => {
    const alert = document.getElementById('successAlert');
    if (alert) {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    }
});