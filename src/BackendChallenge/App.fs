module BackendChallenge.App

open Giraffe
open BackendChallenge.CMS.HttpHandlers
open Microsoft.AspNetCore.Http

let webApp : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ subRoute "/api" (choose [ POST >=> route "/cms" >=> handleCreateRecipe ])
             setStatusCode 404 >=> text "Not Found" ]
