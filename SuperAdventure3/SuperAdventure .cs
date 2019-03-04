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
            // _player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.CurrentHitPoints = 4;
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
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

            /*
            //Check For Quest

            if (_player.CurrentLocation.QuestAvailableHere = true)
            {
                _player.Quests = 
            }
            */





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
