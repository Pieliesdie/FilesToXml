function saveFile(file, Content) {
    var link = document.createElement('a');
    link.download = name;
    link.href = "data:xml;" + encodeURIComponent(Content)
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}