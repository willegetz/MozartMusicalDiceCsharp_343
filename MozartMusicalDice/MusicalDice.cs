using System;
using System.IO;
using System.Text;

namespace MozartMusicalDice
{
    public class MusicalDice
    {
        private string originalComposition;
        private int randomSeed;

        public MusicalDice(string compositionFilePath, int seed = -1)
        {
            originalComposition = File.ReadAllText(compositionFilePath);
            randomSeed = seed;
        }

        public MusicalDice()
        {
            originalComposition = SourceComposition.GetSourceComposition;
            randomSeed = (int)DateTime.UtcNow.Ticks;
        }

        public string CreateRandomComposition()
        {
            var measureArray = GetMeasureArray();
            var newComposition = BuildNewComposition(measureArray);
            return newComposition;
        }

        public int[] GetMeasureArray()
        {
            var totalMeasures = 16;
            var measureArray = new int[totalMeasures];

            for (int i = 0; i < totalMeasures; i++)
            {
                var diceValue = GetDiceValue();
                measureArray[i] = GetMeasureNumber(i, diceValue);
            }

            return measureArray;
        }

        public int GetDiceValue()
        {
            var r = new Random(randomSeed);
            var firstResult = r.Next(0, 6);
            var secondResult = r.Next(0, 6);

            return firstResult + secondResult;
        }

        public int GetMeasureNumber(int rowNumber, int diceValue)
        {
            var measureMap = MeasuresMap.GetMeasuresMap;
            return measureMap[rowNumber, diceValue];
        }

        public string BuildNewComposition(int[] measuresArray)
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

        public string RetrieveBeatsForMeasure(int measure)
        {
            var measureLength = 3;
            var startOfMeasure = (measure - 1) * measureLength;
            var endOfMeasure = startOfMeasure + measureLength;

            var measureArray = originalComposition.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

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
    }
}
