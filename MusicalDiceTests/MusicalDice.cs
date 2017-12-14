using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicalDiceTests
{
    [TestClass]
    public class MusicalDice
    {
        // A measure is 3 beats and is 0 based. Measure 1 would be beat 0, 1, and 2 but not 3. Measure 144 would be beat 432, 422, and 434 but not 435.
        // Given a set of measures, extract the appropriate beats for a given measure.

        public void Measure1HasBeats0UpTo3()
        {
            // arrange: create a sample measure set with 3 beats whose starting time are whole numbers
            // act: pass measure number 1 to the BeatExtractor and assign the result set to a beats variable
            // assert: extracted beats for a measure go from an index of 0 to 3, not inclusive
            Assert.Fail("No beats yet");
        }
    }
}
