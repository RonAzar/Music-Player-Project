url = "https://proj.ruppin.ac.il/cgroup16/test2/tar1/";
const getLeaderBoard = url + "api/MusicUsers/GetTopFiveScoreBoard";
const getAllTheSongs = url + "api/Songs/GetAllSongs";
const getUserScoresURL = url + "api/MusicUsers/GetUserScores?userId=";
const get3ArtistsURL = url + "api/Artists/Get3RandomsArtists?artistName=";
getAllArtistsAPI = url + "api/Artists/GetAllArtists";
insertScoreURL = url + "api/MusicUsers/InsertScore";
allSongs = [];
allArtists = [];
var saveCorrectAnswerId = 0;
$(document).ready(function () {
  getAllSongsFromDB();
  getAllArtistsFromDB();
});
var questions = [];
var count = 0;
var score = 0;
var userId = JSON.parse(localStorage.getItem("user")).id;
function openQuizPage() {
  Swal.fire({
    title: "Your highest score: ",
    html: '<button class="quizBtn" id="play-now-btn" onclick="loadQuizPage()">Play now!</button><br><button class="quizBtn" id="leaderboard-btn" onclick="getLeaderBoardData()">Show leaderboard!</button>',
    showConfirmButton: false,
    allowOutsideClick: true,
    showCloseButton: true,
    scrollbarPadding: false,
  }).then((result) => {
    if (result.isConfirmed) {
      Swal.fire({
        title: "Game Started!",
        text: "Have fun playing the game!",
        icon: "success",
        scrollbarPadding: false,
      });
    } else if (result.isDismissed) {
      // Handle the modal being closed without clicking the buttons (e.g., clicking outside the modal)
      console.log("Modal closed without clicking buttons.");
    }
  });
  getUserScores();
}
const leaderboardBtn = document.getElementById("leaderboard-btn");
const playNowBtn = document.getElementById("play-now-btn");

function getLeaderBoardData() {
  ajaxCall(
    "GET",
    getLeaderBoard,
    "",
    successGetLeaderBoardCB,
    faliedGetLeaderBoardCB
  );
}

function successGetLeaderBoardCB(data) {
  var containAllLeaderBoard = "";

  var i = 1;
  data.forEach((userScore) => {
    containAllLeaderBoard +=
      `<div class="dotsFather" style="display: flex; width:80%; margin:auto; text-align: center; justify-content: space-between; margin-bottom: 5px;">` +
      `<span style="text-align: left;">` +
      i +
      ". " +
      userScore.userName +
      `</span>` +
      `<span class="dots"></span>` +
      `<span style="text-align: right;">` +
      userScore.userScore +
      `</span></div>`;
    i++;
  });

  // Add the "Back" button to the content
  Swal.fire({
    title: "Top 5 - Leaderboard:",
    html: containAllLeaderBoard,
    icon: "info",
    showConfirmButton: false,
    scrollbarPadding: false, // Set this to false to remove the OK button
  });
}

function faliedGetLeaderBoardCB(error) {
  console.log(error);
}

function getUserScores() {
  ajaxCall("GET", getUserScoresURL + userId, "", gotScores, errorScores);
}
function gotScores(data) {
  if (data[0] == null) {
    document.getElementById("swal2-title").innerHTML += "0";
  } else document.getElementById("swal2-title").innerHTML += data[0].userScore;
}

function errorScores(data) {
  document.getElementById("swal2-title").innerHTML = "Quiz";
}

// Step 2: Create a function to show the custom modal
function loadQuizPage() {
  window.open("quiz.html", "_self");
}
function generateQuestion() {
  questionTypeNumber = Math.floor(Math.random() * 4) + 1;
  switch (questionTypeNumber) {
    case 1:
      whoWroteTheSong();
      break;
    case 2:
      whichSongHasTheMostLyrics();
      break;
    case 3:
      whichSongWrittenBy();
      break;
    case 4:
      whichArtistIsTheMostPopular();
      break;
    case 5:
      //which artist was played the most?
      whichArtistIsTheMostPlayer();
      break;
    default:
      console.log("Invalid number generated.");
      break;
  }
}

// Global variables
var timerValue = 0;
var bonusPoints = 30;
var timerInterval;

function updateTimer() {
  if (timerValue == 41) {
    clearInterval(timerInterval);
    gameTimeEnded();
  } else {
    timerValue++;
    if (bonusPoints > 0) bonusPoints--;
  }
  console.log("bonus: " + bonusPoints, "timer: " + timerValue);
}

function gameTimeEnded() {
  nextBtn = document.getElementById("nextBtn");
  nextBtn.innerHTML = "Finish quiz!";
  nextBtn.style.visibility = "visible";
  nextBtn.setAttribute("onclick", "endGame()");
  Swal.fire({
    icon: "info",
    title: "Time ended!",
    showConfirmButton: true,
  }).then((result) => {
    if (result.isConfirmed) {
      // Handle the action when "Show More Info" is clicked
      endGame();
    }
  });
}

function startQuiz() {
  timerValue = 0;
  bonusPoints = 30;
  // Start the timer
  timerInterval = setInterval(updateTimer, 1000);
  count = 0;
  score = 0;
  setTimeout(renderForm, 200);
  updatePlayerScore();
  updateQuestionCounter();
  loadQuestions();
  setTimeout(renderAllQuestions, 200);
  Swal.close();
}
function renderForm() {
  quizDiv = document.getElementById("quizDiv");
  quizDiv.innerHTML = `<div class="container"><div style="margin:5vh">
    <h1 id = "questionHeader">Quiz Form</h1></div>
    <div id = "timerHeader" class="countdown"><svg viewBox="-50 -50 100 100" stroke-width="10"><circle r="35"></circle>
    <circle r="35" pathLength="1"></circle></div>
    <div id="quizContainer">
      <div id="questionContainer"></div>
      <button class="quizAnswerBtn" id="answer1">NA</button>
      <button class="quizAnswerBtn" id="answer2">NA</button>
      <button class="quizAnswerBtn" id="answer3">NA</button>
      <button class="quizAnswerBtn" id="answer4">NA</button>
      <br /><button id="nextBtn" class = "nextQuizBtn" onclick="nextQuestion(questions[count])" style="visibility:hidden;">Next</button>
    </div>
    <div id="resultContainer"></div>
  </div>`;
}
function loadQuestions() {
  for (var i = 0; i < 6; i++) {
    generateQuestion();
  }
}

function startQuestionTimer() {
  const timer = setTimeout(timerOut, 10000);
}

function whoWroteTheSong() {
  randomSongNumber = Math.floor(Math.random() * allSongs.length);
  song = allSongs[randomSongNumber];
  threeArtists = get3Artists(song.artistName);
  question = {
    questionTxt: "Who wrote the song: " + song.songName,
    correctAnswer: song.artistName,
    wrongAnswers: threeArtists,
  };

  questions.push(question);
}

function renderAllQuestions() {
  renderQuestion(questions[count]);
}

function renderQuestion(question) {
  questionContainer = document.getElementById("questionContainer");
  document.getElementById("questionHeader").innerHTML = question.questionTxt;
  randomBtn = Math.floor(Math.random() * 4) + 1;
  answer = document.getElementById("answer" + randomBtn);
  answer.innerHTML = question.correctAnswer;
  answer.setAttribute("onclick", "correctAnswer(this.id)");
  saveCorrectAnswerId = answer.id;
  console.log(saveCorrectAnswerId);
  wrongIdx = 0;
  for (var i = 1; i < 5; i++) {
    answer = document.getElementById("answer" + i);
    if (answer.innerHTML == "NA") {
      answer.innerHTML = question.wrongAnswers[wrongIdx];
      wrongIdx++;
      answer.setAttribute("onclick", "wrongAnswer(this.id)");
    }
  }
}
function resetButtons() {
  document.getElementById("nextBtn").style.visibility = "hidden";
  for (var i = 1; i < 5; i++) {
    answer = document.getElementById("answer" + i);
    answer.setAttribute("onclick", "wrongAnswer(this.id)");
    answer.innerHTML = "NA";
    answer.style.backgroundColor = "rgb(110, 110, 110)";
    answer.disabled = false;
  }
}
function nextQuestion(question) {
  updateQuestionCounter();
  if (count >= 6) {
    document.getElementById("questionCounter").innerHTML =
      ", question number 6/6";
    nextBtn = document.getElementById("nextBtn");
    nextBtn.innerHTML = "Finish quiz!";
    nextBtn.setAttribute("onclick", "endGame()");
    endGame();
    uploadScore();
  } else {
    resetButtons();
    renderQuestion(question);
  }
}
function endGame() {
  timerAnimation = document.getElementById("timerHeader");
  timerAnimation.setAttribute("class", "timerNone");
  timerAnimation.innerHTML = "";

  disableButtons();
  clearInterval(timerInterval); //stop the timer
  score += bonusPoints;
  bonusPoints = 0;
  updatePlayerScore();
  Swal.fire({
    title: "Your score: " + score,
    html: '<button class="quizBtn" id="play-now-btn" onclick="startQuiz()">Play again?</button><br><button class="quizBtn" id="leaderboard-btn" onclick="getLeaderBoardData()">Show leaderboard!</button>',
    showConfirmButton: false,
    allowOutsideClick: true,
    showCloseButton: true,
    scrollbarPadding: false,
  }).then((result) => {
    if (result.isConfirmed) {
      Swal.close();
    } else if (result.isDismissed) {
      // Handle the modal being closed without clicking the buttons (e.g., clicking outside the modal)
      console.log("Modal closed without clicking buttons.");
    }
  });
}
function uploadScore() {
  user = JSON.parse(localStorage.getItem("user"));
  ajaxCall(
    "POST",
    insertScoreURL,
    JSON.stringify(
      (scoreToInsert = {
        id: 0,
        userScore: score,
        userId: user.id,
        userName: user.userName,
      })
    ),
    uploadSuccess,
    uploadError
  );
}

function uploadSuccess(data) {
  console.log("uploaded score: ", data);
}
function uploadError(error) {
  console.log(error);
}

function get4Songs() {
  fourSongs = [];
  while (fourSongs.length < 4) {
    randomSongNumber = Math.floor(Math.random() * allSongs.length);
    potentialSong = allSongs[randomSongNumber];
    if (!fourSongs.includes(potentialSong)) {
      fourSongs.push(potentialSong);
    }
  }
  return fourSongs;
}

function getSongLenth() {}
function disableButtons() {
  for (var i = 1; i < 5; i++) {
    answer = document.getElementById("answer" + i);
    answer.disabled = true;
  }
}
function whoHasTheMostListeners() {}

function whichArtistIsTheMostPlayer() {}

function correctAnswer(buttonId) {
  score += 10;
  updatePlayerScore();
  disableButtons();
  document.getElementById(buttonId).style.backgroundColor = "green";
  document.getElementById("nextBtn").style.visibility = "visible";
  count++;
}

function wrongAnswer(buttonId) {
  document.getElementById(saveCorrectAnswerId).style.backgroundColor = "green";
  for (var i = 1; i < 5; i++) {
    answer = document.getElementById("answer" + i);
    answer.disabled = true;
  }
  document.getElementById(buttonId).style.backgroundColor = "red";
  document.getElementById("nextBtn").style.visibility = "visible";
  count++;
}

function timerOut() {
  Swal.fire({
    title: "Time ended!",
    text: "Get Ready For The Next Question",
    icon: "error",
    timer: 2500,
    scrollbarPadding: false,
  }).then(() => {
    count++;
  });
}

function getAllSongsFromDB() {
  ajaxCall(
    "GET",
    getAllTheSongs,
    "",
    gotAllSongsFromDB,
    ErrorGetAllSongsFromDB
  );
}

function gotAllSongsFromDB(data) {
  allSongs = data;
}

function ErrorGetAllSongsFromDB(error) {
  console.log(error);
}

function getAllArtistsFromDB() {
  ajaxCall(
    "GET",
    getAllArtistsAPI,
    "",
    gotAllArtistsFromDB,
    errorAllArtistsFromDB
  );
}

function gotAllArtistsFromDB(data) {
  allArtists = data;
}
function errorAllArtistsFromDB(error) {
  console.log(error);
}

function gotSong(song) {}
function errorSong(error) {
  console.log(error);
}

function get3Artists(artist) {
  threeArtists = [];
  while (threeArtists.length < 3) {
    randomArtistNumber = Math.floor(Math.random() * allArtists.length);
    potentialArtist = allArtists[randomArtistNumber].name;
    if (potentialArtist != artist && !threeArtists.includes(potentialArtist)) {
      threeArtists.push(potentialArtist);
    }
  }
  return threeArtists;
}

function updatePlayerScore() {
  document.getElementById("playerScore").innerHTML = "Score: " + score;
}

function updateQuestionCounter() {
  document.getElementById("questionCounter").innerHTML =
    ", question number " + (count + 1) + "/6";
}

function whichSongHasTheMostLyrics() {
  fourSongs = get4Songs();
  longestSong = getTheSongWithMostLyrics(fourSongs).songName;

  wrongAnswersNames = [];
  fourSongs.forEach((song) => {
    wrongAnswersNames.push(song.songName);
  });
  question = {
    questionTxt: "Which song has the most lyrics?",
    correctAnswer: wrongAnswersNames.splice(longestSong, 1),
    wrongAnswers: wrongAnswersNames,
  };
  questions.push(question);
}

//return which Song Has The Most Lyrics
function getTheSongWithMostLyrics(songsArray) {
  maxLyrics = 0;
  songToRet = songsArray[0];
  songsArray.forEach((song) => {
    if (song.lyrics.split(" ").length > songToRet.lyrics.split(" ").length) {
      songToRet = song;
    }
  });
  return songToRet;
}

function get3SongsFromDifferentArtists(artName) {
  threeSongs = [];
  while (threeSongs.length < 3) {
    var randomSongNumber = Math.floor(Math.random() * allSongs.length);
    potentialSong = allSongs[randomSongNumber];
    if (
      potentialSong.artistName != artName &&
      !threeSongs.includes(potentialSong)
    ) {
      threeSongs.push(potentialSong);
    }
  }
  retSongsNames = [];
  threeSongs.forEach((song) => {
    retSongsNames.push(song.songName);
  });
  return retSongsNames;
}

function whichSongWrittenBy() {
  var randomSongNumber = Math.floor(Math.random() * allSongs.length);
  randomArtist = allSongs[randomSongNumber].artistName;
  randomSong = allSongs[randomSongNumber];
  wrongAnswersNames = get3SongsFromDifferentArtists(randomSong.artistName);
  question = {
    questionTxt: "Which song was written by " + randomSong.artistName + "?",
    correctAnswer: randomSong.songName,
    wrongAnswers: wrongAnswersNames,
  };
  questions.push(question);
}

async function whichArtistIsTheMostPopular() {
  fourRandomArtists = get4Artists();
  var fourArtists = [];
  // Fetch listener data for each artist concurrently
  const fetchPromises = fourRandomArtists.map((artist) =>
    getArtistLastFMArtistListeners(artist)
  );
  const listenersData = await Promise.all(fetchPromises);

  // Combine artist and listener data
  for (let i = 0; i < fourRandomArtists.length; i++) {
    const artist = fourRandomArtists[i];
    const listeners = listenersData[i];
    art = {
      artistName: artist,
      artistPop: listeners,
    };
    fourArtists.push(art);
  }
  fourArtists.sort((a, b) => b.artistPop - a.artistPop);
  var threeArtists = fourArtists.slice(1, 4).map((artist) => artist.artistName);

  question = {
    questionTxt: "Which artist is the most popular?",
    correctAnswer: fourArtists[0].artistName,
    wrongAnswers: threeArtists,
  };

  questions.push(question);
}

async function getArtistLastFMArtistListeners(artistName) {
  const apiKey = "645890a09eebe9cd0d7bce90c41ff1f1";
  const response = await fetch(
    `https://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=${artistName}&api_key=${apiKey}&format=json`
  );
  const data = await response.json();
  return data.artist.stats.listeners;
}

function get4Artists() {
  fourArtists = [];
  while (fourArtists.length < 4) {
    randomArtistNumber = Math.floor(Math.random() * allArtists.length);
    potentialArtist = allArtists[randomArtistNumber].name;
    if (!fourArtists.includes(potentialArtist)) {
      fourArtists.push(potentialArtist);
    }
  }
  return fourArtists;
}
