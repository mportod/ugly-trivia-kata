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
        private readonly List<string> _players = new List<string>();

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
            for (var i = 0; i < 50; i++)
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

        public bool Add(string playerName)
        {
            _players.Add(playerName);
            places[HowManyPlayers()] = 0;
            purses[HowManyPlayers()] = 0;
            inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(_players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                    Console.WriteLine(_players[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(_players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                Console.WriteLine(_players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == CategoryType.Pop)
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (CurrentCategory() == CategoryType.Science)
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory() == CategoryType.Sports)
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory() == CategoryType.Rock)
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
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

            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Console.WriteLine(_players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    SetNextPlayer();

                    return winner;
                }
                else
                {
                    SetNextPlayer();
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                purses[currentPlayer]++;
                Console.WriteLine(_players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                SetNextPlayer();

                return winner;
            }
        }

        private void SetNextPlayer()
        {
            currentPlayer++;
            if (currentPlayer == _players.Count) currentPlayer = 0;
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[currentPlayer] + " was sent to the penalty box");
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
