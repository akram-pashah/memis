
function confirmDelete(userId, onConfirm) {
  return Swal.fire({
    title: 'Are you sure?',
    text: "You won't be able to revert this!",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Yes, delete it!'
  }).then((result) => {
    onConfirm();
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

document.addEventListener("DOMContentLoaded", function () {
  const formatNumberLive = (input) => {
    let cursorPosition = input.selectionStart; // Get cursor position
    let rawValue = input.value.replace(/,/g, "").trim(); // Remove commas

    if (rawValue === "" || isNaN(rawValue)) return;

    let numberValue = parseInt(rawValue, 10); // Convert to integer
    input.setAttribute("data-value", numberValue); // Store actual number

    // Format number with commas (no decimals)
    let formattedValue = numberValue.toLocaleString("en-US");

    // Count comma differences before and after formatting
    let prevCommas = (input.value.match(/,/g) || []).length;
    input.value = formattedValue;
    let newCommas = (formattedValue.match(/,/g) || []).length;

    // Adjust cursor position, keeping it in place while typing
    let adjustment = newCommas - prevCommas;
    let newCursorPosition = cursorPosition + adjustment;

    if (newCursorPosition <= input.value.length) {
      input.selectionStart = input.selectionEnd = newCursorPosition;
    }
  };

  document.querySelectorAll(".formatted-number").forEach(input => {
    if (input.value) formatNumberLive(input); // Format on load

    // Format while typing without moving cursor to the end
    input.addEventListener("input", function () {
      let initialLength = this.value.length;
      let cursorPosition = this.selectionStart;

      formatNumberLive(this);

      let newLength = this.value.length;
      let diff = newLength - initialLength;
      this.selectionStart = this.selectionEnd = cursorPosition + diff;
    });

    // Remove formatting on focus (show raw number)
    input.addEventListener("focus", function () {
      this.value = this.getAttribute("data-value") || "";
    });

    // Restore formatting on blur
    input.addEventListener("blur", function () {
      formatNumberLive(this);
    });

    // Ensure proper number submission
    input.closest("form")?.addEventListener("submit", function () {
      input.value = input.getAttribute("data-value") || "";
    });
  });
});

//document.addEventListener("DOMContentLoaded", function () {
//  const formatNumber = (input) => {
//    let rawValue = input.value.replace(/,/g, "").trim(); // Remove commas
//    if (rawValue === "" || isNaN(rawValue)) return;

//    let numberValue = parseFloat(rawValue); // Convert to float
//    input.setAttribute("data-value", numberValue); // Store actual number
//    input.value = numberValue.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 }); // Format display
//  };

//  document.querySelectorAll(".formatted-number").forEach(input => {
//    // Format on page load if value exists
//    if (input.value) formatNumber(input);

//    // Format when leaving input
//    input.addEventListener("blur", function () {
//      formatNumber(this);
//    });

//    // Remove formatting on focus (show raw number)
//    input.addEventListener("focus", function () {
//      this.value = this.getAttribute("data-value") || "";
//    });

//    // Ensure raw number is sent to backend
//    input.closest("form")?.addEventListener("submit", function () {
//      input.value = input.getAttribute("data-value") || "";
//    });
//  });
//});
