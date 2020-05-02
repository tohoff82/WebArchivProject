$(document).ready(function () {
    var requestToken = $('input[name="__RequestVerificationToken"]').val();

    $(document).on('click touchstart', '#_add_authors_row', function () {
        $.get('workspace/additem/authorsrow', function (res) {
            $('#_authors_area').append(res);
        });
    });


    $(document).on('click touchstart', '#_get_archive', function () {
        $.get('workspace/search/archive', function (res) {
            $('#_all_search_result').empty();
            $('#_all_search_result').append(res);
        });
    });
});