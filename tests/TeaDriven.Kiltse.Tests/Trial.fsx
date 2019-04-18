#r "System.Windows.dll"
#r "PresentationFramework.dll"
#r "PresentationCore.dll"
#r "WindowsBase.dll"
#r "System.Xaml.dll"
#r @"..\..\packages\FsEye\lib\net40\FsEye.dll"
#r @"..\..\src\TeaDriven.Kiltse\bin\Debug\TeaDriven.Kiltse.dll"

open System
open System.Linq
open System.Threading
open System.Windows
open System.Windows.Controls
open System.Windows.Media

open TeaDriven.Kiltse
open System.Windows.Input

#load "TestWindow.fsx"
//#load @"..\..\packages\FsEye\content\FsEye.fsx"

open System.Windows.Data
open System.Windows.Media.Effects

let window, canvas = TestWindow.window, TestWindow.canvas

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

let createSky numberOfStars =
    [1..numberOfStars]
    |> List.map (fun _ ->
        defineParameters (window.Width, window.Height)
        |> (fun (name, (x, y), fn, items) ->
            let ring = Ring(Radius = radius, DisplayName = name, ItemsSource = (items |> List.map box))
            Canvas.SetLeft(ring, x)
            Canvas.SetTop(ring, y)
            TestWindow.makeDraggable ring

            ring))
    |> List.iter (canvas.Children.Add >> ignore)

//canvas.Children.Clear()

type FizzbuzzConverter() =
    interface IValueConverter with
            member x.Convert(value : obj, targetType : Type, parameter : obj, culture : Globalization.CultureInfo) : obj =
                if null = value then
                    printfn "---- null"
                    Unchecked.defaultof<Brush>
                else
                    printfn "---- Value: %s" (value.GetType().ToString())

                    match value with
                    | :? string -> Brushes.DarkOrange
                    | :? RingItem as ringItem ->
                        match ringItem.ItemIndex with
                        | x when x % 3 = 0 && x % 5 = 0 -> Brushes.Red
                        | x when x % 3 = 0 -> Brushes.GreenYellow
                        | x when x % 5 = 0 -> Brushes.DeepSkyBlue
                        | _ -> Brushes.AntiqueWhite
                    :> Brush
                |> box

            member x.ConvertBack(value : obj, targetType : Type, parameter : obj, culture : Globalization.CultureInfo) : obj =
                failwith "Not implemented yet"

type StringFirstLetterStrokeInfoSelector() =
    inherit StrokeInfoSelector()

    let getEffect (color : Color) =
        let effect = DropShadowEffect(ShadowDepth = 0., Color = color, Opacity = 1., BlurRadius = 12.)
        effect.RenderingBias <- RenderingBias.Quality
        effect

    override __.GetStrokeInfo(value : RingItem) =
        let s = string value.Item

        let red = max ((255 - int s.[0]) * 10) 0

        let color = Color.FromRgb(255uy - byte red, byte red, byte red)

        StrokeInfo(SolidColorBrush color, __.DefaultStrokeThickness, getEffect color)

let addRing () =
    let count = getNumberOfSatellites ()

    let items =
        [ 1 .. count ]
        |> List.map (fun _ -> selectName names)
        |> List.map box

    let ring = Iris(Radius = 35., DisplayName = sprintf "Dings %i" count, ItemsSource = items, GapWidth = 3.)
    ring.StrokeInfoSelector <- StringFirstLetterStrokeInfoSelector(DefaultStrokeThickness = 3.)
    ring.Direction <- SweepDirection.Clockwise
    TestWindow.makeDraggable ring
    Canvas.SetLeft(ring, 200.)
    Canvas.SetTop(ring, 200.)

    ring |> canvas.Children.Add

addRing ()

let button = Button(Content = "Add")
button.Click.AddHandler (fun _ _ ->
    //canvas.Children.OfType<Iris>()
    //|> Seq.toList
    //|> List.iter canvas.Children.Remove

    addRing() |> ignore)

canvas.Children.Add button |> ignore

Canvas.SetTop(button, 25.)
Canvas.SetRight(button, 25.)

//canvas.Children.Clear()

//
//[0. .. 10. .. 360.]
//|> List.iter (fun angle -> ring.StartAngle <- angle; Thread.Sleep 500)
//
//let dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
//
//Thread(ThreadStart(fun () ->
//        [0. .. 10. .. 360.]
//        |> List.iter (fun angle ->
//            dispatcher.InvokeAsync(Action(fun () -> ring.StartAngle <- angle)) |> ignore; Thread.Sleep 500) )).Start()
