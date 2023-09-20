open System
open System.IO
open System.Text
open System.Text.RegularExpressions

let breakpoints =
    [ "-sm", "22.5rem"
      "-md", "40rem"
      "-lg", "80rem"
      "-xl", "120rem" ]

let sb = StringBuilder()
let baseCss = File.ReadAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.base.css"))
let atomsCss = File.ReadAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.atoms.css"))

sb.AppendLine(baseCss) |> ignore
sb.AppendLine(atomsCss) |> ignore

let breakpointCssAtoms breakpointSuffix =
    Regex.Replace(atomsCss, @"^(\.[\w\-\d]+)", $"  $1{breakpointSuffix}", RegexOptions.Multiline)

for (suffix, width) in breakpoints do
    sb.AppendLine() |> ignore
    sb.AppendLine($"@media screen and (min-width: {width}) {{") |> ignore
    sb.AppendLine(breakpointCssAtoms suffix) |> ignore
    sb.AppendLine("}") |> ignore

File.WriteAllText (Path.Join(__SOURCE_DIRECTORY__, "nhlpa.css"), sb.ToString())