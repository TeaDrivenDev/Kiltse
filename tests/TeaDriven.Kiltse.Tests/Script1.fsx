
#r "System.Windows.dll"
#r "PresentationFramework.dll"
#r "PresentationCore.dll"
#r "WindowsBase.dll"
#r "System.Xaml.dll"

#r @"bin\Debug\TeaDriven.Kiltse.dll"

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Media

open TeaDriven.Kiltse
open System.Windows.Input



let createEnvironment() =
    let window = Window(Width = 1000., Height = 800.)
    let canvas = Canvas()
    canvas.Background <- Brushes.Black
    window.Content <- canvas
    window.Show()

    window, canvas

let window, canvas = createEnvironment()
window.SizeChanged.AddHandler <| SizeChangedEventHandler (fun sender e ->
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

let dragDrop = { MouseCaptured = false; CanvasPosition = 0., 0.; ControlPosition = 0.,  0.; Source = null }


let mouseDown (sender : obj) (e : MouseButtonEventArgs) =
    dragDrop.Source <- sender :?> UIElement
    dragDrop.Source |> Mouse.Capture |> ignore
    dragDrop.MouseCaptured <- true
    dragDrop.ControlPosition <- Canvas.GetLeft dragDrop.Source, Canvas.GetTop dragDrop.Source
    dragDrop.CanvasPosition <- e.GetPosition(canvas).X, e.GetPosition(canvas).Y

let mouseMove (sender : obj) (e: MouseEventArgs) =
    if dragDrop.MouseCaptured then
        let x, y = e.GetPosition(canvas).X, e.GetPosition(canvas).Y
        
        let controlX, controlY = dragDrop.ControlPosition
        let canvasX, canvasY = dragDrop.CanvasPosition
        let newControlX, newControlY = controlX + x - canvasX, controlY + y - canvasY

        Canvas.SetLeft(dragDrop.Source, newControlX)
        Canvas.SetTop(dragDrop.Source, newControlY)
        
        dragDrop.ControlPosition <- newControlX, newControlY
        dragDrop.CanvasPosition <- x, y

let mouseUp (sender : obj) (e : MouseButtonEventArgs) =
    Mouse.Capture null |> ignore
    dragDrop.MouseCaptured <- false


let makeDraggable (control : UIElement) =
    control.MouseLeftButtonDown.AddHandler <| MouseButtonEventHandler mouseDown
    control.MouseMove.AddHandler <| MouseEventHandler mouseMove
    control.MouseLeftButtonUp.AddHandler <| MouseButtonEventHandler mouseUp






let names =
    [
       "Alpheratz"
       "Ankaa"
       "Schedar"
       "Diphda"
       "Achernar"
       "Hamal"
       "Acamar"
       "Menkar"
       "Mirfac"
       "Aldebaran"
       "Rigel"
       "Capella"
       "Bellatrix"
       "Elnath"
       "Alnilam"
       "Betelgeuse"
       "Canopus"
       "Sirius"
       "Adhara"
       "Procyon"
       "Pollux"
       "Avior"
       "Suhail"
       "Miaplacidus"
       "Alphard"
       "Regulus"
       "Dubhe"
       "Denebola"
       "Gienah"
       "Acrux"
       "Gacrux"
       "Alioth"
       "Spica"
       "Alkaid"
       "Hadar"
       "Menkent"
       "Arcturus"
       "Rigel"
       "Zubenelgenubi"
       "Kochab"
       "Alpheca"
       "Antares"
       "Atria"
       "Sabic"
       "Shaula"
       "Rasalhague"
       "Eltanin"
       "Kaus"
       "Vega"
       "Nunki"
       "Altair"
       "Peacock"
       "Deneb"
       "Enif"
       "Al Na'ir"
       "Fomalhaut"
       "Markab"
    ]

let maxSatellites = 30

let radius = 40.

let random = Random(DateTime.Now.Millisecond)
    
let getRandomPosition (width, height) =
    let getRandomCoordinate max = (max - 200.) * random.NextDouble() + 100.

    getRandomCoordinate width, getRandomCoordinate height


let selectName names =
    (random.Next(List.length names - 1))
    |> List.nth names



let getNumberOfSatellites () =
    if random.Next(7) < 6 then random.Next(1, 9) else random.Next(10, maxSatellites)


let defineParameters windowSize =
    let name = selectName names
    let position = getRandomPosition windowSize
    let numberOfSatellites = getNumberOfSatellites()


    name, position, (fun () -> ()), [0..numberOfSatellites-1]

let (||||>) (a, b, c, d) f = f a b c d


let createSky numberOfStars =
    [1..numberOfStars]
    |> List.map (fun _ ->
        defineParameters (window.Width, window.Height)
        |> (fun (name, (x, y), fn, items) ->
            let ring = Ring(Radius = radius, DisplayName = name, ItemsSource = (items |> List.map box))
            Canvas.SetLeft(ring, x)
            Canvas.SetTop(ring, y)
            makeDraggable ring

            ring))
    |> List.iter (canvas.Children.Add >> ignore)




canvas.Children.Clear()


createSky 5




let ring = Ring(Radius = 250., DisplayName = "Large", ItemsSource = ([0..200] |> List.map box))
makeDraggable ring
Canvas.SetLeft(ring, 400.)
Canvas.SetTop(ring, 400.)


//ring.ItemsSource <- [0..10] |> List.map box 

ring |> canvas.Children.Add


let items = [0..6] |> List.map box

Ring(Radius = 40., DisplayName = "Da", ItemsSource = items) |> canvas.Children.Add