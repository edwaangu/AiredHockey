using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Aired_Hockey
{
    public partial class Form1 : Form
    {
        // Rectangular collision variables
        Rectangle player1 = new Rectangle(175, 125, 50, 50);
        Rectangle player2 = new Rectangle(175, 525, 50, 50);
        Rectangle puck = new Rectangle(185, 335, 30, 30);

        // Sounds
        SoundPlayer puckHitPlayer = new SoundPlayer(Properties.Resources.puckHit);
        SoundPlayer puckGoalPlayer = new SoundPlayer(Properties.Resources.puckGoal);

        // Movement variables for both players
        bool wKeyDown = false;
        bool sKeyDown = false;
        bool aKeyDown = false;
        bool dKeyDown = false;

        bool upKeyDown = false;
        bool downKeyDown = false;
        bool leftKeyDown = false;
        bool rightKeyDown = false;

        // Game variables
        bool gameStarted;
        bool gameEnded = false;
        double timer = 180;
        int p1Points = 0;
        int p2Points = 0;

        // Puck and player speed variables
        double puckXSpeed = 0;
        double puckYSpeed = 0;
        bool isPuckRight = false;
        int player1XSpeed = 0;
        int player2XSpeed = 0;
        int player1YSpeed = 0;
        int player2YSpeed = 0;

        // Brush for drawing
        SolidBrush solidBrush = new SolidBrush(Color.CornflowerBlue);

        public Form1()
        {
            InitializeComponent();
            gameStarted = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // Check for when a specfic key was left go
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.D:
                    dKeyDown = false;
                    break;

                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
                case Keys.Left:
                    leftKeyDown = false;
                    break;
                case Keys.Right:
                    rightKeyDown = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for when a key is held down
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.D:
                    dKeyDown = true;
                    break;

                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Left:
                    leftKeyDown = true;
                    break;
                case Keys.Right:
                    rightKeyDown = true;
                    break;
            }

        }

        public void resetGame()
        {
            // Reset puck/player related values to their originals when a puck is scored into the goal.
            puck.X = 185;
            puck.Y = 335;

            player1.X = 175;
            player1.Y = 125;
            player2.X = 175;
            player2.Y = 525;

            puckXSpeed = 0;
            puckYSpeed = 0;
            player1XSpeed = 0;
            player1YSpeed = 0;
            player2XSpeed = 0;
            player2YSpeed = 0;
        }

        public void checkForWinner()
        {
            // Depending on the player's points and time left, check the winner every frame to not miss it.
            if(p1Points >= 3)
            {
                winLabel.Visible = true;
                winLabel.Text = "Player 1 Wins!";
                gameEnded = true;
                timerTick.Enabled = false;
            }
            else if(p2Points >= 3)
            {
                winLabel.Visible = true;
                winLabel.Text = "Player 2 Wins!";
                gameEnded = true;
                timerTick.Enabled = false;
            }
            else if(timer <= 0)
            {
                timerTick.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Both players ran out of time!";
                gameEnded = true;
                resetGame();
            }
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Draw the three images
            e.Graphics.DrawImage(Properties.Resources.player1, player1);
            e.Graphics.DrawImage(Properties.Resources.player2, player2);
            e.Graphics.DrawImage(Properties.Resources.puck, puck);
        }

        private void gameTick_Tick(object sender, EventArgs e)
        {
            if (gameEnded == false)
            {
                // Player Movement
                if (wKeyDown == true)
                {
                    player1YSpeed -= 3;
                }
                if (sKeyDown == true)
                {
                    player1YSpeed += 3;
                }
                if (aKeyDown == true)
                {
                    player1XSpeed -= 3;
                }
                if (dKeyDown == true)
                {
                    player1XSpeed += 3;
                }


                // Player 2 Movement
                if (upKeyDown)
                {
                    player2YSpeed -= 3;
                }
                if (downKeyDown)
                {
                    player2YSpeed += 3;
                }
                if (leftKeyDown)
                {
                    player2XSpeed -= 3;
                }
                if (rightKeyDown)
                {
                    player2XSpeed += 3;
                }
            }

            // Move by the player speeds;
            player1.X += player1XSpeed;
            player2.X += player2XSpeed;
            player1.Y += player1YSpeed;
            player2.Y += player2YSpeed;

            // Player 1 Constraints
            if(player1.Y < 58)
            {
                player1.Y = 58;
                player1YSpeed = 0;
            }
            if (player1.Y > 350 - player1.Height)
            {
                player1.Y = 350 - player1.Height;
                player1YSpeed = 0;
            }
            if(player1.X < 8)
            {
                player1.X = 8;
                player1XSpeed = 0;
            }
            if (player1.X > this.Width - player1.Width - 8)
            {
                player1.X = this.Width - player1.Width - 8;
                player1XSpeed = 0;
            }

            // Player 2 Constraints
            if (player2.Y < 350)
            {
                player2.Y = 350;
                player2YSpeed = 0;
            }
            if (player2.Y > this.Height - player2.Height - 8)
            {
                player2.Y = this.Height - player2.Height - 8;
                player2YSpeed = 0;
            }
            if (player2.X < 8)
            {
                player2.X = 8;
                player2XSpeed = 0;
            }
            if (player2.X > this.Width - player2.Width - 8)
            {
                player2.X = this.Width - player2.Width - 8;
                player2XSpeed = 0;
            }

            // Puck movement
            if(player1.IntersectsWith(puck))
            {
                //(((puck.X + (puck.Width / 4)) - (player1.X + (player1.Width / 4))) / 5) * 
                //(((puck.Y + (puck.Height / 4)) - (player1.Y + (player1.Height / 4))) / 5) * 
                puckXSpeed = (player1XSpeed) / 2;
                puckYSpeed = (player1YSpeed) / 2;
                gameStarted = true;
            }
            if (player2.IntersectsWith(puck))
            {
                //(((puck.X + (puck.Width / 4)) - (player2.X + (player2.Width / 4))) / 5) * 
                //(((puck.Y + (puck.Height / 4)) - (player2.Y + (player2.Height / 4))) / 5) * 
                puckXSpeed = (player2XSpeed) / 2;
                puckYSpeed = (player2YSpeed) / 2;
                gameStarted = true;
            }

            // Check for Puck Collisions
            isPuckRight = false;
            if(puck.X > 185)
            {
                isPuckRight = true;
            }
            if(player1.IntersectsWith(puck) || player2.IntersectsWith(puck))
            {
                puckHitPlayer.Play();
            }
            while (player1.IntersectsWith(puck))
            {
                while(player2.IntersectsWith(puck))
                {
                    if(isPuckRight)
                    {
                        puck.X--;
                    }
                    else
                    {
                        puck.X++;
                    }
                    puckYSpeed = 0;
                    puckXSpeed = 0;
                }
                    if (puck.Y + (puck.Height / 2) >= player1.Y + player1.Height / 2)
                    {
                        puck.Y++;
                    }
                    if (puck.Y + (puck.Height / 2) < player1.Y + player1.Height / 2)
                    {
                        puck.Y--;
                    }
                    if (puck.X + (puck.Width / 2) >= player1.X + player1.Width / 2)
                    {
                        puck.X++;
                    }
                    if (puck.X + (puck.Width / 2) < player1.X + player1.Width / 2)
                    {
                        puck.X--;
                    }
            }
            while (player2.IntersectsWith(puck))
            {
                if (puck.Y + (puck.Height / 2) >= player2.Y + player2.Height / 2)
                {
                    puck.Y++;
                }
                if (puck.Y + (puck.Height / 2) < player2.Y + player2.Height / 2)
                {
                    puck.Y--;
                }
                if (puck.X + (puck.Width / 2) >= player2.X + player2.Width / 2)
                {
                    puck.X++;
                }
                if (puck.X + (puck.Width / 2) < player2.X + player2.Width / 2)
                {
                    puck.X--;
                }
            }

            puck.X += Convert.ToInt32(Math.Round(puckXSpeed));
            puck.Y += Convert.ToInt32(Math.Round(puckYSpeed));


            // Check to see if the puck hit the wall
            if (puck.Y < 58 && (puck.X < 104 || puck.X + puck.Width > 296)) {
                puck.Y = 58;
                puckYSpeed *= -0.9;
                puckHitPlayer.Play();
            }
            if (puck.Y > this.Height - puck.Height - 8 && (puck.X < 104 || puck.X + puck.Width > 296))
            {
                puck.Y = this.Height - puck.Height - 8;
                puckYSpeed *= -0.9;
                puckHitPlayer.Play();
            }
            if (puck.X < 8)
            {
                puck.X = 8;
                puckXSpeed *= -0.9;
                puckHitPlayer.Play();
            }
            if (puck.X > this.Width - puck.Width - 8)
            {
                puck.X = this.Width - puck.Width - 8;
                puckXSpeed *= -0.9;
                puckHitPlayer.Play();
            }

            // Check if the puck has been scored in one of the goals
            if((puck.X >= 104 && puck.X + puck.Width <= 296))
            {
                if(puck.Y < 50)
                {
                    p2Points++;
                    puckGoalPlayer.Play();
                    resetGame();
                    scoreLabel.Text = $"{p1Points} - {p2Points}";
                }
                else if(puck.Y > this.Height - puck.Height)
                {
                    p1Points++;
                    puckGoalPlayer.Play();
                    resetGame();
                    scoreLabel.Text = $"{p1Points} - {p2Points}";
                }
            }

            // Move the puck and slow it down a hunch.
            puck.X += Convert.ToInt32(Math.Round(puckXSpeed));
            puck.Y += Convert.ToInt32(Math.Round(puckYSpeed));

            puckXSpeed *= 0.96;
            puckYSpeed *= 0.96;

            // Lower player speeds as needed
            if (player1XSpeed > 0)
            {
                player1XSpeed --;
            }
            if (player1XSpeed < 0)
            {
                player1XSpeed ++;
            }
            if (player1YSpeed > 0)
            {
                player1YSpeed--;
            }
            if (player1YSpeed < 0)
            {
                player2YSpeed++;
            }
            if (player2XSpeed > 0)
            {
                player2XSpeed--;
            }
            if (player2XSpeed < 0)
            {
                player2XSpeed++;
            }
            if (player2YSpeed > 0)
            {
                player2YSpeed--;
            }
            if (player2YSpeed < 0)
            {
                player2YSpeed++;
            }

            // Update the timer if the game is currently going
            if(gameStarted == true)
            {
                timerTick.Enabled = true;

                int mins = Convert.ToInt32(Math.Floor(timer / 60));
                string isZero = (timer % 60) < 10 ? "0" : "";
                timeLabel.Text = $"{mins}:{isZero}{timer % 60}";
            }
            checkForWinner();
            Refresh();
        }

        // Run this 50 times a second.
        private void timerTick_Tick(object sender, EventArgs e)
        {
            timer--;
        }
    }
}
