
#load "TestWindow.fsx"

open System
open System.Windows.Shapes
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Media.Effects
open System.Windows

let window, canvas = TestWindow.window, TestWindow.canvas



let uiColor = Colors.AntiqueWhite
let maxSegments = 30


let topLeftCorner (centerX : float, centerY : float) radius = centerX - radius, centerY - radius

let nthValue initial interval n = initial + (n * interval)

let toCartesian angle = Math.PI * angle / 180.

let angles numberOfPoints =
    let interval = 360. / (float numberOfPoints)

    let nthAngle n =
        (nthValue 0. interval (float n)) % 360.

    [0..numberOfPoints - 1]
    |> List.map nthAngle

//    let relativePeripheralCoordinates radius angle =
//        let cartesianAngle = toCartesian angle
//
//        let relativeX = radius * Math.Cos cartesianAngle |> Math.Round
//        let relativeY = radius * Math.Sin cartesianAngle |> Math.Round
//
//        relativeX, -relativeY
//
//    let peripheralCoordinates (centerX, centerY) radius numberOfPoints =
//        angles numberOfPoints
//        |> List.map (relativePeripheralCoordinates radius)
//        |> List.map (fun (x, y) -> centerX + x, centerY + y)
//

let circumference satelliteRadius distance numOfSatellites =
    (satelliteRadius + distance) * (float numOfSatellites)

let targetRadius minRadius circ =
    Math.Max(minRadius, (circ / Math.PI) / 2.)



let createCircle radius center =
    let circle = Ellipse(Width = 2. * radius, Height = 2. * radius)

    match center with
    | Some c -> 
        let left, top = topLeftCorner c radius
        Canvas.SetLeft(circle, left)
        Canvas.SetTop(circle, top)
    | None -> ()
    circle.Fill <- Brushes.Transparent
    circle.Stroke <- SolidColorBrush uiColor

    circle



let getEffect (color : Color) =
    let effect = DropShadowEffect(ShadowDepth = 0., Color = color, Opacity = 1., BlurRadius = 12.)
    effect.RenderingBias <- RenderingBias.Quality
    effect





let random = Random(DateTime.Now.Millisecond)
    
let getRandomPosition (width, height) =
    let getRandomCoordinate max = (max - 200.) * random.NextDouble() + 100.

    getRandomCoordinate width, getRandomCoordinate height


let selectName names =
    (random.Next(List.length names - 1))
    |> List.nth names



let getNumberOfSegments () =
    if random.Next(7) < 6 then random.Next(1, 9) else random.Next(10, maxSegments)


//    let defineParameters windowSize =
//        let name = selectName names
//        let position = getRandomPosition windowSize
//        let numberOfSatellites = getNumberOfSatellites()
//
//
//        name, position, numberOfSatellites


let withEffect (circle : Ellipse) = circle.Effect <- getEffect(uiColor); circle


let createStarLabel maxWidth name =
    let label = TextBlock()
    label.Text <- name
    label.Foreground <- Brushes.White
    label.Background <- Brushes.Transparent
    label.FontWeight <- FontWeights.Regular

    label.FontSize <- 12.

    label.MaxWidth <- maxWidth
    label.TextWrapping <- TextWrapping.Wrap

    label.HorizontalAlignment <- HorizontalAlignment.Center
    label.VerticalAlignment <- VerticalAlignment.Center
    label.TextAlignment <- TextAlignment.Center

    label

let getLabelLeft radius (label : FrameworkElement) calculatedX =
    if calculatedX >= radius then calculatedX
    else calculatedX - label.ActualWidth

//    let createLabels radius labelTexts =
//        let numberOfSatellites = labelTexts |> List.length
//
//
//        peripheralCoordinates (radius, radius) (radius + 12.) numberOfSatellites
//        |> List.zip labelTexts
//        |> List.map (fun (text, (x, y)) ->
//                        let label = TextBlock()
//                        label.Foreground <- Brushes.AntiqueWhite
//                        label.Text <- text
//
////                        canvas.Children.Add label |> ignore
//                        Canvas.SetLeft(label, x)
//                        Canvas.SetTop(label, y)
////                        Canvas.SetLeft(label, getLabelLeft radius label x)
////                        Canvas.SetTop(label, y - label.ActualHeight / 2.)
//                        
//                        label)
    
   
[<Measure>] type deg
[<Measure>] type rad

let gapPixels = 3.

let toPolar (angle : float<deg>) = Math.PI * 1.<rad> * angle / 180.<deg>

let relativePeripheralCoordinates radius angle =
    let polarAngle = angle |> toPolar

    let relativeX = radius * Math.Cos (polarAngle / 1.<rad>)
    let relativeY = radius * Math.Sin (polarAngle / 1.<rad>)

    relativeY, -relativeX


let arc radius totalInt indexInt =
    let total, index = float totalInt, float indexInt

    let circumference = 2. * Math.PI * radius
    let gapHalfAngle = 360.<deg> * gapPixels / (circumference * 2.)


    let arcAngle = 360.<deg> / total
    let arcStartAngle = index * arcAngle + gapHalfAngle
    let arcEndAngle = (index + 1.) * arcAngle - gapHalfAngle

    let arcStartX, arcStartY = relativePeripheralCoordinates radius arcStartAngle
    let arcEndX, arcEndY = relativePeripheralCoordinates radius arcEndAngle

    let sx, sy = radius + arcEndX, radius + arcEndY
    let ex, ey = radius + arcStartX, radius + arcStartY


    let arcSeg = ArcSegment()
    arcSeg.Point <- Point(ex, ey)
    arcSeg.Size <- Size(radius, radius)
    arcSeg.IsLargeArc <- (total = 1.)


    let pathFigure = PathFigure()
    pathFigure.StartPoint <- Point(sx, sy)
    pathFigure.Segments.Add arcSeg

    let pathGeometry = PathGeometry()
    pathGeometry.Figures.Add pathFigure

    let color =
        match indexInt with
        | 0 -> Colors.Red
        | 1 -> Colors.Cyan
        | _ -> uiColor


    let arcPath = Path()
    arcPath.Stroke <- SolidColorBrush color
    arcPath.StrokeThickness <- 2.
    arcPath.Effect <- getEffect color
    arcPath.Data <- pathGeometry
//        arcPath.ClipToBounds <- false

    arcPath



    

let createStar name (x, y) play releases =
    let numberOfSegments = releases |> List.length
        
    let radius = 40.
    
    let starGrid = Grid(Width = 2. * radius, Height = 2. * radius)
    starGrid.HorizontalAlignment <- HorizontalAlignment.Center
    starGrid.VerticalAlignment <- VerticalAlignment.Center
    Canvas.SetLeft(starGrid, x - radius)
    Canvas.SetTop(starGrid, y - radius)
    starGrid |> TestWindow.makeDraggable
    

    let starCanvas = Canvas(Width = 2. * radius, Height = 2. * radius)
    starCanvas.HorizontalAlignment <- HorizontalAlignment.Stretch
    starCanvas.VerticalAlignment <- VerticalAlignment.Stretch
    starCanvas.Background <- Brushes.Transparent
    starGrid.Children.Add starCanvas |> ignore


    [0 .. numberOfSegments-1]
    |> List.map (arc radius numberOfSegments)
    |> List.iter (starCanvas.Children.Add >> ignore)
        
    name |> createStarLabel (2. * radius - 10.) |> starGrid.Children.Add |> ignore

    starGrid

