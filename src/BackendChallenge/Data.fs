module BackendChallenge.Data

open BackendChallenge.Models.Recipe
open BackendChallenge.CMS.Service
open BackendChallenge.Recipe.Service
open Microsoft.Extensions.DependencyInjection
open MongoDB.Driver

type IServiceCollection with
    member this.AddMongoDb(db: IMongoDatabase) =
        let recipeCollection =
            db.GetCollection<RecipeCollection>("recipes")

        let ingredientCollection =
            db.GetCollection<Ingredient>("ingredients")

        this.AddSingleton<CreateRecipe>(createRecipe recipeCollection ingredientCollection)
        |> ignore

