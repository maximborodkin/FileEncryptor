const updateText = document.getElementById('update-text');
const fileUploadFile = document.getElementById('fileUpload-file');
const updateKey = document.getElementById('update-key');
const fileUploadKey = document.getElementById('fileUpload-key');
const updateIsEncrypted = document.getElementById('update-isEncrypted');
const fileUploadIsEncrypted = document.getElementById('fileUpload-isEncrypted');
const validationAlert = document.getElementById('validation-alert');
const updateSubmit = document.getElementById('update-submit');
const fileUploadSubmit = document.getElementById('fileUpload-submit');

updateKey.addEventListener('change', (event) => {
    fileUploadKey.value = updateKey.value;
});

updateIsEncrypted.addEventListener('change', (event) => {
    fileUploadIsEncrypted.value = updateIsEncrypted.value;
    console.log(fileUploadIsEncrypted.value)
});

validateForm();

// disable buttons if required fields are empty or has invalid data
function validateForm(e) {
	fileUploadSubmit.disabled = updateKey.value == null || updateKey.value.trim().length == 0 || !validateFile(fileUploadFile.value);
	updateSubmit.disabled = updateText.value == null || updateText.value.trim().length == 0 || updateKey.value == null || updateKey.value.trim().length == 0;
}

function validateFile(fileName) {
	if (fileName == null || fileName.trim().length == 0) return false;
	var allowed_extensions = new Array("txt","doc","docx");
    var file_extension = fileName.split('.').pop().toLowerCase();

    for(var i = 0; i <= allowed_extensions.length; i++)
    {
        if(allowed_extensions[i]==file_extension) return true;
    }
    return false;
}