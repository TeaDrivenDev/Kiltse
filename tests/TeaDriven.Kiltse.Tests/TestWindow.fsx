//#r "System.Windows.dll"
#r "PresentationFramework.dll"
#r "PresentationCore.dll"
#r "WindowsBase.dll"
#r "System.Xaml.dll"

open System.Windows
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Input

let createEnvironment() =
    let window = Window(Width = 1000., Height = 800.)
    let canvas = Canvas()
    canvas.Background <- Brushes.Black
    window.Content <- canvas
    window.Show()

    window, canvas

let window, canvas = createEnvironment()
window.SizeChanged.AddHandler <|
    SizeChangedEventHandler (fun sender e ->
        let window = sender :?> Window
        let panel = window.Content :?> Panel
        panel.Height <- window.Height
        panel.Width <- window.Width)

type DragDrop =
    {
        mutable MouseCaptured : bool
        mutable CanvasPosition : float * float
        mutable ControlPosition : float * float
        mutable Source : UIElement
    }

let dragDrop = { MouseCaptured = false; CanvasPosition = 0., 0.; ControlPosition = 0., 0.; Source = null }

let getPosition (relativeTo : IInputElement) (e : MouseEventArgs) =
    let position = e.GetPosition relativeTo
    position.X, position.Y

let mouseDown (sender : obj) (e : MouseButtonEventArgs) =
    dragDrop.Source <- sender :?> UIElement
    dragDrop.Source |> Mouse.Capture |> ignore
    dragDrop.MouseCaptured <- true
    dragDrop.ControlPosition <- Canvas.GetLeft dragDrop.Source, Canvas.GetTop dragDrop.Source

    dragDrop.CanvasPosition <- e |> getPosition canvas

let mouseMove (sender : obj) (e: MouseEventArgs) =
    let add (x1, y1) (x2, y2) = x1 + x2, y1 + y2
    let sub (x2, y2) (x1, y1) = x1 - x2, y1 - y2

    if dragDrop.MouseCaptured then
        let current = e |> getPosition canvas

        let newControlX, newControlY =
            current
            |> add dragDrop.ControlPosition
            |> sub dragDrop.CanvasPosition

        Canvas.SetLeft(dragDrop.Source, newControlX)
        Canvas.SetTop(dragDrop.Source, newControlY)

        dragDrop.ControlPosition <- newControlX, newControlY
        dragDrop.CanvasPosition <- current

let mouseUp (sender : obj) (e : MouseButtonEventArgs) =
    Mouse.Capture null |> ignore
    dragDrop.MouseCaptured <- false

let makeDraggable (control : UIElement) =
    control.MouseLeftButtonDown.AddHandler <| MouseButtonEventHandler mouseDown
    control.MouseMove.AddHandler <| MouseEventHandler mouseMove
    control.MouseLeftButtonUp.AddHandler <| MouseButtonEventHandler mouseUp
