const BASE_URL = 'https://lab3-backend.azurewebsites.net/api/v1/';

$(document).ready(function () {
    setAccordionCreationFormListener();
})


function setAccordionCreationFormListener() {
    $('#accordionCreateForm').submit(function (e) {
        e.preventDefault();

        let data = {
            title: $('#accordionTitle').val(),
            body: $('#accordionBody').val()
        }

        $.ajax({
            type: "POST",
            url: BASE_URL + 'accordions/add',
            headers: {
                "Content-Type": "application/json"
            },
            data: JSON.stringify(data),
            success: function (e) {
                toastr.success("Accordion created!", "New accordion created, its id is " + e);
            },
            error: function (e) {
                toastr.error("Accordion creation error!",
                    "An error occurred while creating an accordion. \nError: " + e);
            }
        });
    });
}