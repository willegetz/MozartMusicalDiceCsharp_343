'Use Strict'

const assert = require('assert');
const musicPlayer = require('../../MusicalDicePlayer/Scripts/Custom/musicPlayer');

describe('Musica Player', function () {
  console.log(JSON.stringify(musicPlayer, null, 4));

  describe('cosmologicon\'s note / octave dictionary', function () {
    it('should provide a note name to index map', function () {
      let notenames = musicPlayer.exportedFunctions.buildNoteIndex();
      let expectedNotenames = require('./approvedFiles/cosmologiconNoteNames.json');

      assert.deepEqual(notenames, expectedNotenames);
    })
  })
});