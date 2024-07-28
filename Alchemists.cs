using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terra
{
    internal class Alchemists : Character
    {
        protected bool strongholdRightUsable = false;
        public Alchemists(int playerNO) : base(playerNO)
        {

        }
        public override void StartingSet()
        {
            name = "魔族";
            worker = 1;
            coin = 15;
            priests = 1;
            occupyingTable = [2, 2, 0, 1, 1, 3, 3];
            priestsBurning[1] = 1;
            priestsBurning[2] = 1;
            color = Color.Gray;
        }
        public override void StartingRight(int shovel)
        {
            priests -= shovel;
            score += (2 * shovel);
        }
        public new void StrongholdLevelUp()
        {
            base.StrongholdLevelUp();
            StrongholdRight();
        }
        public override void StrongholdRight()
        {
            worker -= 3;
            priests += 3;
        }
        public override void ShowMessage()
        {
            MessageBox.Show("Alchemists");
        }
    }
}
