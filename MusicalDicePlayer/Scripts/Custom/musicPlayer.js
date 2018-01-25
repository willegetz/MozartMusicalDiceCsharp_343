'use strict';
const frequencyOfAnchorNoteA4 = 440;
const minuteAsSeconds = 60;
const octaveOfAnchorNoteA4 = 69;
const semitoneFrequencyRatio = Math.pow(2, (1 / 12));
const tempo = 120;

const exportedFunctions = {
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
}

this.exportedFunctions = exportedFunctions;