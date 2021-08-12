$(document).ready(function () {
    $("button").click(function () {
        debugger;
        var Id = [];
        $.each($("input[name='checkboxes']:checked"), function () {
            Id.push($(this).val());
        });
        $.ajax({
            url: "/bulk-delete",
            type: "post",
            datatype: "json",
            data: { Id: Id },
            success: function (response) {
                if (response) {
                    location.href = "https://localhost:44371/bulk-delete";
                }
            },
            error: function (response) {
                location.href = "https://localhost:44371/bulk-delete";
            }

        });
    });
});