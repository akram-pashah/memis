
function confirmDelete(userId) {
  return Swal.fire({
    title: 'Are you sure?',
    text: "You won't be able to revert this!",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Yes, delete it!'
  }).then((result) => {
    return result.isConfirmed;
  });
}

function showSuccessMessage(message, onConfirm) {
  Swal.fire({
    icon: 'success',
    title: 'Success',
    text: message,
    confirmButtonColor: '#12e34a',
    confirmButtonText: 'Ok'
  }).then((result) => {
    onConfirm();
  });
}

function showErrorMessage(message) {
  Swal.fire({
    icon: 'error',
    title: 'Error',
    text: message
  });
}
