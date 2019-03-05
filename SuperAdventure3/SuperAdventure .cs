using Engine;
using System.Windows.Forms;


namespace SuperAdventure3
{
    public partial class SuperAdventure : Form
    {
        private Player _player;

        public SuperAdventure()
        {
            InitializeComponent();

            _player = new Player(20, 0, 1, 10, 10);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();

            //refresh Inventory

            //refresh PlayerQuest

            //refresh cboWeapon

            //refresh cboPotion
        }

        private void MoveTo(Location newLocation)
        {
            //Check For requiyired Item
            if (newLocation.ItemRequiredToEnter != null)
            {
                rtbMessages.Text += $"Required to enter is {newLocation.ItemRequiredToEnter.Name}\n\r";

                bool playerHasRequiredItem = false;

                foreach (InventoryItem ii in _player.Inventory)
                {
                    if (ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
                    {
                        playerHasRequiredItem = true;
                        rtbMessages.Text += "Turns out you have that Item.";
                    }
                }

                if (!playerHasRequiredItem)
                {
                    return;
                }
            }

            //Update Location
            _player.CurrentLocation = newLocation;

            rtbMessages.Text += $"Walked to {_player.CurrentLocation.Name}\n\r";
            rtbLocation.Text = _player.CurrentLocation.Description;

            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            //heal Player   
            _player.CurrentHitPoints = _player.MaximumHitPoints;


            //Check For Quest

            if (_player.CurrentLocation.QuestAvailableHere != null)
            {
                bool playerAlreadyHasQuest = false;
                bool playerAlreadyCompletedQuest = false;

                foreach (PlayerQuest pq in _player.Quests)
                {
                    if (pq.Details.ID == newLocation.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;

                        if (pq.IsCompleted)
                        {
                            playerAlreadyCompletedQuest = true;
                        }
                    }
                }

                //Player AHs quest?
                if (playerAlreadyHasQuest)
                {
                    if (!playerAlreadyCompletedQuest)
                    {
                        bool playerHasAllItemsToCompleteQuest = true;

                        foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                        {
                            bool foundItemInPlayerInventory = false;

                            foreach (InventoryItem ii in _player.Inventory)
                            {
                                if (ii.Details.ID < qci.Quantity)
                                {
                                    foundItemInPlayerInventory = false;
                                    break;
                                }
                                break;
                            }

                            if (!foundItemInPlayerInventory)
                            {
                                playerHasAllItemsToCompleteQuest = false;
                                break;
                            }

                        }

                        if (playerHasAllItemsToCompleteQuest)
                        {
                            rtbMessages.Text += $"You have completed the {newLocation.QuestAvailableHere.Name} Quest.\n\r";

                            foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                            {
                                foreach (InventoryItem ii in _player.Inventory)
                                {
                                    if (ii.Details.ID == qci.Details.ID)
                                    {
                                        ii.Quantity -= qci.Quantity;
                                        break;
                                    }
                                }
                            }

                            rtbMessages.Text += $"You Received {newLocation.QuestAvailableHere.RewardExperiencePoints} XP and {newLocation.QuestAvailableHere.RewardGold} gold\r\n";
                            rtbMessages.Text += $"You also received {newLocation.QuestAvailableHere.RewardItem}";

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            bool addedItemToPlayerInventory = false;

                            foreach (InventoryItem ii in _player.Inventory)
                            {
                                if (ii.Details.ID == newLocation.QuestAvailableHere.RewardItem.ID)
                                {
                                    ii.Quantity++;
                                    addedItemToPlayerInventory = true;
                                    break;
                                }

                                if (!addedItemToPlayerInventory)
                                {
                                    _player.Inventory.Add(new InventoryItem(newLocation.QuestAvailableHere.RewardItem, 1));
                                }
                            }


                            foreach (PlayerQuest pq in _player.Quests)
                            {
                                pq.IsCompleted = true;
                                break;
                            }
                        }
                    }

                }
                else
                {
                    //Display received Quest

                    // Add Quest to questList

                }

                //Is there Monster at location?
                if (newLocation.MonsterLivingHere != null)
                {
                    // Display Message monster here

                    // Spawn Monster

                    // Combat
                    ///// change weapon
                    ///// use potion
                    ///

                }
                else
                {
                    // Hide Combat boxes and buttons
                }


            }
            UpdateUI();
        }

        private void btnNorth_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }
    }
}
