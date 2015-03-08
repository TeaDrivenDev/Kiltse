module TeaDriven.Kiltse.Tests

open TeaDriven.Kiltse
//open NUnit.Framework
//
//[<Test>]
//let ``hello returns 42`` () =
//  let result = Library.hello 42
//  printfn "%i" result
//  Assert.AreEqual(42,result)



//let effect = DropShadowEffect(ShadowDepth = 0., Color = color, Opacity = 1., BlurRadius = 12.)
//effect.RenderingBias <- RenderingBias.Quality




//let createStarLabel maxWidth name =
//    let label = TextBlock()
//    label.Text <- name
//    label.Foreground <- Brushes.White
//    label.Background <- Brushes.Transparent
//    label.FontWeight <- FontWeights.Regular
//
//    label.FontSize <- 12.
//
//    label.MaxWidth <- maxWidth
//    label.TextWrapping <- TextWrapping.Wrap
//
//    label.HorizontalAlignment <- HorizontalAlignment.Center
//    label.VerticalAlignment <- VerticalAlignment.Center
//    label.TextAlignment <- TextAlignment.Center
//
//    label
//
//
//
//
//let numberOfSatellites = releases |> List.length
//        
//let radius = 40.
////                    circumference satelliteRadius minimumDistance numberOfSatellites
////                     |> targetRadius minimumRadius
//    
//let starGrid = Grid(Width = 2. * radius, Height = 2. * radius)
//    starGrid.HorizontalAlignment <- HorizontalAlignment.Center
//    starGrid.VerticalAlignment <- VerticalAlignment.Center
//    Canvas.SetLeft(starGrid, x - radius)
//    Canvas.SetTop(starGrid, y - radius)
//    starGrid |> makeDraggable
//    
//
//let starCanvas = Canvas(Width = 2. * radius, Height = 2. * radius)
//    starCanvas.HorizontalAlignment <- HorizontalAlignment.Stretch
//    starCanvas.VerticalAlignment <- VerticalAlignment.Stretch
//    starCanvas.Background <- Brushes.Transparent
//    starGrid.Children.Add starCanvas |> ignore

//
//let toPolar (angle : float<deg>) = Math.PI * 1.<rad> * angle / 180.<deg>
//
//let relativePeripheralCoordinates radius angle =
//    let polarAngle = angle |> toPolar
//
//    let relativeX = radius * Math.Cos (polarAngle / 1.<rad>)
//    let relativeY = radius * Math.Sin (polarAngle / 1.<rad>)
//
//    relativeY, -relativeX
//
//
//let arc radius totalInt indexInt =
//    let total, index = float totalInt, float indexInt
//
//    let circumference = 2. * Math.PI * radius
//    let gapHalfAngle = 360.<deg> * gapPixels / (circumference * 2.)
//
//
//    let arcAngle = 360.<deg> / total
//    let arcStartAngle = index * arcAngle + gapHalfAngle
//    let arcEndAngle = (index + 1.) * arcAngle - gapHalfAngle
//
//    let arcStartX, arcStartY = relativePeripheralCoordinates radius arcStartAngle
//    let arcEndX, arcEndY = relativePeripheralCoordinates radius arcEndAngle
//
//    let sx, sy = radius + arcEndX, radius + arcEndY
//    let ex, ey = radius + arcStartX, radius + arcStartY
//
//
//    let arcSeg = ArcSegment()
//    arcSeg.Point <- Point(ex, ey)
//    arcSeg.Size <- Size(radius, radius)
//    arcSeg.IsLargeArc <- (total = 1.)
//
//
//    let pathFigure = PathFigure()
//    pathFigure.StartPoint <- Point(sx, sy)
//    pathFigure.Segments.Add arcSeg
//
//    let pathGeometry = PathGeometry()
//    pathGeometry.Figures.Add pathFigure
//
//    let color =
//        match indexInt with
//        | 0 -> Colors.Red
//        | 1 -> Colors.Cyan
//        | _ -> uiColor
//
//
//    let arcPath = Path()
//    arcPath.Stroke <- SolidColorBrush color
//    arcPath.StrokeThickness <- 2.
//    arcPath.Effect <- getEffect color
//    arcPath.Data <- pathGeometry
//    arcPath.ClipToBounds <- false
//
//    arcPath
