$(document).ready(function () {
    var requestToken = $('input[name="__RequestVerificationToken"]').val();

    // Оброботчики добавления/удаления строк имен авторов --- Начало
    $(document).on('click touchstart', '#_add_authors_row', function () {
        $.post('workspace/additem/addrow', {
            names: GetAuthorsNames(),
            __RequestVerificationToken: requestToken
        }, function (res) {
            $('#_authors_area').empty();
            $('#_authors_area').append(res);
        });
    });
    $(document).on('click touchstart', '.del-author-row', function () {
        $('#_del_row_'.concat($(this).attr('id'))).remove();
        $.post('workspace/additem/deleterow', {
            names: GetAuthorsNames(),
            __RequestVerificationToken: requestToken
        }, function (res) {
            $('#_authors_area').empty();
            $('#_authors_area').append(res);
        });
    });
    function GetAuthorsNames() {
        var names = [];
        $('div.author-row > div > input').each(function (index, value) {
            names[index] = $(value).val();
        })
        return names;
    }
    // Оброботчики добавления/удаления строк имен авторов ---- Конец

    // Обработчик кнопки Далее --- Начало
    $(document).on('click touchstart', '#_further_btn', function () {
        $('form#_start_form').submit();
    });
    // Обработчик кнопки Далее --- Конец


    $(document).on('click touchstart', '#_get_archive', function () {
        $.get('workspace/search/archive', function (res) {
            $('#_all_search_result').empty();
            $('#_all_search_result').append(res);
        });
    });
});