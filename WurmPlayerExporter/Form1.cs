using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WurmPlayerExporter
{
    public partial class Form1 : Form
    {
        private string sourceDbFile;
        private KeyValuePair<string, string> player;
        private string destDbFile;
        private string playerFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory =
                    "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Wurm Unlimited Dedicated Server\\Creative\\Sqlite",
                Filter = "db files (*.db)|*.db|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sourceDbFile = openFileDialog1.FileName;
                    var players = new Dictionary<string, string>();
                    string a = $@"Data Source={sourceDbFile};Version=3;New=False;Compress=True;";
                    using (var sqlConn = new SQLiteConnection(a))
                    {
                        sqlConn.Open();
                        var command = sqlConn.CreateCommand();
                        command.CommandText = @"SELECT WurmID, Name FROM PLAYERS";
                        command.CommandType = CommandType.Text;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            players.Add(reader["WurmId"].ToString(), reader["Name"].ToString());
                        }
                        sqlConn.Close();
                    }

                    comboBox1.DataSource = new BindingSource(players, null);
                    comboBox1.DisplayMember = "Value";
                    comboBox1.ValueMember = "Key";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var cmb = (ComboBox) sender;
            player = (KeyValuePair<string, string>) cmb.SelectedItem;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            string a = $@"Data Source={sourceDbFile};Version=3;New=False;Compress=True;";
            using (var sqlConn = new SQLiteConnection(a))
            {
                sqlConn.Open();
                
                //Skills
                var command1 = sqlConn.CreateCommand();
                command1.CommandText = $@"SELECT Id, Owner, Number, Value, Minvalue FROM SKILLS WHERE OWNER = '{player.Key}'";
                command1.CommandType = CommandType.Text;
                var rdr1 = command1.ExecuteReader();
                while (rdr1.Read())
                {
                    sb.AppendFormat(@"INSERT OR REPLACE INTO SKILLS(Id, Owner, Number, Value, Minvalue) VALUES({0}, {1}, {2}, {3}, {4});{5}", rdr1["Id"], rdr1["Owner"], rdr1["Number"], rdr1["Value"], rdr1["Minvalue"], Environment.NewLine);
                }
                rdr1.Dispose();
                
                //Affinities
                var command2 = sqlConn.CreateCommand();
                command2.CommandType = CommandType.Text;
                command2.CommandText = $@"SELECT WurmID, Skill, Number FROM AFFINITIES WHERE WURMID = '{player.Key}'";
                var rdr2 = command2.ExecuteReader();
                while (rdr2.Read())
                {
                    sb.AppendFormat(@"INSERT OR REPLACE INTO AFFINITIES(WurmID, Skill, Number) VALUES({0}, {1}, {2});{3}", rdr2["WurmId"], rdr2["Skill"], rdr2["Number"], Environment.NewLine);
                }
                rdr2.Dispose();

                //Titles
                var command3 = sqlConn.CreateCommand();
                command3.CommandType = CommandType.Text;
                command3.CommandText = $@"SELECT WurmID, TitleId, TitleName FROM TITLES WHERE WURMID = '{player.Key}'";
                var rdr3 = command3.ExecuteReader();
                while (rdr3.Read())
                {
                    sb.AppendFormat(@"INSERT OR REPLACE INTO TITLES(WurmID, TitleId, TitleName) VALUES({0}, {1}, '{2}');{3}", rdr3["WurmId"], rdr3["TitleId"], rdr3["TitleName"], Environment.NewLine);
                }
                rdr3.Dispose();

                //Achievements
                var command4 = sqlConn.CreateCommand();
                command4.CommandType = CommandType.Text;
                command4.CommandText = $@"SELECT Player, Achievement, Counter, ADate FROM ACHIEVEMENTS WHERE Player = '{player.Key}'";
                var rdr4 = command4.ExecuteReader();
                while (rdr4.Read())
                {
                    sb.AppendFormat(@"INSERT OR REPLACE INTO ACHIEVEMENTS(Player, Achievement, Counter, ADate) VALUES({0}, {1}, {2}, '{3}');{4}", rdr4["Player"], rdr4["Achievement"], rdr4["Counter"], Convert.ToDateTime(rdr4["ADate"]).ToString("yyyy-MM-dd HH:mm:ss"), Environment.NewLine);
                }
                rdr4.Dispose();

                //Religion
                var command5 = sqlConn.CreateCommand();
                command5.CommandType = CommandType.Text;
                command5.CommandText = $@"SELECT Faith, Deity, Alignment, God, Favor FROM Players WHERE WurmId = '{player.Key}'";
                var rdr5 = command5.ExecuteReader();
                while (rdr5.Read())
                {
                    sb.AppendFormat(@"UPDATE PLAYERS SET Faith = {0}, Deity = {1}, Alignment = {2}, God = {3}, Favor = {4} WHERE WurmId = {5};{6}", rdr5["Faith"], rdr5["Deity"], rdr5["Alignment"], rdr5["God"], rdr5["Favor"], player.Key, Environment.NewLine);
                }
                rdr5.Dispose();

                sqlConn.Close();
            }

            StreamWriter writer = new StreamWriter($@"{player.Value}.txt");
            writer.Write(sb.ToString());
            writer.Dispose();
            MessageBox.Show($@"Created file: {player.Value}.txt");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog
            {
                InitialDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Wurm Unlimited Dedicated Server\\Creative\\Sqlite",
                Filter = "db files (*.db)|*.db|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };


            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    destDbFile = openFileDialog2.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog3 = new OpenFileDialog
            {
                InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Filter = "txt files (*.txt)|*.txt",
                RestoreDirectory = true
            };

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                playerFile = openFileDialog3.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(destDbFile))
            {
                MessageBox.Show("Select a destination database file first!");
                return;
            }
            if (string.IsNullOrEmpty(playerFile))
            {
                MessageBox.Show("Select a player file first!");
                return;
            }

            try
            {
                using (var streamReader = new StreamReader(playerFile))
                {
                    using (var conn = new SQLiteConnection($@"Data Source={destDbFile};Version=3;New=False;Compress=True;"))
                    {
                        conn.Open();
                        var cmd = conn.CreateCommand();

                        //Get player name and originalID from the txt file
                        var reader = new StreamReader(playerFile);
                        var firstLine = reader.ReadLine();
                        var playerId = firstLine.Split(',')[5].Trim();
                        var playerName = playerFile.Substring(playerFile.LastIndexOf('\\') + 1);
                        playerName = playerName.Replace(".txt", "");
                        reader.Close();

                        cmd.CommandText = $@"SELECT WURMID FROM PLAYERS WHERE Name = '{playerName}'";
                        var rdr = cmd.ExecuteReader();
                        string newId = string.Empty;
                        while (rdr.Read())
                        {
                            newId = rdr["WurmId"].ToString();
                        }

                        if (string.IsNullOrEmpty(newId))
                        {
                            MessageBox.Show("Player not found in destination DB, create the character before attempting to import.");
                            return;
                        }

                        var stringBuilder = new StringBuilder();
                        stringBuilder.Append(streamReader.ReadToEnd());

                        var command = new SQLiteCommand(conn);
                        command.CommandText = stringBuilder.ToString().Replace(playerId, newId).Replace("'", "''");
                        command.CommandType = CommandType.Text;
                        
                        command.ExecuteNonQuery();

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading character into db: " + ex.Message);
                throw;
            }

            MessageBox.Show("Loaded character into db!");
        }
    }
}