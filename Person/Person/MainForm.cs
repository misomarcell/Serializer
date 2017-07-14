using System;
using System.IO;
using System.Windows.Forms;

namespace Person
{
    public partial class MainForm : Form
    {
        Human human;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            human = new Human(txtName.Text, txtAddress.Text, txtPhone.Text);
        }

        private void LoadHumanData()
        {
            if (human != null)
            {
                txtName.Text = human.name;
                txtAddress.Text = human.address;
                txtPhone.Text = human.phone.ToString();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            human = Human.Deserialize();
            LoadHumanData();         
        }

        private void GetNextHuman(object sender, EventArgs e)
        {
            if ( human == null || human.serial >= 100)
            {
                return;
            }

            for (int i = human.serial + 1; i < 100; i++)
            {
                if ( AttemptLoad(i) )
                {
                    break;
                }
            }
        }

        private void GetPreviousHuman(object sender, EventArgs e)
        {
            if (human == null || human.serial <= 0 )
            {
                return;
            }

            for (int i = human.serial - 1; i > 0; i--)
            {
                if ( AttemptLoad(i) )
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns true if the person with the given serial exist and can be loaded, otherwise false.
        /// </summary>
        private Boolean AttemptLoad(int serial)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Person" + serial + ".dat"))
            {
                Human h = Human.Deserialize(serial);
                if (h != null)
                {
                    human = h;
                    LoadHumanData();
                    return true;
                }
            }
            return false;
        }
    }
}
