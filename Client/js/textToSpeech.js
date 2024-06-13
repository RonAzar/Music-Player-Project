var voice = {
  ShalomBijoi: 0,
  Bouton: 1,
  Saadon: 2,
  Wini: 3,
  Yoav: 4,
};

// Function to initiate text-to-speech
function speakText() {
  var speakBtn = document.getElementById("speak");
  speakBtn.innerHTML = "Stop Reading";
  speakBtn.setAttribute("onclick", "stopSpeak()");

  var textToSpeech = document.getElementById("artistSummary").innerText; // Use innerText instead of innerHTML to get only the text content
  textToSpeech = textToSpeech.split("<")[0];
  const msg = new SpeechSynthesisUtterance(textToSpeech);

  msg.voice =
    speechSynthesis.getVoices()[
      voice[document.getElementById("voiceList").value]
    ];

  // Speak the text
  console.log(msg);
  speechSynthesis.speak(msg);
}

function stopSpeak() {
  var speakBtn = document.getElementById("speak");
  speakBtn.innerHTML = "Click To Listen!";
  speakBtn.setAttribute("onclick", "speakText()");
  speechSynthesis.cancel();
}
