namespace ListProcessor

open System.Globalization
open System.IO
open System.Text.RegularExpressions

module Main =

    let parseTxt (filePath: string) =
        seq {
            use reader = new StreamReader(filePath, System.Text.Encoding.UTF8)

            while not reader.EndOfStream do
                yield reader.ReadLine()
        }
        |> String.concat " "

    let lowercase (culture: CultureInfo) (text: string) = text.ToLower(culture)

    let tokenise (line: string) =
        let splitters = Regex("[\'\-]", RegexOptions.Compiled)
        let firstPass = splitters.Replace(line, ", ")

        firstPass.Split([| ' ' |], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

    let removeSymbols (word: string) =
        let regex = Regex("[^a-zA-Zà-ÿÀ-Ÿ]", RegexOptions.Compiled)
        regex.Replace(word, "")

    let clean (tokenList: string list) =
        tokenList |> List.map removeSymbols |> List.distinct |> List.sort

    let writeCsv (outputPath: string) (lines: seq<string>) =
        use writer = new StreamWriter(outputPath)
        lines |> Seq.iter (fun line -> writer.WriteLine(line))

    let processFile (filePath: string) (culture: CultureInfo) =
        filePath
        |> parseTxt
        |> lowercase culture
        |> tokenise
        |> clean
        |> fun cleaned -> (filePath + " ") :: cleaned |> String.concat ","

    [<EntryPoint>]
    let main _ =
        printfn "Data Pipeline v0.1.0"

        let culture = CultureInfo("fr-FR")
        let inputFolder = "./samples"
        let outputFile = "./output.csv"
        printfn $"Converting text docs in {inputFolder}"

        Directory.GetFiles(inputFolder, "*.txt")
        |> Array.toSeq
        |> Seq.map (fun file -> processFile file culture)
        |> writeCsv outputFile

        printfn $"Operation complete. {outputFile} has been written. Exiting..."
        0
