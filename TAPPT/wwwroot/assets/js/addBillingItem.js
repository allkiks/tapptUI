(function () {
    $(document).ready(function () {
        $('#submit-request').click(submitAddBillingItem);
    });
    //Functions Section
  
    function submitAddBillingItem(e) {
        e.preventDefault();

        let url = apiBaseUrl.concat(`/admin/CreateBillingItem`);
        var itemName = $('#ItemName').val();
        var userId = $('#UserId').val();
        var rate = $('#Rate').val();
        $.ajax({
            url: url,
            crossDomain: true,
            contentType: 'application/json',
            accepts: 'application/json',
            type: 'POST',
            data: JSON.stringify({ ItemName: itemName, UserId: userId, Rate: rate})
        }).done(function (data, textStatus, jqXHR) {
            $('#submit-request').html('Yes');
            $('#additem').modal('hide');
            $('.modal-backdrop').remove();

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $('#submit-request').html('Yes');
            $('#additem').modal('hide');
            $('.modal-backdrop').remove();
        });
    }
})();

//Start by getting publication list based on Payment Verification Stage
