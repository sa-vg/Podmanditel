namespace Podmanditel.UI

open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Podmanditel.Core
open Avalonia.Controls
open Avalonia.Layout

module FailureKeysUI =

    type State =
        { pickedItems: string list
          current: string }
    let init =
        { pickedItems = []
          current = Registry.getValue () }

    type Msg =
        | Clear
        | Add of key: string
        | Remove of key: string
        | Reload
        | Apply

    let applyValues state =
        state.pickedItems |> Registry.setValues
        let newValue = Registry.getValue ()
        { state with current = newValue }

    let reloadValue state =
        { state with current = Registry.getValue () }

    let update (msg: Msg) (state: State): State =
        match msg with
        | Add item -> { state with pickedItems = item :: state.pickedItems }
        | Remove item -> { state with pickedItems = state.pickedItems |> List.filter ((<>) item) }
        | Reload -> reloadValue state
        | Apply -> applyValues state

    let keys = Config.keys

    let isChecked data state =
        List.contains data state.pickedItems

    let view (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children [ 
//                                   Button.create [ Button.dock Dock.Bottom
//                                                   Button.onClick (fun _ -> dispatch Reload)
//                                                   Button.content "Reload Keys" ]

                                   Button.create [ Button.dock Dock.Bottom
                                                   Button.onClick (fun _ -> dispatch Apply)
                                                   Button.content "Apply Keys" ]

                                   TextBlock.create [ TextBlock.dock Dock.Top
                                                      TextBlock.fontSize 13.0
                                                      TextBlock.verticalAlignment VerticalAlignment.Center
                                                      TextBlock.horizontalAlignment HorizontalAlignment.Left
                                                      TextBlock.text ("Current: " + state.current) ]

                                   ListBox.create [ ListBox.dock Dock.Top
                                                    ListBox.dataItems keys
                                                    ListBox.itemTemplate
                                                        (DataTemplateView<string>
                                                            .create(fun data ->
                                                                  
                                                                   CheckBox.create [ CheckBox.content data
                                                                                     CheckBox.isChecked false
                                                                                     CheckBox.onChecked (fun _ -> dispatch (Add data))
                                                                                     CheckBox.onUnchecked (fun _ -> dispatch (Remove data)) ])) ]

                                   TextBox.create [ TextBox.dock Dock.Top
                                                    TextBox.text (Core.getResultString state.pickedItems) ]
                                   ]
            ]
