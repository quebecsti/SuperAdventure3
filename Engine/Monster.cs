using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public List<LootItem> LootTable { get; set; }

        public Monster(int iD, string name, int maximumDamage, int rewardExperiencePoints, int rewardGold, int currentHitPoints, int maximumHitPoints ):base(currentHitPoints, maximumHitPoints)
        {
            ID = iD;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;

            LootTable = new List<LootItem>();
        }

        //public void Attack(Player player)
        //{
        //    int damageToPlayer = RandomNumberGenerator.NumberBetween(0, MaximumDamage);

        //  //  rtbMessages.Text += $"The {Name} did {damageToPlayer} points of damage. \n\r";

        //    player.CurrentHitPoints -= damageToPlayer;
            
        //    if (player.CurrentHitPoints <= 0)
        //    {
        //        // player Dead
        //        rtbMessages.Text += $"The {Name} just killed you.\r\n";

        //        MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        //    }
        //}
    }
}
