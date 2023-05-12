function getProductByCategory(id) {
    $.ajax({
        url: '/Products/getProductByCategoryId',
        data: { catId: id },
        success: function (data) {
            var div = $('#productbyCategory');
            div.append('');
            var html = '';
            $.each(data, function (val, text) {
                html += "<div class='product-item'>" +
                    "<div class='pi-pic'>" +
                    "<img src=" + text.picturePath + " alt='' />" +
                    " <div class='icon'>" +
                    "<i class='icon_heart_alt'></i>" +
                    " </div>" +
                    "<ul>  <li class='w-icon active'><a href='#'><i class='icon_bag_alt'></i></a></li>" +
                    "<li class='quick-view'><a href='#'>+ Quick View</a></li>" +
                    " <li class='w-icon'><a href='#'><i class='fa fa-random'></i></a></li>" +
                    "</ul>" +
                    "</div>" +
                    "<div class='pi-text'>" +
                    "<div class='catagory-name'>" + text.category.name + "</div>" +
                    "<a href='#'><h5>" + text.name + "</h5></a>" +
                    "<div class='product-price'>" + text.unitPrice + "</div>" +
                    "</div>" +
                    "</div>";

            });
            div.append(
                html
            );

        },
        error: function (xhr, ajaxOptions, thrownError) {
            //some errror, some show err msg to user and log the error
            alert(xhr.responseText);

        }
    });
}

function removeToCart(id) {
    $.ajax({
        url: '/Home/removeToCart',
        data: { Id: id },
        success: function (data) {
            window.location = "/";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //some errror, some show err msg to user and log the error
            alert(xhr.responseText);

        }
    });
}


function removefromWishtList(id) {
    $.ajax({
        url: '/Home/removefromWishtList',
        data: { proId: id },
        success: function (data) {
            alert(data);
            window.location = "/";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //some errror, some show err msg to user and log the error
            alert(xhr.responseText);

        }
    });
}

function addToWishList(proId) {
    var qty = $('#proQty').val();
    if (qty != "") {
        $.ajax({
            url: '/Home/AddToWishList',
            data: { proId: proId },
            contentType: "application/json",
            success: function (data) {
                alert(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //some errror, some show err msg to user and log the error
                alert(xhr.responseText);
            }
        });
    }
    else {
        alert("Enter Quantity");
    }
}


function addToCard(proId) {
    var qty = $('#proQty').val();
    if (qty != "") {
        //console.log("Token", sessionStorage.getItem('JWTToken'));

        $.ajax({
            url: '/Home/AddToCart',
            data: { proId: proId, qty: qty },
            contentType: "application/json",
            success: function (data) {
                if (data.redirectToUrl) {
                    window.location.replace(data.redirectToUrl);
                } else {
                    //     Handle the response when the user is authenticated
                    window.location = "/";
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //some errror, some show err msg to user and log the error
                alert(xhr.responseText);
            }
        });
    }
    else {
        alert("Enter Quantity");
    }
}

function CheckoutOrder() {
    var fname = $('#firstName').val();
    var lname = $('#lastName').val();
    var email = $('#email').val();
    var mobile = $('#mobile').val();
    var address = $('#address').val();
    var city = $('#city').find(":selected").text();
    var pcode = $('#pcode').val();
    if (fname == "") {
        alert('First name is required');
        return false;
    }
    else if (lname == "") {
        alert('Last name is required');
        return false;
    }

    else if (mobile == "") {
        alert('mobile number is required');
        return false;
    }
    else if (address == "") {
        alert('Address is required');
        return false;
    }
    else if (city == "Choose City") {
        alert('City is required');
        return false;
    }
    else if (pcode == "") {
        alert('Postal Code is required');
        return false;
    }
    else {
        var data = {
            FirstName: fname,
            LastName: lname,
            Email: email,
            Mobile: mobile,
            Address: address,
            City: city,
            PCode: pcode
        }
        $.ajax({
            url: '/ViewCart/CheckoutOrder',
            data: data,
            type: "POST",
            success: function (data) {
                if (data != null) {
                    alert(data);
                    window.location = "/";
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //some errror, some show err msg to user and log the error
                alert(xhr.responseText);

            }
        });
    }
}

function UpdateCart() {
    var cartItem = new Array();
    $("#tblcartitems TBODY TR").each(function () {
        var row = $(this);
        var item = {};
        item.Id = Number(row.find("TD").eq(0).html());
        item.ProductId = Number(row.find("TD").eq(1).html());
        item.PicturePath = row.find("TD").eq(2).find("div:eq(0) img").attr('src');
        item.ProductName = row.find("TD").eq(2).find("div:eq(0) p").html();
        item.Price = parseFloat(row.find("TD").eq(3).find('p').html().replace(/,/g, ''));
        item.Discount = Number(row.find("TD").eq(4).find('p').html());
        item.Qty = Number(row.find("TD").eq(5).find('input').val());
        item.Bill = parseFloat(row.find("TD").eq(6).find('p').html().replace(/,/g, ''));
        cartItem.push(item);
    });

    $.ajax({
        url: '/ViewCart/UpdateCart',
        type: 'POST',
        data: { cartItem: cartItem, customerId: 1 },
        success: function (data) {
            alert(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });

}

function logout() {
    $.ajax({
        url: '/Account/Logout',
        success: function (data) {
            window.location = "/";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}
$('#tblcartitems tbody').on('click', "#productqty", function () {
    var currentRow = $(this).closest("tr");
    //get current product total price
    var currentProductTotalPrice = Number(currentRow.find("td:eq(6)").find('p').html().replace(/,/g, '').replace('$ ', ''));
    //get current total amount
    var currentTotalAmount = Number($("#totalAmount").html().replace(/,/g, '').replace('$ ', ''));
    // minus current product total amount from current total amount
    var changedTotalAmount = currentTotalAmount - currentProductTotalPrice;

    var price = Number(currentRow.find("td:eq(3)").find('p').html().replace(/,/g, '').replace('$ ', ''));
    var discount = Number(currentRow.find("td:eq(4)").find('p').html());
    var qty = Number(currentRow.find("td:eq(5)").find('input').val());
    //var changedPrice = price * qty;
    var changedPrice = (price * qty) - ((price * qty) * (qty * discount) / 100);
    currentRow.find("td:eq(6)").find('p').html("$ " + changedPrice.toFixed(2).toLocaleString());

    // get changed/updated product total price
    var changedProductTotalPrice = Number(currentRow.find("td:eq(6)").find('p').html().replace(/,/g, '').replace('$ ', ''));
    // add product total price into total amount
    changedTotalAmount = changedTotalAmount + changedProductTotalPrice;
    //var totalAmount = Number($("#totalAmount").html().replace(/,/g, ''));
    $("#totalAmount").html("$ " + changedTotalAmount.toFixed(2).toLocaleString());

});

function contactUs() {
    var name = $('#name').val();
    var email = $('#email').val();
    var mobile = $('#mobile').val();
    var address = $('#address').val();
    var city = $('#city').find(":selected").text();
    if (name == "") {
        alert('Name is required');
        return false;
    }
    else if (email == "") {
        alert('email is required');
        return false;
    }
    else if (mobile == "") {
        alert('Mobile number is required');
        return false;
    }
    else if (address == "") {
        alert('Address is required');
        return false;
    }
    else if (city == "Choose City") {
        alert('City is required');
        return false;
    }

    else {
        alert("Thank you for contact us.");
    }
}