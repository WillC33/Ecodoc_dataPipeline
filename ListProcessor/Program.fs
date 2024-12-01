namespace ListProcessor

open System.Globalization
open System.IO
open System.Text.RegularExpressions

module Main =

    ///<description>
    /// A function to load a file into memory and import the contents
    ///</description>
    ///<param name="filePath">the string of the file path</param>
    let parseTxt (filePath: string) =
        seq {
            use reader = new StreamReader(filePath, System.Text.Encoding.UTF8)

            while not reader.EndOfStream do
                yield reader.ReadLine()
        }
        |> String.concat " "

    ///<description>
    /// A function that takes a text string and returns the lowercase version according to a culture (socio-linguistic) variable
    ///</description>
    ///<param name="culture">The socio-linguistic CultureInfo object</param>
    ///<param name="text">The text string to lowercase</param>
    let lowercase (culture: CultureInfo) (text: string) = text.ToLower(culture)

    ///<description>
    /// A function to tokenise the line string into individual words
    ///</description>
    ///<param name="line">The text string to tokenise into words</param>
    let tokenise (line: string) =
        let splitters = Regex("[\'\-]", RegexOptions.Compiled)
        let firstPass = splitters.Replace(line, ", ")

        firstPass.Split([| ' ' |], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

    ///<description>
    /// A function to remove any forbidden characters via regex, those not contained into A-Z, a-z or relevant UTF-8 accented characters (punctuation)
    ///</description>
    ///<param name="word">The text string to remove symbols from</param>
    let removeSymbols (word: string) =
        let regex = Regex("[^a-zA-Zà-ÿÀ-Ÿ]", RegexOptions.Compiled)
        regex.Replace(word, "")

    ///<description>
    /// A function to remove symbols identified in the previous function, take the unique list entries and alphabetically order a list of words
    ///</description>
    ///<param name="tokenList">The list of words to process</param>
    let clean (tokenList: string list) =
        tokenList |> List.map removeSymbols |> List.distinct |> List.sort

    ///<description>
    /// A function to output the processed word list into a CSV File
    ///</description>
    ///<param name="outputPath">The path to output the file to</param>
    ///<param name="lines">The list of CSV lines to write into the file</param>
    let writeCsv (outputPath: string) (lines: seq<string>) =
        use writer = new StreamWriter(outputPath)
        lines |> Seq.iter (fun line -> writer.WriteLine(line))

    ///<description>
    /// A function to run the above definitions and process a single file based on its Path
    ///</description>
    ///<param name="filePath">The file path to load the contents from</param>
    ///<param name="culture">The CultureInfo to use for processing</param>
    let processFile (filePath: string) (culture: CultureInfo) =
        filePath
        |> parseTxt
        |> lowercase culture
        |> tokenise
        |> clean
        |> fun cleaned -> (filePath + " ") :: cleaned |> String.concat ","

    ///<description>
    /// The start point for the programme, using the above definitions we will load all the .txt files from the given folder and process them
    /// They will be loaded, set as lowercase, turned into word lists and cleaned by removing irrelevant special characters, de-duplicating and sorting
    /// The lists will finally be output as a CSV file with a line for each original file
    ///</description>
    [<EntryPoint>]
    let main _ =
        printfn "Data Pipeline v0.1.0"

        let culture = CultureInfo("fr-FR") // We will use the France-French culture object those this can be changed
        let inputFolder = "./samples" // The files to transform are stored in the 'samples' folder``
        let outputFile = "./output.csv" // The final CSV will be output to the output.csv file
        printfn $"Converting text docs in {inputFolder}"

        Directory.GetFiles(inputFolder, "*.txt")
        |> Array.toSeq
        |> Seq.map (fun file -> processFile file culture)
        |> writeCsv outputFile

        printfn $"Operation complete. {outputFile} has been written. Exiting..."
        0
