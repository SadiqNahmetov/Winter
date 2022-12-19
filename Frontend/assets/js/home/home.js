
let swiper = new Swiper(".mySwiperSl",{
    loop:true,
    speed:1500,
    autoplay:true
        
    
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
    speed:1500,
    autoplay: {
      delay: 2000,
    },
    breakpoints:{
      280:{
          slidesPerView:1,
      },
      550:{
          slidesPerView:2,
      },
      800:{
          slidesPerView:2,
      },
      1000:{
          slidesPerView:2,
      },
      1200:{
        slidesPerView:4,
    },
    1400:{
        slidesPerView:4,
    },
  },
  });




  var swiper2 = new Swiper(".mySwiperbrand", {
    slidesPerView: 5,
    speed:1500,
    autoplay:{
      delay:2000
    },
    loop:true,
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

    $(function (){
      let wishLists =  document.querySelectorAll("#slider-product-area .card-pr .icon-shop .wishList");
      let products = [];
        
      if(localStorage.getItem("products") != null){
        products = JSON.parse(localStorage.getItem("products"))
      }
          wishLists.forEach(wishList => {
          wishList.addEventListener("click", function(e){
        
          wishList.classList.toggle("heart-active");

          e.preventDefault();
    
       let productImage =  this.parentNode.parentNode.previousElementSibling.getAttribute("src");
       let productBrand = this.parentNode.parentNode.parentNode.nextElementSibling.firstElementChild.innerText;

       let productName = this.parentNode.parentNode.parentNode.nextElementSibling.firstElementChild.nextElementSibling.innerText;
       let productPrice = this.parentNode.parentNode.parentNode.nextElementSibling.nextElementSibling.firstElementChild.innerText;
       let productDisCountPrice = this.parentNode.parentNode.parentNode.nextElementSibling.nextElementSibling.firstElementChild.nextElementSibling.innerText;
       

       let productId = parseInt(this.parentNode.parentNode.parentNode.parentNode.getAttribute("cart-id"));
       
       let existProduct = products.find(m => m.id == productId);

       if(existProduct != undefined){
          existProduct.count +=0;
       }
          else{
            products.push({
              id:productId,
              name:productName,
              brand:productBrand,
              price:productPrice,
              image:productImage,
              disCountPrice:productDisCountPrice,
              count:1
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
    document.querySelector("#scrol-navbar-area .heart sup").innerHTML = heart.length;
    console.log(heart.length);
  }
  
  GetCount();




  