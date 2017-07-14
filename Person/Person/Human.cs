using Person.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Person
{
    [Serializable]
    public class Human : IDeserializationCallback
    {
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        private DateTime recorded { get; }
        public int serial { get; set; }

        public Human(string _name, string _address, string _phone)
        {
            name = _name;
            address = _address;
            phone = _phone;

            recorded = DateTime.Now;
            serial = Settings.Default.LastSerial + 1;
            Settings.Default.LastSerial = serial;
            Settings.Default.Save();

            Serialize();
        }

        public static Human Deserialize(int serial = -1)
        {
            Human human = null;
            string fileName = String.Empty;

            if ( serial < 0) // If user NOT given serial to deserialize...
            {
                fileName = SearchForPerson();
            }
            else // If user given serial to deserialize...
            {
                string tempFileName = "Person" + serial + ".dat";
                if ( File.Exists(Directory.GetCurrentDirectory() + "\\" + tempFileName) )
                {
                    fileName = tempFileName;
                }
            }

            if ( fileName == String.Empty ) //Check if file was found
            {
                return human;
            }

            FileStream fs = new FileStream(fileName, FileMode.Open); 
            BinaryFormatter bf = new BinaryFormatter();
            human = (Human)bf.Deserialize(fs);
            fs.Close();

            return human;
        }
        public void Serialize()
        {
            string fileName = "Person" + serial + ".dat";
            if ( File.Exists(Directory.GetCurrentDirectory() + "\\" + fileName) )
            {
                serial++;
                Serialize();
            }

            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            fs.Close();
        }

        /// <summary>
        /// Returns the name of the first existing Person.dat file in the working directory, otherwise String.Empty
        /// </summary>
        private static string SearchForPerson()
        {
            for (int i = 0; i < 100; i++) //Check for the first file to open
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "\\Person" + i + ".dat"))
                {
                    return "Person" + i + ".dat";
                }
            }

            return String.Empty;
        }


        public void OnDeserialization(object sender)
        {
            //I have no idea how to get the serial number with this.
        }
    }
}
