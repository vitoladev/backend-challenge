module BackendChallenge.Data

open BackendChallenge.Models.Recipe
open BackendChallenge.CMS.Service
open Microsoft.Extensions.DependencyInjection
open MongoDB.Driver

type IServiceCollection with
  member this.AddMongoDb(collection : IMongoCollection<Recipe>) =
    this.AddSingleton<CreateRecipe>(createRecipe collection) |> ignore