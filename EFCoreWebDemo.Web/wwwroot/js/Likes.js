
$(() => {
    console.log("test")
    const id = $("#id").val();

    $("#like-btn").on('click', function () {
        $.post("/Home/AddLike", { id });
        $("#like-btn").attr('disabled', true);
    });

    setInterval(() => {
        $.get("/Home/CurrentLikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        })
    }, 1000);
});

