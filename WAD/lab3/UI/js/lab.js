$('#accordionCreateForm').submit(function (e) {
   e.preventDefault();

   let form = $(this);
   let url = form.attr('action');

    let data = {
        title: $('#accordionTitle').val(),
        body: $('#accordionBody').val()
    }

    console.log(data);

   $.ajax(
       {
           type: "POST",
           url: url,
           headers: {
               "Content-Type": "application/json"
           },
           data: JSON.stringify(data),
           success: function (e) {
               console.log(e);
               alert('Accordion created' + e);
           },
           error: function (e) {
               console.log(e);
               alert(e);
           }
       }
   );
});