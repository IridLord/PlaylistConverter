//conversion of .m3u8 file with .flac in it to .mp3 with different path
internal class Program {
    //public static string pathToScript = Directory.GetCurrentDirectory();
    public static string pathInPlaylist = ""; //пути в старом плейлисте
    public static string pathInNewPlaylist = ""; //пути в новом плейлисте
    public static string pathTOPlaylist = ""; //пути К старому плейлисту
    public static string pathTONewPlaylist = ""; //пути К новому плейлисту

    /// <summary>
    /// Читаем файл, парсим его в массив строк метода.
    /// </summary>
    /// <param name="input_path">Путь до файла ввода, старого плейлиста</param>
    /// <returns></returns>
    static string[] ReadFromFileIntoArray(string input_path) {
        StreamReader sr; 
        sr = new StreamReader(input_path);
        
        string[] strsFromFile = sr.ReadToEnd().Split('\n');
        sr.Close(); //закрываем чтение потока данных
        return strsFromFile;
    }
    /// <summary>
    /// Заменяем все строки файла (в этом случае пути до файла), а именно
    /// тип файла и его путь.
    /// </summary>
    /// <param name="oldPlaylistpath">Старый путь который заменим</param>
    /// <param name="newPlaylistpath">Новый путь на который заменим</param>
    /// <param name="inputFromFile">Массив строк, считанный с файла</param>
    /// <returns></returns>
    static string[] ReplaceStrs(string oldPlaylistpath, string newPlaylistpath, 
        string[] inputFromFile) {
        string str = "";
        char lastChar = ' ';
        for (int i = 0; i < inputFromFile.Length - 1; i++) {
            str = inputFromFile[i];

            //если последняя буква a (.m4a) c (.flac) g (.ogg)
            str = str.TrimEnd('\r', '\n');
            lastChar = str[str.Length - 1];
            switch (lastChar) {
                case 'c': str = str.Replace(".flac", ".mp3"); break;
                case 'a': str = str.Replace(".m4a", ".mp3"); break;
                case 'g': str = str.Replace(".ogg", ".mp3"); break;
                default: break;
            }
            str = str.Replace(oldPlaylistpath, newPlaylistpath);
            inputFromFile[i] = str;
            } 
        return inputFromFile;
    }
    /// <summary>
    /// Создаем новый файл после замены данных плейлиста
    /// </summary>
    /// <param name="path">Путь для нового файла</param>
    /// <param name="stringArray">Массив строк, кот. запишем в файл</param>
    static void WriteFile(string path, string[] stringArray) {
        //sw реализует класс System.IO - позволяет записывать поток данных в файл
        StreamWriter sw; 

        //открываем поток (false - добавление строк, true - замена строк)
        sw = new StreamWriter(path, false);
        
        int counter = 0;
        Console.WriteLine("Начало записи.");

        foreach (string str in stringArray) {
            sw.WriteLine(str);
            counter++;
        }
        sw.Close(); //закрываем поток, иначе неправильно + файл недоступен*
        Console.WriteLine($"Запись завершена c {counter} заменами.");
    }
    static void Main() {
        string[] strArray;
        string[] strArray_new;

        Console.WriteLine("Скрипт для замены типа файлов и пути до них для плейлистов .m3u8. " +
            "Нужно выбрать стандартные будут ли пути или ввести их вручную. " + 
            "");
        Console.WriteLine("Введите 1 в консоль, чтобы использовать стандартные пути" +
            " или впишите 0 для своего пути.");
        
        //задаем пути до файлов, кот. указаны в .m3u8
        switch (Console.ReadLine()) {
            case "1": //стандартные пути для файлов плейлиста
                pathInPlaylist = @"D:\Downloads\Music\music\AIA v7\";
                pathInNewPlaylist = @"/primary/Music/";
                break;
            case "0":
                Console.WriteLine("Ввод путей в старом плейлисте:");
                pathInPlaylist = @"" + Console.ReadLine();
                Console.WriteLine("Ввод путей в новом плейлисте:");
                pathInNewPlaylist = @"" + Console.ReadLine();
                break;
            //сделать выход из скрипта?
            default: Console.WriteLine("Не выбрано, выход из скрипта..."); break;
        }
        Console.WriteLine("Введите теперь, где расположены сами файлы, т.е полный путь.");
        Console.WriteLine("Полный путь до самого файла:");
        pathTOPlaylist = @"" + Console.ReadLine();
        Console.WriteLine("Полный путь до нового файла:");
        pathTONewPlaylist = @"" + Console.ReadLine();

        //читаем старый, меняем массив, пишем новый
        
        strArray = ReadFromFileIntoArray(pathTOPlaylist);
        strArray_new = ReplaceStrs(pathInPlaylist, pathInNewPlaylist, strArray);
        WriteFile(pathTONewPlaylist, strArray_new);

        Console.WriteLine("Программа завершила работу.\nНажмите любую кнопку для выхода...");
        //Console.ReadKey(true);
        //Console.Clear();
    }
}