//var Success = "Success";
//var Error = "Error";
//var info = "info";
//var warning = "warning";




// Uses
// NioApp.Toast(message, type, {attr});
//
// @message     = 'Your message'
// @type        = 'info|success|warning|error',
// @attr        = {position: 'bottom-right', icon: 'auto', ui: ''}

//NioApp.Toast('<h5>' + Title + '</h5><p>' + Message + '</p>', Success, {
//    icon: 'auto',
//    closeButton: true,
//    progressBar: true,
//    position: 'top-right',
//    timeOut: 4000
//});



// ======= Generic Toast Function =======
function showToast(title, message, type, options = {}) {

    // Default toastr options
    const defaultOptions = {
        icon: 'auto',
        closeButton: true,
        progressBar: true,
        position: 'top-right',
        timeOut: 4000
    };

    // Merge custom options if provided
    const toastOptions = Object.assign({}, defaultOptions, options);

    // Build HTML structure
    const html = `<h5>${title}</h5><p>${message}</p>`;

    // Trigger NioApp toast
    NioApp.Toast(html, type, toastOptions);
}