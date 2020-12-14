module Dash.NET.POC.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Giraffe.ModelBinding

open Dash.NET
open Plotly.NET


module Helpers = 

    ///returns a choropleth plot that has the input country highlighted.
    let createWorldHighlightFigure countryName =
        Chart.ChoroplethMap(locations=[countryName],z=[100],Locationmode = StyleParam.LocationFormat.CountryNames)
        |> Chart.withMapStyle(
            ShowLakes=true,
            ShowOcean=true,
            OceanColor="lightblue",
            ShowRivers=true
        )
        |> Chart.withSize (1000.,1000.)
        |> Chart.withLayout (Layout.init(Paper_bgcolor="rgba(0,0,0,0)",Plot_bgcolor="rgba(0,0,0,0)"))
        |> GenericChart.toFigure

//----------------------------------------------------------------------------------------------------
//============================================== LAYOUT ==============================================
//----------------------------------------------------------------------------------------------------

//The layout describes the components that Dash will render for you. 
open Dash.NET.HTML // this namespace contains the standard html copmponents, such as div, h1, etc.
open Dash.NET.DCC  // this namespace contains the dash core components, the heart of your Dash.NET app.

open HTMLPropTypes
open ComponentPropTypes

//Note that this layout uses css classes defined by Bulma (https://bulma.io/), which gets defined as a css dependency in the app section below.

//Uebung

//let dslLayout = 
//    Table.table [ClassName "myTable"] [
//        H1.h1 [ClassName "title has-text-centered"] [str "Getraenke"]
//        Div.div [ClassName "content"] [
//            P.p [ClassName "has-text-centered"] [str "Hier kannst du gleich ein Getraenk auswaehlen."]
//        ] 
//        Div.div [ClassName "container"] [
//            H4.h4 [] [str "Gib hier den Namen ein"]
//            Input.input "country-selection" [
//                Input.ClassName "input is-primary"
//                Input.Type InputType.Text
//                Input.Value ""
//                Input.Debounce true
//            ] []
//        ]
       
//        Ul.ul [] [
//            Ul.ul [] [str "Coffee"]
//            Ul.ul [] [str "Tea"]
//            Ul.ul [] [str "Milk"]
//        ]
//        H3.h3 [ClassName "title has-text-centered"] [str "Weitere Getraenke"]
//        Ul.ul [] [
//            Ul.ul [] [str "Coke"]
//            Ul.ul [] [str "Water"]
//            Ul.ul [] [str "Juice"]
//        ]
//    ]


//let dslLayout =
//    Div.div [ClassName "section"; Custom ("Id",box "main-section")] [ //the style for 'main-section' is actually defined in a custom css that you can serve with the dash app.
//        H1.h1 [ClassName "title has-text-centered"] [str "Hello Dash from F#"]
//        Div.div [ClassName "content"] [ 
//            P.p [ClassName "has-text-centered"] [str "This is a simple example Dash.NET app that contains an input component, A world map graph, and a callback that highlights the country you type on that graph."]
//        ]
//        Div.div [ClassName "container"] [
//            H4.h4 [] [str "type a country name to highlight (Press enter to update)"]
//            Input.input "country-selection" [
//                Input.ClassName "input is-primary"
//                Input.Type InputType.Text
//                Input.Value "Austria"
//                Input.Debounce false
//            ] []
//        ]
//        Div.div [ClassName "container"] [
//            Graph.graph "world-highlight" [
//                Graph.ClassName "graph-style" 
//                Graph.Figure (Helpers.createWorldHighlightFigure "Austria")
//            ] []
//        ]
//    ]




//Uebung von w3school

  
//let dslLayout =
//    Table.table [ClassName "myTable"] [
//        Tr.tr [] [
//            Th.th [] [str "Firstname"]
//            Th.th [] [str "Lastname"]
//            Th.th [] [str "Age"]
//        ]
//        Tr.tr [] [
//            Td.td [] [str "Jill"]
//            Td.td [] [str "Smith"]
//            Td.td [] [str "50"]
//        ]
//        Tr.tr [] [
//            Td.td [] [str "Eve"]
//            Td.td [] [str "Jackson"]
//            Td.td [] [str "94"]
//        ]
//    ]

//Beispiel:
//let dslLayout =
//    Table.table [ClassName "myTable"] [
//        Tr.tr [] [
//            Th.th [] [str "Firstname"]
//            Th.th [] [str "Lastname"]
//            Th.th [] [str "Age"]
//        ]
//        Tr.tr [] [
//            Td.td [] [str "Jill"]
//            Td.td [] [str "Smith"]
//            Td.td [] [str "50"]
//        ]
//        Tr.tr [] [
//            Td.td [] [str "Eve"]
//            Td.td [] [str "Jackson"]
//            Td.td [] [str "94"]
//        ]
//    ]
let myGraph = Chart.Line ([(1,1);(2,2);(3,2);(4,8)])

let dslLayout =
    Div.div [] [
        H1.h1 [ClassName "title has-text-centered"] [str "Meine Webseite"]
        Table.table [ClassName "myTable"] [
            Tr.tr [] [
                Th.th [] [str "1"]
                Th.th [] [str "2"]
                Th.th [] [str "3"]
            ]
            Tr.tr [] [
                Td.td [] [str "A"]
                Td.td [] [str "B"]
                Td.td [] [str "C"]
            ]
            Tr.tr [] [
                Td.td [] [str "d" ]
                Td.td [] [str "d" ]
                Td.td [] [str "d" ]
            ]
        ]
        Div.div [] [
            H4.h4 [] [str "Eine Liste mit Tieren"]
            Ul.ul [] [
                Ul.ul [] [str "Katze"]
                Ul.ul [] [str "Hund"]
                Ul.ul [] [str "Maus"]
            ]
            H2.h2 [ClassName "title has-text-centered"] [str "Man kann auch Buttons erstellen" ]
            P.p [] [str "Das ist ein Paragraph. Hier unten ist gleich ein Button zu sehen:" ]
            Input.input "hallo" [Input.Type InputType.Text] []
            Div.div [Id "huhu"] []
            ]
        Div.div [] [
            H3.h3 [ClassName "title has-text-centered" ] [str "Graphen sind ebenfalls moeglich" ]
            H2.h2 [ClassName "title has-text-centered"] [str "hier ist er:" ]
            Graph.graph "my-ghraph-id" [Graph.Figure (myGraph |> GenericChart.toFigure)] []
            Img.img [Custom("src", box "Katze.jpg")] []
        ]
        ]

let testCallback =
    Callback(
        [CallbackInput.create("hallo", "value")],
        CallbackOutput.create("huhu","children"),
        (fun (input:string)-> input)
       )

//let test = 
//    DashApp.initDefault()
//    |> DashApp.withLayout dslLayout
//    |> DashApp.addCallback testCallback
//----------------------------------------------------------------------------------------------------
//============================================= Callbacks ============================================
//----------------------------------------------------------------------------------------------------

//Callbacks define how your components can be updated and update each other. A callback has one or 
//more Input components (defined by their id and the property that acts as input) and an output 
//component (again defined by its id and output property). Additionally, a function that handles the 
//input and returns the desired output is needed.

///This callback takes the 'value' property of the component with the 'country-selection' id, and 
///returns a map chart that will update the 'figure' property of the component with the 
///'world-highlight' id
let countryHighlightCallback =
    Callback(
        [|CallbackInput.create("country-selection","value")|],
        (CallbackOutput.create("world-highlight","figure")),
        (fun (countryName:string) -> countryName |> Helpers.createWorldHighlightFigure)
    )

//----------------------------------------------------------------------------------------------------
//============================================= The App ==============================================
//----------------------------------------------------------------------------------------------------

//The 'DashApp' type is your central DashApp that contains all settings, configs, the layout, styles, 
//scripts, etc. that makes up your Dash.NET app. 

let myDashApp =
    DashApp.initDefault() // create a Dash.NET app with default settings
    |> DashApp.withLayout dslLayout // register the layout defined above.
    |> DashApp.appendCSSLinks [ 
        //"https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css" // register bulma as an external css dependency
        "main.css" // serve your custom css
    ]
    |> DashApp.addCallback testCallback

    
 //   |> DashApp.addCallback countryHighlightCallback // register the callback that will update the map


// The things below are Giraffe/ASP:NetCore specific and will likely be abstracted in the future.

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.EnvironmentName with
    | "Development" -> app.UseDeveloperExceptionPage()
    | _ -> app.UseGiraffeErrorHandler(errorHandler))
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(DashApp.toHttpHandler myDashApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Debug)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(webRoot)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    0