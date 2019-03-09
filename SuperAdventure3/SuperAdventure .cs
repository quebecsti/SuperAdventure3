using Engine;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SuperAdventure3
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;

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
                    rtbMessages.Text += $"You received the {newLocation.QuestAvailableHere.Name} quest.\r\n";
                    rtbMessages.Text += $"{newLocation.QuestAvailableHere.Description}\r\n";
                    rtbMessages.Text += $"To completet quest, return here with : \n\r";


                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += $"{qci.Quantity.ToString()} {qci.Details.Name}\r\n";
                        }
                        else
                        {
                            rtbMessages.Text += $"{qci.Quantity.ToString()} {qci.Details.NamePlural}\r\n";
                        }

                        // Add Quest to questList
                        _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                    }

                }


                UpdateUI();
            }

            //Is there Monster at location?
            if (newLocation.MonsterLivingHere != null)
            {
                // Display Message monster here
                rtbMessages.Text += $"You've just encountered a {newLocation.MonsterLivingHere.Name} with {newLocation.MonsterLivingHere.MaximumHitPoints.ToString()} health\r\n";

                // Spawn Monster
                Monster stdMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(stdMonster.ID, stdMonster.Name, stdMonster.RewardGold, stdMonster.RewardExperiencePoints, stdMonster.RewardGold, stdMonster.CurrentHitPoints, stdMonster.MaximumHitPoints);

                //Set LootTable

                foreach (LootItem li in stdMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(li);
                }

                cboPotions.Visible = true;
                cboWeapons.Visible = true;
                btnUsePotion.Visible = true;
                btnUseWeapon.Visible = true;
            }
            else
            {
                _currentMonster = null;

                // Hide Combat boxes and buttons
                cboPotions.Visible = false;
                cboWeapons.Visible = false;
                btnUsePotion.Visible = false;
                btnUseWeapon.Visible = false;
            }


            dgvInventory.Rows.Clear();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { ii.Details.Name, ii.Quantity.ToString() });
                }
            }

            //DatagridView :dgvQuests

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest pq in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { pq.Details.Name, pq.IsCompleted.ToString() });
            }

            // Update cboWaepon

            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details is Weapon)
                {
                    if (ii.Quantity > 0)
                    {
                        weapons.Add((Weapon)ii.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                foreach (Weapon w in weapons)
                {
                    cboWeapons.DataSource = weapons;
                    cboWeapons.DisplayMember = "Name";
                    cboWeapons.ValueMember = "iD";
                }
            }


            //update Healing Potion

            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details is HealingPotion)
                {
                    if (ii.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)ii.Details);
                    }
                }

            }

            if (healingPotions.Count == 0)
            {
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;

            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "iD";
                cboPotions.SelectedIndex = 0;
            }
        }


        #region Movement button
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

        #endregion


    }
}
