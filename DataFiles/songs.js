function load() {
  // Fetch the CSV file asynchronously
  fetch("songs.csv")
    .then((response) => response.text())
    .then((csvData) => {
      // Parse CSV data using PapaParse
      const parsedData = Papa.parse(csvData, {
        header: true,
        dynamicTyping: true,
      });

      // Retrieve the songs from the parsed data
      const songs = parsedData.data;

      // Retrieve unique artists
      const uniqueArtists = [...new Set(songs.map((song) => song.artist))];

      // Shuffle the array of artists
      const shuffledArtists = shuffleArray(uniqueArtists);

      // Select 100 random unique artists
      const selectedArtists = shuffledArtists.slice(0, 100);

      // Filter songs based on selected artists
      const selectedSongs = songs.filter((song) =>
        selectedArtists.includes(song.artist)
      );

      // Shuffle the array of songs
      const shuffledSongs = shuffleArray(selectedSongs);

      // Select 5 songs for each artist and additional songs if necessary
      const finalSongs = [];
      for (const artist of selectedArtists) {
        const artistSongs = shuffledSongs.filter(
          (song) => song.artist === artist
        );
        const additionalSongs = artistSongs.slice(
          0,
          5 - (finalSongs.length % 5)
        );
        finalSongs.push(...additionalSongs);

        if (finalSongs.length >= 500) {
          break;
        }
      }

      // Add additional artists and their songs if necessary
      let remainingSongs = 500 - finalSongs.length;
      if (remainingSongs > 0) {
        const remainingArtists = shuffledArtists.slice(100);
        for (const artist of remainingArtists) {
          const artistSongs = shuffledSongs.filter(
            (song) => song.artist === artist
          );
          const additionalSongs = artistSongs.slice(0, remainingSongs);
          finalSongs.push(...additionalSongs);
          remainingSongs -= additionalSongs.length;

          if (remainingSongs <= 0) {
            break;
          }
        }
      }

      // Save the selected songs to a JSON file
      const jsonData = JSON.stringify(finalSongs, null, 4);
      const jsonBlob = new Blob([jsonData], { type: "application/json" });
      const jsonURL = URL.createObjectURL(jsonBlob);
      const downloadLink = document.createElement("a");
      downloadLink.href = jsonURL;
      downloadLink.download = "selected_songs.json";
      downloadLink.click();

      console.log("Selected songs have been saved to 'selected_songs.json'.");
    })
    .catch((error) => {
      console.error("An error occurred:", error);
    });

  // Helper function to shuffle an array in place
  function shuffleArray(array) {
    for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
  }
}

//VALIDATE DATA

// var artistsArray = [];
// function load() {
//   array.forEach((song) => {
//     if (!artistsArray.includes(song.artist)) {
//       artistsArray.push(song.artist);
//     }
//   });
//   console.log(artistsArray.length);
//   console.log(array.length);
// }
