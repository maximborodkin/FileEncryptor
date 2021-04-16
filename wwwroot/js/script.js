const updateText = document.getElementById('update-text');
const fileUploadFile = document.getElementById('fileUpload-file');
const updateKey = document.getElementById('update-key');
const fileUploadKey = document.getElementById('fileUpload-key');
const updateIsEncrypted = document.getElementById('update-isEncrypted');
const fileUploadIsEncrypted = document.getElementById('fileUpload-isEncrypted');
const validationAlert = document.getElementById('validation-alert');
const updateSubmit = document.getElementById('update-submit');
const fileUploadSubmit = document.getElementById('fileUpload-submit');

fileUploadKey.value = updateKey.value;
updateKey.addEventListener('change', (event) => {
    fileUploadKey.value = updateKey.value;
});

fileUploadIsEncrypted.checked = updateIsEncrypted.checked;
updateIsEncrypted.addEventListener('change', (event) => {
    fileUploadIsEncrypted.checked = updateIsEncrypted.checked;
    console.log(updateIsEncrypted.checked);
});

// disable buttons if required fields are empty or has invalid data
function updateButtonsState() {
    fileUploadSubmit.disabled = !validateKey(updateKey.value) || !validateFile(fileUploadFile.value);
    updateSubmit.disabled = !validateText(updateText.value) || !validateKey(updateKey.value);
}

// is text not empty
function validateText(text) {
    return text != null && text.trim().length > 0;
}

// is key not empty and contains only cyrillic characters
function validateKey(key) {
    console.log(/^[а-яА-ЯЁё]+$/.test(key));
    return key != null && key.trim().length > 0 && /^[а-яА-ЯЁё]+$/.test(key);
}

// is file not empty and has valid extension
function validateFile(fileName) {
	if (fileName == null || fileName.trim().length == 0) return false;
	var allowed_extensions = new Array("txt","docx");
    var file_extension = fileName.split('.').pop().toLowerCase();

    for(var i = 0; i <= allowed_extensions.length; i++)
    {
        if(allowed_extensions[i] == file_extension) return true;
    }
    return false;
}

updateButtonsState();