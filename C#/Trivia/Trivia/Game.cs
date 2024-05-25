using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public enum CategoryType
    {
        Pop,
        Science,
        Sports,
        Rock
    }

    public class Game
    {
        private readonly List<string> players = new List<string>();

        private readonly int[] places = new int[6];
        private readonly int[] purses = new int[6];

        private readonly bool[] inPenaltyBox = new bool[6];

        private readonly LinkedList<string> popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> rockQuestions = new LinkedList<string>();

        private int currentPlayer;
        private bool isGettingOutOfPenaltyBox;

        public Game()
        {
            CreateQuestions(50);
        }

        private void CreateQuestions(int numberOfQuestions)
        {
            for (var i = 0; i < numberOfQuestions; i++)
            {
                popQuestions.AddLast(CreatePopQuestion(i));
                scienceQuestions.AddLast(CreateScienceQuestion(i));
                sportsQuestions.AddLast(CreateSportsQuestion(i));
                rockQuestions.AddLast(CreateRockQuestion(i));
            }
        }

        public string CreateRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public string CreatePopQuestion(int index)
        {
            return "Pop Question " + index;
        }

        public string CreateScienceQuestion(int index)
        {
            return "Science Question " + index;
        }

        public string CreateSportsQuestion(int index)
        {
            return "Sports Question " + index;
        }

        public bool AddPlayer(string playerName)
        {
            players.Add(playerName);
            places[GetPlayersCount()] = 0;
            purses[GetPlayersCount()] = 0;
            inPenaltyBox[GetPlayersCount()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int GetPlayersCount()
        {
            return players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(CurrentPlayerName() + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (!IsCurrentUserInPenaltyBox())
            {
                AdvanceUserPosition(roll);
                Console.WriteLine(CurrentPlayerName() + "'s new location is " + places[currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }

            HandlePenaltyBox(roll);
        }

        private void HandlePenaltyBox(int roll)
        {
            isGettingOutOfPenaltyBox = IsRollEven(roll);
            if (!isGettingOutOfPenaltyBox)
            {
                Console.WriteLine(CurrentPlayerName() + " is not getting out of the penalty box");
                return;
            }

            MovePlayerOutOfPenaltyBox(roll);
            AskQuestion();
        }

        private bool IsCurrentUserInPenaltyBox()
        {
            return inPenaltyBox[currentPlayer];
        }

        public void MovePlayerOutOfPenaltyBox(int roll)
        {
            Console.WriteLine(CurrentPlayerName() + " is getting out of the penalty box");
            AdvanceUserPosition(roll);

            Console.WriteLine(CurrentPlayerName() + "'s new location is " + places[currentPlayer]);
            Console.WriteLine("The category is " + CurrentCategory());
        }

        private string CurrentPlayerName()
        {
            return players[currentPlayer];
        }

        private void AdvanceUserPosition(int roll)
        {
            places[currentPlayer] = places[currentPlayer] + roll;
            if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;
        }

        private static bool IsRollEven(int roll)
        {
            return roll % 2 == 0;
        }

        private void AskQuestion()
        {
            var category = CurrentCategory();
            var question = GetQuestion(category);
            Console.WriteLine(question);
            RemoveQuestion(category);
        }

        private string GetQuestion(CategoryType category)
        {
            switch (category)
            {
                case CategoryType.Pop:
                    return GetAndRemoveFirstQuestion(popQuestions);
                case CategoryType.Science:
                    return GetAndRemoveFirstQuestion(scienceQuestions);
                case CategoryType.Sports:
                    return GetAndRemoveFirstQuestion(sportsQuestions);
                case CategoryType.Rock:
                    return GetAndRemoveFirstQuestion(rockQuestions);
                default:
                    return string.Empty;
            }
        }

        private string GetAndRemoveFirstQuestion(LinkedList<string> questionList)
        {
            var question = questionList.First();
            questionList.RemoveFirst();
            return question;
        }

        private void RemoveQuestion(CategoryType category)
        {
            switch (category)
            {
                case CategoryType.Pop:
                    popQuestions.RemoveFirst();
                    break;
                case CategoryType.Science:
                    scienceQuestions.RemoveFirst();
                    break;
                case CategoryType.Sports:
                    sportsQuestions.RemoveFirst();
                    break;
                case CategoryType.Rock:
                    rockQuestions.RemoveFirst();
                    break;
            }
        }

        private CategoryType CurrentCategory()
        {
            switch (places[currentPlayer])
            {
                case 0:
                case 4:
                case 8:
                    return CategoryType.Pop;
                case 1:
                case 5:
                case 9:
                    return CategoryType.Science;
                case 2:
                case 6:
                case 10:
                    return CategoryType.Sports;
                default:
                    return CategoryType.Rock;
            }
        }

        public bool WasCorrectlyAnswered()
        {
            if (!IsCurrentUserInPenaltyBox() && !isGettingOutOfPenaltyBox)
            {
                SetNextPlayer();
                return true;
            }

            Console.WriteLine("Answer was correct!!!!");
            purses[currentPlayer]++;
            Console.WriteLine(CurrentPlayerName() + " now has " + purses[currentPlayer] + " Gold Coins.");

            var winner = DidPlayerWin();
            SetNextPlayer();

            return winner;
        }

        private void SetNextPlayer()
        {
            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(CurrentPlayerName() + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            SetNextPlayer();
            return true;
        }


        private bool DidPlayerWin()
        {
            return purses[currentPlayer] != 6;
        }
     
    }

}
