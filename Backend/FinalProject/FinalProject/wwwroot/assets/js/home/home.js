"use strict"


$(function () {

    let swiper = new Swiper(".mySwiperSl", {
        loop: true,
        speed: 1300,
        autoplay: true
    })



    var swiper1 = new Swiper(".mySwiper1", {
        slidesPerView: 4,
        spaceBetween: 30,
        freeMode: true,
    });


    var swiper1 = new Swiper(".mySwiper1", {
        slidesPerView: 4,
        spaceBetween: 20,
        loop: true,
        speed: 1500,
        autoplay: {
            delay: 2000,
        },
        breakpoints: {
            280: {
                slidesPerView: 1,
            },
            550: {
                slidesPerView: 2,
            },
            800: {
                slidesPerView: 2,
            },
            1000: {
                slidesPerView: 2,
            },
            1200: {
                slidesPerView: 4,
            },
            1400: {
                slidesPerView: 4,
            },
        },
    });



    var swiper2 = new Swiper(".mySwiperbrand", {
        slidesPerView: 5,
        speed: 1500,
        autoplay: {
            delay: 2000
        },
        loop: true,
        spaceBetween: 100,
        breakpoints: {
            0: {
                slidesPerView: 1,
            },
            550: {
                slidesPerView: 2,
            },
            800: {
                slidesPerView: 3,
            },
            1000: {
                slidesPerView: 4,
            },
            1200: {
                slidesPerView: 5,
            },
        }

    });




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


    let wishlistBtns = document.querySelectorAll("#slider-product-area .card-pr .icon-shop .wishList")

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
                    window.location.reload();
                }

            }

        })
    });

    function getProductsCount(items) {
        return items.length;
    }

    /// basket start

    $(document).on("click", "#addToCart", function () {

        //let productId = parseInt($(this).closest(".nahmetov").children(0).val());
        //let data = { id: productId }
        let id = $(this).attr('cart-id');

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
                    timer: 1500
                })
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














