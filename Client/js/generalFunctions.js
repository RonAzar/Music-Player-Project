//for when the user clicks on home button from other pages
function homeAll() {
  localStorage.setItem("indicator", "home");
  window.open("index.html", "_self");
}
//for when the user clicks on the favorites button from other pages
function homeFavorites() {
  localStorage.setItem("indicator", "favorites");
  window.open("index.html", "_self");
}

//when user click on logout button=> remove user from localStorage and go to log in screen
function logOut() {
  localStorage.removeItem("user");
  window.open("login.html", "_self");
}
//calls the function that renders all the songs in the home page
function clickHome() {
  localStorage.setItem("indicator", "home");
  whatToRender();
}

//calls the function that renders all the favorite songs in the home page
function clickFavorites() {
  localStorage.setItem("indicator", "favorites");
  whatToRender();
}

//Use logIn ajaxCall
function logIn() {
  ajaxCall(
    "POST",
    loginURL + $("#email").val() + "&passwordToLogin=" + $("#pass").val(),
    "",
    successLoginCB, //Register and Login pages using different successLoginCB
    errorLoginCB
  );
}

//error LoginCB
function errorLoginCB(error) {
  Swal.fire({
    position: "center",
    icon: "error",
    title: "Oops! something went wrong!\nCheck the details please!",
    showConfirmButton: false,
    scrollbarPadding: false,
    heightAuto: false,
    timer: 2500,
  });
}

//saves user data in localStorage
function saveUserData(data) {
  console.log(data);
  localStorage.setItem("user", JSON.stringify(data));
}

function ronoam() {
  ronnoam = document.getElementById("ronoam");
  ronnoam.href = "additions.html";
  ronnoam.setAttribute("target", "_blank");
}
ronoam();
