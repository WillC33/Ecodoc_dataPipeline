# A Data Processing Pipeline for the Ecodoc Research Project

A functional data processing pipeline built in F# for linguistic research applications. Originally developed to support the [ECODOC project](https://ecodoc.u-bordeaux.fr/ecodoc/about) project at the University of Bordeaux for processing French text corpora.

## Overview

ListProcessor transforms collections of text files into structured CSV datasets suitable for linguistic analysis. The tool handles French language-specific processing including proper accent preservation, cultural-aware text normalisation, and tokenisation optimised for Romance language characteristics.

## Features

- **Batch text processing** - Processes all `.txt` files in a specified directory
- **Linguistic tokenisation** - Handles French contractions, hyphens, and word boundaries
- **Cultural localisation** - Uses French locale for proper case conversion and sorting
- **UTF-8 accent preservation** - Maintains linguistic integrity of accented characters (à-ÿ, À-Ÿ)
- **Deduplication and sorting** - Produces clean, alphabetically ordered word lists
- **CSV output** - Research-ready format with file provenance tracking

## Quick Start

1. Place your text files in the `samples/` directory
2. Run the application:
   ```bash
   dotnet run
   ```
3. Find your processed data in `output.csv`

## Output Format

The tool generates a CSV file where each line represents one input file:
```
filename.txt,mot1,mot2,mot3,...
```

The first column contains the source filename, followed by all unique words found in that file, alphabetically sorted.

## Processing Pipeline

The application follows a functional pipeline approach:

1. **File Loading** - Reads text files with UTF-8 encoding
2. **Normalisation** - Converts to lowercase using French cultural rules
3. **Tokenisation** - Splits text into words, handling French-specific punctuation
4. **Symbol Removal** - Removes non-alphabetic characters whilst preserving accents
5. **Deduplication** - Eliminates duplicate entries
6. **Sorting** - Alphabetically orders words using French collation rules
7. **CSV Export** - Outputs structured data for analysis

## Configuration

### Changing the Locale

To process text in a different language, modify the culture setting in `Program.fs`:

```fsharp
let culture = CultureInfo("en-GB") // For British English
let culture = CultureInfo("de-DE") // For German
let culture = CultureInfo("es-ES") // For Spanish
```

### Input/Output Paths

Adjust the file paths as needed:

```fsharp
let inputFolder = "./samples"     // Input directory
let outputFile = "./output.csv"   // Output file path
```

### Character Filtering

The regex pattern for character removal can be modified to include additional character sets:

```fsharp
let regex = Regex("[^a-zA-Zà-ÿÀ-Ÿ]", RegexOptions.Compiled)
```

## Requirements

- .NET 8.0 or later
- Input files must be UTF-8 encoded text files (`.txt`)

## Use Cases

This tool is particularly useful for:

- **Corpus linguistics research** - Preparing text corpora for frequency analysis
- **Digital humanities projects** - Processing historical or literary texts
- **Language learning applications** - Extracting vocabulary from texts
- **Lexicographic research** - Building word lists from document collections
- **Text mining preprocessing** - Normalising data before analysis

## Technical Notes

### Performance

The application uses F#'s lazy sequence evaluation (`seq`) for memory-efficient processing of large files. Files are processed one at a time to avoid loading entire datasets into memory simultaneously.

### Character Handling

The tool is designed specifically for French text processing:
- Preserves all French accented characters (à, é, è, ç, etc.)
- Handles French punctuation conventions (apostrophes in contractions)
- Uses French cultural rules for case conversion and sorting

### Error Handling

The current implementation assumes well-formed UTF-8 input files. For production use with untrusted data, consider adding:
- File encoding detection
- Malformed input handling
- Empty file validation

## Contributing

This tool was developed for specific research needs. If you're adapting it for other linguistic research:

1. Test with your target language's character set
2. Verify cultural sorting behaviour meets your requirements
3. Consider whether the tokenisation rules suit your linguistic context

## Licence

Developed for academic research purposes. Please credit the original work if adapting for other research projects.

## Research Context

Originally created to support text corpus analysis for the EcoDOC project at the University of Bordeaux, focusing on environmental and ecological discourse in French academic literature.

---

*Built with F# for functional, reliable text processing in academic research environments.*
