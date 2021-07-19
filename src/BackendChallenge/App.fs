module BackendChallenge.App

open Giraffe
open BackendChallenge.CMS.HttpHandlers
open BackendChallenge.Recipe.HttpHandlers
open Microsoft.AspNetCore.Http

let webApp : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ subRoute
                 "/api"
                 (choose [ POST >=> route "/cms" >=> handleCreateRecipe
                           GET >=> route "/recipes" >=> handleFindRecipes
                           GET
                           >=> routef "/recipes/ingredient/%s" handleFindRecipesByIngredientId ])
             setStatusCode 404 >=> text "Not Found" ]
