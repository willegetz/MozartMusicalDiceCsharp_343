using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MozartMusicalDice
{
    public class MusicalDice
    {
        private string sampleComposition;
        public MusicalDice()
        {
            var sampleCompositionFile = @".\..\..\..\MozartMusicalDice\Resources\mozart-dice-starting.txt";
            sampleComposition = File.ReadAllText(sampleCompositionFile);
        }

        public string RetrieveBeatsForMeasure(int measure)
        {
            var measureLength = 3;
            var startOfMeasure = (measure - 1) * measureLength;
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

        public int GetBeatOffsetForMeasures(int currentMeasure, int newMeasure)
        {
            var beatsInMeasure = 3;

            var measureOffset = newMeasure - currentMeasure;
            var beatOffset = measureOffset * beatsInMeasure;

            return beatOffset;
        }

        public string GetAdjustedBeatsForNewMeasure(string currentMeasureBeats, int beatOffset)
        {
            var measureArray = currentMeasureBeats.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            for (int i = 0; i < measureArray.Length; i++)
            {
                var currentMeasure = measureArray[i];
                var currentMeasureParts = currentMeasure.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var beatNumber = double.Parse(currentMeasureParts[1]);

                double newBeatNumber = beatNumber + beatOffset;

                currentMeasureParts[1] = newBeatNumber.ToString();

                var blah = string.Join(" ", currentMeasureParts);
                sb.Append(blah).Append('\n');
            }

            return sb.ToString().Trim();
        }

        public string CreateNewComposition(int[] measuresArray)
        {
            var sb = new StringBuilder();
            var newMeasureIndex = 0;

            for (int i = 0; i < measuresArray.Length; i++)
            {
                newMeasureIndex++;
                var currentMeasure = measuresArray[i];
                var currentMeasureBeats = RetrieveBeatsForMeasure(currentMeasure);
                var beatOffset = GetBeatOffsetForMeasures(currentMeasure, newMeasureIndex);
                var adjustedBeats = GetAdjustedBeatsForNewMeasure(currentMeasureBeats, beatOffset);
                sb.Append(adjustedBeats).Append("\n");
            }

            return sb.ToString().Trim();
        }
    }
}
