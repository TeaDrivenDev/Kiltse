#load "TestWindow.fsx"

open System
open System.Windows.Shapes
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Media.Effects
open System.Windows

let window, canvas = TestWindow.window, TestWindow.canvas
window.Width <- 250.
window.Height <- 250.

let uiColor = Colors.AntiqueWhite
let maxSegments = 30

let getEffect (color : Color) =
    let effect = DropShadowEffect(ShadowDepth = 0., Color = color, Opacity = 1., BlurRadius = 12.)
    effect.RenderingBias <- RenderingBias.Quality
    effect

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

[<Measure>] type deg
[<Measure>] type rad

let gapPixels = 50.

let toPolar (angle : float<deg>) = Math.PI * 1.<rad> * angle / 180.<deg>

let relativePeripheralCoordinates radius angle =
    let polarAngle = angle |> toPolar

    let relativeX = radius * Math.Cos (polarAngle / 1.<rad>)
    let relativeY = radius * Math.Sin (polarAngle / 1.<rad>)

    relativeX, -relativeY

let contentDistance = 30.

let adjustForDirection direction (startAngle : float<deg>) (angle : float<deg>) =
    let sign = if direction = SweepDirection.Clockwise then -1. else 1.

    startAngle + (sign * angle)

let createTextBlock (cx, cy) name =
    let tx = TextBlock(Text = name)
    tx.Foreground <- Brushes.AntiqueWhite
    tx.FontSize <- 12.
    tx.Background <- SolidColorBrush(Color.FromRgb(0x10uy, 0x10uy, 0x10uy))
    Canvas.SetLeft(tx, cx)
    Canvas.SetTop(tx, cy)

let arc radius totalInt indexInt =
    let total, index = float totalInt, float indexInt

    let circumference = 2. * Math.PI * radius
    let gapHalfAngle = 360.<deg> * gapPixels / (circumference * 2.)

    let arcAngle = 360.<deg> / total

    let direction = SweepDirection.Counterclockwise
    let startAngle = 180.<deg>

    let arcStartAngle = index * arcAngle + gapHalfAngle |> adjustForDirection direction startAngle
    let arcEndAngle = (index + 1.) * arcAngle - gapHalfAngle |> adjustForDirection direction startAngle

    let contentCenterAngle = (index + 0.5) * arcAngle
    let ccRelativeX, ccRelativeY = relativePeripheralCoordinates (radius + contentDistance) contentCenterAngle

    let cx, cy = radius + ccRelativeX, radius + ccRelativeY

    let arcStartX, arcStartY =
        arcEndAngle
        |> relativePeripheralCoordinates radius

    let arcEndX, arcEndY =
        arcStartAngle
        |> relativePeripheralCoordinates radius

    let sx, sy = radius + arcEndX, radius + arcEndY
    let ex, ey = radius + arcStartX, radius + arcStartY

    let tx = createTextBlock (cx, cy) "Dings"

    let arcSeg = ArcSegment()
    arcSeg.Point <- Point(ex, ey)
    arcSeg.Size <- Size(radius, radius)
    arcSeg.IsLargeArc <- (total = 1.)
    arcSeg.SweepDirection <- direction

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
    arcPath.StrokeThickness <- 15.
    arcPath.Effect <- getEffect color
    arcPath.Data <- pathGeometry

    let arcCanvas = Canvas(Width = 2. * radius, Height = 2. * radius)
    arcCanvas.HorizontalAlignment <- HorizontalAlignment.Stretch
    arcCanvas.VerticalAlignment <- VerticalAlignment.Stretch

    [
        arcPath :> UIElement
//        tx :> UIElement
    ]
    |> List.iter (arcCanvas.Children.Add >> ignore)

    arcCanvas

let createStar name radius (x, y) segments =
    let numberOfSegments = segments |> List.length

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

let s = createStar "Dings" 50. (100., 100.) [0..12]
s |> canvas.Children.Add

canvas.Children.Clear()

[1; 7]
|> List.iter (fun n ->
    createStar "Dings" 50. (100., 100.) [0..n-1]
    |> canvas.Children.Add
    |> ignore)
