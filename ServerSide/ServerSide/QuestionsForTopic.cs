using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    public enum GameTopic { AroundTheWorld, ScienceGranite, TechnicalProgress };

    public struct Answer
    {
        public string Title;
        public bool IsRight;
    }

    class QuestionsForTopic
    {
        public static Dictionary<GameTopic, string> TopicsDictionary = new Dictionary<GameTopic, string>()
        {
            [GameTopic.AroundTheWorld] = "Вокруг света",
            [GameTopic.ScienceGranite] = "Гранит науки",
            [GameTopic.TechnicalProgress] = "Технический прогресс"
        };

        public string[] questions;
        public Answer[,] answers;
    }
}
