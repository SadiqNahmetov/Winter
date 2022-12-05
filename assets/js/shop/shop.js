
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