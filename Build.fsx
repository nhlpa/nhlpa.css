open System
open System.IO
open System.Text
open System.Text.RegularExpressions

let breakpoints =
    [
      "-l", "64em"
    ]

let sb = StringBuilder()
let baseCss = File.ReadAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.base.css"))
let componentsCss = File.ReadAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.components.css"))
let atomsCss = File.ReadAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.atoms.css"))

sb.AppendLine(baseCss) |> ignore
sb.AppendLine(componentsCss) |> ignore
sb.AppendLine(atomsCss) |> ignore

let breakpointCssAtoms breakpointSuffix =
    Regex.Replace(atomsCss, @"^(\.[\w\-\d]+)", $"  $1{breakpointSuffix}", RegexOptions.Multiline)

for (suffix, width) in breakpoints do
    sb.AppendLine() |> ignore
    sb.AppendLine($"@media screen and (min-width: {width}) {{") |> ignore
    sb.AppendLine(breakpointCssAtoms suffix) |> ignore
    sb.AppendLine("}") |> ignore

File.WriteAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.css"), sb.ToString())