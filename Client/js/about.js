url = "https://proj.ruppin.ac.il/cgroup16/test2/tar1/";
getArtistInfoByName = url + "api/Artists/GetArtistInfoByName?artistName=";
getAllSongsOfArtist = url + "api/Songs/GetAllSongsByArtistName?artistName=";
var artist;
$(document).ready(function () {
  artist = JSON.parse(localStorage.getItem("song")).artistName;
  getArtistImage(artist); //showArtistImage js
  renderArtistInfo();
  document.getElementById("popularity").innerHTML = artist.popularity;
  document.getElementById("quizBtn").setAttribute("class", "notActive");
});

function renderArtistInfo() {
  link = document.getElementById("linkToSpotify");
  link.setAttribute("target", "_blank");
  getArtistInfo();
  getLastFMInfo();
  getAllSongsByArtist();
}

function getAllSongsByArtist() {
  ajaxCall(
    "GET",
    getAllSongsOfArtist + artist,
    "",
    gotAllSongsCB,
    errorAllSongsCB
  );
}
var swiper;

function gotAllSongsCB(data) {
  var counter = 1;
  document.getElementById("songsInDb").innerHTML = data.length;
  data.forEach((song) => {
    // create the main container div
    var containerDiv = document.createElement("div");
    containerDiv.className = "swiper-slide";
    containerDiv.setAttribute("role", "group");
    containerDiv.style.width = "696px";
    containerDiv.style.marginRight = "40px";
    containerDiv.setAttribute("data-swiper-slide-index", counter);
    counter++;
    // create the testimonial item div
    var testimonialItemDiv = document.createElement("div");
    testimonialItemDiv.className = "testimonial-item";

    testimonialItemDiv.setAttribute(
      "style",
      "background-image: url('images/genericMusicPic.jpg')"
    );

    // create the paragraph element
    var paragraph = document.createElement("p");
    paragraph.setAttribute("class", "songText");

    paragraph.textContent = song.songName;

    // create the profile div
    var profileDiv = document.createElement("div");
    profileDiv.className = "profile mt-auto";

    // create the image element
    var image = document.createElement("img");
    image.src = "assets/img/testimonials/testimonials-3.jpg"; //תמונת נקודה
    image.className = "testimonial-img";
    image.alt = "";

    // Create the h3 element for the name
    var nameHeading = document.createElement("h3");
    nameHeading.textContent = song.artistName;

    // Append all the elements in the appropriate hierarchy
    profileDiv.appendChild(image);
    profileDiv.appendChild(nameHeading);

    testimonialItemDiv.appendChild(paragraph);
    testimonialItemDiv.appendChild(profileDiv);

    containerDiv.appendChild(testimonialItemDiv);

    // Add the container div to the document body or any other desired parent element
    var divWrapper = document.getElementById("allSongs");
    divWrapper.appendChild(containerDiv);
  });
}

function errorAllSongsCB(error) {
  console.log(error);
}

function getArtistInfo() {
  ajaxCall(
    "GET",
    getArtistInfoByName + artist,
    "",
    gotArtistInfoCB,
    errorArtistInfoCB
  );
}

function gotArtistInfoCB(data) {
  document.getElementById("popularity").innerHTML = data.popularity;
  document.getElementById("aboutArtist").innerHTML += data.name;
}
function errorArtistInfoCB(error) {
  console.log("failed to get info " + error);
}

function getLastFMInfo() {
  const apiKey = "645890a09eebe9cd0d7bce90c41ff1f1";
  fetch(
    `https://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=${encodeURIComponent(
      artist
    )}&api_key=${apiKey}&format=json`
  )
    .then((response) => response.json())
    .then((data) => {
      const artistInfo = data.artist;

      if (artistInfo.stats.playcount) {
        const playCount = artistInfo.stats.playcount;
        document.getElementById("playCount").innerHTML = playCount;
        document.getElementById("artistSummary").innerHTML =
          artistInfo.bio.summary;
        
      } else {
        document.getElementById("playCount").innerHTML =
          "Play count data not available.";
      }

      if (artistInfo.stats.listeners) {
        const listeners = artistInfo.stats.listeners;
        document.getElementById("listeners").innerHTML = listeners;
      } else {
        document.getElementById("listeners").innerHTML =
          "Listeners data not available.";
      }
    })
    .catch((error) => {
      console.error("Error fetching data:", error);
      const playCountContainer = document.getElementById("playCountContainer");
      playCountContainer.innerHTML =
        "Error fetching data. Please try again later.";
    });
}
