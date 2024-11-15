// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Security.Authentication.ExtendedProtection;
namespace PlaylistAnalyzer {
    
    class Program {
        //Presets:
        //static string pathInPlaylist = @"D:\Downloads\Music\music\AIA v7\";
        //static string pathInNewPlaylist = @"/primary/Music/";
        
        // represents a path to a playlist file which'd be read
        static string PLAYLIST_PATH_OLD = @"C:\Users\maxim\OneDrive\Документы\GitHub\PlaylistConverter\auto-detect-file-extension-in-m3u8\playlists\_Calm Night_ Raining Streets or Mooning Nights_test.m3u8";
        
        // represents a path to duplicate of a playlist file we're working on
        static string PLAYLIST_PATH_NEW = PLAYLIST_PATH_OLD + @"NEW";
        
        // represents available extensions for a track
        static string[] extType = {".mp3", ".flac", ".ogg", ".m4a", ".wav"};

        // represents amount of failed fixes
        static int failC = 0;

        //represents amount of successful fixes
        static int succC = 0;

        // ###!--- Main ---!###
        static void Main(string[] args) {
            string[] strArrayInput = ReadFromFileIntoArray(PLAYLIST_PATH_OLD);
            string[] newArrOutput = new string[strArrayInput.Length];

            if (strArrayInput != null) {
                Console.WriteLine("[:] Entries found. Beginning check...");
                
                // replace denied entries to duplicate array
                for (int i = 0; i < strArrayInput.Length; i++) {
                    newArrOutput[i] = EntryExistence(strArrayInput[i]);
                }
                Console.WriteLine($"[?] Amount of entries failed: {failC}.");
                Console.WriteLine($"[?] Amount of entries fixed: {succC}.");

                // write duplicate fixed
                WriteFile(PLAYLIST_PATH_NEW, newArrOutput);


            } else {
                Console.WriteLine("[!] No entries found!");
            }
            Console.WriteLine("[:] Finished...");
            Console.ReadLine();
        }

        /// <summary>
        /// Input a string, convert to entry (without \n), check if it exists in the system. If not, find it using global *extType* and fix.
        /// </summary>
        /// <param name="entry">Input string with full absolute path to file.</param>
        /// <returns></returns>
        static string EntryExistence(string entry) {
            int counter = -1; // 1..Length
            int sepIndex = 0;
            int extIndex = 0;
            entry = entry.Replace("\r", "");
            string entryf = entry;
            string entryExtension = "";

            // entry exists, no rewrite needed
            if (File.Exists(entry)) {
                // find index of the last / or \
                foreach (char c in entry) { 
                    counter++; 
                    if (c == '/' && c == '\\') {sepIndex = counter;}
                }
                //int length = entry.Length - 1 - sepIndex;
                Console.WriteLine("[*] Entry *" + entry.Substring(sepIndex) + "* is found.");

                return entry;
            // entry wasn't found... FIND IT or leave it, since it isn't an entry
            } else if (!File.Exists(entry) && entry != "") {
                
                // find index of the last .
                foreach (char c in entry) {
                    counter++;
                    if (c == '.') {extIndex = counter;}
                }
                // string with current extension
                entryExtension = entry.Substring(extIndex); 
                
                // replace extension until it exists
                foreach (string ext in extType) {
                    if (entryExtension == ext) {continue;} // leave it - we already checked
                    entry = entryf;
                    entry = entry.Replace(entryExtension, ext);
                    if (File.Exists(entry)) {
                        Console.WriteLine("[*] New Entry *" + entry.Substring(sepIndex) + "* is now found.");
                        succC++;
                        return entry;
                    } else {
                        Console.WriteLine("[X] Entry *" + entry.Substring(sepIndex) + "* is NOT found. Retrying...");
                        failC++;
                    }
                }
                // leaving loop means that entry wasn't found while replacing extension...
                Console.WriteLine("[!] Entry *" + entry.Substring(sepIndex) + "* is NOT found! All available extensions tried, leaving entry unchanged...");
                return entry;
            } else {
                Console.WriteLine("[!] Entry is not an entry...");
                return entry;
            }
        }


        /// <summary>
        /// Read file, parse each string of a file into string array.
        /// </summary>
        /// <param name="input_path">Path to a file.</param>
        /// <returns></returns>
        static string[] ReadFromFileIntoArray(string input_path) {
            StreamReader sr;
            if (!File.Exists(input_path)){
                Console.WriteLine($"[!] Input file with *{input_path}* doesn't exist!");
                return null;
            }
            sr = new StreamReader(input_path);
            string[] strsFromFile = sr.ReadToEnd().Split('\n');
            Console.WriteLine($"[?] Input file with {strsFromFile.Length} strings.");
            sr.Close();
            return strsFromFile;
        }

        /// <summary>
        /// Create new file from string array.
        /// </summary>
        /// <param name="path">Path to output new file.</param>
        /// <param name="stringArray">Input string array.</param>
        static void WriteFile(string path, string[] stringArray) {
            StreamWriter sw; 
            sw = new StreamWriter(path, false);
            int counter = 0;
            Console.WriteLine("[?] Writing...");

            foreach (string str in stringArray) {
                sw.WriteLine(str);
                counter++;
            }
            sw.Close();
            Console.WriteLine($"[?] Amount of strings written: {counter}.");
        }
    }
    
}
