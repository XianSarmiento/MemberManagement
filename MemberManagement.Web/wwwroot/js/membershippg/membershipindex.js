document.addEventListener('DOMContentLoaded', function () {
    const editModal = document.getElementById('editModal');
    let originalValues = {};

    if (editModal) {
        // Fill modal fields on show
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

            originalValues = {
                name: name,
                code: code,
                fee: parseFloat(fee).toFixed(2),
                desc: desc
            };
        });

        // Detect no changes on submit
        const editForm = editModal.querySelector('form');
        if (editForm) {
            editForm.addEventListener('submit', function (e) {
                const currentValues = {
                    name: document.getElementById('editName').value.trim(),
                    code: document.getElementById('editCode').value.trim(),
                    fee: parseFloat(document.getElementById('editFee').value).toFixed(2),
                    desc: document.getElementById('editDescription').value.trim()
                };

                if (currentValues.name === originalValues.name &&
                    currentValues.code === originalValues.code &&
                    currentValues.fee === originalValues.fee &&
                    currentValues.desc === originalValues.desc) {

                    e.preventDefault(); // Stop form submit

                    // Close the modal
                    const modalInstance = bootstrap.Modal.getInstance(editModal);
                    if (modalInstance) modalInstance.hide();

                    // Show success alert at top
                    const alertDiv = document.createElement('div');
                    alertDiv.id = 'successAlert';
                    alertDiv.className = 'alert alert-success text-center operation-alert';
                    alertDiv.innerHTML = '<i class="fa-solid fa-circle-check"></i> No changes detected';
                    document.body.appendChild(alertDiv);

                    // Auto-hide after 3s
                    setTimeout(() => {
                        alertDiv.style.animation = 'fadeOut 0.5s forwards';
                        setTimeout(() => alertDiv.remove(), 500);
                    }, 3000);
                }
            });
        }
    }

    // Auto-hide existing alerts
    const alerts = document.querySelectorAll('#successAlert, #errorAlert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});
