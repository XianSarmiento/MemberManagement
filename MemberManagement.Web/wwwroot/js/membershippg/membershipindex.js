document.addEventListener('DOMContentLoaded', function () {
    const deleteModal = document.getElementById('deleteModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');

            deleteModal.querySelector('#membershipNameLabel').textContent = name;
            deleteModal.querySelector('#deleteMembershipId').value = id;
            deleteModal.querySelector('#deleteForm').action = '/MembershipTypes/Delete';
        });
    }

    const alerts = document.querySelectorAll('#successAlert, #errorAlert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        }, 3000);
    });
});