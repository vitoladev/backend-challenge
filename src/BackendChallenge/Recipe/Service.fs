module BackendChallenge.Recipe.Service

open System
open BackendChallenge.Models.Recipe
open FSharp.Control.Tasks
open MongoDB.Driver
open BackendChallenge.Ingredients.Service

let convertCollectionToRecipe
    (recipesData: Collections.Generic.List<RecipeCollection>)
    (ingredientsCollection: IMongoCollection<Ingredient>)
    : Recipe list =
    List.ofSeq recipesData
    |> List.map
        (fun data ->
            let ingredients =
                let ref = List.ofSeq data.ingredientsRef
                findIngredients ingredientsCollection ref

            let equipments = List.ofSeq data.equipments

            { id = data.id
              name = data.name
              description = data.description
              ingredients = ingredients
              equipments = equipments })

let findRecipes
    (recipesCollection: IMongoCollection<RecipeCollection>)
    (ingredientsCollection: IMongoCollection<Ingredient>)
    (page: int)
    : Async<RecipePaginatedResult> =
    async {
        let pageSize = 8
        let skipSize = (page - 1) * pageSize
        let filter = Builders.Filter.Empty

        let! recipesData =
            recipesCollection
                .Find(filter)
                .Skip(skipSize)
                .Limit(pageSize)
                .ToListAsync()
            |> Async.AwaitTask

        let recipes =
            convertCollectionToRecipe recipesData ingredientsCollection

        let! count =
            recipesCollection.CountDocumentsAsync(filter)
            |> Async.AwaitTask

        let totalPages =
            int (Math.Ceiling(float count / float pageSize))

        return
            { data = recipes
              totalPages = totalPages }
    }

let findRecipesByIngredientId
    (recipesCollection: IMongoCollection<RecipeCollection>)
    (ingredientsCollection: IMongoCollection<Ingredient>)
    (ingredientId: string)
    =
    async {
        let ingredientsFilter =
            Builders<RecipeCollection>.Filter.Where (fun i -> i.ingredientsRef.Contains(ingredientId))

        let! collection =
            recipesCollection.FindAsync<RecipeCollection>(ingredientsFilter)
            |> Async.AwaitTask

        let! recipesData = collection.ToListAsync() |> Async.AwaitTask

        let recipes =
            convertCollectionToRecipe recipesData ingredientsCollection

        return recipes
    }
