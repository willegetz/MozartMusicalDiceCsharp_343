'Use Strict'

const assert = require('assert');
const musicPlayer = require('../../MusicalDicePlayer/Scripts/Custom/musicPlayer').exportedFunctions;

describe('Musica Player', function () {
  console.log(JSON.stringify(musicPlayer, null, 4));

  describe('cosmologicon\'s note / octave dictionary', function () {
    it('should provide a note name to index map', function () {
      let notenames = musicPlayer.buildNoteIndex();
      let expectedNotenames = require('./approvedFiles/cosmologiconNoteNames.json');

      assert.deepEqual(notenames, expectedNotenames);
    })
  });

  it('should calculate the octave frequency in hz for note C3 to be 261.63hz', function () {
    let notenames = musicPlayer.buildNoteIndex();

    const middleC = 'C4';
    const octaveOfMiddleC = notenames[middleC];

    let middleCFrequency = musicPlayer.calculateFrequencyForNote(octaveOfMiddleC);

    const expectedFrequency = 261.63;
    assert.equal(middleCFrequency, expectedFrequency);
  });
});