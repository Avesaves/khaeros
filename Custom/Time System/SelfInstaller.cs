using System;
using System.Text;
using System.IO;
using Server;

namespace Server.TimeSystem
{
    public class SelfInstaller
    {
        #region Constant Variables

        private const bool m_Enabled = false;

        private const bool m_UpdateSelfInstaller = true;
        private const bool m_UpdateLightCycle = true;
        private const bool m_UpdateClocks = true;
        private const bool m_UpdateSpyglass = true;
        private const bool m_UpdateNightSightSpell = true;
        private const bool m_UpdateNightSightPotion = true;
        private const bool m_UpdatePlayerMobile = true;
        private const bool m_UpdateBaseCreature = true;
        private const bool m_UpdateBaseAI = true;
        private const bool m_UpdateEvilSpirit = true;
        private const bool m_UpdateNonRedMageAI = true;

        //private const string m_ExpectedVersion = "2.0 2357.32527"; // Major.Minor Build.Revision
		private const string m_ExpectedVersion = "2.0 2823.41150";
        private static readonly string m_ModdedPath = Path.Combine(Core.BaseDirectory, @"Scripts\~Modded");

        private static readonly string m_LightCycleFile = Path.Combine(Core.BaseDirectory, @"Scripts\Misc\LightCycle.cs");
        private static readonly string m_ClocksFile = Path.Combine(Core.BaseDirectory, @"Scripts\Items\Skill Items\Tinkering\Clocks.cs");
        private static readonly string m_SpyglassFile = Path.Combine(Core.BaseDirectory, @"Scripts\Items\Skill Items\Tinkering\Spyglass.cs");
        private static readonly string m_NightSightSpellFile = Path.Combine(Core.BaseDirectory, @"Scripts\Spells\First\NightSight.cs");
        private static readonly string m_NightSightPotionFile = Path.Combine(Core.BaseDirectory, @"Scripts\Items\Skill Items\Magical\Potions\NightSight.cs");
        private static readonly string m_PlayerMobileFile = Path.Combine(Core.BaseDirectory, @"Scripts\Mobiles\PlayerMobile.cs");
        private static readonly string m_BaseCreatureFile = Path.Combine(Core.BaseDirectory, @"Scripts\Engines\AI\Creature\BaseCreature.cs");
        private static readonly string m_BaseAIFile = Path.Combine(Core.BaseDirectory, @"Scripts\Engines\AI\AI\BaseAI.cs");

        private const string m_BeginEdit = "// ** EDIT ** Time System";
        private const string m_EndEdit = "// ** END *** Time System";

        #endregion

        #region Private Variables

        private static bool m_Updated = false;

        #endregion

        #region Get Methods

        private static string GetAssemblyVersion()
        {
            Version version = Core.Assembly.GetName().Version;

            return String.Format("{0}.{1} {2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private static string GetDirectoryMinusBase(string directory)
        {
            return directory.Replace(String.Format("{0}\\Scripts\\", Core.BaseDirectory), String.Empty);
        }

        private static string GetDirectoryMinusBaseAndFileName(string fileName)
        {
            return fileName.Replace(String.Format("\\{0}",GetFileName(fileName)),String.Empty).Replace(String.Format("{0}\\Scripts\\", Core.BaseDirectory), String.Empty);
        }

        private static string GetFileName(string fileName)
        {
            string[] fileNameSplit = fileName.Split('\\');

            return fileNameSplit[fileNameSplit.Length - 1];
        }

        private static void FindFile(string dir, string fileName, ref string fullFileName)
        {
            DirectoryInfo di = null;
            FileInfo[] fis = null;
            DirectoryInfo[] dis = null;

            if (fullFileName != null)
            {
                return;
            }

            try
            {
                di = new DirectoryInfo(dir);
                fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    if (fi.Name == fileName)
                    {
                        fullFileName = fi.FullName;

                        return;
                    }
                }

                dis = di.GetDirectories();

                foreach (DirectoryInfo directory in dis)
                {
                    FindFile(directory.FullName, fileName, ref fullFileName);
                }
            }
            catch { }
        }

        #endregion

        #region Check Methods

        private static bool IsExpectedVersion()
        {
            string version = GetAssemblyVersion();

            if (m_ExpectedVersion != version)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void CheckPaths()
        {
            string newLightCyclePath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_LightCycleFile));
            string newClocksPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_ClocksFile));
            string newSpyglassPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_SpyglassFile));
            string newNightSightSpellPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_NightSightSpellFile));
            string newNightSightPotionPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_NightSightPotionFile));
            string newPlayerMobilePath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_PlayerMobileFile));
            string newBaseCreaturePath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_BaseCreatureFile));
            string newBaseAIPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_BaseAIFile));

            CreateDirectory(m_ModdedPath);
            CreateDirectory(newLightCyclePath);
            CreateDirectory(newClocksPath);
            CreateDirectory(newSpyglassPath);
            CreateDirectory(newNightSightSpellPath);
            CreateDirectory(newNightSightPotionPath);
            CreateDirectory(newPlayerMobilePath);
            CreateDirectory(newBaseCreaturePath);
            CreateDirectory(newBaseAIPath);
        }

        private static void CreateDirectory(string directory)
        {
            string directoryMinusBase = GetDirectoryMinusBase(directory);
            string baseDirectory = directory.Replace(directoryMinusBase, String.Empty);

            string[] directorySplit = directoryMinusBase.Split('\\');

            string combineDirectory = String.Empty;

            for (int i = 0; i < directorySplit.Length; i++)
            {
                if (i > 0)
                {
                    combineDirectory = String.Format("{0}\\{1}", combineDirectory, directorySplit[i]);
                }
                else
                {
                    combineDirectory = directorySplit[i];
                }

                string checkDirectory = Path.Combine(baseDirectory, combineDirectory);

                if (i > 0)
                {
                    if (!Directory.Exists(checkDirectory))
                    {
                        Directory.CreateDirectory(checkDirectory);
                    }
                }
            }
        }

        #endregion

        #region Installer

        public static void Install()
        {
            bool enabled = m_Enabled;

            if (enabled)
            {
                Support.ConsoleWriteLine("Time System: SelfInstaller: Executing...");

                if (!IsExpectedVersion())
                {
                    Support.ConsoleWriteLine("Time System: SelfInstaller: RunUO version is different than expected!  SelfInstaller cancelled!");
                }
                else
                {
                    CheckPaths();

                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateLightCycleFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateClocksFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateSpyglassFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateNightSightSpellFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateNightSightPotionFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdatePlayerMobileFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateBaseCreatureFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateBaseAIFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateEvilSpiritFile()));
                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateNonRedMageAIFile()));

                    Support.ConsoleWriteLine(String.Format("Time System: SelfInstaller: {0}", UpdateSelfInstallerFile()));

                    Support.ConsoleWriteLine("Time System: SelfInstaller: Execution has completed.");

                    if (m_Updated)
                    {
                        Support.ConsoleWriteLine("Time System: SelfInstaller: Please restart your server for changes to take effect!");
                    }
                }
            }
            else
            {
                Support.ConsoleWriteLine("Time System: SelfInstaller is disabled.");
            }
        }

        private static void ReadWrite(StreamReader r, StreamWriter w)
        {
            if (r.Peek() > 0)
            {
                w.WriteLine(r.ReadLine());
            }
        }

        #endregion

        #region Update SelfInstaller.cs

        private static string UpdateSelfInstallerFile()
        {
            bool enabled = m_UpdateSelfInstaller;

            if (!enabled)
            {
                return "SelfInstaller.cs has been set to skip the update!";
            }

            string fileName = null;

            FindFile(String.Format("{0}\\Scripts\\", Core.BaseDirectory), "SelfInstaller.cs", ref fileName);

            StreamReader r = new StreamReader(fileName);

            while (r.Peek() > 0)
            {
                string read = r.ReadLine();

                if (read == "private const bool m_Enabled = false;")
                {
                    r.Close();

                    return "SelfInstaller.cs already updated!";
                }
            }

            r.Close();

            string tempFileName = fileName.Replace("SelfInstaller.cs", "SelfInstaller.Temp.cs");

            File.Move(fileName, tempFileName);

            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false, updated = false;

                r = new StreamReader(tempFileName);
                w = new StreamWriter(fileName);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (!updated && read.IndexOf("private const bool m_Enabled = true;") > -1)
                    {
                        ignoreNextWrite = true;
                        updated = true;

                        read = read.Replace("private const bool m_Enabled = true;", "private const bool m_Enabled = false;");

                        w.WriteLine(read);
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Delete(tempFileName);
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("SelfInstaller.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "SelfInstaller.cs updated successfully."; ;
        }

        #endregion

        #region Update LightCycle.cs

        private static string UpdateLightCycleFile()
        {
            bool enabled = m_UpdateLightCycle;

            if (!enabled)
            {
                return "LightCycle.cs has been set to skip the update!";
            }

            string newLightCyclePath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_LightCycleFile));
            string newLightCycleFile = Path.Combine(newLightCyclePath, GetFileName(m_LightCycleFile));

            if (File.Exists(newLightCycleFile))
            {
                return "LightCycle.cs already updated!";
            }

            if (!File.Exists(m_LightCycleFile))
            {
                return "Original LightCycle.cs not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_LightCycleFile);
                w = new StreamWriter(newLightCycleFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("public static void Initialize()") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();

                        string line = r.ReadLine();

                        if (line.IndexOf("new LightCycleTimer().Start();") > -1)
                        {
                            w.WriteLine(line.Replace("new LightCycleTimer().Start();", "//new LightCycleTimer().Start();"));
                        }

                        line = r.ReadLine();

                        if (line.IndexOf("EventSink.Login += new LoginEventHandler( OnLogin );") > -1)
                        {
                            w.WriteLine(line.Replace("EventSink.Login += new LoginEventHandler( OnLogin );", "//EventSink.Login += new LoginEventHandler( OnLogin );"));
                        }

                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                    }

                    if (read.IndexOf("public static int ComputeLevelFor( Mobile from )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        string line = r.ReadLine();

                        while (line.IndexOf("return m_LevelOverride;") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);

                        ReadWrite(r, w);

                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine("\t\t\treturn TimeSystem.TimeEngine.CalculateLightLevel(from);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t/*");

                        line = r.ReadLine();

                        while (line.IndexOf("return NightLevel; // should never be") == -1)
                        {
                            if (line.IndexOf("/*") > -1)
                            {
                                w.WriteLine(line.Replace("/*", "//"));
                            }
                            else if (line.IndexOf("*/") > -1)
                            {
                                w.WriteLine(line.Replace("*/", "//"));
                            }
                            else
                            {
                                w.WriteLine(line);
                            }

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);
                        w.WriteLine("\t\t\t*/");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (read.IndexOf("public class NightSightTimer : Timer") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        string line = r.ReadLine();

                        while (line.IndexOf("BuffInfo.RemoveBuff( m_Owner, BuffIcon.NightSight );") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine("\t\t\t\tTimeSystem.EffectsEngine.SetNightSightOff(m_Owner);");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_LightCycleFile, String.Format("{0}_old",m_LightCycleFile));
            }
            catch(Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("LightCycle.cs update failed!\r\n{0}",e.ToString());
            }

            m_Updated = true;

            return "LightCycle.cs updated successfully.";
        }

        #endregion

        #region Update Clocks.cs

        private static string UpdateClocksFile()
        {
            bool enabled = m_UpdateClocks;

            if (!enabled)
            {
                return "Clocks.cs has been set to skip the update!";
            }

            string newClocksPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_ClocksFile));
            string newClocksFile = Path.Combine(newClocksPath, GetFileName(m_ClocksFile));

            if (File.Exists(newClocksFile))
            {
                return "Clocks.cs already updated!";
            }

            if (!File.Exists(m_ClocksFile))
            {
                return "Original Clocks.cs not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_ClocksFile);
                w = new StreamWriter(newClocksFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("using Server;") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine("using Server.Mobiles;");
                        w.WriteLine("using Server.Network;");
                        w.WriteLine(m_EndEdit);
                    }

                    if (read.IndexOf("public static void GetTime( Map map, int x, int y, out int hours, out int minutes, out int totalMinutes )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine("\t\t\ttotalMinutes = 0;");
                        w.WriteLine();
                        w.WriteLine("\t\t\tTimeSystem.TimeEngine.GetTimeMinHour(map, x, out minutes, out hours);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t/*");

                        string line = r.ReadLine();

                        while (line.IndexOf("minutes = totalMinutes % 60;") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);
                        w.WriteLine("\t\t\t*/");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (read.IndexOf("public override void OnDoubleClick( Mobile from )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine("\t\t\tTimeSystem.Support.SendClockData(this, from);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t/*");

                        string line = r.ReadLine();

                        while (line.IndexOf("SendLocalizedMessageTo( from, 1042958, exactTime ); // ~1_TIME~ to be exact") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);
                        w.WriteLine("\t\t\t*/");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_ClocksFile, String.Format("{0}_old", m_ClocksFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("Clocks.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "Clocks.cs updated successfully.";
        }

        #endregion

        #region Update Spyglass.cs

        private static string UpdateSpyglassFile()
        {
            bool enabled = m_UpdateSpyglass;

            if (!enabled)
            {
                return "Spyglass.cs has been set to skip the update!";
            }

            string newSpyglassPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_SpyglassFile));
            string newSpyglassFile = Path.Combine(newSpyglassPath, GetFileName(m_SpyglassFile));

            if (File.Exists(newSpyglassFile))
            {
                return "Spyglass.cs already updated!";
            }

            if (!File.Exists(m_SpyglassFile))
            {
                return "Original Spyglass.cs not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_SpyglassFile);
                w = new StreamWriter(newSpyglassFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("public override void OnDoubleClick( Mobile from )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine("\t\t\tTimeSystem.Support.SendSpyglassData(from);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t/*");

                        string line = r.ReadLine();

                        while (line.IndexOf("from.Send( new MessageLocalizedAffix( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 1008146 + (int)Clock.GetMoonPhase( Map.Felucca, from.X, from.Y ), \"\", AffixType.Prepend, \"Felucca : \", \"\" ) );") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);
                        w.WriteLine("\t\t\t*/");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_SpyglassFile, String.Format("{0}_old", m_SpyglassFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("Spyglass.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "Spyglass.cs updated successfully.";
        }

        #endregion

        #region Update NightSight.cs (spell)

        private static string UpdateNightSightSpellFile()
        {
            bool enabled = m_UpdateNightSightSpell;

            if (!enabled)
            {
                return "NightSight.cs (spell) has been set to skip the update!";
            }

            string newNightSightSpellPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_NightSightSpellFile));
            string newNightSightSpellFile = Path.Combine(newNightSightSpellPath, GetFileName(m_NightSightSpellFile));

            if (File.Exists(newNightSightSpellFile))
            {
                return "NightSight.cs (spell) already updated!";
            }

            if (!File.Exists(m_NightSightSpellFile))
            {
                return "Original NightSight.cs (spell) not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_NightSightSpellFile);
                w = new StreamWriter(newNightSightSpellFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("private class NightSightTarget : Target") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        string line = r.ReadLine();

                        while (line.IndexOf("if ( targ.BeginAction( typeof( LightCycle ) ) )") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);

                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();

                        line = r.ReadLine();

                        if (line.IndexOf("new LightCycle.NightSightTimer( targ ).Start();") > -1)
                        {
                            w.WriteLine(line.Replace("new LightCycle.NightSightTimer( targ ).Start();", "//new LightCycle.NightSightTimer( targ ).Start();"));
                        }

                        line = r.ReadLine();

                        while (line.IndexOf("targ.LightLevel = level;") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine("\t\t\t\t\t\tint oldLevel = level;");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\t\t\tlevel = TimeSystem.EffectsEngine.GetNightSightLevel(targ, level);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\t\t\tif (level > -1)");
                        w.WriteLine("\t\t\t\t\t\t{");
                        w.WriteLine(String.Format("\t{0}", line));

                        line = r.ReadLine();

                        while (line.IndexOf("BuffInfo.AddBuff( targ, new BuffInfo( BuffIcon.NightSight, 1075643 ) );	//Night Sight/You ignore lighting effects") == -1)
                        {
                            w.WriteLine(String.Format("\t{0}", line));

                            line = r.ReadLine();
                        }

                        w.WriteLine(String.Format("\t{0}", line));
                        w.WriteLine();
                        w.WriteLine("\t\t\t\t\t\t\tTimeSystem.EffectsEngine.SetNightSightOn(targ, oldLevel);");
                        w.WriteLine("\t\t\t\t\t\t}");
                        w.WriteLine("\t\t\t\t\t\telse");
                        w.WriteLine("\t\t\t\t\t\t{");
                        w.WriteLine("\t\t\t\t\t\t\ttarg.EndAction(typeof(LightCycle));");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\t\t\t\tfrom.SendMessage(\"Your spell seems to have no effect.\");");
                        w.WriteLine("\t\t\t\t\t\t}");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_NightSightSpellFile, String.Format("{0}_old", m_NightSightSpellFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("NightSight.cs (spell) update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "NightSight.cs (spell) updated successfully.";
        }

        #endregion

        #region Update NightSight.cs (potion)

        private static string UpdateNightSightPotionFile()
        {
            bool enabled = m_UpdateNightSightPotion;

            if (!enabled)
            {
                return "NightSight.cs (potion) has been set to skip the update!";
            }

            string newNightSightPotionPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_NightSightPotionFile));
            string newNightSightPotionFile = Path.Combine(newNightSightPotionPath, GetFileName(m_NightSightPotionFile));

            if (File.Exists(newNightSightPotionFile))
            {
                return "NightSight.cs (potion) already updated!";
            }

            if (!File.Exists(m_NightSightPotionFile))
            {
                return "Original NightSight.cs (potion) not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_NightSightPotionFile);
                w = new StreamWriter(newNightSightPotionFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("public override void Drink( Mobile from )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);
                        ReadWrite(r, w);
                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();

                        string line = r.ReadLine();

                        if(line.IndexOf("new LightCycle.NightSightTimer( from ).Start();") > -1)
                        {
                            w.WriteLine(line.Replace("new LightCycle.NightSightTimer( from ).Start();", "//new LightCycle.NightSightTimer( from ).Start();"));
                        }

                        line = r.ReadLine();

                        if (line.IndexOf("from.LightLevel = LightCycle.DungeonLevel / 2;") > -1)
                        {
                            w.WriteLine(line.Replace("from.LightLevel = LightCycle.DungeonLevel / 2;", "//from.LightLevel = LightCycle.DungeonLevel / 2;"));
                        }

                        line = r.ReadLine();

                        while (line.IndexOf("this.Consume();") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);

                        w.WriteLine();
                        w.WriteLine("\t\t\t\tint oldLevel = LightCycle.DungeonLevel / 2;");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\tint level = TimeSystem.EffectsEngine.GetNightSightLevel(from, oldLevel);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\tif (level > -1)");
                        w.WriteLine("\t\t\t\t{");
                        w.WriteLine("\t\t\t\t\tfrom.LightLevel = level;");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\t\tTimeSystem.EffectsEngine.SetNightSightOn(from, oldLevel);");
                        w.WriteLine("\t\t\t\t}");
                        w.WriteLine("\t\t\t\telse");
                        w.WriteLine("\t\t\t\t{");
                        w.WriteLine("\t\t\t\t\tfrom.EndAction(typeof(LightCycle));");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\t\tfrom.SendMessage(\"The potion seems to have no effect.\");");
                        w.WriteLine("\t\t\t\t}");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_NightSightPotionFile, String.Format("{0}_old", m_NightSightPotionFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("NightSight.cs (potion) update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "NightSight.cs (potion) updated successfully.";
        }

        #endregion

        #region Update PlayerMobile.cs

        private static string UpdatePlayerMobileFile()
        {
            bool enabled = m_UpdatePlayerMobile;

            if (!enabled)
            {
                return "PlayerMobile.cs has been set to skip the update!";
            }

            string newPlayerMobilePath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_PlayerMobileFile));
            string newPlayerMobileFile = Path.Combine(newPlayerMobilePath, GetFileName(m_PlayerMobileFile));

            if (File.Exists(newPlayerMobileFile))
            {
                return "PlayerMobile.cs already updated!";
            }

            if (!File.Exists(m_PlayerMobileFile))
            {
                return "Original PlayerMobile.cs not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_PlayerMobileFile);
                w = new StreamWriter(newPlayerMobileFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("public override void ComputeBaseLightLevels( out int global, out int personal )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);
                        ReadWrite(r, w);
                        ReadWrite(r, w);

                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();

                        string line = r.ReadLine();

                        if (line.IndexOf("if ( this.LightLevel < 21 && AosAttributes.GetValue( this, AosAttribute.NightSight ) > 0 )") > -1)
                        {
                            w.WriteLine(line.Replace("if ( this.LightLevel < 21 && AosAttributes.GetValue( this, AosAttribute.NightSight ) > 0 )", "/*if ( this.LightLevel < 21 && AosAttributes.GetValue( this, AosAttribute.NightSight ) > 0 )"));
                        }

                        line = r.ReadLine();

                        while (line.IndexOf("personal = this.LightLevel;") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        if (line.IndexOf("personal = this.LightLevel;") > -1)
                        {
                            w.WriteLine(line.Replace("personal = this.LightLevel;", "personal = this.LightLevel;*/"));
                        }

                        w.WriteLine();
                        w.WriteLine("\t\t\tif (this.LightLevel < 21 && AosAttributes.GetValue(this, AosAttribute.NightSight) > 0)");
                        w.WriteLine("\t\t\t{");
                        w.WriteLine("\t\t\t\tint level = TimeSystem.EffectsEngine.GetNightSightLevel(this, 21);");
                        w.WriteLine();
                        w.WriteLine("\t\t\t\tif (level > -1)");
                        w.WriteLine("\t\t\t\t{");
                        w.WriteLine("\t\t\t\t\tpersonal = level;");
                        w.WriteLine("\t\t\t\t}");
                        w.WriteLine("\t\t\t\telse");
                        w.WriteLine("\t\t\t\t{");
                        w.WriteLine("\t\t\t\t\tpersonal = 0;");
                        w.WriteLine("\t\t\t\t}");
                        w.WriteLine("\t\t\t}");
                        w.WriteLine("\t\t\telse");
                        w.WriteLine("\t\t\t{");
                        w.WriteLine("\t\t\t\tpersonal = this.LightLevel;");
                        w.WriteLine("\t\t\t}");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_PlayerMobileFile, String.Format("{0}_old", m_PlayerMobileFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("PlayerMobile.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "PlayerMobile.cs updated successfully.";
        }

        #endregion

        #region Update BaseCreature.cs

        private static string UpdateBaseCreatureFile()
        {
            bool enabled = m_UpdateBaseCreature;

            if (!enabled)
            {
                return "BaseCreature.cs has been set to skip the update!";
            }

            string newBaseCreaturePath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_BaseCreatureFile));
            string newBaseCreatureFile = Path.Combine(newBaseCreaturePath, GetFileName(m_BaseCreatureFile));

            if (File.Exists(newBaseCreatureFile))
            {
                return "BaseCreature.cs already updated!";
            }

            if (!File.Exists(m_BaseCreatureFile))
            {
                return "Original BaseCreature.cs not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_BaseCreatureFile);
                w = new StreamWriter(newBaseCreatureFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("public enum FightMode") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        string line = r.ReadLine();

                        while (line.IndexOf("Evil			// Only attack aggressor -or- negative karma") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        if (line.IndexOf("Evil			// Only attack aggressor -or- negative karma") > -1)
                        {
                            line = line.Replace("Evil			// Only attack aggressor -or- negative karma", "Evil,			// Only attack aggressor -or- negative karma");
                        }
                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine(line);
                        w.WriteLine("\t\tNonRed\t\t\t// Only attack non-red mobiles");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (read.IndexOf("public  void ChangeAIType( AIType NewAI )") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        string line = r.ReadLine();

                        while (line.IndexOf("case AIType.AI_Thief:") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        w.WriteLine(line);

                        ReadWrite(r, w);
                        ReadWrite(r, w);

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine("\t\t\t\tcase AIType.AI_NonRedMage:");
                        w.WriteLine("\t\t\t\t\tm_AI = new NonRedMageAI(this);");
                        w.WriteLine("\t\t\t\t\tbreak;");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_BaseCreatureFile, String.Format("{0}_old", m_BaseCreatureFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("BaseCreature.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "BaseCreature.cs updated successfully.";
        }

        #endregion

        #region Update BaseAI.cs

        private static string UpdateBaseAIFile()
        {
            bool enabled = m_UpdateBaseAI;

            if (!enabled)
            {
                return "BaseAI.cs has been set to skip the update!";
            }

            string newBaseAIPath = Path.Combine(m_ModdedPath, GetDirectoryMinusBaseAndFileName(m_BaseAIFile));
            string newBaseAIFile = Path.Combine(newBaseAIPath, GetFileName(m_BaseAIFile));

            if (File.Exists(newBaseAIFile))
            {
                return "BaseAI.cs already updated!";
            }

            if (!File.Exists(m_BaseAIFile))
            {
                return "Original BaseAI.cs not found!";
            }

            StreamReader r = null;
            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(m_BaseAIFile);
                w = new StreamWriter(newBaseAIFile);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("public enum AIType") > -1)
                    {
                        ignoreNextWrite = true;

                        w.WriteLine(read);

                        ReadWrite(r, w);

                        string line = r.ReadLine();

                        while (line.IndexOf("AI_Thief") == -1)
                        {
                            w.WriteLine(line);

                            line = r.ReadLine();
                        }

                        if (line.IndexOf("AI_Thief") > -1)
                        {
                            line = line.Replace("AI_Thief", "AI_Thief,");
                        }

                        w.WriteLine();
                        w.WriteLine(m_BeginEdit);
                        w.WriteLine();
                        w.WriteLine(line);

                        w.WriteLine("\t\tAI_NonRedMage");
                        w.WriteLine();
                        w.WriteLine(m_EndEdit);
                        w.WriteLine();
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Move(m_BaseAIFile, String.Format("{0}_old", m_BaseAIFile));
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("BaseAI.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "BaseAI.cs updated successfully.";
        }

        #endregion

        #region Update EvilSpirit.cs

        private static string UpdateEvilSpiritFile()
        {
            bool enabled = m_UpdateEvilSpirit;

            if (!enabled)
            {
                return "EvilSpirit.cs has been set to skip the update!";
            }

            string fileName = null;

            FindFile(String.Format("{0}\\Scripts\\", Core.BaseDirectory), "EvilSpirit.cs", ref fileName);

            StreamReader r = new StreamReader(fileName);

            while (r.Peek() > 0)
            {
                string read = r.ReadLine();

                if (read == "#define UseNonRedMageAI")
                {
                    r.Close();

                    return "EvilSpirit.cs already updated!";
                }
            }

            r.Close();

            string tempFileName = fileName.Replace("EvilSpirit.cs", "EvilSpirit.Temp.cs");

            File.Move(fileName, tempFileName);

            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(tempFileName);
                w = new StreamWriter(fileName);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("//#define UseNonRedMageAI") > -1)
                    {
                        ignoreNextWrite = true;

                        read = read.Replace("//#define UseNonRedMageAI", "#define UseNonRedMageAI");

                        w.WriteLine(read);
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Delete(tempFileName);
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("EvilSpirit.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "EvilSpirit.cs updated successfully."; ;
        }

        #endregion

        #region Update NonRedMageAI.cs

        private static string UpdateNonRedMageAIFile()
        {
            bool enabled = m_UpdateNonRedMageAI;

            if (!enabled)
            {
                return "NonRedMageAI.cs has been set to skip the update!";
            }

            string fileName = null;

            FindFile(String.Format("{0}\\Scripts\\", Core.BaseDirectory), "NonRedMageAI.cs", ref fileName);

            StreamReader r = new StreamReader(fileName);

            while (r.Peek() > 0)
            {
                string read = r.ReadLine();

                if (read == "#define UseNonRedMageAI")
                {
                    r.Close();

                    return "NonRedMageAI.cs already updated!";
                }
            }

            r.Close();

            string tempFileName = fileName.Replace("NonRedMageAI.cs", "NonRedMageAI.Temp.cs");

            File.Move(fileName, tempFileName);

            StreamWriter w = null;

            try
            {
                bool ignoreNextWrite = false;

                r = new StreamReader(tempFileName);
                w = new StreamWriter(fileName);

                while (r.Peek() > 0)
                {
                    string read = r.ReadLine();

                    if (read.IndexOf("//#define UseNonRedMageAI") > -1)
                    {
                        ignoreNextWrite = true;

                        read = read.Replace("//#define UseNonRedMageAI", "#define UseNonRedMageAI");

                        w.WriteLine(read);
                    }

                    if (!ignoreNextWrite)
                    {
                        w.WriteLine(read);
                    }
                    else
                    {
                        ignoreNextWrite = false;
                    }
                }

                r.Close();
                w.Close();

                File.Delete(tempFileName);
            }
            catch (Exception e)
            {
                if (r != null)
                {
                    r.Close();
                }

                if (w != null)
                {
                    w.Close();
                }

                return String.Format("NonRedMageAI.cs update failed!\r\n{0}", e.ToString());
            }

            m_Updated = true;

            return "NonRedMageAI.cs updated successfully."; ;
        }

        #endregion
    }
}
