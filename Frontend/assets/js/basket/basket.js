


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


