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
