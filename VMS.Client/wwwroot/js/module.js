export function showToast(selector) {
    console.log("showToast called with selector:", selector);
    const toastElement = document.querySelector(selector);
    if (toastElement) {
        const toast = new bootstrap.Toast(toastElement);
        toast.show();
    }
}