using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersLogic;
using CheckersLogic.enums;

namespace Ex05.windowsUI
{
    public partial class CheckersForm : Form
    {
        private readonly SettingsForm r_SettingsForm = new SettingsForm();
        private readonly Label labelPlayer1 = new Label();
        private readonly Label labelPlayer2 = new Label();
        private readonly System.Windows.Forms.Timer timer;
        private PictureBox pictureBoxPlayer1PointingFinger = new PictureBox();
        private PictureBox pictureBoxPlayer2PointingFinger = new PictureBox();
        private ButtonCell[,] m_ButtonsBoard;
        private GameLogic m_Game;
        private bool m_CurrPosIsChosen = false;
        private int m_CurrPosRow;
        private int m_CurrPosCol;
        
        public CheckersForm()
        {
            this.Text = "Checkers Game";
            this.StartPosition = FormStartPosition.Manual;
            this.MaximizeBox = false;
            this.Left = 20;
            this.Top = 20;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.Load += checkersForm_Load;
            this.Shown += checkersForm_Shown;
        }

        private void checkersForm_Load(object sender, EventArgs e)
        {
            r_SettingsForm.ShowDialog();
        }

        private void checkersForm_Shown(object sender, EventArgs e)
        {
            Player player1 = new Player(ePlayerType.Human, eEntity.Player1, r_SettingsForm.FirstPlayerName, 0);
            Player player2 = new Player(r_SettingsForm.SecondPlayerType, eEntity.Player2, r_SettingsForm.SecondPlayerName, 0);
            m_Game = new GameLogic(r_SettingsForm.BoardSize, player1, player2);
            m_Game.InitRound();
            initControls();
            graphicButtonsInit();
            updateButtonsAvailableToStartMove();
        }

        private void initControls()
        {
            labelPlayer1.Font = new Font("Corbel", 11, FontStyle.Bold);
            labelPlayer1.Location = new System.Drawing.Point(30, 6);
            labelPlayer1.Text = m_Game.TurnToPlay.Name + ": " + m_Game.TurnToPlay.Score.ToString();
            labelPlayer1.AutoSize = true;
            this.Controls.Add(this.labelPlayer1);

            labelPlayer2.Font = new Font("Corbel", 11, FontStyle.Bold);
            labelPlayer2.Location = new System.Drawing.Point(260, 6);
            labelPlayer2.Text = m_Game.NextToPlay.Name + ": " + m_Game.NextToPlay.Score.ToString();
            labelPlayer2.AutoSize = true;
            this.Controls.Add(this.labelPlayer2);

            pictureBoxPlayer1PointingFinger = new PictureBox();
            pictureBoxPlayer1PointingFinger.Image = Ex05.windowsUI.Properties.Resources.blackPointingFinger;
            pictureBoxPlayer1PointingFinger.BackColor = Color.Transparent;
            pictureBoxPlayer1PointingFinger.Location = new System.Drawing.Point(0, 6);
            Size size1 = new Size(30, 20);
            pictureBoxPlayer1PointingFinger.Size = size1;
            pictureBoxPlayer1PointingFinger.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPlayer1PointingFinger.BringToFront();
            this.Controls.Add(this.pictureBoxPlayer1PointingFinger);
           
            pictureBoxPlayer2PointingFinger = new PictureBox();
            pictureBoxPlayer2PointingFinger.Image = Ex05.windowsUI.Properties.Resources.whitePointingFinger;
            pictureBoxPlayer2PointingFinger.BackColor = Color.Transparent;
            pictureBoxPlayer2PointingFinger.Location = new System.Drawing.Point(230, 6);
            Size size2 = new Size(30, 20);
            pictureBoxPlayer2PointingFinger.Size = size2;
            pictureBoxPlayer2PointingFinger.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPlayer2PointingFinger.BringToFront();
            this.Controls.Add(this.pictureBoxPlayer2PointingFinger);
            pictureBoxPlayer2PointingFinger.Hide();

            m_ButtonsBoard = new ButtonCell[m_Game.Board.GetLength(0), m_Game.Board.GetLength(1)];
            addCellButtons(m_Game.Board.GetLength(0));
        }

        private void addCellButtons(int i_BoardSize)
        {
            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    addCell(i, j, m_Game.Board[i, j].Entity);
                }
            }
        }

        private void addCell(int i_RowPosition, int i_ColPosition, eEntity i_Entity)
        {
            ButtonCell button = new ButtonCell(i_RowPosition, i_ColPosition, i_Entity);
            button.Left = 6 + (i_ColPosition * 62);
            button.Top = 32 + (i_RowPosition * 62);
            button.Size = new System.Drawing.Size(62, 62);
            button.Click += new System.EventHandler(buttonCell_Click);
            button.Enabled = false;
            this.Controls.Add(button);
            m_ButtonsBoard[i_RowPosition, i_ColPosition] = button;
        }

        private void graphicButtonsInit()
        {
            for (int i = 0; i < m_Game.GetSizeOfBoard(); i++)
            {
                for (int j = 0; j < m_Game.GetSizeOfBoard(); j++)
                {
                    m_ButtonsBoard[i, j].AddImageToButton(m_Game.Board[i, j]);
                }
            }
        }

        private void updateButtonsAvailableToStartMove()
        {
            foreach (ButtonCell button in m_ButtonsBoard)
            {
                if (button.Active)
                {
                    button.Enabled = false;
                }
            }

            for (int i = 0; i < m_Game.Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Game.Board.GetLength(1); j++)
                {
                    if (m_Game.Board[i, j].Entity != eEntity.Empty && m_Game.Board[i, j].Entity != eEntity.Inaccessible)
                    {
                        m_ButtonsBoard[i, j].Enabled = true;
                    }
                }
            }
        }

        private void updateButtonsAvailableToEndMove()
        {
            foreach (ButtonCell button in m_ButtonsBoard)
            {
                if (button.Active)
                {
                    button.Enabled = true;
                }
            }
        }

        private void buttonCell_Click(object sender, EventArgs e)
        {
            ButtonCell button = sender as ButtonCell;

            if (m_CurrPosIsChosen == false)
            {
                chooseDestinationCell(button);
                m_CurrPosIsChosen = true;
            }
            else if (m_CurrPosRow == button.Row && m_CurrPosCol == button.Col)
            {
                cancelChoise(button);
                m_CurrPosIsChosen = false;
            }
            else
            {
                makeUserMove(button);
                m_CurrPosIsChosen = false;
            }
        }

        private void makeUserMove(ButtonCell i_ButtonCell)
        {
            Move move = new Move(m_CurrPosCol, m_CurrPosRow, i_ButtonCell.Col, i_ButtonCell.Row);
            bool legalMove = false;

            legalMove = m_Game.ApplayMoveOfHuman(move);

            if (legalMove == false)
            {
                m_ButtonsBoard[m_CurrPosRow, m_CurrPosCol].BackColor = System.Drawing.Color.LightGoldenrodYellow;
                MessageBox.Show("Illegal Move");
            }
            else
            {
                if (m_Game.IfDoubleJumpPossible == false)
                {     
                    m_Game.SwitchPlayerTurn();
                    swapPointingFingerDirection();
                }

                endUserMove(i_ButtonCell);
            }
        }

        private void endUserMove(ButtonCell i_ButtonCell)
        {
            i_ButtonCell.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            m_ButtonsBoard[m_CurrPosRow, m_CurrPosCol].BackColor = System.Drawing.Color.LightGoldenrodYellow;
            graphicButtonsInit();
            updateButtonsAvailableToStartMove();

            if (m_Game.RoundWinner != null)
            {
                finishTheRound();
            }
            else
            {
                if (m_Game.TurnToPlay.Type == ePlayerType.Computer)
                {
                    makeComputerMove();
                }

                if (m_Game.RoundWinner != null)
                {
                    finishTheRound();
                }
            }
        }

        private void makeComputerMove()
        {
            Move move = m_Game.ApplayMoveOfComputer();
            if (move == null)
            {
                return;
            }

            m_ButtonsBoard[move.DestRow, move.DestCol].BackColor = System.Drawing.Color.LightGoldenrodYellow;
            m_ButtonsBoard[move.CurrRow, move.CurrCol].BackColor = System.Drawing.Color.LightGoldenrodYellow;

            timer.Start();

            if(m_Game.IfDoubleJumpPossible == true)
            {
                makeComputerMove();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            graphicButtonsInit();
            updateButtonsAvailableToStartMove();

            if (m_Game.IfDoubleJumpPossible == false)
            {
                m_Game.SwitchPlayerTurn();
                swapPointingFingerDirection();
            }

            timer.Stop();
        }

        private void chooseDestinationCell(ButtonCell i_ButtonCell)
        {
            m_CurrPosRow = i_ButtonCell.Row;
            m_CurrPosCol = i_ButtonCell.Col;
            i_ButtonCell.BackColor = System.Drawing.Color.PaleTurquoise;
            updateButtonsAvailableToEndMove();
        }

        private void cancelChoise(ButtonCell i_ButtonCell)
        {
            i_ButtonCell.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            updateButtonsAvailableToStartMove();
        }

        private void swapPointingFingerDirection()
        {
            if(m_Game.TurnToPlay.EntityPlayer == eEntity.Player1)
            {
                pictureBoxPlayer1PointingFinger.Show();
                pictureBoxPlayer2PointingFinger.Hide();
            }
            else
            {
                pictureBoxPlayer2PointingFinger.Show();
                pictureBoxPlayer1PointingFinger.Hide();
            }
        }
        
        private void finishTheRound()
        {
            string resultMessage;

            if (m_Game.RoundWinner != null)
            {
                resultMessage = string.Format(@"{0} Won!{1}Another Round?", m_Game.RoundWinner.Name, Environment.NewLine);
            }
            else
            {
                resultMessage = string.Format(@"Tie!{0}Another Round?", Environment.NewLine);
            }

            DialogResult dialogResult = MessageBox.Show(resultMessage, "Checkers", MessageBoxButtons.YesNo);
            
            if (dialogResult == DialogResult.Yes)
            {
                initGameBeforeNewRound();
            }
            else
            {
                this.Close();
            }
        }

        private void clearButtonsBoard()
        {
            foreach (ButtonCell button in m_ButtonsBoard)
            {
                if (button.Enabled)
                {
                    resetActiveButton(button);
                }
            }
        }

        private void resetActiveButton(ButtonCell i_ButtonCell)
        {
            i_ButtonCell.BackgroundImage = null;
            i_ButtonCell.Enabled = false;
            i_ButtonCell.BackColor = System.Drawing.Color.LightGoldenrodYellow;
        }

        private void initGameBeforeNewRound()
        {
            m_Game.InitRound();
            swapPointingFingerDirection();
            clearButtonsBoard();
            graphicButtonsInit();
            updateButtonsAvailableToStartMove();
            updateScore();
        }

        private void updateScore()
        {
            labelPlayer1.Text = m_Game.TurnToPlay.Name + ": " + m_Game.TurnToPlay.Score.ToString();
            labelPlayer2.Text = m_Game.NextToPlay.Name + ": " + m_Game.NextToPlay.Score.ToString();
        }
    }
}