using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MozartMusicalDice
{
    public class MusicalDice
    {
        private string originalComposition;
        public MusicalDice(string compositionFilePath)
        {
            originalComposition = File.ReadAllText(compositionFilePath);
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

        public int GetMeasureNumber(int rowNumber, int diceValue)
        {
            var adjustedRowNumber = rowNumber - 1;
            var adjustedDiceValue = diceValue - 2;

            var randomMeasures = new[,]
            {
                {96, 32, 69, 40, 148, 104, 152, 119, 98, 3, 54},
                {22, 6, 95, 17, 74, 157, 60, 84, 142, 87, 130},
                {141, 128, 158, 113, 163, 27, 171, 114, 42, 165, 10},
                {41, 63, 13, 85, 45, 167, 53, 50, 156, 61, 103},
                {105, 146, 153, 161, 80, 154, 99, 140, 75, 135, 28},
                {122, 46, 55, 2, 97, 68, 133, 86, 129, 47, 37},
                {11, 134, 110, 159, 36, 118, 21, 169, 62, 147, 106},
                {30, 81, 24, 100, 107, 91, 127, 94, 123, 33, 5},
                {70, 117, 66, 90, 25, 138, 16, 120, 65, 102, 35},
                {121, 39, 136, 176, 143, 71, 155, 88, 77, 4, 20},
                {26, 126, 15, 7, 64, 150, 57, 48, 19, 31, 108},
                {9, 56, 132, 34, 125, 29, 175, 166, 82, 164, 92},
                {112, 174, 73, 67, 76, 101, 43, 51, 137, 144, 12},
                {49, 18, 58, 160, 136, 162, 168, 115, 38, 59, 124},
                {109, 116, 145, 52, 1, 23, 89, 72, 149, 173, 44},
                {14, 83, 79, 170, 93, 151, 172, 111, 8, 78, 131}
            };
            return randomMeasures[adjustedRowNumber, adjustedDiceValue];
        }

        public int GetDiceValue(int seed)
        {
            var r = new Random(seed);
            var firstResult = r.Next(1, 7);
            var secondResult = r.Next(1, 7);

            return firstResult + secondResult;
        }
    }
}
