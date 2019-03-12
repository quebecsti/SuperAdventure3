using System.Collections.Generic;


namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }
        public Location CurrentLocation { get; set; }

        public Player(int gold, int experiencePoints, int level, int currentHitPoints, int maximumHitPoints) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter != null)
            {
                return true;
            }

            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == location.ItemRequiredToEnter.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasThisQuest(Quest quest)
        {
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasCompletedQuest(Quest quest)
        {
            foreach (PlayerQuest pq in Quests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    return pq.IsCompleted;
                }
            }
            return false;
        }

        public void AddQuestToPlayersQuest(Quest newQuest)
        {
            foreach (QuestCompletionItem qci in newQuest.QuestCompletionItems)
            {
                if (qci.Quantity == 1)
                {
                    // rtbMessages.Text += $"{qci.Quantity.ToString()} {qci.Details.Name}\r\n";
                }
                else
                {
                    //  rtbMessages.Text += $"{qci.Quantity.ToString()} {qci.Details.NamePlural}\r\n";
                }

                // Add Quest to questList
                Quests.Add(new PlayerQuest(newQuest));
            }
        }

        //public bool HasAllRequiredItemsForQuests(Quest newQuest)
        //{
        //    foreach (QuestCompletionItem qci in newQuest.QuestCompletionItems)
        //    {
        //        bool foundItem = false;

        //        foreach (InventoryItem ii in Inventory)
        //        {
        //            if (ii.Details.ID == qci.Details.ID)
        //            {
        //                foundItem = true;
        //                if (ii.Quantity < qci.Quantity)
        //                {
        //                    return false;
        //                }
        //            }
        //        }

        //        if (!foundItem)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public void RemoveQuestCompletionItem(Quest newQuest)
        {
            foreach (QuestCompletionItem qci in newQuest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if (ii.Details.ID == qci.Details.ID)
                    {
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void AddRewardItem(Quest newQuest)
        {
            bool addedItemToPlayerInventory = false;

            foreach (InventoryItem ii in Inventory)
            {
                if (ii.Details.ID == newQuest.RewardItem.ID)
                {
                    ii.Quantity++;
                    addedItemToPlayerInventory = true;
                    break;
                }

                if (!addedItemToPlayerInventory)
                {
                    Inventory.Add(new InventoryItem(newQuest.RewardItem, 1));
                }
            }
        }

        public void MarkAsQuestAsComplete(Quest pq)
        {
            foreach(PlayerQuest _pq in Quests)
            {
                _pq.IsCompleted = true;
                break;
            }
        }
    }
}

