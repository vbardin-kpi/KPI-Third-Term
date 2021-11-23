const BASE_URL = 'https://lab3-backend.azurewebsites.net/api/v1/';

$(document).ready(function () {
    loadAccordions();
});

function loadAccordions() {
    $.ajax({
        type: "GET",
        url: BASE_URL + 'accordions/all',
        success: function (e) {
            addAccordionsHtml(e)
        },
        error: function (e) {
            toastr.error(e);
        }
    })
}

function addAccordionsHtml(accordions) {
    let html = '';

    for (let i in accordions) {
        let accHtml = '';

        accHtml += "<button class=\"accordion\" acid=\"" + accordions[i].id + "\">" + accordions[i].title + "</button>";
        accHtml += "<div class=\"panel\"><p>" + accordions[i].body + "</p></div>";

        html += accHtml;
    }

    document.querySelector('#menu-center').innerHTML = html;

    setAccordionEventListeners(removeAccordion, editAccordion);
}

function reloadAccordions() {
    document.querySelector('#menu-center').innerHTML += '';
    loadAccordions();
}

function removeAccordion(accordion) {
    let id = accordion.getAttribute('acid');

    $.ajax({
        type: "DELETE",
        url: BASE_URL + 'accordions/delete/' + id,
        success: function () {
            toastr.success('Accordion deleted!');
        },
        error: function (e) {
            toastr.error('An error occurred while accordion removing', e);
        }
    })

    reloadAccordions();
}

function editAccordion(accordion) {
    let id = accordion.getAttribute('acid');
    Swal.fire({
        title: 'Edit ' + id,
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'Update',
        showLoaderOnConfirm: true,
        html:
            '<input id="swal-input1" class="swal2-input" placeholder="Title...">' +
            '<input id="swal-input2" class="swal2-input" placeholder="Body...">',
        preConfirm: () => {
            let data = {
                id: id,
                title: $('#swal-input1').val(),
                body: $('#swal-input2').val()
            };

            $.ajax({
                type: "PATCH",
                url: BASE_URL + 'accordions/patch',
                headers: {
                    "Content-Type": "application/json"
                },
                data: JSON.stringify(data)
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then(() => {
        reloadAccordions();
    });
}
