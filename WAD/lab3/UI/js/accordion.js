$(document).ready(function () {
    setAccordionEventListeners();
})

function setAccordionEventListeners(auxEventHandler, doubleClickEventHandler) {
    let acc = document.querySelectorAll(".accordion");
    let i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function() {
            this.classList.toggle("active");

            let panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
        });

        acc[i].addEventListener('auxclick', function (e) {
            auxEventHandler(e.target);
        });

        acc[i].addEventListener('dblclick', function (e) {
            doubleClickEventHandler(e.target);
        })
    }
}