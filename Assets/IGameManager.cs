using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsLearning
{
    internal interface IGameManager
    {
        Action<IQuestion, List<string>> CorrectAnsweredEvent { get; set; }
        Action LevelFinishedEvent { get; set; }
        Action<bool> BeginPuzzleEvent { get; set; }

        List<string> WrongAnswers { get; set; }
        List<string> AllCorrectAnswers { get; set; }
        List<string> AllWrongAnswers { get; set; }
    }
}
