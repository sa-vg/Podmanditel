namespace Podmanditel.Core
open Microsoft.Win32
open FSharp.Configuration

module Config =
    
    type DebugKeyName = YamlConfig<"Config.yaml">
    let config = DebugKeyName()
    let registryPath = config.RegistryPath
    let debugKeyName = config.DebugKeyName
    let keys = config.DebugKeys |> Seq.toList
    let separator = config.DebugKeySeparator

module Core =
    let getResultString items = items |> List.sort |> String.concat Config.separator

module Registry =
    let getValue () = Registry.GetValue(Config.registryPath, Config.debugKeyName, "") |> string 
    let setValues items =
       let result = Core.getResultString items
       Registry.SetValue(Config.registryPath, Config.debugKeyName, result)
     