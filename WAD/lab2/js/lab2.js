// task 1
document.querySelector('#x-header').addEventListener("click", function () {
    let xHeader = document.querySelector('.x-header')
    let yHeader = document.querySelector('.y-header')

    swap(xHeader, yHeader);
})

document.querySelector('.y-header').addEventListener("click", function () {
    let xHeader = document.querySelector('.x-header')
    let yHeader = document.querySelector('.y-header')

    swap(xHeader, yHeader)
})

function swap(xh, yh) {
    let temp = xh.innerHTML
    xh.innerHTML = yh.innerHTML
    yh.innerHTML = temp
}

// task 2
CIRCLE_RADIUS = 10

document.getElementById('p2-output')
    .append("Circle square is: " + (Math.PI * Math.pow(CIRCLE_RADIUS, 2)))

// task 3
T3_COOKIE_NAME = 't3-greatest-number'

if (get_cookie(T3_COOKIE_NAME) !== undefined) {
    notify_about_cookies();
}
else {
    add_form()
}

function notify_about_cookies() {

    let confirm = window.confirm("The greatest value from the previous time is: " + get_cookie(T3_COOKIE_NAME) +
        ". Do you want to delete the cookie: " + T3_COOKIE_NAME)

    if (confirm === true) {
        delete_cookie(T3_COOKIE_NAME)
        add_form()
    }
    else {
        alert("The cookie from the previous execution is still exists, " +
            "so the page should be updated! The cookie will be removed!")
        delete_cookie(T3_COOKIE_NAME)
        window.location.reload()
    }
}

function delete_cookie(name) {
    if(get_cookie(name)) {
        document.cookie = name + "=" +
            ";expires=Thu, 01 Jan 1970 00:00:01 GMT";
    }
}

function add_form()
{
    let form = document.createElement('form')
    form.id = 't3-form'

    let br = document.createElement('br')

    let label = document.createElement('label')
    label.htmlFor = 'nums-input'
    label.innerHTML = 'Enter 10 numbers separated by coma:'

    let numsInput = document.createElement('input')
    numsInput.type = 'text'
    numsInput.id = 'nums-input'
    numsInput.placeholder = '1,2,3,4,5,6,7,8,9,10'

    let numsSubmit = document.createElement('input')
    numsSubmit.type = 'submit'
    numsSubmit.value = 'submit'
    numsSubmit.addEventListener('click', (e) => {
        e.preventDefault()
        process_input()
    })

    form.appendChild(label)
    form.appendChild(br)
    form.appendChild(br)
    form.appendChild(numsInput)
    form.appendChild(br)
    form.appendChild(numsSubmit)

    document.getElementById('t3').appendChild(form)
}

function process_input() {
    let greatestNumber = document.querySelector('#nums-input')
        .value.split(',').map(Number)
        .sort((x, y) => y - x)[0]

    document.getElementById('t3-form').remove()
    document.getElementById('t3').append("The greatest number is: " + greatestNumber)

    set_t3_cookie(greatestNumber)
}

function set_t3_cookie(number) {
    let name = "t3-greatest-number"
    let value = number
    // 1 day
    let expireDate = new Date(Date.now() + 86400e3);

    document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + "; expires=" + expireDate
}

function get_cookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

// task 4
document.querySelector('.top-left').addEventListener('blur', () =>  {
    applyAndSaveColor()
})

document.querySelector('.top-left').style.backgroundColor =
    localStorage.getItem('t4-color') ?? document.querySelector('.top-left').style.backgroundColor

function applyAndSaveColor() {
    let color = document.querySelector('#t4-color-selector').value

    document.querySelector('.top-left').style.backgroundColor = color
    localStorage.setItem('t4-color', color)
}

// task 5
document.querySelectorAll('.editable').forEach((el) => {
    el.addEventListener('dblclick', (event) => {
        let savedInfoKey = event.target.id + "-original-version"
        let topBlockId = '#' + event.target.id

        let targetElemInnerHtml = event.target.innerHTML

        let textarea = document.createElement('textarea')
        textarea.innerText = targetElemInnerHtml

        let btnCancel = document.createElement('input')
        btnCancel.type = 'submit'
        btnCancel.value = 'Undo'
        btnCancel.onclick = () => {
            if (localStorage.getItem(savedInfoKey) !== null) {
                localStorage.removeItem(savedInfoKey)
            }
            event.target.innerHTML = targetElemInnerHtml
        }

        let btnSubmit = document.createElement('input')
        btnSubmit.type = 'submit'
        btnSubmit.value = 'Save'
        btnSubmit.onclick = () => {
            event.target.innerHTML = textarea.value
            localStorage.setItem(savedInfoKey, targetElemInnerHtml)

            let rollbackBtn = document.createElement('input')
            rollbackBtn.type = 'submit'
            rollbackBtn.value = 'Rollback'
            rollbackBtn.addEventListener('click', (event) => {
                let target = event.target

                document.querySelector(topBlockId).innerHTML = localStorage.getItem(savedInfoKey)
                localStorage.removeItem(savedInfoKey)
            })

            event.target.appendChild(rollbackBtn)
        }

        event.target.appendChild(document.createElement('br'))
        event.target.appendChild(document.createElement('br'))
        event.target.appendChild(textarea)
        event.target.appendChild(btnCancel)
        event.target.appendChild(btnSubmit)
    })
})
