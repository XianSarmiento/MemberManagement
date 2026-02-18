document.addEventListener('DOMContentLoaded', function () {
    const editModal = document.getElementById('editModal');
    let originalValues = {};

    if (editModal) {
        editModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;

            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const code = button.getAttribute('data-code');
            const address = button.getAttribute('data-address') || "";

            document.getElementById('editBranchId').value = id;
            document.getElementById('editBranchName').value = name;
            document.getElementById('editBranchCode').value = code;
            document.getElementById('editBranchAddress').value = address;

            originalValues = { name, code, address };
        });

        const editForm = editModal.querySelector('form');
        if (editForm) {
            editForm.addEventListener('submit', function (e) {
                const currentValues = {
                    name: document.getElementById('editBranchName').value.trim(),
                    code: document.getElementById('editBranchCode').value.trim(),
                    address: document.getElementById('editBranchAddress').value.trim()
                };

                if (
                    currentValues.name === originalValues.name &&
                    currentValues.code === originalValues.code &&
                    currentValues.address === originalValues.address
                ) {
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

    const deleteModal = document.getElementById('deleteModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const branchId = button.getAttribute('data-id');
            const branchName = button.getAttribute('data-name');
            const isActive = button.getAttribute('data-status') === 'true';

            document.getElementById('deleteBranchId').value = branchId;
            document.getElementById('deleteModalBodyText').textContent = isActive
                ? `"${branchName}" will be deactivated. Proceed?`
                : `"${branchName}" will be activated. Proceed?`;

            document.getElementById('deleteModalConfirmBtn').className = isActive
                ? 'btn btn-sm btn-danger px-3'
                : 'btn btn-sm btn-success px-3';
        });
    }

    // Fade out alerts
    document.querySelectorAll('#successAlert, #errorAlert').forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});
