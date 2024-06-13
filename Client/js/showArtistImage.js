var expiresIn;
const clientId = "76eb58574dca4292942f6b50535fe9b9";
const clientSecret = "df624510f02e46a39a9f04d250b1cde8";

let accessToken = null;
let tokenExpirationTime = 0;

// Fetch access token using Client Credentials Flow
function getAccessToken() {
  if (Date.now() < tokenExpirationTime) {
    const secondsLeft = Math.ceil((tokenExpirationTime - Date.now()) / 1000);
    console.log(`Access token still valid. Seconds left: ${secondsLeft}`);
    return Promise.resolve(accessToken); // Token still valid, no need to refresh
  }

  const authString = `${clientId}:${clientSecret}`;
  const encodedAuth = btoa(authString);
  const headers = {
    "Content-Type": "application/x-www-form-urlencoded",
    Authorization: `Basic ${encodedAuth}`,
  };
  const body = "grant_type=client_credentials";

  return axios
    .post("https://accounts.spotify.com/api/token", body, { headers })
    .then((response) => {
      accessToken = response.data.access_token;
      expiresIn = response.data.expires_in;
      //console.log(expiresIn); REMOVE COMMENT TO SEE EXIPRIATION
      return accessToken;
    })
    .catch((error) => {
      console.log("Error occurred while getting access token:", error.message);
      return null;
    });
}

// Fetch artist information and images
function getArtistImages(artistName, accessToken) {
  const apiUrl = `https://api.spotify.com/v1/search?q=${encodeURIComponent(
    artistName
  )}&type=artist`;

  axios
    .get(apiUrl, { headers: { Authorization: `Bearer ${accessToken}` } })
    .then((response) => {
      const artists = response.data.artists.items;
      if (artists.length > 0) {
        const artistInfo = artists[0];
        const artistImages = artistInfo.images || [];
        if (artistImages.length > 0) {
          document.getElementById("artistImg").src = artistImages[0].url;
          document
            .getElementById("linkToSpotify")
            .setAttribute("href", artistInfo.external_urls.spotify);
        } else {
          console.log(`No images found for ${artistInfo.name}.`);
          document.getElementById("artistImg").src = "/images/artistImage.jpg";
        }
      } else {
        console.log(
          `Artist "${artistName}" not found in the Spotify API response.`
        );
      }
      const genres = artists[0].genres;
      genresDiv = document.getElementById("genres");
      genresDiv.innerHTML += " " + genres[0];
      let i = 0;
      genres.forEach((genre) => {
        if (i > 0) {
          genresDiv.innerHTML += " | " + genre;
        }
        i++;
      });
    })
    .catch((error) => {
      console.log("Error occurred:", error.message);
    });
}

// Entry point (called on page load)
function getArtistImage(artistToShow) {
  const artist = artistToShow;
  getAccessToken().then((token) => {
    if (token) {
      getArtistImages(artist, token);
    }
  });
}
