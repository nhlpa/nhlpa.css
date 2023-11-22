open System
open System.IO
open System.Text
open System.Text.RegularExpressions

let sb = StringBuilder()
let append (x : string) = sb.AppendLine(x) |> ignore
let appendLine () = sb.AppendLine() |> ignore


let breakpoints =
    [
      "-l", "64em"
    ]

let cssFiles =
    [
        "nhlpa.base.css"
        "nhlpa.components.css"
        "nhlpa.atoms.display.css"
        "nhlpa.atoms.flexbox.css"
        "nhlpa.atoms.sizing.css"
        "nhlpa.atoms.spacing.css"
        "nhlpa.atoms.text.css"
    ]

// process base styles
cssFiles
|> List.map (fun cssPath ->
    let cssContent = File.ReadAllText(Path.Join(__SOURCE_DIRECTORY__, cssPath))
    append cssContent
    appendLine ()
    cssPath, cssContent)
|> List.filter (fun (cssPath, _) -> cssPath.Contains("atoms"))
|> List.iter (fun (_, cssContent) ->
    let breakpointCssAtoms breakpointSuffix =
        Regex.Replace(cssContent, @"^(\.[\w\-\d]+)", $"  $1{breakpointSuffix}", RegexOptions.Multiline)

    for (suffix, width) in breakpoints do
        append $"@media screen and (min-width: {width}) {{"
        append (breakpointCssAtoms suffix)
        append "}")

File.WriteAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.css"), sb.ToString())