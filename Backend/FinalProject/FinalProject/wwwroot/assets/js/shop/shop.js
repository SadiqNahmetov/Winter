
"use strict"


$(function () {

  let scrollSection = document.getElementById("scrol-navbar-area")

  window.onscroll = function () { scrollFunction() };

  function scrollFunction() {
    if (document.body.scrollTop > 195 || document.documentElement.scrollTop > 195) {
      scrollSection.style.top = "0";
    } else {
      scrollSection.style.top = "-0px";
      scrollSection.classList.remove("visibl");
    }
    }





    // product wishlist start
    let wishlistBtns = document.querySelectorAll("#product-area .card .icon-shop .wishList")

    let products = [];

    if (localStorage.getItem("products")) {
        products = JSON.parse(localStorage.getItem("products"))
    }

    document.querySelector(".heart sup").innerText = getProductsCount(products);
    document.querySelector("#scrol-navbar-area .heart sup").innerHTML = getProductsCount(products);

    wishlistBtns.forEach(wishlistBtn => {

        let productId = parseInt(wishlistBtn.parentNode.parentNode.parentNode.parentNode.getAttribute("cart-id"));

        let existProduct = products.find(m => m.id == productId);
        if (existProduct && products.includes(existProduct)) {
            wishlistBtn.classList.add('heart-active')
        }
        wishlistBtn.addEventListener("click", function (e) {
            e.preventDefault();
            if (!wishlistBtn.classList.contains("heart-active")) {

                wishlistBtn.classList.add("heart-active")

                let productImage = this.parentNode.parentNode.previousElementSibling.getAttribute("src");
                let productBrand = this.parentNode.parentNode.parentNode.nextElementSibling.firstElementChild.innerText;
                let productName = this.parentNode.parentNode.parentNode.nextElementSibling.firstElementChild.nextElementSibling.innerText;
                let productPrice = this.parentNode.parentNode.parentNode.nextElementSibling.nextElementSibling.firstElementChild.innerText;
                let productDisCountPrice = this.parentNode.parentNode.parentNode.nextElementSibling.nextElementSibling.firstElementChild.nextElementSibling.innerText;
                let productId = parseInt(this.parentNode.parentNode.parentNode.parentNode.getAttribute("cart-id"));

                let existProduct = products.find(m => m.id == productId);
                if (existProduct != undefined) {
                    existProduct.count += 0;
                }
                else {
                    products.push({

                        id: productId,
                        brand: productBrand,
                        image: productImage,
                        name: productName,
                        price: productPrice,
                        discountprice: productDisCountPrice,
                        count: 1
                    })
                    document.querySelector(".heart sup").innerText = getProductsCount(products);
                    document.querySelector("#scrol-navbar-area .heart sup").innerHTML = getProductsCount(products);

                }


                localStorage.setItem("products", JSON.stringify(products));
                document.querySelector(".heart sup").innerText = getProductsCount(products);
                document.querySelector("#scrol-navbar-area .heart sup").innerHTML = getProductsCount(products);
            }
            else {

                wishlistBtn.classList.remove("heart-active");

                let productId = parseInt(this.parentNode.parentNode.parentNode.parentNode.getAttribute("cart-id"));

                let existProduct = products.find(m => m.id == productId);
                if (existProduct) {

                    const newProducts = products.filter(m => m !== existProduct);
                    localStorage.setItem('products', JSON.stringify(newProducts));
                    document.querySelector(".heart sup").innerText = getProductsCount(newProducts);
                    document.querySelector("#scrol-navbar-area .heart sup").innerHTML = getProductsCount(newProducts);
                    window.location.reload()
                }

            }


        })
    });

    function getProductsCount(items) {
        return items.length;
    }

    //wishlist end



    let headers = document.querySelectorAll("#product-area .category .my-btn");
    let contents = document.querySelectorAll(".card")

    headers.forEach(header => {
        header.addEventListener("click", function () {
            let activeElem = document.querySelector(".tab-active");
            activeElem.classList.remove("tab-active");
            this.classList.add("tab-active");

            contents.forEach(content => {
                if (this.getAttribute("data-id") == content.getAttribute("data-id")) {
                    content.parentNode.classList.remove("d-none")
                } else {
                    content.parentNode.classList.add("d-none")
                }
                if (this.getAttribute("data-id") == undefined) {
                    content.parentNode.classList.remove("d-none")
                }
            });
        })
    });
    //range input start

    const rangeInput = document.querySelectorAll(".range-input input"),
        priceInput = document.querySelectorAll(".input-price input"),
        progress = document.querySelector(".range .progress");

    let priceGap = 50;

    priceInput.forEach((input) => {
        input.addEventListener("input", (e) => {
            let minVal = parseInt(priceInput[0].value),
                maxVal = parseInt(priceInput[1].value);

            if (maxVal - minVal >= priceGap && maxVal <= 800) {
                if (e.target.className === "input-min") {
                    rangeInput[0].value = minVal;
                    progress.style.left = (minVal / rangeInput[0].max) * 100 + "%";
                } else {
                    rangeInput[1].value = maxVal;
                    progress.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
                }
            }
        });
    });

    rangeInput.forEach((input) => {
        input.addEventListener("input", (e) => {
            let minVal = parseInt(rangeInput[0].value),
                maxVal = parseInt(rangeInput[1].value);

            if (maxVal - minVal < priceGap) {
                if (e.target.className === "range-min") {
                    rangeInput[0].value = maxVal - priceGap;
                } else {
                    rangeInput[1].value = minVal + priceGap;
                }
            } else {
                priceInput[0].value = minVal;
                priceInput[1].value = maxVal;
                progress.style.left = (minVal / rangeInput[0].max) * 100 + "%";
                progress.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
            }
        });
    });

// renge input end

    /// basket start

    $(document).on("click", "#addToCart", function () {

        let id = $(this).attr('cart-id');
        let basketCount = $("#basketCount")
        let basketCurrentCount = $("#basketCount").html()
        $.ajax({
            method: "POST",
            url: "/basket/addbasket",
            data: {
                id: id
            },
            content: "application/x-www-from-urlencoded",
            success: function (res) {
                Swal.fire({
                    icon: 'success',
                    title: 'Product added',
                    showConfirmButton: false,
                    timer: 1500,
                })
                console.log(basketCount);
                basketCurrentCount++;
                basketCount.html("")
                basketCount.append(basketCurrentCount)
            }


        });

    });
// basket end



    $(document).ready(function () {
        $(window).scroll(function () {
            if ($(window).scrollTop() > 200) {
                $('.angleUp').css({
                    "opacity": "1", "pointer-events": "auto"
                });
            } else {
                $('.angleUp').css({
                    "opacity": "0", "pointer-events": "none"
                })
            }
        });
        $('.angleUp').click(function () {
            $('html').animate({ scrollTop: 0 }, 800)
        })
    });
});

