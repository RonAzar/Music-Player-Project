const api = "https://proj.ruppin.ac.il/cgroup16/test2/tar1/";
const getAllFavorites = api + "api/MusicUsers/GetFavorites?userId=";
const registerUserURL = api + "api/MusicUsers/Registration";
const loginURL = api + "api/MusicUsers/LogIn?emailOrUserNameToLogin=";

var user;
$(document).ready(function () {
  $("#signup-form").submit(function (e) {
    e.preventDefault();

    // Get the form values
    const userName = $("#userName").val();
    const firstName = $("#firstName").val();
    const lastName = $("#lastName").val();
    const email = $("#email").val();
    const phoneNumber = $("#phone").val();
    const password = $("#pass").val();

    // Perform validations
    if (!validateEmail(email)) {
      Swal.fire({
        position: "center",
        icon: "warning",
        title: "Please enter a valid email address.",
        showConfirmButton: false,
        scrollbarPadding: false,
        heightAuto: false,
        timer: 2500,
      });
      return;
    }
    if (!validatePhoneNumber(phoneNumber)) {
      Swal.fire({
        position: "center",
        icon: "warning",
        title: "Please enter a valid 10-digit phone number...",
        showConfirmButton: false,
        scrollbarPadding: false,
        heightAuto: false,
        timer: 2500,
      });
      return;
    }
    if (!validatePassword(password)) {
      Swal.fire({
        position: "center",
        icon: "warning",
        title:
          "Please enter a valid password with at least 8 characters, including one digit.",
        showConfirmButton: false,
        scrollbarPadding: false,
        heightAuto: false,
        timer: 3000,
      });
      return;
    }
    // Function to validate the email format
    function validateEmail(email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(email);
    }

    // Function to validate the phone number format
    function validatePhoneNumber(phoneNumber) {
      const phoneRegex = /^05\d{8}$/;
      return phoneRegex.test(phoneNumber);
    }
    function validatePassword(password) {
      const passRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
      return passRegex.test(password);
    }

    //keep all user data in user object
    user = {
      firstName: String($("#firstName").val()),
      lastName: String($("#lastName").val()),
      email: String($("#email").val()),
      password: String($("#pass").val()),
      phone: String($("#phone").val()),
      userName: String($("#userName").val()),
    };
    //use ajaxCall to register User
    ajaxCall(
      "POST",
      registerUserURL,
      JSON.stringify(user),
      successRegisterCB,
      errorRegisterCB
    );
    e.preventDefault();
  });
});
//success RegisterCB
function successRegisterCB(data) {
  localStorage.setItem("indicator", "home");
  localStorage.setItem("favoriteSongs", []);
  Swal.fire({
    position: "center",
    icon: "success",
    title: "User added successfully! Logging you in...",
    showConfirmButton: false,
    scrollbarPadding: false,
    heightAuto: false,
    timer: 2500,
  });
  // Delay the redirect by 1.5 seconds
  setTimeout(function () {
    //logIn in generalFunctions JS file
    logIn();
  }, 1000);
}

//success LoginCB
function successLoginCB(data) {
  localStorage.setItem("user", JSON.stringify(data)); // Pass the data as an argument to the saveUserData function
  // Delay the redirect by 1.5 seconds
  setTimeout(function () {
    window.open("index.html", "_self");
  }, 1000);
}

//error RegisterCB
function errorRegisterCB(error) {
  Swal.fire({
    position: "center",
    icon: "error",
    title: "A user with that username/email already exists!",
    showConfirmButton: false,
    scrollbarPadding: false,
    heightAuto: false,
    timer: 2500,
  });
}
