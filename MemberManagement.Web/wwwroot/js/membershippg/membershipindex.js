document.addEventListener('DOMContentLoaded', function () {
    // --- DELETE/DEACTIVATE MODAL LOGIC ---
    const deleteModal = document.getElementById('deleteModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const isActive = button.getAttribute('data-status') === 'true';

            // Change text based on current status
            const actionTitle = isActive ? 'Deactivate' : 'Activate';
            const actionWarning = isActive ? 'deactivate' : 'activate';

            deleteModal.querySelector('.modal-title').innerHTML = `<i class="bi bi-exclamation-triangle me-2"></i>Confirm ${actionTitle}`;
            deleteModal.querySelector('#membershipNameLabel').textContent = name;
            deleteModal.querySelector('.modal-body').innerHTML = `Are you sure you want to ${actionWarning} <strong>${name}</strong>?`;

            // Update button color and text
            const confirmBtn = deleteModal.querySelector('button[type="submit"]');
            confirmBtn.textContent = actionTitle;
            confirmBtn.className = `btn ${isActive ? 'btn-danger' : 'btn-success'} px-4`;

            deleteModal.querySelector('#deleteMembershipId').value = id;
            // Point to your ToggleStatus action
            deleteModal.querySelector('#deleteForm').action = '/MembershipTypes/ToggleStatus';
        });
    }

    // --- EDIT MODAL LOGIC ---
    const editModal = document.getElementById('editModal');
    if (editModal) {
        editModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const code = button.getAttribute('data-code');
            const fee = button.getAttribute('data-fee');
            const desc = button.getAttribute('data-desc');

            editModal.querySelector('#editMembershipId').value = id;
            editModal.querySelector('#editName').value = name;
            editModal.querySelector('#editCode').value = code;
            editModal.querySelector('#editFee').value = fee;
            editModal.querySelector('#editDescription').value = desc;
        });
    }

    // --- ALERT AUTO-HIDE ---
    const alerts = document.querySelectorAll('#successAlert, #errorAlert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});