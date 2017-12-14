using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace MusicalDiceTests
{
    [TestClass]
    public class MusicalDiceTest
    {
        // A measure is 3 beats and is 0 based. Measure 1 would be beat 0, 1, and 2 but not 3. Measure 144 would be beat 432, 422, and 434 but not 435.
        // Given a set of measures, extract the appropriate beats for a given measure.

        const string sampleMeasures = @"F3 0 1
F5 0 1
D3 1 1
D5 1 1
G3 2 1
G5 2 1
A4 3 1
B2 3 2
G3 3 2";

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

        private string RetrieveBeatsForMeasure(int measure)
        {
            var measureLength = 2;
            var beatsForMeasure = measure + measureLength;

            string separator = Environment.NewLine;
            var measureArray = sampleMeasures.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();

            for (int i = 0; i < measureArray.Length; i++)
            {
                var currentMeasure = measureArray[i];
                var currentMeasureParts = currentMeasure.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var measureNumber = double.Parse(currentMeasureParts[1]);

                if (measureNumber < beatsForMeasure)
                {
                    sb.Append(currentMeasure).Append("\n");
                }
            }

            return sb.ToString().Trim();
        }
    }
}
