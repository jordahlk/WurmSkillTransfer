using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NLog;

namespace WurmPlayerExporter
{
    public partial class Form1 : Form
    {
        private string sourceDbFile;
        private KeyValuePair<string, string> player;
        private string destDbFile;
        private string playerFile;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                InitialDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Wurm Unlimited Dedicated Server\\Creative\\Sqlite",
                Filter = @"db files (*.db)|*.db|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sourceDbFile = openFileDialog1.FileName;
                    var players = new Dictionary<string, string>();
                    string connectionString = $"Data Source={sourceDbFile};Version=3;New=False;Compress=True;";
                    _logger.Debug($"Making connection to {connectionString}");
                    using (var sqlConn = new SQLiteConnection(connectionString))
                    {
                        sqlConn.Open();
                        var command = sqlConn.CreateCommand();
                        command.CommandText = @"SELECT WurmID, Name FROM PLAYERS";
                        command.CommandType = CommandType.Text;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            _logger.Debug($"Adding {reader["Name"]} to players collection");
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
                    _logger.Error($"Exception thrown during player lookup: {ex}");
                    MessageBox.Show($"Error: Could not read file from disk. Original error: {ex.Message}");
                    throw;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var cmb = (ComboBox)sender;
            player = (KeyValuePair<string, string>)cmb.SelectedItem;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string connectionString = $"Data Source={sourceDbFile};Version=3;New=False;Compress=True;";
                _logger.Debug($"Making connection to {connectionString}");
                using (var sqlConn = new SQLiteConnection(connectionString))
                {
                    sqlConn.Open();

                    //Skills
                    var command1 = sqlConn.CreateCommand();
                    command1.CommandText = $"SELECT Id, Owner, Number, Value, Minvalue FROM SKILLS WHERE OWNER = '{player.Key}'";
                    command1.CommandType = CommandType.Text;
                    var rdr1 = command1.ExecuteReader();
                    while (rdr1.Read())
                    {
                        var statement = $"INSERT OR REPLACE INTO SKILLS(Id, Owner, Number, Value, Minvalue) VALUES({rdr1["Id"]}, {rdr1["Owner"]}, {rdr1["Number"]}, {rdr1["Value"]}, {rdr1["Minvalue"]});";
                        _logger.Debug($"Adding to Skills export: {statement}");
                        sb.AppendLine(statement);
                    }
                    rdr1.Dispose();

                    //Affinities
                    var command2 = sqlConn.CreateCommand();
                    command2.CommandType = CommandType.Text;
                    command2.CommandText = $"SELECT WurmID, Skill, Number FROM AFFINITIES WHERE WURMID = '{player.Key}'";
                    var rdr2 = command2.ExecuteReader();
                    while (rdr2.Read())
                    {
                        var statement = $"INSERT OR REPLACE INTO AFFINITIES(WurmID, Skill, Number) VALUES({rdr2["WurmId"]}, {rdr2["Skill"]}, {rdr2["Number"]});";
                        _logger.Debug($"Adding to Affinities export: {command2.CommandText}");
                        sb.AppendLine(statement);
                    }
                    rdr2.Dispose();

                    //Titles
                    var command3 = sqlConn.CreateCommand();
                    command3.CommandType = CommandType.Text;
                    command3.CommandText = $"SELECT WurmID, TitleId, TitleName FROM TITLES WHERE WURMID = '{player.Key}'";
                    var rdr3 = command3.ExecuteReader();
                    while (rdr3.Read())
                    {
                        var statement = $"INSERT OR REPLACE INTO TITLES(WurmID, TitleId, TitleName) VALUES({rdr3["WurmId"]}, {rdr3["TitleId"]}, \"{rdr3["TitleName"]}\");";
                        _logger.Debug($"Adding to Titles export: {statement}");
                        sb.AppendLine(statement);
                    }
                    rdr3.Dispose();

                    //Achievements
                    var command4 = sqlConn.CreateCommand();
                    command4.CommandType = CommandType.Text;
                    command4.CommandText = $"SELECT Player, Achievement, Counter, ADate FROM ACHIEVEMENTS WHERE Player = '{player.Key}'";
                    _logger.Debug($"Adding to Achievements export: {command4.CommandText}");
                    var rdr4 = command4.ExecuteReader();
                    while (rdr4.Read())
                    {
                        var statement = $"INSERT OR REPLACE INTO ACHIEVEMENTS(Player, Achievement, Counter, ADate) VALUES({rdr4["Player"]}, {rdr4["Achievement"]}, {rdr4["Counter"]}, '{Convert.ToDateTime(rdr4["ADate"]):yyyy-MM-dd HH:mm:ss}');";
                        _logger.Debug($"Adding to Achievements export: {statement}");
                        sb.AppendLine(statement);
                    }
                    rdr4.Dispose();

                    //Religion
                    var command5 = sqlConn.CreateCommand();
                    command5.CommandType = CommandType.Text;
                    command5.CommandText = $"SELECT Faith, Deity, Alignment, God, Favor FROM Players WHERE WurmId = '{player.Key}'";
                    
                    var rdr5 = command5.ExecuteReader();
                    while (rdr5.Read())
                    {
                        var statement = $"UPDATE PLAYERS SET Faith = {rdr5["Faith"]}, Deity = {rdr5["Deity"]}, Alignment = {rdr5["Alignment"]}, God = {rdr5["God"]}, Favor = {rdr5["Favor"]} WHERE WurmId = {player.Key};";
                        _logger.Debug($"Adding to Religion export: {statement}");
                        sb.AppendLine(statement);
                    }
                    rdr5.Dispose();

                    sqlConn.Close();
                }

                StreamWriter writer = new StreamWriter($"{player.Value}.txt");
                _logger.Debug($"Writing to {player.Value}.txt: {sb}");
                writer.Write(sb.ToString());
                writer.Dispose();
                _logger.Info($"Created file {player.Value}");
                MessageBox.Show($"Created file: {player.Value}.txt");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception thrown during export: {ex}");
                MessageBox.Show($"Error occurred attempting to export {player.Value}: {ex}");
                throw;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SelectDatabase();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog3 = new OpenFileDialog
            {
                InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Filter = @"txt files (*.txt)|*.txt",
                RestoreDirectory = true
            };

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                playerFile = openFileDialog3.FileName;
                var playerName = playerFile.Substring(playerFile.LastIndexOf('\\') + 1);
                lblCharacterName.Text = playerName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(destDbFile))
            {
                MessageBox.Show(@"Select a destination database file first!");
                return;
            }
            if (string.IsNullOrEmpty(playerFile))
            {
                MessageBox.Show(@"Select a player file first!");
                return;
            }

            try
            {
                using (var streamReader = new StreamReader(playerFile))
                {
                    using (var conn = new SQLiteConnection($"Data Source={destDbFile};Version=3;New=False;Compress=True;"))
                    {
                        _logger.Debug($"Opening connection to {conn}");
                        conn.Open();
                        var cmd = conn.CreateCommand();

                        //Get player name and originalID from the txt file
                        var reader = new StreamReader(playerFile);
                        var firstLine = reader.ReadLine();
                        var playerId = firstLine.Split(',')[5].Trim();
                        _logger.Debug($"Got a player Id of {playerId} from the first line of the file");
                        var playerName = playerFile.Substring(playerFile.LastIndexOf('\\') + 1);
                        playerName = playerName.Replace(".txt", "");
                        _logger.Debug($"Got a player name of {playerName} from the file name");
                        reader.Close();

                        cmd.CommandText = $"SELECT WURMID FROM PLAYERS WHERE LOWER(Name) = '{playerName.ToLower()}'";
                        var rdr = cmd.ExecuteReader();
                        string newId = string.Empty;
                        while (rdr.Read())
                        {
                            newId = rdr["WurmId"].ToString();
                        }

                        if (string.IsNullOrEmpty(newId))
                        {
                            MessageBox.Show(@"Player not found in destination DB, create the character before attempting to import.");
                            _logger.Warn($"The player name {playerName} was not found in {destDbFile}");
                            return;
                        }
                        _logger.Debug($"Got a WurmId of {newId} from {destDbFile}");

                        var stringBuilder = new StringBuilder();
                        stringBuilder.Append(streamReader.ReadToEnd());

                        var command = new SQLiteCommand(conn)
                        {
                            CommandText = stringBuilder.ToString().Replace(playerId, newId),
                            CommandType = CommandType.Text
                        };

                        _logger.Debug($"Sending the following command to DB: {command.CommandText}");
                        command.ExecuteNonQuery();

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception thrown during import: {ex}");
                MessageBox.Show($"Error loading character into db: {ex.Message}");
                throw;
            }

            MessageBox.Show(@"Loaded character into db!");
        }

        private void btnDumpSelectDB_Click(object sender, EventArgs e)
        {
            SelectDatabase();
        }

        private void SelectDatabase()
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog
            {
                InitialDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Wurm Unlimited Dedicated Server\\Creative\\Sqlite",
                Filter = @"db files (*.db)|*.db|All files (*.*)|*.*",
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
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnSelectCharacterDump_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog3 = new OpenFileDialog
            {
                InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Filter = @"txt files (*.txt)|*.txt",
                RestoreDirectory = true
            };

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                playerFile = openFileDialog3.FileName;
                var playerName = playerFile.Substring(playerFile.LastIndexOf('\\') + 1);
                lblDumpPlayerName.Text = playerName;
            }
        }

        private void btnImportDump_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(destDbFile))
            {
                MessageBox.Show(@"Select a destination database file first!");
                return;
            }
            if (string.IsNullOrEmpty(playerFile))
            {
                MessageBox.Show(@"Select a player file first!");
                return;
            }

            try
            {
                using (var streamReader = new StreamReader(playerFile))
                {
                    using (var conn = new SQLiteConnection($"Data Source={destDbFile};Version=3;New=False;Compress=True;"))
                    {
                        _logger.Debug($"Opening connection to {conn}");
                        conn.Open();
                        var cmd = conn.CreateCommand();
                        var playerName = playerFile.Substring(playerFile.LastIndexOf('\\') + 1);
                        playerName = playerName.Replace(".txt", "");
                        _logger.Debug($"Got a player name of {playerName} from the file name");

                        cmd.CommandText = $"SELECT WURMID FROM PLAYERS WHERE LOWER(Name) = '{playerName.ToLower()}'";
                        var rdr = cmd.ExecuteReader();
                        string newId = string.Empty;
                        while (rdr.Read())
                        {
                            newId = rdr["WurmId"].ToString();
                        }
                        if (string.IsNullOrEmpty(newId))
                        {
                            MessageBox.Show(@"Player not found in destination DB, create the character before attempting to import.");
                            _logger.Warn($"The player name {playerName} was not found in {destDbFile}");
                            return;
                        }
                        _logger.Debug($"Got a WurmId of {newId} from {destDbFile}");

                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine($"DELETE FROM Skills WHERE OWNER = '{newId}';");
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (line.StartsWith("Skills") || line.StartsWith("----") || line.StartsWith("Characteristics") || line.StartsWith("Religion"))
                                continue;

                            var skillName = line.Substring(0, line.IndexOf(":")).Trim();
                            var skillPart = line.Substring(line.IndexOf(":") + 2).Trim();
                            var skillValue = skillPart.Split(' ')[1];
                            var affinityPart = skillPart.Split(' ')[2];
                            _logger.Debug($"**** Processing record from dump with values: SkillName={skillName} - SkillValue={skillValue} - Affinities={affinityPart} ****");

                            if (skillName.ToLower() == "faith")
                            {
                                var statement = $"UPDATE PLAYERS SET FAITH = '{skillValue}' WHERE WURMID = '{newId}';";
                                stringBuilder.AppendLine(statement);
                                _logger.Debug($"Adding to SQL Command: {statement}");
                            }
                            else if (skillName.ToLower() == "favor")
                            {
                                var statement = $"UPDATE PLAYERS SET FAVOR = '{skillValue}' WHERE WURMID = '{newId}';";
                                stringBuilder.AppendLine(statement);
                                _logger.Debug($"Adding to SQL Command: {statement}");
                            }
                            else if (skillName.ToLower() == "alignment")
                            {
                                var statement = $"UPDATE PLAYERS SET ALIGNMENT = '{skillValue}' WHERE WURMID = '{newId}';";
                                stringBuilder.AppendLine(statement);
                                _logger.Debug($"Adding to SQL Command: {statement}");
                            }
                            else
                            {
                                var skillId = Skills.SkillDictionary[skillName.ToLower()];
                                _logger.Debug($"Converted {skillName} to an Id of {skillId}");
                                var statement = $"INSERT INTO Skills (Id, Owner, Number, Value, MinValue) VALUES ((SELECT max(id) FROM SKILLS)+1, {newId}, {skillId}, {skillValue}, {skillValue});";
                                stringBuilder.AppendLine(statement);
                                _logger.Debug($"Adding to SQL Command: {statement}");
                                if (affinityPart != "0")
                                {
                                    var affinityStatement = $"INSERT OR REPLACE INTO AFFINITIES(WurmID, Skill, Number) VALUES({newId}, {skillId}, {affinityPart});";
                                    stringBuilder.AppendLine(affinityStatement);
                                    _logger.Debug($"Adding to SQL Command: {affinityStatement}");
                                }
                            }
                        }

                        switch (cmbPriest.SelectedIndex)
                        {
                            case -1:
                            case 0:
                                var x = $"UPDATE PLAYERS SET PRIEST = '0', DEITY = '0' WHERE WURMID = {newId};";
                                stringBuilder.AppendLine(x);
                                _logger.Debug($"Adding to SQL Command: {x}");
                                break;
                            default:
                                var y = $"UPDATE PLAYERS SET PRIEST = '1', DEITY = '{cmbPriest.SelectedIndex}' WHERE WURMID = {newId};";
                                stringBuilder.AppendLine(y);
                                _logger.Debug($"Adding to SQL Command: {y}");
                                break;
                        }

                        var command = new SQLiteCommand(conn)
                        {
                            CommandText = stringBuilder.ToString(),
                            CommandType = CommandType.Text
                        };

                        _logger.Debug($"Sending following SQL Command to DB: {command.CommandText}");
                        command.ExecuteNonQuery();

                        conn.Close();
                    }
                }
                MessageBox.Show(@"Player imported!");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception thrown while importing dump: {ex}");
                MessageBox.Show($"Error loading character into db {ex.Message}");
                throw;
            }
        }
    }
}