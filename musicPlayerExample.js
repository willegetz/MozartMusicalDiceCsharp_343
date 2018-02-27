"use strict"
window.onerror = function (error, url, line) {
	document.body.innerHTML = "<p>Error in: "+url+"<p>line "+line+"<pre>"+error+"</pre>"
}

let notenames = {};
"C C# D D# E F F# G G# A A# B".split(" ").forEach((n, j) => {
	for (let octave = -1 ; octave < 8 ; ++octave) {
		notenames[n + octave] = 12 + 12 * octave + j
	};
});

function totext(compo) {
	let values = compo.replace(/,/g, " ").match(/\S+/g) || []
	let ret = []
	for (let j = 0 ; j < values.length ; j += 3) {
		ret.push(values.slice(j, j + 3).join(" "))
	}
	return ret.join("\n")
}
function toquery(compo) {
	let values = compo.replace(/,/g, " ").match(/\S+/g) || []
	return values.join(",")
}
function updateurl() {
	let compo = document.getElementById("compo").value
//	history.replaceState(null, "", "?s=" + toquery(compo))
}

window.AudioContext = window.AudioContext || window.webkitAudioContext // assign audio context if it exists, or if webkit exists
let tempo = 120 // beats per minute, average for classical music (Moderato, Allegrato, Allegro Moderato, Allegro)
let wavetype = "square" // Sweet sweet chiptune
let tsnum = 3 // time signature number: 3 beats or quarter notes per measure.
let context = null;
let mastervolume = null;

function play() {
	stop()

	// Function called once. Audio context, master volume, and connection to the speakers happens 'once' here.
	// All gain nodes created in playnote are attached to the master volume
	
	// Create the audio context with a "master volume" and connect it to the computer speakers.
	context = new AudioContext()
	mastervolume = context.createGain()
	mastervolume.gain.value = 0.6 // starting volume
	mastervolume.connect(context.destination) // computer speakers
	
	
	let values = document.getElementById("compo").value.replace(/,/g, " ").match(/\S+/g) || []
	for (let j = 0 ; j < values.length - 2 ; j += 3) {
		playnote(values[j], +values[j+1], +values[j+2])
	}
}

function stop() {
	if (context) {
		context.close()
		context = null
	}
}
function loadexample() {
	stop()
	document.getElementById("compo").value = examplesolution
}

function playnote(notename, on, duration) {
	let tone = notenames[notename]

	let start = on * 60 / tempo // What time in the measure to start the note on // 60 seconds / 120 beats per minute
	let dt = duration * 60 / tempo // How many long the note plays for // 60 seconds / 120 beats per minute
	let stop = start + dt;

	let gain = context.createGain() // Is this gain a child of the 'mastervolume' gain?
	gain.gain.setValueAtTime(1, start) // Adjusts volume of app. param1: Value to which the AudioParam will change to. param2: time to start.
	gain.gain.linearRampToValueAtTime(0.5, stop) // Adjusts volume of note from 1 to .5 at/by the end of the note // Manipulates the 'sound'/'feel'? when transitioning from one tone to another? This is still a bit of mystery to me, but I've tried other values to replace 0.5, 0 makes the notes sound like a GameBoy while higher values make the note sound 'weird'.
	gain.gain.value = 0 // Why are we doing this?
	gain.connect(mastervolume) // Connect the playnote gain to the mastervolume.
	
	let node = context.createOscillator()
	node.type = wavetype
	node.frequency.value = 261.6 // Frequency of Middle C.
	node.detune.value = (tone - 60) * 100 // adjust to the desired frequency based on the initial frequency of Middel C. // tone is index of note // index of note - index of middle c
	
	node.connect(gain)
	node.start(start)
	node.stop(stop)
}
window.setInterval(function () { // From the start, this function is called every 10 milliseconds.
	let value = ""
	if (context) {
		let beat = context.currentTime * tempo / 60 // 120 beats per minute / 60 seconds
		let measure = Math.floor(beat / tsnum) + 1
		value = "measure: " + measure + ", beat: " + beat.toFixed(1)
	}
	document.getElementById("current").value = value
}, 10)

let qobj = {}
let query = window.location.search.replace(/^\?/, '')
query.split("&").forEach(param => {
	if (!param.length) return
	if (!param.includes("=")) throw "Unparseable param in query string: " + param
	let parts = param.split("=")
	qobj[parts[0]] = parts[1]
})

if (qobj.tempo) {
	tempo = +qobj.tempo
}
if (qobj.tsnum) {
	tempo = +qobj.tsnum
}
if (qobj.wavetype) {
	wavetype = qobj.wavetype
}
if (qobj.s) {
//	document.getElementById("compo").value = totext(qobj.s)
}