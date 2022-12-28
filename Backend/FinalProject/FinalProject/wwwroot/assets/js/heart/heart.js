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


    let products = [];

    if (localStorage.getItem("products") != undefined) {
           products = JSON.parse(localStorage.getItem("products"))
   }
  let tableBody = document.querySelector(".table .tb-body");

   addDatasToTable(products);
   showIconBasketCount();

let deleteBtns = document.querySelectorAll(".delete-icon");

   deleteBtns.forEach(btn => {
         btn.addEventListener("click", function () {
        deleteProducts(this);
       Swal.fire({
       position: 'top-end',
       icon: 'success',
       title: 'Product deleted',
       showConfirmButton: false,
       timer: 1000
    })

  })
});

function deleteProducts(btn) {

  let id = parseInt(btn.parentNode.parentNode.firstElementChild.getAttribute("heart-id"));

  products = products.filter(m => m.id != id);

  localStorage.setItem("products", JSON.stringify(products));

  btn.parentNode.parentNode.remove();

  showIconBasketCount();

}
function addDatasToTable(products) {
  for (const product of products) {
    tableBody.innerHTML += `<tr>
              <td heart-id="${product.id}"><img src="${product.image}" style="width:170px; height:170px" alt=""></td>
              <td>${product.name}</td>
              <td>${product.brand}</td>
              <td>${product.discountprice}</td>
              <td>${product.price}</td>
             
              <td class="basket-delete"><i class="fa-solid fa-cart-plus basket-heart"></i>
              <i class="fa-solid fa-trash delete-icon"></i>
              </td>
              </tr>
              `
  }
}
function getProductsCount(items) {
  let resultCount = 0;

  for (const item of items) {
    resultCount += item.count
  }
  return resultCount;
}
function showIconBasketCount() {
  document.querySelector(".heart sup").innerHTML = getProductsCount(products);
  document.querySelector("#scrol-navbar-area .heart sup").innerHTML = getProductsCount(products);
}


