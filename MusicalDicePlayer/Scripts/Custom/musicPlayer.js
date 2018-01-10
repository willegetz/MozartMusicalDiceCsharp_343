'use strict';

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
        const frequencyOfAnchorNoteA4 = 440;
        const octaveOfAnchorNoteA4 = 69;

        const semitoneFrequencyRatio = Math.pow(2, (1 / 12));

        const stepsFromAnchorNote = octaveOfNote - octaveOfAnchorNoteA4;

        const newNoteFrequency = frequencyOfAnchorNoteA4 * Math.pow(semitoneFrequencyRatio, stepsFromAnchorNote);
        const roundedFrequency = Math.round(newNoteFrequency * 100) / 100;

        return roundedFrequency;
    },
}

this.exportedFunctions = exportedFunctions;