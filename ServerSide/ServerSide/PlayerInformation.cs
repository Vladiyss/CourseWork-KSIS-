using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class PlayerInformation
    {
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
            winsNumber = 0;
            drawsNumber = 0;
            losesNumber = 0;

            rightAnswersNumber = rightAnswers;
            wrongAnswersNumber = wrongAnswers;
        }

        public StatusType DefineStatus()
        {
            StatusType currentStatus;
            if ((winsNumber - losesNumber > 1) && (pointsNumber > 10))
            {
                currentStatus = StatusType.Master;
            }
            else if ((winsNumber >= losesNumber) && (pointsNumber > 5))
            {
                currentStatus = StatusType.Specialist;
            }
            else
                currentStatus = StatusType.Beginner;
            return currentStatus;
        }
    }
}
