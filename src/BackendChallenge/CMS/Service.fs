module BackendChallenge.CMS.Service

open System
open System.Collections.Generic
open BackendChallenge.Models.Recipe
open Giraffe
open BackendChallenge.Ingredients.Service
open MongoDB.Driver

let convertPayloadToCollection (payload: Recipe) (ingredients: Ingredient list) =
    let collectionEquipments = List<Equipment>(payload.equipments)

    let ingredientsRef =
        ingredients |> getIngredientRefs |> List<string>

    { id = ShortGuid.fromGuid (Guid.NewGuid())
      name = payload.name
      description = payload.description
      ingredientsRef = ingredientsRef
      equipments = collectionEquipments }

let createRecipe
    (recipesCollection: IMongoCollection<RecipeCollection>)
    (ingredientsCollection: IMongoCollection<Ingredient>)
    (recipePayload: Recipe)
    : CreateRecipeResult =
    async {
        let createdIngredientsReference =
            recipePayload.ingredients
            |> List.filter (fun i -> i.id <> null)

        let createdIngredients =
            createdIngredientsReference
            |> List.map (fun i -> i.id)
            |> findIngredients ingredientsCollection

        let couldNotFindAIngredient =
            createdIngredientsReference.Length
            <> createdIngredients.Length

        if couldNotFindAIngredient then
            let notFoundIngredients =
                List.except (createdIngredients |> getIngredientRefs) (createdIngredientsReference |> getIngredientRefs)

            return Error $"Ingredients with IDs {notFoundIngredients} does not exist"
        else
            let! session =
                recipesCollection.Database.Client.StartSessionAsync()
                |> Async.AwaitTask

            session.StartTransaction()

            try
                let newIngredients =
                    recipePayload.ingredients
                    |> List.except createdIngredientsReference
                    |> List.map createNewIngredientId

                newIngredients
                |> ingredientsCollection.InsertManyAsync
                |> Async.AwaitTask
                |> ignore

                let ingredients = createdIngredients @ newIngredients

                let collection =
                    convertPayloadToCollection recipePayload ingredients

                collection
                |> recipesCollection.InsertOneAsync
                |> Async.AwaitTask
                |> ignore

                session.CommitTransactionAsync()
                |> Async.AwaitTask
                |> ignore

                let recipe =
                    { id = collection.id
                      name = collection.name
                      description = collection.description
                      ingredients = ingredients
                      equipments = recipePayload.equipments }

                return Ok recipe
            with _ ->
                session.AbortTransactionAsync()
                |> Async.AwaitTask
                |> ignore

                return Error "Failed to create collection"
    }
