using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terra
{
    internal class Nomads : Character
    {
        protected bool strongholdRightUsable = false;
        public Nomads(int playerNO) : base(playerNO) 
        {
            StartingRight();
        }
        public override void StartingSet()
        {
            name = "遊牧民族";
            worker = 20;
            coin = 150;
            occupyingTable = [0, 3, 2, 1, 3, 1, 2];
            priestsBurning[0] = 1;
            priestsBurning[2] = 1;
            color = Color.Gold;
            thirdMana = 30;
        }
        public override void StartingRight() 
        {
            dwellings = 5;
        }
        public new void StrongholdLevelUp()
        {
            base.StrongholdLevelUp();
            strongholdRightUsable = true;
        }
        public override void StrongholdRight()
        {
            
        }
        public override void ShowMessage()
        {
            MessageBox.Show("Nomads");
        }
    }
}
