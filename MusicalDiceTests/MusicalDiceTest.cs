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
        public void Measure22HasBeats63To65()
        {
            var actual = RetrieveBeatsForMeasure(22);

            var expected = @"C3 63 2
E5 63 1
C5 64 1
G4 65 1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Measure176HasBeat525To528(){
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

        [TestMethod]
        public void FirstMeasureBeatsFromMeasure22Ordered0To3()
        {
            var firstMeasureRaw = RetrieveBeatsForMeasure(22);

            var actual = OrderBeatsForNewMeasurePlacement(1, firstMeasureRaw);

            var expected = @"C3 0 2
E5 0 1
C5 1 1
G4 2 1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstMeasureBeatsFromMeasure176Ordered0To3()
        {
            var firstMeasureRaw = RetrieveBeatsForMeasure(176);

            var actual = OrderBeatsForNewMeasurePlacement(1, firstMeasureRaw);

            var expected = @"A5 0 0.5
B2 0 2
D3 0 2
G5 0.5 0.5
B5 1 0.5
G5 1.5 0.5
B2 2 1
D3 2 1
D5 2 0.5
G5 2.5 0.5";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SecondMeasureBeatsFromMeasure1Ordered3To6()
        {
            var secondMeasureRaw = RetrieveBeatsForMeasure(1);

            var actual = OrderBeatsForNewMeasurePlacement(2, secondMeasureRaw);

            var expected = @"F3 3 1
F5 3 1
D3 4 1
D5 4 1
G3 5 1
G5 5 1";

            Assert.AreEqual(expected, actual);
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

        private string OrderBeatsForNewMeasurePlacement(int desiredMeasureNumber, string firstMeasureRaw)
        {
            var sb = new StringBuilder();

            var measureLength = 3;
            var firstBeat = (desiredMeasureNumber - 1) * measureLength;
            var lastBeat = firstBeat + measureLength;

            var newBeatOrder = new[] { firstBeat, firstBeat++, lastBeat };

            var measureArray = firstMeasureRaw.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var firstOfMeasuresParts = measureArray[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var measureOffset = double.Parse(firstOfMeasuresParts[1]);

            foreach (var measure in measureArray)
            {
                var currentMeasureParts = measure.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Get a the beat number, like 2 or 3.5
                var currentMeasureNumber = double.Parse(currentMeasureParts[1]);

                // Assign the new beat number, like 6 or 9.5



                var beatIndex = currentMeasureNumber - measureOffset;

                
                currentMeasureParts[1] = newBeatOrder[0].ToString();

                var newMeasure = string.Join(" ", currentMeasureParts);
                sb.Append(newMeasure).Append("\n");
            }

            return sb.ToString().Trim();
        }
    }
}
