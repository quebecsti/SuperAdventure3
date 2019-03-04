using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;


namespace SuperAdventure3
{
    public partial class Form1 : Form
    {
        private Player _player;

        public Form1()
        {
            InitializeComponent();

            Location location = new Location(1, "home", "This is our home");
            _player = new Player(20, 0, 1, 10, 10);




            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }
    }
}
