module BackendChallenge.Recipe.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open BackendChallenge.Models.Recipe

let handleFindRecipes : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            match ctx.GetQueryStringValue "page" with
            | Error msg -> return! RequestErrors.BAD_REQUEST(Error msg) next ctx
            | Ok pageQuery ->
                try
                    let page = int pageQuery

                    if page < 1 then
                        return!
                            RequestErrors.BAD_REQUEST
                                (Error "query string value 'page' should should be bigger than 0")
                                next
                                ctx
                    else
                        let findRecipes = ctx.GetService<FindRecipesPaginated>()
                        let! recipes = findRecipes page

                        return! Successful.OK recipes next ctx
                with :? System.FormatException ->
                    return!
                        RequestErrors.BAD_REQUEST
                            (Error "query string value 'page' should be in a integer format")
                            next
                            ctx
        }
