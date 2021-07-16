module BackendChallenge.CMS.Service

open BackendChallenge.Models.Recipe
open MongoDB.Driver

let createRecipe (collection: IMongoCollection<Recipe>) (recipe: Recipe) : Async<Recipe> =
    async {
        collection.InsertOneAsync recipe
        |> Async.AwaitTask
        |> ignore

        return recipe
    }
