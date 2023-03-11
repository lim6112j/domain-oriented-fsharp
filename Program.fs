namespace DomainTypes// For more information see https://aka.ms/fsharp-console-apps
open BusinessLogic
module Program =
  let point = {x=1.1; y=2.2}
  let vehicle = {CurrentLoc=point; Id= "1"; Name=Some "lim"}
  printfn "vehicle object : %A" vehicle
