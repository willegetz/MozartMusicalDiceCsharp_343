using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.IO;
using MozartMusicalDice;
using ApprovalTests.Reporters;
using ApprovalTests;

namespace MusicalDiceTests
{
    [TestClass]
    [UseReporter(typeof(BeyondCompareReporter))]
    public class MusicalDiceTest
    {
        // A measure is 3 beats and is 0 based. Measure 1 would be beat 0, 1, and 2 but not 3. Measure 144 would be beat 432, 422, and 434 but not 435.
        // Given a set of measures, extract the appropriate beats for a given measure.

        private static MusicalDice musicDice;

        [ClassInitialize]
        public static void LoadSampleComposition(TestContext testContext)
        {
            musicDice = new MusicalDice();
        }

        [TestMethod]
        public void Measure1HasBeats0UpTo3()
        {
            var beatsForMeasure1 = musicDice.RetrieveBeatsForMeasure(1);

            Approvals.Verify(beatsForMeasure1);
        }

        [TestMethod]
        public void Measure22HasBeats63UpTo66()
        {
            var beatsForMeasure22 = musicDice.RetrieveBeatsForMeasure(22);

            Approvals.Verify(beatsForMeasure22);
        }

        [TestMethod]
        public void Measure176HasBeat525UpTo528(){
            var beatsForMeasure176 = musicDice.RetrieveBeatsForMeasure(176);

            Approvals.Verify(beatsForMeasure176);
        }

        [TestMethod]
        public void GenerateRawCompositionFrom22nd176thAnd1stMeasures()
        {
            var composition = new StringBuilder();

            var firstMeasure = musicDice.RetrieveBeatsForMeasure(22);
            composition.Append(firstMeasure).Append("\n");

            var secondMeasure = musicDice.RetrieveBeatsForMeasure(176);
            composition.Append(secondMeasure).Append("\n");

            var thirdMeasure = musicDice.RetrieveBeatsForMeasure(1);
            composition.Append(thirdMeasure);

            var rawComposition = composition.ToString().Trim();
            
            Approvals.Verify(rawComposition);
        }

        // Measure 22 (beats 63 to 66) to become Measure 1 (beats 0 to 3)
        [TestMethod]
        public void Measure22ToBecomeMeasure1()
        {
            var currentMeasure = 22;
            var newMeasure = 1;

            var beatOffset = musicDice.GetBeatOffsetForMeasures(currentMeasure, newMeasure);

            var expected = -63;

            Assert.AreEqual(expected, beatOffset);
        }

        // Measure 176 (beats 525 to 528) to become Measure 6 (beats 15 to 18)
        [TestMethod]
        public void Measure176ToBecomeMeasure6()
        {
            var currentMeasure = 176;
            var newMeasure = 6;

            var beatOffset = musicDice.GetBeatOffsetForMeasures(currentMeasure, newMeasure);

            var expected = -510;

            Assert.AreEqual(beatOffset, expected);
        }

        // Measure 8 (21 to 24) to become Measure 13 (36 to 39)
        [TestMethod]
        public void Measure8ToBecomeMeasure13()
        {
            var currentMeasure = 8;
            var newMeasure = 13;

            var beatOffset = musicDice.GetBeatOffsetForMeasures(currentMeasure, newMeasure);

            var expected = 15;

            Assert.AreEqual(beatOffset, expected);
        }

        [TestMethod]
        public void Measure22BeatsCalculatedToBeMeasure1Beats()
        {
            var currentMeasure = 22;
            var newMeasure = 1;

            var currentMeasureBeats = musicDice.RetrieveBeatsForMeasure(currentMeasure);
            var beatOffset = musicDice.GetBeatOffsetForMeasures(currentMeasure, newMeasure);
            var newMeasureBeats = musicDice.GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);

            Approvals.Verify(newMeasureBeats);
        }

        [TestMethod]
        public void Measure176BeatsCalculatedToBeMeasure6Beats()
        {
            var currentMeasure = 176;
            var newMeasure = 6;

            var currentMeasureBeats = musicDice.RetrieveBeatsForMeasure(currentMeasure);
            var beatOffset = musicDice.GetBeatOffsetForMeasures(currentMeasure, newMeasure);
            var newMeasureBeats = musicDice.GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);

            Approvals.Verify(newMeasureBeats);
        }

        [TestMethod]
        public void Measure8BeatsCalculatedToBeMeasure13Beats()
        {
            var currentMeasure = 8;
            var newMeasure = 13;

            var currentMeasureBeats = musicDice.RetrieveBeatsForMeasure(currentMeasure);
            var beatOffset = musicDice.GetBeatOffsetForMeasures(currentMeasure, newMeasure);
            var newMeasureBeats = musicDice.GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);

            Approvals.Verify(newMeasureBeats);
        }

        [TestMethod]
        public void GenerateNewCompositionFrom22nd176thAnd1stMeasures()
        {
            var composition = new StringBuilder();

            var measure22 = 22;
            var measure176 = 176;
            var measure1 = 1;

            var firstMeasure = 1;
            var secondMeasure = 2;
            var thirdMeasure = 3;

            var beatsForMeasure22 = musicDice.RetrieveBeatsForMeasure(measure22);
            var firstMeasureBeatOffset = musicDice.GetBeatOffsetForMeasures(measure22, firstMeasure);
            var newFirstMeasure = musicDice.GetAdjustedBeatsForNewMeasure(beatsForMeasure22, firstMeasureBeatOffset);
            composition.Append(newFirstMeasure).Append("\n");

            var beatsForMeasure176 = musicDice.RetrieveBeatsForMeasure(measure176);
            var secondMeasureBeatOffset = musicDice.GetBeatOffsetForMeasures(measure176, secondMeasure);
            var newSecondMeasure = musicDice.GetAdjustedBeatsForNewMeasure(beatsForMeasure176, secondMeasureBeatOffset);
            composition.Append(newSecondMeasure).Append("\n");

            var beatsForMeasure1 = musicDice.RetrieveBeatsForMeasure(measure1);
            var thirdMeasureBeatOffset = musicDice.GetBeatOffsetForMeasures(measure1, thirdMeasure);
            var newThirdMeasure = musicDice.GetAdjustedBeatsForNewMeasure(beatsForMeasure1, thirdMeasureBeatOffset);
            composition.Append(newThirdMeasure).Append("\n");

            var newComposition = composition.ToString().Trim();

            Approvals.Verify(newComposition);
        }

        [TestMethod]
        public void GenerateNewCompositionFromArrayOfMeasures()
        {
            // create array of measures from sample matrix in challengeinfo.txt
            var measuresArray = new[] { 96, 22, 141, 41, 105, 122, 11, 30, 70, 121, 26, 9, 112, 49, 109, 14 };

            // pass array of measures into the generate new composition function on the musical dice object
            var newComposition = musicDice.CreateNewComposition(measuresArray);

            // verify the new composition is correct.
            Approvals.Verify(newComposition);
        }
    }
}
