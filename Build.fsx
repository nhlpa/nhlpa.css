open System.IO
open System.Text
open System.Text.RegularExpressions

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
        "nhlpa.atoms.color.css"
    ]

let sb = StringBuilder()

let append (x : string) = sb.AppendLine(x) |> ignore
let appendLine () = sb.AppendLine() |> ignore

cssFiles
|> List.map (fun cssPath ->
    let cssContent = File.ReadAllText(Path.Join(__SOURCE_DIRECTORY__, cssPath))
    append cssContent
    appendLine ()
    cssPath, cssContent)
|> List.filter (fun (cssPath, _) -> cssPath.Contains("atoms"))
|> List.map snd
|> String.concat "\n\n"
|> fun breakpointCss ->
    for (suffix, width) in breakpoints do
        let breakpointCssAtoms =
            Regex.Replace(
                input = breakpointCss,
                pattern = @"^(\.[\w\-\d]+)",
                replacement = $"  $1{suffix}",
                options = RegexOptions.Multiline)

        append $"@media screen and (min-width: {width}) {{"
        append breakpointCssAtoms
        append "}"

File.WriteAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.css"), sb.ToString())