updateUser = url + "api/MusicUsers/UpdateUserDetails";

var user;
var firstName;
var lastName;
var email;
var phone;
var userName;
var password;
function showDetails() {
  const dateOfRegistration = JSON.parse(
    localStorage.getItem("user")
  ).dateOfRegistration;
  datePart = dateOfRegistration.split("T")[0];
  datePart = datePart.replaceAll("-", "/");
  Swal.fire({
    title: "Update User Details",
    html:
      '<h2 id="signupDate">Sign up Date: ' +
      datePart +
      "</h2><table>Please fill in all fields" +
      '<tr><input id="changeFirstName" class="swal2-input" placeholder="First Name..."></tr>' +
      '<tr><input id="changeLastName" class="swal2-input" placeholder="Last Name..."></tr>' +
      '<tr><input id="changeEmail" class="swal2-input" placeholder="Email..."></tr>' +
      '<tr><input id="changePhoneNumber" class="swal2-input" placeholder="Phone Number..."></tr>' +
      '<tr><input id="changePassword" class="swal2-input" placeholder="Password..."></tr>' +
      "</table>",
    focusConfirm: false,
    scrollbarPadding: false,
    preConfirm: () => {
      var errorMsg = "";
      firstName = Swal.getPopup()
        .querySelector("#changeFirstName")
        .value.trim();
      lastName = Swal.getPopup().querySelector("#changeLastName").value.trim();
      email = Swal.getPopup().querySelector("#changeEmail").value.trim();
      phone = Swal.getPopup().querySelector("#changePhoneNumber").value.trim();
      userName = JSON.parse(localStorage.getItem("user")).userName;
      password = Swal.getPopup().querySelector("#changePassword").value.trim();

      if (email !== "" && !isValidEmail(email)) {
        errorMsg += "Please enter a valid email address</br>";
      }
      if (phone !== "" && !isValidPhoneNumber(phone)) {
        errorMsg += "Please enter a valid phone number</br>";
      }
      if (!validatePassword(password)) {
        errorMsg +=
          "Please enter a valid password with at least 8 characters, including one digit.</br>";
      }
      if (
        email == "" ||
        password == "" ||
        phone == "" ||
        lastName == "" ||
        firstName == "" ||
        userName == ""
      ) {
        errorMsg = "Please fill in all fields";
      }

      // Display error message if there is one
      if (errorMsg !== "") {
        Swal.showValidationMessage(errorMsg);
        return false;
      }

      return {
        id: JSON.parse(localStorage.getItem("user")).id,
        firstName: firstName,
        lastName: lastName,
        email: email,
        phone: phone,
        userName: userName,
        password: password,
        dateOfRegistration: dateOfRegistration,
      };
    },
  }).then((result) => {
    if (result.isConfirmed) {
      user = {
        id: JSON.parse(localStorage.getItem("user")).id,
        firstName: firstName,
        lastName: lastName,
        email: email,
        phone: phone,
        userName: userName,
        password: password,
        dateOfRegistration: dateOfRegistration,
      };
      ajaxCall(
        "POST",
        updateUser,
        JSON.stringify(user),
        updateSuccess,
        updateFail
      );
      console.log(result.value);
    }
  });
}

function validatePassword(password) {
  const passRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
  return passRegex.test(password);
}

function isValidEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

// Function to validate the phone number format
function isValidPhoneNumber(phoneNumber) {
  const phoneRegex = /^05\d{8}$/;
  return phoneRegex.test(phoneNumber);
}

function updateSuccess(data) {
  localStorage.setItem("user", JSON.stringify(user));
  Swal.fire({
    title: "User Updated!",
    icon: "success",
    scrollbarPadding: false,
  });
  document.getElementById("userFirstName").innerHTML = user.firstName;
}
function updateFail(error) {
  Swal.fire({
    icon: "error",
    title: "Oops...",
    text: "Email/UserName already taken.",
    scrollbarPadding: false,
  });
}
