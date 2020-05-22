using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class Game
    {
        public int firstPlayerID;
        public int secondPlayerID;

        public string firstPlayerName;
        public string secondPlayerName;

        public int firstPlayerPoints;
        public int secondPlayerPoints;

        public int firstNumberAnsweredQuestions;
        public int secondNumberAnsweredQuestions;

        public string gameResults;

        public bool isPlayedNow;
        public bool isFirstFinished;
        public bool isSecondFinished;

        public Game(int firstPlayerID, int secondPlayerID, string firstPlayerName, string secondPlayerName)
        {
            this.firstPlayerID = firstPlayerID;
            this.secondPlayerID = secondPlayerID;

            this.firstPlayerName = firstPlayerName;
            this.secondPlayerName = secondPlayerName;

            firstPlayerPoints = 0;
            secondPlayerPoints = 0;

            firstNumberAnsweredQuestions = 0;
            secondNumberAnsweredQuestions = 0;

            isPlayedNow = true;
            isFirstFinished = false;
            isSecondFinished = false;
        }
    }
}
