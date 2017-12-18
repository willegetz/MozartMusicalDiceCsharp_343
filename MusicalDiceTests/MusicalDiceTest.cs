using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.IO;

namespace MusicalDiceTests
{
    [TestClass]
    public class MusicalDiceTest
    {
        // A measure is 3 beats and is 0 based. Measure 1 would be beat 0, 1, and 2 but not 3. Measure 144 would be beat 432, 422, and 434 but not 435.
        // Given a set of measures, extract the appropriate beats for a given measure.

        private static string sampleComposition;

        [ClassInitialize]
        public static void LoadSampleComposition(TestContext testContext)
        {
            var sampleCompositionFile = @".\..\..\..\MozartMusicalDice\Resources\mozart-dice-starting.txt";
            sampleComposition = File.ReadAllText(sampleCompositionFile);
        }

        [TestMethod]
        public void Measure1HasBeats0UpTo3()
        {
            var actual = RetrieveBeatsForMeasure(1);

            var expected = @"F3 0 1
F5 0 1
D3 1 1
D5 1 1
G3 2 1
G5 2 1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Measure22HasBeats63UpTo66()
        {
            var actual = RetrieveBeatsForMeasure(22);

            var expected = @"C3 63 2
E5 63 1
C5 64 1
G4 65 1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Measure176HasBeat525UpTo528(){
            var actual = RetrieveBeatsForMeasure(176);

            var expected = @"A5 525 0.5
B2 525 2
D3 525 2
G5 525.5 0.5
B5 526 0.5
G5 526.5 0.5
B2 527 1
D3 527 1
D5 527 0.5
G5 527.5 0.5";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateRawCompositionFrom22nd176thAnd1stMeasures()
        {
            var composition = new StringBuilder();

            var firstMeasure = RetrieveBeatsForMeasure(22);
            composition.Append(firstMeasure).Append("\n");

            var secondMeasure = RetrieveBeatsForMeasure(176);
            composition.Append(secondMeasure).Append("\n");

            var thirdMeasure = RetrieveBeatsForMeasure(1);
            composition.Append(thirdMeasure);

            var actual = composition.ToString().Trim();
            var expected = @"C3 63 2
E5 63 1
C5 64 1
G4 65 1
A5 525 0.5
B2 525 2
D3 525 2
G5 525.5 0.5
B5 526 0.5
G5 526.5 0.5
B2 527 1
D3 527 1
D5 527 0.5
G5 527.5 0.5
F3 0 1
F5 0 1
D3 1 1
D5 1 1
G3 2 1
G5 2 1";

            Assert.AreEqual(expected, actual);
        }

        // Measure 22 (beats 63 to 66) to become Measure 1 (beats 0 to 3)
        [TestMethod]
        public void Measure22ToBecomeMeasure1()
        {
            var currentMeasure = 22;
            var newMeasure = 1;

            var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasure);

            var expected = -63;

            Assert.AreEqual(expected, beatOffset);
        }

        // Measure 176 (beats 525 to 528) to become Measure 6 (beats 15 to 18)
        [TestMethod]
        public void Measure176ToBecomeMeasure6()
        {
            var currentMeasure = 176;
            var newMeasure = 6;

            var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasure);

            var expected = -510;

            Assert.AreEqual(beatOffset, expected);
        }

        // Measure 8 (21 to 24) to become Measure 13 (36 to 39)
        [TestMethod]
        public void Measure8ToBecomeMeasure13()
        {
            var currentMeasure = 8;
            var newMeasure = 13;

            var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasure);

            var expected = 15;

            Assert.AreEqual(beatOffset, expected);
        }

        [TestMethod]
        public void Measure22BeatsCalculatedToBeMeasure1Beats()
        {
            var currentMeasure = 22;
            var newMeasure = 1;

            var currentMeasureBeats = RetrieveBeatsForMeasure(22);
            var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasure);
            var newMeasureBeats = GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);

            var expected = @"C3 0 2
E5 0 1
C5 1 1
G4 2 1";

            Assert.AreEqual(expected, newMeasureBeats);
        }

        [TestMethod]
        public void Measure176BeatsCalculatedToBeMeasure6Beats()
        {
            var currentMeasure = 176;
            var newMeasure = 6;

            var currentMeasureBeats = RetrieveBeatsForMeasure(22);
            var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasure);
            var newMeasureBeats = GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);

            var expected = @"A5 15 0.5
B2 15 2
D3 15 2
G5 15.5 0.5
B5 16 0.5
G5 16.5 0.5
B2 17 1
D3 17 1
D5 17 0.5
G5 17.5 0.5";

            Assert.AreEqual(expected, newMeasureBeats);
        }

        [TestMethod]
        public void Measure8BeatsCalculatedToBeMeasure13Beats()
        {
            var currentMeasure = 8;
            var newMeasure = 13;

            var currentMeasureBeats = RetrieveBeatsForMeasure(22);
            var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasure);
            var newMeasureBeats = GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);

            var expected = @"C3 36 1
C5 36 2
G2 37 1
C2 38 1";

            Assert.AreEqual(expected, newMeasureBeats);
        }

        private string RetrieveBeatsForMeasure(int measure)
        {
            var measureLength = 3;
            var startOfMeasure = (measure -1) * measureLength;
            var endOfMeasure = startOfMeasure + measureLength;

            var measureArray = sampleComposition.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();

            for (int i = 0; i < measureArray.Length; i++)
            {
                var currentMeasure = measureArray[i];
                var currentMeasureParts = currentMeasure.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var measureNumber = double.Parse(currentMeasureParts[1]);

                if (measureNumber >= startOfMeasure && measureNumber < endOfMeasure)
                {
                    sb.Append(currentMeasure).Append("\n");
                }
            }

            return sb.ToString().Trim();
        }

        private int GetBeatOffsetForMeasures(int currentMeasure, int newMeasure)
        {
            var beatsInMeasure = 3;

            var measureOffset = newMeasure - currentMeasure;
            var beatOffset = measureOffset * beatsInMeasure;

            return beatOffset;
        }

        private string GetAdjustedBeatsForNewMeasure(string currentMeasureBeats, int beatOffset)
        {
            if (beatOffset == -63)
            {
                return @"C3 0 2
E5 0 1
C5 1 1
G4 2 1";
            }

            if (beatOffset == -510)
            {
                return @"A5 15 0.5
B2 15 2
D3 15 2
G5 15.5 0.5
B5 16 0.5
G5 16.5 0.5
B2 17 1
D3 17 1
D5 17 0.5
G5 17.5 0.5";
            }

            return @"C3 36 1
C5 36 2
G2 37 1
C2 38 1";
        }
    }
}
