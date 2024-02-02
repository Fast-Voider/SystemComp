using System;
using System.Management;

class Program
{
    static void Main()
    {
        Console.WriteLine("Listing Computer Components:");

        DisplayComponentInformation("Win32_ComputerSystem", "Manufacturer", "Model");

        Console.WriteLine("\nList of Processors (CPUs):");
        DisplayComponentInformation("Win32_Processor", "Manufacturer", "Name", "NumberOfCores");

        Console.WriteLine("\nList of GPUs:");
        DisplayComponentInformation("Win32_VideoController", "AdapterCompatibility", "Name");

        Console.WriteLine("\nList of Disk Drives (SSD):");
        DisplayComponentInformation("Win32_DiskDrive", "Manufacturer", "Model", "Size", "MediaType");

        Console.WriteLine("\nList of Motherboards:");
        DisplayComponentInformation("Win32_BaseBoard", "Manufacturer", "Product");

        Console.WriteLine("\nList of RAM Modules:");
        DisplayRAMInformation("Win32_PhysicalMemory", "Manufacturer", "Capacity", "Speed");

        Console.WriteLine("\nTotal Physical Memory:");
        DisplayTotalPhysicalMemory("Win32_ComputerSystem", "TotalPhysicalMemory");

        Console.ReadLine(); // Keep the console window open
    }

    static void DisplayComponentInformation(string className, params string[] properties)
    {
        // Construct the query with specified properties
        string query = $"SELECT {string.Join(", ", properties)} FROM {className}";

        // Create a ManagementObjectSearcher with the specified query
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
        {
            // Execute the query and get the collection of ManagementObjects
            ManagementObjectCollection queryCollection = searcher.Get();

            // Iterate through each ManagementObject in the collection
            foreach (ManagementObject m in queryCollection)
            {
                // Print information about the component
                foreach (string property in properties)
                {
                    Console.WriteLine($"{property}: {m[property]}");
                }
                Console.WriteLine();
            }
        }
    }

    static void DisplayRAMInformation(string className, params string[] properties)
    {
        // Construct the query with specified properties
        string query = $"SELECT {string.Join(", ", properties)} FROM {className}";

        // Create a ManagementObjectSearcher with the specified query
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
        {
            // Execute the query and get the collection of ManagementObjects
            ManagementObjectCollection queryCollection = searcher.Get();

            // Iterate through each ManagementObject in the collection
            foreach (ManagementObject m in queryCollection)
            {
                // Print information about RAM modules
                foreach (string property in properties)
                {
                    Console.WriteLine($"{property}: {m[property]}");
                }
                Console.WriteLine();
            }
        }
    }

    static void DisplayTotalPhysicalMemory(string className, string property)
    {
        // Construct the query to retrieve the total physical memory
        string query = $"SELECT {property} FROM {className}";

        // Create a ManagementObjectSearcher with the specified query
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
        {
            // Execute the query and get the collection of ManagementObjects
            ManagementObjectCollection queryCollection = searcher.Get();

            // Iterate through each ManagementObject in the collection
            foreach (ManagementObject m in queryCollection)
            {
                // Check if TotalPhysicalMemory is not null before attempting conversion
                if (m["TotalPhysicalMemory"] != null)
                {
                    // Convert the total physical memory to GB and round to the nearest whole number
                    ulong totalMemoryInBytes = Convert.ToUInt64(m["TotalPhysicalMemory"]);
                    double totalMemoryInGB = Math.Round(totalMemoryInBytes / (1024.0 * 1024.0 * 1024.0));
                    Console.WriteLine($"Total Physical Memory: {totalMemoryInGB} GB");
                }
                else
                {
                    Console.WriteLine("Total Physical Memory: Not Available");
                }
            }
        }
    }
}
