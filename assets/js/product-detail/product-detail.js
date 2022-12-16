
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




//////////// Product detail area

function changeSlide(evt, Product_tumbnailName) {
    var i, x, slide_options;
    x = document.getElementsByClassName("Product_tumbnail");
    for (i = 0; i < x.length; i++) {
    x[i].style.display = "none";
    }
    slide_options = document.getElementsByClassName("slide_option");
    for (i = 0; i < x.length; i++) {
    slide_options[i].className = slide_options[i].className.replace(" slide_image_color", "");
    }
    document.getElementById(Product_tumbnailName).style.display = "block";
    evt.currentTarget.className += " slide_image_color";
    }
    
    
    function opentab_type(evt, tab_typeName) {
    var i, x, tablinks;
    x = document.getElementsByClassName("tab_type");
    for (i = 0; i < x.length; i++) {
    x[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablink");
    for (i = 0; i < x.length; i++) {
    tablinks[i].className = tablinks[i].className.replace("active_button", ""); 
    }
    document.getElementById(tab_typeName).style.display = "block";
    evt.currentTarget.className += " active_button";
    }
    
    
    
    
    
    //SCROLL ANIMATE
    var scroll = window.requestAnimationFrame ||
    function(callback){ window.setTimeout(callback, 1000/60)};
    var elementsToShow = document.querySelectorAll('.show-on-scroll'); 
    function loop() {
    
    Array.prototype.forEach.call(elementsToShow, function(element){
    if (isElementInViewport(element)) {
    element.classList.add('is-visible');
    } else {
    element.classList.remove('is-visible');
    }
    });
    
    scroll(loop);
    }
    loop();
    
    function isElementInViewport(el) {
    // special bonus for those using jQuery
    if (typeof jQuery === "function" && el instanceof jQuery) {
    el = el[0];
    }
    var rect = el.getBoundingClientRect();
    return (
    (rect.top <= 0
    && rect.bottom >= 0)
    ||
    (rect.bottom >= (window.innerHeight || document.documentElement.clientHeight) &&
    rect.top <= (window.innerHeight || document.documentElement.clientHeight))
    ||
    (rect.top >= 0 &&
    rect.bottom <= (window.innerHeight || document.documentElement.clientHeight))
    );
    }
    
    
    
    
    //SCROLL ANIMATE
    function changeSlide(evt, Product_tumbnailName) {
        var i, x, slide_options;
        x = document.getElementsByClassName("Product_tumbnail");
        for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
        }
        slide_options = document.getElementsByClassName("slide_option");
        for (i = 0; i < x.length; i++) {
        slide_options[i].className = slide_options[i].className.replace(" slide_image_color", "");
        }
        document.getElementById(Product_tumbnailName).style.display = "block";
        evt.currentTarget.className += " slide_image_color";
        }
        
        
        function opentab_type(evt, tab_typeName) {
        var i, x, tablinks;
        x = document.getElementsByClassName("tab_type");
        for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablink");
        for (i = 0; i < x.length; i++) {
        tablinks[i].className = tablinks[i].className.replace("active_button", ""); 
        }
        document.getElementById(tab_typeName).style.display = "block";
        evt.currentTarget.className += " active_button";
        }
        
        
        
        
        
        //SCROLL ANIMATE
        var scroll = window.requestAnimationFrame ||
        function(callback){ window.setTimeout(callback, 1000/60)};
        var elementsToShow = document.querySelectorAll('.show-on-scroll'); 
        function loop() {
        
        Array.prototype.forEach.call(elementsToShow, function(element){
        if (isElementInViewport(element)) {
        element.classList.add('is-visible');
        } else {
        element.classList.remove('is-visible');
        }
        });
        
        scroll(loop);
        }
        loop();
        
        function isElementInViewport(el) {
        // special bonus for those using jQuery
        if (typeof jQuery === "function" && el instanceof jQuery) {
        el = el[0];
        }
        var rect = el.getBoundingClientRect();
        return (
        (rect.top <= 0
        && rect.bottom >= 0)
        ||
        (rect.bottom >= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.top <= (window.innerHeight || document.documentElement.clientHeight))
        ||
        (rect.top >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight))
        );
        }
        
        
        
        
        //SCROLL ANIMATE
        var scroll2 = window.requestAnimationFrame ||
        function(callback2){ window.setTimeout(callback2, 1000/60)};
        var elementsToShow2 = document.querySelectorAll('.show-on-scroll2'); 
        function loop2() {
        
        Array.prototype.forEach.call(elementsToShow2, function(element){
        if (isElementInViewport(element)) {
        element.classList.add('is-visible2');
        } else {
        element.classList.remove('is-visible2');
        }
        });
        
        scroll2(loop2);
        }
        loop2();
        
        function isElementInViewport(el) {
        // special bonus for those using jQuery
        if (typeof jQuery === "function" && el instanceof jQuery) {
        el = el[0];
        }
        var rect2 = el.getBoundingClientRect();
        return (
        (rect2.top <= 0
        && rect2.bottom >= 0)
        ||
        (rect2.bottom >= (window.innerHeight || document.documentElement.clientHeight) &&
        rect2.top <= (window.innerHeight || document.documentElement.clientHeight))
        ||
        (rect2.top >= 0 &&
        rect2.bottom <= (window.innerHeight || document.documentElement.clientHeight))
        );
        }