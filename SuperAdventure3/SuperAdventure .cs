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

            //Update Location
            _player.CurrentLocation = newLocation;

            bool playerHasItemToEnterThisLocation = _player.HasRequiredItemToEnterThisLocation(newLocation);

            bool playerAlreadyHasThisQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
            bool playerAlreadyCompletedThisQuest = _player.HasCompletedQuest(newLocation.QuestAvailableHere);
           // bool playerHasAllRequiredItemsForQuests = _player.HasAllRequiredItemsForQuests(newLocation.QuestAvailableHere);

            //Check For requiyired Item
            if (!playerHasItemToEnterThisLocation)
            {
                rtbMessages.Text += $"Required to enter is {newLocation.ItemRequiredToEnter.Name}\n\r";
                return;
            }

            //Check for Quest
            if (!playerAlreadyHasThisQuest)
            {
                //Add Quest
                //Display received Quest
                rtbMessages.Text += $"You received the {newLocation.QuestAvailableHere.Name} quest.\r\n";
                rtbMessages.Text += $"{newLocation.QuestAvailableHere.Description}\r\n";
                rtbMessages.Text += $"To completet quest, return here with : \n\r";

                _player.AddQuestToPlayersQuest(newLocation.QuestAvailableHere);
            }
            else
            {
                //Check if player Completed Quest
                if (!playerAlreadyCompletedThisQuest)
                {
                    if (playerHasAllRequiredItemsForQuests)
                    {
                        _player.RemoveQuestCompletionItem(newLocation.QuestAvailableHere);

                        rtbMessages.Text += $"You have completed the {newLocation.QuestAvailableHere.Name} Quest.\n\r";
                        rtbMessages.Text += $"You Received {newLocation.QuestAvailableHere.RewardExperiencePoints} XP and {newLocation.QuestAvailableHere.RewardGold} gold\r\n";
                        rtbMessages.Text += $"You also received {newLocation.QuestAvailableHere.RewardItem}";

                        _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                        _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                        _player.AddRewardItem(newLocation.QuestAvailableHere);

                        _player.MarkAsQuestAsComplete(newLocation.QuestAvailableHere);
                    }
                }
            }

            rtbMessages.Text += $"Walked to {_player.CurrentLocation.Name}\n\r";

            rtbLocation.Text = _player.CurrentLocation.Description;

            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            //heal Player   
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            //Check For Quest
            _player.HasThisQuest(newLocation.QuestAvailableHere);

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


            /////----------- UI Maintenance ----------------------------------------
            UpdateInventoryUI();
            UpdateWeaponUi();
            UpdateQuestUI();
            UpdatePotionUI();

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

        private void UpdateQuestUI()
        {
            dgvQuests.Rows.Clear();

            foreach (PlayerQuest pq in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { pq.Details.Name, pq.IsCompleted.ToString() });
            }
        }

        private void UpdateInventoryUI()
        {
            dgvInventory.Rows.Clear();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { ii.Details.Name, ii.Quantity.ToString() });
                }
            }
        }

        private void UpdateWeaponUi()
        {
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
        }

        private void UpdatePotionUI()
        {
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
    }
}
