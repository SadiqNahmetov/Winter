
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



/// Product detail area
let imgs = document.querySelectorAll('.img-select a');
    let imgBtns = [...imgs];
    let imgId = 1;

    imgBtns.forEach((imgItem) => {
        imgItem.addEventListener('click', (event) => {
            event.preventDefault();
            imgId = imgItem.dataset.id;
            slideImage();
        });
    });

    function slideImage() {
        let displayWidth = document.querySelector('.img-showcase img:first-child').clientWidth;

        document.querySelector('.img-showcase').style.transform = `translateX(${- (imgId - 1) * displayWidth}px)`;
    }

    window.addEventListener('resize', slideImage);

/// Product detail end


//// Product detail description  
    
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
    
    


    
    
    $(document).ready(function(){
      $(window).scroll(function(){
        if($(window).scrollTop() > 200){
          $('.angleUp').css({
                    "opacity":"1", "pointer-events":"auto"
          });
        }else{
          $('.angleUp').css({
            "opacity":"0","pointer-events":"none"
          })
        }
      });
      $('.angleUp').click(function(){
        $('html').animate({scrollTop:0},800)
      })
    });