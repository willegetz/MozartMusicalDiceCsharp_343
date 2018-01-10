'use strict';

const exportedFunctions = {
    buildNoteIndex: function() {
        let notenames = {};
        "C C# D D# E F F# G G# A A# B".split(" ").forEach((n, j) => {
            for (let octave = -1; octave < 8; ++octave) {
                notenames[n + octave] = 12 + 12 * octave + j
            }
        });

        return notenames;
    },
}

this.exportedFunctions = exportedFunctions;