using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidsLearning
{
    public class SpellingScoreSaver : ScoreSaver
    {
        public override string ConstructSummary(IQuestion wordGuess, List<string> wrongAnswers)
        {
            return $"Spelled {wordGuess.ToString()} correctly";
        }
    }
}