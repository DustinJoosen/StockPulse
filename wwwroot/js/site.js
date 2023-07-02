
$(document).ready(function () {

    // Toggle the partial view tables on the products.
    $(".product-stock-product-header").on("click", function () {
        let table = $("#hidden-stock-product-table-" + $(this).attr("value"));

        if (table.css("display") == "block") {
            table.css("display", "none");
            $(this).text($(this).text().replace("-", "+"));
        } else if (table.css("display") == "none") {
            table.css("display", "block");
            $(this).text($(this).text().replace("+", "-"));
        }
    });

    // Toggle the partial view tables on the warehouses.
    $(".product-stock-warehouse-header").on("click", function () {
        let table = $("#hidden-stock-warehouse-table-" + $(this).attr("value"));

        if (table.css("display") == "block") {
            table.css("display", "none");
            $(this).text($(this).text().replace("-", "+"));
        } else if (table.css("display") == "none") {
            table.css("display", "block");
            $(this).text($(this).text().replace("+", "-"));
        }
    });

    // Switching the partial views
    function hideAllPartialViews() {
        $(".product-stock-view-default").css("display", "none");
        $(".product-stock-view-product").css("display", "none");
        $(".product-stock-view-warehouse").css("display", "none");
    }

    $('.btn-product-stock-set-default').click(function () {
        hideAllPartialViews();
        $(".product-stock-view-default").css("display", "block");
    });
    $('.btn-product-stock-set-product').click(function () {
        hideAllPartialViews();
        $(".product-stock-view-product").css("display", "block");
    });
    $('.btn-product-stock-set-warehouse').click(function () {
        hideAllPartialViews();
        $(".product-stock-view-warehouse").css("display", "block");
    });

    // Start with the default
    $(".btn-product-stock-set-default").trigger("click");


    // Have the sidebar set the active.
    let path = window.location.pathname;
    let page = path.substr(1, path.length).toLowerCase().replace("account/", "");

    let flag = false;

    let links = document.getElementsByClassName("sidebar-hyperlink");


    for (let i = 0; i < links.length; i++) {
        links[i].classList.remove("active");

        if (!flag && (links[i].innerHTML.toLowerCase()).includes(page)) {
            links[i].classList.add("active");
            flag = true;
        }
    }

})

