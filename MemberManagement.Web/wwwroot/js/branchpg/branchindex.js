document.addEventListener('DOMContentLoaded', function () {
    // DELETE MODAL LOGIC
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

    // EDIT MODAL LOGIC (New)
    const editModal = document.getElementById('editModal');
    if (editModal) {
        editModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;

            // Extract info from data-* attributes
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const address = button.getAttribute('data-address');
            const code = button.getAttribute('data-code');

            // Populate the form fields
            editModal.querySelector('#editBranchId').value = id;
            editModal.querySelector('#editBranchName').value = name;
            editModal.querySelector('#editBranchAddress').value = address;
            editModal.querySelector('#editBranchCode').value = code;
        });
    }

    // ALERT AUTO-HIDE LOGIC
    const alerts = document.querySelectorAll('#successAlert, #errorAlert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});