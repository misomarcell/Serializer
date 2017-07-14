using Person.Properties;
using System;
using System.Diagnostics;
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
            human.Serialize();
        }

        private void LoadHumanData()
        {
            if (human != null)
            {
                txtName.Text = human.name;
                txtAddress.Text = human.address;
                txtPhone.Text = human.phone.ToString();

                Debug.WriteLine("SERIAL: " + human.serial);
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
                if ( File.Exists(Directory.GetCurrentDirectory() + "\\Person" + i + ".dat") )
                {
                    Debug.WriteLine("FILE FOUND AT SERIAL #" + i);
                    Human h = Human.Deserialize(i);
                    if (h != null)
                    {
                        human = h;
                        LoadHumanData();
                        break;
                    }
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
                if (File.Exists(Directory.GetCurrentDirectory() + "\\Person" + i + ".dat"))
                {
                    Debug.WriteLine("FILE FOUND AT SERIAL #" + i);
                    Human h = Human.Deserialize(i);
                    if (h != null)
                    {
                        human = h;
                        LoadHumanData();
                        break;
                    }
                }
            }
        }
    }
}
