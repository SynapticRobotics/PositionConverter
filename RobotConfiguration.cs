using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace FanucUtilities
{
    /// <summary>
    /// Represents the Denavit-Hartenberg parameters for a robot
    /// </summary>
    public class DHParameters
    {
        public double j1LinkA { get; set; }
        public double j2LinkA { get; set; }
        public double j3LinkA { get; set; }
        public double j4LinkD { get; set; }
        public double facePlateThickness { get; set; }
    }

    /// <summary>
    /// Represents a single robot configuration
    /// </summary>
    public class RobotConfig
    {
        public string name { get; set; }
        public string matchPattern { get; set; }
        public string specificPattern { get; set; }
        public string excludePattern { get; set; }
        public DHParameters dhParameters { get; set; }
    }

    /// <summary>
    /// Root configuration object
    /// </summary>
    public class RobotConfigurationFile
    {
        public List<RobotConfig> robots { get; set; }
    }

    /// <summary>
    /// Loads and manages robot configurations from JSON file
    /// </summary>
    public class RobotConfigurationLoader
    {
        private static RobotConfigurationFile _configuration;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Loads the robot configuration file. Thread-safe singleton pattern.
        /// </summary>
        public static RobotConfigurationFile LoadConfiguration(string configPath = null)
        {
            if (_configuration != null)
                return _configuration;

            lock (_lockObject)
            {
                if (_configuration != null)
                    return _configuration;

                // Determine config file path
                if (string.IsNullOrEmpty(configPath))
                {
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string exeDir = Path.GetDirectoryName(exePath);
                    configPath = Path.Combine(exeDir, "RobotConfigurations.json");
                }

                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException($"Robot configuration file not found: {configPath}");
                }

                try
                {
                    string jsonContent = File.ReadAllText(configPath);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    _configuration = serializer.Deserialize<RobotConfigurationFile>(jsonContent);

                    if (_configuration == null || _configuration.robots == null || _configuration.robots.Count == 0)
                    {
                        throw new Exception("Robot configuration file is empty or invalid");
                    }

                    return _configuration;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to load robot configuration: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Finds a robot configuration based on the robot type string from the backup file.
        /// Uses pattern matching similar to the original SetDHParams logic.
        /// </summary>
        public static DHParameters FindRobotConfiguration(string robotType)
        {
            if (_configuration == null)
            {
                LoadConfiguration();
            }

            if (robotType == null)
            {
                throw new ArgumentNullException(nameof(robotType));
            }

            // Try to find a matching configuration
            // Process in order, checking more specific patterns first
            var sortedConfigs = _configuration.robots
                .OrderByDescending(r => !string.IsNullOrEmpty(r.specificPattern) ? r.specificPattern.Length : 0)
                .ThenByDescending(r => !string.IsNullOrEmpty(r.matchPattern) ? r.matchPattern.Length : 0);

            foreach (var config in sortedConfigs)
            {
                bool matchesMain = string.IsNullOrEmpty(config.matchPattern) || robotType.Contains(config.matchPattern);
                bool matchesSpecific = string.IsNullOrEmpty(config.specificPattern) || robotType.Contains(config.specificPattern);
                bool matchesExclude = !string.IsNullOrEmpty(config.excludePattern) && robotType.Contains(config.excludePattern);

                if (matchesMain && matchesSpecific && !matchesExclude)
                {
                    return config.dhParameters;
                }
            }

            // No match found
            throw new ArgumentException($"Unsupported Fanuc Robot Type: {robotType}. " +
                $"To add support for this robot, edit RobotConfigurations.json and add the DH parameters.");
        }

        /// <summary>
        /// Reloads the configuration from disk. Useful if the configuration file has been modified.
        /// </summary>
        public static void ReloadConfiguration(string configPath = null)
        {
            lock (_lockObject)
            {
                _configuration = null;
                LoadConfiguration(configPath);
            }
        }

        /// <summary>
        /// Gets all available robot configurations
        /// </summary>
        public static List<RobotConfig> GetAllConfigurations()
        {
            if (_configuration == null)
            {
                LoadConfiguration();
            }
            return _configuration.robots;
        }
    }
}
