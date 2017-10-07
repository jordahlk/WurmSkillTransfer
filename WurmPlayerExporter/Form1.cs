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
        private string sourcePlayerDbFile;
        private KeyValuePair<string, string> player;
        private string destinationPlayerDBFile;
        private string sourceItemDbFile;
        private string destItemDbFile;
        private string playerFile;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnSelectDB_Click(object sender, EventArgs e)
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
                    sourcePlayerDbFile = openFileDialog1.FileName;
                    var players = new Dictionary<string, string>();
                    string connectionString = $"Data Source={sourcePlayerDbFile};Version=3;New=False;Compress=True;";
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
                    MessageBox.Show($@"Error: Could not read file from disk. Original error: {ex.Message}");
                    throw;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var cmb = (ComboBox)sender;
            player = (KeyValuePair<string, string>)cmb.SelectedItem;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string playerdbConnString = $"Data Source={sourcePlayerDbFile};Version=3;New=False;Compress=True;";
                _logger.Debug($"Making connection to {playerdbConnString}");
                using (var sqlConn = new SQLiteConnection(playerdbConnString))
                {
                    sqlConn.Open();

                    //Skills
                    using (var command = sqlConn.CreateCommand())
                    {
                        command.CommandText = $"SELECT Id, Owner, Number, Value, Minvalue FROM SKILLS WHERE OWNER = '{player.Key}'";
                        command.CommandType = CommandType.Text;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var statement = $"INSERT OR REPLACE INTO SKILLS(Id, Owner, Number, Value, Minvalue) VALUES({reader["Id"]}, {reader["Owner"]}, {reader["Number"]}, {reader["Value"]}, {reader["Minvalue"]});";
                                _logger.Debug($"Adding to Skills export: {statement}");
                                sb.AppendLine(statement);
                            }
                        }
                    }

                    //Affinities
                    using (var command = sqlConn.CreateCommand())
                    {
                        sb.AppendLine("***************************AFFINITIES***************************");
                        command.CommandType = CommandType.Text;
                        command.CommandText = $"SELECT WurmID, Skill, Number FROM AFFINITIES WHERE WURMID = '{player.Key}'";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var statement = $"INSERT OR REPLACE INTO AFFINITIES(WurmID, Skill, Number) VALUES({reader["WurmId"]}, {reader["Skill"]}, {reader["Number"]});";
                                _logger.Debug($"Adding to Affinities export: {command.CommandText}");
                                sb.AppendLine(statement);
                            }
                        }
                    }

                    //Titles
                    using (var command = sqlConn.CreateCommand())
                    {
                        sb.AppendLine("***************************TITLES***************************");
                        command.CommandType = CommandType.Text;
                        command.CommandText = $"SELECT WurmID, TitleId, TitleName FROM TITLES WHERE WURMID = '{player.Key}'";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var statement = $"INSERT OR REPLACE INTO TITLES(WurmID, TitleId, TitleName) VALUES({reader["WurmId"]}, {reader["TitleId"]}, \"{reader["TitleName"]}\");";
                                _logger.Debug($"Adding to Titles export: {statement}");
                                sb.AppendLine(statement);
                            }
                        }
                    }

                    //Achievements
                    using (var command = sqlConn.CreateCommand())
                    {
                        sb.AppendLine("***************************ACHIEVEMENTS***************************");
                        command.CommandType = CommandType.Text;
                        command.CommandText = $"SELECT Player, Achievement, Counter, ADate FROM ACHIEVEMENTS WHERE Player = '{player.Key}'";
                        _logger.Debug($"Adding to Achievements export: {command.CommandText}");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var statement = $"INSERT OR REPLACE INTO ACHIEVEMENTS(Player, Achievement, Counter, ADate) VALUES({reader["Player"]}, {reader["Achievement"]}, {reader["Counter"]}, '{Convert.ToDateTime(reader["ADate"]):yyyy-MM-dd HH:mm:ss}');";
                                _logger.Debug($"Adding to Achievements export: {statement}");
                                sb.AppendLine(statement);
                            }
                        }
                    }

                    //Religion
                    using (var command = sqlConn.CreateCommand())
                    {
                        sb.AppendLine("***************************RELIGION***************************");
                        command.CommandType = CommandType.Text;
                        command.CommandText = $"SELECT Faith, Deity, Alignment, God, Favor FROM Players WHERE WurmId = '{player.Key}'";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var statement = $"UPDATE PLAYERS SET Faith = {reader["Faith"]}, Deity = {reader["Deity"]}, Alignment = {reader["Alignment"]}, God = {reader["God"]}, Favor = {reader["Favor"]} WHERE WurmId = {player.Key};";
                                _logger.Debug($"Adding to Religion export: {statement}");
                                sb.AppendLine(statement);
                            }
                        }
                    }
                    sqlConn.Close();
                }

                if (cbxExportInventory.Checked)
                {
                    string itemdbConnString = $"Data Source={sourceItemDbFile};Version=3;New=False;Compress=True;";
                    _logger.Debug($"Making connection to {itemdbConnString}");
                    using (var sqlConn = new SQLiteConnection(itemdbConnString))
                    {
                        sqlConn.Open();

                        //Inventory
                        sb.AppendLine("***************************INVENTORY***************************");
                        //Locate player's container "items"
                        List<string> containerIds = new List<string>();
                        string inventoryId = "-10";
                        using (var command = sqlConn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = $"SELECT WurmId, Name, ParentId from Items WHERE WurmId IN (SELECT DISTINCT ParentId FROM Items WHERE(POSX IS NULL OR POSX = 0) AND OWNERID = {player.Key})";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    sb.AppendLine(reader["Name"].ToString() == "inventory" ? $"--PlayerInventoryId:{reader["WurmId"]}" : $"--PlayerContainerId:{reader["WurmId"]}");
                                    containerIds.Add(reader["WurmId"].ToString());
                                    //Identify the "Inventory" item for the player
                                    if (reader["ParentId"].ToString() == "-10")
                                        inventoryId = reader["WurmId"].ToString();
                                    _logger.Debug($"Found a player container. Id: {reader["WurmId"]} - Name: {reader["Name"]}");
                                }
                            }
                        }

                        //Locate all of a players items (except for settlement deeds)
                        using (var command = sqlConn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = $"SELECT WurmId, TemplateId, Name, Description, QualityLevel, OriginalQualityLevel, Capacity, ParentID, LastMaintained, CreationDate, CreationState, OwnerId, LastOwnerId, Temperature, ZoneId, Damage, SizeX, SizeY, SizeZ, Weight, Material, LockId, Price, Bless, Enchant, Banked, AuxData, RealTemplate, Color, Female, Mailed, MailTimes, Transferred, Creator, Hidden, Rarity, OnBridge, Settings FROM Items WHERE OwnerId='{player.Key}' AND TemplateId <> '663'";
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    //If this item's parent is not a container or -10, then replace its parent with the inventory
                                    //This is necessary for worn items like armor, weapons, backpack, toolbelt.  No way I can find to identify the bodypart to attach it to on the other side
                                    var currentParentId = reader["ParentId"].ToString();
                                    var parentId = (containerIds.Contains(currentParentId) || currentParentId == "-10") ? currentParentId : inventoryId;
                                    var statement = $"INSERT INTO Items(WurmId, TemplateId, Name, Description, QualityLevel, OriginalQualityLevel, Capacity, ParentID, LastMaintained, CreationDate, CreationState, OwnerId, LastOwnerId, Temperature, ZoneId, Damage, SizeX, SizeY, SizeZ, Weight, Material, LockId, Price, Bless, Enchant, Banked, AuxData, RealTemplate, Color, Female, Mailed, MailTimes, Transferred, Creator, Hidden, Rarity, OnBridge, Settings) VALUES({reader["WurmId"]}, {reader["TemplateId"]}, \"{reader["Name"]}\", \"{reader["Description"]}\", {reader["QualityLevel"]}, {reader["OriginalQualityLevel"]}, {reader["Capacity"]}, {parentId}, {reader["LastMaintained"]}, {reader["CreationDate"]}, {reader["CreationState"]}, {reader["OwnerId"]}, {reader["LastOwnerId"]}, {reader["Temperature"]}, {reader["ZoneId"]}, {reader["Damage"]}, {reader["SizeX"]}, {reader["SizeY"]}, {reader["SizeZ"]}, {reader["Weight"]}, {reader["Material"]}, {reader["LockId"]}, {reader["Price"]}, {reader["Bless"]}, {reader["Enchant"]}, {reader["Banked"]}, {reader["AuxData"]}, {reader["RealTemplate"]}, {reader["Color"]}, {reader["Female"]}, {reader["Mailed"]}, {reader["MailTimes"]}, {reader["Transferred"]}, \"{reader["Creator"]}\", {reader["Hidden"]}, {reader["Rarity"]}, {reader["OnBridge"]}, {reader["Settings"]});";
                                    _logger.Debug($"Adding to Inventory export: {statement}");
                                    sb.AppendLine(statement);
                                }
                            }
                        }
                        sqlConn.Close();
                    }
                }

                StreamWriter writer = new StreamWriter($"{player.Value}.txt");
                _logger.Debug($"Writing to {player.Value}.txt: {sb}");
                writer.Write(sb.ToString());
                writer.Dispose();
                _logger.Info($"Created file {player.Value}");
                MessageBox.Show($@"Created file: {player.Value}.txt");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception thrown during export: {ex}");
                MessageBox.Show($@"Error occurred attempting to export {player.Value}: {ex}");
                throw;
            }
        }

        private void btnSelectDBImport_Click(object sender, EventArgs e)
        {
            SelectDestinationPlayerDatabase();
        }

        private void btnSelectCharacter_Click(object sender, EventArgs e)
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(destinationPlayerDBFile))
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
                    //Get player name and originalID from the txt file
                    var reader = new StreamReader(playerFile);
                    var firstLine = reader.ReadLine();
                    var playerId = firstLine.Split(',')[5].Trim();
                    _logger.Debug($"Got a player Id of {playerId} from the first line of the file");
                    var playerName = playerFile.Substring(playerFile.LastIndexOf('\\') + 1);
                    playerName = playerName.Replace(".txt", "");
                    _logger.Debug($"Got a player name of {playerName} from the file name");
                    reader.Close();

                    var playerStringBuilder = new StringBuilder();
                    var itemStringBuilder = new StringBuilder();
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line == "***************************INVENTORY***************************")
                            break;
                        if (!line.StartsWith("***************************"))
                            playerStringBuilder.Append(line);
                    }

                    Int64 newId;
                    using (var conn = new SQLiteConnection($"Data Source={destinationPlayerDBFile};Version=3;New=False;Compress=True;"))
                    {
                        _logger.Debug($"Opening connection to {conn}");
                        conn.Open();
                        var command = new SQLiteCommand(conn)
                        {
                            CommandText = $"SELECT WURMID FROM PLAYERS WHERE LOWER(Name) = '{playerName.ToLower()}'",
                            CommandType = CommandType.Text
                        };
                        
                        newId = Convert.ToInt64(command.ExecuteScalar());
                        
                        if (newId == 0)
                        {
                            MessageBox.Show(@"Player not found in destination DB, create the character before attempting to import.");
                            _logger.Warn($"The player name {playerName} was not found in {destinationPlayerDBFile}");
                            return;
                        }
                        _logger.Debug($"Got a WurmId of {newId} from {destinationPlayerDBFile}");

                        command.CommandText = playerStringBuilder.ToString().Replace(playerId, newId.ToString());
                        _logger.Debug($"Sending the following command to PlayersDB: {command.CommandText}");
                        command.ExecuteNonQuery();

                        conn.Close();
                    }

                    using (var conn = new SQLiteConnection($"Data Source={destItemDbFile};Version=3;New=False;Compress=True;"))
                    {
                        _logger.Debug($"Opening connection to {conn}");
                        conn.Open();

                        var itemCommand = new SQLiteCommand(conn)
                        {
                            CommandType = CommandType.Text,
                            CommandText = "SELECT MAX(WurmID) FROM Items"
                        };
                        var maxId = Convert.ToInt64(itemCommand.ExecuteScalar());
                        
                        itemCommand.CommandText = $"SELECT WurmId FROM Items WHERE Name='inventory' AND OwnerId={newId}";
                        var newInventoryId = itemCommand.ExecuteScalar().ToString();
                        _logger.Debug($"Found an inventory Id of {newInventoryId}");

                        string oldInventoryId = string.Empty;
                        List<string> containerIds = new List<string>();
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (!line.StartsWith("--"))
                            {
                                var idPart = line.Split(',')[37];
                                var currentId = idPart.Substring(idPart.IndexOf("(") + 1);
                                //Replace the Id with MAX(ID)+1 if this isnt a container
                                if (containerIds.Contains(currentId))
                                {
                                    itemStringBuilder.AppendLine(line);
                                } else if(!line.Contains("\"inventory\","))
                                {
                                    maxId++;
                                    itemStringBuilder.AppendLine(line.Replace(currentId, maxId.ToString()));
                                }
                            } else
                            {
                                if (line.Contains("PlayerInventoryId"))
                                {   //Dont add this item to the container list.  Just note the old Id so we can replace it later
                                    oldInventoryId = line.Substring(line.IndexOf(":") + 1);
                                } else
                                {
                                    containerIds.Add(line.Substring(line.IndexOf(":") + 1));
                                }
                            }
                        }
                        
                        for (int i = 1; i <= containerIds.Count; i++)
                        {
                            itemStringBuilder.Replace(containerIds[i - 1], (maxId + i).ToString());
                        }

                        //Replace the old inventory Id with the new one
                        itemStringBuilder.Replace(oldInventoryId, newInventoryId);

                        itemCommand.CommandText = itemStringBuilder.ToString().Replace(playerId, newId.ToString()).Replace(" ,", " NULL,");

                        _logger.Debug($"Sending the following command to ItemsDB: {itemCommand.CommandText}");
                        itemCommand.ExecuteNonQuery();

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
            SelectDestinationPlayerDatabase();
        }

        private void SelectDestinationPlayerDatabase()
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
                    destinationPlayerDBFile = openFileDialog2.FileName;
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
            if (string.IsNullOrEmpty(destinationPlayerDBFile))
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
                    using (var conn = new SQLiteConnection($"Data Source={destinationPlayerDBFile};Version=3;New=False;Compress=True;"))
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
                            _logger.Warn($"The player name {playerName} was not found in {destinationPlayerDBFile}");
                            return;
                        }
                        _logger.Debug($"Got a WurmId of {newId} from {destinationPlayerDBFile}");

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

        private void cbxExportInventory_CheckedChanged(object sender, EventArgs e)
        {
            btnSelectItemDB.Visible = cbxExportInventory.Checked;
        }

        private void btnSelectItemDB_Click(object sender, EventArgs e)
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
                    sourceItemDbFile = openFileDialog2.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnImportItemDb_Click(object sender, EventArgs e)
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
                    destItemDbFile = openFileDialog2.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}