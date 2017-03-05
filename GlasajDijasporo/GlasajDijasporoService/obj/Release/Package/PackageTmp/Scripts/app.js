var wrapper = document.getElementById("signature-pad"),
        clearButton = wrapper.querySelector("[data-action=clear]"),
        canvas = wrapper.querySelector("canvas"),
        signaturePad;

var submitButton = document.getElementById("submitApplication");
var signatureField = document.getElementById("Signature");
var embassyEmail = document.getElementById("embassyEmail");
var votingLocation = document.getElementById("votingLocation");

function resizeCanvas() {
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    canvas.width = canvas.offsetWidth * ratio;
    canvas.height = canvas.offsetHeight * ratio;
    canvas.getContext("2d").scale(ratio, ratio);
}

window.onresize = resizeCanvas;
resizeCanvas();

signaturePad = new SignaturePad(canvas);

clearButton.addEventListener("click", function (event) {
    signaturePad.clear();
});

submitButton.addEventListener("click", function (event) {
    signatureField.value = signaturePad.toDataURL();
});

function votingLocationSelected() {
    embassyEmail.innerHTML = votingLocation.value.split('|')[1];
}