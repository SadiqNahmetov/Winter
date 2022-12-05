
let swiper = new Swiper(".mySwiper",{
    loop:true,
    autoplay:{
        delay:2000
    }
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