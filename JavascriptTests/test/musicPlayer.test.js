'Use Strict'

const assert = require('assert');
const musicPlayer = require('../../MusicalDicePlayer/Scripts/Custom/musicPlayer').exportedFunctions;

describe('Musical Player', function () {
  console.log(JSON.stringify(musicPlayer, null, 4));

  describe('cosmologicon\'s note / octave dictionary', function () {
    it('should provide a note name to index map', function () {
      let notenames = musicPlayer.buildNoteIndex();
      let expectedNotenames = require('./approvedFiles/cosmologiconNoteNames.json');

      assert.deepEqual(notenames, expectedNotenames);
    })
  });

  it('should calculate the octave frequency in hz for note C4 to be 261.63hz', function () {
    let notenames = musicPlayer.buildNoteIndex();

    const middleC = 'C4';
    const octaveOfMiddleC = notenames[middleC];

    let middleCFrequency = musicPlayer.calculateFrequencyForNote(octaveOfMiddleC);

    const expectedFrequency = 261.63;
    assert.equal(middleCFrequency, expectedFrequency);
  });

  it('should calculate the start and stop time of a note given a tempo of 120, a starting beat of 3, and a duration of 2 beats', function () {
    const tempo = 120;
    const minuteAsSeconds = 60;

    const startBeat = 3;
    const duration = 2;

    let beatToTime = minuteAsSeconds / tempo;

    let startingTime = startBeat * beatToTime;
    let durationTime = duration * beatToTime;
    let stoppingTime = startingTime + durationTime;

    const expectedStartTime = 1.5;
    const expectedStopTime = 2.5;

    assert.equal(startingTime, expectedStartTime);
    assert.equal(stoppingTime, expectedStopTime);
  });
});