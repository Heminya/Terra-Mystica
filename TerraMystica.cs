using Microsoft.VisualBasic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace terra
{
    public partial class TerraMystica : Form
    {
        public TerraMystica()
        {
            InitializeComponent();
            GameSetting();
            GameRoundStrat();
        }
        private int playerRound = 1;
        private int playerNum = 3;
        private int passCnt = 0;
        private int gameRoundCnt = 1;
        private int[] bonus = [0, 0, 0, 0, 0, 0];
        private int[] bonusCoin = [0, 0, 0, 0, 0, 0];
        private int[] order;
        private int[] newOrder;
        private int[] score;
        public int[] favorList = [1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3];
        private int[][] bridgeExample = [[14, 39], [14, 28], [3, 28], [17, 28], [30, 41], [30, 55],
            [30, 44], [18, 32], [7, 32], [21, 32], [34, 45], [47, 58], [47, 61], [36, 47],
            [22, 36], [11, 36], [25, 36], [25, 50], [64, 89], [65, 90], [65, 79], [54, 79],
            [68, 79], [81, 106], [81, 95], [69, 83], [58, 83], [85, 96], [98, 109]];
        private int[] scoreCard = [0, 0, 0, 0, 0, 0];
        private int winnerNO = 0;
        private string winner = "";
        private Character[] playerList;
        private Character rounder;
        private Color desert = Color.Gold;              //沙漠
        private Color forest = Color.DarkOliveGreen;    //雨林
        private Color swamp = Color.Gray;               //沼澤
        private Color plains = Color.DarkGoldenrod;     //曠野
        private Color lakes = Color.LightSeaGreen;      //湖泊
        private Color wasteland = Color.IndianRed;      //廢土
        private Color mountains = Color.LightGray;      //山區
        private Color river = Color.DeepSkyBlue;        //河
        #region GAME METHOD

        public void GameSetting()
        {
            order = new int[playerNum];
            newOrder = new int[playerNum];
            score = new int[playerNum];
            for (int i = 0; i < order.Length; i++)
            {
                order[i] = i;
                newOrder[i] = i;
                score[i] = 0;
            }
            playerList = new Character[playerNum];
            playerList[0] = new Nomads(0);
            playerList[1] = new Witches(1);
            playerList[2] = new Alchemists(2);
            rounder = playerList[order[playerRound - 1]];
            Random selectedBonus = new Random();
            for (int i = 0; i < 6; i++)
            {
                bonus[i] = selectedBonus.Next(1, 10);

                for (int j = 0; j < i; j++)
                {
                    while (bonus[j] == bonus[i])
                    {
                        j = 0;
                        bonus[i] = selectedBonus.Next(1, 10);
                    }
                }
            }
            ThreePeopleSettings();
        }
        public void ThreePeopleSettings()
        {
            bonus = [1, 2, 3, 4, 5, 9];
            scoreCard = [2, 5, 7, 3, 6, 4];
            btnMap55.Text = "平房";
            btnMap61.Text = "平房";
            btnMap68.Text = "平房";
            btnMap69.Text = "平房";
            btnMap85.Text = "平房";
            btnMap86.Text = "平房";
            btnMap107.Text = "平房";
        }
        private int NextPlayerRound(int rnd)
        {
            if (passCnt != playerNum)
            {
                do
                {
                    rnd++;
                    if (rnd > playerNum)
                    {
                        rnd = 1;
                    }
                }
                while (playerList[rnd - 1].IsPass());
                rounder = playerList[order[rnd - 1]];
                labRounder.Text = $"現在是{rounder.ShowName()}的行動";
            }


            return rnd;
        }
        private void GameOverCheck()
        {
            if (gameRoundCnt > 6)
            {
                for (int i = 0; i < playerNum; i++)
                {
                    score[i] = playerList[i].ShowScore();
                }
                winnerNO = Array.IndexOf(score, score.Max());
                winner = playerList[winnerNO].ShowName();
                MessageBox.Show($"遊戲結束\nThe Winner is {winner}", "Game Over");
                Application.Exit();
            }
        }
        private void GameRoundStrat()
        {
            playerRound = 1;
            labGameRound.Text = $"第{gameRoundCnt}回合";
            for (int i = 0; i < playerNum; i++)
            {
                order[i] = newOrder[i];
            }
            rounder = playerList[order[0]];
            labRounder.Text = $"現在是{rounder.ShowName()}的行動";
            btnManaShop1.Text = "3魔力->橋\r\nusable:可用";
            btnManaShop1.Enabled = true;
            btnManaShop2.Text = "3魔力->祭司\r\nusable:可用";
            btnManaShop2.Enabled = true;
            btnManaShop3.Text = "4魔力->2工人\r\nusable:可用";
            btnManaShop3.Enabled = true;
            btnManaShop4.Text = "4魔力->7錢\r\nusable:可用";
            btnManaShop4.Enabled = true;
            btnManaShop5.Text = "4魔力->鏟\r\nusable:可用";
            btnManaShop5.Enabled = true;
            btnManaShop6.Text = "6魔力->2鏟\r\nusable:可用";
            btnManaShop6.Enabled = true;
        }
        public void GameRoundOver()
        {
            PutCoin();
        }
        public void PutCoin()
        {
            for (int i = 1; i < 10; i++)
            {
                if (Array.IndexOf(bonus, i) != -1)
                {
                    bonusCoin[Array.IndexOf(bonus, i)] += 1;
                }
            }
        }
        public void ScoreCardRoundEnd()
        {
            for (int i = 0; i < playerNum; i++)
            {
                if (scoreCard[gameRoundCnt - 1] == 1)
                {
                    int getPriests = playerList[i].ShowTowerValue(1) / 4;
                    rounder.AdjustPriests(getPriests);
                }
                if (scoreCard[gameRoundCnt - 1] == 2)
                {
                    int getMana = playerList[i].ShowTowerValue(0) / 4;
                    rounder.GetMana(getMana);
                }
                if (scoreCard[gameRoundCnt - 1] == 3)
                {
                    int getPriests = playerList[i].ShowTowerValue(3) / 4;
                }
                if (scoreCard[gameRoundCnt - 1] == 4)
                {
                    int getPriests = playerList[i].ShowTowerValue(1) / 4;
                }
                if (scoreCard[gameRoundCnt - 1] == 5)
                {
                    int getWorker = playerList[i].ShowTowerValue(3) / 2;
                    rounder.AdjustWorker(getWorker);
                }
                if (scoreCard[gameRoundCnt - 1] == 6)
                {
                    int getWorker = playerList[i].ShowTowerValue(0) / 2;
                    rounder.AdjustWorker(getWorker);
                }
                if (scoreCard[gameRoundCnt - 1] == 7)
                {
                    int getCoin = playerList[i].ShowTowerValue(2) / 1;
                    rounder.AdjustCoin(getCoin);
                }
                if (scoreCard[gameRoundCnt - 1] == 8)
                {
                    int getPriests = playerList[i].ShowTowerValue(2) / 4;
                }
            }
        }
        public void FinalScorePriestsBurning()
        {
            int[] fire = new int[playerNum];
            int[] water = new int[playerNum];
            int[] earth = new int[playerNum];
            int[] air = new int[playerNum];
            for (int i = 0; i < playerNum; i++)
            {
                fire[i] = playerList[i].ShowTowerValue(0);
                water[i] = playerList[i].ShowTowerValue(1);
                earth[i] = playerList[i].ShowTowerValue(2);
                air[i] = playerList[i].ShowTowerValue(3);
            }
        }
        public void SuckMana(Button btn)
        {
            int[] nearArea = IsDirectlyNear(btn);
            for (int i = 0; i < playerNum; i++)
            {
                if (MessageBox.Show("是否吸魔?", "吸魔", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (btn.BackColor != playerList[i].ShowColor())
                    {
                        int getMana = 0;
                        int removeScore = 0;
                        for (int j = 0; j < nearArea.Length; j++)
                        {
                            string btnName = "btnMap";
                            btnName += Convert.ToString(nearArea[j]);
                            Button button = this.Controls[btnName] as Button;
                            if (button.BackColor == playerList[i].ShowColor() && button.Text != "")
                            {
                                if (button.Text == "平房")
                                {
                                    getMana += 1;
                                }
                                else if (button.Text == "大房" || button.Text == "圓房")
                                {
                                    getMana += 2;
                                }
                                else if (button.Text == "要塞" || button.Text == "聖殿")
                                {
                                    getMana += 3;
                                }
                            }
                        }
                        removeScore = getMana;
                        playerList[i].GetMana(getMana);
                        playerList[i].RemoveScore(removeScore);
                        CharacterInfoUpDate(i);
                    }
                }


            }
        }
        private void SourceNotEnough()
        {
            MessageBox.Show("資源不足", "警告");
        }
        private void CharacterInfoUpDate(int playerNO)
        {
            string rtb = "rtbPlayer";
            rtb += Convert.ToString(playerNO + 1);
            if (rtbPlayer1.Name == rtb)
            {
                rtbPlayer1.Text = playerList[playerNO].ShowInfo();
                labPlayer1.Text = playerList[playerNO].ShowName();
            }
            if (rtbPlayer2.Name == rtb)
            {
                rtbPlayer2.Text = playerList[playerNO].ShowInfo();
                labPlayer2.Text = playerList[playerNO].ShowName();
            }
            if (rtbPlayer3.Name == rtb)
            {
                rtbPlayer3.Text = playerList[playerNO].ShowInfo();
                labPlayer3.Text = playerList[playerNO].ShowName();
            }
            if (rtbPlayer4.Name == rtb)
            {
                rtbPlayer4.Text = playerList[playerNO].ShowInfo();
                labPlayer4.Text = playerList[playerNO].ShowName();
            }
            if (rtbPlayer5.Name == rtb)
            {
                rtbPlayer5.Text = playerList[playerNO].ShowInfo();
                labPlayer5.Text = playerList[playerNO].ShowName();
            }
        }
        #endregion

        #region SCORE CARD
        public void ScoreCard1()
        {

        }
        #endregion

        #region BONUS CARD
        public void ChooseBonus()
        {
            using (ButtonChoice selectionForm = new ButtonChoice())
            {
                selectionForm.ChooseBonusCard(bonus, bonusCoin);
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    int returnBonus = rounder.ChooseBonus(selectionForm.SelectedBonusCard, bonusCoin[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)]);
                    bonusCoin[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                    if (returnBonus == 1)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 1;
                    }
                    else if (returnBonus == 2)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 2;
                    }
                    else if (returnBonus == 3)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 3;
                    }
                    else if (returnBonus == 4)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 4;
                    }
                    else if (returnBonus == 5)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 5;
                    }
                    else if (returnBonus == 6)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 6;
                    }
                    else if (returnBonus == 7)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 7;
                    }
                    else if (returnBonus == 8)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 8;
                    }
                    else if (returnBonus == 9)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 9;
                    }
                    CharacterInfoUpDate(rounder.ShowPlayerNO());
                }
                else
                {
                    MessageBox.Show("No instance created.");
                }
            }
        }
        public void ChooseBonus(int playerNO)
        {
            using (ButtonChoice selectionForm = new ButtonChoice())
            {
                selectionForm.ChooseBonusCard(bonus, bonusCoin);
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    int returnBonus = playerList[playerNO].ChooseBonus(selectionForm.SelectedBonusCard, bonusCoin[selectionForm.SelectedBonusCard - 1]);
                    if (returnBonus == 1)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 1;
                    }
                    else if (returnBonus == 2)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 2;
                    }
                    else if (returnBonus == 3)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 3;
                    }
                    else if (returnBonus == 4)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 4;
                    }
                    else if (returnBonus == 5)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 5;
                    }
                    else if (returnBonus == 6)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 6;
                    }
                    else if (returnBonus == 7)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 7;
                    }
                    else if (returnBonus == 8)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 8;
                    }
                    else if (returnBonus == 9)
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                        bonus[Array.IndexOf(bonus, 0)] = 9;
                    }
                    else
                    {
                        bonus[Array.IndexOf(bonus, selectionForm.SelectedBonusCard)] = 0;
                    }
                    CharacterInfoUpDate(rounder.ShowPlayerNO());
                }
                else
                {
                    MessageBox.Show("No instance created.");
                }
            }
        }
        #endregion

        public void SelectBridgeLocation()
        {
            /*for (int i = 1; i <= 113; i++)
            {
                string btnName = "btnMap";
                btnName += Convert.ToString(i);
                Button button = this.Controls[btnName] as Button;
                if (button != null)
                {
                    string clickName = "btnMap";
                    clickName += Convert.ToString(i);
                    clickName += "_Click";
                    Type type = this.GetType();
                    MethodInfo method = type.GetMethod(clickName, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method != null)
                    {
                        EventHandler handler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, method);
                        button.Click -= handler;
                        button.Click += ChooseArea;
                    }
                }
            }*/
            for (int i = 1; i <= 29; i++)
            {
                string pbxName = "pbxBridge";
                pbxName += Convert.ToString(i);
                PictureBox pbx = this.Controls[pbxName] as PictureBox;
                if (pbx != null && pbx.Visible == false)
                {
                    pbx.Visible = true;
                    pbx.Click += PbxClick;
                }
            }
        }

        public void PbxClick(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = rounder.ShowColor();
            string pictureBoxName = pictureBox.Name;
            int bridgeNO = Convert.ToInt32(pictureBoxName.Remove(0, 9));
            int[] bridge = bridgeExample[bridgeNO-1];
            rounder.BuildBridge(bridge);
            for (int i = 1; i <= 29; i++)
            {
                string pbxName = "pbxBridge";
                pbxName += Convert.ToString(i);
                PictureBox pbx = this.Controls[pbxName] as PictureBox;
                if (pbx != null && pbx.BackColor == Color.DarkKhaki)
                {
                    pbx.Visible = false;
                    pbx.Click -= PbxClick;
                }
            }

        }
        
        public int[] IsIndirectlyNear(Button btnA)
        {
            string buttonName = btnA.Name.Remove(0, 6);
            int A = Convert.ToInt32(buttonName);
            int[][] bridges = rounder.ShowBridges();
            int bridgeNum = 0;
            int[] connectArea = [0, 0, 0];
            int[] near;
            for (int i = 0; i < 3; i++)
            {
                if (Array.Exists(bridges[i], element => element == A))
                {
                    if (A == bridges[i][0])
                    {
                        connectArea[bridgeNum] = bridges[i][1];
                    }
                    else if (A == bridges[i][1])
                    {
                        connectArea[bridgeNum] = bridges[i][0];
                    }
                    bridgeNum++;
                }
            }
            if (A % 25 != 13 && A % 25 != 1 && A % 25 != 14 && A % 25 != 0)
            {
                if (A > 13 && A < 101)
                {
                    near = new int[6 + bridgeNum];
                    near[0] = A - 13; near[1] = A - 12; near[3] = A - 1; near[4] = A + 1; near[5] = A + 12; near[6] = A + 13;
                    for (int i = 0; i < bridgeNum; i++)
                    {
                        near[6 + i] = connectArea[i];
                    }
                }
                else if (A > 101 && A < 113)
                {
                    near = new int[4 + bridgeNum];
                    near = [A - 13, A - 12, A - 1, A + 1];
                    for (int i = 0; i < bridgeNum; i++)
                    {
                        near[4 + i] = connectArea[i];
                    }
                }
                else
                {
                    near = new int[4 + bridgeNum];
                    near = [A - 1, A + 1, A + 12, A + 13];
                    for (int i = 0; i < bridgeNum; i++)
                    {
                        near[4 + i] = connectArea[i];
                    }
                }
            }
            else if (A % 25 == 13)
            {
                if (A == 13)
                {
                    return [12, 25];
                }
                else if (A == 113)
                {
                    return [100, 112];
                }
                else
                {
                    near = new int[3];
                    near = [A - 13, A - 1, A + 12];
                }
            }
            else if (A % 25 == 1)
            {
                if (A == 1)
                {
                    return [2, 14];
                }
                else if (A == 101)
                {
                    return [89, 102];
                }
                else
                {
                    near = new int[3];
                    near = [A - 12, A + 1, A + 13];
                }
            }
            else if (A % 25 == 0)
            {
                near = new int[5];
                near = [A - 13, A - 12, A + 1, A + 12, A + 13];
            }
            else
            {
                near = new int[5 + bridgeNum];
                near = [A - 13, A - 12, A - 1, A + 12, A + 13];
                for (int i = 0; i < bridgeNum; i++)
                {
                    near[5 + i] = connectArea[i];
                }
            }
            return near;

        }
        public int[] IsDirectlyNear(Button btnA)
        {
            string buttonName = btnA.Name.Remove(0, 6);
            int A = Convert.ToInt32(buttonName);
            int[][] bridges = rounder.ShowBridges();
            int bridgeNum = 0;
            int[] connectArea = [0, 0, 0];
            int[] near;
            for (int i = 0; i < 3; i++)
            {
                if (Array.Exists(bridges[i], element => element == A))
                {
                    if (A == bridges[i][0])
                    {
                        connectArea[bridgeNum] = bridges[i][1];
                    }
                    else if (A == bridges[i][1])
                    {
                        connectArea[bridgeNum] = bridges[i][0];
                    }
                    bridgeNum++;
                }
            }
            if (A % 25 != 13 && A % 25 != 1 && A % 25 != 14 && A % 25 != 0)
            {
                if (A > 13 && A < 101)
                {
                    near = new int[6 + bridgeNum];
                    near[0] = A - 13; near[1] = A - 12; near[3] = A - 1; near[4] = A + 1; near[5] = A + 12; near[6] = A + 13;
                    for (int i = 0; i < bridgeNum; i++)
                    {
                        near[6 + i] = connectArea[i];
                    }
                }
                else if (A > 101 && A < 113)
                {
                    near = new int[4 + bridgeNum];
                    near = [A - 13, A - 12, A - 1, A + 1];
                    for (int i = 0; i < bridgeNum; i++)
                    {
                        near[4 + i] = connectArea[i];
                    }
                }
                else
                {
                    near = new int[4 + bridgeNum];
                    near = [A - 1, A + 1, A + 12, A + 13];
                    for (int i = 0; i < bridgeNum; i++)
                    {
                        near[4 + i] = connectArea[i];
                    }
                }
            }
            else if (A % 25 == 13)
            {
                if (A == 13)
                {
                    return [12, 25];
                }
                else if (A == 113)
                {
                    return [100, 112];
                }
                else
                {
                    near = new int[3];
                    near = [A - 13, A - 1, A + 12];
                }
            }
            else if (A % 25 == 1)
            {
                if (A == 1)
                {
                    return [2, 14];
                }
                else if (A == 101)
                {
                    return [89, 102];
                }
                else
                {
                    near = new int[3];
                    near = [A - 12, A + 1, A + 13];
                }
            }
            else if (A % 25 == 0)
            {
                near = new int[5];
                near = [A - 13, A - 12, A + 1, A + 12, A + 13];
            }
            else
            {
                near = new int[5 + bridgeNum];
                near = [A - 13, A - 12, A - 1, A + 12, A + 13];
                for (int i = 0; i < bridgeNum; i++)
                {
                    near[5 + i] = connectArea[i];
                }
            }
            return near;

        }

        #region OCUPPY
        private bool OccupyCheck(int[] near, Character chr)
        {
            for (int i = 0; i < near.Length; i++)
            {
                string btnName = "btnMap";
                btnName += Convert.ToString(near[i]);
                Button button = this.Controls[btnName] as Button;
                if (button != null)
                {
                    if (button.Text != "" && button.BackColor == chr.ShowColor())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        private void NomadsOccupied(Button btn)
        {
            btn.BackColor = desert;
        }
        private void WitchesOccupied(Button btn)
        {
            btn.BackColor = forest;
        }
        private void AlchemistsOccupied(Button btn)
        {
            btn.BackColor = swamp;
        }
        private bool OccupySort(Button btn, Character chr)
        {
            bool check = false;
            if (btn.BackColor == desert)
            {
                check = chr.OccupyDesert();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else if (btn.BackColor == forest)
            {
                check = chr.OccupyForest();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else if (btn.BackColor == swamp)
            {
                check = chr.OccupySwamp();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else if (btn.BackColor == plains)
            {
                check = chr.OccupyPlains();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else if (btn.BackColor == lakes)
            {
                check = chr.OccupyLakes();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else if (btn.BackColor == wasteland)
            {
                check = chr.OccupyWasteland();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else if (btn.BackColor == mountains)
            {
                check = chr.OccupyMountains();
                if (check)
                {
                    return true;
                }
                else
                {
                    SourceNotEnough();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void Occupy(Button btn)
        {

            bool occupyCheck = OccupyCheck(IsIndirectlyNear(btn), rounder);
            if (occupyCheck)
            {
                occupyCheck = OccupySort(btn, rounder);
                if (rounder.ShowName() == "遊牧民族")
                {
                    if (occupyCheck)
                    {
                        NomadsOccupied(btn);
                        if (MessageBox.Show("產地同時蓋房?", "蓋房", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bool levelUpCheck = rounder.DwellingsLevelUp();

                            if (levelUpCheck)
                            {
                                btn.Text = "平房";
                            }
                            else if (!levelUpCheck)
                            {
                                SourceNotEnough();
                            }
                        }
                    }
                }
                if (rounder.ShowName() == "女巫")
                {
                    if (occupyCheck)
                    {

                        WitchesOccupied(btn);
                        if (MessageBox.Show("產地同時蓋房?", "蓋房", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bool levelUpCheck = rounder.DwellingsLevelUp();

                            if (levelUpCheck)
                            {
                                btn.Text = "平房";
                            }
                            else if (!levelUpCheck)
                            {
                                SourceNotEnough();
                            }
                        }
                    }
                }
                if (rounder.ShowName() == "魔族")
                {
                    if (occupyCheck)
                    {
                        AlchemistsOccupied(btn);
                        if (MessageBox.Show("產地同時蓋房?", "蓋房", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            DwellingsLevelUp(btn);
                        }
                    }

                }
                CharacterInfoUpDate(order[playerRound - 1]);
            }
            else
            {
                MessageBox.Show("僅能在已有建築物旁鏟地", "警告");
            }



        }
        #endregion

        #region BUILDINGS LEVELUP
        public void DwellingsLevelUp(Button btn)
        {
            bool dwellingsLevelUpCheck = rounder.DwellingsLevelUp();
            if (dwellingsLevelUpCheck)
            {
                SuckMana(btn);
                btn.Text = "平房";
                if (scoreCard[gameRoundCnt - 1] == 1 || scoreCard[gameRoundCnt - 1] == 2)
                {
                    rounder.AddScore(2);
                }
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        public void TradingHouseLevelUp(Button btn)
        {
            bool tradingHouseLevelUpCheck = rounder.TradingHouseLevelUp();
            if (tradingHouseLevelUpCheck)
            {
                SuckMana(btn);
                btn.Text = "大房";
                if (scoreCard[gameRoundCnt - 1] == 3 || scoreCard[gameRoundCnt - 1] == 4)
                {
                    rounder.AddScore(3);
                }
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        public void StrongholdLevelUp(Button btn)
        {
            bool strongholdLevelUpCheck = rounder.StrongholdLevelUp();
            if (strongholdLevelUpCheck)
            {
                SuckMana(btn);
                btn.Text = "要塞";
                if (scoreCard[gameRoundCnt - 1] == 5 || scoreCard[gameRoundCnt - 1] == 6)
                {
                    rounder.AddScore(5);
                }
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        public void TemplesLevelUp(Button btn)
        {
            bool templesLevelUpCheck = rounder.TemplesLevelUp();
            if (templesLevelUpCheck)
            {
                SuckMana(btn);
                btn.Text = "圓房";
                using (ButtonChoice selectionForm = new ButtonChoice())
                {
                    selectionForm.ChooseFavor(favorList, rounder);
                    if (selectionForm.ShowDialog() == DialogResult.OK)
                    {
                        rounder.ChooseFavor(selectionForm.selectedFavor);
                        FavorMove(selectionForm.selectedFavor);
                    }
                }
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        public void SanctuaryLevelUp(Button btn)
        {
            bool sanctuaryLevelUpCheck = rounder.SanctuaryLevelUp();
            if (sanctuaryLevelUpCheck)
            {
                SuckMana(btn);
                btn.Text = "聖殿";
                using (ButtonChoice selectionForm = new ButtonChoice())
                {
                    selectionForm.ChooseFavor(favorList, rounder);
                    if (selectionForm.ShowDialog() == DialogResult.OK)
                    {
                        rounder.ChooseFavor(selectionForm.selectedFavor);
                        FavorMove(selectionForm.selectedFavor);
                    }
                }
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        #endregion

        #region MAP BUTTON
        private void ButtonMapClick(Button btn, Character chr)
        {
            if (btn.Text == "")
            {
                Occupy(btn);
                CharacterInfoUpDate(chr.ShowPlayerNO());
            }
            else if (btn.BackColor == chr.ShowColor())
            {
                if (btn.Text == "平房")
                {
                    TradingHouseLevelUp(btn);
                }
                else if (btn.Text == "大房")
                {
                    using (ButtonChoice selectionForm = new ButtonChoice())
                    {
                        selectionForm.ChooseLevelUp();
                        if (selectionForm.ShowDialog() == DialogResult.OK)
                        {
                            if (selectionForm.SelectedLevelUp == selectionForm.strongholdLevelUp)
                            {
                                StrongholdLevelUp(btn);
                            }
                            else if (selectionForm.SelectedLevelUp == selectionForm.templesLevelUp)
                            {
                                TemplesLevelUp(btn);
                            }
                            CharacterInfoUpDate(chr.ShowPlayerNO());

                        }
                        else
                        {
                            MessageBox.Show("No instance created.");
                        }
                    }
                }
                else if (btn.Text == "圓房")
                {
                    SanctuaryLevelUp(btn);
                }
                CharacterInfoUpDate(chr.ShowPlayerNO());
            }
            else
            {
                MessageBox.Show("這個板塊已經屬於其他玩家!", "警告");
            }
        }
        private void btnMap1_Click(object sender, EventArgs e)
        {

            ButtonMapClick(btnMap1, rounder);
        }

        private void btnMap2_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap2, rounder);
        }
        private void btnMap3_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap3, rounder);
        }
        private void btnMap4_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap4, rounder);
        }
        private void btnMap5_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap5, rounder);
        }
        private void btnMap6_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap6, rounder);
        }
        private void btnMap7_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap7, rounder);
        }
        private void btnMap8_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap8, rounder);
        }
        private void btnMap9_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap9, rounder);
        }
        private void btnMap10_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap10, rounder);
        }
        private void btnMap11_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap11, rounder);
        }
        private void btnMap12_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap12, rounder);
        }
        private void btnMap13_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap13, rounder);
        }
        private void btnMap14_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap14, rounder);
        }
        private void btnMap17_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap17, rounder);
        }
        private void btnMap18_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap18, rounder);
        }
        private void btnMap21_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap21, rounder);
        }
        private void btnMap22_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap22, rounder);
        }
        private void btnMap25_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap25, rounder);
        }
        private void btnMap28_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap28, rounder);
        }
        private void btnMap30_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap30, rounder);
        }
        private void btnMap32_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap32, rounder);
        }
        private void btnMap34_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap34, rounder);
        }
        private void btnMap36_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap36, rounder);
        }
        private void btnMap39_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap39, rounder);
        }
        private void btnMap40_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap40, rounder);
        }
        private void btnMap41_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap41, rounder);
        }
        private void btnMap44_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap44, rounder);
        }
        private void btnMap45_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap45, rounder);
        }
        private void btnMap47_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap47, rounder);
        }
        private void btnMap49_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap49, rounder);
        }
        private void btnMap50_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap50, rounder);
        }
        private void btnMap51_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap51, rounder);
        }
        private void btnMap52_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap52, rounder);
        }
        private void btnMap53_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap53, rounder);
        }
        private void btnMap54_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap54, rounder);
        }
        private void btnMap55_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap55, rounder);
        }
        private void btnMap56_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap56, rounder);
        }
        private void btnMap57_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap57, rounder);
        }
        private void btnMap58_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap58, rounder);
        }
        private void btnMap61_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap61, rounder);
        }
        private void btnMap62_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap62, rounder);
        }
        private void btnMap63_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap63, rounder);
        }
        private void btnMap64_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap64, rounder);
        }
        private void btnMap65_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap65, rounder);
        }
        private void btnMap68_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap68, rounder);
        }
        private void btnMap69_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap69, rounder);
        }
        private void btnMap73_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap73, rounder);
        }
        private void btnMap74_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap74, rounder);
        }
        private void btnMap75_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap75, rounder);
        }
        private void btnMap79_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap79, rounder);
        }
        private void btnMap81_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap81, rounder);
        }
        private void btnMap83_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap83, rounder);
        }
        private void btnMap85_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap85, rounder);
        }
        private void btnMap86_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap86, rounder);
        }
        private void btnMap87_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap87, rounder);
        }
        private void btnMap88_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap88, rounder);
        }
        private void btnMap89_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap89, rounder);
        }
        private void btnMap90_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap90, rounder);
        }
        private void btnMap91_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap91, rounder);
        }
        private void btnMap95_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap95, rounder);
        }
        private void btnMap96_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap96, rounder);
        }
        private void btnMap98_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap98, rounder);
        }
        private void btnMap99_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap99, rounder);
        }
        private void btnMap100_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap100, rounder);
        }
        private void btnMap101_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap101, rounder);
        }
        private void btnMap102_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap102, rounder);
        }
        private void btnMap103_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap103, rounder);
        }
        private void btnMap104_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap104, rounder);
        }
        private void btnMap105_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap105, rounder);
        }
        private void btnMap106_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap106, rounder);
        }
        private void btnMap107_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap107, rounder);
        }
        private void btnMap108_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap108, rounder);
        }
        private void btnMap109_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap109, rounder);
        }
        private void btnMap111_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap111, rounder);
        }
        private void btnMap112_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap112, rounder);
        }
        private void btnMap113_Click(object sender, EventArgs e)
        {
            ButtonMapClick(btnMap113, rounder);
        }
        #endregion

        #region OTHER BUTTON
        private void btnReset_Click(object sender, EventArgs e)
        {
            gameRoundCnt = 1;
            using (ButtonChoice selectionForm = new ButtonChoice())
            {
                selectionForm.CreateCharacter();
                for (int i = 0; i < playerNum; i++)
                {
                    if (selectionForm.ShowDialog() == DialogResult.OK)
                    {
                        playerList[i] = (Character)Activator.CreateInstance(selectionForm.SelectedType, i);
                        playerList[i].ShowMessage();
                        CharacterInfoUpDate(i);
                        if (i == 0)
                        {
                            ptbPlayer1Fire.BackColor = playerList[i].ShowColor();
                            ptbPlayer1Water.BackColor = playerList[i].ShowColor();
                            ptbPlayer1Earth.BackColor = playerList[i].ShowColor();
                            ptbPlayer1Air.BackColor = playerList[i].ShowColor();
                        }
                        if (i == 1)
                        {
                            ptbPlayer2Fire.BackColor = playerList[i].ShowColor();
                            ptbPlayer2Water.BackColor = playerList[i].ShowColor();
                            ptbPlayer2Earth.BackColor = playerList[i].ShowColor();
                            ptbPlayer2Air.BackColor = playerList[i].ShowColor();
                        }
                        if (i == 2)
                        {
                            ptbPlayer3Fire.BackColor = playerList[i].ShowColor();
                            ptbPlayer3Water.BackColor = playerList[i].ShowColor();
                            ptbPlayer3Earth.BackColor = playerList[i].ShowColor();
                            ptbPlayer3Air.BackColor = playerList[i].ShowColor();
                        }
                        if (i == 3)
                        {
                            ptbPlayer4Fire.BackColor = playerList[i].ShowColor();
                            ptbPlayer4Water.BackColor = playerList[i].ShowColor();
                            ptbPlayer4Earth.BackColor = playerList[i].ShowColor();
                            ptbPlayer4Air.BackColor = playerList[i].ShowColor();
                        }
                        if (i == 4)
                        {
                            ptbPlayer5Fire.BackColor = playerList[i].ShowColor();
                            ptbPlayer5Water.BackColor = playerList[i].ShowColor();
                            ptbPlayer5Earth.BackColor = playerList[i].ShowColor();
                            ptbPlayer5Air.BackColor = playerList[i].ShowColor();
                        }

                    }
                    else
                    {
                        MessageBox.Show("No instance created.");
                    }
                }
            }
            rounder = playerList[order[playerRound - 1]];
            for (int i = 0; i < playerNum; i++)
            {
                ChooseBonus(playerNum - (i + 1));
                playerList[playerNum - (i + 1)].RoundStart();
                CharacterInfoUpDate(playerNum - (i + 1));
            }
            rounder.RoundStart();
            labRounder.Text = $"現在是{rounder.ShowName()}的行動";
            //string inputBox = "";
            //inputBox = Interaction.InputBox("共有幾位玩家?", "玩家人數", "", 100, 300);
            //player = Convert.ToInt32(inputBox);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            playerRound = NextPlayerRound(playerRound);
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            newOrder[passCnt] = rounder.ShowPlayerNO();
            passCnt += 1;
            rounder.RoundOver();
            ChooseBonus();
            rounder.RoundStart();
            CharacterInfoUpDate(rounder.ShowPlayerNO());
            playerList[playerRound - 1].Pass();
            playerRound = NextPlayerRound(playerRound);
            if (passCnt == playerNum)
            {
                gameRoundCnt++;
                passCnt = 0;
                GameOverCheck();
                for (int i = 0; i < playerNum; i++)
                {
                    playerList[i].ResetPass();
                }
                GameRoundOver();
                GameRoundStrat();
            }
        }

        #endregion

        #region MANA SHOP
        private void btnManaShop1_Click(object sender, EventArgs e)
        {
            bool checkManaShop = rounder.UseManaShop1();
            if (checkManaShop)
            {
                SelectBridgeLocation();
                btnManaShop1.Text = "3魔力->橋\r\nusable:已使用";
                btnManaShop1.Enabled = false;
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnManaShop2_Click(object sender, EventArgs e)
        {
            bool checkManaShop = rounder.UseManaShop2();
            if (checkManaShop)
            {
                btnManaShop2.Text = "3魔力->祭司\r\nusable:已使用";
                btnManaShop2.Enabled = false;
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnManaShop3_Click(object sender, EventArgs e)
        {
            bool checkManaShop = rounder.UseManaShop3();
            if (checkManaShop)
            {
                btnManaShop3.Text = "4魔力->2工人\r\nusable:已使用";
                btnManaShop3.Enabled = false;
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnManaShop4_Click(object sender, EventArgs e)
        {
            bool checkManaShop = rounder.UseManaShop4();
            if (checkManaShop)
            {
                btnManaShop4.Text = "4魔力->7錢\r\nusable:已使用";
                btnManaShop4.Enabled = false;
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnManaShop5_Click(object sender, EventArgs e)
        {
            bool checkManaShop = rounder.UseManaShop5();
            if (checkManaShop)
            {
                btnManaShop5.Text = "4魔力->鏟\r\nusable:已使用";
                btnManaShop5.Enabled = false;
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnManaShop6_Click(object sender, EventArgs e)
        {
            bool checkManaShop = rounder.UseManaShop6();
            if (checkManaShop)
            {
                btnManaShop6.Text = "6魔力->2鏟\r\nusable:已使用";
                btnManaShop6.Enabled = false;
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        #endregion

        #region SKILL LEVELUP
        private void btnWorkerLevelUp_Click(object sender, EventArgs e)
        {
            bool workingLevelUpCheck = rounder.WorkingLevelUp();
            if (workingLevelUpCheck)
            {
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnShippingLevelUp_Click(object sender, EventArgs e)
        {
            bool shippingLevelUpCheck = rounder.ShippingLevelUp();
            if (shippingLevelUpCheck)
            {
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        #endregion

        #region BURN PRIESTS BUTTON
        private void BurnTextUpdate(Button bnt, Character chr)
        {
            bnt.Text += ":";
            bnt.Text += chr.ShowName();
            bnt.Enabled = false;
        }
        public bool TopCheck(int burnType)
        {
            for (int i = 0; i < playerNum; i++)
            {
                if (playerList[i].ReachTop(burnType))
                {
                    return false;
                }
            }
            return true;
        }
        private void MoveItem(int burnType, Character chr, int burnValue, Button btn)
        {
            bool top = TopCheck(burnType);
            bool burnPriestsCheck = chr.BurnPriests(burnType, burnValue, top);
            if (burnPriestsCheck || btn == null)
            {
                CharacterInfoUpDate(chr.ShowPlayerNO());
                if (burnType == 0)
                {
                    string ptbName = $"ptbPlayer{chr.ShowPlayerNO() + 1}Fire";
                    PictureBox ptb = this.Controls[ptbName] as PictureBox;
                    ptb.Location = new System.Drawing.Point(ptb.Location.X, ptb.Location.Y - 19 * burnValue);
                }
                else if (burnType == 1)
                {
                    string ptbName = $"ptbPlayer{chr.ShowPlayerNO() + 1}Water";
                    PictureBox ptb = this.Controls[ptbName] as PictureBox;
                    ptb.Location = new System.Drawing.Point(ptb.Location.X, ptb.Location.Y - 19 * burnValue);
                }
                else if (burnType == 2)
                {
                    string ptbName = $"ptbPlayer{chr.ShowPlayerNO() + 1}Earth";
                    PictureBox ptb = this.Controls[ptbName] as PictureBox;
                    ptb.Location = new System.Drawing.Point(ptb.Location.X, ptb.Location.Y - 19 * burnValue);
                }
                else if (burnType == 3)
                {
                    string ptbName = $"ptbPlayer{chr.ShowPlayerNO() + 1}Air";
                    PictureBox ptb = this.Controls[ptbName] as PictureBox;
                    ptb.Location = new System.Drawing.Point(ptb.Location.X, ptb.Location.Y - 19 * burnValue);
                }
                if (btn != null)
                {
                    BurnTextUpdate(btn, chr);
                }
            }
            else if (btn != null)
            {
                SourceNotEnough();
            }
        }
        public void FavorMove(int favorNO)
        {
            if (favorNO == 0)
            {
                MoveItem(0, rounder, 3, null);
            }
            else if (favorNO == 1)
            {
                MoveItem(1, rounder, 3, null);
            }
            else if (favorNO == 2)
            {
                MoveItem(2, rounder, 3, null);
            }
            else if (favorNO == 3)
            {
                MoveItem(3, rounder, 3, null);
            }
            else if (favorNO == 4)
            {
                MoveItem(0, rounder, 2, null);
            }
            else if (favorNO == 5)
            {
                MoveItem(1, rounder, 2, null);
            }
            else if (favorNO == 6)
            {
                MoveItem(3, rounder, 2, null);
            }
            else if (favorNO == 7)
            {
                MoveItem(2, rounder, 2, null);
            }
            else if (favorNO == 8)
            {
                MoveItem(0, rounder, 1, null);
            }
            else if (favorNO == 9)
            {
                MoveItem(1, rounder, 1, null);
            }
            else if (favorNO == 10)
            {
                MoveItem(3, rounder, 1, null);
            }
            else if (favorNO == 11)
            {
                MoveItem(2, rounder, 1, null);
            }
        }
        private void btnBurnFire1_Click(object sender, EventArgs e)
        {
            MoveItem(0, rounder, 3, btnBurnFire1);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnFire2_Click(object sender, EventArgs e)
        {
            MoveItem(0, rounder, 2, btnBurnFire2);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnFire3_Click(object sender, EventArgs e)
        {
            MoveItem(0, rounder, 2, btnBurnFire3);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnFire4_Click(object sender, EventArgs e)
        {
            MoveItem(0, rounder, 2, btnBurnFire4);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnWater1_Click(object sender, EventArgs e)
        {
            MoveItem(1, rounder, 3, btnBurnWater1);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnWater2_Click(object sender, EventArgs e)
        {
            MoveItem(1, rounder, 2, btnBurnWater2);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnWater3_Click(object sender, EventArgs e)
        {
            MoveItem(1, rounder, 2, btnBurnWater3);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnWater4_Click(object sender, EventArgs e)
        {
            MoveItem(1, rounder, 2, btnBurnWater4);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnEarth4_Click(object sender, EventArgs e)
        {
            MoveItem(2, rounder, 2, btnBurnEarth4);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnEarth3_Click(object sender, EventArgs e)
        {
            MoveItem(2, rounder, 2, btnBurnEarth3);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnEarth2_Click(object sender, EventArgs e)
        {
            MoveItem(2, rounder, 2, btnBurnEarth2);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnEarth1_Click(object sender, EventArgs e)
        {
            MoveItem(2, rounder, 3, btnBurnEarth1);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnAir4_Click(object sender, EventArgs e)
        {
            MoveItem(3, rounder, 2, btnBurnAir4);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnAir3_Click(object sender, EventArgs e)
        {
            MoveItem(3, rounder, 2, btnBurnAir3);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnAir2_Click(object sender, EventArgs e)
        {
            MoveItem(3, rounder, 2, btnBurnAir2);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }

        private void btnBurnAir1_Click(object sender, EventArgs e)
        {
            MoveItem(3, rounder, 3, btnBurnAir1);
            CharacterInfoUpDate(rounder.ShowPlayerNO());
        }
        #endregion

        #region MANA CHANGE
        private void btnBurnMana_Click(object sender, EventArgs e)
        {
            if (rounder.BurnMana(1))
            {
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        private void btnChangeMana1_Click(object sender, EventArgs e)
        {
            if (rounder.ManaChange1())
            {
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnChangeMana2_Click(object sender, EventArgs e)
        {
            if (rounder.ManaChange2())
            {
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }

        private void btnChangeMana3_Click(object sender, EventArgs e)
        {
            if (rounder.ManaChange3())
            {
                CharacterInfoUpDate(rounder.ShowPlayerNO());
            }
            else
            {
                SourceNotEnough();
            }
        }
        #endregion

        #region CASTLE
        public void StoreCastleValue(Button btn, int value)
        {
            if (btn.Tag != null)
            {
                btn.Tag = value;
            }
            else
            {
                btn.Tag = value;
            }
        }
        #endregion
    }
}
