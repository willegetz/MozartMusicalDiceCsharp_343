'Use Strict'

const assert = require('assert');
const musicPlayerApi = require('../../MusicalDicePlayer/Scripts/Custom/musicPlayer');
const sinon = require('sinon');

describe('Musical Player', function () {
  let noteNames = {};
  let musicPlayer = {};

  let fakeWindow = {
    AudioContext: function () { },
  }

  fakeWindow.AudioContext.prototype.createGain = function () {
    return {
      gain: {
        value: 0,
        setValueAtTime: sinon.spy(),
        linearRampToValueAtTime: sinon.spy(),
        setTargetAtTime: sinon.spy()
      },
      connect: sinon.spy(),
    }
  };

  fakeWindow.AudioContext.prototype.createOscillator = function () {
    return {
      type: sinon.spy(),
      frequency: {
        setTargetAtTime: sinon.spy()
      },
      connect: sinon.spy(),
      start: sinon.spy()
    }
  };

  musicPlayerApi(fakeWindow);
  musicPlayer = fakeWindow.exportedFunctions;

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

  it('should create a gain node with a start time of 5 and a stop time of 7', function () {
    const createNewGainNodeSpy = sinon.spy(musicPlayer, 'createNewGainNode');
    const createGainSpy = sinon.spy(fakeWindow.AudioContext.prototype, 'createGain');

    const fakeContext = new fakeWindow.AudioContext();
    const startTime = 5;
    const stopTime = 7;

    const newGain = musicPlayer.createNewGainNode(fakeContext, startTime, stopTime);

    assert(createNewGainNodeSpy.calledOnce, `createNewGainNode was called ${createNewGainNodeSpy.callCount} time(s)`);
    assert(createGainSpy.calledOnce, `createGain was called ${createGainSpy.callCount} time(s)`);

    const setValueAtTimeArgs = createNewGainNodeSpy.returnValues[0].gain.setValueAtTime.args[0];
    const linearRampToValueAtTimeArgs = createNewGainNodeSpy.returnValues[0].gain.linearRampToValueAtTime.args[0];
    const setTargetAtTimeArgs = createNewGainNodeSpy.returnValues[0].gain.setTargetAtTime.args[0];

    assert.equal(setValueAtTimeArgs[1], startTime, `Paramter value was actually ${setValueAtTimeArgs[1]}`);
    assert.equal(linearRampToValueAtTimeArgs[1], stopTime, `Paramter value was actually ${linearRampToValueAtTimeArgs[1]}`);
    assert.equal(setTargetAtTimeArgs[1], stopTime, `Paramter value was actually ${setTargetAtTimeArgs[1]}`);

    fakeWindow.AudioContext.prototype.createGain.restore();
    musicPlayer.createNewGainNode.restore();
  });

  it('should create an oscilator node for note G4, 392hz, and a start time of 5', function () {
    const createNewOscillatorNodeSpy = sinon.spy(musicPlayer, 'createNewOscillatorNode');
    const createOscillatorSpy = sinon.spy(fakeWindow.AudioContext.prototype, 'createOscillator');

    const fakeContext = new fakeWindow.AudioContext();
    const g4Frequency = 392.00;
    const startTime = 5;


    const newOscillator = musicPlayer.createNewOscillatorNode(fakeContext, g4Frequency, startTime);

    assert(createNewOscillatorNodeSpy.calledOnce, `createNewOscillatorNode was called ${createNewOscillatorNodeSpy.callCount} time(s)`);
    assert(createOscillatorSpy.calledOnce, `createOscillator was called ${createOscillatorSpy.callCount} time(s)`);

    const setTargetAtTimeArgs = createOscillatorSpy.returnValues[0].frequency.setTargetAtTime.args[0];

    assert.equal(setTargetAtTimeArgs[0], g4Frequency);
    assert.equal(setTargetAtTimeArgs[1], startTime)


    fakeWindow.AudioContext.prototype.createOscillator.restore();
    musicPlayer.createNewOscillatorNode.restore();
  });

  it('should play a sound given the note D4, a start beat of 2, and a duration of 0.5', function () {
    const playNoteSpy = sinon.spy(musicPlayer, 'playNote');
    const calculateFrequencyForNoteSpy = sinon.spy(musicPlayer, 'calculateFrequencyForNote');
    const getNoteStartAndStopTimesSpy = sinon.spy(musicPlayer, 'getNoteStartAndStopTimes');
    const createNewGainNodeSpy = sinon.spy(musicPlayer, 'createNewGainNode');
    const createNewOscillatorNodeSpy = sinon.spy(musicPlayer, 'createNewOscillatorNode')



    const fakeContext = new fakeWindow.AudioContext();
    const note = 'D4';

    const startBeat = 2;
    const durationBeat = 0.5;

    musicPlayer.playNote(fakeContext, note, startBeat, durationBeat);
    
    assert(playNoteSpy.calledOnce, `playNote was called ${playNoteSpy.callCount} time(s)`);
    assert(calculateFrequencyForNoteSpy.calledOnce, `calculateFrequencyForNote was called ${calculateFrequencyForNoteSpy.callCount} time(s)`);
    assert(getNoteStartAndStopTimesSpy.calledOnce, `getNoteStartAndStopTimes was called ${getNoteStartAndStopTimesSpy.callCount} time(s)`);
    assert(createNewGainNodeSpy.calledOnce, `createNewGainNode was called ${createNewGainNodeSpy.callCount} time(s)`);
    assert(createNewOscillatorNodeSpy.calledOnce, `createNewOscillatorNode was called ${createNewOscillatorNodeSpy.callCount} time(s)`);

    assert(createNewGainNodeSpy.returnValues[0].connect.calledOnce, `newGainNode.connect was called ${createNewGainNodeSpy.returnValues[0].connect.callCount} time(s)`);
    assert(createNewOscillatorNodeSpy.returnValues[0].connect.calledOnce, `newOscillatorNode.connect was called ${createNewOscillatorNodeSpy.returnValues[0].connect.callCount} time(s)`);
    assert(createNewOscillatorNodeSpy.returnValues[0].start.calledOnce, `newOscillatorNode.start was called ${createNewOscillatorNodeSpy.returnValues[0].start.callCount} time(s)`);    
    assert(createNewOscillatorNodeSpy.returnValues[0].stop.calledOnce, `newOscillatorNode.stop was called ${createNewOscillatorNodeSpy.returnValues[0].stop.callCount} time(s)`);    

    musicPlayer.playNote.restore();
    musicPlayer.calculateFrequencyForNote.restore();
    musicPlayer.getNoteStartAndStopTimes.restore();
    musicPlayer.createNewGainNode.restore();
    musicPlayer.createNewOscillatorNode.restore();
  });
});