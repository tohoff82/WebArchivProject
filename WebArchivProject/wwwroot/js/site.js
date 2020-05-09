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
    // Обработчик переключения типа Книга--Методичка --- Конец

    // Обработчик кнопки ВсеПубликации --- Начало
    $(document).on('click touchstart', '#_get_archive', function () {
        $.get('workspace/search/spinnerwave', function (res) {
            $('#_all_search_result').empty();
            $('#_all_search_result').append(res);
        });
        $.post('workspace/search/archive', {
            __RequestVerificationToken: requestToken
        }, function (res) {
            $('#_all_search_result').empty();
            $('#_all_search_result').append(res);
        });
    });
    // Обработчик кнопки ВсеПубликации --- Конец

    // Обработчик пейджера --- Начало
    $(document).on('click touchstart', '.pager-click', function () {
        var param = $(this).attr('id');
        var table = $(this).attr('data-target');
        var container = $(this).attr('data-container');
        if (container === 'all_cont') {
            $.post('workspace/search/currentarchiveall', {
                tableType: table,
                action: param,
                target: container, 
                __RequestVerificationToken: requestToken
            }, function (res) {
                if (table === 'book') {
                    $('#_table_books_result').empty();
                    $('#_table_books_result').append(res);
                }
                if (table === 'post') {
                    $('#_table_posts_result').empty();
                    $('#_table_posts_result').append(res);
                }
                if (table === 'thesis') {
                    $('#_table_theses_result').empty();
                    $('#_table_theses_result').append(res);
                }
            });
        } else {
            $.post('workspace/search/modalsearchpagination', {
                tableType: table,
                action: param,
                target: container, 
                __RequestVerificationToken: requestToken
            }, function (res) {
                if (table === 'book') {
                    $('#_book_container_modal').empty();
                    $('#_book_container_modal').append(res);
                }
                if (table === 'post') {
                    $('#_post_container_modal').empty();
                    $('#_post_container_modal').append(res);
                }
                if (table === 'thesis') {
                    $('#_thesis_container_modal').empty();
                    $('#_thesis_container_modal').append(res);
                }
            });
        }
    });
    // Обработчик пейджера --- Конец

    //Обработка принажатии кнопки УДАЛИТЬ --- Начало
    $(document).on('click touchstart', '.btn-delete-item', function () {
        $($(this).attr('data-trigger')).remove();
        $.post('workspace/search/deleteitem', {
            tableType: $(this).attr('data-target'),
            itemId: $(this).attr('id'),
            __RequestVerificationToken: requestToken
        });
    });
    //Обработка принажатии кнопки УДАЛИТЬ --- Конец

    //Обработка кнопок фильтрации --- Начало
    $(document).on('click touchstart', '#_btn_book_srch_filter', function () {
        $.get('workspace/search/spinnerwave', function (res) {
            $('#_book_container_modal').empty();
            $('#_book_container_modal').append(res);
        });
        $.post('workspace/search/bookssearchfilter', {
            year: $('#_book_year_select').val(),
            author: $('#_book_author_select').val(),
            name: $('#_book_name_select').val(),
            __RequestVerificationToken: requestToken
        }, function (res) {
            $('#_book_container_modal').empty();
            $('#_book_container_modal').append(res);
        });
    });
    $(document).on('click touchstart', '#_btn_post_srch_filter', function () {
        $.get('workspace/search/spinnerwave', function (res) {
            $('#_post_container_modal').empty();
            $('#_post_container_modal').append(res);
        });
        $.post('workspace/search/postssearchfilter', {
            year: $('#_post_year_select').val(),
            author: $('#_post_author_select').val(),
            name: $('#_post_name_select').val(),
            magazine: $('#_post_magazine_select').val(),
            __RequestVerificationToken: requestToken
        }, function (res) {
            $('#_post_container_modal').empty();
            $('#_post_container_modal').append(res);
        });
    });
    $(document).on('click touchstart', '#_btn_thesis_srch_filter', function () {
        $.get('workspace/search/spinnerwave', function (res) {
            $('#_thesis_container_modal').empty();
            $('#_thesis_container_modal').append(res);
        });
        $.post('workspace/search/thesessearchfilter', {
            year: $('#_thesis_year_select').val(),
            author: $('#_thesis_author_select').val(),
            name: $('#_thesis_name_select').val(),
            pages: $('#_thesis_pages_select').val(),
            __RequestVerificationToken: requestToken
        }, function (res) {
                console.log(res);
            $('#_thesis_container_modal').empty();
            $('#_thesis_container_modal').append(res);
        });
    });

    $(document).on('click touchstart', '.modal-close-action', function () {
        $('#_book_container_modal').empty();
        $('#_post_container_modal').empty();
        $('#_thesis_container_modal').empty();
    });
    //Обработка кнопок фильтрации --- Конец
});