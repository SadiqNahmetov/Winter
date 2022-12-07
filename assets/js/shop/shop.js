
"use strict"

let headers = document.querySelectorAll("#product-area .category .my-btn");
let contents = document.querySelectorAll(".card")

headers.forEach(header => {
    header.addEventListener("click",function() {
        let activeElem = document.querySelector(".tab-active");
        activeElem.classList.remove("tab-active");
        this.classList.add("tab-active");

        contents.forEach(content => {
            if(this.getAttribute("data-id") == content.getAttribute("data-id")){
                content.parentNode.classList.remove("d-none")
            }else{
                content.parentNode.classList.add("d-none")
            }
            if(this.getAttribute("data-id")=="0"){
                content.parentNode.classList.remove("d-none")
            }
        });
    })
});


window.onload = function () {
    slideOne();
    slideTwo();
};

let sliderOne = document.querySelector("#slider-1");
let sliderTwo = document.querySelector("#slider-2");

let displayValOne = document.getElementById("range1");
let displayValTwo = document.getElementById("range2");

let minRag = 10;

let sliderTrack = document.querySelector(".slider-track");
let sliderMaxValue = document.getElementById("slider-1").max;


function slideOne() {
    if (parseInt(sliderTwo.value) - parseInt(sliderOne.value) <= minRag) {
        sliderOne.value = parseInt(sliderTwo.value) - minRag;
    }
    displayValOne.textContent = sliderOne.value;
    fillColor();
}

function slideTwo() {

    if (parseInt(sliderTwo.value) - parseInt(sliderOne.value) <= minRag) {
        sliderTwo.value = parseInt(sliderOne.value) + minRag;
    }
    displayValTwo.textContent = sliderTwo.value;
    fillColor();
}


function fillColor() {

   let percent1 = (sliderOne.value / sliderMaxValue) * 100;
   let percent2 = (sliderTwo.value / sliderMaxValue) * 100;

    sliderTrack.style.background = `linear-gradient(to right, #dadae5 ${percent1}% ,
        #FF3368 ${percent1}% , #FF3368 ${percent2}%, #dadae5 ${percent2}%)`;
}
