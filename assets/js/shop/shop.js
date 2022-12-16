
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

});




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
      if (this.getAttribute("data-id") == "0") {
        content.parentNode.classList.remove("d-none")
      }
    });
  })
});

const rangeInput = document.querySelectorAll(".range-input input"),
  priceInput = document.querySelectorAll(".price-input input"),
  range = document.querySelector(".slider .progress");
let priceGap = 150;

priceInput.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minPrice = parseInt(priceInput[0].value),
      maxPrice = parseInt(priceInput[1].value);

    if (maxPrice - minPrice >= priceGap && maxPrice <= rangeInput[1].max) {
      if (e.target.className === "input-min") {
        rangeInput[0].value = minPrice;
        range.style.left = (minPrice / rangeInput[0].max) * 100 + "%";
      } else {
        rangeInput[1].value = maxPrice;
        range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
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
      range.style.left = (minVal / rangeInput[0].max) * 100 + "%";
      range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
    }
  });
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