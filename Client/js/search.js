// Define the base URL and API endpoint for making API calls
var url = "https://proj.ruppin.ac.il/cgroup16/test2/tar1/";
const fullUrl = url + "api/Songs/";

// Get the element to display the search results
const showing = document.getElementById("showing");//הכיתוב מעל השירים

// Function to handle the search button click event
function searchClick() {
  // Show a custom modal dialog using SweetAlert2 library for user input
  Swal.fire({
    title: "What do you want to search by?",
    showDenyButton: true,
    confirmButtonText: "Search",
    denyButtonText: "Cancel",
    showCloseButton: true,
    html: `<div class="swal2-radio">
            <input type="radio" id="searchByArtist" name="searchOption" checked="checked" value="GetAllSongsByArtistName?artistName=">
            <label for="searchByArtist">Search by Artist Name</label>
          </div>
          <div class="swal2-radio">
            <input type="radio" id="searchBySong" name="searchOption" value="GetSongsByName?songName=">
            <label for="searchBySong">Search by Song Name</label>
          </div>
          <div class="swal2-radio">
            <input type="radio" id="searchByLyrics" name="searchOption" value="GetAllSongsByLyrics?lyrics=">
            <label for="searchByLyrics">Search by Song Lyrics</label>
          </div>
          <input type="text" id="swal-input1" class="swal2-input" placeholder="Enter your search...">
          `,
    showLoaderOnConfirm: true,
    scrollbarPadding: false,
    heightAuto: false,
    preConfirm: () => {
      // set the active class for the search button when searching
      document.getElementById("homeBtn").setAttribute("class", "notActive");
      document.getElementById("favBtn").setAttribute("class", "notActive");
      document.getElementById("searchBtn").setAttribute("class", "active");

      return new Promise((resolve) => {
        // Get the selected search option and user input
        const searchOption = document.querySelector(
          'input[name="searchOption"]:checked'
        ).value;
        const searchValue = document.getElementById("swal-input1").value;

        // Update the 'showing' element based on the selected search option and user input
        switch (searchOption) {
          case "GetAllSongsByArtistName?artistName=":
            showing.innerHTML = "Showing all songs by Artist: " + searchValue;
            break;
          case "GetSongsByName?songName=":
            showing.innerHTML = "Showing all songs by name: " + searchValue;
            break;
          case "GetAllSongsByLyrics?lyrics=":
            showing.innerHTML =
              "Showing all songs that contain the words: " + searchValue;
            break;
          default:
            break;
        }

        // Log the search operation and make an AJAX call to the API
        console.log(`Searching by ${searchOption}: ${searchValue}`);
        ajaxCall(
          "GET",
          fullUrl + searchOption + searchValue,
          "",
          GetAllSongsSuccess,
          ErrorGetSong
        );
        resolve();
      });
    },
  });
  
}

// Function to handle the error when songs are not found
function ErrorGetSong(error) {
  Swal.fire({
    position: "center",
    icon: "error",
    title: "Oops!\nSongs not found!",
    showConfirmButton: false,
    scrollbarPadding: false,
    timer: 2500,
  });

  // Clear the search results and display a songs by the user inputs
  allSongs = document.getElementById("allSongs");
  allSongs.innerHTML = "";
  showing.innerHTML = "No songs found";
}


