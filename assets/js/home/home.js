
let swiper = new Swiper(".mySwiper",{
    loop:true,
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
          slidesPerView:1,
      },
      1000:{
          slidesPerView:2,
      },
      1200:{
        slidesPerView:2,
    },
    1400:{
        slidesPerView:4,
    },
  },
  });




  var swiper2 = new Swiper(".mySwiperbrand", {
    slidesPerView: 5,
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