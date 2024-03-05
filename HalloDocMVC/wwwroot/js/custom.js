var input = document.querySelector("#phone");
window.intlTelInput(input, {
    separateDialCode: true
});
var input = document.querySelector("#phonenumber");
window.intlTelInput(input, {
    separateDialCode: true
});

$("#files").change(function () {
    filename = this.files[0].name;
    console.log(filename);
    $("#choosenfile").text(filename);
});


/*/ Modal /

var targetModal = new bootstrap.Modal(document.getElementById('targetModal', {}))
targetModal.show();

var dismissModal = document.getElementById('dismissModal');
dismissModal.addEventListener("click", () => {
    targetModal.hide();
})*/




const phoneInputField2 = document.querySelector("#phone");
const phoneInput2 = window.intlTelInput(phoneInputField2, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
const phoneInputField1 = document.querySelector("#phone1");
const phoneInput1 = window.intlTelInput(phoneInputField1, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});

//SubNav
// Get the tab container element
var tabContainer = document.getElementById("tab-container");

// Get all the tab elements
var tabs = tabContainer.getElementsByClassName("tab");

// Loop through the tabs and add a click event listener to each one
for (var i = 0; i < tabs.length; i++) {
    tabs[i].addEventListener("click", function () {
        // Remove the active class from the current active tab
        var current = tabContainer.getElementsByClassName("active");
        current[0].className = current[0].className.replace(" active", "");

        // Add the active class to the clicked tab
        this.className += " active";
    });
}
















































































































































































//Darkmode
/*function myFunction() {
    let element = document.body;
    element.classList.toggle("dark");
}*/