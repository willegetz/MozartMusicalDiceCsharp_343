﻿using System;
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
    }
}