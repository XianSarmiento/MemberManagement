document.addEventListener('DOMContentLoaded', function () {
    // Modal Logic
    const deleteModal = document.getElementById('deleteModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');

            deleteModal.querySelector('#branchNameLabel').textContent = name;
            deleteModal.querySelector('#deleteBranchId').value = id;
            deleteModal.querySelector('#deleteForm').action = '/Branches/Delete';
        });
    }

    // Alert Auto-Hide Logic (Handles both Success and Error)
    const alerts = document.querySelectorAll('#successAlert, #errorAlert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});