using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace KidsLearning.Assets
{
    public class MathScoreSaver : ScoreSaver {        

        public override string ConstructSummary(IQuestion arithmetic, List<string> wrongAnswers)
        {
            string summary = $"Solved {arithmetic.ToString()}";
            if (wrongAnswers.Count > 0)
            {
                summary += $"\nWrong Answers:\n{string.Join('\n', wrongAnswers)}";
            }

            return summary;
        }
    }
}