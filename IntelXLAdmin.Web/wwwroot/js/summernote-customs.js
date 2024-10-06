$(document).ready(function () {
    summernote();
})
function summernote() {
    $('.summernote').summernote({
        height: 170,
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['superscript', 'subscript']],            
            ['para', ['ul', 'ol', 'paragraph']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],         
            ['insert', ['table']],
            ['insert', ['picture']],
            ['view', ['fullscreen']],            
        ],
        callbacks: {
            //To Avoid break when paste
            onPaste: function (e) {                
                var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');    
                if (bufferText.includes('http://') || bufferText.includes('https://')) {
                    // If it contains a link, prevent the default paste behavior
                    e.preventDefault();
                    alert('Links are not allowed');
                    return false;
                }
                e.preventDefault();
                document.execCommand('insertText', false, bufferText);
            },            
        }
    });
}