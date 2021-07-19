module BackendChallenge.CMS.HttpHandlers

open BackendChallenge.Models.Recipe
open BackendChallenge.Models.Validations.Recipe
open AccidentalFish.FSharp.Validation
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Giraffe

let handleCreateRecipe : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! recipePayload = ctx.BindJsonAsync<Recipe>()

            let validationResult = recipePayload |> validateRecipe

            match validationResult with
            | Ok ->
                let createRecipe = ctx.GetService<CreateRecipe>()
                let! createdRecipe = createRecipe recipePayload

                match createdRecipe with
                | Result.Ok recipe -> return! Successful.CREATED recipe next ctx
                | Error err -> return! (setStatusCode 400 >=> json (Error err)) next ctx

            | Errors errors -> return! (setStatusCode 422 >=> json (Errors errors)) next ctx
        }
