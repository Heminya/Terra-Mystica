using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terra
{
    internal class Character
    {
        #region DATA
        protected string name = "";
        protected int playerNO = 0;
        protected int score = 0;            //分數
        protected int dwellings = 0;        //平房
        protected int maxDwellings = 0;     //平房上限
        protected int tradingHouse = 0;     //大房
        protected int maxTradingHouse = 0;  //大房上限
        protected int stronghold = 0;       //要塞
        protected int maxStronghold = 0;    //要塞上限
        protected int temples = 0;          //圓房
        protected int maxTemples = 0;       //圓房上限
        protected int sanctuary = 0;        //聖殿
        protected int maxSanctuary = 0;     //聖殿上限
        protected int priests = 0;          //祭司
        protected int maxPriests = 0;       //祭司上限
        protected int coin = 0;             //錢
        protected int worker = 0;           //工人
        protected int key = 0;              //鑰匙
        protected int workingLevel = 0;     //鏟地等級
        protected int shippingLevel = 0;    //航海等級
        protected int firstMana = 0;        //一區魔力
        protected int secondMana = 0;       //二區魔力
        protected int thirdMana = 0;        //三區魔力
        protected int bonus = 0;            //海克斯
        protected int[] favor;              //神恩
        protected int[][] bridges;          //橋
        protected int bridgeNum = 0;        //橋數量
        protected bool pass = false;        //PASS
        protected Color color = Color.White; 
        protected int[] occupyingTable = [0, 0, 0, 0, 0, 0, 0]; //鏟地表(0:沙漠/1:雨林/2:沼澤/3:曠野/4:湖泊/5:廢土/6:山區)
        protected int[] priestsBurning = [0, 0, 0, 0];          //燒祭司(0:火/1:水/2:土/3:風)
        #endregion
        public int ShowPlayerNO()
        {
            return playerNO;
        }
        #region METHOD
        public Character(int playerNO)
        {
            this.playerNO = playerNO;
            score = 20;
            dwellings = 6;
            tradingHouse = 4;
            stronghold = 1;     
            temples = 3;        
            sanctuary = 1;      
            priests = 0;
            maxPriests = 7;
            coin = 0;           
            worker = 0;
            bridgeNum = 0;
            workingLevel = 1;   
            shippingLevel = 0;  
            firstMana = 5;      
            secondMana = 7;     
            thirdMana = 0;
            occupyingTable = [0, 0, 0, 0, 0, 0, 0];  
            priestsBurning = [0, 0, 0, 0];
            favor = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            bridges = [[0, 0], [0, 0], [0, 0]];
            StartingSet();
        }
        public void RoundStart()
        {
            int getWorker = 0;
            int getCoin = 0;
            int getMana = 0;
            if (dwellings != 0)
            {
                getWorker += 9 - dwellings;
            }
            else
            {
                getWorker += 8;
            }
            getCoin += 8 - 2 * tradingHouse;
            getMana += 4 - tradingHouse;
            if (favor[6]==1)
            {
                getMana += 4;
            }
            if (favor[7] == 1)
            {
                getMana += 1;
                getWorker += 1;
            }
            if (favor[8] == 1)
            {
                getCoin += 3;
            }

            worker += getWorker;
            coin += getCoin;
            GetMana(getMana);
        }
        public void RoundOver()
        {            
            int getScore = 0;            
            if(bonus==7)
            {
                getScore += (10 - dwellings);
            }
            else if(bonus==8)
            {
                getScore += 2 * (4 - tradingHouse);
            }
            else if (bonus==9)
            {
                getScore += 4 * (2 - stronghold - sanctuary);
            }
            if (favor[10]==1)
            {
                if(tradingHouse==3)
                {
                    getScore += 2;
                }
                else if(tradingHouse == 2)
                {
                    getScore += 3;
                }
                else if (tradingHouse == 1)
                {
                    getScore += 3;
                }
                else if (tradingHouse == 0)
                {
                    getScore += 4;
                }
            }
            score += getScore;
        }
        public int ChooseBonus(int thisRoundBonus,int coin)
        {
            int lastBonus = bonus;
            bonus = thisRoundBonus;
            this.coin += coin;
            return lastBonus;
        }
        public void ChooseFavor(int selectedFavor)
        {
            favor[selectedFavor] = 1;
        }
        public int[] ShowFavor()
        {
            return favor;
        }
        #endregion
        #region BONUS
        public void Bonus1()
        {
            priests += 1;
        }
        public void Bonus2()
        {
            worker += 1;
            GetMana(3);
        }
        public void Bonus3()
        {
            coin += 6;
        }
        public void Bonus4()
        {
            GetMana(3);
        }
        public void Bonus5()
        {
            coin += 2;
        }
        public void Bonus6()
        {
            coin += 4;
        }
        public void Bonus7()
        {
            coin += 2;
        }
        public void Bonus8()
        {
            worker += 1;
        }
        public void Bonus9()
        {
            worker += 2;
        }
        public void Bonus()
        {
            if(bonus == 1)
            {
                Bonus1();
            }
            if (bonus == 2)
            {
                Bonus2();
            }
            if (bonus == 3)
            {
                Bonus3();
            }
            if (bonus == 4)
            {
                Bonus4();
            }
            if (bonus == 5)
            {
                Bonus5();
            }
            if (bonus == 6)
            {
                Bonus6();
            }
            if (bonus == 7)
            {
                Bonus7();
            }
            if (bonus == 8)
            {
                Bonus8();
            }
            if (bonus == 9)
            {
                Bonus9();
            }
        }
        #endregion

        #region SKILL LEVELUP
        public bool WorkingLevelUp()
        {
            if (worker >= 2 && coin >= 5 && priests >= 1 && workingLevel < 3)
            {
                worker -= 2;
                coin -= 5;
                priests -= 1;
                workingLevel += 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ShippingLevelUp() 
        {
            if (coin >= 4 && priests >= 1 && shippingLevel < 4)
            {
                coin -= 4;
                priests -= 1;
                shippingLevel += 1;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        #endregion
        public int ShowScore()
        {
            return score;
        }
        public bool BurnPriests(int burnType, int burnValue, bool top)
        {
            if (priests > 0)
            {
                priests--;
                int originValue = priestsBurning[burnType];
                priestsBurning[burnType] += burnValue;
                if (originValue < 3 && priestsBurning[burnType] >= 3)
                {
                    GetMana(1);
                }
                if (originValue < 5 && priestsBurning[burnType] >= 5)
                {
                    GetMana(2);
                }
                if (originValue < 7 && priestsBurning[burnType] >= 7)
                {
                    GetMana(2);
                }
                if (priestsBurning[burnType]>=10)
                {
                    if (top && key > 0)
                    {
                        priestsBurning[burnType] = 10;
                        GetMana(3);
                        key--;
                    }
                    else
                    {
                        priestsBurning[burnType] = 9;
                    }
                }
                maxPriests--;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ReachTop(int burnType)
        {
            if (priestsBurning[burnType] == 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region BUILDINGS LEVELUP
        public bool DwellingsLevelUp()
        {
            if (worker >= 1 && coin >= 2 && dwellings >= 1)
            {
                worker -= 1;
                coin -= 2;
                dwellings -= 1;
                if (favor[11] == 1)
                {
                    score += 2;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool TradingHouseLevelUp()
        {
            if (worker >= 2 && coin >= 3 && tradingHouse >= 1)
            {
                worker -= 2;
                coin -= 3;
                dwellings += 1;
                tradingHouse -= 1;
                if (favor[9] == 1)
                {
                    score += 3;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool StrongholdLevelUp()
        {
            if (worker >= 4 && coin >= 6 && stronghold >= 1)
            {
                worker -= 4;
                coin -= 6;
                tradingHouse += 1;
                stronghold -= 1;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool TemplesLevelUp()
        {
            if (worker >= 2 && coin >= 5 && temples >= 1)
            {
                worker -= 2;
                coin -= 5;
                tradingHouse += 1;
                temples -= 1;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool SanctuaryLevelUp()
        {
            if (worker >= 4 && coin >= 6 && sanctuary >= 1)
            {
                worker -= 4;
                coin -= 6;
                temples += 1;
                sanctuary -= 1;
                return true;
            }
            else
            {
                return false;
            }
            
        }
        #endregion
        public void BuildBridge(int[] newBridge)
        {
            bridges[bridgeNum] = newBridge;
            bridgeNum++;
        }
        #region MANA SHOP
        public bool UseManaShop1()
        {
            if (thirdMana >= 3 && bridgeNum<3)
            {
                thirdMana -= 3;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UseManaShop2()
        {
            if (thirdMana >= 3 && (maxPriests - priests) > 1)
            {
                thirdMana -= 3;
                priests += 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UseManaShop3()
        {
            if (thirdMana >= 4)
            {
                thirdMana -= 4;
                worker += 2;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UseManaShop4()
        {
            if (thirdMana >= 4)
            {
                thirdMana -= 4;
                coin += 7;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UseManaShop5()
        {
            if (thirdMana >= 4)
            {
                thirdMana -= 4;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UseManaShop6()
        {
            if (thirdMana >= 6)
            {
                thirdMana -= 6;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ADJUST INFOMATION
        public void AddScore(int score)
        {
            this.score += score;
        }
        public void RemoveScore(int score)
        {
            this.score -= score;
        }
        public void AdjustWorker(int adjustNum)
        {
            worker += adjustNum;
        }
        public void AdjustCoin(int adjustNum)
        {
            coin += adjustNum;
        }
        public void AdjustPriests(int adjustNum)
        {
            if(maxPriests-priests-adjustNum>0)
            {
                priests++;
            }
            else
            {
                priests = maxPriests;
            }
        }
        public void AdjustKey(int adjustNum)
        {
            key += adjustNum;
        }
        #endregion

        #region MANA SPECIAL ACTION
        public void GetMana(int getNum)
        {
            if (firstMana >= getNum)
            {
                secondMana += getNum;
                firstMana -= getNum;
            }
            else
            {
                firstMana = 0;
                secondMana += firstMana;
                secondMana -= (getNum - firstMana);
            }
        }
        public void SpendMana(int spendNum)
        {
            thirdMana -= spendNum;
            firstMana += spendNum;
        }
        public bool ManaChange1()
        {
            if(thirdMana>=1)
            {
                AdjustCoin(1);
                SpendMana(1);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ManaChange2()
        {
            if (thirdMana >= 3)
            {
                AdjustWorker(1);
                SpendMana(3);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ManaChange3()
        {
            if (thirdMana >= 5 && maxPriests - priests > 0)
            {
                priests++;
                SpendMana(5);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool BurnMana(int burnNum) 
        {
            if(secondMana >=2)
            {
                secondMana -= (2 * burnNum);
                thirdMana += burnNum;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        public virtual void StartingSet()
        {

        }
        public virtual void StartingRight()
        {

        }
        public virtual void StartingRight(int num)
        {
            
        }
        public virtual void StrongholdRight()
        {

        }
        #region SHOW INFOMATION
        public virtual void ShowMessage()
        {
            
        }
        public string ShowInfo()
        {
            string info = "";
            info += $"工人:{worker}\r\n錢:{coin}\r\n祭司:{priests}\r\n魔力1:{firstMana}\r\n魔力2:{secondMana}\r\n魔力3:{thirdMana}\r\n";
            info += $"鏟地:{workingLevel}\r\n航海:{shippingLevel}\r\n";
            info += $"剩餘平房:{dwellings}\n";
            info += $"剩餘大房:{tradingHouse}\n";
            info += $"剩餘圓房:{temples}\n";
            info += $"剩餘要塞:{stronghold}\n";
            info += $"剩餘聖殿:{sanctuary}\n";
            return info;
        }
        public string ShowName()
        {
            return name;
        }
        public int[][] ShowBridges()
        {
            return bridges;
        }
        public Color ShowColor()
        {
            return color;
        }
        public int ShowTowerValue(int tower)
        {
            return priestsBurning[tower];
        }
        #endregion
        public void Pass()
        {
            pass = true;
        }
        public bool IsPass()
        {
            return pass;
        }
        public void ResetPass()
        {
            pass = false;
        }
        #region OCCUPY METHOD

        public bool OccupyDesert()
        {
            int cost = (occupyingTable[0] * (4 - workingLevel));
            if (worker >= cost)
            {
                worker -= (occupyingTable[0] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool OccupyForest()
        {
            int cost = (occupyingTable[1] * (4 - workingLevel));
            if(worker>=cost) 
            {
                worker -= (occupyingTable[1] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool OccupySwamp()
        {
            int cost = (occupyingTable[2] * (4 - workingLevel));
            if (worker >= cost)
            {
                worker -= (occupyingTable[2] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool OccupyPlains()
        {
            int cost = (occupyingTable[3] * (4 - workingLevel));
            if (worker >= cost)
            {
                worker -= (occupyingTable[3] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool OccupyLakes()
        {
            int cost = (occupyingTable[4] * (4 - workingLevel));
            if (worker >= cost)
            {
                worker -= (occupyingTable[4] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool OccupyWasteland()
        {
            int cost = (occupyingTable[5] * (4 - workingLevel));
            if (worker >= cost)
            {
                worker -= (occupyingTable[5] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool OccupyMountains()
        {
            int cost = (occupyingTable[6] * (4 - workingLevel));
            if (worker >= cost)
            {
                worker -= (occupyingTable[6] * (4 - workingLevel));
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
