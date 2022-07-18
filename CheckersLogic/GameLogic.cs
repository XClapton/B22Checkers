using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersLogic.enums;

namespace CheckersLogic
{
    public class GameLogic
    {
        private readonly Random r_Rnd;
        private readonly Cell[,] r_Board;
        private readonly List<Move> r_PossibleStepsList;
        private readonly List<Move> r_PossibleJumpsList;
        private readonly List<Move> r_PossibleMultiplyJumpsList;
        private Player m_TurnToPlay;
        private Player m_NextToPlay;
        private Player m_RoundWinner;
        private bool m_IfDoubleJumpPossible;

        public GameLogic(int i_SizeOfBoard, Player i_Player1, Player i_Player2)
        {
            r_Board = new Cell[i_SizeOfBoard, i_SizeOfBoard];
            m_TurnToPlay = i_Player1;
            m_NextToPlay = i_Player2;
            m_RoundWinner = null;
            r_PossibleStepsList = new List<Move>();
            r_PossibleJumpsList = new List<Move>();
            r_PossibleMultiplyJumpsList = new List<Move>();
            m_IfDoubleJumpPossible = false;
            r_Rnd = new Random();
        }

        public Player TurnToPlay
        {
            get { return m_TurnToPlay; }
            set { m_TurnToPlay = value; }
        }

        public Player NextToPlay
        {
            get { return m_NextToPlay; }
            set { m_NextToPlay = value; }
        }

        public Player RoundWinner
        {
            get { return m_RoundWinner; }
        }

        public bool IfDoubleJumpPossible
        {
            get { return m_IfDoubleJumpPossible; }
        }

        public Cell[,] Board
        {
            get { return r_Board; }
        }

        public void SetBoard()
        {
            bool isValidSettingHelper = false;

            for (int i = 0; i < r_Board.GetLength(0); i++)
            {
                for (int j = 0; j < r_Board.GetLength(1); j++)
                {
                    r_Board[i, j] = new Cell(eEntity.Inaccessible);

                    if (!isValidSettingHelper)
                    {
                        r_Board[i, j].Entity = eEntity.Inaccessible;
                    }
                    else if (i == (r_Board.GetLength(0) / 2) - 1 || i == r_Board.GetLength(0) / 2)
                    {
                        r_Board[i, j].Entity = eEntity.Empty;
                    }
                    else if (i < (r_Board.GetLength(0) / 2) - 1)
                    {
                        r_Board[i, j].Entity = eEntity.Player2;
                    }
                    else
                    {
                        r_Board[i, j].Entity = eEntity.Player1;
                    }

                    if (j != r_Board.GetLength(0) - 1)
                    {
                        isValidSettingHelper = !isValidSettingHelper;
                    }
                }
            }
        }

        private void deleteInBetween(Move i_Move)
        {
            int rowDistance = i_Move.DestRow - i_Move.CurrRow;
            int colDistance = i_Move.DestCol - i_Move.CurrCol;

            if (rowDistance == -2 && colDistance == 2)
            {
                r_Board[i_Move.CurrRow - 1, i_Move.CurrCol + 1].Entity = eEntity.Empty;
                r_Board[i_Move.CurrRow - 1, i_Move.CurrCol + 1].IsKing = false;
            }

            if (rowDistance == -2 && colDistance == -2)
            {
                r_Board[i_Move.CurrRow - 1, i_Move.CurrCol - 1].Entity = eEntity.Empty;
                r_Board[i_Move.CurrRow - 1, i_Move.CurrCol - 1].IsKing = false;
            }

            if (rowDistance == 2 && colDistance == 2)
            {
                r_Board[i_Move.CurrRow + 1, i_Move.CurrCol + 1].Entity = eEntity.Empty;
                r_Board[i_Move.CurrRow + 1, i_Move.CurrCol + 1].IsKing = false;
            }

            if (rowDistance == 2 && colDistance == -2)
            {
                r_Board[i_Move.CurrRow + 1, i_Move.CurrCol - 1].Entity = eEntity.Empty;
                r_Board[i_Move.CurrRow + 1, i_Move.CurrCol - 1].IsKing = false;
            }
        }

        private List<Move> buildAllPossStepsForCurrToPlay()
        {
            r_PossibleStepsList.Clear();

            for (int i = 0; i < r_Board.GetLength(0); i++)
            {
                for (int j = 0; j < r_Board.GetLength(1); j++)
                {
                    if (r_Board[i, j].Entity == m_TurnToPlay.EntityPlayer)
                    {
                        if (m_TurnToPlay.EntityPlayer == eEntity.Player1)
                        {
                            allPossStepsForPlayer1(i, j);
                        }
                        else if (m_TurnToPlay.EntityPlayer == eEntity.Player2)
                        {
                            allPossStepsForPlayer2(i, j);
                        }
                    }
                }
            }

            return r_PossibleStepsList;
        }

        private void allPossStepsForPlayer1(int i_Row, int i_Col)
        {
            if (checkTypeOfStepsValidation(i_Row - 1, i_Col + 1))
            {
                Move stepMove = new Move(i_Col, i_Row, i_Col + 1, i_Row - 1);
                r_PossibleStepsList.Add(stepMove);
            }

            if (checkTypeOfStepsValidation(i_Row - 1, i_Col - 1))
            {
                Move stepMove = new Move(i_Col, i_Row, i_Col - 1, i_Row - 1);
                r_PossibleStepsList.Add(stepMove);
            }

            if (r_Board[i_Row, i_Col].IsKing)
            {
                if (checkTypeOfStepsValidation(i_Row + 1, i_Col - 1))
                {
                    Move stepMove = new Move(i_Col, i_Row, i_Col - 1, i_Row + 1);
                    r_PossibleStepsList.Add(stepMove);
                }

                if (checkTypeOfStepsValidation(i_Row + 1, i_Col + 1))
                {
                    Move stepMove = new Move(i_Col, i_Row, i_Col + 1, i_Row + 1);
                    r_PossibleStepsList.Add(stepMove);
                }
            }
        }

        private void allPossStepsForPlayer2(int i_Row, int i_Col)
        {
            if (checkTypeOfStepsValidation(i_Row + 1, i_Col - 1))
            {
                Move stepMove = new Move(i_Col, i_Row, i_Col - 1, i_Row + 1);
                r_PossibleStepsList.Add(stepMove);
            }

            if (checkTypeOfStepsValidation(i_Row + 1, i_Col + 1))
            {
                Move stepMove = new Move(i_Col, i_Row, i_Col + 1, i_Row + 1);
                r_PossibleStepsList.Add(stepMove);
            }

            if (r_Board[i_Row, i_Col].IsKing)
            {
                if (checkTypeOfStepsValidation(i_Row - 1, i_Col + 1))
                {
                    Move stepMove = new Move(i_Col, i_Row, i_Col + 1, i_Row - 1);
                    r_PossibleStepsList.Add(stepMove);
                }

                if (checkTypeOfStepsValidation(i_Row - 1, i_Col - 1))
                {
                    Move stepMove = new Move(i_Col, i_Row, i_Col - 1, i_Row - 1);
                    r_PossibleStepsList.Add(stepMove);
                }
            }
        }

        private List<Move> buildAllPossJumpsForCurrToPlay()
        {
            r_PossibleJumpsList.Clear();

            for (int i = 0; i < r_Board.GetLength(0); i++)
            {
                for (int j = 0; j < r_Board.GetLength(1); j++)
                {
                    if (r_Board[i, j].Entity == m_TurnToPlay.EntityPlayer)
                    {
                        if (m_TurnToPlay.EntityPlayer == eEntity.Player1 || r_Board[i, j].IsKing)
                        {
                            if (checkTypeOfJumpingValidation(i - 2, j + 2, i - 1, j + 1))
                            {
                                Move eatMove = new Move(j, i, j + 2, i - 2);
                                r_PossibleJumpsList.Add(eatMove);
                            }

                            if (checkTypeOfJumpingValidation(i - 2, j - 2, i - 1, j - 1))
                            {
                                Move eatMove = new Move(j, i, j - 2, i - 2);
                                r_PossibleJumpsList.Add(eatMove);
                            }
                        }

                        if (m_TurnToPlay.EntityPlayer == eEntity.Player2 || r_Board[i, j].IsKing)
                        {
                            if (checkTypeOfJumpingValidation(i + 2, j + 2, i + 1, j + 1))
                            {
                                Move eatMove = new Move(j, i, j + 2, i + 2);
                                r_PossibleJumpsList.Add(eatMove);
                            }

                            if (checkTypeOfJumpingValidation(i + 2, j - 2, i + 1, j - 1))
                            {
                                Move eatMove = new Move(j, i, j - 2, i + 2);
                                r_PossibleJumpsList.Add(eatMove);
                            }
                        }
                    }
                }
            }

            return r_PossibleJumpsList;
        }

        private void buildPossiblePieceJumpsList(Move i_Move)
        {
            List<Move> allPossibleJumpsList = buildAllPossJumpsForCurrToPlay();

            foreach (Move jumpMove in allPossibleJumpsList)
            {
                if (jumpMove.CurrRow == i_Move.DestRow && jumpMove.CurrCol == i_Move.DestCol)
                {
                    Move secondJump = new Move(jumpMove.CurrCol, jumpMove.CurrRow, jumpMove.DestCol, jumpMove.DestRow);
                    r_PossibleMultiplyJumpsList.Add(secondJump);
                    m_IfDoubleJumpPossible = true;
                }
            }
        }

        private bool checkTypeOfJumpingValidation(int i_DestRow, int i_DestCol, int i_JumpAbovedRow, int i_JumpAbovedCol)
        {
            bool ifValidMove = false;

            if (i_DestRow >= 0 && i_DestRow < r_Board.GetLength(0) && i_DestCol >= 0 && i_DestCol < r_Board.GetLength(1) && r_Board[i_DestRow, i_DestCol].Entity == eEntity.Empty)
            {
                if (i_JumpAbovedRow >= 0 && i_JumpAbovedCol < r_Board.GetLength(1) && r_Board[i_JumpAbovedRow, i_JumpAbovedCol].Entity == m_NextToPlay.EntityPlayer)
                {
                    ifValidMove = true;
                }
            }

            return ifValidMove;
        }

        public bool ApplayMoveOfHuman(Move i_Move)
        {
            bool isValidMove = false;

            buildAllPossJumpsForCurrToPlay();
            buildAllPossStepsForCurrToPlay();

            if (r_PossibleMultiplyJumpsList.Count() != 0)
            {
                if (r_PossibleMultiplyJumpsList.Any
                    (x => x.CurrRow == i_Move.CurrRow && x.CurrCol == i_Move.CurrCol && x.DestRow == i_Move.DestRow && x.DestCol == i_Move.DestCol))
                {
                    isValidMove = true;
                    handleJump(i_Move);
                    r_PossibleMultiplyJumpsList.Clear();

                    if (r_PossibleMultiplyJumpsList.Count() == 0)
                    {
                        m_IfDoubleJumpPossible = false;
                    }
                }
            }
            else if (r_PossibleJumpsList.Count() != 0)
            {
                if (r_PossibleJumpsList.Any
                        (x => x.CurrRow == i_Move.CurrRow && x.CurrCol == i_Move.CurrCol && x.DestRow == i_Move.DestRow && x.DestCol == i_Move.DestCol))
                {
                    isValidMove = true;
                    handleJump(i_Move);
                }
            }
            else if (r_PossibleStepsList.Count() != 0)
            {
                if (r_PossibleStepsList.Any
                       (x => x.CurrRow == i_Move.CurrRow && x.CurrCol == i_Move.CurrCol && x.DestRow == i_Move.DestRow && x.DestCol == i_Move.DestCol))
                {
                    applayMoveOnBoard(i_Move);
                    isValidMove = true;
                }
            }
            else
            {
                m_TurnToPlay.IfOutOfMoves = true;
            }

            if (isValidMove == true)
            {
                checkIfThereIsAWinner();
            }

            return isValidMove;
        }

        private void handleJump(Move i_Move)
        {
            m_NextToPlay.CurrAmountOfPieces--;
            applayMoveOnBoard(i_Move);
            deleteInBetween(i_Move);
            buildPossiblePieceJumpsList(i_Move);

            if (r_PossibleMultiplyJumpsList.Count() != 0)
            {
                m_IfDoubleJumpPossible = true;
            }
        }

        public Move ApplayMoveOfComputer()
        {
            Move move = null;
            int drawnNumber;

            buildAllPossJumpsForCurrToPlay();
            buildAllPossStepsForCurrToPlay();

            if (r_PossibleMultiplyJumpsList.Count() != 0)
            {
                m_NextToPlay.CurrAmountOfPieces--;
                drawnNumber = r_Rnd.Next(0, r_PossibleMultiplyJumpsList.Count() - 1);
                move = r_PossibleMultiplyJumpsList[drawnNumber];
                applayMoveOnBoard(move);
                deleteInBetween(move);
                r_PossibleMultiplyJumpsList.Clear();
                buildPossiblePieceJumpsList(move);
                if (r_PossibleMultiplyJumpsList.Count() != 0)
                {
                    m_IfDoubleJumpPossible = true;
                }
                else
                {
                    m_IfDoubleJumpPossible = false;
                }
            }
            else if (r_PossibleJumpsList.Count() != 0)
            {
                m_NextToPlay.CurrAmountOfPieces--;
                drawnNumber = r_Rnd.Next(0, r_PossibleJumpsList.Count() - 1);
                move = r_PossibleJumpsList[drawnNumber];
                applayMoveOnBoard(move);
                deleteInBetween(move);
                buildPossiblePieceJumpsList(move);
                if (r_PossibleMultiplyJumpsList.Count() != 0)
                {
                    m_IfDoubleJumpPossible = true;
                }
            }
            else if (r_PossibleStepsList.Count() != 0)
            {
                drawnNumber = r_Rnd.Next(0, r_PossibleStepsList.Count() - 1);
                move = r_PossibleStepsList[drawnNumber];
                applayMoveOnBoard(move);
            }
            else
            {
                m_TurnToPlay.IfOutOfMoves = true;
            }

            checkIfThereIsAWinner();

            return move;
        }

        public int GetSizeOfBoard()
        {
            return r_Board.GetLength(0);
        }

        public void SwitchPlayerTurn()
        {
            Player temp = m_TurnToPlay;

            m_TurnToPlay = m_NextToPlay;

            m_NextToPlay = temp;
        }

        private void applayMoveOnBoard(Move i_Move)
        {
            bool ifCurrPieceIsKing = r_Board[i_Move.CurrRow, i_Move.CurrCol].IsKing;

            r_Board[i_Move.CurrRow, i_Move.CurrCol].Entity = eEntity.Empty;
            r_Board[i_Move.CurrRow, i_Move.CurrCol].IsKing = false;
            r_Board[i_Move.DestRow, i_Move.DestCol].Entity = m_TurnToPlay.EntityPlayer;

            if ((i_Move.DestRow == 0 && m_TurnToPlay.EntityPlayer == eEntity.Player1) ||
                (i_Move.DestRow == r_Board.GetLength(0) - 1 && m_TurnToPlay.EntityPlayer == eEntity.Player2))
            {
                r_Board[i_Move.DestRow, i_Move.DestCol].IsKing = true;
            }
            else
            {
                r_Board[i_Move.DestRow, i_Move.DestCol].IsKing = false;
            }

            if (ifCurrPieceIsKing)
            {
                r_Board[i_Move.DestRow, i_Move.DestCol].IsKing = true;
            }
        }

        private bool checkTypeOfStepsValidation(int i_DestRow, int i_DestCol)
        {
            bool ifValidMove = false;

            if (i_DestRow >= 0 && i_DestRow < r_Board.GetLength(0) && i_DestCol >= 0 && i_DestCol < r_Board.GetLength(1))
            {
                if (r_Board[i_DestRow, i_DestCol].Entity == eEntity.Empty)
                {
                    ifValidMove = true;
                }
            }

            return ifValidMove;
        }

        private void checkIfThereIsAWinner()
        {
            Player winner = null;

            if (m_NextToPlay.CurrAmountOfPieces == 0)
            {
                winner = m_TurnToPlay;
            }
            else if (m_TurnToPlay.IfOutOfMoves == true)
            {
                winner = m_NextToPlay;
            }

            m_RoundWinner = winner;

            if (winner != null)
            {
                calculatePlayersScore();
            }
        }

        private void calculatePlayersScore()
        {
            int player1Score = 0, player2Score = 0;
            int winnerFinalScore;

            foreach (Cell cell in r_Board)
            {
                if (cell.Entity == eEntity.Player1)
                {
                    if (cell.IsKing)
                    {
                        player1Score += 4;
                    }
                    else
                    {
                        player1Score++;
                    }
                }

                if (cell.Entity == eEntity.Player2)
                {
                    if (cell.IsKing)
                    {
                        player2Score += 4;
                    }
                    else
                    {
                        player2Score++;
                    }
                }
            }

            winnerFinalScore = Math.Max(player1Score, player2Score) - Math.Min(player1Score, player2Score);
            m_RoundWinner.Score += winnerFinalScore;
        }

        public void InitRound()
        {
            int boardSize = r_Board.GetLength(0);
            int playerAmountOfPieces = (int)((boardSize - 2) * 0.5 * 0.5 * boardSize);

            m_TurnToPlay.CurrAmountOfPieces = playerAmountOfPieces;
            m_NextToPlay.CurrAmountOfPieces = playerAmountOfPieces;

            m_TurnToPlay.IfOutOfMoves = false;
            m_NextToPlay.IfOutOfMoves = false;

            if (m_TurnToPlay.EntityPlayer != eEntity.Player1)
            {
                SwitchPlayerTurn();
            }

            SetBoard();
        }
    }
}