module BackendChallenge.Config

open BackendChallenge.App
open BackendChallenge.Data
open BackendChallenge.Models.Recipe
open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Giraffe
open MongoDB.Driver

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

    clearResponse
    >=> setStatusCode 500
    >=> text ex.Message

let configureCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:5000", "https://localhost:5001")
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env =
        app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false ->
         app
             .UseGiraffeErrorHandler(errorHandler)
             .UseHttpsRedirection())
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureServices (services: IServiceCollection) =
    let dbConnectionString =
        Environment.GetEnvironmentVariable("MONGO_URL")

    let mongo = MongoClient(dbConnectionString)
    let db = mongo.GetDatabase "challengedb"

    services.AddMongoDb(db.GetCollection<Recipe>("recipes"))
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore
