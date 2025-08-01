﻿function downloadFile(mimeType, base64String, fileName) {
    var fileDataUrl = "data:" + mimeType + ";base64," + base64String;
    fetch(fileDataUrl)
        .then(response => response.blob())
        .then(blob => {
            var link = window.document.createElement("a");
            link.href = window.URL.createObjectURL(blob, { type: mimeType });
            link.download = fileName;

            document.body.appendChild(link);
            link.click();
            window.open(link.href, '_blank');
            document.body.removeChild(link);
        });
}

function focusById(elementId) {
    var element = document.getElementById(elementId);

    if (element) {
        element.focus();
    }
}