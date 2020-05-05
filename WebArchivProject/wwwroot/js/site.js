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

    // Обработчик переключения типа Книга--Методичка --- Начало
    $(document).on('change touchstart', '#_book_type_switcher', function () {
        if ($(this).is(':checked')) {
            $.post('workspace/addsubitem/typeswitcher', {
                type: $(this).val(),
                __RequestVerificationToken: requestToken
            }, function (res) {
                $('#_book_or_method_select').empty();
                $('#_book_or_method_select').append(res);
            });
        }
    });
    $(document).on('change touchstart', '#_method_type_switcher', function () {
        if ($(this).is(':checked')) {
            $.post('workspace/addsubitem/typeswitcher', {
                type: $(this).val(),
                __RequestVerificationToken: requestToken
            }, function (res) {
                $('#_book_or_method_select').empty();
                $('#_book_or_method_select').append(res);
            });
        }
    });
    // Обработчик переключения типа Книга--Методичка --- Начало

    // Обработчики кнопок кнопки Далее и Добавить --- Начало
    $(document).on('click touchstart', '#_further_btn', function () {
        $('form#_start_form').submit();
    });
    $(document).on('click touchstart', '#_add_book_btn', function () {
        $('form#_add_book_form').submit();
    });
    $(document).on('click touchstart', '#_add_post_btn', function () {
        $('form#_add_post_form').submit();
    });
    $(document).on('click touchstart', '#_add_thesis_btn', function () {
        $('form#_add_thesis_form').submit();
    });
    // Обработчики кнопок кнопки Далее и Добавить --- Конец


    $(document).on('click touchstart', '#_get_archive', function () {
        $.get('workspace/search/archive', function (res) {
            $('#_all_search_result').empty();
            $('#_all_search_result').append(res);
        });
    });
});