'use strict';

function buildApi(window) {
    window.AudioContext = window.AudioContext || window.webkitAudioContext;

    const context = new window.AudioContext();
    const mastervolume = context.createGain();

	mastervolume.gain.value = 0.6;
	mastervolume.connect(context.destination);

    const frequencyOfAnchorNoteA4 = 440;
    const minuteAsSeconds = 60;
    const octaveOfAnchorNoteA4 = 69;
    const semitoneFrequencyRatio = Math.pow(2, (1 / 12));
    const tempo = 120;

    const musicPlayer = {
        buildNoteIndex: function () {
            let notenames = {};
            "C C# D D# E F F# G G# A A# B".split(" ").forEach((n, j) => {
                for (let octave = -1; octave < 8; ++octave) {
                    notenames[n + octave] = 12 + 12 * octave + j
                }
            });

            return notenames;
        },
        calculateFrequencyForNote: function (octaveOfNote) {
            const stepsFromAnchorNote = octaveOfNote - octaveOfAnchorNoteA4;

            const newNoteFrequency = frequencyOfAnchorNoteA4 * Math.pow(semitoneFrequencyRatio, stepsFromAnchorNote);
            const roundedFrequency = Math.round(newNoteFrequency * 100) / 100;

            return roundedFrequency;
        },
        getNoteStartAndStopTimes: function (startBeat, durationBeats) {
            let start = startBeat * minuteAsSeconds / tempo;
            let duration = durationBeats * minuteAsSeconds / tempo;
            let stop = start + duration;

            return { start: start, stop: stop };
        },
        createNewGainNode: function(context, startTime, stopTime){
            let newGainNode = context.createGain();
            newGainNode.gain.setValueAtTime(1, startTime);
            newGainNode.gain.linearRampToValueAtTime(0.5, stopTime);
            newGainNode.gain.setTargetAtTime(0, stopTime, 0);

            return newGainNode;
        },
        createNewOscillatorNode: function(context, noteFrequencyHz, startTime){
            let newOscillator = context.createOscillator();
            newOscillator.type = 'square';
            newOscillator.frequency.setTargetAtTime(noteFrequencyHz, startTime, 0);

            return newOscillator;
        },
        playNote: function(note, startBeat, durationBeat){
            const octaveOfNote = this.buildNoteIndex()[note];
            const frequencyOfNote = this.calculateFrequencyForNote(octaveOfNote);
            const noteTiming = this.getNoteStartAndStopTimes(startBeat, durationBeat);

            const newGainNode = this.createNewGainNode(context, noteTiming.start, noteTiming.stop);
            const newOscillator = this.createNewOscillatorNode(context, frequencyOfNote, noteTiming.start);

            newGainNode.connect(mastervolume);

            newOscillator.connect(newGainNode);
            newOscillator.start(noteTiming.start);
            newOscillator.stop(noteTiming.stop);
        }
    }

    window.musicPlayer = musicPlayer;
}


if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
    module.exports = buildApi;
}