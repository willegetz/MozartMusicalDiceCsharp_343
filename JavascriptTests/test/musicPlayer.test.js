'Use Strict'

const assert = require('assert');
const musicPlayer = require('../../MusicalDicePlayer/Scripts/Custom/musicPlayer').exportedFunctions;

describe('Musical Player', function () {
  let noteNames = {};

  beforeEach(function () {
    noteNames = musicPlayer.buildNoteIndex();
  });

  describe('cosmologicon\'s note / octave dictionary', function () {
    it('should provide a note name to index map', function () {
      let expectedNoteNames = require('./approvedFiles/cosmologiconNoteNames.json');

      assert.deepEqual(noteNames, expectedNoteNames);
    })
  });

  it('should calculate the octave frequency in hz for note C4 to be 261.63hz', function () {
    const middleC = 'C4';
    const octaveOfMiddleC = noteNames[middleC];

    let middleCFrequency = musicPlayer.calculateFrequencyForNote(octaveOfMiddleC);

    const expectedFrequency = 261.63;
    assert.equal(middleCFrequency, expectedFrequency);
  });

  it('should calculate the start and stop time of a note given a tempo of 120, a starting beat of 3, and a duration of 2 beats', function () {
    const startBeat = 3;
    const duration = 2;

    let noteTiming = musicPlayer.getNoteStartAndStopTimes(startBeat, duration);

    const expectedStartTime = 1.5;
    const expectedStopTime = 2.5;

    assert.equal(noteTiming.start, expectedStartTime);
    assert.equal(noteTiming.stop, expectedStopTime);
  });
});