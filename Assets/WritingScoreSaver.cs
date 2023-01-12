using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidsLearning.Assets
{
    public class WritingScoreSaver : ScoreSaver
    {
        public override string ConstructSummary(IQuestion arithmetic, List<string> wrongAnswers)
        {
            string summary = $"Written the letter {arithmetic.ToString()} correctly";
            summary += $"\nNo. of trials: {Mathf.Max(1, wrongAnswers.Count)}";

            return summary;
        }
    }
}