using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class PlayerInformation
    {
        private const int FirstPointsBoundary = 5;
        private const int SecondPointsBoundary = 10;
        private const int StartNumber = 0;

        public enum StatusType { Beginner, Specialist, Master };

        public string playerName;
        public StatusType playerStatus;

        public int numberOfPlayedGames;
        public int pointsNumber;

        public int winsNumber;
        public int drawsNumber;
        public int losesNumber;
        public int rightAnswersNumber;
        public int wrongAnswersNumber;

        public PlayerInformation(string name, int rightAnswers, int wrongAnswers)
        {
            playerName = name;
            playerStatus = StatusType.Beginner;
            numberOfPlayedGames = 1;
            winsNumber = StartNumber;
            drawsNumber = StartNumber;
            losesNumber = StartNumber;

            rightAnswersNumber = rightAnswers;
            wrongAnswersNumber = wrongAnswers;
        }

        public StatusType DefineStatus()
        {
            StatusType currentStatus;
            if ((winsNumber - losesNumber > 1) && (pointsNumber > SecondPointsBoundary))
            {
                currentStatus = StatusType.Master;
            }
            else if ((winsNumber >= losesNumber) && (pointsNumber > FirstPointsBoundary))
            {
                currentStatus = StatusType.Specialist;
            }
            else
            {
                currentStatus = StatusType.Beginner;
            }     
            return currentStatus;
        }
    }
}
