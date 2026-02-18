document.addEventListener('DOMContentLoaded', function () {
    const editModal = document.getElementById('editModal');
    let originalValues = {};

    if (editModal) {
        editModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;

            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const code = button.getAttribute('data-code');
            const fee = button.getAttribute('data-fee');
            const desc = button.getAttribute('data-desc') || "";

            document.getElementById('editMembershipId').value = id;
            document.getElementById('editName').value = name;
            document.getElementById('editCode').value = code;
            document.getElementById('editFee').value = fee;
            document.getElementById('editDescription').value = desc;

            originalValues = { name, code, fee, desc };
        });

        const editForm = editModal.querySelector('form');
        if (editForm) {
            editForm.addEventListener('submit', function (e) {
                const currentValues = {
                    name: document.getElementById('editName').value.trim(),
                    code: document.getElementById('editCode').value.trim(),
                    fee: document.getElementById('editFee').value.trim(),
                    desc: document.getElementById('editDescription').value.trim()
                };

                if (
                    currentValues.name === originalValues.name &&
                    currentValues.code === originalValues.code &&
                    currentValues.fee === originalValues.fee &&
                    currentValues.desc === originalValues.desc
                ) {
                    e.preventDefault();
                    bootstrap.Modal.getInstance(editModal).hide();

                    const alertDiv = document.createElement('div');
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
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const isActive = button.getAttribute('data-status') === 'true';

            document.getElementById('deleteMembershipId').value = id;
            document.getElementById('deleteModalBodyText').textContent = isActive
                ? `"${name}" membership will be deactivated. Proceed?`
                : `"${name}" membership will be reactivated. Proceed?`;
        });
    }

    // Common Alert Auto-Fade
    document.querySelectorAll('#successAlert, #errorAlert').forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});