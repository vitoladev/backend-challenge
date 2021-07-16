module BackendChallenge.CMS.HttpHandlers

open System
open BackendChallenge.Models.Recipe
open BackendChallenge.Models.Validations.Recipe
open AccidentalFish.FSharp.Validation
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Giraffe

let handleCreateRecipe : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let createRecipe = ctx.GetService<CreateRecipe>()

            let! recipePayload = ctx.BindJsonAsync<Recipe>()


            let recipe =
                { recipePayload with
                      id = ShortGuid.fromGuid (Guid.NewGuid()) }

            let validationResult = recipePayload |> validateRecipe

            match validationResult with
            | Ok ->
                let! createdRecipe = createRecipe recipe
                return! Successful.CREATED createdRecipe next ctx
            | Errors errors ->
                return! (setStatusCode 422 >=> json (Errors errors)) next ctx
        }
