
"use strict"


$(function (){
  
  let scrollSection = document.getElementById("scrol-navbar-area")
   
  window.onscroll = function () {scrollFunction()};

  function scrollFunction(){
    if (document.body.scrollTop > 195 || document.documentElement.scrollTop > 195) {
      scrollSection.style.top = "0";    
   } else {
      scrollSection.style.top = "-0px";
      scrollSection.classList.remove("visibl");
   }
  }

});





if(localStorage.getItem('products') === null) {
  localStorage.setItem('products',JSON.stringify([]))
}

$(function () {
  let wishLists = document.querySelectorAll("#product-area .card .icon-shop .wishList");
  let products = [];

  if (localStorage.getItem("products") != null) {
    products = JSON.parse(localStorage.getItem("products"))
  }


  wishLists.forEach(wishList => {
    wishList.addEventListener("click", function (e) {

      wishList.classList.toggle("heart-active");
      e.preventDefault();

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
          name: productName,
          brand: productBrand,
          price: productPrice,
          image: productImage,
          disCountPrice: productDisCountPrice,
          count: 1
        })
      }

      localStorage.setItem("products", JSON.stringify(products));
      GetCount();
    })
  })
});

function GetCount() {
  let heart = JSON.parse(localStorage.getItem('products'))
  document.querySelector('.heart sup').innerHTML = heart.length;
  document.querySelector("#scrol-navbar-area .heart sup").innerHTML =heart.length;
  console.log(heart.length);
}

GetCount();


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