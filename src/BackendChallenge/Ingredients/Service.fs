module BackendChallenge.Ingredients.Service

open System
open BackendChallenge.Models.Recipe
open Giraffe
open MongoDB.Driver

let findIngredients (collection: IMongoCollection<Ingredient>) (ingredientsRef: string list) =
    let filterDef =
        Builders<Ingredient>.Filter.In ((fun i -> i.id), ingredientsRef)

    collection.Find(filterDef).ToEnumerable()
    |> List.ofSeq

let getIngredientRefs (ingredients: Ingredient list) = ingredients |> List.map (fun i -> i.id)

let createNewIngredientId (ingredient: Ingredient) =
    { ingredient with
          id = ShortGuid.fromGuid (Guid.NewGuid()) }
