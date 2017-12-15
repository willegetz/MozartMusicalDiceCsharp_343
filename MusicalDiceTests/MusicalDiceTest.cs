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

        private string RetrieveBeatsForMeasure(int measure)
        {
            var measureLength = 3;

            var startOfMeasure = (measure -1) * measureLength;

            var endOfMeasure = startOfMeasure + measureLength;

            string separator = Environment.NewLine;
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
    }
}
