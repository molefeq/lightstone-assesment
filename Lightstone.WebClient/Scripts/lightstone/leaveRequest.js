$(document).ready(function () {    
    $('body').on('click', '.btn-approve', function (e) {
        e.preventDefault();

        performAction($(this));
    });
    
    $('body').on('click', '.btn-decline', function (e) {
        e.preventDefault();

        performAction($(this));
    });

    $('body').on('click', '.btn-delete', function (e) {
        e.preventDefault();

        var that = this;
        var isDeleteAllowed = confirm("Are you sure you want to delete this request.");

        if (isDeleteAllowed) {
            performAction($(that));
        }
        
    });

    function performAction(button) {
      
        $.ajax({
            type: "POST",
            url: $(button).attr('data-url'),
            data: { LeaveRequestId: $(button).attr('data-id') },
            success: function (response) {
                window.location.href = response.data_url;
            }
        });
    }

});
